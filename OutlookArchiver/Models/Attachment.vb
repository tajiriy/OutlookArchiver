Namespace Models

    Public Class Attachment

        Public Property Id As Integer
        Public Property EmailId As Integer
        Public Property FileName As String
        ''' <summary>添付ファイルの保存先パス（attachments/ 配下の相対パス）</summary>
        Public Property FilePath As String
        Public Property FileSize As Long
        Public Property MimeType As String
        Public Property CreatedAt As DateTime

    End Class

End Namespace
