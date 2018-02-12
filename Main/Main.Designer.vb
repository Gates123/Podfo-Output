<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main
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
        Me.StatusStrip = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripRPM = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripPercent = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReRunToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FullToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelectRangeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelectLetterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SampleCreatorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cbProdTest = New System.Windows.Forms.ComboBox()
        Me.lblPODFO = New System.Windows.Forms.Label()
        Me.StatusStrip.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'StatusStrip
        '
        Me.StatusStrip.AutoSize = False
        Me.StatusStrip.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel, Me.ToolStripStatusLabel1, Me.ToolStripRPM, Me.ToolStripPercent, Me.ToolStripProgressBar1})
        Me.StatusStrip.Location = New System.Drawing.Point(0, 826)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Padding = New System.Windows.Forms.Padding(1, 0, 19, 0)
        Me.StatusStrip.Size = New System.Drawing.Size(1343, 37)
        Me.StatusStrip.TabIndex = 7
        Me.StatusStrip.Text = "StatusStrip"
        '
        'ToolStripStatusLabel
        '
        Me.ToolStripStatusLabel.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripStatusLabel.Name = "ToolStripStatusLabel"
        Me.ToolStripStatusLabel.Size = New System.Drawing.Size(63, 32)
        Me.ToolStripStatusLabel.Text = "Status"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(903, 32)
        Me.ToolStripStatusLabel1.Spring = True
        '
        'ToolStripRPM
        '
        Me.ToolStripRPM.Name = "ToolStripRPM"
        Me.ToolStripRPM.Size = New System.Drawing.Size(37, 32)
        Me.ToolStripRPM.Text = "000"
        '
        'ToolStripPercent
        '
        Me.ToolStripPercent.Name = "ToolStripPercent"
        Me.ToolStripPercent.Size = New System.Drawing.Size(51, 32)
        Me.ToolStripPercent.Text = "100%"
        '
        'ToolStripProgressBar1
        '
        Me.ToolStripProgressBar1.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripProgressBar1.Name = "ToolStripProgressBar1"
        Me.ToolStripProgressBar1.Size = New System.Drawing.Size(267, 31)
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolsToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(5, 2, 0, 2)
        Me.MenuStrip1.Size = New System.Drawing.Size(1343, 28)
        Me.MenuStrip1.TabIndex = 9
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ReRunToolStripMenuItem, Me.SampleCreatorToolStripMenuItem})
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(57, 24)
        Me.ToolsToolStripMenuItem.Text = "Tools"
        '
        'ReRunToolStripMenuItem
        '
        Me.ReRunToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FullToolStripMenuItem, Me.SelectRangeToolStripMenuItem, Me.SelectLetterToolStripMenuItem})
        Me.ReRunToolStripMenuItem.Name = "ReRunToolStripMenuItem"
        Me.ReRunToolStripMenuItem.Size = New System.Drawing.Size(181, 24)
        Me.ReRunToolStripMenuItem.Text = "Re-Run"
        '
        'FullToolStripMenuItem
        '
        Me.FullToolStripMenuItem.Name = "FullToolStripMenuItem"
        Me.FullToolStripMenuItem.Size = New System.Drawing.Size(164, 24)
        Me.FullToolStripMenuItem.Text = "Full"
        '
        'SelectRangeToolStripMenuItem
        '
        Me.SelectRangeToolStripMenuItem.Name = "SelectRangeToolStripMenuItem"
        Me.SelectRangeToolStripMenuItem.Size = New System.Drawing.Size(164, 24)
        Me.SelectRangeToolStripMenuItem.Text = "Select Range"
        '
        'SelectLetterToolStripMenuItem
        '
        Me.SelectLetterToolStripMenuItem.Name = "SelectLetterToolStripMenuItem"
        Me.SelectLetterToolStripMenuItem.Size = New System.Drawing.Size(164, 24)
        Me.SelectLetterToolStripMenuItem.Text = "Select Letter"
        '
        'SampleCreatorToolStripMenuItem
        '
        Me.SampleCreatorToolStripMenuItem.Name = "SampleCreatorToolStripMenuItem"
        Me.SampleCreatorToolStripMenuItem.Size = New System.Drawing.Size(181, 24)
        Me.SampleCreatorToolStripMenuItem.Text = "Sample Creator"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.LightBlue
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.cbProdTest)
        Me.Panel1.Controls.Add(Me.lblPODFO)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 28)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1343, 44)
        Me.Panel1.TabIndex = 11
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(1027, 7)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(102, 25)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Database:"
        '
        'cbProdTest
        '
        Me.cbProdTest.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbProdTest.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbProdTest.FormattingEnabled = True
        Me.cbProdTest.Location = New System.Drawing.Point(1163, 3)
        Me.cbProdTest.Margin = New System.Windows.Forms.Padding(4)
        Me.cbProdTest.Name = "cbProdTest"
        Me.cbProdTest.Size = New System.Drawing.Size(167, 33)
        Me.cbProdTest.TabIndex = 1
        '
        'lblPODFO
        '
        Me.lblPODFO.AutoSize = True
        Me.lblPODFO.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPODFO.Location = New System.Drawing.Point(29, 7)
        Me.lblPODFO.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblPODFO.Name = "lblPODFO"
        Me.lblPODFO.Size = New System.Drawing.Size(99, 29)
        Me.lblPODFO.TabIndex = 0
        Me.lblPODFO.Text = "PODFO"
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1343, 863)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.StatusStrip)
        Me.Controls.Add(Me.MenuStrip1)
        Me.IsMdiContainer = True
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "Main"
        Me.Text = "PODFO"
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents ToolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReRunToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FullToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SelectRangeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SelectLetterToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents cbProdTest As System.Windows.Forms.ComboBox
    Friend WithEvents lblPODFO As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripProgressBar1 As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents ToolStripRPM As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripPercent As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents SampleCreatorToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
