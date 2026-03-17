Option Explicit On
Option Strict On
Option Infer Off

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()

        ' ── MenuStrip ──────────────────────────────────────────────
        Me.menuStrip = New System.Windows.Forms.MenuStrip()
        Me.menuItemFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuItemFileExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuItemImport = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuItemImportNow = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuItemSearch = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuItemSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuItemHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuItemHelpAbout = New System.Windows.Forms.ToolStripMenuItem()

        ' ── ToolStrip ──────────────────────────────────────────────
        Me.toolStrip = New System.Windows.Forms.ToolStrip()
        Me.btnImportNow = New System.Windows.Forms.ToolStripButton()
        Me.btnAutoImport = New System.Windows.Forms.ToolStripButton()
        Me.toolStripSep1 = New System.Windows.Forms.ToolStripSeparator()
        Me.txtSearch = New System.Windows.Forms.ToolStripTextBox()
        Me.btnSearch = New System.Windows.Forms.ToolStripButton()

        ' ── SplitContainers ────────────────────────────────────────
        Me.splitMain = New System.Windows.Forms.SplitContainer()
        Me.splitRight = New System.Windows.Forms.SplitContainer()

        ' ── フォルダツリー ─────────────────────────────────────────
        Me.treeViewFolders = New System.Windows.Forms.TreeView()

        ' ── メール一覧 ─────────────────────────────────────────────
        Me.listViewEmails = New System.Windows.Forms.ListView()
        Me.colSubject = New System.Windows.Forms.ColumnHeader()
        Me.colSender = New System.Windows.Forms.ColumnHeader()
        Me.colReceivedAt = New System.Windows.Forms.ColumnHeader()

        ' ── プレビュー TabControl ───────────────────────────────────
        Me.tabControl = New System.Windows.Forms.TabControl()
        Me.tabPageNormal = New System.Windows.Forms.TabPage()
        Me.tabPageThread = New System.Windows.Forms.TabPage()

        ' ── StatusStrip ────────────────────────────────────────────
        Me.statusStrip = New System.Windows.Forms.StatusStrip()
        Me.lblStatusCount = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblStatusSep = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblStatusLastImport = New System.Windows.Forms.ToolStripStatusLabel()

        ' ── Begin Init ─────────────────────────────────────────────
        CType(Me.splitMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitMain.Panel1.SuspendLayout()
        Me.splitMain.Panel2.SuspendLayout()
        Me.splitMain.SuspendLayout()
        CType(Me.splitRight, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitRight.Panel1.SuspendLayout()
        Me.splitRight.Panel2.SuspendLayout()
        Me.splitRight.SuspendLayout()
        Me.menuStrip.SuspendLayout()
        Me.toolStrip.SuspendLayout()
        Me.tabControl.SuspendLayout()
        Me.tabPageNormal.SuspendLayout()
        Me.tabPageThread.SuspendLayout()
        Me.statusStrip.SuspendLayout()
        Me.SuspendLayout()

        ' ── MenuStrip 設定 ──────────────────────────────────────────
        Me.menuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {
            Me.menuItemFile, Me.menuItemImport, Me.menuItemSearch,
            Me.menuItemSettings, Me.menuItemHelp})
        Me.menuStrip.Location = New System.Drawing.Point(0, 0)
        Me.menuStrip.Name = "menuStrip"
        Me.menuStrip.Size = New System.Drawing.Size(1200, 24)
        Me.menuStrip.TabIndex = 0

        Me.menuItemFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menuItemFileExit})
        Me.menuItemFile.Name = "menuItemFile"
        Me.menuItemFile.Text = "ファイル(&F)"

        Me.menuItemFileExit.Name = "menuItemFileExit"
        Me.menuItemFileExit.Text = "終了(&X)"
        Me.menuItemFileExit.ShortcutKeys = System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.F4

        Me.menuItemImport.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menuItemImportNow})
        Me.menuItemImport.Name = "menuItemImport"
        Me.menuItemImport.Text = "取り込み(&I)"

        Me.menuItemImportNow.Name = "menuItemImportNow"
        Me.menuItemImportNow.Text = "今すぐ取り込み(&N)"
        Me.menuItemImportNow.ShortcutKeys = System.Windows.Forms.Keys.F5

        Me.menuItemSearch.Name = "menuItemSearch"
        Me.menuItemSearch.Text = "検索(&S)"

        Me.menuItemSettings.Name = "menuItemSettings"
        Me.menuItemSettings.Text = "設定(&T)"

        Me.menuItemHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.menuItemHelpAbout})
        Me.menuItemHelp.Name = "menuItemHelp"
        Me.menuItemHelp.Text = "ヘルプ(&H)"

        Me.menuItemHelpAbout.Name = "menuItemHelpAbout"
        Me.menuItemHelpAbout.Text = "バージョン情報(&A)"

        ' ── ToolStrip 設定 ──────────────────────────────────────────
        Me.toolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {
            Me.btnImportNow, Me.btnAutoImport, Me.toolStripSep1, Me.txtSearch, Me.btnSearch})
        Me.toolStrip.Location = New System.Drawing.Point(0, 24)
        Me.toolStrip.Name = "toolStrip"
        Me.toolStrip.Size = New System.Drawing.Size(1200, 25)
        Me.toolStrip.TabIndex = 1

        Me.btnImportNow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnImportNow.Name = "btnImportNow"
        Me.btnImportNow.Text = "今すぐ取り込み"
        Me.btnImportNow.ToolTipText = "対象フォルダのメールを今すぐ取り込みます (F5)"

        Me.btnAutoImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnAutoImport.Name = "btnAutoImport"
        Me.btnAutoImport.Text = "自動: ▶"
        Me.btnAutoImport.ToolTipText = "自動取り込みの開始/停止"

        Me.toolStripSep1.Name = "toolStripSep1"

        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(200, 25)
        Me.txtSearch.ToolTipText = "検索キーワードを入力してEnterまたは検索ボタンを押してください"

        Me.btnSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Text = "検索"
        Me.btnSearch.ToolTipText = "メールを検索します"

        ' ── splitMain 設定（左: フォルダツリー, 右: splitRight） ──────
        Me.splitMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitMain.Name = "splitMain"
        Me.splitMain.SplitterDistance = 200
        Me.splitMain.Panel1.Controls.Add(Me.treeViewFolders)
        Me.splitMain.Panel2.Controls.Add(Me.splitRight)

        ' ── フォルダツリー 設定 ─────────────────────────────────────
        Me.treeViewFolders.Dock = System.Windows.Forms.DockStyle.Fill
        Me.treeViewFolders.Name = "treeViewFolders"
        Me.treeViewFolders.HideSelection = False

        ' ── splitRight 設定（上: メール一覧, 下: TabControl） ─────────
        Me.splitRight.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitRight.Name = "splitRight"
        Me.splitRight.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.splitRight.SplitterDistance = 300
        Me.splitRight.Panel1.Controls.Add(Me.listViewEmails)
        Me.splitRight.Panel2.Controls.Add(Me.tabControl)

        ' ── メール一覧 ListView 設定 ────────────────────────────────
        Me.listViewEmails.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {
            Me.colSubject, Me.colSender, Me.colReceivedAt})
        Me.listViewEmails.Dock = System.Windows.Forms.DockStyle.Fill
        Me.listViewEmails.FullRowSelect = True
        Me.listViewEmails.HideSelection = False
        Me.listViewEmails.MultiSelect = False
        Me.listViewEmails.Name = "listViewEmails"
        Me.listViewEmails.View = System.Windows.Forms.View.Details
        Me.listViewEmails.VirtualMode = True
        Me.listViewEmails.VirtualListSize = 0

        Me.colSubject.Text = "件名"
        Me.colSubject.Width = 360

        Me.colSender.Text = "差出人"
        Me.colSender.Width = 160

        Me.colReceivedAt.Text = "受信日時"
        Me.colReceivedAt.Width = 140

        ' ── TabControl 設定 ─────────────────────────────────────────
        Me.tabControl.Controls.AddRange(New System.Windows.Forms.TabPage() {Me.tabPageNormal, Me.tabPageThread})
        Me.tabControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabControl.Name = "tabControl"
        Me.tabControl.SelectedIndex = 0

        Me.tabPageNormal.Name = "tabPageNormal"
        Me.tabPageNormal.Text = "通常表示"
        Me.tabPageNormal.UseVisualStyleBackColor = True

        Me.tabPageThread.Name = "tabPageThread"
        Me.tabPageThread.Text = "会話ビュー"
        Me.tabPageThread.UseVisualStyleBackColor = True

        ' ── StatusStrip 設定 ────────────────────────────────────────
        Me.statusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {
            Me.lblStatusCount, Me.lblStatusSep, Me.lblStatusLastImport})
        Me.statusStrip.Name = "statusStrip"
        Me.statusStrip.Size = New System.Drawing.Size(1200, 22)
        Me.statusStrip.TabIndex = 3

        Me.lblStatusCount.Name = "lblStatusCount"
        Me.lblStatusCount.Text = "総数 0件"

        Me.lblStatusSep.Name = "lblStatusSep"
        Me.lblStatusSep.Text = " | "

        Me.lblStatusLastImport.Name = "lblStatusLastImport"
        Me.lblStatusLastImport.Text = "最終取り込み: -"

        ' ── Form 設定 ──────────────────────────────────────────────
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1200, 660)
        Me.Controls.Add(Me.splitMain)
        Me.Controls.Add(Me.toolStrip)
        Me.Controls.Add(Me.menuStrip)
        Me.Controls.Add(Me.statusStrip)
        Me.MainMenuStrip = Me.menuStrip
        Me.Name = "MainForm"
        Me.Text = "OutlookArchiver"

        ' ── End Init ───────────────────────────────────────────────
        CType(Me.splitMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitMain.Panel1.ResumeLayout(False)
        Me.splitMain.Panel2.ResumeLayout(False)
        Me.splitMain.ResumeLayout(False)
        CType(Me.splitRight, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitRight.Panel1.ResumeLayout(False)
        Me.splitRight.Panel2.ResumeLayout(False)
        Me.splitRight.ResumeLayout(False)
        Me.menuStrip.ResumeLayout(False)
        Me.menuStrip.PerformLayout()
        Me.toolStrip.ResumeLayout(False)
        Me.toolStrip.PerformLayout()
        Me.tabControl.ResumeLayout(False)
        Me.tabPageNormal.ResumeLayout(False)
        Me.tabPageThread.ResumeLayout(False)
        Me.statusStrip.ResumeLayout(False)
        Me.statusStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    ' ── コントロール宣言 ────────────────────────────────────────────
    Friend WithEvents menuStrip As System.Windows.Forms.MenuStrip
    Friend WithEvents menuItemFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemFileExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemImport As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemImportNow As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemSearch As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemSettings As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuItemHelpAbout As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents toolStrip As System.Windows.Forms.ToolStrip
    Friend WithEvents btnImportNow As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnAutoImport As System.Windows.Forms.ToolStripButton
    Friend WithEvents toolStripSep1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents txtSearch As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents btnSearch As System.Windows.Forms.ToolStripButton
    Friend WithEvents splitMain As System.Windows.Forms.SplitContainer
    Friend WithEvents splitRight As System.Windows.Forms.SplitContainer
    Friend WithEvents treeViewFolders As System.Windows.Forms.TreeView
    Friend WithEvents listViewEmails As System.Windows.Forms.ListView
    Friend WithEvents colSubject As System.Windows.Forms.ColumnHeader
    Friend WithEvents colSender As System.Windows.Forms.ColumnHeader
    Friend WithEvents colReceivedAt As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabControl As System.Windows.Forms.TabControl
    Friend WithEvents tabPageNormal As System.Windows.Forms.TabPage
    Friend WithEvents tabPageThread As System.Windows.Forms.TabPage
    Friend WithEvents statusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents lblStatusCount As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents lblStatusSep As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents lblStatusLastImport As System.Windows.Forms.ToolStripStatusLabel

End Class
