<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ReRunLetter
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
        Me.btnProcess = New System.Windows.Forms.Button()
        Me.dvBatches = New System.Windows.Forms.DataGridView()
        Me.rbACO = New System.Windows.Forms.RadioButton()
        Me.rbCPC = New System.Windows.Forms.RadioButton()
        Me.rbENT = New System.Windows.Forms.RadioButton()
        Me.rbDIS = New System.Windows.Forms.RadioButton()
        Me.rbMBP = New System.Windows.Forms.RadioButton()
        Me.rbWC = New System.Windows.Forms.RadioButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblStep = New System.Windows.Forms.Label()
        CType(Me.dvBatches, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnProcess
        '
        Me.btnProcess.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnProcess.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnProcess.Location = New System.Drawing.Point(321, 411)
        Me.btnProcess.Margin = New System.Windows.Forms.Padding(2)
        Me.btnProcess.Name = "btnProcess"
        Me.btnProcess.Size = New System.Drawing.Size(122, 30)
        Me.btnProcess.TabIndex = 39
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
        Me.dvBatches.Location = New System.Drawing.Point(9, 10)
        Me.dvBatches.Margin = New System.Windows.Forms.Padding(2)
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
        Me.dvBatches.Size = New System.Drawing.Size(720, 386)
        Me.dvBatches.TabIndex = 38
        '
        'rbACO
        '
        Me.rbACO.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.rbACO.AutoSize = True
        Me.rbACO.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbACO.Location = New System.Drawing.Point(11, 429)
        Me.rbACO.Margin = New System.Windows.Forms.Padding(2)
        Me.rbACO.Name = "rbACO"
        Me.rbACO.Size = New System.Drawing.Size(54, 20)
        Me.rbACO.TabIndex = 40
        Me.rbACO.TabStop = True
        Me.rbACO.Text = "ACO"
        Me.rbACO.UseVisualStyleBackColor = True
        '
        'rbCPC
        '
        Me.rbCPC.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.rbCPC.AutoSize = True
        Me.rbCPC.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbCPC.Location = New System.Drawing.Point(11, 451)
        Me.rbCPC.Margin = New System.Windows.Forms.Padding(2)
        Me.rbCPC.Name = "rbCPC"
        Me.rbCPC.Size = New System.Drawing.Size(53, 20)
        Me.rbCPC.TabIndex = 41
        Me.rbCPC.TabStop = True
        Me.rbCPC.Text = "CPC"
        Me.rbCPC.UseVisualStyleBackColor = True
        '
        'rbENT
        '
        Me.rbENT.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.rbENT.AutoSize = True
        Me.rbENT.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbENT.Location = New System.Drawing.Point(92, 429)
        Me.rbENT.Margin = New System.Windows.Forms.Padding(2)
        Me.rbENT.Name = "rbENT"
        Me.rbENT.Size = New System.Drawing.Size(54, 20)
        Me.rbENT.TabIndex = 42
        Me.rbENT.TabStop = True
        Me.rbENT.Text = "ENT"
        Me.rbENT.UseVisualStyleBackColor = True
        '
        'rbDIS
        '
        Me.rbDIS.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.rbDIS.AutoSize = True
        Me.rbDIS.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbDIS.Location = New System.Drawing.Point(11, 473)
        Me.rbDIS.Margin = New System.Windows.Forms.Padding(2)
        Me.rbDIS.Name = "rbDIS"
        Me.rbDIS.Size = New System.Drawing.Size(48, 20)
        Me.rbDIS.TabIndex = 43
        Me.rbDIS.TabStop = True
        Me.rbDIS.Text = "DIS"
        Me.rbDIS.UseVisualStyleBackColor = True
        '
        'rbMBP
        '
        Me.rbMBP.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.rbMBP.AutoSize = True
        Me.rbMBP.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbMBP.Location = New System.Drawing.Point(92, 450)
        Me.rbMBP.Margin = New System.Windows.Forms.Padding(2)
        Me.rbMBP.Name = "rbMBP"
        Me.rbMBP.Size = New System.Drawing.Size(55, 20)
        Me.rbMBP.TabIndex = 44
        Me.rbMBP.TabStop = True
        Me.rbMBP.Text = "MBP"
        Me.rbMBP.UseVisualStyleBackColor = True
        '
        'rbWC
        '
        Me.rbWC.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.rbWC.AutoSize = True
        Me.rbWC.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbWC.Location = New System.Drawing.Point(92, 472)
        Me.rbWC.Margin = New System.Windows.Forms.Padding(2)
        Me.rbWC.Name = "rbWC"
        Me.rbWC.Size = New System.Drawing.Size(48, 20)
        Me.rbWC.TabIndex = 45
        Me.rbWC.TabStop = True
        Me.rbWC.Text = "WC"
        Me.rbWC.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(11, 411)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(193, 16)
        Me.Label1.TabIndex = 46
        Me.Label1.Text = "Select a Letter Type to Re Run:"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(489, 411)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(2)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(122, 30)
        Me.btnCancel.TabIndex = 47
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(278, 472)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(42, 18)
        Me.Label2.TabIndex = 48
        Me.Label2.Text = "Step:"
        '
        'lblStep
        '
        Me.lblStep.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblStep.AutoSize = True
        Me.lblStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep.Location = New System.Drawing.Point(324, 472)
        Me.lblStep.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblStep.Name = "lblStep"
        Me.lblStep.Size = New System.Drawing.Size(0, 18)
        Me.lblStep.TabIndex = 50
        '
        'ReRunLetter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(738, 519)
        Me.Controls.Add(Me.lblStep)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.rbWC)
        Me.Controls.Add(Me.rbMBP)
        Me.Controls.Add(Me.rbDIS)
        Me.Controls.Add(Me.rbENT)
        Me.Controls.Add(Me.rbCPC)
        Me.Controls.Add(Me.rbACO)
        Me.Controls.Add(Me.btnProcess)
        Me.Controls.Add(Me.dvBatches)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "ReRunLetter"
        Me.Text = "ReRunLetter"
        CType(Me.dvBatches, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnProcess As System.Windows.Forms.Button
    Friend WithEvents dvBatches As System.Windows.Forms.DataGridView
    Friend WithEvents rbACO As System.Windows.Forms.RadioButton
    Friend WithEvents rbCPC As System.Windows.Forms.RadioButton
    Friend WithEvents rbENT As System.Windows.Forms.RadioButton
    Friend WithEvents rbDIS As System.Windows.Forms.RadioButton
    Friend WithEvents rbMBP As System.Windows.Forms.RadioButton
    Friend WithEvents rbWC As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblStep As System.Windows.Forms.Label
End Class
