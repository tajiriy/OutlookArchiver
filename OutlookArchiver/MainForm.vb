Option Explicit On
Option Strict On
Option Infer Off

Imports System.Threading.Tasks

Public Class MainForm

    ' ════════════════════════════════════════════════════════════
    '  フィールド
    ' ════════════════════════════════════════════════════════════

    Private _settings As Config.AppSettings
    Private _dbManager As Data.DatabaseManager
    Private _repo As Data.EmailRepository
    Private _autoImportTimer As System.Windows.Forms.Timer

    Private _emailCache As List(Of Models.Email)
    Private _currentFolder As String      ' Nothing = すべて
    Private _isImporting As Boolean
    Private _autoImportEnabled As Boolean
    Private _searchQuery As String        ' 現在の検索クエリ（Nothing = 検索なし）

    ' ════════════════════════════════════════════════════════════
    '  フォーム初期化
    ' ════════════════════════════════════════════════════════════

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' デザイナーが SplitterDistance を上書きするため、ここで正しい比率に設定する
        ' フォルダツリー幅: 150px、メール一覧高さ: コンテンツ領域の約 40%
        splitMain.SplitterDistance = 150
        Dim contentHeight As Integer = Me.ClientSize.Height - menuStrip.Height - toolStrip.Height - statusStrip.Height
        splitRight.SplitterDistance = CInt(contentHeight * 0.4)

        InitializeServices()
        SetupAutoImportTimer()
        SetupListViewContextMenu()
        LoadFolderTree()
        UpdateStatusBar()

        If _settings.AutoImportEnabled Then
            StartAutoImport()
        End If
    End Sub

    Private Sub InitializeServices()
        _settings = Config.AppSettings.Instance
        _dbManager = New Data.DatabaseManager(_settings.DbFilePath)
        _dbManager.Initialize()
        _repo = New Data.EmailRepository(_dbManager)
        _emailCache = New List(Of Models.Email)()
    End Sub

    Private Sub SetupAutoImportTimer()
        _autoImportTimer = New System.Windows.Forms.Timer()
        _autoImportTimer.Interval = _settings.AutoImportIntervalMinutes * 60 * 1000
        AddHandler _autoImportTimer.Tick, AddressOf AutoImportTimer_Tick
    End Sub

    ''' <summary>メール一覧の複数選択・コンテキストメニューをコードで設定する。</summary>
    Private Sub SetupListViewContextMenu()
        listViewEmails.MultiSelect = True

        Dim ctxMenu As New System.Windows.Forms.ContextMenuStrip()
        Dim deleteItem As New System.Windows.Forms.ToolStripMenuItem("削除(&D)")
        AddHandler deleteItem.Click, AddressOf OnDeleteEmailMenuClick
        ctxMenu.Items.Add(CType(deleteItem, System.Windows.Forms.ToolStripItem))
        listViewEmails.ContextMenuStrip = ctxMenu
    End Sub

    ' ════════════════════════════════════════════════════════════
    '  フォルダツリー
    ' ════════════════════════════════════════════════════════════

    Private Sub LoadFolderTree()
        treeViewFolders.BeginUpdate()
        treeViewFolders.Nodes.Clear()

        Dim nodeAll As New TreeNode("すべて")
        nodeAll.Tag = Nothing
        treeViewFolders.Nodes.Add(nodeAll)

        Dim folders As List(Of String) = _repo.GetFolderNames()
        For Each folder As String In folders
            Dim node As New TreeNode(folder)
            node.Tag = folder
            treeViewFolders.Nodes.Add(node)
        Next

        treeViewFolders.EndUpdate()
        treeViewFolders.SelectedNode = nodeAll
    End Sub

    Private Sub treeViewFolders_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles treeViewFolders.AfterSelect
        If e.Node Is Nothing Then Return
        _currentFolder = TryCast(e.Node.Tag, String)
        ' 検索ボックスと検索クエリをクリアしてフォルダ切り替え
        txtSearch.Text = String.Empty
        _searchQuery = Nothing
        LoadEmails(_currentFolder)
    End Sub

    ' ════════════════════════════════════════════════════════════
    '  メール一覧（VirtualMode）
    ' ════════════════════════════════════════════════════════════

    ''' <summary>DBからメールを読み込んでListViewのVirtualListSizeを更新する。</summary>
    Private Sub LoadEmails(Optional folderName As String = Nothing)
        _emailCache = _repo.GetEmails(folderName, pageSize:=500)
        listViewEmails.VirtualListSize = _emailCache.Count
        listViewEmails.Invalidate()
        UpdateStatusBar()
    End Sub

    ''' <summary>メール一覧で行が選択されたときにプレビューと会話ビューを更新する。</summary>
    Private Sub listViewEmails_SelectedIndexChanged(sender As Object, e As EventArgs) Handles listViewEmails.SelectedIndexChanged
        If listViewEmails.SelectedIndices.Count = 0 Then
            emailPreview.ClearPreview()
            conversationView.ClearView()
            Return
        End If

        Dim idx As Integer = listViewEmails.SelectedIndices(0)
        If idx < 0 OrElse idx >= _emailCache.Count Then Return

        Dim email As Models.Email = _repo.GetEmailById(_emailCache(idx).Id)
        If email IsNot Nothing Then
            emailPreview.ShowEmail(email, _searchQuery)
            LoadConversationView(email)
        End If
    End Sub

    ''' <summary>選択メールのスレッドを取得して会話ビューに表示する。</summary>
    Private Sub LoadConversationView(email As Models.Email)
        Dim threadEmails As List(Of Models.Email)
        If String.IsNullOrEmpty(email.ThreadId) Then
            ' スレッドIDなし → 単独メールとして表示
            threadEmails = New List(Of Models.Email)() From {email}
        Else
            threadEmails = _repo.GetEmailsByThreadId(email.ThreadId)
            If threadEmails.Count = 0 Then
                threadEmails = New List(Of Models.Email)() From {email}
            End If
        End If
        conversationView.ShowThread(threadEmails, email.Id)
    End Sub

    ''' <summary>VirtualMode: 指定インデックスのListViewItemを返す。</summary>
    Private Sub listViewEmails_RetrieveVirtualItem(sender As Object, e As RetrieveVirtualItemEventArgs) Handles listViewEmails.RetrieveVirtualItem
        If e.ItemIndex < 0 OrElse e.ItemIndex >= _emailCache.Count Then
            e.Item = New ListViewItem(String.Empty)
            Return
        End If

        Dim email As Models.Email = _emailCache(e.ItemIndex)
        Dim subject As String = If(String.IsNullOrEmpty(email.Subject), "(件名なし)", email.Subject)
        Dim displaySender As String = If(Not String.IsNullOrEmpty(email.SenderName), email.SenderName, email.SenderEmail)

        Dim item As New ListViewItem(subject)
        item.SubItems.Add(If(displaySender, String.Empty))
        item.SubItems.Add(email.ReceivedAt.ToString("yyyy/MM/dd HH:mm"))
        item.Tag = email.Id
        e.Item = item
    End Sub

    ' ════════════════════════════════════════════════════════════
    '  取り込み
    ' ════════════════════════════════════════════════════════════

    Private Async Sub btnImportNow_Click(sender As Object, e As EventArgs) Handles btnImportNow.Click, menuItemImportNow.Click
        Await RunImportAsync()
    End Sub

    ''' <summary>
    ''' Outlook COM は STA スレッドが必要なため、TaskCompletionSource + 専用STAスレッドで実行する。
    ''' </summary>
    Private Async Function RunImportAsync() As Task
        If _isImporting Then Return
        _isImporting = True
        btnImportNow.Enabled = False
        menuItemImportNow.Enabled = False
        lblStatusCount.Text = "取り込み中..."

        Try
            Dim progress As New Progress(Of Services.ImportProgress)(
                Sub(p)
                    lblStatusCount.Text = String.Format("取り込み中... {0}/{1} ({2})",
                        p.ProcessedCount, p.TotalCount, p.FolderName)
                End Sub)

            Dim tcs As New TaskCompletionSource(Of Services.ImportResult)()
            Dim targetFolders As List(Of String) = _settings.TargetFolders
            Dim maxCount As Integer = _settings.MaxImportCount
            Dim repo As Data.EmailRepository = _repo
            Dim settings As Config.AppSettings = _settings

            Dim staThread As New System.Threading.Thread(
                Sub()
                    Try
                        Using outlookSvc As Services.OutlookService = Services.OutlookService.Connect()
                            Dim threadingSvc As New Services.ThreadingService(repo)
                            Dim importSvc As New Services.ImportService(outlookSvc, repo, threadingSvc, settings)
                            tcs.SetResult(importSvc.ImportFolders(targetFolders, maxCount, progress))
                        End Using
                    Catch ex As Exception
                        tcs.SetException(ex)
                    End Try
                End Sub)
            staThread.SetApartmentState(System.Threading.ApartmentState.STA)
            staThread.IsBackground = True
            staThread.Start()

            Dim result As Services.ImportResult = Await tcs.Task

            Dim msg As String = String.Format(
                "取り込み完了{0}取り込み: {1}件 / スキップ: {2}件 / エラー: {3}件",
                vbCrLf, result.ImportedCount, result.SkippedCount, result.ErrorCount)
            If result.ErrorCount > 0 Then
                msg &= vbCrLf & vbCrLf & "エラー詳細:" & vbCrLf & String.Join(vbCrLf, result.Errors)
            End If
            MessageBox.Show(msg, "取り込み結果", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' フォルダツリーとメール一覧を更新
            LoadFolderTree()
            LoadEmails(_currentFolder)

        Catch ex As Exception
            MessageBox.Show("取り込みエラー:" & vbCrLf & ex.Message, "エラー",
                MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _isImporting = False
            btnImportNow.Enabled = True
            menuItemImportNow.Enabled = True
            UpdateStatusBar()
        End Try
    End Function

    ' ════════════════════════════════════════════════════════════
    '  自動取り込み
    ' ════════════════════════════════════════════════════════════

    Private Sub StartAutoImport()
        _autoImportEnabled = True
        _autoImportTimer.Start()
        btnAutoImport.Text = "自動: ●"
        btnAutoImport.ToolTipText = "自動取り込み停止（クリックで停止）"
    End Sub

    Private Sub StopAutoImport()
        _autoImportEnabled = False
        _autoImportTimer.Stop()
        btnAutoImport.Text = "自動: ▶"
        btnAutoImport.ToolTipText = "自動取り込み開始（クリックで開始）"
    End Sub

    Private Sub btnAutoImport_Click(sender As Object, e As EventArgs) Handles btnAutoImport.Click
        If _autoImportEnabled Then
            StopAutoImport()
        Else
            StartAutoImport()
        End If
    End Sub

    Private Async Sub AutoImportTimer_Tick(sender As Object, e As EventArgs)
        Await RunImportAsync()
    End Sub

    ' ════════════════════════════════════════════════════════════
    '  検索
    ' ════════════════════════════════════════════════════════════

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        RunSearch()
    End Sub

    Private Sub menuItemSearch_Click(sender As Object, e As EventArgs) Handles menuItemSearch.Click
        ' 検索ボックスにフォーカスを移してユーザーが入力できる状態にする
        txtSearch.Focus()
        txtSearch.SelectAll()
    End Sub

    Private Sub txtSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            RunSearch()
        End If
    End Sub

    Private Sub RunSearch()
        Dim query As String = txtSearch.Text.Trim()
        _searchQuery = If(String.IsNullOrEmpty(query), Nothing, query)

        If String.IsNullOrEmpty(query) Then
            ' クエリ空 → 現在のフォルダを再表示
            LoadEmails(_currentFolder)
            Return
        End If

        Try
            _emailCache = _repo.SearchEmails(query)
            listViewEmails.VirtualListSize = _emailCache.Count
            listViewEmails.Invalidate()
            ' プレビューをクリア（新しい検索結果を選択するまで）
            emailPreview.ClearPreview()
            conversationView.ClearView()
            lblStatusCount.Text = String.Format("検索結果: {0}件", _emailCache.Count)
        Catch ex As Exception
            MessageBox.Show("検索エラー:" & vbCrLf & ex.Message, "エラー",
                MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ════════════════════════════════════════════════════════════
    '  削除
    ' ════════════════════════════════════════════════════════════

    ''' <summary>メール一覧で選択中のメールを削除する（添付ファイル実体も削除）。</summary>
    Private Sub DeleteSelectedEmails()
        If listViewEmails.SelectedIndices.Count = 0 Then Return

        Dim selectedIds As New List(Of Integer)()
        For Each idx As Integer In listViewEmails.SelectedIndices
            If idx >= 0 AndAlso idx < _emailCache.Count Then
                selectedIds.Add(_emailCache(idx).Id)
            End If
        Next
        If selectedIds.Count = 0 Then Return

        Dim msg As String
        If selectedIds.Count = 1 Then
            msg = "選択したメールを削除しますか？" & vbCrLf &
                  "添付ファイルも削除されます。この操作は元に戻せません。"
        Else
            msg = String.Format("選択した {0} 件のメールを削除しますか？" & vbCrLf &
                  "添付ファイルも削除されます。この操作は元に戻せません。", selectedIds.Count)
        End If

        If MessageBox.Show(msg, "削除の確認",
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Warning,
                           MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then
            Return
        End If

        ' プレビューをクリア
        emailPreview.ClearPreview()
        conversationView.ClearView()

        For Each emailId As Integer In selectedIds
            ' 添付ファイルの物理削除
            Dim attachments As List(Of Models.Attachment) = _repo.GetAttachmentsByEmailId(emailId)
            For Each att As Models.Attachment In attachments
                If IO.File.Exists(att.FilePath) Then
                    Try
                        IO.File.Delete(att.FilePath)
                    Catch
                        ' 物理削除失敗は無視して DB 削除を継続
                    End Try
                End If
            Next
            ' DB から削除（トゥームストーン記録を含む）
            _repo.DeleteEmail(emailId)
        Next

        LoadEmails(_currentFolder)
        UpdateStatusBar()
    End Sub

    ''' <summary>コンテキストメニュー「削除」クリック。</summary>
    Private Sub OnDeleteEmailMenuClick(sender As Object, e As EventArgs)
        DeleteSelectedEmails()
    End Sub

    ''' <summary>メール一覧で Delete キーが押されたときに選択メールを削除する。</summary>
    Private Sub listViewEmails_KeyDown(sender As Object, e As KeyEventArgs) Handles listViewEmails.KeyDown
        If e.KeyCode = Keys.Delete Then
            DeleteSelectedEmails()
            e.Handled = True
        End If
    End Sub

    ' ════════════════════════════════════════════════════════════
    '  ステータスバー
    ' ════════════════════════════════════════════════════════════

    Private Sub UpdateStatusBar()
        Try
            Dim count As Integer = _repo.GetTotalCount()
            lblStatusCount.Text = String.Format("総数 {0}件", count)
        Catch
            lblStatusCount.Text = "総数 -件"
        End Try
        lblStatusLastImport.Text = "最終取り込み: -"
    End Sub

    ' ════════════════════════════════════════════════════════════
    '  メニューハンドラ
    ' ════════════════════════════════════════════════════════════

    Private Sub menuItemFileExit_Click(sender As Object, e As EventArgs) Handles menuItemFileExit.Click
        Me.Close()
    End Sub

    Private Sub menuItemSettings_Click(sender As Object, e As EventArgs) Handles menuItemSettings.Click
        Using frm As New SettingsForm()
            frm.ShowDialog(Me)
        End Using
        ' OK/キャンセルどちらでも設定を再適用（設定が変わった場合のみ影響あり）
        ApplySettings()
    End Sub

    ''' <summary>設定変更をランタイムに反映する（タイマー間隔・自動取り込み開始/停止）。</summary>
    Private Sub ApplySettings()
        _autoImportTimer.Interval = _settings.AutoImportIntervalMinutes * 60 * 1000
        If _settings.AutoImportEnabled AndAlso Not _autoImportEnabled Then
            StartAutoImport()
        ElseIf Not _settings.AutoImportEnabled AndAlso _autoImportEnabled Then
            StopAutoImport()
        End If
    End Sub

    Private Sub menuItemHelpAbout_Click(sender As Object, e As EventArgs) Handles menuItemHelpAbout.Click
        MessageBox.Show("OutlookArchiver" & vbCrLf & "Outlook メールアーカイブツール",
            "バージョン情報", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

End Class
