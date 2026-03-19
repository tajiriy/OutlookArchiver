Option Explicit On
Option Strict On
Option Infer Off

Namespace Services

    ''' <summary>
    ''' 自動削除ルールの適用を担当するサービス。
    ''' 取り込み時の自動適用と、既存メールへの再適用をサポートする。
    ''' </summary>
    Public Class AutoDeleteService

        Private ReadOnly _ruleRepo As Data.AutoDeleteRuleRepository
        Private ReadOnly _emailRepo As Data.EmailRepository

        Public Sub New(ruleRepo As Data.AutoDeleteRuleRepository, emailRepo As Data.EmailRepository)
            _ruleRepo = ruleRepo
            _emailRepo = emailRepo
        End Sub

        ''' <summary>
        ''' 有効な全ルールを取り込み済みの新規メールに適用し、マッチしたメールを論理削除する。
        ''' </summary>
        ''' <param name="importedIds">今回取り込んだメールの ID リスト。</param>
        ''' <returns>論理削除した件数。</returns>
        Public Function ApplyRulesToNewEmails(importedIds As List(Of Integer)) As Integer
            If importedIds Is Nothing OrElse importedIds.Count = 0 Then Return 0

            Dim rules As List(Of Models.AutoDeleteRule) = _ruleRepo.GetEnabledRules()
            If rules.Count = 0 Then Return 0

            Dim deletedTotal As Integer = 0

            For Each rule As Models.AutoDeleteRule In rules
                Try
                    Dim matchedIds As List(Of Integer) = GetMatchingEmailIds(rule.FilterExpression, importedIds)
                    If matchedIds.Count > 0 Then
                        _emailRepo.SoftDeleteEmailsByIds(matchedIds)
                        deletedTotal += matchedIds.Count
                        Logger.Info(String.Format("自動削除ルール '{0}' により {1} 件を削除しました", rule.Name, matchedIds.Count))
                    End If
                Catch ex As Exception
                    Logger.Warn(String.Format("自動削除ルール '{0}' の適用中にエラーが発生しました: {1}", rule.Name, ex.Message))
                End Try
            Next

            Return deletedTotal
        End Function

        ''' <summary>
        ''' 指定ルール（または全有効ルール）を既存の未削除メールに再適用し、マッチしたメールを論理削除する。
        ''' </summary>
        ''' <param name="ruleId">特定ルールの ID。Nothing の場合は全有効ルールを適用。</param>
        ''' <returns>論理削除した件数。</returns>
        Public Function ApplyRulesToExistingEmails(Optional ruleId As Integer? = Nothing) As Integer
            Dim rules As List(Of Models.AutoDeleteRule)
            If ruleId.HasValue Then
                Dim all As List(Of Models.AutoDeleteRule) = _ruleRepo.GetAllRules()
                rules = New List(Of Models.AutoDeleteRule)()
                For Each r As Models.AutoDeleteRule In all
                    If r.Id = ruleId.Value Then
                        rules.Add(r)
                        Exit For
                    End If
                Next
            Else
                rules = _ruleRepo.GetEnabledRules()
            End If

            If rules.Count = 0 Then Return 0

            Dim deletedTotal As Integer = 0

            For Each rule As Models.AutoDeleteRule In rules
                Try
                    Dim matchedIds As List(Of Integer) = GetMatchingEmailIds(rule.FilterExpression, Nothing)
                    If matchedIds.Count > 0 Then
                        _emailRepo.SoftDeleteEmailsByIds(matchedIds)
                        deletedTotal += matchedIds.Count
                        Logger.Info(String.Format("自動削除ルール '{0}' の再適用により {1} 件を削除しました", rule.Name, matchedIds.Count))
                    End If
                Catch ex As Exception
                    Logger.Warn(String.Format("自動削除ルール '{0}' の再適用中にエラーが発生しました: {1}", rule.Name, ex.Message))
                End Try
            Next

            Return deletedTotal
        End Function

        ''' <summary>
        ''' フィルタ式にマッチする未削除メールの一覧を返す（プレビュー用）。
        ''' </summary>
        Public Function PreviewRule(filterExpression As String) As List(Of Models.Email)
            Return _emailRepo.SearchEmailsFiltered(filterExpression)
        End Function

        ''' <summary>
        ''' フィルタ式にマッチするメール ID を取得する。
        ''' scopeIds が指定された場合はその ID に限定する。
        ''' </summary>
        Private Function GetMatchingEmailIds(filterExpression As String,
                                              scopeIds As List(Of Integer)) As List(Of Integer)
            ' フィルタ式で検索（未削除メールのみ対象）
            Dim matchedEmails As List(Of Models.Email) = _emailRepo.SearchEmailsFiltered(filterExpression)

            If scopeIds Is Nothing Then
                ' 全対象
                Dim ids As New List(Of Integer)()
                For Each email As Models.Email In matchedEmails
                    ids.Add(email.Id)
                Next
                Return ids
            Else
                ' scopeIds に含まれるもののみ
                Dim scopeSet As New HashSet(Of Integer)(scopeIds)
                Dim ids As New List(Of Integer)()
                For Each email As Models.Email In matchedEmails
                    If scopeSet.Contains(email.Id) Then
                        ids.Add(email.Id)
                    End If
                Next
                Return ids
            End If
        End Function

    End Class

End Namespace
