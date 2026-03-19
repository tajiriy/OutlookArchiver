Option Explicit On
Option Strict On
Option Infer Off

Namespace Forms

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AutoDeleteRuleForm
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()> _
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
        Me.dgvRules = New System.Windows.Forms.DataGridView()
        Me.colName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colFilter = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colEnabled = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.panelButtons = New System.Windows.Forms.Panel()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnToggle = New System.Windows.Forms.Button()
        Me.btnReapply = New System.Windows.Forms.Button()
        Me.lblCount = New System.Windows.Forms.Label()
        Me.btnClose = New System.Windows.Forms.Button()
        CType(Me.dgvRules, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.panelButtons.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvRules
        '
        Me.dgvRules.AllowUserToAddRows = False
        Me.dgvRules.AllowUserToDeleteRows = False
        Me.dgvRules.AllowUserToResizeRows = False
        Me.dgvRules.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvRules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvRules.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colName, Me.colFilter, Me.colEnabled})
        Me.dgvRules.Location = New System.Drawing.Point(12, 12)
        Me.dgvRules.MultiSelect = False
        Me.dgvRules.Name = "dgvRules"
        Me.dgvRules.ReadOnly = True
        Me.dgvRules.RowHeadersVisible = False
        Me.dgvRules.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvRules.Size = New System.Drawing.Size(560, 300)
        Me.dgvRules.TabIndex = 0
        '
        'colName
        '
        Me.colName.HeaderText = "ルール名"
        Me.colName.Name = "colName"
        Me.colName.ReadOnly = True
        Me.colName.Width = 180
        '
        'colFilter
        '
        Me.colFilter.HeaderText = "フィルタ式"
        Me.colFilter.Name = "colFilter"
        Me.colFilter.ReadOnly = True
        Me.colFilter.Width = 280
        '
        'colEnabled
        '
        Me.colEnabled.HeaderText = "有効"
        Me.colEnabled.Name = "colEnabled"
        Me.colEnabled.ReadOnly = True
        Me.colEnabled.Width = 50
        '
        'panelButtons
        '
        Me.panelButtons.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panelButtons.Controls.Add(Me.btnAdd)
        Me.panelButtons.Controls.Add(Me.btnEdit)
        Me.panelButtons.Controls.Add(Me.btnDelete)
        Me.panelButtons.Controls.Add(Me.btnToggle)
        Me.panelButtons.Controls.Add(Me.btnReapply)
        Me.panelButtons.Location = New System.Drawing.Point(580, 12)
        Me.panelButtons.Name = "panelButtons"
        Me.panelButtons.Size = New System.Drawing.Size(100, 200)
        Me.panelButtons.TabIndex = 1
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(0, 0)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(100, 28)
        Me.btnAdd.TabIndex = 0
        Me.btnAdd.Text = "追加(&A)"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnEdit
        '
        Me.btnEdit.Location = New System.Drawing.Point(0, 34)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(100, 28)
        Me.btnEdit.TabIndex = 1
        Me.btnEdit.Text = "編集(&E)"
        Me.btnEdit.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(0, 68)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(100, 28)
        Me.btnDelete.TabIndex = 2
        Me.btnDelete.Text = "削除(&D)"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnToggle
        '
        Me.btnToggle.Location = New System.Drawing.Point(0, 102)
        Me.btnToggle.Name = "btnToggle"
        Me.btnToggle.Size = New System.Drawing.Size(100, 28)
        Me.btnToggle.TabIndex = 3
        Me.btnToggle.Text = "有効/無効"
        Me.btnToggle.UseVisualStyleBackColor = True
        '
        'btnReapply
        '
        Me.btnReapply.Location = New System.Drawing.Point(0, 150)
        Me.btnReapply.Name = "btnReapply"
        Me.btnReapply.Size = New System.Drawing.Size(100, 28)
        Me.btnReapply.TabIndex = 4
        Me.btnReapply.Text = "再適用(&R)"
        Me.btnReapply.UseVisualStyleBackColor = True
        '
        'lblCount
        '
        Me.lblCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblCount.AutoSize = True
        Me.lblCount.Location = New System.Drawing.Point(12, 322)
        Me.lblCount.Name = "lblCount"
        Me.lblCount.Size = New System.Drawing.Size(0, 12)
        Me.lblCount.TabIndex = 2
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.Location = New System.Drawing.Point(600, 318)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(80, 28)
        Me.btnClose.TabIndex = 3
        Me.btnClose.Text = "閉じる"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'AutoDeleteRuleForm
        '
        Me.CancelButton = Me.btnClose
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(692, 353)
        Me.Controls.Add(Me.dgvRules)
        Me.Controls.Add(Me.panelButtons)
        Me.Controls.Add(Me.lblCount)
        Me.Controls.Add(Me.btnClose)
        Me.MinimizeBox = False
        Me.Name = "AutoDeleteRuleForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "自動削除ルール"
        CType(Me.dgvRules, System.ComponentModel.ISupportInitialize).EndInit()
        Me.panelButtons.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents dgvRules As System.Windows.Forms.DataGridView
    Friend WithEvents colName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colFilter As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colEnabled As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents panelButtons As System.Windows.Forms.Panel
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnEdit As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnToggle As System.Windows.Forms.Button
    Friend WithEvents btnReapply As System.Windows.Forms.Button
    Friend WithEvents lblCount As System.Windows.Forms.Label
    Friend WithEvents btnClose As System.Windows.Forms.Button

End Class

End Namespace
