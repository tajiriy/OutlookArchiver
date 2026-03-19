Option Explicit On
Option Strict On
Option Infer Off

Imports System.Text.RegularExpressions

Namespace Services

    ''' <summary>
    ''' メール HTML から危険な要素（スクリプト、iframe、イベントハンドラ等）を除去するサニタイザー。
    ''' アプリ側で注入するスクリプト（検索ハイライト等）はサニタイズ後に追加するため影響を受けない。
    ''' </summary>
    Public Class HtmlSanitizerService

        ''' <summary>ブロック対象のタグ名（小文字）。開始タグ・終了タグおよびその内容をすべて除去する。</summary>
        Private Shared ReadOnly BlockedTags() As String = {
            "script", "iframe", "frame", "frameset",
            "object", "embed", "applet", "link", "meta"
        }

        ' ── コンパイル済み正規表現（Shared ReadOnly でインスタンス共有） ──
        ''' <summary>ブロックタグのペア除去パターン（タグ名ごとに生成）。</summary>
        Private Shared ReadOnly BlockedTagPairPatterns As Dictionary(Of String, Regex) = BuildBlockedTagPairPatterns()
        ''' <summary>ブロックタグの自己閉じ除去パターン（タグ名ごとに生成）。</summary>
        Private Shared ReadOnly BlockedTagSelfPatterns As Dictionary(Of String, Regex) = BuildBlockedTagSelfPatterns()
        ''' <summary>イベントハンドラ属性除去パターン。</summary>
        Private Shared ReadOnly EventHandlerPattern As New Regex(
            "\s+on\w+\s*=\s*(?:""[^""]*""|'[^']*'|[^\s>]+)",
            RegexOptions.IgnoreCase Or RegexOptions.Compiled)
        ''' <summary>javascript: スキーム除去パターン。</summary>
        Private Shared ReadOnly JavascriptSchemePattern As New Regex(
            "(href|src|action)\s*=\s*([""'])?\s*javascript\s*:[^""'>]*\2?",
            RegexOptions.IgnoreCase Or RegexOptions.Compiled)
        ''' <summary>vbscript: スキーム除去パターン。</summary>
        Private Shared ReadOnly VbscriptSchemePattern As New Regex(
            "(href|src|action)\s*=\s*([""'])?\s*vbscript\s*:[^""'>]*\2?",
            RegexOptions.IgnoreCase Or RegexOptions.Compiled)
        ''' <summary>data: スキーム除去パターン（data:image は許可）。</summary>
        Private Shared ReadOnly DataSchemePattern As New Regex(
            "(href|src|action)\s*=\s*([""'])\s*data\s*:(?!image/)[^""']*\2",
            RegexOptions.IgnoreCase Or RegexOptions.Compiled)
        ''' <summary>CSS expression() 除去パターン。</summary>
        Private Shared ReadOnly CssExpressionPattern As New Regex(
            "expression\s*\(",
            RegexOptions.IgnoreCase Or RegexOptions.Compiled)
        ''' <summary>CSS -moz-binding / behavior 除去パターン。</summary>
        Private Shared ReadOnly CssBindingPattern As New Regex(
            "(-moz-binding|behavior)\s*:\s*[^;""'}>]+",
            RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        Private Shared Function BuildBlockedTagPairPatterns() As Dictionary(Of String, Regex)
            Dim result As New Dictionary(Of String, Regex)()
            For Each tag As String In BlockedTags
                result(tag) = New Regex(
                    "<" & tag & "\b[^>]*>.*?</" & tag & "\s*>",
                    RegexOptions.Singleline Or RegexOptions.IgnoreCase Or RegexOptions.Compiled)
            Next
            Return result
        End Function

        Private Shared Function BuildBlockedTagSelfPatterns() As Dictionary(Of String, Regex)
            Dim result As New Dictionary(Of String, Regex)()
            For Each tag As String In BlockedTags
                result(tag) = New Regex(
                    "<" & tag & "\b[^>]*/?\s*>",
                    RegexOptions.Singleline Or RegexOptions.IgnoreCase Or RegexOptions.Compiled)
            Next
            Return result
        End Function

        ''' <summary>
        ''' 指定された HTML 文字列から危険な要素を除去して返す。
        ''' </summary>
        Public Function Sanitize(html As String) As String
            If String.IsNullOrEmpty(html) Then Return html

            Dim result As String = html

            ' 1. 危険なタグとその内容を除去（<script>...</script> 等）
            For Each tag As String In BlockedTags
                result = BlockedTagPairPatterns(tag).Replace(result, String.Empty)
                result = BlockedTagSelfPatterns(tag).Replace(result, String.Empty)
            Next

            ' 2. イベントハンドラ属性を除去（on*="..."）
            result = EventHandlerPattern.Replace(result, String.Empty)

            ' 3. javascript: スキームを除去
            result = JavascriptSchemePattern.Replace(result, "$1=$2about:blank$2")

            ' 4. vbscript: スキームを除去
            result = VbscriptSchemePattern.Replace(result, "$1=$2about:blank$2")

            ' 5. data: スキームを除去（画像以外）
            result = DataSchemePattern.Replace(result, "$1=$2about:blank$2")

            ' 6. expression() を除去（IE の CSS expression）
            result = CssExpressionPattern.Replace(result, "blocked(")

            ' 7. style 属性内の -moz-binding / behavior を除去
            result = CssBindingPattern.Replace(result, String.Empty)

            Return result
        End Function

    End Class

End Namespace
