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

        ''' <summary>
        ''' 指定された HTML 文字列から危険な要素を除去して返す。
        ''' </summary>
        Public Function Sanitize(html As String) As String
            If String.IsNullOrEmpty(html) Then Return html

            Dim result As String = html

            ' 1. 危険なタグとその内容を除去（<script>...</script> 等）
            For Each tag As String In BlockedTags
                ' 開始～終了タグのペア（内容含む）を除去
                result = Regex.Replace(
                    result,
                    "<" & tag & "\b[^>]*>.*?</" & tag & "\s*>",
                    String.Empty,
                    RegexOptions.Singleline Or RegexOptions.IgnoreCase)
                ' 自己閉じタグを除去（<embed ... /> や <meta ...>）
                result = Regex.Replace(
                    result,
                    "<" & tag & "\b[^>]*/?\s*>",
                    String.Empty,
                    RegexOptions.Singleline Or RegexOptions.IgnoreCase)
            Next

            ' 2. イベントハンドラ属性を除去（on*="..."）
            '    on で始まる属性名を持つものを除去する（onclick, onerror, onload 等）
            result = Regex.Replace(
                result,
                "\s+on\w+\s*=\s*(?:""[^""]*""|'[^']*'|[^\s>]+)",
                String.Empty,
                RegexOptions.IgnoreCase)

            ' 3. javascript: スキームを除去（href="javascript:..." や src="javascript:..."）
            result = Regex.Replace(
                result,
                "(href|src|action)\s*=\s*([""'])?\s*javascript\s*:[^""'>]*\2?",
                "$1=$2about:blank$2",
                RegexOptions.IgnoreCase)

            ' 4. vbscript: スキームを除去
            result = Regex.Replace(
                result,
                "(href|src|action)\s*=\s*([""'])?\s*vbscript\s*:[^""'>]*\2?",
                "$1=$2about:blank$2",
                RegexOptions.IgnoreCase)

            ' 5. data: スキームを除去（画像以外。data:image は許可）
            result = Regex.Replace(
                result,
                "(href|src|action)\s*=\s*([""'])\s*data\s*:(?!image/)[^""']*\2",
                "$1=$2about:blank$2",
                RegexOptions.IgnoreCase)

            ' 6. expression() を除去（IE の CSS expression）
            result = Regex.Replace(
                result,
                "expression\s*\(",
                "blocked(",
                RegexOptions.IgnoreCase)

            ' 7. style 属性内の -moz-binding / behavior を除去
            result = Regex.Replace(
                result,
                "(-moz-binding|behavior)\s*:\s*[^;""'}>]+",
                String.Empty,
                RegexOptions.IgnoreCase)

            Return result
        End Function

    End Class

End Namespace
