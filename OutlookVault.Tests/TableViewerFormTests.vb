Option Explicit On
Option Strict On
Option Infer Off

Imports NUnit.Framework
Imports OutlookVault.Forms
Imports System.Windows.Forms

Namespace Tests

    <TestFixture>
    Public Class TableViewerFormTests

        Private _dgv As DataGridView

        <SetUp>
        Public Sub SetUp()
            _dgv = New DataGridView()
            _dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable
            _dgv.SelectionMode = DataGridViewSelectionMode.CellSelect
            _dgv.Columns.Add("ColA", "A")
            _dgv.Columns.Add("ColB", "B")
            _dgv.Columns.Add("ColC", "C")
            _dgv.Rows.Add("R0A", "R0B", "R0C")
            _dgv.Rows.Add("R1A", "R1B", "R1C")
            _dgv.Rows.Add("R2A", "R2B", "R2C")
            _dgv.ClearSelection()
        End Sub

        <TearDown>
        Public Sub TearDown()
            If _dgv IsNot Nothing Then _dgv.Dispose()
        End Sub

        <Test>
        Public Sub BuildCopyText_NoSelection_ReturnsEmpty()
            Dim result As String = TableViewerForm.BuildCopyText(_dgv.SelectedCells)
            Assert.AreEqual("", result)
        End Sub

        <Test>
        Public Sub BuildCopyText_SingleCell_ReturnsCellValue()
            _dgv.Rows(0).Cells(1).Selected = True
            Dim result As String = TableViewerForm.BuildCopyText(_dgv.SelectedCells)
            Assert.AreEqual("R0B", result)
        End Sub

        <Test>
        Public Sub BuildCopyText_SingleRowMultipleCells_ReturnsTabSeparated()
            ' 同一行の複数セル → タブ区切り
            _dgv.Rows(1).Cells(0).Selected = True
            _dgv.Rows(1).Cells(1).Selected = True
            _dgv.Rows(1).Cells(2).Selected = True
            Dim result As String = TableViewerForm.BuildCopyText(_dgv.SelectedCells)
            Assert.AreEqual("R1A" & vbTab & "R1B" & vbTab & "R1C", result)
        End Sub

        <Test>
        Public Sub BuildCopyText_RectangularSelection_ReturnsTabAndNewline()
            ' 矩形選択 (2行×2列)
            _dgv.Rows(0).Cells(0).Selected = True
            _dgv.Rows(0).Cells(1).Selected = True
            _dgv.Rows(1).Cells(0).Selected = True
            _dgv.Rows(1).Cells(1).Selected = True
            Dim result As String = TableViewerForm.BuildCopyText(_dgv.SelectedCells)
            Dim expected As String = "R0A" & vbTab & "R0B" & vbCrLf & "R1A" & vbTab & "R1B"
            Assert.AreEqual(expected, result)
        End Sub

        <Test>
        Public Sub BuildCopyText_SingleColumnMultipleRows_ReturnsNewlineSeparated()
            ' 同一列の複数行 → 矩形(n行×1列) → 改行区切り
            _dgv.Rows(0).Cells(2).Selected = True
            _dgv.Rows(1).Cells(2).Selected = True
            _dgv.Rows(2).Cells(2).Selected = True
            Dim result As String = TableViewerForm.BuildCopyText(_dgv.SelectedCells)
            Dim expected As String = "R0C" & vbCrLf & "R1C" & vbCrLf & "R2C"
            Assert.AreEqual(expected, result)
        End Sub

        <Test>
        Public Sub BuildCopyText_NonContiguousCells_ReturnsTabSeparatedFlat()
            ' とびとび選択 (行0列0, 行2列2) → タブ区切りフラット
            _dgv.Rows(0).Cells(0).Selected = True
            _dgv.Rows(2).Cells(2).Selected = True
            Dim result As String = TableViewerForm.BuildCopyText(_dgv.SelectedCells)
            ' 2行×2列=4 ≠ 2セル → とびとび
            Assert.AreEqual("R0A" & vbTab & "R2C", result)
        End Sub

        <Test>
        Public Sub BuildCopyText_NullCellValue_ReturnsEmptyString()
            _dgv.Rows(0).Cells(0).Value = Nothing
            _dgv.Rows(0).Cells(0).Selected = True
            Dim result As String = TableViewerForm.BuildCopyText(_dgv.SelectedCells)
            Assert.AreEqual("", result)
        End Sub

        <Test>
        Public Sub BuildCopyText_ThreeScatteredCells_ReturnsTabSeparatedFlat()
            ' 3セルとびとび: (0,0), (1,1), (2,2) → 3行×3列=9 ≠ 3 → フラット
            _dgv.Rows(0).Cells(0).Selected = True
            _dgv.Rows(1).Cells(1).Selected = True
            _dgv.Rows(2).Cells(2).Selected = True
            Dim result As String = TableViewerForm.BuildCopyText(_dgv.SelectedCells)
            Assert.AreEqual("R0A" & vbTab & "R1B" & vbTab & "R2C", result)
        End Sub

    End Class

End Namespace
