Imports System.ComponentModel
Imports System.Net.NetworkInformation
Imports System.Globalization
Imports System.IO
Imports System.Text.RegularExpressions

Public Class Form1
    Public PCNames(5000) As String 'array for storing PC names from OU
    Dim oThread(1) As System.Threading.Thread 'delay thread needed for stability
    Public intRange As Integer 'global needed for interacting with the array
    Public arraySize As Double 'needed for determining last entry in array
    Public OUSelection As String 'used for user OU selection
    Public today As Date = Date.Now.ToShortDateString 'used for email and filename
    Public fmt As DateTimeFormatInfo = (New CultureInfo("hr-HR")).DateTimeFormat 'used for email and filename
    Public fileDate As String = (today.ToString("d", fmt)) 'used for email and filename
    Public attachment As String = "" 'needed for attachment
    Public letsStart As Integer = 0 'used for do loop holding for thread completion
    Public allDone As Integer = 0 'used for do loop holding for thread completion
    Public cancelCheck As Boolean = False 'used to ensure we can write to the C:\drive


    Private Sub scanOU_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles scanOU.Click
        scanOU.Enabled = False 'while the program is running the buttons has to be taken away
        Dim validated As Boolean = False 'needed for ensuring user makes a selection
        'initial resets on all labels, counters, boxes, etc.
        Status.Visible = False
        lblThreads.Text = "0"
        lblActiveThreads.Text = "0"
        lstNonPingable.Items.Clear()
        lstPingable.Items.Clear()
        Status.Text = "Scanning..."
        intRange = 0
        letsStart = 0
        allDone = 0
        OUSelection = ""
        arraySize = 0
        cancelCheck = False
        'end reset values

        'clear out the array to start fresh for each run should user do multiple runs
        For i = LBound(PCNames) To UBound(PCNames)
            PCNames(i) = ""
        Next

        'Check to make sure that user made a selection, then assign correct information
        'of respective OU based on user selection
        If OUSelector.Text = "" Then
            MsgBox("You must make a selection")
        ElseIf OUSelector.Text = "Graveyard OU" Then
            OUSelection = "LDAP://CGHS-DC1/OU=Computer Graveyard,DC=cghsnt,DC=mccg,DC=org"
            validated = True
        ElseIf OUSelector.Text = "Computers OU" Then
            OUSelection = "LDAP://CGHS-DC1/CN=Computers,DC=cghsnt,DC=mccg,DC=org"
            validated = True
        ElseIf OUSelector.Text = "Managed Computers Auto Install OU" Then
            OUSelection = "LDAP://CGHS-DC1/OU=Managed Computers Auto Install,DC=cghsnt,DC=mccg,DC=org"
            validated = True
        ElseIf OUSelector.Text = "Non-Managed Computers OU" Then
            OUSelection = "LDAP://CGHS-DC1/OU=Non-Managed Computers,DC=cghsnt,DC=mccg,DC=org"
            validated = True
        End If

        'once a good selection has been made, initiate the background worker
        If validated = True Then
            Status.Visible = True
            ADLoader.RunWorkerAsync()
        Else
            MsgBox("You must make a selection")
        End If
    End Sub

    Private Sub ADLoader_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles ADLoader.DoWork
        'this background worker scans the selected OU, and loads all PC names 
        'into an array

        'variables used for AD connection
        Dim objRootDSE, strDomain, objConnection, objCommand, objRecordSet 'path
        Const ADS_SCOPE_SUBTREE = 2
        'variable used for array interaction
        Dim i As Integer = 0

        'Get domain components
        'this determines what the root LDAP is and looks in the entire AD environment
        objRootDSE = GetObject("LDAP://RootDSE")
        strDomain = objRootDSE.Get("DefaultNamingContext")

        'Set ADO connection
        objConnection = CreateObject("ADODB.Connection")
        objConnection.Provider = "ADsDSOObject"
        objConnection.Open("Active Directory Provider")

        'Set ADO command
        objCommand = CreateObject("ADODB.Command")
        objCommand.ActiveConnection = objConnection
        objCommand.Properties("Searchscope") = ADS_SCOPE_SUBTREE
        objCommand.CommandText = "SELECT name,cn,adsPath FROM '" & OUSelection & " ' WHERE objectCategory='computer' "
        'objCommand.CommandText = "SELECT accountExpires,mail,displayName FROM 'LDAP://" & strDomain & "' WHERE objectCategory='user' "
        'AND samAccountName = '" & strUsername & "'"
        objCommand.Properties("Page Size") = 15000

        'Set recordset to hold the query result
        objRecordSet = objCommand.Execute
        objRecordSet.Movefirst()

        Do Until objRecordSet.EOF = True
            'check to see if the OU entry in null, if it is move to the next record
            If objRecordSet.Fields("cn").Value Is DBNull.Value Then
                objRecordSet.MoveNext()
            Else
                'load each computername into the array
                PCNames(i) = objRecordSet.Fields("cn").Value
                'sometimes for absolutely no reason AD will not correctly populate
                'the CN into the array and loads a null value. if this happens, it can
                'be repaired by checking for null and manually loading the name instead
                'this happens about 1% of the time

                If PCNames(i) = "" Then
                    PCNames(i) = objRecordSet.Fields("name").Value
                End If
                i = i + 1
                objRecordSet.MoveNext()
            End If
        Loop

        'because of the setup of the array Ubound cannot be used to determine last entry
        'as it is statically set to 5000. this counter loop will determine
        'the last entry in the array and store that into arraysize
        arraySize = 0
        For i = LBound(PCNames) To UBound(PCNames)
            If PCNames(i) <> "" Then
                arraySize = arraySize + 1
            Else
                Exit For
            End If
        Next

        'close connections to AD
        objRecordSet.Close()
        objConnection.Close()
    End Sub

    Private Sub getExpirations_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ADLoader.RunWorkerCompleted
        'begin pinging computers
        cmdPingComputers()
    End Sub

    Private Sub cmdPingComputers()
        'we are going to begin firing off unmanaged threads. this is necessary
        'to ping all computers simultaneously. running this in serial would result
        'in a very long drawn out process.  in this code each computers pinging action
        'is handled by an independent thread.  so, as fast as we can fire threads,
        'thats how fast PC's start getting pinged.  unfortunately, this happens too fast
        'with the current code and causes instability.  it becomes necessary to slightly
        'delay each thread so that the program remains stable.

        'necessary to ensure that we don't fire more threads than pc's in the array
        'please note that intRange is currently incremented inside each independent thread
        'this is critical.  incrementing inside this function causes instablity
        'and faulty data
        intRange = 0
        Status.Text = "Processing..."
        'begin thread creation, NO MORE THAN 1000
        ReDim oThread(1000)
        For Q As Integer = 0 To 1000 - 1 Step 1
            'delay next thread creation. without this, the program does some 
            'weird things
            System.Threading.Thread.Sleep(40)
            'once intRange is the same as arraySize, we stop thread creation
            'everything is now pinged
            If intRange >= arraySize Then
                Exit For
            End If

            'actual thread creations
            oThread(Q) = New System.Threading.Thread(AddressOf PingAllPCs)
            oThread(Q).Name = "Thread" & Q
            oThread(Q).Start()
            letsStart = letsStart + 1
            'refresh thread counter label
            lblThreads.Text = lblThreads.Text + 1
            lblActiveThreads.Text = lblActiveThreads.Text + 1
            lblActiveThreads.Refresh()
            lblThreads.Refresh()

        Next

        'hold program in a do loop until all ping threads report complete
        'this must be done in a background worker otherwise it locks the program
        completionChecker.RunWorkerAsync()

    End Sub

    Private Sub completionChecker_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles completionChecker.DoWork
        'here we are going to hold a thread in a loop until all ping threads
        'report that they are done.
        'lets check to see if they are all done!
        Do While allDone < letsStart
            'MsgBox("inside do loop")
            If letsStart = allDone Then
                Exit Do
            Else
                System.Threading.Thread.Sleep(4000)
                Me.Refresh()
            End If
        Loop
    End Sub

    Private Sub completionChecker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles completionChecker.RunWorkerCompleted
        'start delay backround worker
        'necessary to ensure we don't process the report creation or send the
        'email until all threads are complete
        'note that this will not be called until all threads have been fired for
        'all PC's inside the OU
        lblActiveThreads.Text = "0"
        Me.Refresh()
        delay.RunWorkerAsync()
    End Sub

    Private Sub PingAllPCs()
        'the basics of this is, this sub will be assigned 10 pc's which it will be
        'responsible for pinging.  the counters ensure that the sub (with its one thread)
        'will only ping its respective 10 PC's.
        'just remember, each instance of PingAllPCs is running on its own unique thread
        'with 10 PC's per thread

        Dim strBS As String = ""
        'some sloppy coding here, but...hey, it works.
        Control.CheckForIllegalCrossThreadCalls = False
        Dim _ping As New Ping

        'counter variables to ensure this thread only pings the right 10 PC's
        Dim i As Integer
        Dim j As Integer

        'again note that intRange is incremented HERE, not in the orignial sub
        'the below code is kind of a mind fuck as far as counter incrementing.
        'basically just trust that this is correct (it is, its been tested)
        'this sets up the for loop to correctly ping the correct 10 PC's in the array.
        If intRange = 0 Then
            i = intRange
            intRange = intRange + 10
            j = i + 10
        Else
            i = intRange + 1
            intRange = intRange + 10
            j = i + 9
        End If

        Dim testLocal As String = ""

        'begin ping process, and ascertain ping status.
        'names and status are then dumped into a listbox for later use
        'actual ping code developed by Justin Saylor, adapted for this use
        For x = i To j
            If x < arraySize Then
                Try
                    Dim _pingreply = _ping.Send(PCNames(x), 1000) '2000
                    If _pingreply.Status = IPStatus.Success Then
                        SyncLock lstPingable
                            lstPingable.Items.Add(PCNames(x))
                        End SyncLock
                        SyncLock lstNonPingable
                            If InStr(_pingreply.Status, "0") <> Nothing Then
                                strBS = "Success"
                            End If
                            If InStr(_pingreply.Status, "11010") <> Nothing Then
                                strBS = "Timed Out"
                            End If
                            lstNonPingable.Items.Add(PCNames(x) & "," & strBS)
                        End SyncLock
                        lstPingable.SelectedIndex = lstPingable.Items.Count - 1
                    ElseIf _pingreply.Status = IPStatus.TimedOut Then
                        SyncLock lstNonPingable
                            If InStr(_pingreply.Status, "0") <> Nothing Then
                                strBS = "Success"
                            End If
                            If InStr(_pingreply.Status, "11010") <> Nothing Then
                                strBS = "Timed Out"
                            End If
                            lstNonPingable.Items.Add(PCNames(x) & "," & strBS)
                        End SyncLock

                        'ElseIf _pingreply.Status = IPStatus.DestinationHostUnreachable Then
                        '    SyncLock lstNonPingable
                        '        If InStr(_pingreply.Status, "0") <> Nothing Then
                        '            'strBS = "Unknown"
                        '            'lstNonPingable.Items.Add(PCNames(x) & "," & strBS)
                        '            strBS = "Success"
                        '            'strBS = "Dest UNR|Success"
                        '            lstNonPingable.Items.Add(PCNames(x) & "," & strBS)
                        '        ElseIf InStr(_pingreply.Status, "11010") <> Nothing Then
                        '            'strBS = "Unknown"
                        '            'lstNonPingable.Items.Add(PCNames(x) & "," & strBS)
                        '            strBS = "Timed Out"
                        '            lstNonPingable.Items.Add(PCNames(x) & "," & strBS)
                        '        ElseIf InStr(_pingreply.Status, "11003") <> Nothing Then
                        '            strBS = "No such host is known"
                        '            lstNonPingable.Items.Add(PCNames(x) & "," & strBS)
                        '        End If
                        '    End SyncLock
                    Else
                        SyncLock lstNonPingable
                            If InStr(_pingreply.Status, "0") <> Nothing Then
                                strBS = "Success"
                            End If
                            If InStr(_pingreply.Status, "11010") <> Nothing Then
                                strBS = "Timed Out"
                            End If
                            If InStr(_pingreply.Status, "11003") <> Nothing Then
                                strBS = "No such host is known"
                            End If
                            lstNonPingable.Items.Add(PCNames(x) & "," & strBS)
                        End SyncLock
                    End If
                Catch ex As System.Net.NetworkInformation.PingException
                    strBS = ex.InnerException.ToString
                    strBS = strBS.Remove(0, 48)
                    strBS = strBS.Remove(22, Int(strBS.Length - 22))
                    strBS = Trim(strBS)
                    SyncLock lstNonPingable
                        lstNonPingable.Items.Add(PCNames(x) & "," & strBS)
                    End SyncLock
                Catch ex As System.Net.Sockets.SocketException
                    strBS = ex.InnerException.ToString
                    strBS = strBS.Remove(0, 48)
                    strBS = strBS.Remove(22, Int(strBS.Length - 22))
                    strBS = Trim(strBS)
                    SyncLock lstNonPingable
                        lstNonPingable.Items.Add(PCNames(x) & "," & strBS)
                    End SyncLock
                Catch ex As System.Exception
                    strBS = ex.InnerException.ToString
                    strBS = strBS.Remove(0, 48)
                    strBS = strBS.Remove(22, Int(strBS.Length - 22))
                    strBS = Trim(strBS)
                    SyncLock lstNonPingable
                        If Not ex.InnerException Is Nothing Then
                            lstNonPingable.Items.Add(PCNames(x) & "," & strBS)
                        Else
                            lstNonPingable.Items.Add(PCNames(x) & "," & InStr((ex.InnerException.ToString), "): "))
                        End If
                    End SyncLock
                End Try

            Else
                SyncLock lstNonPingable
                End SyncLock

                Me.Refresh()
                allDone = allDone + 1
                Exit Sub
            End If
        Next

        SyncLock lstNonPingable
        End SyncLock

        Me.Refresh()
        allDone = allDone + 1
        lblActiveThreads.Text = lblActiveThreads.Text - 1
    End Sub

    Private Sub delay_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles delay.DoWork
        'update user as to current status of the program.  at this point all names
        'have been loaded, all pc's have been pinged, and listboxes have been populated
        'however, the program because of the # of threads tends to lag a little
        'this will pause everything for about 3 seconds to let everything catch up
        'it will then prompt the use to save the csv report
        Status.Text = "Select Save Location"
        System.Threading.Thread.Sleep(1000) 'pause

        'this deletes the annoying period at the end of the date
        fileDate = fileDate.Substring(0, fileDate.Length - 1)

        'declare a new save option
        Dim Save As New SaveFileDialog()
        'specify a few options for the save
        Save.DefaultExt = "csv"
        Save.FileName = "OUData_" & fileDate
        Save.Filter = "CSV file (*.csv)|*.csv"
        Save.CheckPathExists = True
        Save.Title = "Save OU Report"
        'show the user the save box
        cancelCheck = True
        If Save.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
            'update the user as to what is going on
            Status.Text = "Generating Report..."
            'begin saving the report to the specified location
            'check for any errors
            Try
                Using csv As New System.IO.StreamWriter(Save.FileName)
                    For Each oitem In lstNonPingable.Items
                        csv.WriteLine(oitem)
                    Next
                    cancelCheck = False
                End Using
            Catch ex As Exception
                MessageBox.Show("There has been an error in the saving process")
                cancelCheck = True
            End Try
        Else

        End If
    End Sub
    Private Sub delay_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles delay.RunWorkerCompleted
        'check to see cancelCheck is false, if it is let user know everything went OK
        'if not let the user know that there was an error
        If cancelCheck = False Then
            Status.Text = "Complete. Report Saved."
        Else
            Status.Text = "ERROR Saving Report."
        End If
        scanOU.Enabled = True
    End Sub

End Class