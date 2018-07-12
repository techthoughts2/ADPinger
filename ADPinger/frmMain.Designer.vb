<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series2 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.btnScan = New System.Windows.Forms.Button()
        Me.ADLoader = New System.ComponentModel.BackgroundWorker()
        Me.delay = New System.ComponentModel.BackgroundWorker()
        Me.lblThreads = New System.Windows.Forms.Label()
        Me.lstNonPingable = New System.Windows.Forms.ListBox()
        Me.lstPingable = New System.Windows.Forms.ListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.completionChecker = New System.ComponentModel.BackgroundWorker()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblActiveThreads = New System.Windows.Forms.Label()
        Me.treADDomain = New System.Windows.Forms.TreeView()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblGoodCount = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblTotalCount = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Chart1 = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lblPercentGood = New System.Windows.Forms.Label()
        Me.rbAD = New System.Windows.Forms.RadioButton()
        Me.rbList = New System.Windows.Forms.RadioButton()
        Me.lblListStatus = New System.Windows.Forms.Label()
        Me.lblADStatus = New System.Windows.Forms.Label()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.lstDesktops = New System.Windows.Forms.ListBox()
        Me.grpbAction = New System.Windows.Forms.GroupBox()
        Me.cboAction = New System.Windows.Forms.ComboBox()
        Me.grpbOptions = New System.Windows.Forms.GroupBox()
        Me.txtRebootTime = New System.Windows.Forms.TextBox()
        Me.cboPingTimeout = New System.Windows.Forms.ComboBox()
        Me.grpbList = New System.Windows.Forms.GroupBox()
        Me.chkForceReboot = New System.Windows.Forms.CheckBox()
        Me.pnlScanFile = New System.Windows.Forms.Panel()
        Me.txtFileScan = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.pnlReboot = New System.Windows.Forms.Panel()
        Me.lblCustomMessage = New System.Windows.Forms.Label()
        Me.lblTime = New System.Windows.Forms.Label()
        Me.txtShutdownMessage = New System.Windows.Forms.TextBox()
        Me.pnlPing = New System.Windows.Forms.Panel()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.btnReset = New System.Windows.Forms.Button()
        Me.pnlScanDir = New System.Windows.Forms.Panel()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtDirScan = New System.Windows.Forms.TextBox()
        Me.pnlCopy = New System.Windows.Forms.Panel()
        Me.btnCopySource = New System.Windows.Forms.Button()
        Me.chkCreateDir = New System.Windows.Forms.CheckBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtCopySource = New System.Windows.Forms.TextBox()
        Me.txtCopyDestination = New System.Windows.Forms.TextBox()
        Me.pnlServices = New System.Windows.Forms.Panel()
        Me.rdStart = New System.Windows.Forms.RadioButton()
        Me.rdStop = New System.Windows.Forms.RadioButton()
        Me.rdRestart = New System.Windows.Forms.RadioButton()
        Me.rdStatus = New System.Windows.Forms.RadioButton()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtServiceName = New System.Windows.Forms.TextBox()
        Me.OpenFileDialog2 = New System.Windows.Forms.OpenFileDialog()
        Me.MenuStrip1.SuspendLayout()
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpbAction.SuspendLayout()
        Me.grpbList.SuspendLayout()
        Me.pnlScanFile.SuspendLayout()
        Me.pnlReboot.SuspendLayout()
        Me.pnlPing.SuspendLayout()
        Me.pnlScanDir.SuspendLayout()
        Me.pnlCopy.SuspendLayout()
        Me.pnlServices.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnScan
        '
        Me.btnScan.Location = New System.Drawing.Point(12, 237)
        Me.btnScan.Name = "btnScan"
        Me.btnScan.Size = New System.Drawing.Size(137, 43)
        Me.btnScan.TabIndex = 9
        Me.btnScan.Text = "Begin"
        Me.btnScan.UseVisualStyleBackColor = True
        '
        'ADLoader
        '
        Me.ADLoader.WorkerReportsProgress = True
        Me.ADLoader.WorkerSupportsCancellation = True
        '
        'delay
        '
        Me.delay.WorkerSupportsCancellation = True
        '
        'lblThreads
        '
        Me.lblThreads.AutoSize = True
        Me.lblThreads.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblThreads.Location = New System.Drawing.Point(472, 234)
        Me.lblThreads.Name = "lblThreads"
        Me.lblThreads.Size = New System.Drawing.Size(16, 17)
        Me.lblThreads.TabIndex = 13
        Me.lblThreads.Text = "0"
        '
        'lstNonPingable
        '
        Me.lstNonPingable.FormattingEnabled = True
        Me.lstNonPingable.Location = New System.Drawing.Point(12, 321)
        Me.lstNonPingable.Name = "lstNonPingable"
        Me.lstNonPingable.Size = New System.Drawing.Size(310, 238)
        Me.lstNonPingable.TabIndex = 16
        '
        'lstPingable
        '
        Me.lstPingable.FormattingEnabled = True
        Me.lstPingable.Location = New System.Drawing.Point(328, 321)
        Me.lstPingable.Name = "lstPingable"
        Me.lstPingable.Size = New System.Drawing.Size(159, 238)
        Me.lstPingable.TabIndex = 17
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(520, 66)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(87, 13)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Select an OU:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(316, 237)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(120, 13)
        Me.Label2.TabIndex = 11
        Me.Label2.Text = "# of threads launched..."
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.Location = New System.Drawing.Point(181, 26)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(132, 24)
        Me.lblStatus.TabIndex = 6
        Me.lblStatus.Text = "Processing..."
        Me.lblStatus.Visible = False
        '
        'completionChecker
        '
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(316, 263)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(113, 13)
        Me.Label3.TabIndex = 12
        Me.Label3.Text = "# of threads working..."
        '
        'lblActiveThreads
        '
        Me.lblActiveThreads.AutoSize = True
        Me.lblActiveThreads.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblActiveThreads.Location = New System.Drawing.Point(472, 263)
        Me.lblActiveThreads.Name = "lblActiveThreads"
        Me.lblActiveThreads.Size = New System.Drawing.Size(16, 17)
        Me.lblActiveThreads.TabIndex = 14
        Me.lblActiveThreads.Text = "0"
        '
        'treADDomain
        '
        Me.treADDomain.Location = New System.Drawing.Point(523, 89)
        Me.treADDomain.Name = "treADDomain"
        Me.treADDomain.Size = New System.Drawing.Size(290, 564)
        Me.treADDomain.TabIndex = 8
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(12, 286)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(475, 29)
        Me.ProgressBar1.TabIndex = 15
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Dock = System.Windows.Forms.DockStyle.Right
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(1140, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(58, 678)
        Me.MenuStrip1.TabIndex = 26
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(45, 19)
        Me.AboutToolStripMenuItem.Text = "About"
        '
        'lblGoodCount
        '
        Me.lblGoodCount.AutoSize = True
        Me.lblGoodCount.Location = New System.Drawing.Point(449, 576)
        Me.lblGoodCount.Name = "lblGoodCount"
        Me.lblGoodCount.Size = New System.Drawing.Size(13, 13)
        Me.lblGoodCount.TabIndex = 24
        Me.lblGoodCount.Text = "0"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(378, 576)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(51, 13)
        Me.Label4.TabIndex = 23
        Me.Label4.Text = "Success:"
        '
        'lblTotalCount
        '
        Me.lblTotalCount.AutoSize = True
        Me.lblTotalCount.Location = New System.Drawing.Point(276, 576)
        Me.lblTotalCount.Name = "lblTotalCount"
        Me.lblTotalCount.Size = New System.Drawing.Size(13, 13)
        Me.lblTotalCount.TabIndex = 20
        Me.lblTotalCount.Text = "0"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(226, 576)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(34, 13)
        Me.Label5.TabIndex = 19
        Me.Label5.Text = "Total:"
        '
        'Chart1
        '
        Me.Chart1.BackColor = System.Drawing.SystemColors.Control
        Me.Chart1.BackSecondaryColor = System.Drawing.SystemColors.Control
        Me.Chart1.BorderlineColor = System.Drawing.SystemColors.Control
        ChartArea1.Name = "ChartArea1"
        Me.Chart1.ChartAreas.Add(ChartArea1)
        Legend1.Name = "Legend1"
        Me.Chart1.Legends.Add(Legend1)
        Me.Chart1.Location = New System.Drawing.Point(3, 596)
        Me.Chart1.Name = "Chart1"
        Series1.ChartArea = "ChartArea1"
        Series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedBar100
        Series1.Color = System.Drawing.Color.Green
        Series1.IsVisibleInLegend = False
        Series1.Legend = "Legend1"
        Series1.Name = "Total"
        Series2.ChartArea = "ChartArea1"
        Series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedBar100
        Series2.Color = System.Drawing.SystemColors.ControlDark
        Series2.IsVisibleInLegend = False
        Series2.Legend = "Legend1"
        Series2.Name = "Good"
        Me.Chart1.Series.Add(Series1)
        Me.Chart1.Series.Add(Series2)
        Me.Chart1.Size = New System.Drawing.Size(487, 47)
        Me.Chart1.TabIndex = 18
        Me.Chart1.Text = "Chart1"
        Me.Chart1.Visible = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(213, 636)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(33, 13)
        Me.Label6.TabIndex = 21
        Me.Label6.Text = "Alive:"
        Me.Label6.Visible = False
        '
        'lblPercentGood
        '
        Me.lblPercentGood.AutoSize = True
        Me.lblPercentGood.Location = New System.Drawing.Point(254, 636)
        Me.lblPercentGood.Name = "lblPercentGood"
        Me.lblPercentGood.Size = New System.Drawing.Size(13, 13)
        Me.lblPercentGood.TabIndex = 22
        Me.lblPercentGood.Text = "0"
        Me.lblPercentGood.Visible = False
        '
        'rbAD
        '
        Me.rbAD.AutoSize = True
        Me.rbAD.Enabled = False
        Me.rbAD.Location = New System.Drawing.Point(6, 21)
        Me.rbAD.Name = "rbAD"
        Me.rbAD.Size = New System.Drawing.Size(90, 17)
        Me.rbAD.TabIndex = 0
        Me.rbAD.TabStop = True
        Me.rbAD.Text = "Load from AD"
        Me.rbAD.UseVisualStyleBackColor = True
        '
        'rbList
        '
        Me.rbList.AutoSize = True
        Me.rbList.Location = New System.Drawing.Point(6, 61)
        Me.rbList.Name = "rbList"
        Me.rbList.Size = New System.Drawing.Size(91, 17)
        Me.rbList.TabIndex = 2
        Me.rbList.TabStop = True
        Me.rbList.Text = "Load from File"
        Me.rbList.UseVisualStyleBackColor = True
        '
        'lblListStatus
        '
        Me.lblListStatus.AutoSize = True
        Me.lblListStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblListStatus.Location = New System.Drawing.Point(33, 85)
        Me.lblListStatus.Name = "lblListStatus"
        Me.lblListStatus.Size = New System.Drawing.Size(40, 12)
        Me.lblListStatus.TabIndex = 3
        Me.lblListStatus.Text = "ListLoad"
        Me.lblListStatus.Visible = False
        '
        'lblADStatus
        '
        Me.lblADStatus.AutoSize = True
        Me.lblADStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblADStatus.Location = New System.Drawing.Point(33, 45)
        Me.lblADStatus.Name = "lblADStatus"
        Me.lblADStatus.Size = New System.Drawing.Size(87, 12)
        Me.lblADStatus.TabIndex = 1
        Me.lblADStatus.Text = "No domain detected"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'lstDesktops
        '
        Me.lstDesktops.FormattingEnabled = True
        Me.lstDesktops.Location = New System.Drawing.Point(381, 644)
        Me.lstDesktops.Name = "lstDesktops"
        Me.lstDesktops.Size = New System.Drawing.Size(113, 17)
        Me.lstDesktops.TabIndex = 25
        Me.lstDesktops.Visible = False
        '
        'grpbAction
        '
        Me.grpbAction.Controls.Add(Me.cboAction)
        Me.grpbAction.Location = New System.Drawing.Point(12, 12)
        Me.grpbAction.Name = "grpbAction"
        Me.grpbAction.Size = New System.Drawing.Size(163, 71)
        Me.grpbAction.TabIndex = 0
        Me.grpbAction.TabStop = False
        Me.grpbAction.Text = "1. Select an Action"
        '
        'cboAction
        '
        Me.cboAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAction.FormattingEnabled = True
        Me.cboAction.Items.AddRange(New Object() {"Ping", "Reboot", "Shutdown", "Scan for file", "Scan for directory", "Copy File", "Delete File", "Services"})
        Me.cboAction.Location = New System.Drawing.Point(6, 21)
        Me.cboAction.Name = "cboAction"
        Me.cboAction.Size = New System.Drawing.Size(150, 21)
        Me.cboAction.TabIndex = 0
        '
        'grpbOptions
        '
        Me.grpbOptions.Location = New System.Drawing.Point(12, 89)
        Me.grpbOptions.Name = "grpbOptions"
        Me.grpbOptions.Size = New System.Drawing.Size(295, 120)
        Me.grpbOptions.TabIndex = 1
        Me.grpbOptions.TabStop = False
        Me.grpbOptions.Text = "2. Options"
        '
        'txtRebootTime
        '
        Me.txtRebootTime.Location = New System.Drawing.Point(87, 66)
        Me.txtRebootTime.Name = "txtRebootTime"
        Me.txtRebootTime.Size = New System.Drawing.Size(54, 20)
        Me.txtRebootTime.TabIndex = 3
        Me.txtRebootTime.Text = "15"
        '
        'cboPingTimeout
        '
        Me.cboPingTimeout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPingTimeout.FormattingEnabled = True
        Me.cboPingTimeout.Items.AddRange(New Object() {"500", "1000", "2000", "5000"})
        Me.cboPingTimeout.Location = New System.Drawing.Point(78, 53)
        Me.cboPingTimeout.Name = "cboPingTimeout"
        Me.cboPingTimeout.Size = New System.Drawing.Size(121, 21)
        Me.cboPingTimeout.TabIndex = 1
        '
        'grpbList
        '
        Me.grpbList.Controls.Add(Me.rbAD)
        Me.grpbList.Controls.Add(Me.rbList)
        Me.grpbList.Controls.Add(Me.lblListStatus)
        Me.grpbList.Controls.Add(Me.lblADStatus)
        Me.grpbList.Location = New System.Drawing.Point(313, 89)
        Me.grpbList.Name = "grpbList"
        Me.grpbList.Size = New System.Drawing.Size(174, 120)
        Me.grpbList.TabIndex = 6
        Me.grpbList.TabStop = False
        Me.grpbList.Text = "3. List Location"
        '
        'chkForceReboot
        '
        Me.chkForceReboot.AutoSize = True
        Me.chkForceReboot.Location = New System.Drawing.Point(167, 68)
        Me.chkForceReboot.Name = "chkForceReboot"
        Me.chkForceReboot.Size = New System.Drawing.Size(91, 17)
        Me.chkForceReboot.TabIndex = 4
        Me.chkForceReboot.Text = "Force Reboot"
        Me.chkForceReboot.UseVisualStyleBackColor = True
        '
        'pnlScanFile
        '
        Me.pnlScanFile.Controls.Add(Me.txtFileScan)
        Me.pnlScanFile.Controls.Add(Me.Label10)
        Me.pnlScanFile.Location = New System.Drawing.Point(882, 236)
        Me.pnlScanFile.Name = "pnlScanFile"
        Me.pnlScanFile.Size = New System.Drawing.Size(283, 93)
        Me.pnlScanFile.TabIndex = 4
        Me.pnlScanFile.Visible = False
        '
        'txtFileScan
        '
        Me.txtFileScan.Location = New System.Drawing.Point(3, 57)
        Me.txtFileScan.Name = "txtFileScan"
        Me.txtFileScan.Size = New System.Drawing.Size(277, 20)
        Me.txtFileScan.TabIndex = 1
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(3, 21)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(213, 24)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "Enter full file path for the file:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(ex: C:\Program Files\Paint.NET\PaintDotNet.e" & _
    "xe)"
        '
        'pnlReboot
        '
        Me.pnlReboot.Controls.Add(Me.lblCustomMessage)
        Me.pnlReboot.Controls.Add(Me.lblTime)
        Me.pnlReboot.Controls.Add(Me.txtShutdownMessage)
        Me.pnlReboot.Controls.Add(Me.chkForceReboot)
        Me.pnlReboot.Controls.Add(Me.txtRebootTime)
        Me.pnlReboot.Location = New System.Drawing.Point(882, 137)
        Me.pnlReboot.Name = "pnlReboot"
        Me.pnlReboot.Size = New System.Drawing.Size(283, 93)
        Me.pnlReboot.TabIndex = 3
        Me.pnlReboot.Visible = False
        '
        'lblCustomMessage
        '
        Me.lblCustomMessage.AutoSize = True
        Me.lblCustomMessage.Location = New System.Drawing.Point(0, 13)
        Me.lblCustomMessage.Name = "lblCustomMessage"
        Me.lblCustomMessage.Size = New System.Drawing.Size(152, 13)
        Me.lblCustomMessage.TabIndex = 0
        Me.lblCustomMessage.Text = "Shutdown Message (Optional):"
        '
        'lblTime
        '
        Me.lblTime.AutoSize = True
        Me.lblTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTime.Location = New System.Drawing.Point(0, 69)
        Me.lblTime.Name = "lblTime"
        Me.lblTime.Size = New System.Drawing.Size(82, 13)
        Me.lblTime.TabIndex = 2
        Me.lblTime.Text = "Time (seconds):"
        '
        'txtShutdownMessage
        '
        Me.txtShutdownMessage.Location = New System.Drawing.Point(3, 33)
        Me.txtShutdownMessage.Name = "txtShutdownMessage"
        Me.txtShutdownMessage.Size = New System.Drawing.Size(277, 20)
        Me.txtShutdownMessage.TabIndex = 1
        '
        'pnlPing
        '
        Me.pnlPing.Controls.Add(Me.Label7)
        Me.pnlPing.Controls.Add(Me.cboPingTimeout)
        Me.pnlPing.Location = New System.Drawing.Point(882, 38)
        Me.pnlPing.Name = "pnlPing"
        Me.pnlPing.Size = New System.Drawing.Size(283, 93)
        Me.pnlPing.TabIndex = 2
        Me.pnlPing.Visible = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(3, 11)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(198, 26)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Adjust the time-out (in milliseconds)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "1000 is recommended for most situations"
        '
        'btnReset
        '
        Me.btnReset.Location = New System.Drawing.Point(155, 237)
        Me.btnReset.Name = "btnReset"
        Me.btnReset.Size = New System.Drawing.Size(75, 42)
        Me.btnReset.TabIndex = 10
        Me.btnReset.Text = "Reset"
        Me.btnReset.UseVisualStyleBackColor = True
        '
        'pnlScanDir
        '
        Me.pnlScanDir.Controls.Add(Me.Label11)
        Me.pnlScanDir.Controls.Add(Me.txtDirScan)
        Me.pnlScanDir.Location = New System.Drawing.Point(882, 335)
        Me.pnlScanDir.Name = "pnlScanDir"
        Me.pnlScanDir.Size = New System.Drawing.Size(283, 93)
        Me.pnlScanDir.TabIndex = 27
        Me.pnlScanDir.Visible = False
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(3, 16)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(141, 36)
        Me.Label11.TabIndex = 2
        Me.Label11.Text = "Enter full file path for the directory" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "you would like to search for." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(ex: C:\Pr" & _
    "ogram Files\Paint.NET)"
        '
        'txtDirScan
        '
        Me.txtDirScan.Location = New System.Drawing.Point(3, 68)
        Me.txtDirScan.Name = "txtDirScan"
        Me.txtDirScan.Size = New System.Drawing.Size(277, 20)
        Me.txtDirScan.TabIndex = 0
        '
        'pnlCopy
        '
        Me.pnlCopy.Controls.Add(Me.btnCopySource)
        Me.pnlCopy.Controls.Add(Me.chkCreateDir)
        Me.pnlCopy.Controls.Add(Me.Label13)
        Me.pnlCopy.Controls.Add(Me.Label12)
        Me.pnlCopy.Controls.Add(Me.txtCopySource)
        Me.pnlCopy.Controls.Add(Me.txtCopyDestination)
        Me.pnlCopy.Location = New System.Drawing.Point(882, 434)
        Me.pnlCopy.Name = "pnlCopy"
        Me.pnlCopy.Size = New System.Drawing.Size(283, 93)
        Me.pnlCopy.TabIndex = 28
        Me.pnlCopy.Visible = False
        '
        'btnCopySource
        '
        Me.btnCopySource.Location = New System.Drawing.Point(251, 20)
        Me.btnCopySource.Name = "btnCopySource"
        Me.btnCopySource.Size = New System.Drawing.Size(29, 23)
        Me.btnCopySource.TabIndex = 30
        Me.btnCopySource.Text = "..."
        Me.btnCopySource.UseVisualStyleBackColor = True
        '
        'chkCreateDir
        '
        Me.chkCreateDir.AutoSize = True
        Me.chkCreateDir.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCreateDir.Location = New System.Drawing.Point(179, 49)
        Me.chkCreateDir.Name = "chkCreateDir"
        Me.chkCreateDir.Size = New System.Drawing.Size(79, 17)
        Me.chkCreateDir.TabIndex = 7
        Me.chkCreateDir.Text = "Create Dir?"
        Me.chkCreateDir.UseVisualStyleBackColor = True
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(3, 48)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(88, 13)
        Me.Label13.TabIndex = 6
        Me.Label13.Text = "Destination Path:"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(3, 3)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(68, 13)
        Me.Label12.TabIndex = 5
        Me.Label12.Text = "Source Path:"
        '
        'txtCopySource
        '
        Me.txtCopySource.Location = New System.Drawing.Point(3, 21)
        Me.txtCopySource.Name = "txtCopySource"
        Me.txtCopySource.Size = New System.Drawing.Size(245, 20)
        Me.txtCopySource.TabIndex = 4
        '
        'txtCopyDestination
        '
        Me.txtCopyDestination.Location = New System.Drawing.Point(3, 68)
        Me.txtCopyDestination.Name = "txtCopyDestination"
        Me.txtCopyDestination.Size = New System.Drawing.Size(277, 20)
        Me.txtCopyDestination.TabIndex = 3
        '
        'pnlServices
        '
        Me.pnlServices.Controls.Add(Me.rdStart)
        Me.pnlServices.Controls.Add(Me.rdStop)
        Me.pnlServices.Controls.Add(Me.rdRestart)
        Me.pnlServices.Controls.Add(Me.rdStatus)
        Me.pnlServices.Controls.Add(Me.Label8)
        Me.pnlServices.Controls.Add(Me.txtServiceName)
        Me.pnlServices.Location = New System.Drawing.Point(882, 533)
        Me.pnlServices.Name = "pnlServices"
        Me.pnlServices.Size = New System.Drawing.Size(280, 93)
        Me.pnlServices.TabIndex = 29
        '
        'rdStart
        '
        Me.rdStart.AutoSize = True
        Me.rdStart.Location = New System.Drawing.Point(220, 63)
        Me.rdStart.Name = "rdStart"
        Me.rdStart.Size = New System.Drawing.Size(47, 17)
        Me.rdStart.TabIndex = 35
        Me.rdStart.TabStop = True
        Me.rdStart.Text = "Start"
        Me.rdStart.UseVisualStyleBackColor = True
        '
        'rdStop
        '
        Me.rdStop.AutoSize = True
        Me.rdStop.Location = New System.Drawing.Point(152, 63)
        Me.rdStop.Name = "rdStop"
        Me.rdStop.Size = New System.Drawing.Size(47, 17)
        Me.rdStop.TabIndex = 34
        Me.rdStop.TabStop = True
        Me.rdStop.Text = "Stop"
        Me.rdStop.UseVisualStyleBackColor = True
        '
        'rdRestart
        '
        Me.rdRestart.AutoSize = True
        Me.rdRestart.Location = New System.Drawing.Point(78, 63)
        Me.rdRestart.Name = "rdRestart"
        Me.rdRestart.Size = New System.Drawing.Size(59, 17)
        Me.rdRestart.TabIndex = 33
        Me.rdRestart.TabStop = True
        Me.rdRestart.Text = "Restart"
        Me.rdRestart.UseVisualStyleBackColor = True
        '
        'rdStatus
        '
        Me.rdStatus.AutoSize = True
        Me.rdStatus.Location = New System.Drawing.Point(6, 63)
        Me.rdStatus.Name = "rdStatus"
        Me.rdStatus.Size = New System.Drawing.Size(55, 17)
        Me.rdStatus.TabIndex = 32
        Me.rdStatus.TabStop = True
        Me.rdStatus.Text = "Status"
        Me.rdStatus.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(3, 10)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(112, 13)
        Me.Label8.TabIndex = 31
        Me.Label8.Text = "Service Display Name:"
        '
        'txtServiceName
        '
        Me.txtServiceName.Location = New System.Drawing.Point(3, 26)
        Me.txtServiceName.Name = "txtServiceName"
        Me.txtServiceName.Size = New System.Drawing.Size(277, 20)
        Me.txtServiceName.TabIndex = 31
        '
        'OpenFileDialog2
        '
        Me.OpenFileDialog2.FileName = "OpenFileDialog2"
        '
        'Form1
        '
        Me.AcceptButton = Me.btnScan
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(1198, 678)
        Me.Controls.Add(Me.pnlServices)
        Me.Controls.Add(Me.pnlCopy)
        Me.Controls.Add(Me.pnlScanDir)
        Me.Controls.Add(Me.btnReset)
        Me.Controls.Add(Me.pnlPing)
        Me.Controls.Add(Me.pnlReboot)
        Me.Controls.Add(Me.pnlScanFile)
        Me.Controls.Add(Me.grpbList)
        Me.Controls.Add(Me.grpbOptions)
        Me.Controls.Add(Me.grpbAction)
        Me.Controls.Add(Me.lstDesktops)
        Me.Controls.Add(Me.lblPercentGood)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Chart1)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.lblTotalCount)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lblGoodCount)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.treADDomain)
        Me.Controls.Add(Me.lblActiveThreads)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lstPingable)
        Me.Controls.Add(Me.lstNonPingable)
        Me.Controls.Add(Me.lblThreads)
        Me.Controls.Add(Me.btnScan)
        Me.Controls.Add(Me.MenuStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.Text = "ADPinger"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpbAction.ResumeLayout(False)
        Me.grpbList.ResumeLayout(False)
        Me.grpbList.PerformLayout()
        Me.pnlScanFile.ResumeLayout(False)
        Me.pnlScanFile.PerformLayout()
        Me.pnlReboot.ResumeLayout(False)
        Me.pnlReboot.PerformLayout()
        Me.pnlPing.ResumeLayout(False)
        Me.pnlPing.PerformLayout()
        Me.pnlScanDir.ResumeLayout(False)
        Me.pnlScanDir.PerformLayout()
        Me.pnlCopy.ResumeLayout(False)
        Me.pnlCopy.PerformLayout()
        Me.pnlServices.ResumeLayout(False)
        Me.pnlServices.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnScan As System.Windows.Forms.Button
    Friend WithEvents ADLoader As System.ComponentModel.BackgroundWorker
    Friend WithEvents delay As System.ComponentModel.BackgroundWorker
    Friend WithEvents lblThreads As System.Windows.Forms.Label
    Friend WithEvents lstNonPingable As System.Windows.Forms.ListBox
    Friend WithEvents lstPingable As System.Windows.Forms.ListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents completionChecker As System.ComponentModel.BackgroundWorker
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblActiveThreads As System.Windows.Forms.Label
    Friend WithEvents treADDomain As System.Windows.Forms.TreeView
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblGoodCount As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblTotalCount As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Chart1 As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lblPercentGood As System.Windows.Forms.Label
    Friend WithEvents rbAD As System.Windows.Forms.RadioButton
    Friend WithEvents rbList As System.Windows.Forms.RadioButton
    Friend WithEvents lblListStatus As System.Windows.Forms.Label
    Friend WithEvents lblADStatus As System.Windows.Forms.Label
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents lstDesktops As System.Windows.Forms.ListBox
    Friend WithEvents grpbAction As System.Windows.Forms.GroupBox
    Friend WithEvents cboAction As System.Windows.Forms.ComboBox
    Friend WithEvents grpbOptions As System.Windows.Forms.GroupBox
    Friend WithEvents grpbList As System.Windows.Forms.GroupBox
    Friend WithEvents cboPingTimeout As System.Windows.Forms.ComboBox
    Friend WithEvents txtRebootTime As System.Windows.Forms.TextBox
    Friend WithEvents chkForceReboot As System.Windows.Forms.CheckBox
    Friend WithEvents pnlScanFile As System.Windows.Forms.Panel
    Friend WithEvents pnlReboot As System.Windows.Forms.Panel
    Friend WithEvents pnlPing As System.Windows.Forms.Panel
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtShutdownMessage As System.Windows.Forms.TextBox
    Friend WithEvents lblCustomMessage As System.Windows.Forms.Label
    Friend WithEvents lblTime As System.Windows.Forms.Label
    Friend WithEvents txtFileScan As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents btnReset As System.Windows.Forms.Button
    Friend WithEvents pnlScanDir As System.Windows.Forms.Panel
    Friend WithEvents pnlCopy As System.Windows.Forms.Panel
    Friend WithEvents txtDirScan As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtCopyDestination As System.Windows.Forms.TextBox
    Friend WithEvents txtCopySource As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents pnlServices As System.Windows.Forms.Panel
    Friend WithEvents chkCreateDir As System.Windows.Forms.CheckBox
    Friend WithEvents btnCopySource As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog2 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtServiceName As System.Windows.Forms.TextBox
    Friend WithEvents rdStart As System.Windows.Forms.RadioButton
    Friend WithEvents rdStop As System.Windows.Forms.RadioButton
    Friend WithEvents rdRestart As System.Windows.Forms.RadioButton
    Friend WithEvents rdStatus As System.Windows.Forms.RadioButton

End Class
