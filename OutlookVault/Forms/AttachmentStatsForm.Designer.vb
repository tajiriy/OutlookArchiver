Option Explicit On
Option Strict On
Option Infer Off

Namespace Forms

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class AttachmentStatsForm
        Inherits System.Windows.Forms.Form

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing Then
                    If _dataTable IsNot Nothing Then _dataTable.Dispose()
                    If _bindingSource IsNot Nothing Then _bindingSource.Dispose()
                    If components IsNot Nothing Then components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        Private components As System.ComponentModel.IContainer

        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Me.pnlBottom = New System.Windows.Forms.Panel()
            Me.lblSummary = New System.Windows.Forms.Label()
            Me.btnClose = New System.Windows.Forms.Button()
            Me.dgv = New System.Windows.Forms.DataGridView()
            Me.colExtension = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colCount = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colTotalSize = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colPercent = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.pnlBottom.SuspendLayout()
            CType(Me.dgv, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            ' pnlBottom
            '
            Me.pnlBottom.Controls.Add(Me.lblSummary)
            Me.pnlBottom.Controls.Add(Me.btnClose)
            Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlBottom.Height = 40
            Me.pnlBottom.Name = "pnlBottom"
            Me.pnlBottom.Padding = New System.Windows.Forms.Padding(8, 6, 8, 6)
            '
            ' lblSummary
            '
            Me.lblSummary.AutoSize = True
            Me.lblSummary.Dock = System.Windows.Forms.DockStyle.Left
            Me.lblSummary.Name = "lblSummary"
            Me.lblSummary.Text = ""
            Me.lblSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            ' btnClose
            '
            Me.btnClose.Dock = System.Windows.Forms.DockStyle.Right
            Me.btnClose.Name = "btnClose"
            Me.btnClose.Size = New System.Drawing.Size(80, 28)
            Me.btnClose.Text = "閉じる"
            Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
            '
            ' dgv
            '
            Me.dgv.AllowUserToAddRows = False
            Me.dgv.AllowUserToDeleteRows = False
            Me.dgv.AllowUserToResizeColumns = True
            Me.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgv.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithAutoHeaderText
            Me.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgv.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colExtension, Me.colCount, Me.colTotalSize, Me.colPercent})
            Me.dgv.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgv.Name = "dgv"
            Me.dgv.ReadOnly = True
            Me.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            '
            ' colExtension
            '
            Me.colExtension.HeaderText = "拡張子"
            Me.colExtension.Name = "colExtension"
            Me.colExtension.FillWeight = 30
            '
            ' colCount
            '
            Me.colCount.HeaderText = "件数"
            Me.colCount.Name = "colCount"
            Me.colCount.FillWeight = 20
            Me.colCount.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
            '
            ' colTotalSize
            '
            Me.colTotalSize.HeaderText = "合計サイズ"
            Me.colTotalSize.Name = "colTotalSize"
            Me.colTotalSize.FillWeight = 25
            Me.colTotalSize.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
            '
            ' colPercent
            '
            Me.colPercent.HeaderText = "割合 (%)"
            Me.colPercent.Name = "colPercent"
            Me.colPercent.FillWeight = 25
            Me.colPercent.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
            '
            ' AttachmentStatsForm
            '
            Me.AcceptButton = Me.btnClose
            Me.CancelButton = Me.btnClose
            Me.Controls.Add(Me.dgv)
            Me.Controls.Add(Me.pnlBottom)
            Me.MinimumSize = New System.Drawing.Size(400, 300)
            Me.Name = "AttachmentStatsForm"
            Me.Size = New System.Drawing.Size(550, 450)
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "添付ファイル拡張子統計"
            Me.pnlBottom.ResumeLayout(False)
            Me.pnlBottom.PerformLayout()
            CType(Me.dgv, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
        End Sub

        Friend pnlBottom As System.Windows.Forms.Panel
        Friend lblSummary As System.Windows.Forms.Label
        Friend WithEvents btnClose As System.Windows.Forms.Button
        Friend WithEvents dgv As System.Windows.Forms.DataGridView
        Friend colExtension As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend colCount As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend colTotalSize As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend colPercent As System.Windows.Forms.DataGridViewTextBoxColumn

    End Class

End Namespace
