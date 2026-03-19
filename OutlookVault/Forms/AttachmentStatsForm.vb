Option Explicit On
Option Strict On
Option Infer Off

Imports System.Data.SQLite
Imports System.Windows.Forms

Namespace Forms

    ''' <summary>
    ''' 添付ファイルの拡張子別統計情報を表示するフォーム。
    ''' 拡張子ごとの件数・合計サイズ・割合を一覧表示する。
    ''' </summary>
    Partial Public Class AttachmentStatsForm

        Private ReadOnly _dbManager As Data.DatabaseManager
        Private _dataTable As System.Data.DataTable
        Private _bindingSource As BindingSource

        Public Sub New(dbManager As Data.DatabaseManager)
            _dbManager = dbManager
            _bindingSource = New BindingSource()
            InitializeComponent()
            AddHandler dgv.CellFormatting, AddressOf Dgv_CellFormatting
            LoadData()
        End Sub

        Private Sub LoadData()
            Dim totalCount As Long = 0
            Dim totalSize As Long = 0
            Dim extMap As New System.Collections.Generic.Dictionary(Of String, ExtensionRow)()

            Using conn As SQLiteConnection = _dbManager.GetConnection()
                Using cmd As New SQLiteCommand("SELECT file_name, COALESCE(file_size, 0) FROM attachments", conn)
                    Using reader As SQLiteDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim fileName As String = reader.GetString(0)
                            Dim fileSize As Long = reader.GetInt64(1)
                            Dim ext As String = IO.Path.GetExtension(fileName).ToLowerInvariant()
                            If String.IsNullOrEmpty(ext) Then ext = "(なし)"

                            Dim existing As ExtensionRow = Nothing
                            If extMap.TryGetValue(ext, existing) Then
                                extMap(ext) = New ExtensionRow(ext, existing.Count + 1L, existing.TotalSize + fileSize)
                            Else
                                extMap(ext) = New ExtensionRow(ext, 1L, fileSize)
                            End If
                            totalCount += 1L
                            totalSize += fileSize
                        End While
                    End Using
                End Using
            End Using

            Dim rows As New System.Collections.Generic.List(Of ExtensionRow)(extMap.Values)
            rows.Sort(Function(a, b) b.Count.CompareTo(a.Count))

            dgv.Rows.Clear()
            For Each row As ExtensionRow In rows
                Dim pct As Double = If(totalCount > 0, row.Count * 100.0 / CDbl(totalCount), 0.0)
                dgv.Rows.Add(row.Extension, row.Count, row.TotalSize, pct)
            Next

            lblSummary.Text = String.Format("合計: {0:#,##0} 件 / {1}  ({2} 種類)",
                                            totalCount, FormatFileSize(totalSize), rows.Count)
        End Sub

        Private Sub Dgv_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
            If e.ColumnIndex = colTotalSize.Index AndAlso e.Value IsNot Nothing Then
                Dim bytes As Long = CLng(e.Value)
                e.Value = FormatFileSize(bytes)
                e.FormattingApplied = True
            End If
        End Sub

        Private Shared Function FormatFileSize(bytes As Long) As String
            Return Services.FileHelper.FormatFileSize(bytes)
        End Function

        Private Structure ExtensionRow
            Public ReadOnly Extension As String
            Public ReadOnly Count As Long
            Public ReadOnly TotalSize As Long

            Public Sub New(ext As String, cnt As Long, size As Long)
                Extension = ext
                Count = cnt
                TotalSize = size
            End Sub
        End Structure

    End Class

End Namespace
