Option Explicit On
Option Strict On
Option Infer Off

Namespace Forms

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class AboutForm
        Inherits System.Windows.Forms.Form

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing Then
                    If picIcon IsNot Nothing AndAlso picIcon.Image IsNot Nothing Then
                        picIcon.Image.Dispose()
                    End If
                    If components IsNot Nothing Then components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        Private components As System.ComponentModel.IContainer

        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Me.picIcon = New System.Windows.Forms.PictureBox()
            Me.lblAppName = New System.Windows.Forms.Label()
            Me.lblVersion = New System.Windows.Forms.Label()
            Me.lblDescription = New System.Windows.Forms.Label()
            Me.btnOk = New System.Windows.Forms.Button()
            CType(Me.picIcon, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            ' picIcon
            '
            Me.picIcon.Location = New System.Drawing.Point(24, 24)
            Me.picIcon.Name = "picIcon"
            Me.picIcon.Size = New System.Drawing.Size(64, 64)
            Me.picIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
            '
            ' lblAppName
            '
            Me.lblAppName.AutoSize = True
            Me.lblAppName.Font = New System.Drawing.Font(Me.Font.FontFamily, 14.0F, System.Drawing.FontStyle.Bold)
            Me.lblAppName.Location = New System.Drawing.Point(104, 24)
            Me.lblAppName.Name = "lblAppName"
            Me.lblAppName.Text = "OutlookVault"
            '
            ' lblVersion
            '
            Me.lblVersion.AutoSize = True
            Me.lblVersion.Location = New System.Drawing.Point(104, 56)
            Me.lblVersion.Name = "lblVersion"
            '
            ' lblDescription
            '
            Me.lblDescription.AutoSize = True
            Me.lblDescription.Location = New System.Drawing.Point(104, 80)
            Me.lblDescription.Name = "lblDescription"
            Me.lblDescription.Text = "Outlook メール保管ツール"
            '
            ' btnOk
            '
            Me.btnOk.Location = New System.Drawing.Point(124, 130)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(80, 28)
            Me.btnOk.Text = "OK"
            '
            ' AboutForm
            '
            Me.AcceptButton = Me.btnOk
            Me.ClientSize = New System.Drawing.Size(324, 171)
            Me.Controls.Add(Me.picIcon)
            Me.Controls.Add(Me.lblAppName)
            Me.Controls.Add(Me.lblVersion)
            Me.Controls.Add(Me.lblDescription)
            Me.Controls.Add(Me.btnOk)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "AboutForm"
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "バージョン情報"
            CType(Me.picIcon, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()
        End Sub

        Friend picIcon As System.Windows.Forms.PictureBox
        Friend lblAppName As System.Windows.Forms.Label
        Friend lblVersion As System.Windows.Forms.Label
        Friend lblDescription As System.Windows.Forms.Label
        Friend WithEvents btnOk As System.Windows.Forms.Button

    End Class

End Namespace
