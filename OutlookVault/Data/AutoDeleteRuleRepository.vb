Option Explicit On
Option Strict On
Option Infer Off

Imports System.Data.SQLite

Namespace Data

    ''' <summary>auto_delete_rules テーブルへのアクセスを担当するリポジトリクラス。</summary>
    Public Class AutoDeleteRuleRepository

        Private ReadOnly _dbManager As DatabaseManager

        Public Sub New(dbManager As DatabaseManager)
            _dbManager = dbManager
        End Sub

        ''' <summary>全ルールを sort_order 昇順で取得する。</summary>
        Public Function GetAllRules() As List(Of Models.AutoDeleteRule)
            Const sql As String = "SELECT * FROM auto_delete_rules ORDER BY sort_order, id"
            Dim result As New List(Of Models.AutoDeleteRule)()
            Using conn As SQLiteConnection = _dbManager.GetConnection()
                Using cmd As New SQLiteCommand(sql, conn)
                    Using reader As SQLiteDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            result.Add(MapRule(reader))
                        End While
                    End Using
                End Using
            End Using
            Return result
        End Function

        ''' <summary>有効なルールのみを sort_order 昇順で取得する。</summary>
        Public Function GetEnabledRules() As List(Of Models.AutoDeleteRule)
            Const sql As String = "SELECT * FROM auto_delete_rules WHERE enabled = 1 ORDER BY sort_order, id"
            Dim result As New List(Of Models.AutoDeleteRule)()
            Using conn As SQLiteConnection = _dbManager.GetConnection()
                Using cmd As New SQLiteCommand(sql, conn)
                    Using reader As SQLiteDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            result.Add(MapRule(reader))
                        End While
                    End Using
                End Using
            End Using
            Return result
        End Function

        ''' <summary>ルールを追加し、自動採番された ID を返す。</summary>
        Public Function InsertRule(name As String, filterExpression As String) As Integer
            Const sql As String =
                "INSERT INTO auto_delete_rules (name, filter_expression, sort_order) " &
                "VALUES (@name, @filter, (SELECT COALESCE(MAX(sort_order), 0) + 1 FROM auto_delete_rules)); " &
                "SELECT last_insert_rowid();"
            Using conn As SQLiteConnection = _dbManager.GetConnection()
                Using cmd As New SQLiteCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@name", name)
                    cmd.Parameters.AddWithValue("@filter", filterExpression)
                    Return Convert.ToInt32(cmd.ExecuteScalar())
                End Using
            End Using
        End Function

        ''' <summary>ルールを更新する。</summary>
        Public Sub UpdateRule(id As Integer, name As String, filterExpression As String, enabled As Boolean)
            Const sql As String =
                "UPDATE auto_delete_rules SET name = @name, filter_expression = @filter, " &
                "enabled = @enabled, updated_at = datetime('now', 'localtime') WHERE id = @id"
            Using conn As SQLiteConnection = _dbManager.GetConnection()
                Using cmd As New SQLiteCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@id", CType(id, Object))
                    cmd.Parameters.AddWithValue("@name", name)
                    cmd.Parameters.AddWithValue("@filter", filterExpression)
                    cmd.Parameters.AddWithValue("@enabled", If(enabled, 1, 0))
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        ''' <summary>ルールを削除する。</summary>
        Public Sub DeleteRule(id As Integer)
            Const sql As String = "DELETE FROM auto_delete_rules WHERE id = @id"
            Using conn As SQLiteConnection = _dbManager.GetConnection()
                Using cmd As New SQLiteCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@id", CType(id, Object))
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        ''' <summary>ルールの有効/無効を切り替える。</summary>
        Public Sub SetEnabled(id As Integer, enabled As Boolean)
            Const sql As String =
                "UPDATE auto_delete_rules SET enabled = @enabled, updated_at = datetime('now', 'localtime') WHERE id = @id"
            Using conn As SQLiteConnection = _dbManager.GetConnection()
                Using cmd As New SQLiteCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@id", CType(id, Object))
                    cmd.Parameters.AddWithValue("@enabled", If(enabled, 1, 0))
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub

        ''' <summary>ルールの件数を返す。</summary>
        Public Function GetRuleCount() As Integer
            Const sql As String = "SELECT COUNT(1) FROM auto_delete_rules"
            Using conn As SQLiteConnection = _dbManager.GetConnection()
                Using cmd As New SQLiteCommand(sql, conn)
                    Return Convert.ToInt32(cmd.ExecuteScalar())
                End Using
            End Using
        End Function

        Private Shared Function MapRule(reader As SQLiteDataReader) As Models.AutoDeleteRule
            Dim rule As New Models.AutoDeleteRule()
            rule.Id = reader.GetInt32(reader.GetOrdinal("id"))
            rule.Name = reader.GetString(reader.GetOrdinal("name"))
            rule.FilterExpression = reader.GetString(reader.GetOrdinal("filter_expression"))
            rule.Enabled = reader.GetInt32(reader.GetOrdinal("enabled")) = 1
            rule.SortOrder = reader.GetInt32(reader.GetOrdinal("sort_order"))

            Dim createdStr As String = reader.GetString(reader.GetOrdinal("created_at"))
            Dim createdParsed As DateTime
            If DateTime.TryParse(createdStr, createdParsed) Then rule.CreatedAt = createdParsed

            Dim updatedStr As String = reader.GetString(reader.GetOrdinal("updated_at"))
            Dim updatedParsed As DateTime
            If DateTime.TryParse(updatedStr, updatedParsed) Then rule.UpdatedAt = updatedParsed

            Return rule
        End Function

    End Class

End Namespace
