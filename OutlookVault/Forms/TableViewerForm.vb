Option Explicit On
Option Strict On
Option Infer Off

Imports System.Data
Imports System.Data.SQLite
Imports System.Windows.Forms

Namespace Forms

    ''' <summary>
    ''' データベーステーブルの内容を DataGridView で表示するフォーム。
    ''' 列ヘッダクリックでソート、テキストボックスで行フィルタ、プルダウンでテーブル切替が可能。
    ''' </summary>
    Partial Public Class TableViewerForm

        Private Shared ReadOnly TableNames() As String = {"emails", "attachments", "deleted_message_ids", "exchange_address_cache", "error_message_ids", "folder_sync_state"}
        Private Const MaxColumnWidth As Integer = 200

        Private ReadOnly _dbManager As Data.DatabaseManager

        Private Const FilterDelayMs As Integer = 800

        Private _dataTable As DataTable
        Private _bindingSource As BindingSource

        Public Sub New(dbManager As Data.DatabaseManager, tableName As String)
            _dbManager = dbManager
            Me.DoubleBuffered = True
            _bindingSource = New BindingSource()
            InitializeComponent()
            cboTable.Items.AddRange(DirectCast(TableNames, Object()))
            dgv.DataSource = _bindingSource
            AddHandler txtFilter.TextChanged, AddressOf TxtFilter_TextChanged
            AddHandler _filterTimer.Tick, AddressOf FilterTimer_Tick
            ' 初期テーブルを選択（イベント経由で LoadData が走る）
            cboTable.SelectedItem = tableName
        End Sub

        ''' <summary>
        ''' スクロール・リサイズ時のゴースト表示を防止する DataGridView サブクラス。
        ''' DoubleBuffered を有効化し、スクロール/リサイズ時に全面再描画を強制する。
        ''' </summary>
        Private Class BufferedDataGridView
            Inherits DataGridView

            Public Sub New()
                Me.DoubleBuffered = True
                SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint, True)
            End Sub

            Protected Overrides Sub OnResize(e As EventArgs)
                MyBase.OnResize(e)
                Me.Invalidate()
            End Sub

        End Class

        Private Sub cboTable_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTable.SelectedIndexChanged
            If cboTable.SelectedItem Is Nothing Then Return
            Dim selectedTable As String = CStr(cboTable.SelectedItem)
            Me.Text = "テーブルビューア - " & selectedTable
            txtFilter.Text = ""
            LoadData(selectedTable)
        End Sub

        Private Sub LoadData(tableName As String)
            dgv.SuspendLayout()
            Try
                If _dataTable IsNot Nothing Then
                    _bindingSource.DataSource = Nothing
                    _dataTable.Dispose()
                End If
                _dataTable = _dbManager.GetTableData(tableName)
                _bindingSource.DataSource = _dataTable
                ApplyMaxColumnWidth()
                UpdateRowCount()
            Finally
                dgv.ResumeLayout()
            End Try
        End Sub

        Private Sub ApplyMaxColumnWidth()
            ' 表示中のセルに基づいて1回だけ自動サイズ計算し、上限を適用
            dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells)
            For Each col As DataGridViewColumn In dgv.Columns
                If col.Width > MaxColumnWidth Then
                    col.Width = MaxColumnWidth
                End If
                ' ユーザーによるドラッグリサイズを確実に有効化
                col.Resizable = DataGridViewTriState.True
            Next
        End Sub

        Private Sub TxtFilter_TextChanged(sender As Object, e As EventArgs)
            ' キー入力のたびにタイマーをリセットして再スタート
            _filterTimer.Stop()
            _filterTimer.Start()
        End Sub

        Private Sub FilterTimer_Tick(sender As Object, e As EventArgs)
            _filterTimer.Stop()
            ApplyFilter()
        End Sub

        Private Sub ApplyFilter()
            Dim filterText As String = txtFilter.Text.Trim()
            If String.IsNullOrEmpty(filterText) Then
                _dataTable.DefaultView.RowFilter = ""
                UpdateRowCount()
                Return
            End If

            Try
                _dataTable.DefaultView.RowFilter = Filters.FilterParser.Parse(filterText, _dataTable.Columns)
            Catch ex As Exception
                _dataTable.DefaultView.RowFilter = ""
            End Try

            UpdateRowCount()
        End Sub

        Private Sub UpdateRowCount()
            Dim total As Integer = _dataTable.Rows.Count
            Dim filtered As Integer = _dataTable.DefaultView.Count
            If total = filtered Then
                lblRowCount.Text = String.Format("{0:N0} 件", total)
            Else
                lblRowCount.Text = String.Format("{0:N0} / {1:N0} 件", filtered, total)
            End If
            ' 右上に配置
            lblRowCount.Location = New Drawing.Point(
                pnlTop.ClientSize.Width - lblRowCount.Width - 10, 10)
        End Sub

        Private Sub dgv_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgv.ColumnHeaderMouseClick
            Dim colName As String = dgv.Columns(e.ColumnIndex).DataPropertyName
            Dim currentSort As String = If(_dataTable.DefaultView.Sort, "")

            If currentSort.StartsWith(String.Format("[{0}]", colName), StringComparison.OrdinalIgnoreCase) AndAlso
               Not currentSort.EndsWith("DESC", StringComparison.OrdinalIgnoreCase) Then
                _dataTable.DefaultView.Sort = String.Format("[{0}] DESC", colName)
            Else
                _dataTable.DefaultView.Sort = String.Format("[{0}] ASC", colName)
            End If
        End Sub

    End Class

End Namespace
