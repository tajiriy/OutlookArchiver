Option Explicit On
Option Strict On
Option Infer Off

Imports NUnit.Framework
Imports OutlookVault.Data
Imports OutlookVault.Models
Imports OutlookVault.Services
Imports System.IO

Namespace Tests

    <TestFixture>
    Public Class AutoDeleteRuleRepositoryTests

        Private _testDbPath As String
        Private _dbManager As DatabaseManager
        Private _ruleRepo As AutoDeleteRuleRepository

        <SetUp>
        Public Sub SetUp()
            _testDbPath = Path.Combine(Path.GetTempPath(), "OutlookVaultTest_" & Guid.NewGuid().ToString("N") & ".db")
            _dbManager = New DatabaseManager(_testDbPath)
            _dbManager.Initialize()
            _ruleRepo = New AutoDeleteRuleRepository(_dbManager)
        End Sub

        <TearDown>
        Public Sub TearDown()
            If File.Exists(_testDbPath) Then File.Delete(_testDbPath)
            If File.Exists(_testDbPath & "-wal") Then File.Delete(_testDbPath & "-wal")
            If File.Exists(_testDbPath & "-shm") Then File.Delete(_testDbPath & "-shm")
        End Sub

        <Test>
        Public Sub InsertRule_ReturnsPositiveId()
            Dim id As Integer = _ruleRepo.InsertRule("テストルール", "差出人: noreply")
            Assert.Greater(id, 0)
        End Sub

        <Test>
        Public Sub GetAllRules_ReturnsInsertedRules()
            _ruleRepo.InsertRule("ルール1", "差出人: noreply")
            _ruleRepo.InsertRule("ルール2", "件名: 広告")

            Dim rules As List(Of AutoDeleteRule) = _ruleRepo.GetAllRules()
            Assert.AreEqual(2, rules.Count)
            Assert.AreEqual("ルール1", rules(0).Name)
            Assert.AreEqual("ルール2", rules(1).Name)
        End Sub

        <Test>
        Public Sub GetEnabledRules_ExcludesDisabled()
            Dim id1 As Integer = _ruleRepo.InsertRule("有効", "差出人: a")
            Dim id2 As Integer = _ruleRepo.InsertRule("無効", "差出人: b")
            _ruleRepo.SetEnabled(id2, False)

            Dim rules As List(Of AutoDeleteRule) = _ruleRepo.GetEnabledRules()
            Assert.AreEqual(1, rules.Count)
            Assert.AreEqual("有効", rules(0).Name)
        End Sub

        <Test>
        Public Sub UpdateRule_ChangesNameAndFilter()
            Dim id As Integer = _ruleRepo.InsertRule("旧名前", "旧フィルタ")

            _ruleRepo.UpdateRule(id, "新名前", "新フィルタ", True)

            Dim rules As List(Of AutoDeleteRule) = _ruleRepo.GetAllRules()
            Assert.AreEqual("新名前", rules(0).Name)
            Assert.AreEqual("新フィルタ", rules(0).FilterExpression)
        End Sub

        <Test>
        Public Sub DeleteRule_RemovesFromDb()
            Dim id As Integer = _ruleRepo.InsertRule("削除対象", "件名: test")

            _ruleRepo.DeleteRule(id)

            Assert.AreEqual(0, _ruleRepo.GetRuleCount())
        End Sub

        <Test>
        Public Sub SetEnabled_TogglesState()
            Dim id As Integer = _ruleRepo.InsertRule("切替テスト", "件名: test")
            Assert.IsTrue(_ruleRepo.GetAllRules()(0).Enabled)

            _ruleRepo.SetEnabled(id, False)
            Assert.IsFalse(_ruleRepo.GetAllRules()(0).Enabled)

            _ruleRepo.SetEnabled(id, True)
            Assert.IsTrue(_ruleRepo.GetAllRules()(0).Enabled)
        End Sub

    End Class

    <TestFixture>
    Public Class AutoDeleteServiceTests

        Private _testDbPath As String
        Private _dbManager As DatabaseManager
        Private _emailRepo As EmailRepository
        Private _ruleRepo As AutoDeleteRuleRepository
        Private _svc As AutoDeleteService

        <SetUp>
        Public Sub SetUp()
            _testDbPath = Path.Combine(Path.GetTempPath(), "OutlookVaultTest_" & Guid.NewGuid().ToString("N") & ".db")
            _dbManager = New DatabaseManager(_testDbPath)
            _dbManager.Initialize()
            _emailRepo = New EmailRepository(_dbManager)
            _ruleRepo = New AutoDeleteRuleRepository(_dbManager)
            _svc = New AutoDeleteService(_ruleRepo, _emailRepo)
        End Sub

        <TearDown>
        Public Sub TearDown()
            If File.Exists(_testDbPath) Then File.Delete(_testDbPath)
            If File.Exists(_testDbPath & "-wal") Then File.Delete(_testDbPath & "-wal")
            If File.Exists(_testDbPath & "-shm") Then File.Delete(_testDbPath & "-shm")
        End Sub

        Private Function CreateTestEmail(messageId As String, Optional subject As String = "テスト件名",
                                          Optional senderEmail As String = "sender@example.com") As Email
            Dim email As New Email()
            email.MessageId = messageId
            email.Subject = subject
            email.NormalizedSubject = subject
            email.SenderName = "テスト送信者"
            email.SenderEmail = senderEmail
            email.ReceivedAt = DateTime.Now
            email.FolderName = "受信トレイ"
            Return email
        End Function

        <Test>
        Public Sub ApplyRulesToNewEmails_NoRules_ReturnsZero()
            Dim id As Integer = _emailRepo.InsertEmail(CreateTestEmail("a@test.com"))

            Dim deleted As Integer = _svc.ApplyRulesToNewEmails(New List(Of Integer)() From {id})

            Assert.AreEqual(0, deleted)
        End Sub

        <Test>
        Public Sub ApplyRulesToNewEmails_MatchingRule_SoftDeletes()
            _ruleRepo.InsertRule("noreply削除", "メール: noreply")
            Dim id1 As Integer = _emailRepo.InsertEmail(CreateTestEmail("a@test.com", "お知らせ", "noreply@example.com"))
            Dim id2 As Integer = _emailRepo.InsertEmail(CreateTestEmail("b@test.com", "重要", "boss@example.com"))

            Dim deleted As Integer = _svc.ApplyRulesToNewEmails(New List(Of Integer)() From {id1, id2})

            Assert.AreEqual(1, deleted)
            ' id1 はゴミ箱に
            Assert.AreEqual(1, _emailRepo.GetTrashCount())
            ' id2 は残っている
            Assert.AreEqual(1, _emailRepo.GetEmailsForList().Count)
        End Sub

        <Test>
        Public Sub ApplyRulesToNewEmails_DisabledRule_NotApplied()
            Dim ruleId As Integer = _ruleRepo.InsertRule("無効ルール", "メール: noreply")
            _ruleRepo.SetEnabled(ruleId, False)
            Dim id As Integer = _emailRepo.InsertEmail(CreateTestEmail("a@test.com", "お知らせ", "noreply@example.com"))

            Dim deleted As Integer = _svc.ApplyRulesToNewEmails(New List(Of Integer)() From {id})

            Assert.AreEqual(0, deleted)
        End Sub

        <Test>
        Public Sub ApplyRulesToNewEmails_OnlyScopedIds()
            _ruleRepo.InsertRule("全削除", "件名: テスト")
            Dim id1 As Integer = _emailRepo.InsertEmail(CreateTestEmail("a@test.com", "テスト件名"))
            Dim id2 As Integer = _emailRepo.InsertEmail(CreateTestEmail("b@test.com", "テスト件名"))

            ' id1 のみを対象として適用
            Dim deleted As Integer = _svc.ApplyRulesToNewEmails(New List(Of Integer)() From {id1})

            Assert.AreEqual(1, deleted)
            ' id2 はスコープ外なので残っている
            Assert.AreEqual(1, _emailRepo.GetEmailsForList().Count)
        End Sub

        <Test>
        Public Sub ApplyRulesToExistingEmails_MatchingRule_SoftDeletes()
            _ruleRepo.InsertRule("広告削除", "件名: 広告")
            _emailRepo.InsertEmail(CreateTestEmail("a@test.com", "広告メール"))
            _emailRepo.InsertEmail(CreateTestEmail("b@test.com", "重要メール"))

            Dim deleted As Integer = _svc.ApplyRulesToExistingEmails()

            Assert.AreEqual(1, deleted)
            Assert.AreEqual(1, _emailRepo.GetTrashCount())
        End Sub

        <Test>
        Public Sub PreviewRule_ReturnsMatchingEmails()
            _emailRepo.InsertEmail(CreateTestEmail("a@test.com", "広告メール"))
            _emailRepo.InsertEmail(CreateTestEmail("b@test.com", "重要メール"))

            Dim matched As List(Of Email) = _svc.PreviewRule("件名: 広告")

            Assert.AreEqual(1, matched.Count)
        End Sub

    End Class

End Namespace
