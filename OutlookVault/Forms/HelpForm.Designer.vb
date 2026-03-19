Option Explicit On
Option Strict On
Option Infer Off

Namespace Forms

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class HelpForm
        Inherits System.Windows.Forms.Form

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing Then
                    If webBrowser IsNot Nothing Then webBrowser.Dispose()
                    If components IsNot Nothing Then components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        Private components As System.ComponentModel.IContainer

        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Me.webBrowser = New System.Windows.Forms.WebBrowser()
            Me.SuspendLayout()
            '
            'webBrowser
            '
            Me.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill
            Me.webBrowser.Location = New System.Drawing.Point(0, 0)
            Me.webBrowser.Name = "webBrowser"
            Me.webBrowser.ScriptErrorsSuppressed = True
            Me.webBrowser.Size = New System.Drawing.Size(1056, 681)
            Me.webBrowser.TabIndex = 0
            '
            'HelpForm
            '
            Me.ClientSize = New System.Drawing.Size(1056, 681)
            Me.Controls.Add(Me.webBrowser)
            Me.MinimumSize = New System.Drawing.Size(600, 400)
            Me.Name = "HelpForm"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "ユーザーマニュアル - OutlookVault"
            Me.ResumeLayout(False)

        End Sub

        Friend WithEvents webBrowser As System.Windows.Forms.WebBrowser

    End Class

End Namespace
