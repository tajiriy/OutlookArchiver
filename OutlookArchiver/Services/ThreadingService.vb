Option Explicit On
Option Strict On
Option Infer Off

Namespace Services

    ''' <summary>
    ''' メールのスレッド ID 付与と件名正規化を担当するサービスクラス。
    ''' </summary>
    Public Class ThreadingService

        Private ReadOnly _repo As Data.EmailRepository

        Public Sub New(repo As Data.EmailRepository)
            _repo = repo
        End Sub

        ' ════════════════════════════════════════════════════════════
        '  スレッド ID 付与
        ' ════════════════════════════════════════════════════════════

        ''' <summary>
        ''' email.ThreadId を決定する。以下の優先順位で判定する。
        ''' 1. In-Reply-To ヘッダー → 親メールの ThreadId を継承
        ''' 2. References ヘッダー → 祖先メールの ThreadId を継承
        ''' 3. 正規化済み件名フォールバック → 同名スレッドに合流
        ''' 4. 新規スレッド → email.MessageId を ThreadId として使用
        ''' </summary>
        Public Sub AssignThreadId(email As Models.Email)

            ' ── Strategy 1: In-Reply-To ─────────────────────────────
            If Not String.IsNullOrEmpty(email.InReplyTo) Then
                Dim parentId As String = CleanMessageId(email.InReplyTo)
                Dim parent As Models.Email = _repo.GetEmailByMessageId(parentId)
                If parent IsNot Nothing AndAlso Not String.IsNullOrEmpty(parent.ThreadId) Then
                    email.ThreadId = parent.ThreadId
                    Return
                End If
                ' 親がまだ取り込まれていない場合は親の MessageId をスレッドルートとする
                email.ThreadId = parentId
                Return
            End If

            ' ── Strategy 2: References ──────────────────────────────
            If Not String.IsNullOrEmpty(email.References) Then
                Dim refs As String() = ParseReferences(email.References)
                ' 最新の祖先から順に DB を検索する（References は古い順に並ぶため末尾から）
                Dim i As Integer = refs.Length - 1
                Do While i >= 0
                    Dim refId As String = CleanMessageId(refs(i))
                    Dim ancestor As Models.Email = _repo.GetEmailByMessageId(refId)
                    If ancestor IsNot Nothing AndAlso Not String.IsNullOrEmpty(ancestor.ThreadId) Then
                        email.ThreadId = ancestor.ThreadId
                        Return
                    End If
                    i -= 1
                Loop
                ' DB に祖先がなければ最古の References をルートとする
                If refs.Length > 0 Then
                    email.ThreadId = CleanMessageId(refs(0))
                    Return
                End If
            End If

            ' ── Strategy 3: 正規化済み件名フォールバック ─────────────
            If Not String.IsNullOrEmpty(email.NormalizedSubject) Then
                Dim related As List(Of Models.Email) =
                    _repo.GetEmailsByNormalizedSubject(email.NormalizedSubject, 1)
                If related.Count > 0 AndAlso Not String.IsNullOrEmpty(related(0).ThreadId) Then
                    email.ThreadId = related(0).ThreadId
                    Return
                End If
            End If

            ' ── Strategy 4: 新規スレッド ────────────────────────────
            email.ThreadId = If(Not String.IsNullOrEmpty(email.MessageId),
                                email.MessageId,
                                System.Guid.NewGuid().ToString())
        End Sub

        ' ════════════════════════════════════════════════════════════
        '  件名正規化
        ' ════════════════════════════════════════════════════════════

        ''' <summary>
        ''' 返信・転送プレフィックスを除去した正規化済み件名を返す。
        ''' 除去対象: Re: / FW: / Fwd: / 返信: / 転送: およびその変形。
        ''' </summary>
        Public Shared Function NormalizeSubject(subject As String) As String
            If String.IsNullOrWhiteSpace(subject) Then Return String.Empty

            Dim result As String = subject.Trim()
            Dim prefixes As String() = {
                "re:", "re :", "re. ",
                "fw:", "fw :", "fw. ",
                "fwd:", "fwd :", "fwd. ",
                "返信:", "転送:", "回复:", "转发:"
            }

            Dim changed As Boolean = True
            Do While changed
                changed = False
                For Each prefix As String In prefixes
                    If result.ToLower().StartsWith(prefix) Then
                        result = result.Substring(prefix.Length).TrimStart()
                        changed = True
                    End If
                Next
            Loop

            Return result
        End Function

        ' ════════════════════════════════════════════════════════════
        '  ヘルパー
        ' ════════════════════════════════════════════════════════════

        ''' <summary>MessageID の前後の山括弧を除去する。</summary>
        Private Shared Function CleanMessageId(messageId As String) As String
            If String.IsNullOrEmpty(messageId) Then Return messageId
            Dim result As String = messageId.Trim()
            If result.StartsWith("<") AndAlso result.EndsWith(">") Then
                result = result.Substring(1, result.Length - 2)
            End If
            Return result
        End Function

        ''' <summary>References ヘッダー値をスペース区切りで分割して配列にする。</summary>
        Private Shared Function ParseReferences(references As String) As String()
            Return references.Split(New Char() {" "c, Chr(9), Chr(13), Chr(10)},
                                    StringSplitOptions.RemoveEmptyEntries)
        End Function

    End Class

End Namespace
