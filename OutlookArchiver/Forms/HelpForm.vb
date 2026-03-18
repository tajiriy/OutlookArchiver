Option Explicit On
Option Strict On
Option Infer Off

Imports System.IO
Imports System.Windows.Forms

Namespace Forms

    Public Class HelpForm
        Inherits Form

        Private WithEvents webBrowser As WebBrowser

        Public Sub New()
            Me.Text = "ユーザーマニュアル - OutlookArchiver"
            Me.Size = New Drawing.Size(920, 720)
            Me.StartPosition = FormStartPosition.CenterParent
            Me.MinimumSize = New Drawing.Size(600, 400)

            webBrowser = New WebBrowser()
            webBrowser.Dock = DockStyle.Fill
            webBrowser.ScriptErrorsSuppressed = True
            Me.Controls.Add(webBrowser)

            Dim helpPath As String = Path.Combine(Application.StartupPath, "Help", "user-manual.html")
            If File.Exists(helpPath) Then
                webBrowser.Navigate(helpPath)
            Else
                webBrowser.DocumentText = "<html><body style='font-family:Meiryo UI;padding:32px;'>" &
                    "<h2>ユーザーマニュアルが見つかりません</h2>" &
                    "<p>以下のパスにファイルが存在しません:</p>" &
                    "<pre>" & helpPath & "</pre></body></html>"
            End If
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing Then
                If webBrowser IsNot Nothing Then
                    webBrowser.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

    End Class

End Namespace
