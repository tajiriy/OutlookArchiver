Option Explicit On
Option Strict On
Option Infer Off

Imports Microsoft.VisualBasic.ApplicationServices

Namespace My

    Partial Friend Class MyApplication

        Private Sub MyApplication_StartupNextInstance(
                sender As Object,
                e As StartupNextInstanceEventArgs) Handles Me.StartupNextInstance

            Dim mainForm As Form = Me.MainForm
            If mainForm IsNot Nothing Then
                If mainForm.WindowState = FormWindowState.Minimized Then
                    mainForm.WindowState = FormWindowState.Normal
                End If
                mainForm.Activate()
            End If
        End Sub

    End Class

End Namespace
