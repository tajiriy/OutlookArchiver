Option Explicit On
Option Strict On
Option Infer Off

Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Windows.Forms

Namespace Icons

    ''' <summary>shell32.dll / SHGetFileInfo からアイコンを取得するヘルパー。</summary>
    Friend NotInheritable Class ShellIconHelper

        Private Sub New()
        End Sub

        ' ────────────────────────────────────────────────────────
        '  Win32 API 宣言
        ' ────────────────────────────────────────────────────────

        <DllImport("shell32.dll", CharSet:=CharSet.Unicode)>
        Private Shared Function ExtractIconEx(
            lpszFile As String,
            nIconIndex As Integer,
            <Out()> phiconLarge() As IntPtr,
            <Out()> phiconSmall() As IntPtr,
            nIcons As UInteger) As UInteger
        End Function

        <DllImport("shell32.dll", CharSet:=CharSet.Unicode)>
        Private Shared Function SHGetFileInfo(
            pszPath As String,
            dwFileAttributes As UInteger,
            ByRef psfi As SHFILEINFO,
            cbFileInfo As UInteger,
            uFlags As UInteger) As IntPtr
        End Function

        <DllImport("user32.dll", SetLastError:=True)>
        Private Shared Function DestroyIcon(hIcon As IntPtr) As Boolean
        End Function

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Private Structure SHFILEINFO
            Public hIcon As IntPtr
            Public iIcon As Integer
            Public dwAttributes As UInteger
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=260)>
            Public szDisplayName As String
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=80)>
            Public szTypeName As String
        End Structure

        ' SHGetFileInfo フラグ
        Private Const SHGFI_ICON As UInteger = &H100UI
        Private Const SHGFI_SMALLICON As UInteger = &H1UI
        Private Const SHGFI_LARGEICON As UInteger = &H0UI
        Private Const SHGFI_USEFILEATTRIBUTES As UInteger = &H10UI
        Private Const FILE_ATTRIBUTE_NORMAL As UInteger = &H80UI

        ' shell32.dll アイコンインデックス
        Private Const ShellIconFolder As Integer = 3          ' 閉じフォルダ
        Private Const ShellIconFolderOpen As Integer = 4      ' 開きフォルダ
        Private Const ShellIconTrash As Integer = 31          ' ゴミ箱（空）
        Private Const ShellIconAllFolders As Integer = 172    ' スタック／すべて

        ''' <summary>ImageList 内のインデックス定数（フォルダツリー用）。</summary>
        Friend Const IndexAll As Integer = 0
        Friend Const IndexFolder As Integer = 1
        Friend Const IndexFolderOpen As Integer = 2
        Friend Const IndexTrash As Integer = 3

        ' 拡張子アイコンのキャッシュ（キー: 小文字拡張子, 値: アイコン画像）
        Private Shared ReadOnly _smallIconCache As New Dictionary(Of String, Bitmap)(StringComparer.OrdinalIgnoreCase)
        Private Shared ReadOnly _largeIconCache As New Dictionary(Of String, Bitmap)(StringComparer.OrdinalIgnoreCase)

        ' ────────────────────────────────────────────────────────
        '  shell32.dll ExtractIconEx（フォルダツリー用）
        ' ────────────────────────────────────────────────────────

        ''' <summary>shell32.dll から小さいアイコン (16x16) を取得する。</summary>
        Private Shared Function GetShellIcon(index As Integer) As Icon
            Dim large(0) As IntPtr
            Dim small(0) As IntPtr
            ExtractIconEx("shell32.dll", index, large, small, 1UI)
            Try
                If small(0) <> IntPtr.Zero Then
                    Return CType(Icon.FromHandle(small(0)).Clone(), Icon)
                End If
            Finally
                If large(0) <> IntPtr.Zero Then DestroyIcon(large(0))
                If small(0) <> IntPtr.Zero Then DestroyIcon(small(0))
            End Try
            Return Nothing
        End Function

        ' ────────────────────────────────────────────────────────
        '  SHGetFileInfo（拡張子アイコン取得）
        ' ────────────────────────────────────────────────────────

        ''' <summary>拡張子に対応する小さいアイコン (16x16) を Bitmap で返す。</summary>
        Friend Shared Function GetExtensionIconSmall(extension As String) As Bitmap
            Dim ext As String = NormalizeExtension(extension)
            Dim cached As Bitmap = Nothing
            If _smallIconCache.TryGetValue(ext, cached) Then Return cached
            Dim bmp As Bitmap = ExtractExtensionIcon(ext, smallIcon:=True)
            _smallIconCache(ext) = bmp
            Return bmp
        End Function

        ''' <summary>拡張子に対応する大きいアイコン (32x32) を Bitmap で返す。</summary>
        Friend Shared Function GetExtensionIconLarge(extension As String) As Bitmap
            Dim ext As String = NormalizeExtension(extension)
            Dim cached As Bitmap = Nothing
            If _largeIconCache.TryGetValue(ext, cached) Then Return cached
            Dim bmp As Bitmap = ExtractExtensionIcon(ext, smallIcon:=False)
            _largeIconCache(ext) = bmp
            Return bmp
        End Function

        ''' <summary>拡張子を正規化する（先頭にドットを付け、小文字化）。</summary>
        Private Shared Function NormalizeExtension(ext As String) As String
            If String.IsNullOrEmpty(ext) Then Return ""
            ext = ext.Trim().ToLower()
            If Not ext.StartsWith(".", StringComparison.Ordinal) Then ext = "." & ext
            Return ext
        End Function

        ''' <summary>SHGetFileInfo で拡張子アイコンを取得する。</summary>
        Private Shared Function ExtractExtensionIcon(ext As String, smallIcon As Boolean) As Bitmap
            Dim fileName As String = If(String.IsNullOrEmpty(ext), "file", "file" & ext)
            Dim flags As UInteger = SHGFI_ICON Or SHGFI_USEFILEATTRIBUTES
            If smallIcon Then
                flags = flags Or SHGFI_SMALLICON
            Else
                flags = flags Or SHGFI_LARGEICON
            End If

            Dim shfi As New SHFILEINFO()
            Dim result As IntPtr = SHGetFileInfo(fileName, FILE_ATTRIBUTE_NORMAL, shfi,
                                                  CUInt(Marshal.SizeOf(shfi)), flags)
            If result <> IntPtr.Zero AndAlso shfi.hIcon <> IntPtr.Zero Then
                Try
                    Dim ico As Icon = CType(Icon.FromHandle(shfi.hIcon).Clone(), Icon)
                    Return ico.ToBitmap()
                Finally
                    DestroyIcon(shfi.hIcon)
                End Try
            End If

            ' フォールバック: 空の Bitmap を返す
            Dim size As Integer = If(smallIcon, 16, 32)
            Return New Bitmap(size, size)
        End Function

        ''' <summary>フォルダツリー用の ImageList を生成する。</summary>
        Friend Shared Function CreateFolderImageList() As ImageList
            Dim imgList As New ImageList()
            imgList.ColorDepth = ColorDepth.Depth32Bit
            imgList.ImageSize = New Size(16, 16)
            imgList.TransparentColor = Color.Transparent

            ' IndexAll = 0
            Dim iconAll As Icon = GetShellIcon(ShellIconAllFolders)
            If iconAll IsNot Nothing Then
                imgList.Images.Add(iconAll)
            Else
                ' フォールバック: 通常フォルダアイコン
                Dim iconFallback As Icon = GetShellIcon(ShellIconFolder)
                If iconFallback IsNot Nothing Then imgList.Images.Add(iconFallback)
            End If

            ' IndexFolder = 1
            Dim iconFolder As Icon = GetShellIcon(ShellIconFolder)
            If iconFolder IsNot Nothing Then imgList.Images.Add(iconFolder)

            ' IndexFolderOpen = 2
            Dim iconFolderOpen As Icon = GetShellIcon(ShellIconFolderOpen)
            If iconFolderOpen IsNot Nothing Then imgList.Images.Add(iconFolderOpen)

            ' IndexTrash = 3
            Dim iconTrash As Icon = GetShellIcon(ShellIconTrash)
            If iconTrash IsNot Nothing Then imgList.Images.Add(iconTrash)

            Return imgList
        End Function

    End Class

End Namespace
