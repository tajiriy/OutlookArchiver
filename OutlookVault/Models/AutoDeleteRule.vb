Option Explicit On
Option Strict On
Option Infer Off

Namespace Models

    ''' <summary>自動削除ルールのエンティティ。</summary>
    Public Class AutoDeleteRule

        Public Property Id As Integer
        Public Property Name As String
        Public Property FilterExpression As String
        Public Property Enabled As Boolean
        Public Property SortOrder As Integer
        Public Property CreatedAt As DateTime
        Public Property UpdatedAt As DateTime

    End Class

End Namespace
