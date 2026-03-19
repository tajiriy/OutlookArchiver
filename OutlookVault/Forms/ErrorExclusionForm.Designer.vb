Option Explicit On
Option Strict On
Option Infer Off

Namespace Forms

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class ErrorExclusionForm
        Inherits System.Windows.Forms.Form

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing Then
                    If _bindingSource IsNot Nothing Then _bindingSource.Dispose()
                    If _dataTable IsNot Nothing Then _dataTable.Dispose()
                    If components IsNot Nothing Then components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        Private components As System.ComponentModel.IContainer

        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Me.dgv = New System.Windows.Forms.DataGridView()
            Me.pnlBottom = New System.Windows.Forms.Panel()
            Me.btnRemoveSelected = New System.Windows.Forms.Button()
            Me.btnClearAll = New System.Windows.Forms.Button()
            Me.lblCount = New System.Windows.Forms.Label()
            CType(Me.dgv, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlBottom.SuspendLayout()
            Me.SuspendLayout()
            '
            ' pnlBottom
            '
            Me.pnlBottom.Controls.Add(Me.btnRemoveSelected)
            Me.pnlBottom.Controls.Add(Me.btnClearAll)
            Me.pnlBottom.Controls.Add(Me.lblCount)
            Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlBottom.Height = 44
            Me.pnlBottom.Name = "pnlBottom"
            Me.pnlBottom.Padding = New System.Windows.Forms.Padding(6, 6, 6, 6)
            '
            ' btnRemoveSelected
            '
            Me.btnRemoveSelected.AutoSize = True
            Me.btnRemoveSelected.Location = New System.Drawing.Point(8, 10)
            Me.btnRemoveSelected.Name = "btnRemoveSelected"
            Me.btnRemoveSelected.Text = "選択した項目の除外を解除(&R)"
            '
            ' btnClearAll
            '
            Me.btnClearAll.AutoSize = True
            Me.btnClearAll.Location = New System.Drawing.Point(220, 10)
            Me.btnClearAll.Name = "btnClearAll"
            Me.btnClearAll.Text = "すべての除外を解除(&A)"
            '
            ' lblCount
            '
            Me.lblCount.Anchor = CType(System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right, System.Windows.Forms.AnchorStyles)
            Me.lblCount.AutoSize = True
            Me.lblCount.Location = New System.Drawing.Point(790, 14)
            Me.lblCount.Name = "lblCount"
            Me.lblCount.Text = ""
            '
            ' dgv
            '
            Me.dgv.AllowUserToAddRows = False
            Me.dgv.AllowUserToDeleteRows = False
            Me.dgv.AllowUserToOrderColumns = True
            Me.dgv.AllowUserToResizeColumns = True
            Me.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None
            Me.dgv.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgv.MultiSelect = True
            Me.dgv.Name = "dgv"
            Me.dgv.ReadOnly = True
            Me.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            '
            ' ErrorExclusionForm
            '
            Me.Controls.Add(Me.dgv)
            Me.Controls.Add(Me.pnlBottom)
            Me.MinimumSize = New System.Drawing.Size(600, 300)
            Me.Name = "ErrorExclusionForm"
            Me.Size = New System.Drawing.Size(900, 500)
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "エラー除外リスト"
            CType(Me.dgv, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlBottom.ResumeLayout(False)
            Me.pnlBottom.PerformLayout()
            Me.ResumeLayout(False)
        End Sub

        Friend WithEvents dgv As System.Windows.Forms.DataGridView
        Friend pnlBottom As System.Windows.Forms.Panel
        Friend WithEvents btnRemoveSelected As System.Windows.Forms.Button
        Friend WithEvents btnClearAll As System.Windows.Forms.Button
        Friend lblCount As System.Windows.Forms.Label

    End Class

End Namespace
