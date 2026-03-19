Option Explicit On
Option Strict On
Option Infer Off

Namespace Controls

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class ConversationViewControl

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        Private components As System.ComponentModel.IContainer

        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.splitConversation = New System.Windows.Forms.SplitContainer()
            Me.listViewThread = New System.Windows.Forms.ListView()
            Me.colThreadSender = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.colThreadDate = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.colThreadSubject = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.pnlMsgBody = New System.Windows.Forms.Panel()
            Me.webBrowserMsg = New System.Windows.Forms.WebBrowser()
            Me.txtBodyMsg = New System.Windows.Forms.RichTextBox()
            Me.pnlMsgToolbar = New System.Windows.Forms.Panel()
            Me.btnToggleMsgView = New System.Windows.Forms.Button()
            Me.pnlMsgHeader = New System.Windows.Forms.Panel()
            Me.tlpMsgHeader = New System.Windows.Forms.TableLayoutPanel()
            Me.lblMsgFromCaption = New System.Windows.Forms.Label()
            Me.lblMsgFrom = New System.Windows.Forms.Label()
            Me.lblMsgDateCaption = New System.Windows.Forms.Label()
            Me.lblMsgDate = New System.Windows.Forms.Label()
            CType(Me.splitConversation, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.splitConversation.Panel1.SuspendLayout()
            Me.splitConversation.Panel2.SuspendLayout()
            Me.splitConversation.SuspendLayout()
            Me.pnlMsgBody.SuspendLayout()
            Me.pnlMsgToolbar.SuspendLayout()
            Me.pnlMsgHeader.SuspendLayout()
            Me.tlpMsgHeader.SuspendLayout()
            Me.SuspendLayout()
            '
            'splitConversation
            '
            Me.splitConversation.Dock = System.Windows.Forms.DockStyle.Fill
            Me.splitConversation.Location = New System.Drawing.Point(0, 0)
            Me.splitConversation.Name = "splitConversation"
            Me.splitConversation.Orientation = System.Windows.Forms.Orientation.Horizontal
            '
            'splitConversation.Panel1
            '
            Me.splitConversation.Panel1.Controls.Add(Me.listViewThread)
            '
            'splitConversation.Panel2
            '
            Me.splitConversation.Panel2.Controls.Add(Me.pnlMsgBody)
            Me.splitConversation.Panel2.Controls.Add(Me.pnlMsgToolbar)
            Me.splitConversation.Panel2.Controls.Add(Me.pnlMsgHeader)
            Me.splitConversation.Size = New System.Drawing.Size(800, 600)
            Me.splitConversation.SplitterDistance = 150
            Me.splitConversation.TabIndex = 0
            '
            'listViewThread
            '
            Me.listViewThread.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colThreadSender, Me.colThreadDate, Me.colThreadSubject})
            Me.listViewThread.Dock = System.Windows.Forms.DockStyle.Fill
            Me.listViewThread.FullRowSelect = True
            Me.listViewThread.HideSelection = False
            Me.listViewThread.Location = New System.Drawing.Point(0, 0)
            Me.listViewThread.MultiSelect = False
            Me.listViewThread.Name = "listViewThread"
            Me.listViewThread.Size = New System.Drawing.Size(800, 150)
            Me.listViewThread.TabIndex = 0
            Me.listViewThread.UseCompatibleStateImageBehavior = False
            Me.listViewThread.View = System.Windows.Forms.View.Details
            '
            'colThreadSender
            '
            Me.colThreadSender.Text = "差出人"
            Me.colThreadSender.Width = 160
            '
            'colThreadDate
            '
            Me.colThreadDate.Text = "日時"
            Me.colThreadDate.Width = 140
            '
            'colThreadSubject
            '
            Me.colThreadSubject.Text = "件名"
            Me.colThreadSubject.Width = 400
            '
            'pnlMsgBody
            '
            Me.pnlMsgBody.Controls.Add(Me.webBrowserMsg)
            Me.pnlMsgBody.Controls.Add(Me.txtBodyMsg)
            Me.pnlMsgBody.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlMsgBody.Location = New System.Drawing.Point(0, 80)
            Me.pnlMsgBody.Name = "pnlMsgBody"
            Me.pnlMsgBody.Size = New System.Drawing.Size(800, 366)
            Me.pnlMsgBody.TabIndex = 0
            '
            'webBrowserMsg
            '
            Me.webBrowserMsg.Dock = System.Windows.Forms.DockStyle.Fill
            Me.webBrowserMsg.IsWebBrowserContextMenuEnabled = False
            Me.webBrowserMsg.Location = New System.Drawing.Point(0, 0)
            Me.webBrowserMsg.Name = "webBrowserMsg"
            Me.webBrowserMsg.ScriptErrorsSuppressed = True
            Me.webBrowserMsg.Size = New System.Drawing.Size(800, 366)
            Me.webBrowserMsg.TabIndex = 0
            Me.webBrowserMsg.WebBrowserShortcutsEnabled = False
            '
            'txtBodyMsg
            '
            Me.txtBodyMsg.BackColor = System.Drawing.SystemColors.Window
            Me.txtBodyMsg.Dock = System.Windows.Forms.DockStyle.Fill
            Me.txtBodyMsg.Location = New System.Drawing.Point(0, 0)
            Me.txtBodyMsg.Name = "txtBodyMsg"
            Me.txtBodyMsg.ReadOnly = True
            Me.txtBodyMsg.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical
            Me.txtBodyMsg.Size = New System.Drawing.Size(800, 366)
            Me.txtBodyMsg.TabIndex = 1
            Me.txtBodyMsg.Text = ""
            Me.txtBodyMsg.Visible = False
            '
            'pnlMsgToolbar
            '
            Me.pnlMsgToolbar.Controls.Add(Me.btnToggleMsgView)
            Me.pnlMsgToolbar.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlMsgToolbar.Location = New System.Drawing.Point(0, 50)
            Me.pnlMsgToolbar.Name = "pnlMsgToolbar"
            Me.pnlMsgToolbar.Padding = New System.Windows.Forms.Padding(4, 3, 4, 3)
            Me.pnlMsgToolbar.Size = New System.Drawing.Size(800, 30)
            Me.pnlMsgToolbar.TabIndex = 1
            '
            'btnToggleMsgView
            '
            Me.btnToggleMsgView.AutoSize = True
            Me.btnToggleMsgView.Enabled = False
            Me.btnToggleMsgView.Location = New System.Drawing.Point(0, 0)
            Me.btnToggleMsgView.Name = "btnToggleMsgView"
            Me.btnToggleMsgView.Size = New System.Drawing.Size(75, 23)
            Me.btnToggleMsgView.TabIndex = 0
            Me.btnToggleMsgView.Text = "テキスト表示"
            '
            'pnlMsgHeader
            '
            Me.pnlMsgHeader.BackColor = System.Drawing.SystemColors.Info
            Me.pnlMsgHeader.Controls.Add(Me.tlpMsgHeader)
            Me.pnlMsgHeader.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlMsgHeader.Location = New System.Drawing.Point(0, 0)
            Me.pnlMsgHeader.Name = "pnlMsgHeader"
            Me.pnlMsgHeader.Padding = New System.Windows.Forms.Padding(4, 2, 4, 2)
            Me.pnlMsgHeader.Size = New System.Drawing.Size(800, 50)
            Me.pnlMsgHeader.TabIndex = 2
            '
            'tlpMsgHeader
            '
            Me.tlpMsgHeader.ColumnCount = 2
            Me.tlpMsgHeader.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60.0!))
            Me.tlpMsgHeader.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.tlpMsgHeader.Controls.Add(Me.lblMsgFromCaption, 0, 0)
            Me.tlpMsgHeader.Controls.Add(Me.lblMsgFrom, 1, 0)
            Me.tlpMsgHeader.Controls.Add(Me.lblMsgDateCaption, 0, 1)
            Me.tlpMsgHeader.Controls.Add(Me.lblMsgDate, 1, 1)
            Me.tlpMsgHeader.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tlpMsgHeader.Location = New System.Drawing.Point(4, 2)
            Me.tlpMsgHeader.Name = "tlpMsgHeader"
            Me.tlpMsgHeader.RowCount = 2
            Me.tlpMsgHeader.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.tlpMsgHeader.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.tlpMsgHeader.Size = New System.Drawing.Size(792, 46)
            Me.tlpMsgHeader.TabIndex = 0
            '
            'lblMsgFromCaption
            '
            Me.lblMsgFromCaption.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblMsgFromCaption.Location = New System.Drawing.Point(3, 0)
            Me.lblMsgFromCaption.Name = "lblMsgFromCaption"
            Me.lblMsgFromCaption.Size = New System.Drawing.Size(54, 23)
            Me.lblMsgFromCaption.TabIndex = 0
            Me.lblMsgFromCaption.Text = "差出人:"
            Me.lblMsgFromCaption.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblMsgFrom
            '
            Me.lblMsgFrom.AutoEllipsis = True
            Me.lblMsgFrom.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblMsgFrom.Location = New System.Drawing.Point(63, 0)
            Me.lblMsgFrom.Name = "lblMsgFrom"
            Me.lblMsgFrom.Size = New System.Drawing.Size(726, 23)
            Me.lblMsgFrom.TabIndex = 1
            Me.lblMsgFrom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'lblMsgDateCaption
            '
            Me.lblMsgDateCaption.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblMsgDateCaption.Location = New System.Drawing.Point(3, 23)
            Me.lblMsgDateCaption.Name = "lblMsgDateCaption"
            Me.lblMsgDateCaption.Size = New System.Drawing.Size(54, 23)
            Me.lblMsgDateCaption.TabIndex = 2
            Me.lblMsgDateCaption.Text = "日時:"
            Me.lblMsgDateCaption.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblMsgDate
            '
            Me.lblMsgDate.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblMsgDate.Location = New System.Drawing.Point(63, 23)
            Me.lblMsgDate.Name = "lblMsgDate"
            Me.lblMsgDate.Size = New System.Drawing.Size(726, 23)
            Me.lblMsgDate.TabIndex = 3
            Me.lblMsgDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'ConversationViewControl
            '
            Me.Controls.Add(Me.splitConversation)
            Me.Name = "ConversationViewControl"
            Me.Size = New System.Drawing.Size(800, 600)
            Me.splitConversation.Panel1.ResumeLayout(False)
            Me.splitConversation.Panel2.ResumeLayout(False)
            CType(Me.splitConversation, System.ComponentModel.ISupportInitialize).EndInit()
            Me.splitConversation.ResumeLayout(False)
            Me.pnlMsgBody.ResumeLayout(False)
            Me.pnlMsgToolbar.ResumeLayout(False)
            Me.pnlMsgToolbar.PerformLayout()
            Me.pnlMsgHeader.ResumeLayout(False)
            Me.tlpMsgHeader.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

        ' ── コントロール宣言 ────────────────────────────────────────────
        Friend WithEvents splitConversation As System.Windows.Forms.SplitContainer
        Friend WithEvents listViewThread As System.Windows.Forms.ListView
        Friend WithEvents colThreadSender As System.Windows.Forms.ColumnHeader
        Friend WithEvents colThreadDate As System.Windows.Forms.ColumnHeader
        Friend WithEvents colThreadSubject As System.Windows.Forms.ColumnHeader
        Friend WithEvents pnlMsgHeader As System.Windows.Forms.Panel
        Friend WithEvents tlpMsgHeader As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents lblMsgFromCaption As System.Windows.Forms.Label
        Friend WithEvents lblMsgFrom As System.Windows.Forms.Label
        Friend WithEvents lblMsgDateCaption As System.Windows.Forms.Label
        Friend WithEvents lblMsgDate As System.Windows.Forms.Label
        Friend WithEvents pnlMsgToolbar As System.Windows.Forms.Panel
        Friend WithEvents btnToggleMsgView As System.Windows.Forms.Button
        Friend WithEvents pnlMsgBody As System.Windows.Forms.Panel
        Friend WithEvents webBrowserMsg As System.Windows.Forms.WebBrowser
        Friend WithEvents txtBodyMsg As System.Windows.Forms.RichTextBox

    End Class

End Namespace
