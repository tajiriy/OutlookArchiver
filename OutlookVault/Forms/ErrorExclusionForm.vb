Option Explicit On
Option Strict On
Option Infer Off

Imports System.Data
Imports System.Windows.Forms

Namespace Forms

    ''' <summary>
    ''' エラー除外リストを管理するフォーム。
    ''' エラーで取り込みに失敗し、次回以降スキップ対象になったメールの一覧を表示する。
    ''' 個別または全件の除外解除が可能。
    ''' </summary>
    Public Class ErrorExclusionForm
        Inherits Form

        Private ReadOnly _repo As Data.EmailRepository
        Private WithEvents dgv As DataGridView
        Private pnlBottom As Panel
        Private WithEvents btnRemoveSelected As Button
        Private WithEvents btnClearAll As Button
        Private lblCount As Label
        Private _dataTable As DataTable
        Private _bindingSource As BindingSource

        Public Sub New(repo As Data.EmailRepository)
            _repo = repo
            InitializeComponents()
            LoadData()
        End Sub

        Private Sub InitializeComponents()
            Me.Text = "エラー除外リスト"
            Me.Size = New Drawing.Size(900, 500)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.MinimumSize = New Drawing.Size(600, 300)

            ' ── 下部パネル（ボタン） ──
            pnlBottom = New Panel()
            pnlBottom.Dock = DockStyle.Bottom
            pnlBottom.Height = 44
            pnlBottom.Padding = New Padding(6, 6, 6, 6)

            btnRemoveSelected = New Button()
            btnRemoveSelected.Text = "選択した項目の除外を解除(&R)"
            btnRemoveSelected.AutoSize = True
            btnRemoveSelected.Location = New Drawing.Point(8, 10)

            btnClearAll = New Button()
            btnClearAll.Text = "すべての除外を解除(&A)"
            btnClearAll.AutoSize = True
            btnClearAll.Location = New Drawing.Point(220, 10)

            lblCount = New Label()
            lblCount.AutoSize = True
            lblCount.Anchor = CType(AnchorStyles.Top Or AnchorStyles.Right, AnchorStyles)
            lblCount.Location = New Drawing.Point(Me.ClientSize.Width - 100, 14)
            lblCount.Text = ""

            pnlBottom.Controls.Add(btnRemoveSelected)
            pnlBottom.Controls.Add(btnClearAll)
            pnlBottom.Controls.Add(lblCount)

            ' ── DataGridView ──
            dgv = New DataGridView()
            dgv.Dock = DockStyle.Fill
            dgv.ReadOnly = True
            dgv.AllowUserToAddRows = False
            dgv.AllowUserToDeleteRows = False
            dgv.AllowUserToOrderColumns = True
            dgv.AllowUserToResizeColumns = True
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgv.MultiSelect = True
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            _bindingSource = New BindingSource()
            dgv.DataSource = _bindingSource

            Me.Controls.Add(dgv)
            Me.Controls.Add(pnlBottom)
        End Sub

        Private Sub LoadData()
            dgv.SuspendLayout()
            Try
                If _dataTable IsNot Nothing Then
                    _bindingSource.DataSource = Nothing
                    _dataTable.Dispose()
                End If
                _dataTable = _repo.GetErrorMessageEntries()
                _bindingSource.DataSource = _dataTable

                ' 列ヘッダーを日本語に設定
                SetColumnHeaders()
                ApplyMaxColumnWidth()
                UpdateCount()
            Finally
                dgv.ResumeLayout()
            End Try
        End Sub

        Private Sub SetColumnHeaders()
            If dgv.Columns.Count = 0 Then Return
            Dim headers As New Dictionary(Of String, String)() From {
                {"message_id", "MessageID"},
                {"folder_name", "フォルダ"},
                {"subject", "件名"},
                {"error_message", "エラー内容"},
                {"received_date", "受信日時"},
                {"sender_name", "送信者"},
                {"error_date", "エラー発生日時"}
            }
            For Each col As DataGridViewColumn In dgv.Columns
                If headers.ContainsKey(col.DataPropertyName) Then
                    col.HeaderText = headers(col.DataPropertyName)
                End If
            Next
        End Sub

        Private Sub ApplyMaxColumnWidth()
            Const MaxWidth As Integer = 250
            dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells)
            For Each col As DataGridViewColumn In dgv.Columns
                If col.Width > MaxWidth Then col.Width = MaxWidth
                col.Resizable = DataGridViewTriState.True
            Next
        End Sub

        Private Sub UpdateCount()
            Dim count As Integer = _dataTable.Rows.Count
            lblCount.Text = String.Format("{0:N0} 件", count)
            lblCount.Location = New Drawing.Point(
                pnlBottom.ClientSize.Width - lblCount.Width - 10, 14)
            btnRemoveSelected.Enabled = count > 0
            btnClearAll.Enabled = count > 0
        End Sub

        Private Sub btnRemoveSelected_Click(sender As Object, e As EventArgs) Handles btnRemoveSelected.Click
            If dgv.SelectedRows.Count = 0 Then
                MessageBox.Show("除外を解除する項目を選択してください。",
                    "選択なし", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim answer As DialogResult = MessageBox.Show(
                String.Format("選択した {0} 件の除外を解除しますか？" & vbCrLf &
                    "解除すると、次回の取り込みで再度取り込みを試みます。",
                    dgv.SelectedRows.Count),
                "除外解除の確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If answer <> DialogResult.Yes Then Return

            For Each row As DataGridViewRow In dgv.SelectedRows
                Dim messageId As String = CStr(row.Cells("message_id").Value)
                _repo.DeleteErrorMessageId(messageId)
            Next
            LoadData()
        End Sub

        Private Sub btnClearAll_Click(sender As Object, e As EventArgs) Handles btnClearAll.Click
            Dim count As Integer = _dataTable.Rows.Count
            If count = 0 Then Return

            Dim answer As DialogResult = MessageBox.Show(
                String.Format("すべてのエラー除外（{0} 件）を解除しますか？" & vbCrLf &
                    "解除すると、次回の取り込みで再度取り込みを試みます。",
                    count),
                "全除外解除の確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If answer <> DialogResult.Yes Then Return

            _repo.ClearAllErrorMessageIds()
            LoadData()
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing Then
                If _bindingSource IsNot Nothing Then _bindingSource.Dispose()
                If _dataTable IsNot Nothing Then _dataTable.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

    End Class

End Namespace
