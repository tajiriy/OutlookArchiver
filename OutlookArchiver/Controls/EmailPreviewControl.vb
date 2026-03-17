Option Explicit On
Option Strict On
Option Infer Off

Imports System.IO

Namespace Controls

    ''' <summary>
    ''' メールプレビュー UserControl。
    ''' ヘッダー（差出人・日時・件名・宛先）、本文（HTML/テキスト切り替え）、
    ''' 添付ファイルパネルを提供する。
    ''' </summary>
    Public Class EmailPreviewControl
        Inherits System.Windows.Forms.UserControl

        Private _currentEmail As Models.Email
        Private _showHtml As Boolean

        ' ════════════════════════════════════════════════════════════
        '  初期化
        ' ════════════════════════════════════════════════════════════

        Public Sub New()
            InitializeComponent()
            ' キャプションラベルにボールドフォントを適用
            Dim boldFont As New System.Drawing.Font(Me.Font, System.Drawing.FontStyle.Bold)
            lblFromCaption.Font = boldFont
            lblDateCaption.Font = boldFont
            lblSubjectCaption.Font = boldFont
            lblToCaption.Font = boldFont
        End Sub

        ' ════════════════════════════════════════════════════════════
        '  公開メソッド
        ' ════════════════════════════════════════════════════════════

        ''' <summary>指定メールの内容をプレビューに表示する。添付ファイル一覧も含む。</summary>
        Public Sub ShowEmail(email As Models.Email)
            _currentEmail = email
            UpdateHeader(email)
            UpdateBody(email)
            LoadAttachments(email.Attachments)
        End Sub

        ''' <summary>プレビュー内容をクリアし、空の状態に戻す。</summary>
        Public Sub ClearPreview()
            _currentEmail = Nothing
            lblFromValue.Text = String.Empty
            lblDateValue.Text = String.Empty
            lblSubjectValue.Text = String.Empty
            lblToValue.Text = String.Empty
            webBrowser.DocumentText = "<html><body></body></html>"
            txtBodyText.Text = String.Empty
            flowAttachments.Controls.Clear()
            pnlAttachments.Visible = False
            btnToggleView.Enabled = False
            btnToggleView.Text = "テキスト表示"
        End Sub

        ' ════════════════════════════════════════════════════════════
        '  プライベートメソッド
        ' ════════════════════════════════════════════════════════════

        Private Sub UpdateHeader(email As Models.Email)
            Dim senderName As String = If(String.IsNullOrEmpty(email.SenderName), String.Empty, email.SenderName)
            Dim senderMail As String = If(String.IsNullOrEmpty(email.SenderEmail), String.Empty, email.SenderEmail)
            If senderName.Length > 0 AndAlso senderMail.Length > 0 Then
                lblFromValue.Text = senderName & " <" & senderMail & ">"
            ElseIf senderMail.Length > 0 Then
                lblFromValue.Text = senderMail
            Else
                lblFromValue.Text = senderName
            End If

            lblDateValue.Text = email.ReceivedAt.ToString("yyyy/MM/dd HH:mm")
            lblSubjectValue.Text = If(String.IsNullOrEmpty(email.Subject), "(件名なし)", email.Subject)
            lblToValue.Text = FormatRecipientsJson(email.ToRecipients)
        End Sub

        Private Sub UpdateBody(email As Models.Email)
            Dim hasHtml As Boolean = Not String.IsNullOrEmpty(email.BodyHtml)
            Dim hasText As Boolean = Not String.IsNullOrEmpty(email.BodyText)

            ' 両方ある場合のみトグル有効
            btnToggleView.Enabled = hasHtml AndAlso hasText
            _showHtml = hasHtml

            If _showHtml Then
                webBrowser.DocumentText = email.BodyHtml
                webBrowser.Visible = True
                txtBodyText.Visible = False
                btnToggleView.Text = "テキスト表示"
            Else
                txtBodyText.Text = If(email.BodyText, String.Empty)
                txtBodyText.Visible = True
                webBrowser.Visible = False
                btnToggleView.Text = "HTML 表示"
            End If
        End Sub

        ''' <summary>JSON 配列文字列（["a@b.com","c@d.com"]）をカンマ区切りテキストに変換する。</summary>
        Private Function FormatRecipientsJson(json As String) As String
            If String.IsNullOrEmpty(json) Then Return String.Empty
            Dim s As String = json.Trim()
            If s.StartsWith("[") Then s = s.Substring(1)
            If s.EndsWith("]") Then s = s.Substring(0, s.Length - 1)
            Return s.Replace("""", String.Empty).Trim()
        End Function

        Private Sub LoadAttachments(attachments As List(Of Models.Attachment))
            flowAttachments.Controls.Clear()

            If attachments Is Nothing OrElse attachments.Count = 0 Then
                pnlAttachments.Visible = False
                Return
            End If

            For Each att As Models.Attachment In attachments
                Dim btn As New System.Windows.Forms.Button()
                btn.Text = att.FileName
                btn.Tag = att
                btn.Height = 26
                btn.AutoSize = True
                btn.Padding = New System.Windows.Forms.Padding(4, 0, 4, 0)
                AddHandler btn.Click, AddressOf AttachmentButton_Click
                flowAttachments.Controls.Add(btn)
            Next
            pnlAttachments.Visible = True
        End Sub

        ' ════════════════════════════════════════════════════════════
        '  イベントハンドラ
        ' ════════════════════════════════════════════════════════════

        Private Sub btnToggleView_Click(sender As Object, e As EventArgs) Handles btnToggleView.Click
            If _currentEmail Is Nothing Then Return
            _showHtml = Not _showHtml
            If _showHtml Then
                webBrowser.DocumentText = If(_currentEmail.BodyHtml, String.Empty)
                webBrowser.Visible = True
                txtBodyText.Visible = False
                btnToggleView.Text = "テキスト表示"
            Else
                txtBodyText.Text = If(_currentEmail.BodyText, String.Empty)
                txtBodyText.Visible = True
                webBrowser.Visible = False
                btnToggleView.Text = "HTML 表示"
            End If
        End Sub

        Private Sub AttachmentButton_Click(sender As Object, e As EventArgs)
            Dim btn As System.Windows.Forms.Button = TryCast(sender, System.Windows.Forms.Button)
            If btn Is Nothing Then Return

            Dim att As Models.Attachment = TryCast(btn.Tag, Models.Attachment)
            If att Is Nothing Then Return

            If Not File.Exists(att.FilePath) Then
                MessageBox.Show("ファイルが見つかりません:" & vbCrLf & att.FilePath,
                    "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim ext As String = Path.GetExtension(att.FileName).ToLower()
            If ext = ".jpg" OrElse ext = ".jpeg" OrElse ext = ".png" OrElse
               ext = ".gif" OrElse ext = ".bmp" Then
                ShowImagePreview(att.FilePath, att.FileName)
            Else
                Try
                    Dim psi As New System.Diagnostics.ProcessStartInfo(att.FilePath)
                    psi.UseShellExecute = True
                    System.Diagnostics.Process.Start(psi)
                Catch ex As Exception
                    MessageBox.Show("ファイルを開けませんでした:" & vbCrLf & ex.Message,
                        "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Sub

        Private Sub ShowImagePreview(filePath As String, fileName As String)
            Dim img As System.Drawing.Image
            Try
                img = System.Drawing.Image.FromFile(filePath)
            Catch ex As Exception
                MessageBox.Show("画像を読み込めませんでした:" & vbCrLf & ex.Message,
                    "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End Try

            Dim frm As New System.Windows.Forms.Form()
            frm.Text = fileName
            frm.Size = New System.Drawing.Size(800, 600)
            frm.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent

            Dim pb As New System.Windows.Forms.PictureBox()
            pb.Dock = System.Windows.Forms.DockStyle.Fill
            pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
            pb.Image = img

            frm.Controls.Add(pb)
            frm.ShowDialog(Me)

            img.Dispose()
            frm.Dispose()
        End Sub

    End Class

End Namespace
