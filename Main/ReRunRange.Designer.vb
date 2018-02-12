<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ReRunRange
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
        Me.cbWCpdfs = New System.Windows.Forms.CheckBox()
        Me.CBNonWcPDFS = New System.Windows.Forms.CheckBox()
        Me.btnProcess = New System.Windows.Forms.Button()
        Me.dvBatches = New System.Windows.Forms.DataGridView()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tbStart = New System.Windows.Forms.MaskedTextBox()
        Me.tbEnd = New System.Windows.Forms.MaskedTextBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblStep = New System.Windows.Forms.Label()
        CType(Me.dvBatches, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cbWCpdfs
        '
        Me.cbWCpdfs.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cbWCpdfs.AutoSize = True
        Me.cbWCpdfs.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbWCpdfs.Location = New System.Drawing.Point(198, 460)
        Me.cbWCpdfs.Margin = New System.Windows.Forms.Padding(2)
        Me.cbWCpdfs.Name = "cbWCpdfs"
        Me.cbWCpdfs.Size = New System.Drawing.Size(123, 20)
        Me.cbWCpdfs.TabIndex = 26
        Me.cbWCpdfs.Text = "Make WC PDFs"
        Me.cbWCpdfs.UseVisualStyleBackColor = True
        '
        'CBNonWcPDFS
        '
        Me.CBNonWcPDFS.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CBNonWcPDFS.AutoSize = True
        Me.CBNonWcPDFS.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CBNonWcPDFS.Location = New System.Drawing.Point(198, 482)
        Me.CBNonWcPDFS.Margin = New System.Windows.Forms.Padding(2)
        Me.CBNonWcPDFS.Name = "CBNonWcPDFS"
        Me.CBNonWcPDFS.Size = New System.Drawing.Size(151, 20)
        Me.CBNonWcPDFS.TabIndex = 25
        Me.CBNonWcPDFS.Text = "Make Non WC PDFs"
        Me.CBNonWcPDFS.UseVisualStyleBackColor = True
        '
        'btnProcess
        '
        Me.btnProcess.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnProcess.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnProcess.Location = New System.Drawing.Point(402, 460)
        Me.btnProcess.Margin = New System.Windows.Forms.Padding(2)
        Me.btnProcess.Name = "btnProcess"
        Me.btnProcess.Size = New System.Drawing.Size(122, 30)
        Me.btnProcess.TabIndex = 22
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
        Me.dvBatches.Size = New System.Drawing.Size(726, 434)
        Me.dvBatches.TabIndex = 21
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 452)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(154, 16)
        Me.Label1.TabIndex = 32
        Me.Label1.Text = "Select a Range to Rerun"
        '
        'tbStart
        '
        Me.tbStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.tbStart.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbStart.Location = New System.Drawing.Point(10, 473)
        Me.tbStart.Margin = New System.Windows.Forms.Padding(2)
        Me.tbStart.Mask = "0000000"
        Me.tbStart.Name = "tbStart"
        Me.tbStart.Size = New System.Drawing.Size(76, 22)
        Me.tbStart.TabIndex = 36
        '
        'tbEnd
        '
        Me.tbEnd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.tbEnd.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbEnd.Location = New System.Drawing.Point(104, 472)
        Me.tbEnd.Margin = New System.Windows.Forms.Padding(2)
        Me.tbEnd.Mask = "0000000"
        Me.tbEnd.Name = "tbEnd"
        Me.tbEnd.Size = New System.Drawing.Size(76, 22)
        Me.tbEnd.TabIndex = 37
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(547, 460)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(2)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(122, 30)
        Me.btnCancel.TabIndex = 38
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(353, 496)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(42, 18)
        Me.Label2.TabIndex = 39
        Me.Label2.Text = "Step:"
        '
        'lblStep
        '
        Me.lblStep.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblStep.AutoSize = True
        Me.lblStep.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStep.Location = New System.Drawing.Point(399, 496)
        Me.lblStep.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblStep.Name = "lblStep"
        Me.lblStep.Size = New System.Drawing.Size(0, 18)
        Me.lblStep.TabIndex = 40
        '
        'ReRunRange
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(744, 521)
        Me.Controls.Add(Me.lblStep)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.tbEnd)
        Me.Controls.Add(Me.tbStart)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cbWCpdfs)
        Me.Controls.Add(Me.CBNonWcPDFS)
        Me.Controls.Add(Me.btnProcess)
        Me.Controls.Add(Me.dvBatches)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "ReRunRange"
        Me.Text = "ReRunRange"
        CType(Me.dvBatches, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cbWCpdfs As System.Windows.Forms.CheckBox
    Friend WithEvents CBNonWcPDFS As System.Windows.Forms.CheckBox
    Friend WithEvents btnProcess As System.Windows.Forms.Button
    Friend WithEvents dvBatches As System.Windows.Forms.DataGridView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tbStart As System.Windows.Forms.MaskedTextBox
    Friend WithEvents tbEnd As System.Windows.Forms.MaskedTextBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblStep As System.Windows.Forms.Label
End Class
