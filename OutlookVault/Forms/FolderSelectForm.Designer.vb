Option Explicit On
Option Strict On
Option Infer Off

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FolderSelectForm
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
        Me.txtFilter = New System.Windows.Forms.TextBox()
        Me.lblFilter = New System.Windows.Forms.Label()
        Me.clbFolders = New System.Windows.Forms.CheckedListBox()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        ' lblFilter
        '
        Me.lblFilter.AutoSize = True
        Me.lblFilter.Location = New System.Drawing.Point(8, 12)
        Me.lblFilter.Name = "lblFilter"
        Me.lblFilter.Text = "フィルタ:"
        '
        ' txtFilter
        '
        Me.txtFilter.Location = New System.Drawing.Point(68, 9)
        Me.txtFilter.Name = "txtFilter"
        Me.txtFilter.Size = New System.Drawing.Size(304, 22)
        Me.txtFilter.TabIndex = 0
        '
        ' clbFolders
        '
        Me.clbFolders.CheckOnClick = True
        Me.clbFolders.Location = New System.Drawing.Point(8, 38)
        Me.clbFolders.Name = "clbFolders"
        Me.clbFolders.Size = New System.Drawing.Size(364, 310)
        Me.clbFolders.TabIndex = 1
        '
        ' lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(8, 354)
        Me.lblStatus.Name = "lblStatus"
        '
        ' btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(212, 354)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(72, 26)
        Me.btnOk.TabIndex = 2
        Me.btnOk.Text = "OK"
        '
        ' btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(292, 354)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(80, 26)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "キャンセル"
        '
        ' FolderSelectForm
        '
        Me.AcceptButton = Me.btnOk
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(384, 390)
        Me.Controls.Add(Me.lblFilter)
        Me.Controls.Add(Me.txtFilter)
        Me.Controls.Add(Me.clbFolders)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.btnCancel)
        Me.Font = New System.Drawing.Font("Meiryo UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FolderSelectForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "対象フォルダの選択"
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents txtFilter As System.Windows.Forms.TextBox
    Friend lblFilter As System.Windows.Forms.Label
    Friend clbFolders As System.Windows.Forms.CheckedListBox
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend lblStatus As System.Windows.Forms.Label

End Class
