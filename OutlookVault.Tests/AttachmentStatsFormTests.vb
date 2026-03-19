Option Explicit On
Option Strict On
Option Infer Off

Imports NUnit.Framework
Imports OutlookVault.Data
Imports OutlookVault.Forms
Imports System.Data.SQLite
Imports System.IO

Namespace Tests

    <TestFixture>
    Public Class AttachmentStatsFormTests

        Private _testDbPath As String
        Private _dbManager As DatabaseManager

        <SetUp>
        Public Sub SetUp()
            _testDbPath = Path.Combine(Path.GetTempPath(), "OutlookVaultTest_" & Guid.NewGuid().ToString("N") & ".db")
            _dbManager = New DatabaseManager(_testDbPath)
            _dbManager.Initialize()
        End Sub

        <TearDown>
        Public Sub TearDown()
            If File.Exists(_testDbPath) Then File.Delete(_testDbPath)
            If File.Exists(_testDbPath & "-wal") Then File.Delete(_testDbPath & "-wal")
            If File.Exists(_testDbPath & "-shm") Then File.Delete(_testDbPath & "-shm")
        End Sub

        Private Sub InsertEmail(conn As SQLiteConnection, id As Integer, messageId As String)
            Using cmd As New SQLiteCommand(
                "INSERT INTO emails (id, message_id, subject, sender_email, received_at, folder_name) " &
                "VALUES (@id, @mid, 'test', 'test@test.com', '2026-01-01', 'Inbox')", conn)
                cmd.Parameters.AddWithValue("@id", id)
                cmd.Parameters.AddWithValue("@mid", messageId)
                cmd.ExecuteNonQuery()
            End Using
        End Sub

        Private Sub InsertAttachment(conn As SQLiteConnection, emailId As Integer, fileName As String, fileSize As Long)
            Using cmd As New SQLiteCommand(
                "INSERT INTO attachments (email_id, file_name, file_path, file_size) " &
                "VALUES (@eid, @fn, @fp, @fs)", conn)
                cmd.Parameters.AddWithValue("@eid", emailId)
                cmd.Parameters.AddWithValue("@fn", fileName)
                cmd.Parameters.AddWithValue("@fp", "dummy/" & fileName)
                cmd.Parameters.AddWithValue("@fs", fileSize)
                cmd.ExecuteNonQuery()
            End Using
        End Sub

        <Test>
        Public Sub Form_ShowsEmptyGrid_WhenNoAttachments()
            Using frm As New AttachmentStatsForm(_dbManager)
                Dim dgv As System.Windows.Forms.DataGridView = DirectCast(frm.Controls.Find("dgv", True)(0), System.Windows.Forms.DataGridView)
                Assert.AreEqual(0, dgv.Rows.Count)
            End Using
        End Sub

        <Test>
        Public Sub Form_ShowsExtensionStats_WhenAttachmentsExist()
            Using conn As SQLiteConnection = _dbManager.GetConnection()
                InsertEmail(conn, 1, "msg-1")
                InsertAttachment(conn, 1, "doc.pdf", 1000)
                InsertAttachment(conn, 1, "report.pdf", 2000)
                InsertAttachment(conn, 1, "image.png", 500)
            End Using

            Using frm As New AttachmentStatsForm(_dbManager)
                Dim dgv As System.Windows.Forms.DataGridView = DirectCast(frm.Controls.Find("dgv", True)(0), System.Windows.Forms.DataGridView)
                Assert.AreEqual(2, dgv.Rows.Count)
                ' 件数降順なので pdf が先
                Assert.AreEqual(".pdf", dgv.Rows(0).Cells(0).Value.ToString())
                Assert.AreEqual(2L, dgv.Rows(0).Cells(1).Value)
                Assert.AreEqual(".png", dgv.Rows(1).Cells(0).Value.ToString())
                Assert.AreEqual(1L, dgv.Rows(1).Cells(1).Value)
            End Using
        End Sub

        <Test>
        Public Sub Form_HandlesFilesWithoutExtension()
            Using conn As SQLiteConnection = _dbManager.GetConnection()
                InsertEmail(conn, 1, "msg-1")
                InsertAttachment(conn, 1, "README", 100)
            End Using

            Using frm As New AttachmentStatsForm(_dbManager)
                Dim dgv As System.Windows.Forms.DataGridView = DirectCast(frm.Controls.Find("dgv", True)(0), System.Windows.Forms.DataGridView)
                Assert.AreEqual(1, dgv.Rows.Count)
                Assert.AreEqual("(なし)", dgv.Rows(0).Cells(0).Value.ToString())
            End Using
        End Sub

        <Test>
        Public Sub Form_ShowsSummaryLabel()
            Using conn As SQLiteConnection = _dbManager.GetConnection()
                InsertEmail(conn, 1, "msg-1")
                InsertAttachment(conn, 1, "a.pdf", 1000)
                InsertAttachment(conn, 1, "b.xlsx", 2000)
            End Using

            Using frm As New AttachmentStatsForm(_dbManager)
                Dim lbl As System.Windows.Forms.Label = DirectCast(frm.Controls.Find("lblSummary", True)(0), System.Windows.Forms.Label)
                Assert.IsTrue(lbl.Text.Contains("2"))
                Assert.IsTrue(lbl.Text.Contains("2 種類"))
            End Using
        End Sub

    End Class

End Namespace
