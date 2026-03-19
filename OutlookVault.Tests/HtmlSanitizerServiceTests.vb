Option Explicit On
Option Strict On
Option Infer Off

Imports NUnit.Framework
Imports OutlookVault.Services

Namespace Tests

    <TestFixture>
    Public Class HtmlSanitizerServiceTests

        Private _sanitizer As HtmlSanitizerService

        <SetUp>
        Public Sub SetUp()
            _sanitizer = New HtmlSanitizerService()
        End Sub

        ' ── script タグの除去 ────────────────────────────────────

        <Test>
        Public Sub Sanitize_RemovesScriptTag()
            Dim html As String = "<html><body><p>Hello</p><script>alert('xss')</script></body></html>"
            Dim result As String = _sanitizer.Sanitize(html)
            Assert.That(result, Does.Not.Contain("<script"))
            Assert.That(result, Does.Not.Contain("alert"))
            Assert.That(result, Does.Contain("<p>Hello</p>"))
        End Sub

        <Test>
        Public Sub Sanitize_RemovesScriptTagCaseInsensitive()
            Dim html As String = "<html><body><SCRIPT>alert('xss')</SCRIPT></body></html>"
            Dim result As String = _sanitizer.Sanitize(html)
            Assert.That(result, Does.Not.Contain("alert"))
        End Sub

        <Test>
        Public Sub Sanitize_RemovesScriptWithAttributes()
            Dim html As String = "<html><body><script type=""text/javascript"" src=""evil.js""></script></body></html>"
            Dim result As String = _sanitizer.Sanitize(html)
            Assert.That(result, Does.Not.Contain("<script"))
            Assert.That(result, Does.Not.Contain("evil.js"))
        End Sub

        ' ── iframe / object / embed の除去 ────────────────────────

        <Test>
        Public Sub Sanitize_RemovesIframe()
            Dim html As String = "<html><body><iframe src=""http://evil.com""></iframe><p>Safe</p></body></html>"
            Dim result As String = _sanitizer.Sanitize(html)
            Assert.That(result, Does.Not.Contain("<iframe"))
            Assert.That(result, Does.Contain("<p>Safe</p>"))
        End Sub

        <Test>
        Public Sub Sanitize_RemovesObjectTag()
            Dim html As String = "<html><body><object data=""malware.swf""></object></body></html>"
            Dim result As String = _sanitizer.Sanitize(html)
            Assert.That(result, Does.Not.Contain("<object"))
        End Sub

        <Test>
        Public Sub Sanitize_RemovesEmbedTag()
            Dim html As String = "<html><body><embed src=""plugin.swf"" /></body></html>"
            Dim result As String = _sanitizer.Sanitize(html)
            Assert.That(result, Does.Not.Contain("<embed"))
        End Sub

        ' ── イベントハンドラ属性の除去 ──────────────────────────────

        <Test>
        Public Sub Sanitize_RemovesOnClickAttribute()
            Dim html As String = "<html><body><a href=""#"" onclick=""alert('xss')"">Click</a></body></html>"
            Dim result As String = _sanitizer.Sanitize(html)
            Assert.That(result, Does.Not.Contain("onclick"))
            Assert.That(result, Does.Contain(">Click</a>"))
        End Sub

        <Test>
        Public Sub Sanitize_RemovesOnErrorAttribute()
            Dim html As String = "<img src=""x"" onerror=""alert('xss')"" />"
            Dim result As String = _sanitizer.Sanitize(html)
            Assert.That(result, Does.Not.Contain("onerror"))
            Assert.That(result, Does.Contain("<img"))
        End Sub

        <Test>
        Public Sub Sanitize_RemovesOnLoadAttribute()
            Dim html As String = "<body onload=""alert('xss')"">"
            Dim result As String = _sanitizer.Sanitize(html)
            Assert.That(result, Does.Not.Contain("onload"))
            Assert.That(result, Does.Contain("<body"))
        End Sub

        ' ── javascript: スキームの除去 ─────────────────────────────

        <Test>
        Public Sub Sanitize_RemovesJavascriptHref()
            Dim html As String = "<a href=""javascript:alert('xss')"">Link</a>"
            Dim result As String = _sanitizer.Sanitize(html)
            Assert.That(result, Does.Not.Contain("javascript:"))
            Assert.That(result, Does.Contain("about:blank"))
        End Sub

        <Test>
        Public Sub Sanitize_RemovesVbscriptHref()
            Dim html As String = "<a href=""vbscript:MsgBox('xss')"">Link</a>"
            Dim result As String = _sanitizer.Sanitize(html)
            Assert.That(result, Does.Not.Contain("vbscript:"))
            Assert.That(result, Does.Contain("about:blank"))
        End Sub

        ' ── data: スキーム ────────────────────────────────────────

        <Test>
        Public Sub Sanitize_BlocksDataSchemeNonImage()
            Dim html As String = "<a href=""data:text/html,<script>alert('xss')</script>"">Click</a>"
            Dim result As String = _sanitizer.Sanitize(html)
            Assert.That(result, Does.Not.Contain("data:text"))
        End Sub

        <Test>
        Public Sub Sanitize_AllowsDataImageScheme()
            Dim html As String = "<img src=""data:image/png;base64,iVBOR..."" />"
            Dim result As String = _sanitizer.Sanitize(html)
            Assert.That(result, Does.Contain("data:image/png"))
        End Sub

        ' ── CSS expression の除去 ─────────────────────────────────

        <Test>
        Public Sub Sanitize_RemovesCssExpression()
            Dim html As String = "<div style=""width:expression(alert('xss'))"">Text</div>"
            Dim result As String = _sanitizer.Sanitize(html)
            Assert.That(result, Does.Not.Contain("expression("))
        End Sub

        ' ── 正常な HTML が保持されること ──────────────────────────────

        <Test>
        Public Sub Sanitize_PreservesNormalHtml()
            Dim html As String = "<html><body><h1>Title</h1><p>Hello <b>world</b></p><img src=""photo.jpg"" /></body></html>"
            Dim result As String = _sanitizer.Sanitize(html)
            Assert.AreEqual(html, result)
        End Sub

        <Test>
        Public Sub Sanitize_PreservesNormalLinks()
            Dim html As String = "<a href=""https://example.com"">Link</a>"
            Dim result As String = _sanitizer.Sanitize(html)
            Assert.That(result, Does.Contain("https://example.com"))
        End Sub

        ' ── 空・Nothing の処理 ────────────────────────────────────

        <Test>
        Public Sub Sanitize_ReturnsNothingForNothing()
            Dim result As String = _sanitizer.Sanitize(Nothing)
            Assert.IsNull(result)
        End Sub

        <Test>
        Public Sub Sanitize_ReturnsEmptyForEmpty()
            Dim result As String = _sanitizer.Sanitize(String.Empty)
            Assert.AreEqual(String.Empty, result)
        End Sub

        ' ── link / meta タグの除去 ────────────────────────────────

        <Test>
        Public Sub Sanitize_RemovesLinkTag()
            Dim html As String = "<html><head><link rel=""stylesheet"" href=""http://evil.com/style.css""></head><body>OK</body></html>"
            Dim result As String = _sanitizer.Sanitize(html)
            Assert.That(result, Does.Not.Contain("<link"))
        End Sub

        <Test>
        Public Sub Sanitize_RemovesMetaRefresh()
            Dim html As String = "<html><head><meta http-equiv=""refresh"" content=""0;url=http://evil.com""></head><body>OK</body></html>"
            Dim result As String = _sanitizer.Sanitize(html)
            Assert.That(result, Does.Not.Contain("<meta"))
        End Sub

    End Class

End Namespace
