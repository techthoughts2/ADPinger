Imports System.ComponentModel
Imports System.Net.NetworkInformation
Imports System.Globalization
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.DirectoryServices

Public Class Form1
    Public listLoad As Boolean = False 'determine if user wants to load from list
    Public adLoad As Boolean = False 'determine if user wants to load from AD
    Public goodADConnection As Boolean = False 'used to determine if user gets the option of using AD
    Public PCNames(10000) As String 'array for storing PC names from OU
    Dim oThread(1) As System.Threading.Thread 'delay thread needed for stability
    Public intRange As Integer 'global needed for interacting with the array
    Public arraySize As Double 'needed for determining last entry in array
    '###Variables for LDAP String Manipulation###
    Public parts As String() 'first array for reversing LDAP Path
    Public parts2 As String() 'second array for reversing LDAP path
    Public partsCount As Integer = 0 'determine array length
    Public rebuild As String = "" 'variable for rebuilding the LDAP string
    '###End Variables for LDAP String Manipulation
    Public OUSelection As String 'used for user OU selection
    Public today As Date = Date.Now.ToShortDateString 'used for email and filename
    Public fmt As DateTimeFormatInfo = (New CultureInfo("hr-HR")).DateTimeFormat 'used for email and filename
    Public fileDate As String = (today.ToString("d", fmt)) 'used for email and filename
    Public letsStart As Integer = 0 'used for do loop holding for thread completion
    Public allDone As Integer = 0 'used for do loop holding for thread completion
    Public cancelCheck As Boolean = False 'used to ensure we can write to the C:\drive
    Public saveCheck As Boolean = False 'used to determine if user wishes to save or not
    Dim goodADLoad As Boolean = False ' used to ensure good AD load is completed
    Dim domainName As String ' needed for storing the users current domain
    Public tooMany As Boolean = False ' boolean check for total number of devices in OU.
    '###Variables for user selections###
    Public pingTimeout As Integer = 1000 'time for ping timeout in ms
    Public strFileScanPath As String = "" 'path to be scanned
    Public fileScanGood As Boolean = False 'determines if good scan path is available
    Public strRebootCommand As String = "" 'string for storing time and force text for reboots
    Public strDirScanPath As String = "" 'store user path for directory scan
    Public dirScanGood As Boolean = False 'determine if good directory path is available
    Public strCopySource As String = "" 'store user entered source path
    Public strCopyDestination As String = "" 'store user entered destination path
    Public createDirectory As Boolean = False 'whether or not the dir should be created during copy
    Public strjustDirectory As String = "" 'store just the directory for the destination
    Public copyGood As Boolean = False 'determine if good path for copy on both source and destination
    Public serviceName As String = "" 'stores the service name from the user 


    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'on form load we will be:
        '--setting the initial form size
        '--setting initial timeout value for pings
        '--testing the AD connection
        '--formatting the chart area to display correctly

        'set initial from size
        Me.Width = 245 'set the form width to match the form scaling later on
        Me.Height = 115

        'set initial ping timeout value (1000)
        cboPingTimeout.SelectedIndex = 1

        'set the reboot times maximum digit length
        txtRebootTime.MaxLength = 5
        'set the reboot message maximum character length
        txtShutdownMessage.MaxLength = 511

        '############################################################
        'Begin Test of AD Connection
        'this will test to see if we can give the user the option of selecting an OU
        Dim domainADsPath As String = Nothing
        Dim testDomainConnect As New DirectoryEntry
        Dim initialDomainStringHolder As String = ""
        Dim finalDomainHolder As String = ""
        'we will try to get a connection to the domain path
        Try
            If DirectoryEntry.Exists(domainADsPath) Then
                'if successful we will put the name of the domain in the label as green
                rbAD.Enabled = True
                lblADStatus.ForeColor = Color.Green
                'we will truncate the domain name down to 10 characters in case someone
                'has a really long domain name
                initialDomainStringHolder = testDomainConnect.Name.ToString
                finalDomainHolder = Strings.Left(initialDomainStringHolder, 10)
                lblADStatus.Text = finalDomainHolder
                goodADConnection = True
            Else
                'nothing, we will do it in the catch
            End If
        Catch ex As Exception
            'we could not find a domain, lets adjust the options accordingly
            rbAD.Enabled = False
            lblADStatus.ForeColor = Color.Red
        End Try
        'End Test of AD Connection
        '############################################################

        'MsgBox(Me.Width.ToString)

        'going to format how we want the percenetage chart to appear
        'we're going to hide grid lines, so that just the information
        'appears in a pretty graph
        Chart1.ChartAreas(0).AxisX.LineWidth = 0
        Chart1.ChartAreas(0).AxisY.LineWidth = 0
        Chart1.ChartAreas(0).AxisX.LabelStyle.Enabled = False
        Chart1.ChartAreas(0).AxisY.LabelStyle.Enabled = False
        Chart1.ChartAreas(0).AxisX.MajorGrid.Enabled = False
        Chart1.ChartAreas(0).AxisY.MajorGrid.Enabled = False
        Chart1.ChartAreas(0).AxisX.MinorGrid.Enabled = False
        Chart1.ChartAreas(0).AxisY.MinorGrid.Enabled = False
        Chart1.ChartAreas(0).AxisX.MajorTickMark.Enabled = False
        Chart1.ChartAreas(0).AxisY.MajorTickMark.Enabled = False
        Chart1.ChartAreas(0).AxisX.MinorTickMark.Enabled = False
        Chart1.ChartAreas(0).AxisY.MinorTickMark.Enabled = False
        Chart1.ChartAreas(0).BackColor = SystemColors.Control
        Me.Refresh()
    End Sub

    Private Sub btnScan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScan.Click
        Me.Cursor = Cursors.WaitCursor
        disableFormControls() 'so the user doesn't do anything stupid like click things during the run

        Dim validated As Boolean = False 'needed for ensuring user makes a selection

        '###########initial resets on all labels, counters, boxes, etc.#########
        resetEverything()
        'clear out the array to start fresh for each run should user do multiple runs
        For i = LBound(PCNames) To UBound(PCNames)
            PCNames(i) = ""
        Next
        '################end reset values##################

        '#####################################################################
        'begin selection determinations
        If cboAction.SelectedItem = "Ping" Then
            'set pingtimeout to user's selection
            pingTimeout = cboPingTimeout.SelectedItem
        ElseIf cboAction.SelectedItem = "Reboot" Then
            'since reboots are kind of a big deal we will ask the user
            'a few questions
            Dim askRebootSure As DialogResult
            'hey, dumbass, you didn't set a reboot time, default is 15 seconds.
            'is that OK?
            If txtRebootTime.Text = "" Then
                askRebootSure = MessageBox.Show("You did not specify a reboot time, the default will be 15 seconds.  Are you OK with this?", "No Reboot Time?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If askRebootSure = DialogResult.No Then
                    MsgBox("Stopped. No reboots performed.", MsgBoxStyle.Information)
                    cboAction.SelectedItem = "Reboot"
                    enableFormControls()
                    Me.Refresh()
                    Me.Cursor = Cursors.Arrow
                    Exit Sub
                End If
            End If
            'we are going to make sure the user really wants to
            'reboot the shit out of everything
            askRebootSure = MessageBox.Show("This option will send a reboot command to all devices in the list that you specified. This action cannot be cancelled once begun.  Are you *SURE* you want to proceed?", "Are you *SURE* you wan to reboot these devices?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            'if the user doesn't want to reboot, we will stop everything
            If askRebootSure = DialogResult.No Then
                MsgBox("Stopped. No reboots performed.", MsgBoxStyle.Information)
                cboAction.SelectedItem = "Reboot"
                enableFormControls()
                Me.Refresh()
                Me.Cursor = Cursors.Arrow
                Exit Sub
            ElseIf askRebootSure = DialogResult.Yes Then
                'otherwise, best of luck to them, this is the point of no return
                MsgBox("Proceeding with reboots.")
                strRebootCommand = ""
                strRebootCommand = modReboot.rebootCommand(strRebootCommand)
            End If
        ElseIf cboAction.SelectedItem = "Shutdown" Then
            'since shutdowns are A REALLY big deal we will ask the user
            'a few questions
            Dim askShutdownSure As DialogResult
            'hey, dumbass, you didn't set a shutdown time, default is 15 seconds.
            'is that OK?
            If txtRebootTime.Text = "" Then
                askShutdownSure = MessageBox.Show("You did not specify a shutdown time, the default will be 15 seconds.  Are you OK with this?", "No Shutdown Time?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If askShutdownSure = DialogResult.No Then
                    MsgBox("Stopped. No shutdowns performed.", MsgBoxStyle.Information)
                    cboAction.SelectedItem = "Shutdown"
                    enableFormControls()
                    Me.Refresh()
                    Me.Cursor = Cursors.Arrow
                    Exit Sub
                End If
            End If
            'we are going to make sure the user really wants to
            'shutdown the shit out of everything
            askShutdownSure = MessageBox.Show("This option will send a shutdown command to all devices in the list that you specified. This action cannot be cancelled once begun.  Are you *SURE* you wan to shutdown these devices?" & vbNewLine & "THIS IS NOT A REBOOT" & vbNewLine & "Have you considered the ramifications of this action?", "Shutdown confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            'if the user doesn't want to reboot, we will stop everything
            If askShutdownSure = DialogResult.No Then
                MsgBox("Stopped. No shutdowns performed.", MsgBoxStyle.Information)
                cboAction.SelectedItem = "Shutdown"
                enableFormControls()
                Me.Refresh()
                Me.Cursor = Cursors.Arrow
                Exit Sub
            ElseIf askShutdownSure = DialogResult.Yes Then
                'otherwise, best of luck to them, this is the point of no return
                MsgBox("Proceeding with shutdowns.")
                strRebootCommand = ""
                strRebootCommand = modReboot.rebootCommand(strRebootCommand)
            End If
        ElseIf cboAction.SelectedItem = "Scan for file" Then
            'ensure user did not leave file path blank
            If txtFileScan.Text <> "" Then
                fileScanGood = False 'reset just in case
                'first thing, lets load up what the user entered
                strFileScanPath = txtFileScan.Text.Trim
                fileScanGood = modScan.checkFilePath(strFileScanPath, fileScanGood) 'function to setup the filepath and check its validity
                If fileScanGood = True Then
                    'filePath looks ok, lets format the file scan path
                    strFileScanPath = modScan.formatFilePath(strFileScanPath)
                Else
                    'the filepath the user entered didn't check out
                    'lets shut this puppy down
                    enableFormControls()
                    Me.Cursor = Cursors.Arrow
                    Exit Sub
                End If
            Else
                'the user left the file path blank. tell them they suck
                MsgBox("You must enter a file path to scan", MsgBoxStyle.Information)
                enableFormControls()
                Me.Cursor = Cursors.Arrow
                Exit Sub
            End If
        ElseIf cboAction.SelectedItem = "Scan for directory" Then
            'ensure user did not leave directory path blank
            If txtDirScan.Text <> "" Then
                dirScanGood = False 'reset just in case
                formatDirPath() 'function to setup the directory and check its validity
                If dirScanGood = False Then
                    'the directory the user entered didn't check out
                    'lets shut this puppy down
                    enableFormControls()
                    Me.Cursor = Cursors.Arrow
                    Exit Sub
                End If
            Else
                'the user left the directory path blank. tell them they suck
                MsgBox("You must enter a directory to scan", MsgBoxStyle.Information)
                enableFormControls()
                Me.Cursor = Cursors.Arrow
                Exit Sub
            End If
        ElseIf cboAction.SelectedItem = "Copy File" Then
            'ensure user did not leave copy path blank
            If txtCopySource.Text <> "" And txtCopyDestination.Text <> "" Then
                copyGood = False 'reset just in case
                formatCopy() 'function to setup the copy path and check its validity
                If copyGood = False Then
                    'the copy path the user entered didn't check out
                    'lets shut this puppy down
                    enableFormControls()
                    Me.Cursor = Cursors.Arrow
                    Exit Sub
                End If
            Else
                'the user left the copy path blank. tell them they suck
                MsgBox("You must enter a file path for both source and desination", MsgBoxStyle.Information)
                enableFormControls()
                Me.Cursor = Cursors.Arrow
                Exit Sub
            End If
        ElseIf cboAction.SelectedItem = "Delete File" Then
            'ensure user did not leave file path blank
            If txtFileScan.Text <> "" Then
                fileScanGood = False 'reset just in case
                'first thing, lets load up what the user entered
                strFileScanPath = txtFileScan.Text.Trim
                fileScanGood = modScan.checkFilePath(strFileScanPath, fileScanGood) 'function to setup the filepath and check its validity
                If fileScanGood = True Then
                    'filePath looks ok, lets format the file scan path
                    strFileScanPath = modScan.formatFilePath(strFileScanPath)
                Else
                    'the filepath the user entered didn't check out
                    'lets shut this puppy down
                    enableFormControls()
                    Me.Cursor = Cursors.Arrow
                    Exit Sub
                End If
            Else
                'the user left the file path blank. tell them they suck
                MsgBox("You must enter a file path for deletion", MsgBoxStyle.Information)
                enableFormControls()
                Me.Cursor = Cursors.Arrow
                Exit Sub
            End If
        ElseIf cboAction.SelectedItem = "Services" Then
            'ensure service name is not blank
            If txtServiceName.Text <> "" Then
                If rdRestart.Checked = True Or rdStart.Checked = True Or rdStatus.Checked = True Or rdStop.Checked = True Then
                    'just set the serviceName, the action code towards the bottom will do the rest
                    serviceName = txtServiceName.Text
                Else
                    MsgBox("You must enter a Service action", MsgBoxStyle.Information)
                    enableFormControls()
                    Me.Cursor = Cursors.Arrow
                    Exit Sub
                End If
            Else
                'the user left the file path blank. tell them they suck
                MsgBox("You must enter a Service Name", MsgBoxStyle.Information)
                enableFormControls()
                Me.Cursor = Cursors.Arrow
                Exit Sub
            End If

        End If
        'end selection determinations
        '#####################################################################

        'depending on user choice we will now either pull a list from an OU
        'or from the list that the user selected

        If adLoad = True Then
            'we are going to begin creating the string for the LDAP path
            'ie LDAP://Domain/OU=Graveyard,DC=domain,DC=path,DC=com

            'get the current domain based on what the currect PC the user is running has
            'LDAP://Domain
            domainName = Environment.UserDomainName

            'get the users selection from the treeview selection
            'LDAP://Domain/OU=UserSelection
            'if they make a good selection, continue, otherwise halt program progress
            Try
                OUSelection = treADDomain.SelectedNode.FullPath.ToString
                'mark good selection as true to continue program
                validated = True
            Catch ex As Exception
                enableFormControls()
                validated = False
            End Try

            '###########Begin LDAP String Manipulation##########
            'the format of the user selection is not in the right format.
            'manipulate string to be in the desired format by removing the initial
            'treenode text
            OUSelection = Regex.Replace(OUSelection, "TreeNode:", "")
            'standard path down from tree is going to look something like
            'Firstpart\SecondPart\Thirdpart
            'this is bad, LDAP is reversed, lets first find those pesky backslashes
            'and change those guys to a useful ,OU=
            OUSelection = Regex.Replace(OUSelection, "\\", ",OU=")
            'now, we are going to check if the user selected something down the tree
            'or in the root.  if they selected root, there will be no ,OU= found
            'if there is one found, we are going to begin some intense string
            'manipulation to get the folder path reversed to the LDAP path
            If OUSelection.Contains(",OU=") Then
                'we start by splitting the string up and loading the values into two
                'arrays. these two arrays will then be used to reverse the order
                'of the entries
                parts = OUSelection.Split(",")
                parts2 = OUSelection.Split(",")
                partsCount = parts.Length 'determine array length for For loop
                'second x variable for reverse counting
                Dim x As Integer = parts.Length - 1
                'now we just loop through and switch around the order
                For i = 0 To partsCount - 1
                    parts(i) = parts2(x)
                    x = x - 1
                Next
                'now we rebuild the string to something useful
                For i = 0 To partsCount - 1
                    If i = parts.Length - 1 Then
                        rebuild = rebuild + "OU=" & parts(i)
                        Exit For
                    End If
                    'add the commas back in which we took out earlier
                    rebuild = rebuild + parts(i) & ","
                Next
                'load the rebuilt LDAP string into the OUSelection variable for actual use
                OUSelection = rebuild
                'get rid of all unwanted spaces from the selection
                OUSelection = OUSelection.Trim
                'build the final completed string
                OUSelection = "LDAP://" & domainName & "/" & OUSelection
                'End If
                'empty the arrays should the user wish to run again
                ReDim parts(-1)
                ReDim parts2(-1)
            Else
                'in this case, the user is selecting a root level OU
                'get rid of all unwanted spaces from the selection
                OUSelection = OUSelection.Trim
                'build the final completed string
                OUSelection = "LDAP://" & domainName & "/OU=" & OUSelection
            End If
            '###########End LDAP String Manipulation##########

            'once a good selection has been made, initiate the background worker
            If validated = True Then
                lblStatus.Visible = True
                ADLoader.RunWorkerAsync()
                lblStatus.Refresh()
                Me.Refresh()
            Else
                MsgBox("You must make an OU selection", MsgBoxStyle.Information)
                enableFormControls()
                Me.Cursor = Cursors.Arrow
                Exit Sub
            End If
        ElseIf listLoad = True Then
            'the whole thing goes into a try catch so we can determine for sure we have a
            'good list to work with.
            Try
                If lstDesktops.Items.Count = 0 Then
                    'we will again check to see if the users selected list contained names
                    'if it is empty, we won't progress or let the user do anything
                    MsgBox("The list you selected appears to be empty. Please select a list that contains properly formatted PC Names", MsgBoxStyle.Information)
                    enableFormControls()
                    Me.Cursor = Cursors.Arrow
                    Exit Sub
                Else
                    'this is where the list we just snagged gets imported into the array.  Very imporant step
                    For i = 0 To lstDesktops.Items.Count - 1
                        PCNames(i) = lstDesktops.Items(i)
                    Next
                    'we will now check the load to make sure it meets some compliance checks
                    'first is to determine the size of the list
                    arraySize = 0
                    For i = LBound(PCNames) To UBound(PCNames)
                        If PCNames(i) <> "" Then
                            arraySize = arraySize + 1
                        Else
                            Exit For
                        End If
                    Next
                    'if the list is too big we will warn the user
                    If arraySize >= 9999 Then
                        MsgBox("This list contains " & arraySize & " devices." & vbCrLf & "ADPinger is only designed to handle lists containingn less than 10,000 devices." & vbCrLf & "Sorry for the inconvenience", MsgBoxStyle.Critical)
                        validated = False
                        tooMany = True
                    End If
                End If
                validated = True
            Catch ex As Exception
                validated = False
            End Try

            'basic checks to see if everything is ready to actually,
            'FINALLY get started
            If validated = True And tooMany = False Then
                'if validation is good and there aren't too many, lets start!
                lblStatus.Visible = True
                lblStatus.Refresh()
                Me.Refresh()
                cmdPingComputers()
            ElseIf validated = True And tooMany = True Then
                'too many, lets indicate this and shut this puppy down
                lblStatus.ForeColor = Color.Red
                lblStatus.Text = "Size > 10000"
                enableFormControls()
                Me.Cursor = Cursors.Arrow
                Me.Refresh()
                Exit Sub
            ElseIf validated = False Then
                'something has gone horribly, horribly wrong
                MsgBox("Something went wrong with loading your PC Names from the specified file.  Is it formatted correctly?", MsgBoxStyle.Exclamation)
                enableFormControls()
                Me.Cursor = Cursors.Arrow
                Exit Sub
            End If
        Else
            'in this case the user didn't choose one of the radio buttons. idiots
            MsgBox("You must choose to load from an OU or from a file", MsgBoxStyle.Information)
            enableFormControls()
            Me.Cursor = Cursors.Arrow
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub cboAction_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboAction.SelectedIndexChanged
        'the action perfomed will essentially start the process from scratch when the user selects a new action
        'the form will resize and all settings will be put to stock
        'ALL SETTINGS

        '############################################################
        'begin resize of form
        Me.Cursor = Cursors.WaitCursor
        If Me.Width > 505 Then
            'Me.Cursor = Cursors.WaitCursor
            Do Until Me.Width = 505
                Me.Width = Me.Width - 1
            Loop
            'Me.Cursor = Cursors.Arrow
        ElseIf Me.Width < 505 Then
            'Me.Cursor = Cursors.WaitCursor
            Do Until Me.Width = 505
                Me.Width = Me.Width + 1
            Loop
            'Me.Cursor = Cursors.Arrow
        End If

        If Me.Height > 250 Then
            'Me.Cursor = Cursors.WaitCursor
            Do Until Me.Height = 250
                Me.Height = Me.Height - 1
            Loop
            'Me.Cursor = Cursors.Arrow
        ElseIf Me.Height < 250 Then
            'Me.Cursor = Cursors.WaitCursor
            Do Until Me.Height = 250
                Me.Height = Me.Height + 1
            Loop
            'Me.Cursor = Cursors.Arrow
        End If
        Me.Cursor = Cursors.Arrow
        '############################################################

        resetEverything() 'function that resets most form stuff
        resetSelections() 'function that resets user selected choices
        lblListStatus.Visible = False

        '###########################################################
        'begin checks to show necessary options
        If cboAction.SelectedItem = "Ping" Then
            pnlPing.Location = New Point(15, 103)
            pnlPing.Visible = True
            btnScan.Text = "Begin" & vbNewLine & "Pings"
        ElseIf cboAction.SelectedItem = "Reboot" Then
            chkForceReboot.Text = "Force Reboot"
            pnlReboot.Location = New Point(13, 103)
            pnlReboot.Visible = True
            btnScan.Text = "Begin" & vbNewLine & "Reboots"
        ElseIf cboAction.SelectedItem = "Shutdown" Then
            chkForceReboot.Text = "Force Shutdown"
            lblCustomMessage.ForeColor = Color.Red
            chkForceReboot.ForeColor = Color.Red
            lblTime.ForeColor = Color.Red
            pnlReboot.Location = New Point(13, 103)
            pnlReboot.Visible = True
            btnScan.Text = "Begin" & vbNewLine & "Shutdowns"
        ElseIf cboAction.SelectedItem = "Scan for file" Then
            pnlScanFile.Location = New Point(15, 103)
            pnlScanFile.Visible = True
            btnScan.Text = "Begin" & vbNewLine & "Scan"
        ElseIf cboAction.SelectedItem = "Scan for directory" Then
            pnlScanDir.Location = New Point(15, 103)
            pnlScanDir.Visible = True
            btnScan.Text = "Begin" & vbNewLine & "Scan"
        ElseIf cboAction.SelectedItem = "Copy File" Then
            pnlCopy.Location = New Point(15, 103)
            pnlCopy.Visible = True
            btnScan.Text = "Begin" & vbNewLine & "Copy"
        ElseIf cboAction.SelectedItem = "Delete File" Then
            pnlScanFile.Location = New Point(15, 103)
            pnlScanFile.Visible = True
            btnScan.Text = "Begin" & vbNewLine & "Deletion"
        ElseIf cboAction.SelectedItem = "Services" Then
            pnlServices.Location = New Point(15, 103)
            pnlServices.Visible = True
            btnScan.Text = "Begin" & vbNewLine & "Services"
        End If
        'end options check
        '###########################################################
    End Sub

    Private Sub rbAD_CheckedChanged(sender As Object, e As EventArgs) Handles rbAD.CheckedChanged
        'this is based on a change status to the radio button which is not ideal.
        'to compensate for this we will always start off by checking to see if it is actually checked or not
        If rbAD.Checked = True Then
            'reset selection
            'necessary to compensate for situations where the user might switch back and forth between the buttons
            adLoad = False
            listLoad = False
            lblListStatus.Visible = False

            'this radial button only available if a good domain is detected (see formload)
            'when the user selects this radial button the first thing we will do
            'is delete the existing contents (if any) of the treeview.
            'this prevents the treeview from being populated with the same content multiple times
            treADDomain.Nodes.Clear()
            'since the user selected an AD list that is OU based we will change the form size
            'to accomadate the treeview size
            '############################################################
            'begin resize of form
            Me.Cursor = Cursors.WaitCursor
            If Me.Width > 850 Then
                Do Until Me.Width = 850
                    Me.Width = Me.Width - 1
                Loop
            ElseIf Me.Width < 850 Then
                Do Until Me.Width = 850
                    Me.Width = Me.Width + 1
                Loop
            End If
            If Me.Height > 700 Then
                Do Until Me.Height = 700
                    Me.Height = Me.Height - 1
                Loop
            ElseIf Me.Height < 700 Then
                Do Until Me.Height = 700
                    Me.Height = Me.Height + 1
                Loop
            End If
            Me.Cursor = Cursors.Arrow
            'end resize of form
            '############################################################
            'scan the AD domain and populates containers or OU's it finds
            'into a treeview for the user to interact with
            Dim domainADsPath As String = Nothing
            Dim childEntry As DirectoryEntry
            Dim ParentEntry As New DirectoryEntry(domainADsPath)

            'attempt to populate, if it fails this is likely due to the user not having AD
            'access permissions
            Try
                Me.Cursor = Cursors.WaitCursor
                For Each childEntry In ParentEntry.Children
                    Dim newNode As New TreeNode(childEntry.Name)
                    Select Case childEntry.SchemaClassName
                        Case "organizationalUnit"
                            Dim ParentDomain As New TreeNode(childEntry.Name.Substring(Name.Length - 2))
                            treADDomain.Nodes.AddRange(New TreeNode() {ParentDomain})
                            ParentDomain.Tag = childEntry.Path
                            'load dummy entries to populate plus boxes in treeview
                            'these are later removed.
                            If ParentDomain IsNot Nothing Then
                                If ParentDomain.Nodes.Count = 0 Then
                                    ParentDomain.Nodes.Add("Dummy")
                                End If
                            End If
                        Case "container" 'important for catching default containers like Computers
                            Dim ParentDomain As New TreeNode(childEntry.Name.Substring(Name.Length - 2))
                            treADDomain.Nodes.AddRange(New TreeNode() {ParentDomain})
                            ParentDomain.Tag = childEntry.Path
                            'load dummy entries to populate plus boxes in treeview
                            'these are later removed.
                            If ParentDomain IsNot Nothing Then
                                If ParentDomain.Nodes.Count = 0 Then
                                    ParentDomain.Nodes.Add("Dummy")
                                End If
                            End If
                    End Select
                Next childEntry
            Catch Excep As Exception
                MsgBox(Err.Description, MsgBoxStyle.Exclamation)
            Finally
                ParentEntry = Nothing
                Me.Cursor = Cursors.Arrow
                'the user has successfully chosen to work with an OU
                adLoad = True
            End Try
        End If
    End Sub

    Private Sub rdList_CheckedChanged(sender As Object, e As EventArgs) Handles rbList.CheckedChanged
        'this is based on a change status to the radio button which is not ideal.
        'to compensate for this we will always start off by checking to see if it is actually checked or not
        If rbList.Checked = True Then
            '############################################################
            'begin resize of form
            Me.Cursor = Cursors.WaitCursor
            If Me.Width > 505 Then
                'Me.Cursor = Cursors.WaitCursor
                Do Until Me.Width = 505
                    Me.Width = Me.Width - 1
                Loop
                'Me.Cursor = Cursors.Arrow
            ElseIf Me.Width < 505 Then
                'Me.Cursor = Cursors.WaitCursor
                Do Until Me.Width = 505
                    Me.Width = Me.Width + 1
                Loop
                'Me.Cursor = Cursors.Arrow
            End If

            If Me.Height > 250 Then
                'Me.Cursor = Cursors.WaitCursor
                Do Until Me.Height = 250
                    Me.Height = Me.Height - 1
                Loop
                'Me.Cursor = Cursors.Arrow
            ElseIf Me.Height < 250 Then
                'Me.Cursor = Cursors.WaitCursor
                Do Until Me.Height = 250
                    Me.Height = Me.Height + 1
                Loop
                'Me.Cursor = Cursors.Arrow
            End If
            Me.Cursor = Cursors.Arrow
            '############################################################

            'reset selection
            lblListStatus.Visible = False
            listLoad = False
            adLoad = False
            lstDesktops.Items.Clear()

            'explain to the user how to format the text file, formatting is important
            MsgBox("Please select a properly formatted Text/CSV file containing PC Names" & vbNewLine & "Load PC Names into a text file with the following format:" & vbNewLine & "Ex:" & vbNewLine & "PCName1" & vbNewLine & "PCName2" & vbNewLine & "PCName3" & vbNewLine & "etc...")

            Me.Cursor = Cursors.WaitCursor
            verifyList() 'function gets the selected list and performs control checks on it

            'either way we are getting rid of the radio button selection
            'if the user fucked up the list, they are going to get to try again
            'if it worked perefctly, we get rid of the button so they can load
            'a different list later if they want
            rbList.Checked = False
            Me.Cursor = Cursors.Arrow
        End If
    End Sub

    Private Sub ADLoader_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles ADLoader.DoWork
        'this background worker scans the selected OU, and loads all PC names 
        'into an array
        'Me.Cursor = Cursors.WaitCursor
        Dim adProgressTracker As Integer = 0 'variable for fake reporting to the progress bar

        'variables used for AD connection
        Dim objRootDSE, strDomain, objConnection, objCommand, objRecordSet 'path
        Const ADS_SCOPE_SUBTREE = 2
        'variable used for array interaction
        Dim i As Integer = 0

        'fake reporting to progress bar to approximate to user how far along we are
        'in getting the info we need from AD
        adProgressTracker = 5
        ADLoader.ReportProgress(adProgressTracker)

        'Get domain components
        'this determines what the root LDAP is and looks in the entire AD environment
        objRootDSE = GetObject("LDAP://RootDSE")
        strDomain = objRootDSE.Get("DefaultNamingContext")
        OUSelection = OUSelection & "," & strDomain

        'fake reporting to progress bar to approximate to user how far along we are
        'in getting the info we need from AD
        adProgressTracker = 10
        ADLoader.ReportProgress(adProgressTracker)

        'Set ADO connection
        objConnection = CreateObject("ADODB.Connection")
        objConnection.Provider = "ADsDSOObject"
        objConnection.Open("Active Directory Provider")

        'fake reporting to progress bar to approximate to user how far along we are
        'in getting the info we need from AD
        adProgressTracker = 15
        ADLoader.ReportProgress(adProgressTracker)

        'Set ADO command
        objCommand = CreateObject("ADODB.Command")
        objCommand.ActiveConnection = objConnection
        objCommand.Properties("Searchscope") = ADS_SCOPE_SUBTREE
        objCommand.CommandText = "SELECT name,cn,adsPath FROM '" & OUSelection & " ' WHERE objectCategory='computer' "
        'objCommand.CommandText = "SELECT accountExpires,mail,displayName FROM 'LDAP://" & strDomain & "' WHERE objectCategory='user' "
        'AND samAccountName = '" & strUsername & "'"
        objCommand.Properties("Page Size") = 15000

        'fake reporting to progress bar to approximate to user how far along we are
        'in getting the info we need from AD
        adProgressTracker = 25
        ADLoader.ReportProgress(adProgressTracker)
        'we have a try here because there is small chance the user will scan
        'a default container (like Computers) instead of an OU. default containers
        'have a CN instead of OU so the string we created up to this point won't work
        'if the user tries to ping a default container
        Try
            'Set recordset to hold the query result
            objRecordSet = objCommand.Execute
            objRecordSet.Movefirst()
            goodADLoad = True
            'fake reporting to progress bar to approximate to user how far along we are
            'in getting the info we need from AD
            adProgressTracker = 70
            ADLoader.ReportProgress(adProgressTracker)
        Catch ex As Exception
            'if the user in fact loaded a default container, we will rebuild the string to
            'have a CN instead of OU and try again.
            If goodADLoad = False Then
                Try
                    'OUSelection = "LDAP://CGHS-DC1/CN=Computers,DC=cghsnt,DC=mccg,DC=org"
                    OUSelection = Regex.Replace(OUSelection, "OU", "CN")
                    'rebuild string
                    objCommand.CommandText = "SELECT name,cn,adsPath FROM '" & OUSelection & " ' WHERE objectCategory='computer' "
                    'Set recordset to hold the query result
                    objRecordSet = objCommand.Execute
                    objRecordSet.Movefirst()
                    goodADLoad = True
                    'fake reporting to progress bar to approximate to user how far along we are
                    'in getting the info we need from AD
                    adProgressTracker = 70
                    ADLoader.ReportProgress(adProgressTracker)
                    Exit Try
                Catch ey As Exception
                    'in this case we successfully accessed the Object, but there were no computers
                    'or computers + objects we can't ping. Alert the user to try again. Kill thread, set
                    'back to defaults
                    adProgressTracker = 0
                    ADLoader.ReportProgress(adProgressTracker)
                    MsgBox("The OU you have selected cannot be processed." & vbCrLf & "There are two possbilities:" & vbCrLf & "1. The OU you selected contains users instead of computers." & vbCrLf & "2. The OU you selected is empty" & vbCrLf & vbCrLf & "Please try another selection/OU.", MsgBoxStyle.Critical)
                    goodADLoad = False
                    ADLoader.CancelAsync()
                    ADLoader.Dispose()
                    Exit Sub
                End Try
                If goodADLoad = False Then
                    'in this case we successfully accessed the Object, but there were no computers
                    'or computers + objects we can't ping. Alert the user to try again. Kill thread, set
                    'back to defaults
                    adProgressTracker = 0
                    ADLoader.ReportProgress(adProgressTracker)
                    MsgBox("The OU you have selected appears to contain users instead of computers. Please try another selection/OU.", MsgBoxStyle.Critical)
                    goodADLoad = False
                    ADLoader.CancelAsync()
                    ADLoader.Dispose()
                    Exit Sub
                ElseIf goodADLoad = True Then
                    Exit Try
                Else
                    'Something really wrong has happened if we reach this point.
                    'kill the program, don't know how to handle exactly because anything could go wrong
                    MsgBox("Critical Unknown Error", MsgBoxStyle.Critical)
                    Me.Close()
                End If
            End If
        End Try
        'fake reporting to progress bar to approximate to user how far along we are
        'in getting the info we need from AD
        adProgressTracker = 75
        ADLoader.ReportProgress(adProgressTracker)
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
        'fake reporting to progress bar to approximate to user how far along we are
        'in getting the info we need from AD
        adProgressTracker = 90
        ADLoader.ReportProgress(adProgressTracker)

        'because of the setup of the array Ubound cannot be used to determine last entry
        'as it is statically set to 10000. this counter loop will determine
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
        'end of sub cleanup. finish up the progress bar, and pause for one second before pinging
        adProgressTracker = 100
        ADLoader.ReportProgress(adProgressTracker)
        'Me.Cursor = Cursors.Arrow
        'check to ensure that total OU devices doesn't exceed 10,000.
        'if it does, we're going to display an error message and not let the program go further
        If arraySize >= 9999 Then
            MsgBox("This OU contains " & arraySize & " devices." & vbCrLf & "ADPinger is only designed to handle OU's with less than 10,000 devices." & vbCrLf & "Sorry for the inconvenience", MsgBoxStyle.Critical)
            tooMany = True
        End If
        System.Threading.Thread.Sleep(1000)
    End Sub

    Private Sub ADLoader_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles ADLoader.ProgressChanged
        'sub that changes the progress bar
        ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub ADLoader_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ADLoader.RunWorkerCompleted
        'check to see that everything went smooth with the AD load
        If goodADLoad = True Then
            'also check to make sure we don't have too many devices.
            If tooMany = False Then
                'begin pinging computers
                cmdPingComputers()
                'if we do have too many devices, let the user try again
            Else
                lblStatus.ForeColor = Color.Red
                lblStatus.Text = "OU Size > 10000"
                enableFormControls()
                Me.Cursor = Cursors.Arrow
                Me.Refresh()
            End If
            'if not give the user an error and let the user try again
        Else
            lblStatus.ForeColor = Color.Red
            lblStatus.Text = "OU Selection Error"
            enableFormControls()
            Me.Cursor = Cursors.Arrow
            Me.Refresh()
        End If
    End Sub

    Private Sub cmdPingComputers()
        'we are going to begin firing off unmanaged threads. this is necessary
        'to ping all computers simultaneously. running this in serial would result
        'in a very long drawn out process.  in this code each computer's pinging action
        'is handled by an independent thread.  so, as fast as we can fire threads,
        'thats how fast PC's start getting pinged.  unfortunately, this happens too fast
        'with the current code and causes instability.  it becomes necessary to slightly
        'delay each thread so that the program remains stable.

        Me.ProgressBar1.Value = 0

        'necessary to ensure that we don't fire more threads than pc's in the array
        'please note that intRange is currently incremented inside each independent thread
        'this is critical.  incrementing inside this function causes instablity
        'and faulty data
        intRange = 0
        lblStatus.Text = "Pinging PC's..."
        Me.Refresh()
        'begin thread creation, NO MORE THAN 1000
        ReDim oThread(1000)
        For Q As Integer = 0 To 1000 - 1 Step 1
            'delay next thread creation. without this, the program does some 
            'weird things
            System.Threading.Thread.Sleep(40)
            'once intRange is the same as arraySize, we stop thread creation
            'everything will now be pinged by the threads we created
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
        Me.ProgressBar1.Maximum = letsStart 'set progress par to number of threads created to track progress
        completionChecker.RunWorkerAsync()
    End Sub

    Private Sub completionChecker_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles completionChecker.DoWork
        'here we are going to hold a thread in a loop until all ping threads
        'report that they are done.
        Me.Cursor = Cursors.WaitCursor
        'lets check to see if they are all done!
        Do While allDone < letsStart
            If letsStart = allDone Then
                Exit Do
            Else
                'while the threads are still working we will update the progress
                'bar once every second for the user and delay checking the status
                'of the threads for four seconds total
                Me.ProgressBar1.Value = allDone
                System.Threading.Thread.Sleep(1000)
                Me.ProgressBar1.Value = allDone
                System.Threading.Thread.Sleep(1000)
                Me.ProgressBar1.Value = allDone
                System.Threading.Thread.Sleep(1000)
                Me.ProgressBar1.Value = allDone
                System.Threading.Thread.Sleep(1000)
            End If
        Loop
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub completionChecker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles completionChecker.RunWorkerCompleted
        'start delay backround worker
        'necessary to ensure we don't process the report creation or send the
        'email until all threads are complete
        'note that this will not be called until all threads have been fired for
        'all PC's inside the OU
        lblActiveThreads.Text = "0" 'program never brings it all the way back down. known bug, this is a quick fix
        Me.ProgressBar1.Value = letsStart 'program never maxes it 100% complete, known bug, this is a quick fix
        'Me.Refresh()
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
        'original ping code developed by Justin Saylor, it has been heavily
        'modified and adapted for this use, but still has many of the core
        'logic and functionality
        For x = i To j
            If x < arraySize Then
                Try
                    Dim _pingreply = _ping.Send(PCNames(x), pingTimeout) '1000 default
                    If _pingreply.Status = IPStatus.Success Then
                        SyncLock lstPingable
                            lstPingable.Items.Add(PCNames(x))
                        End SyncLock
                        SyncLock lstNonPingable
                            If InStr(_pingreply.Status, "0") <> Nothing Then
                                If cboAction.SelectedItem = "Ping" Then
                                    strBS = "Success"
                                ElseIf cboAction.SelectedItem = "Reboot" Or cboAction.SelectedItem = "Shutdown" Then
                                    If InStr(_pingreply.Status, "0") <> Nothing Then
                                        MsgBox("ShutDown " & strRebootCommand & PCNames(x))
                                        Try
                                            System.Diagnostics.Process.Start("ShutDown", strRebootCommand & PCNames(x))
                                            If cboAction.SelectedItem = "Reboot" Then
                                                strBS = "Reboot Sent"
                                            ElseIf cboAction.SelectedItem = "Shutdown" Then
                                                strBS = "Shutdown Sent"
                                            End If
                                        Catch ex As Exception
                                            strBS = "Unable to send command"
                                        End Try
                                    End If
                                ElseIf cboAction.SelectedItem = "Scan for file" Then
                                    Try
                                        If File.Exists("\\" & PCNames(x) & "\" & strFileScanPath) Then
                                            strBS = "File Found"
                                        Else
                                            strBS = "File Not Found"
                                        End If
                                    Catch ex As Exception
                                        strBS = "Unable to determine"
                                    End Try
                                ElseIf cboAction.SelectedItem = "Scan for directory" Then
                                    Try
                                        If Directory.Exists("\\" & PCNames(x) & "\" & strDirScanPath) Then
                                            strBS = "Directory Found"
                                        Else
                                            strBS = "Directory Not Found"
                                        End If
                                    Catch ex As Exception
                                        strBS = "Unable to determine"
                                    End Try
                                ElseIf cboAction.SelectedItem = "Copy File" Then
                                    Try
                                        If createDirectory = True Then
                                            If Directory.Exists("\\" & PCNames(x) & "\" & strjustDirectory) Then
                                                File.Copy(strCopySource, "\\" & PCNames(x) & "\" & strCopyDestination)
                                            Else
                                                Directory.CreateDirectory("\\" & PCNames(x) & "\" & strjustDirectory)
                                                File.Copy(strCopySource, "\\" & PCNames(x) & "\" & strCopyDestination)
                                            End If
                                            strBS = "File Copied - Dir Created"
                                        Else
                                            If Directory.Exists("\\" & PCNames(x) & "\" & strjustDirectory) Then
                                                File.Copy(strCopySource, "\\" & PCNames(x) & "\" & strCopyDestination)
                                                strBS = "File Copied"
                                            Else
                                                strBS = "File *NOT* copied - Dir does not exist"
                                            End If
                                        End If
                                    Catch ex As Exception
                                        strBS = "File *NOT* copied"
                                    End Try
                                ElseIf cboAction.SelectedItem = "Delete File" Then
                                    Try
                                        If File.Exists("\\" & PCNames(x) & "\" & strFileScanPath) Then
                                            Try
                                                File.Delete("\\" & PCNames(x) & "\" & strFileScanPath)
                                                strBS = "File Deleted"
                                            Catch ex As Exception
                                                strBS = "File could not be deleted"
                                            End Try
                                        Else
                                            strBS = "File Not Found"
                                        End If
                                    Catch ex As Exception
                                        strBS = "Unable to determine"
                                    End Try
                                ElseIf cboAction.SelectedItem = "Services" Then
                                    If rdStatus.Checked = True Then
                                        Try
                                            strBS = modServices.serviceStatus(serviceName, 1, PCNames(x))
                                        Catch ex As Exception
                                            strBS = "Error interacting with remote service"
                                        End Try
                                    ElseIf rdRestart.Checked = True Then
                                        Try
                                            strBS = modServices.serviceStatus(serviceName, 2, PCNames(x))
                                        Catch ex As Exception
                                            strBS = "Error interacting with remote service"
                                        End Try
                                    ElseIf rdStop.Checked = True Then
                                        Try
                                            strBS = modServices.serviceStatus(serviceName, 3, PCNames(x))
                                        Catch ex As Exception
                                            strBS = "Error interacting with remote service"
                                        End Try
                                    ElseIf rdStart.Checked = True Then
                                        Try
                                            strBS = modServices.serviceStatus(serviceName, 4, PCNames(x))
                                        Catch ex As Exception
                                            strBS = "Error interacting with remote service"
                                        End Try
                                    End If
                                End If
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

                allDone = allDone + 1
                Exit Sub
            End If
        Next

        SyncLock lstNonPingable
        End SyncLock
        'this thread (remember, there are lots of these) is now complete.
        'report that this thread is now over
        allDone = allDone + 1
        lblActiveThreads.Text = lblActiveThreads.Text - 1
    End Sub

    Private Sub delay_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles delay.DoWork
        'update user as to current status of the program.  at this point all names
        'have been loaded, all pc's have been pinged, and listboxes have been populated
        'however, the program because of the # of threads tends to lag a little
        'this will pause everything for about 1 second to let everything catch up
        'it will then prompt the use to save the csv report

        'some sloppy coding here, but...hey, it works.
        Control.CheckForIllegalCrossThreadCalls = False

        System.Threading.Thread.Sleep(1000) 'pause

        lblGoodCount.Text = lstPingable.Items.Count 'show total PC's pinged
        lblTotalCount.Text = lstNonPingable.Items.Count 'shows total PC's with good pings

        lblStatus.Text = "Select Save Location"

        'this deletes the annoying period at the end of the date
        fileDate = fileDate.Substring(0, fileDate.Length - 1)

        'ask the user if they want to actually save a report of the results
        Dim askSave As DialogResult
        askSave = MessageBox.Show("Would you like to save a CSV report of the results?", "Save?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        'if they do want to save, show them the save dialogo
        saveCheck = True
        If askSave = DialogResult.Yes Then
            saveCheck = False
            'declare a new save option
            Dim Save As New SaveFileDialog()
            'specify a few options for the save
            Save.DefaultExt = "csv"
            Save.FileName = cboAction.Text & "_" & fileDate
            Save.Filter = "CSV file (*.csv)|*.csv"
            Save.CheckPathExists = True
            Save.Title = "Save OU Report"
            'cancelcheck and savecheck will be used to determine how the report turned out
            'there are a few possibilites.
            'the user may elect not to save at all
            'elect to save but click cancel on save dialog
            'encounter an error in the saving process (no write access to drive)
            'these checks will see how everything turns out and report
            'the correct results
            cancelCheck = True
            'show the user the save box
            If Save.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                'update the user as to what is going on
                lblStatus.Text = "Generating Report..."
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
                    cancelCheck = ""
                End Try
            Else
                cancelCheck = True
            End If
        Else
            saveCheck = True
        End If
    End Sub

    Private Sub delay_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles delay.RunWorkerCompleted
        'check to see cancelCheck is false, if it is let user know everything went OK
        'if not let the user know that there was an error
        'cancelcheck and savecheck will be used to determine how the report turned out
        'there are a few possibilites.
        'the user may elect not to save at all
        'elect to save but click cancel on save dialog
        'encounter an error in the saving process (no write access to drive)
        'these checks will see how everything turns out and report
        'the correct results
        If saveCheck = False And cancelCheck = False Then
            lblStatus.Text = "Complete. Report Saved."
        ElseIf saveCheck = False And cancelCheck = True Then
            lblStatus.Text = "Complete. Report Canceled."
        ElseIf saveCheck = True Then
            lblStatus.Text = "Complete. No Report Saved."
        ElseIf saveCheck = False And cancelCheck = "" Then
            lblStatus.Text = "ERROR Saving Report."
        End If
        enableFormControls()

        'from this point forward we begin formating the data for the percentage chart
        Dim good As Double = lstPingable.Items.Count 'good percentage
        Dim total As Double = lstNonPingable.Items.Count 'total number pinged
        Dim percentage As Decimal 'variable used to calculate percentage

        percentage = CDec(good) / CDec(total) 'calculate percentage
        percentage = percentage * 100 'format percentage

        total = total - good 'this is needed to properly format the graph

        lblPercentGood.Text = Format(percentage, "0.00") & "%" 'change decimal places
        'it should be noted that these are reversed, i'm not sure why
        Chart1.Series("Good").Points.Add.SetValueXY(2, total) 'plot data on graph
        Chart1.Series("Total").Points.Add.SetValueXY(2, good) 'plot data on graph
        'show all the label information to the user
        lblPercentGood.Visible = True
        Label6.Visible = True
        Chart1.Visible = True
    End Sub

    Private Sub treADDomain_BeforeExpand(sender As Object, e As TreeViewCancelEventArgs) Handles treADDomain.BeforeExpand
        e.Node.Nodes.Clear() 'clears previous dummy entries entered to show the plus sign in the treeview
        'populates child structures beneath parent in the treeview
        Dim domainADsPath As String = e.Node.Tag
        Dim childEntry As DirectoryEntry
        Dim ParentEntry As New DirectoryEntry(domainADsPath)
        Dim SubParentEntry As New DirectoryEntry
        'Me.Cursor = Cursors.WaitCursor 'show the wait cursor, not sure if it works or not
        Me.Refresh()
        'attempt to populate, if it fails this is likely due to the user not having AD
        'access permissions
        Try
            Me.Cursor = Cursors.WaitCursor
            For Each childEntry In ParentEntry.Children
                Dim newNode As New TreeNode(childEntry.Name)
                Select Case childEntry.SchemaClassName
                    Case "organizationalUnit"
                        Dim ParentDomain As New TreeNode(childEntry.Name.Substring(Name.Length - 2))
                        e.Node.Nodes.AddRange(New TreeNode() {ParentDomain})
                        ParentDomain.Tag = childEntry.Path
                        'load dummy entries to populate plus boxes in treeview
                        'these are later removed.
                        If ParentDomain IsNot Nothing Then
                            If ParentDomain.Nodes.Count = 0 Then
                                ParentDomain.Nodes.Add("Dummy")
                            End If
                        End If
                    Case "user"
                        Dim ParentDomain As New TreeNode(childEntry.Name.Substring(Name.Length - 2))
                        e.Node.Nodes.AddRange(New TreeNode() {ParentDomain})
                        ParentDomain.Tag = childEntry.Path
                        'load dummy entries to populate plus boxes in treeview
                        'these are later removed.
                        If ParentDomain IsNot Nothing Then
                            If ParentDomain.Nodes.Count = 0 Then
                                ParentDomain.Nodes.Add("Dummy")
                            End If
                        End If
                End Select
            Next childEntry
        Catch Excep As Exception
            MsgBox(Err.Description, MsgBoxStyle.Exclamation)
        Finally
            ParentEntry = Nothing
            Me.Cursor = Cursors.Arrow
            treADDomain.SelectedNode = Nothing
        End Try
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        aboutBox.Show()
    End Sub

    Private Function removeDups()
        'this function writes all the ontents of the listbox into an array and checks for
        'duplicates.  it then has a good array list with no duplicates. it clears the original
        'list box and writes the good array back into the list box
        Dim i, j As Integer
        Dim arr As New ArrayList
        Dim itemFound As Boolean
        For i = 0 To lstDesktops.Items.Count - 1
            itemFound = False
            For j = 0 To i - 1
                If lstDesktops.Items.Item(i) = lstDesktops.Items.Item(j) Then
                    itemFound = True
                    Exit For
                End If
            Next
            If Not itemFound Then
                arr.Add(lstDesktops.Items.Item(i))
            End If
        Next
        lstDesktops.Items.Clear()
        lstDesktops.Items.AddRange(arr.ToArray)
        arr = Nothing
    End Function

    Function resetSelections()
        'selectables
        rbList.Checked = False
        rbAD.Checked = False

        cboPingTimeout.SelectedIndex = 1
        pingTimeout = 1000

        txtRebootTime.Text = ""
        chkForceReboot.Checked = False
        strRebootCommand = ""

        txtFileScan.Text = ""
        strFileScanPath = ""
        fileScanGood = False

        txtDirScan.Text = ""
        strDirScanPath = ""
        dirScanGood = False

        txtCopyDestination.Text = ""
        strCopySource = ""
        strCopyDestination = ""
        strjustDirectory = ""
        copyGood = False
        chkCreateDir.Checked = False
        createDirectory = False

        serviceName = ""
        txtServiceName.Text = ""

        listLoad = False
        adLoad = False

        lstDesktops.Items.Clear()
        'end selectables

        'visables
        pnlPing.Visible = False
        pnlReboot.Visible = False
        pnlScanFile.Visible = False
        pnlScanDir.Visible = False
        pnlCopy.Visible = False
        pnlServices.Visible = False
        'end visables
    End Function

    Function resetEverything()

        '###########initial resets on all labels, counters, boxes, etc.#########

        'form controls revert
        lblStatus.Visible = False
        lblStatus.Text = "Loading PC's from OU..."
        lblStatus.ForeColor = Color.Black
        lblThreads.Text = "0"
        lblActiveThreads.Text = "0"
        lblGoodCount.Text = 0
        lblTotalCount.Text = 0
        Chart1.Visible = False
        Label6.Visible = False
        lblPercentGood.Visible = False
        'end form controls

        'clear boxes
        lstNonPingable.Items.Clear()
        lstPingable.Items.Clear()
        'treADDomain.Nodes.Clear()
        'end clear boxes

        'reset counters
        intRange = 0
        letsStart = 0
        allDone = 0
        arraySize = 0
        ProgressBar1.Value = 0
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = 100
        partsCount = 0

        'reset booleans
        cancelCheck = False
        saveCheck = False
        goodADLoad = False
        tooMany = False
        'end reset booleans

        'reset strings
        rebuild = ""
        OUSelection = ""
        'end reset strings

        'reset warning labels
        lblCustomMessage.ForeColor = SystemColors.ControlText
        chkForceReboot.ForeColor = SystemColors.ControlText
        lblTime.ForeColor = SystemColors.ControlText
        '################end reset values##################
    End Function

    Function disableFormControls()
        'disable some form controls so the user doesn't do anything stupid
        btnScan.Enabled = False 'while the program is running the buttons has to be taken away
        btnReset.Enabled = False
        rbAD.Enabled = False
        rbList.Enabled = False
        cboAction.Enabled = False
        treADDomain.Enabled = False
        cboPingTimeout.Enabled = False
        chkForceReboot.Enabled = False
        txtRebootTime.Enabled = False
        txtFileScan.Enabled = False
        txtDirScan.Enabled = False
        txtCopyDestination.Enabled = False
        txtCopySource.Enabled = False
        chkForceReboot.Enabled = False
        chkCreateDir.Enabled = False
        'end disable form controls
    End Function

    Function enableFormControls()
        btnScan.Enabled = True
        btnReset.Enabled = True
        rbList.Enabled = True
        cboAction.Enabled = True
        treADDomain.Enabled = True
        cboPingTimeout.Enabled = True
        chkForceReboot.Enabled = True
        txtRebootTime.Enabled = True
        txtFileScan.Enabled = True
        txtDirScan.Enabled = True
        txtCopyDestination.Enabled = True
        txtCopySource.Enabled = True
        chkForceReboot.Enabled = True
        chkCreateDir.Enabled = True
        'special case, if there was no good ad connection,
        'then we will not re-enable this form
        If goodADConnection = True Then
            rbAD.Enabled = True
        End If
    End Function

    Function verifyList()
        'oy.. this whole thing could probably stand to be re-written.
        'the gist of this function is that it will prompt the user to open
        'a file and then will perform numerous checks to ensure that the file
        'selected by the user is something we can work with.
        'if it doesn't meet the criteria, we don't let anything happen

        'here are some initial settings that setup the Browse settings
        'for how Windows acts as the user browses for the text file
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
        OpenFileDialog1.Title = "Open a Text File"
        OpenFileDialog1.Filter = "Text or CSV Files (*.txt;*.csv)|*.txt;*.csv"

        'lets check to see if the user actually wants to load the file
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then

            'check to make sure a valid file was selected

            '###########################################################
            'begin file checks and processing

            '##check1 - did user select correct file type?
            Dim extension As String 'variable for determining file type
            Try
                extension = Path.GetExtension(OpenFileDialog1.FileName)
                If extension = ".txt" Or extension = ".csv" Then
                    '##processing1 - load the selected list into the listbox
                    'the user at least selected the right file type, lets load
                    'the data in and do some more checking
                    lstDesktops.Items.AddRange(Split(My.Computer.FileSystem.ReadAllText(OpenFileDialog1.FileName), vbNewLine))
                    '##end processing1
                Else
                    MsgBox("Only a Text File (*.txt) or CSV file (*.csv) is allowed", MsgBoxStyle.Exclamation)
                    lblListStatus.Visible = True
                    lblListStatus.ForeColor = Color.Red
                    lblListStatus.Text = "File not loaded."
                    lstDesktops.Items.Clear()
                    Exit Function
                End If
            Catch ex As Exception
                MsgBox("There was an error determining if the file you selected is the right file type.", MsgBoxStyle.Exclamation)
                lblListStatus.Visible = True
                lblListStatus.ForeColor = Color.Red
                lblListStatus.Text = "File not loaded."
                lstDesktops.Items.Clear()
                Exit Function
            End Try
            '##end check1

            '##check 2 - is the list the user selected empty?
            Try
                If lstDesktops.Items.Count = 0 Then
                    MsgBox("The list you specified is empty, please try again", MsgBoxStyle.Exclamation)
                    lblListStatus.Visible = True
                    lblListStatus.ForeColor = Color.Red
                    lblListStatus.Text = "File not loaded."
                    lstDesktops.Items.Clear()
                    Exit Function
                End If
            Catch ex As Exception
                MsgBox("There was an error determining if the file you selected is empty or not.", MsgBoxStyle.Exclamation)
                lblListStatus.Visible = True
                lblListStatus.ForeColor = Color.Red
                lblListStatus.Text = "File not loaded."
                lstDesktops.Items.Clear()
                Exit Function
            End Try
            '##end check 2

            '##processing2 - formatting csv entries properly
            Try
                If extension = ".csv" Then
                    Dim strPar As String = """"
                    'there is often a space at the end of a csv file, lets remove it
                    lstDesktops.Items.RemoveAt(lstDesktops.Items.Count - 1)
                    'if the names are in string format in the csv they will have quatations
                    'around them ie "PCNAME" instead of PCNAME
                    'lets loop through and remove all the unwanted quotes.
                    'if there are no quotes, no big deal, nothing is changed
                    For i As Integer = 0 To lstDesktops.Items.Count - 1
                        lstDesktops.Items(i) = Regex.Replace(lstDesktops.Items(i), strPar, "")
                    Next
                End If
            Catch ex As Exception
                MsgBox("The .csv file you selected could not be properly processed by ADPinger.", MsgBoxStyle.Exclamation)
                lblListStatus.Visible = True
                lblListStatus.ForeColor = Color.Red
                lblListStatus.Text = "File not loaded."
                lstDesktops.Items.Clear()
                Exit Function
            End Try
            '##end processing2

            '##check 3 - is the list formatted in a way we can process?
            Dim strhostName As String = ""
            Try
                For i As Integer = 0 To lstDesktops.Items.Count - 1
                    strhostName = lstDesktops.Items(i)
                    strhostName = strhostName.Trim
                    If strhostName <> "" Then

                    Else
                        'if the .txt file isn't formatted correctly, warn the user
                        MsgBox("Incorrectly formatted PCList")
                        lblListStatus.Visible = True
                        lblListStatus.ForeColor = Color.Red
                        lblListStatus.Text = "File not loaded."
                        lstDesktops.Items.Clear()
                        Exit Function
                    End If
                Next
            Catch ex As Exception
                MsgBox("There was an error determining if the file you selected is empty or not.", MsgBoxStyle.Exclamation)
                lblListStatus.Visible = True
                lblListStatus.ForeColor = Color.Red
                lblListStatus.Text = "File not loaded."
                lstDesktops.Items.Clear()
                Exit Function
            End Try
            '##end check 3

            '####AT this point the data is most likely OK - begin list processing####

            '##processing 3 - remove duplicates
            Try
                'remove dups from the list if any found
                removeDups()
            Catch ex As Exception
                MsgBox("ADPinger encountered an error while removing duplicates from your selected list.", MsgBoxStyle.Exclamation)
                lblListStatus.Visible = True
                lblListStatus.ForeColor = Color.Red
                lblListStatus.Text = "File not loaded."
                lstDesktops.Items.Clear()
                Exit Function
            End Try
            '##end processing 3

            '###########################################################
            'end file checks and processing

            'the user has successfully chosen to work with a list
            listLoad = True ' very important
            lblListStatus.Visible = True
            lblListStatus.ForeColor = Color.Green
            lblListStatus.Text = "File loaded." & vbNewLine & "Click Begin when ready."
            '############################################################
            'begin resize of form
            If Me.Width > 505 Then
                Do Until Me.Width = 505
                    Me.Width = Me.Width - 1

                Loop
            ElseIf Me.Width < 505 Then
                Do Until Me.Width = 505
                    Me.Width = Me.Width + 1
                Loop
            End If
            If Me.Height > 700 Then
                'Me.Cursor = Cursors.WaitCursor
                Do Until Me.Height = 700
                    Me.Height = Me.Height - 1
                Loop
                'Me.Cursor = Cursors.Arrow
            ElseIf Me.Height < 700 Then
                'Me.Cursor = Cursors.WaitCursor
                Do Until Me.Height = 700
                    Me.Height = Me.Height + 1
                Loop
                'Me.Cursor = Cursors.Arrow
            End If
            'end resize of form
            '############################################################
        Else
            'in this case the user elected not to proceed with the file load
            MsgBox("You did not correctly select a file", MsgBoxStyle.Information)
            lblListStatus.Visible = True
            lblListStatus.ForeColor = Color.Red
            lblListStatus.Text = "File not loaded."
            Exit Function
        End If

    End Function

    Function formatDirPath()
        'this function will check the validity of the users
        'input for the directory path.
        'if it checks out, we will build the string needed
        'to check the directory path on remote computers

        'first thing, lets load up what the user entered
        strDirScanPath = txtDirScan.Text.Trim

        'check to see if the basic structure of the directory path is good

        If strDirScanPath Like "*:\*" Then
            'if it is we will begin manipulating the string for remote PC use
            If Char.IsLetter(strDirScanPath(0)) Then
                Dim strDriveLetter As String = strDirScanPath(0)
                strDriveLetter = strDriveLetter + "$"
                Dim strAlteredFilePath As String = strDirScanPath.Remove(0, 2)
                strDirScanPath = strDriveLetter + strAlteredFilePath
                dirScanGood = True 'return good directory structure
            Else
                'the first character is not a drive letter. idiot user.
                MsgBox("It does not appear that you entered a valid directory path" & vbNewLine & "ex. C:\FilePath\", MsgBoxStyle.Exclamation)
                dirScanGood = False
                Return dirScanGood
                Exit Function
            End If
        Else
            'the directory path structure is not correct
            MsgBox("It does not appear that you entered a valid directory path" & vbNewLine & "ex. C:\FilePath\", MsgBoxStyle.Exclamation)
            dirScanGood = False
            Return dirScanGood
            Exit Function
        End If

        Return dirScanGood
    End Function

    Function formatCopy()
        'oy... complicated

        'first thing, lets load up what the user entered
        'notice everything will be in pairs...kind of.
        'handling both entries at the same time whenever possible
        strCopySource = txtCopySource.Text.Trim
        strCopyDestination = txtCopyDestination.Text.Trim

        'if the user checked create dir we will mark it as true
        If chkCreateDir.Checked = True Then
            createDirectory = True
        Else
            createDirectory = False
        End If

        'check to see if the basic structure of a file path is good for source
        'there are two possible ways this could be, drive letter, or whack-whack path
        If strCopySource Like "*:\*.*" Or strCopySource Like "*\\*.*" Then
            'now to check the basic structure of the file path for the desination
            If strCopyDestination Like "*:\*.*" Then
                'if all checks out we will check the drive letters
                'the source can have a letter or \
                If Char.IsLetter(strCopySource(0)) Or strCopySource(0) = "\" Then
                    'now check drive letter for destination
                    If Char.IsLetter(strCopyDestination(0)) Then
                        'now we need to manipulate the string of the destination
                        'the source string does not need to be manipulated
                        Dim strDriveLetterDestination As String = strCopyDestination(0)
                        strDriveLetterDestination = strDriveLetterDestination + "$"
                        Dim strAlteredDestinationPath As String = strCopyDestination.Remove(0, 2)
                        strCopyDestination = strDriveLetterDestination + strAlteredDestinationPath
                        'now to check if the source file actually exists.
                        'no action if it we can't find it!
                        If File.Exists(strCopySource) Then
                            'funny things can happen with relative paths that are specific to the OS
                            'lets warn the user
                            If strCopyDestination.ToUpper.Contains("C$\USERS") Then
                                Dim askRelativePath As DialogResult
                                askRelativePath = MessageBox.Show("Your destination contains C:\Users. If your list/OU contains XP devices, the copy function may not achieve the results you are looking for." & vbNewLine & "**It is recommended that you NOT check Create Dir? in this circumstance**" & vbNewLine & "Would you still like to proceed?", "OS Specific Path Detected", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                                If askRelativePath = DialogResult.No Then
                                    copyGood = False
                                    Return copyGood
                                    Exit Function
                                End If
                            ElseIf strCopyDestination.ToUpper.Contains("C$\DOCUMENTS") Then
                                Dim askRelativePath As DialogResult
                                askRelativePath = MessageBox.Show("Your destination contains C:\Documents. If your list/OU contains non-XP devices, the copy function may not achieve the results you are looking for." & vbNewLine & "**It is recommended that you NOT check Create Dir? in this circumstance**" & vbNewLine & "Would you still like to proceed?", "OS Specific Path Detected", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                                If askRelativePath = DialogResult.No Then
                                    copyGood = False
                                    Return copyGood
                                    Exit Function
                                End If
                            End If
                            strjustDirectory = System.IO.Path.GetDirectoryName(strCopyDestination)
                            copyGood = True 'return good file structure
                        Else
                            'the file the user specified could not be located
                            MsgBox("The source file you specified could not be found." & vbNewLine & "ex. C:\FilePath\Filename.txt", MsgBoxStyle.Exclamation)
                            copyGood = False
                            Return copyGood
                            Exit Function
                        End If
                    Else
                        'the first character is not a drive letter. idiot user.
                        MsgBox("It does not appear that you entered a valid destination file path" & vbNewLine & "ex. C:\FilePath\Filename.txt", MsgBoxStyle.Exclamation)
                        copyGood = False
                        Return copyGood
                        Exit Function
                    End If
                Else
                    'the first character is not a drive letter or \. idiot user.
                    MsgBox("It does not appear that you entered a valid source file path" & vbNewLine & "ex. C:\FilePath\Filename.txt", MsgBoxStyle.Exclamation)
                    copyGood = False
                    Return copyGood
                    Exit Function
                End If
            Else
                'the first character is not a drive letter. idiot user.
                MsgBox("It does not appear that you entered a valid destination file path" & vbNewLine & "ex. C:\FilePath\Filename.txt", MsgBoxStyle.Exclamation)
                copyGood = False
                Return copyGood
                Exit Function
            End If
        Else
            'the file path structure is not correct
            MsgBox("It does not appear that you entered a valid source file path" & vbNewLine & "ex. C:\FilePath\Filename.txt", MsgBoxStyle.Exclamation)
            copyGood = False
            Return copyGood
            Exit Function
        End If

        Return copyGood
    End Function

    Private Sub txtRebootTime_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtRebootTime.KeyPress
        'limits the reboot time to numbers only

        '97 - 122 = Ascii codes for simple letters
        '65 - 90  = Ascii codes for capital letters
        '48 - 57  = Ascii codes for numbers

        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        If cboAction.SelectedItem = "Ping" Then
            '############################################################
            'begin resize of form
            Me.Cursor = Cursors.WaitCursor
            If Me.Width > 390 Then
                'Me.Cursor = Cursors.WaitCursor
                Do Until Me.Width = 390
                    Me.Width = Me.Width - 1
                Loop
                'Me.Cursor = Cursors.Arrow
            ElseIf Me.Width < 390 Then
                'Me.Cursor = Cursors.WaitCursor
                Do Until Me.Width = 390
                    Me.Width = Me.Width + 1
                Loop
                'Me.Cursor = Cursors.Arrow
            End If

            If Me.Height > 215 Then
                'Me.Cursor = Cursors.WaitCursor
                Do Until Me.Height = 215
                    Me.Height = Me.Height - 1
                Loop
                'Me.Cursor = Cursors.Arrow
            ElseIf Me.Height < 215 Then
                'Me.Cursor = Cursors.WaitCursor
                Do Until Me.Height = 215
                    Me.Height = Me.Height + 1
                Loop
                'Me.Cursor = Cursors.Arrow
            End If
            Me.Cursor = Cursors.Arrow
            '############################################################
            resetEverything()
            resetSelections()
            lblListStatus.Visible = False
            pnlPing.Visible = True
        Else
            cboAction.SelectedItem = "Ping"
        End If

    End Sub

    Private Sub btnCopySource_Click(sender As Object, e As EventArgs) Handles btnCopySource.Click
        OpenFileDialog2.FileName = ""
        OpenFileDialog2.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
        If OpenFileDialog2.ShowDialog() = DialogResult.OK Then
            Dim copyFilePath As String 'variable for determining file type
            copyFilePath = Path.GetFullPath(OpenFileDialog2.FileName)
            txtCopySource.Text = copyFilePath
        Else
            'in this case the user elected not to proceed with the file load
            MsgBox("You did not correctly select a file", MsgBoxStyle.Information)
            Exit Sub
        End If
    End Sub

End Class