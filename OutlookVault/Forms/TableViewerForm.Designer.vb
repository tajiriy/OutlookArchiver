Option Explicit On
Option Strict On
Option Infer Off

Namespace Forms

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class TableViewerForm
        Inherits System.Windows.Forms.Form

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing Then
                    If _filterTimer IsNot Nothing Then _filterTimer.Dispose()
                    If _bindingSource IsNot Nothing Then _bindingSource.Dispose()
                    If _dataTable IsNot Nothing Then _dataTable.Dispose()
                    If cmsGrid IsNot Nothing Then cmsGrid.Dispose()
                    If components IsNot Nothing Then components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        Private components As System.ComponentModel.IContainer

        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Me.pnlTop = New System.Windows.Forms.Panel()
            Me.lblTable = New System.Windows.Forms.Label()
            Me.cboTable = New System.Windows.Forms.ComboBox()
            Me.lblFilter = New System.Windows.Forms.Label()
            Me.txtFilter = New System.Windows.Forms.TextBox()
            Me.lblRowCount = New System.Windows.Forms.Label()
            Me._filterTimer = New System.Windows.Forms.Timer()
            Me.dgv = New OutlookVault.Forms.TableViewerForm.BufferedDataGridView()
            Me.cmsGrid = New System.Windows.Forms.ContextMenuStrip()
            Me.tsmiCopy = New System.Windows.Forms.ToolStripMenuItem()
            Me.pnlTop.SuspendLayout()
            CType(Me.dgv, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            ' pnlTop
            '
            Me.pnlTop.Controls.Add(Me.lblTable)
            Me.pnlTop.Controls.Add(Me.cboTable)
            Me.pnlTop.Controls.Add(Me.lblFilter)
            Me.pnlTop.Controls.Add(Me.txtFilter)
            Me.pnlTop.Controls.Add(Me.lblRowCount)
            Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlTop.Height = 36
            Me.pnlTop.Name = "pnlTop"
            Me.pnlTop.Padding = New System.Windows.Forms.Padding(6, 6, 6, 4)
            '
            ' lblTable
            '
            Me.lblTable.AutoSize = True
            Me.lblTable.Location = New System.Drawing.Point(8, 10)
            Me.lblTable.Name = "lblTable"
            Me.lblTable.Text = "テーブル:"
            '
            ' cboTable
            '
            Me.cboTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboTable.Location = New System.Drawing.Point(66, 7)
            Me.cboTable.Name = "cboTable"
            Me.cboTable.Width = 180
            '
            ' lblFilter
            '
            Me.lblFilter.AutoSize = True
            Me.lblFilter.Location = New System.Drawing.Point(260, 10)
            Me.lblFilter.Name = "lblFilter"
            Me.lblFilter.Text = "フィルタ:"
            '
            ' txtFilter
            '
            Me.txtFilter.Location = New System.Drawing.Point(312, 7)
            Me.txtFilter.Name = "txtFilter"
            Me.txtFilter.Width = 300
            '
            ' lblRowCount
            '
            Me.lblRowCount.Anchor = CType(System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right, System.Windows.Forms.AnchorStyles)
            Me.lblRowCount.AutoSize = True
            Me.lblRowCount.Name = "lblRowCount"
            Me.lblRowCount.Text = ""
            '
            ' _filterTimer
            '
            Me._filterTimer.Interval = 800
            '
            ' cmsGrid
            '
            Me.cmsGrid.Items.Add(Me.tsmiCopy)
            Me.cmsGrid.Name = "cmsGrid"
            '
            ' tsmiCopy
            '
            Me.tsmiCopy.Name = "tsmiCopy"
            Me.tsmiCopy.Text = "コピー(&C)"
            Me.tsmiCopy.ShortcutKeys = CType(System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C, System.Windows.Forms.Keys)
            Me.tsmiCopy.ShowShortcutKeys = True
            '
            ' dgv
            '
            Me.dgv.AllowUserToAddRows = False
            Me.dgv.AllowUserToDeleteRows = False
            Me.dgv.AllowUserToOrderColumns = True
            Me.dgv.AllowUserToResizeColumns = True
            Me.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None
            Me.dgv.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable
            Me.dgv.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgv.Name = "dgv"
            Me.dgv.ReadOnly = True
            Me.dgv.ContextMenuStrip = Me.cmsGrid
            Me.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
            '
            ' TableViewerForm
            '
            Me.Controls.Add(Me.dgv)
            Me.Controls.Add(Me.pnlTop)
            Me.MinimumSize = New System.Drawing.Size(600, 400)
            Me.Name = "TableViewerForm"
            Me.Size = New System.Drawing.Size(1000, 600)
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "テーブルビューア"
            Me.pnlTop.ResumeLayout(False)
            Me.pnlTop.PerformLayout()
            CType(Me.dgv, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
        End Sub

        Friend lblTable As System.Windows.Forms.Label
        Friend WithEvents cboTable As System.Windows.Forms.ComboBox
        Friend lblFilter As System.Windows.Forms.Label
        Friend txtFilter As System.Windows.Forms.TextBox
        Friend lblRowCount As System.Windows.Forms.Label
        Friend pnlTop As System.Windows.Forms.Panel
        Friend WithEvents dgv As System.Windows.Forms.DataGridView
        Friend _filterTimer As System.Windows.Forms.Timer
        Friend cmsGrid As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents tsmiCopy As System.Windows.Forms.ToolStripMenuItem

    End Class

End Namespace
