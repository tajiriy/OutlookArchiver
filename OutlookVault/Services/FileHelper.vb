Option Explicit On
Option Strict On
Option Infer Off

Namespace Services

    ''' <summary>
    ''' ファイル関連のユーティリティメソッドを提供する。
    ''' </summary>
    Public Class FileHelper

        ''' <summary>バイト数を人間可読な形式（B / KB / MB / GB）に変換する。</summary>
        Public Shared Function FormatFileSize(bytes As Long) As String
            If bytes < 1024L Then
                Return bytes.ToString("#,##0") & " B"
            ElseIf bytes < 1024L * 1024L Then
                Return (bytes / 1024.0).ToString("#,##0.0") & " KB"
            ElseIf bytes < 1024L * 1024L * 1024L Then
                Return (bytes / (1024.0 * 1024.0)).ToString("#,##0.0") & " MB"
            Else
                Return (bytes / (1024.0 * 1024.0 * 1024.0)).ToString("#,##0.0") & " GB"
            End If
        End Function

    End Class

End Namespace
