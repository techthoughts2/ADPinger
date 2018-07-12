Module modScan
    Function checkFilePath(ByVal strFileScanPath As String, ByVal fileScanGood As Boolean)
        'NOTE: This functions used for both Scan File and Delete File options

        'this function will check the validity of the users
        'input for the file path.

        'check to see if the basic structure of a file path is good
        If strFileScanPath Like "*:\*.*" Then
            'if it is we will begin manipulating the string for remote PC use
            If Char.IsLetter(strFileScanPath(0)) Then
                fileScanGood = True
                Return fileScanGood
            Else
                'the first character is not a drive letter. idiot user.
                MsgBox("It does not appear that you entered a valid file path" & vbNewLine & "ex. C:\FilePath\Filename.txt", MsgBoxStyle.Exclamation)
                fileScanGood = False
                Return fileScanGood
                Exit Function
            End If
        Else
            'the file path structure is not correct
            MsgBox("It does not appear that you entered a valid file path" & vbNewLine & "ex. C:\FilePath\Filename.txt", MsgBoxStyle.Exclamation)
            fileScanGood = False
            Return fileScanGood
            Exit Function
        End If
    End Function

    Function formatFilePath(ByVal strFileScanPath As String)
        'if it checks out, we will build the string needed
        'to check the file path on remote computers
        Dim strDriveLetter As String = strFileScanPath(0)
        strDriveLetter = strDriveLetter + "$"
        Dim strAlteredFilePath As String = strFileScanPath.Remove(0, 2)
        strFileScanPath = strDriveLetter + strAlteredFilePath
        'reset
        strDriveLetter = Nothing
        strAlteredFilePath = Nothing
        Return strFileScanPath
    End Function
End Module
