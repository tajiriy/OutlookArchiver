Option Explicit On
Option Strict On
Option Infer Off

Namespace Forms

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class EmailViewForm
    Inherits System.Windows.Forms.Form

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.toolStrip = New System.Windows.Forms.ToolStrip()
        Me.btnToggleView = New System.Windows.Forms.ToolStripButton()
        Me.emailPreview = New OutlookArchiver.Controls.EmailPreviewControl()
        Me.toolStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        ' toolStrip
        '
        Me.toolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnToggleView})
        Me.toolStrip.Location = New System.Drawing.Point(0, 0)
        Me.toolStrip.Name = "toolStrip"
        Me.toolStrip.Size = New System.Drawing.Size(800, 25)
        Me.toolStrip.TabIndex = 0
        '
        ' btnToggleView
        '
        Me.btnToggleView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnToggleView.Name = "btnToggleView"
        Me.btnToggleView.Size = New System.Drawing.Size(70, 22)
        Me.btnToggleView.Text = "テキスト表示"
        Me.btnToggleView.Visible = False
        '
        ' emailPreview
        '
        Me.emailPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.emailPreview.Location = New System.Drawing.Point(0, 25)
        Me.emailPreview.Name = "emailPreview"
        Me.emailPreview.Size = New System.Drawing.Size(800, 575)
        Me.emailPreview.TabIndex = 1
        '
        ' EmailViewForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 12.0F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.Controls.Add(Me.emailPreview)
        Me.Controls.Add(Me.toolStrip)
        Me.Name = "EmailViewForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "メール表示"
        Me.toolStrip.ResumeLayout(False)
        Me.toolStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents toolStrip As System.Windows.Forms.ToolStrip
    Friend WithEvents btnToggleView As System.Windows.Forms.ToolStripButton
    Friend WithEvents emailPreview As OutlookArchiver.Controls.EmailPreviewControl

    End Class

End Namespace
