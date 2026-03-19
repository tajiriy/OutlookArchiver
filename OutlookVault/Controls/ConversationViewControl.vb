Option Explicit On
Option Strict On
Option Infer Off

Namespace Controls

    ''' <summary>
    ''' 会話ビュー UserControl。
    ''' 同一スレッドのメール一覧（時系列昇順）と選択メッセージの引用除去済み本文を表示する。
    ''' </summary>
    Partial Class ConversationViewControl
        Inherits System.Windows.Forms.UserControl

        Private ReadOnly _quoteStripper As New Services.QuoteStripperService()
        Private ReadOnly _htmlSanitizer As New Services.HtmlSanitizerService()
        Private _threadEmails As List(Of Models.Email)
        Private _showHtml As Boolean

        ' ════════════════════════════════════════════════════════════
        '  初期化
        ' ════════════════════════════════════════════════════════════

        Public Sub New()
            InitializeComponent()
        End Sub

        ' ════════════════════════════════════════════════════════════
        '  公開プロパティ・メソッド
        ' ════════════════════════════════════════════════════════════

        ''' <summary>スレッド一覧とメッセージ本文の分割位置（ピクセル）。</summary>
        Public Property SplitterDistance As Integer
            Get
                Return splitConversation.SplitterDistance
            End Get
            Set(value As Integer)
                If value > 0 AndAlso value < splitConversation.Height Then
                    splitConversation.SplitterDistance = value
                End If
            End Set
        End Property

        ''' <summary>スレッド内のメール一覧を表示し、指定IDのメールを選択状態にする。</summary>
        Public Sub ShowThread(emails As List(Of Models.Email), selectedId As Integer)
            _threadEmails = emails

            listViewThread.BeginUpdate()
            listViewThread.Items.Clear()

            Dim selectIndex As Integer = 0
            For i As Integer = 0 To emails.Count - 1
                Dim em As Models.Email = emails(i)
                Dim sender As String
                If Not String.IsNullOrEmpty(em.SenderName) Then
                    sender = em.SenderName
                ElseIf Not String.IsNullOrEmpty(em.SenderEmail) Then
                    sender = em.SenderEmail
                Else
                    sender = "(不明)"
                End If

                Dim item As New System.Windows.Forms.ListViewItem(sender)
                item.SubItems.Add(em.ReceivedAt.ToString("yyyy/MM/dd HH:mm"))
                item.SubItems.Add(If(String.IsNullOrEmpty(em.Subject), "(件名なし)", em.Subject))
                item.Tag = CType(em.Id, Object)
                listViewThread.Items.Add(item)

                If em.Id = selectedId Then selectIndex = i
            Next

            listViewThread.EndUpdate()

            If listViewThread.Items.Count > 0 Then
                listViewThread.Items(selectIndex).Selected = True
                listViewThread.Items(selectIndex).EnsureVisible()
            End If
        End Sub

        ''' <summary>会話ビューをクリアして空の状態に戻す。</summary>
        Public Sub ClearView()
            _threadEmails = Nothing
            listViewThread.Items.Clear()
            lblMsgFrom.Text = String.Empty
            lblMsgDate.Text = String.Empty
            webBrowserMsg.DocumentText = "<html><body></body></html>"
            txtBodyMsg.Text = String.Empty
            btnToggleMsgView.Enabled = False
            btnToggleMsgView.Text = "テキスト表示"
        End Sub

        ' ════════════════════════════════════════════════════════════
        '  プライベートメソッド
        ' ════════════════════════════════════════════════════════════

        ''' <summary>指定メールのヘッダーと引用除去済み本文を表示する。</summary>
        Private Sub ShowMessage(email As Models.Email)
            ' ヘッダー更新
            Dim senderName As String = If(String.IsNullOrEmpty(email.SenderName), String.Empty, email.SenderName)
            Dim senderMail As String = If(String.IsNullOrEmpty(email.SenderEmail), String.Empty, email.SenderEmail)
            If senderName.Length > 0 AndAlso senderMail.Length > 0 Then
                lblMsgFrom.Text = senderName & " <" & senderMail & ">"
            ElseIf senderMail.Length > 0 Then
                lblMsgFrom.Text = senderMail
            Else
                lblMsgFrom.Text = senderName
            End If
            lblMsgDate.Text = email.ReceivedAt.ToString("yyyy/MM/dd HH:mm")

            ' 本文表示モード決定
            Dim hasHtml As Boolean = Not String.IsNullOrEmpty(email.BodyHtml)
            Dim hasText As Boolean = Not String.IsNullOrEmpty(email.BodyText)
            btnToggleMsgView.Enabled = hasHtml AndAlso hasText
            _showHtml = hasHtml

            RenderBody(email)
        End Sub

        ''' <summary>現在の _showHtml に応じて引用除去済み本文をレンダリングする。</summary>
        Private Sub RenderBody(email As Models.Email)
            If _showHtml AndAlso Not String.IsNullOrEmpty(email.BodyHtml) Then
                Dim sanitizedHtml As String = _htmlSanitizer.Sanitize(email.BodyHtml)
                webBrowserMsg.DocumentText = _quoteStripper.StripQuotesFromHtml(sanitizedHtml)
                webBrowserMsg.Visible = True
                txtBodyMsg.Visible = False
                btnToggleMsgView.Text = "テキスト表示"
            Else
                Dim stripped As String = _quoteStripper.StripQuotesFromText(
                    If(email.BodyText, String.Empty))
                txtBodyMsg.Text = If(stripped, String.Empty)
                txtBodyMsg.Visible = True
                webBrowserMsg.Visible = False
                btnToggleMsgView.Text = "HTML 表示"
            End If
        End Sub

        ''' <summary>現在選択中のメールを _threadEmails から取得する。</summary>
        Private Function FindSelectedEmail() As Models.Email
            If listViewThread.SelectedItems.Count = 0 Then Return Nothing
            If _threadEmails Is Nothing Then Return Nothing
            Dim selectedId As Integer = CInt(listViewThread.SelectedItems(0).Tag)
            For Each em As Models.Email In _threadEmails
                If em.Id = selectedId Then Return em
            Next
            Return Nothing
        End Function

        ' ════════════════════════════════════════════════════════════
        '  イベントハンドラ
        ' ════════════════════════════════════════════════════════════

        Private Sub listViewThread_SelectedIndexChanged(sender As Object, e As EventArgs) Handles listViewThread.SelectedIndexChanged
            Dim email As Models.Email = FindSelectedEmail()
            If email Is Nothing Then Return
            ShowMessage(email)
        End Sub

        Private Sub btnToggleMsgView_Click(sender As Object, e As EventArgs) Handles btnToggleMsgView.Click
            Dim email As Models.Email = FindSelectedEmail()
            If email Is Nothing Then Return
            _showHtml = Not _showHtml
            RenderBody(email)
        End Sub

        ''' <summary>
        ''' WebBrowser のナビゲーションを制御する。
        ''' about:blank 以外のナビゲーションをブロックし、外部 URL は確認後にデフォルトブラウザで開く。
        ''' </summary>
        Private Sub WebBrowserMsg_Navigating(sender As Object, e As System.Windows.Forms.WebBrowserNavigatingEventArgs) Handles webBrowserMsg.Navigating
            Dim url As String = e.Url.ToString()
            If url.Equals("about:blank", StringComparison.OrdinalIgnoreCase) Then Return
            e.Cancel = True

            If url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) OrElse
               url.StartsWith("https://", StringComparison.OrdinalIgnoreCase) Then
                Dim result As System.Windows.Forms.DialogResult = MessageBox.Show(
                    "外部リンクをブラウザで開きますか？" & vbCrLf & vbCrLf & url,
                    "リンクを開く", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If result = System.Windows.Forms.DialogResult.Yes Then
                    Try
                        Dim psi As New System.Diagnostics.ProcessStartInfo(url)
                        psi.UseShellExecute = True
                        System.Diagnostics.Process.Start(psi)
                    Catch ex As Exception
                        MessageBox.Show("リンクを開けませんでした:" & vbCrLf & ex.Message,
                            "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If
            End If
        End Sub

    End Class

End Namespace
