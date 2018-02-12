<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ReRunFull
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.lblStep = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.cbBatch = New System.Windows.Forms.CheckBox()
        Me.cbMoveToProdcution = New System.Windows.Forms.CheckBox()
        Me.cbEmailReport = New System.Windows.Forms.CheckBox()
        Me.cbJobTicket = New System.Windows.Forms.CheckBox()
        Me.cbMerge = New System.Windows.Forms.CheckBox()
        Me.cbWCpdfs = New System.Windows.Forms.CheckBox()
        Me.CBNonWcPDFS = New System.Windows.Forms.CheckBox()
        Me.CBSort = New System.Windows.Forms.CheckBox()
        Me.CBImport = New System.Windows.Forms.CheckBox()
        Me.btnProcess = New System.Windows.Forms.Button()
        Me.dvBatches = New System.Windows.Forms.DataGridView()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.lblStepCal = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnProcessCalanderReRunCancel = New System.Windows.Forms.Button()
        Me.cbCALENDARnewBatch = New System.Windows.Forms.CheckBox()
        Me.cbCALENDARmoveToProduction = New System.Windows.Forms.CheckBox()
        Me.cbCALENDARemailReport = New System.Windows.Forms.CheckBox()
        Me.cbCALENDARjobticket = New System.Windows.Forms.CheckBox()
        Me.cbCALENDARmerge = New System.Windows.Forms.CheckBox()
        Me.cbCALENDARmakeWCpdfs = New System.Windows.Forms.CheckBox()
        Me.cbCALENDARmakeNonWCpdfs = New System.Windows.Forms.CheckBox()
        Me.cbCALENDARsort = New System.Windows.Forms.CheckBox()
        Me.cbCALENDARimport = New System.Windows.Forms.CheckBox()
        Me.btnProcessCalanderReRun = New System.Windows.Forms.Button()
        Me.rerunCalendar = New System.Windows.Forms.MonthCalendar()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.dvBatches, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(997, 641)
        Me.TabControl1.TabIndex = 10
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.lblStep)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Controls.Add(Me.btnCancel)
        Me.TabPage1.Controls.Add(Me.cbBatch)
        Me.TabPage1.Controls.Add(Me.cbMoveToProdcution)
        Me.TabPage1.Controls.Add(Me.cbEmailReport)
        Me.TabPage1.Controls.Add(Me.cbJobTicket)
        Me.TabPage1.Controls.Add(Me.cbMerge)
        Me.TabPage1.Controls.Add(Me.cbWCpdfs)
        Me.TabPage1.Controls.Add(Me.CBNonWcPDFS)
        Me.TabPage1.Controls.Add(Me.CBSort)
        Me.TabPage1.Controls.Add(Me.CBImport)
        Me.TabPage1.Controls.Add(Me.btnProcess)
        Me.TabPage1.Controls.Add(Me.dvBatches)
        Me.TabPage1.Location = New System.Drawing.Point(4, 25)
        Me.TabPage1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage1.Size = New System.Drawing.Size(989, 612)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "GridView"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'lblStep
        '
        Me.lblStep.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblStep.AutoSize = True
        Me.lblStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep.Location = New System.Drawing.Point(432, 542)
        Me.lblStep.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblStep.Name = "lblStep"
        Me.lblStep.Size = New System.Drawing.Size(53, 25)
        Me.lblStep.TabIndex = 23
        Me.lblStep.Text = "Step"
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(361, 542)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(59, 25)
        Me.Label1.TabIndex = 22
        Me.Label1.Text = "Step:"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Enabled = False
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(815, 452)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(163, 50)
        Me.btnCancel.TabIndex = 21
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'cbBatch
        '
        Me.cbBatch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cbBatch.AutoSize = True
        Me.cbBatch.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbBatch.Location = New System.Drawing.Point(13, 399)
        Me.cbBatch.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cbBatch.Name = "cbBatch"
        Me.cbBatch.Size = New System.Drawing.Size(271, 24)
        Me.cbBatch.TabIndex = 20
        Me.cbBatch.Text = "Make New Batch-Runs and Sort"
        Me.cbBatch.UseVisualStyleBackColor = True
        '
        'cbMoveToProdcution
        '
        Me.cbMoveToProdcution.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cbMoveToProdcution.AutoSize = True
        Me.cbMoveToProdcution.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbMoveToProdcution.Location = New System.Drawing.Point(365, 427)
        Me.cbMoveToProdcution.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cbMoveToProdcution.Name = "cbMoveToProdcution"
        Me.cbMoveToProdcution.Size = New System.Drawing.Size(180, 24)
        Me.cbMoveToProdcution.TabIndex = 19
        Me.cbMoveToProdcution.Text = "Move To Production"
        Me.cbMoveToProdcution.UseVisualStyleBackColor = True
        '
        'cbEmailReport
        '
        Me.cbEmailReport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cbEmailReport.AutoSize = True
        Me.cbEmailReport.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbEmailReport.Location = New System.Drawing.Point(365, 399)
        Me.cbEmailReport.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cbEmailReport.Name = "cbEmailReport"
        Me.cbEmailReport.Size = New System.Drawing.Size(128, 24)
        Me.cbEmailReport.TabIndex = 18
        Me.cbEmailReport.Text = "Email Report"
        Me.cbEmailReport.UseVisualStyleBackColor = True
        '
        'cbJobTicket
        '
        Me.cbJobTicket.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cbJobTicket.AutoSize = True
        Me.cbJobTicket.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbJobTicket.Location = New System.Drawing.Point(365, 373)
        Me.cbJobTicket.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cbJobTicket.Name = "cbJobTicket"
        Me.cbJobTicket.Size = New System.Drawing.Size(108, 24)
        Me.cbJobTicket.TabIndex = 17
        Me.cbJobTicket.Text = "Job Ticket"
        Me.cbJobTicket.UseVisualStyleBackColor = True
        '
        'cbMerge
        '
        Me.cbMerge.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cbMerge.AutoSize = True
        Me.cbMerge.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbMerge.Location = New System.Drawing.Point(13, 507)
        Me.cbMerge.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cbMerge.Name = "cbMerge"
        Me.cbMerge.Size = New System.Drawing.Size(78, 24)
        Me.cbMerge.TabIndex = 16
        Me.cbMerge.Text = "Merge"
        Me.cbMerge.UseVisualStyleBackColor = True
        '
        'cbWCpdfs
        '
        Me.cbWCpdfs.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cbWCpdfs.AutoSize = True
        Me.cbWCpdfs.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbWCpdfs.Location = New System.Drawing.Point(13, 480)
        Me.cbWCpdfs.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cbWCpdfs.Name = "cbWCpdfs"
        Me.cbWCpdfs.Size = New System.Drawing.Size(152, 24)
        Me.cbWCpdfs.TabIndex = 15
        Me.cbWCpdfs.Text = "Make WC PDFs"
        Me.cbWCpdfs.UseVisualStyleBackColor = True
        '
        'CBNonWcPDFS
        '
        Me.CBNonWcPDFS.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CBNonWcPDFS.AutoSize = True
        Me.CBNonWcPDFS.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CBNonWcPDFS.Location = New System.Drawing.Point(13, 453)
        Me.CBNonWcPDFS.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.CBNonWcPDFS.Name = "CBNonWcPDFS"
        Me.CBNonWcPDFS.Size = New System.Drawing.Size(187, 24)
        Me.CBNonWcPDFS.TabIndex = 14
        Me.CBNonWcPDFS.Text = "Make Non WC PDFs"
        Me.CBNonWcPDFS.UseVisualStyleBackColor = True
        '
        'CBSort
        '
        Me.CBSort.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CBSort.AutoSize = True
        Me.CBSort.Enabled = False
        Me.CBSort.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CBSort.Location = New System.Drawing.Point(13, 426)
        Me.CBSort.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.CBSort.Name = "CBSort"
        Me.CBSort.Size = New System.Drawing.Size(62, 24)
        Me.CBSort.TabIndex = 13
        Me.CBSort.Text = "Sort"
        Me.CBSort.UseVisualStyleBackColor = True
        '
        'CBImport
        '
        Me.CBImport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CBImport.AutoSize = True
        Me.CBImport.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CBImport.Location = New System.Drawing.Point(13, 372)
        Me.CBImport.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.CBImport.Name = "CBImport"
        Me.CBImport.Size = New System.Drawing.Size(78, 24)
        Me.CBImport.TabIndex = 12
        Me.CBImport.Text = "Import"
        Me.CBImport.UseVisualStyleBackColor = True
        '
        'btnProcess
        '
        Me.btnProcess.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnProcess.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnProcess.Location = New System.Drawing.Point(815, 370)
        Me.btnProcess.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnProcess.Name = "btnProcess"
        Me.btnProcess.Size = New System.Drawing.Size(163, 50)
        Me.btnProcess.TabIndex = 11
        Me.btnProcess.Text = "Go!"
        Me.btnProcess.UseVisualStyleBackColor = True
        '
        'dvBatches
        '
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dvBatches.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.dvBatches.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dvBatches.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.dvBatches.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dvBatches.DefaultCellStyle = DataGridViewCellStyle3
        Me.dvBatches.Location = New System.Drawing.Point(13, 7)
        Me.dvBatches.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.dvBatches.Name = "dvBatches"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dvBatches.RowHeadersDefaultCellStyle = DataGridViewCellStyle4
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dvBatches.RowsDefaultCellStyle = DataGridViewCellStyle5
        Me.dvBatches.RowTemplate.Height = 24
        Me.dvBatches.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dvBatches.Size = New System.Drawing.Size(964, 348)
        Me.dvBatches.TabIndex = 10
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.lblStepCal)
        Me.TabPage2.Controls.Add(Me.Label2)
        Me.TabPage2.Controls.Add(Me.btnProcessCalanderReRunCancel)
        Me.TabPage2.Controls.Add(Me.cbCALENDARnewBatch)
        Me.TabPage2.Controls.Add(Me.cbCALENDARmoveToProduction)
        Me.TabPage2.Controls.Add(Me.cbCALENDARemailReport)
        Me.TabPage2.Controls.Add(Me.cbCALENDARjobticket)
        Me.TabPage2.Controls.Add(Me.cbCALENDARmerge)
        Me.TabPage2.Controls.Add(Me.cbCALENDARmakeWCpdfs)
        Me.TabPage2.Controls.Add(Me.cbCALENDARmakeNonWCpdfs)
        Me.TabPage2.Controls.Add(Me.cbCALENDARsort)
        Me.TabPage2.Controls.Add(Me.cbCALENDARimport)
        Me.TabPage2.Controls.Add(Me.btnProcessCalanderReRun)
        Me.TabPage2.Controls.Add(Me.rerunCalendar)
        Me.TabPage2.Location = New System.Drawing.Point(4, 25)
        Me.TabPage2.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage2.Size = New System.Drawing.Size(989, 612)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Calander"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'lblStepCal
        '
        Me.lblStepCal.AutoSize = True
        Me.lblStepCal.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStepCal.Location = New System.Drawing.Point(385, 471)
        Me.lblStepCal.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblStepCal.Name = "lblStepCal"
        Me.lblStepCal.Size = New System.Drawing.Size(0, 25)
        Me.lblStepCal.TabIndex = 33
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(315, 471)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(59, 25)
        Me.Label2.TabIndex = 32
        Me.Label2.Text = "Step:"
        '
        'btnProcessCalanderReRunCancel
        '
        Me.btnProcessCalanderReRunCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnProcessCalanderReRunCancel.Location = New System.Drawing.Point(28, 348)
        Me.btnProcessCalanderReRunCancel.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnProcessCalanderReRunCancel.Name = "btnProcessCalanderReRunCancel"
        Me.btnProcessCalanderReRunCancel.Size = New System.Drawing.Size(303, 50)
        Me.btnProcessCalanderReRunCancel.TabIndex = 31
        Me.btnProcessCalanderReRunCancel.Text = "Cancel"
        Me.btnProcessCalanderReRunCancel.UseVisualStyleBackColor = True
        '
        'cbCALENDARnewBatch
        '
        Me.cbCALENDARnewBatch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbCALENDARnewBatch.AutoSize = True
        Me.cbCALENDARnewBatch.Location = New System.Drawing.Point(391, 54)
        Me.cbCALENDARnewBatch.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cbCALENDARnewBatch.Name = "cbCALENDARnewBatch"
        Me.cbCALENDARnewBatch.Size = New System.Drawing.Size(231, 21)
        Me.cbCALENDARnewBatch.TabIndex = 30
        Me.cbCALENDARnewBatch.Text = "Make New Batch-Runs and Sort"
        Me.cbCALENDARnewBatch.UseVisualStyleBackColor = True
        '
        'cbCALENDARmoveToProduction
        '
        Me.cbCALENDARmoveToProduction.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbCALENDARmoveToProduction.AutoSize = True
        Me.cbCALENDARmoveToProduction.Location = New System.Drawing.Point(647, 82)
        Me.cbCALENDARmoveToProduction.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cbCALENDARmoveToProduction.Name = "cbCALENDARmoveToProduction"
        Me.cbCALENDARmoveToProduction.Size = New System.Drawing.Size(157, 21)
        Me.cbCALENDARmoveToProduction.TabIndex = 29
        Me.cbCALENDARmoveToProduction.Text = "Move To Production"
        Me.cbCALENDARmoveToProduction.UseVisualStyleBackColor = True
        '
        'cbCALENDARemailReport
        '
        Me.cbCALENDARemailReport.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbCALENDARemailReport.AutoSize = True
        Me.cbCALENDARemailReport.Location = New System.Drawing.Point(647, 54)
        Me.cbCALENDARemailReport.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cbCALENDARemailReport.Name = "cbCALENDARemailReport"
        Me.cbCALENDARemailReport.Size = New System.Drawing.Size(111, 21)
        Me.cbCALENDARemailReport.TabIndex = 28
        Me.cbCALENDARemailReport.Text = "Email Report"
        Me.cbCALENDARemailReport.UseVisualStyleBackColor = True
        '
        'cbCALENDARjobticket
        '
        Me.cbCALENDARjobticket.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbCALENDARjobticket.AutoSize = True
        Me.cbCALENDARjobticket.Location = New System.Drawing.Point(647, 26)
        Me.cbCALENDARjobticket.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cbCALENDARjobticket.Name = "cbCALENDARjobticket"
        Me.cbCALENDARjobticket.Size = New System.Drawing.Size(95, 21)
        Me.cbCALENDARjobticket.TabIndex = 27
        Me.cbCALENDARjobticket.Text = "Job Ticket"
        Me.cbCALENDARjobticket.UseVisualStyleBackColor = True
        '
        'cbCALENDARmerge
        '
        Me.cbCALENDARmerge.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbCALENDARmerge.AutoSize = True
        Me.cbCALENDARmerge.Location = New System.Drawing.Point(391, 167)
        Me.cbCALENDARmerge.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cbCALENDARmerge.Name = "cbCALENDARmerge"
        Me.cbCALENDARmerge.Size = New System.Drawing.Size(70, 21)
        Me.cbCALENDARmerge.TabIndex = 26
        Me.cbCALENDARmerge.Text = "Merge"
        Me.cbCALENDARmerge.UseVisualStyleBackColor = True
        '
        'cbCALENDARmakeWCpdfs
        '
        Me.cbCALENDARmakeWCpdfs.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbCALENDARmakeWCpdfs.AutoSize = True
        Me.cbCALENDARmakeWCpdfs.Location = New System.Drawing.Point(391, 139)
        Me.cbCALENDARmakeWCpdfs.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cbCALENDARmakeWCpdfs.Name = "cbCALENDARmakeWCpdfs"
        Me.cbCALENDARmakeWCpdfs.Size = New System.Drawing.Size(128, 21)
        Me.cbCALENDARmakeWCpdfs.TabIndex = 25
        Me.cbCALENDARmakeWCpdfs.Text = "Make WC PDFs"
        Me.cbCALENDARmakeWCpdfs.UseVisualStyleBackColor = True
        '
        'cbCALENDARmakeNonWCpdfs
        '
        Me.cbCALENDARmakeNonWCpdfs.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbCALENDARmakeNonWCpdfs.AutoSize = True
        Me.cbCALENDARmakeNonWCpdfs.Location = New System.Drawing.Point(391, 111)
        Me.cbCALENDARmakeNonWCpdfs.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cbCALENDARmakeNonWCpdfs.Name = "cbCALENDARmakeNonWCpdfs"
        Me.cbCALENDARmakeNonWCpdfs.Size = New System.Drawing.Size(158, 21)
        Me.cbCALENDARmakeNonWCpdfs.TabIndex = 24
        Me.cbCALENDARmakeNonWCpdfs.Text = "Make Non WC PDFs"
        Me.cbCALENDARmakeNonWCpdfs.UseVisualStyleBackColor = True
        '
        'cbCALENDARsort
        '
        Me.cbCALENDARsort.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbCALENDARsort.AutoSize = True
        Me.cbCALENDARsort.Enabled = False
        Me.cbCALENDARsort.Location = New System.Drawing.Point(391, 82)
        Me.cbCALENDARsort.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cbCALENDARsort.Name = "cbCALENDARsort"
        Me.cbCALENDARsort.Size = New System.Drawing.Size(56, 21)
        Me.cbCALENDARsort.TabIndex = 23
        Me.cbCALENDARsort.Text = "Sort"
        Me.cbCALENDARsort.UseVisualStyleBackColor = True
        '
        'cbCALENDARimport
        '
        Me.cbCALENDARimport.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbCALENDARimport.AutoSize = True
        Me.cbCALENDARimport.Location = New System.Drawing.Point(391, 26)
        Me.cbCALENDARimport.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cbCALENDARimport.Name = "cbCALENDARimport"
        Me.cbCALENDARimport.Size = New System.Drawing.Size(69, 21)
        Me.cbCALENDARimport.TabIndex = 22
        Me.cbCALENDARimport.Text = "Import"
        Me.cbCALENDARimport.UseVisualStyleBackColor = True
        '
        'btnProcessCalanderReRun
        '
        Me.btnProcessCalanderReRun.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnProcessCalanderReRun.Location = New System.Drawing.Point(28, 262)
        Me.btnProcessCalanderReRun.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnProcessCalanderReRun.Name = "btnProcessCalanderReRun"
        Me.btnProcessCalanderReRun.Size = New System.Drawing.Size(303, 50)
        Me.btnProcessCalanderReRun.TabIndex = 21
        Me.btnProcessCalanderReRun.Text = "Go!"
        Me.btnProcessCalanderReRun.UseVisualStyleBackColor = True
        '
        'rerunCalendar
        '
        Me.rerunCalendar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rerunCalendar.Location = New System.Drawing.Point(28, 12)
        Me.rerunCalendar.Name = "rerunCalendar"
        Me.rerunCalendar.TabIndex = 0
        '
        'ReRunFull
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(997, 641)
        Me.Controls.Add(Me.TabControl1)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "ReRunFull"
        Me.Text = "ReRunFull"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        CType(Me.dvBatches, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents cbMoveToProdcution As System.Windows.Forms.CheckBox
    Friend WithEvents cbEmailReport As System.Windows.Forms.CheckBox
    Friend WithEvents cbJobTicket As System.Windows.Forms.CheckBox
    Friend WithEvents cbMerge As System.Windows.Forms.CheckBox
    Friend WithEvents cbWCpdfs As System.Windows.Forms.CheckBox
    Friend WithEvents CBNonWcPDFS As System.Windows.Forms.CheckBox
    Friend WithEvents CBSort As System.Windows.Forms.CheckBox
    Friend WithEvents CBImport As System.Windows.Forms.CheckBox
    Friend WithEvents btnProcess As System.Windows.Forms.Button
    Friend WithEvents dvBatches As System.Windows.Forms.DataGridView
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents cbBatch As System.Windows.Forms.CheckBox
    Friend WithEvents cbCALENDARnewBatch As System.Windows.Forms.CheckBox
    Friend WithEvents cbCALENDARmoveToProduction As System.Windows.Forms.CheckBox
    Friend WithEvents cbCALENDARemailReport As System.Windows.Forms.CheckBox
    Friend WithEvents cbCALENDARjobticket As System.Windows.Forms.CheckBox
    Friend WithEvents cbCALENDARmerge As System.Windows.Forms.CheckBox
    Friend WithEvents cbCALENDARmakeWCpdfs As System.Windows.Forms.CheckBox
    Friend WithEvents cbCALENDARmakeNonWCpdfs As System.Windows.Forms.CheckBox
    Friend WithEvents cbCALENDARsort As System.Windows.Forms.CheckBox
    Friend WithEvents cbCALENDARimport As System.Windows.Forms.CheckBox
    Friend WithEvents btnProcessCalanderReRun As System.Windows.Forms.Button
    Friend WithEvents rerunCalendar As System.Windows.Forms.MonthCalendar
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnProcessCalanderReRunCancel As System.Windows.Forms.Button
    Friend WithEvents lblStep As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblStepCal As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
