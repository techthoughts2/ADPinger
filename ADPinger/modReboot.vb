Module modReboot
    Dim strRebootMessage As String = "" 'string for storing reboot mesage
    Function rebootCommand(ByVal strRebootCommand As String)
        'this function only performs basic empty checks
        'its purpose is to build up two strings for the reboot or shutdown command
        'the first string will contain the time and force
        '(-t 15 -f)
        'the second string will be the actual message for restart/shutdown
        Dim strTime As String
        Dim strForce As String
        'if the user doesn't select a time, we make it default 15
        If Form1.txtRebootTime.Text = "" Then
            strTime = "-t 15"
        Else
            strTime = "-t " & Form1.txtRebootTime.Text
        End If
        If Form1.chkForceReboot.Checked = True Then
            strForce = "-f"
        Else
            strForce = ""
        End If

        'build the final string
        'the string will vary depending on whether or not the user wants to
        'reboot or shutdown. note that we are using this function for both tasks
        If Form1.txtShutdownMessage.Text <> "" Then
            'if the user wants a shutdown/reboot message we will include it here in this string creation
            strRebootMessage = Form1.txtShutdownMessage.Text
            If Form1.cboAction.SelectedItem = "Reboot" Then
                strRebootCommand = "-r " & strTime & " " & strForce & " -c " & """" & strRebootMessage & """" & " -m \\"
            ElseIf Form1.cboAction.SelectedItem = "Shutdown" Then
                strRebootCommand = "-s " & strTime & " " & strForce & " -c " & """" & strRebootMessage & """" & " -m \\"
            End If

        Else
            'otherwise we create a different reboot/shutdown string
            If Form1.cboAction.SelectedItem = "Reboot" Then
                strRebootCommand = "-r " & strTime & " " & strForce & " -m \\"
            ElseIf Form1.cboAction.SelectedItem = "Shutdown" Then
                strRebootCommand = "-s " & strTime & " " & strForce & " -m \\"
            End If

        End If
        strRebootMessage = ""
        Return strRebootCommand
    End Function
End Module
