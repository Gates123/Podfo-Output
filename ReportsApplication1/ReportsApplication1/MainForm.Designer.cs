namespace ReportsApplication1
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.PODMasterLetterTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.PODFODataSet = new ReportsApplication1.PODFODataSet();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.PODMasterLetterTypeTableAdapter = new ReportsApplication1.PODFODataSetTableAdapters.PODMasterLetterTypeTableAdapter();
            this.btGenerate = new System.Windows.Forms.Button();
            this.txtBatch = new System.Windows.Forms.TextBox();
            this.txtRun = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnMerge = new System.Windows.Forms.Button();
            this.btnJobTicket = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnEmail = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnSort = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnMove = new System.Windows.Forms.Button();
            this.btnReport = new System.Windows.Forms.Button();
            this.pODFODataSet1 = new ReportsApplication1.PODFODataSet1();
            this.uSPSelectBatchAddressToSortBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.uSP_Select_Batch_Address_To_SortTableAdapter = new ReportsApplication1.PODFODataSet1TableAdapters.USP_Select_Batch_Address_To_SortTableAdapter();
            this.uSPSelectENTBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.uSPSelectACOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.uSPSelectCPCBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.uSPSelectMBPBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.uSPSelectDISBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.button3 = new System.Windows.Forms.Button();
            this.uSP_SELECT_Letter_CountBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.uSP_SELECT_Letter_CountTableAdapter = new ReportsApplication1.PODFODataSet1TableAdapters.USP_SELECT_Letter_CountTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.PODMasterLetterTypeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PODFODataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pODFODataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uSPSelectBatchAddressToSortBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uSPSelectENTBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uSPSelectACOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uSPSelectCPCBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uSPSelectMBPBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uSPSelectDISBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uSP_SELECT_Letter_CountBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // PODMasterLetterTypeBindingSource
            // 
            this.PODMasterLetterTypeBindingSource.DataMember = "PODMasterLetterType";
            this.PODMasterLetterTypeBindingSource.DataSource = this.PODFODataSet;
            // 
            // PODFODataSet
            // 
            this.PODFODataSet.DataSetName = "PODFODataSet";
            this.PODFODataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.reportViewer1.Location = new System.Drawing.Point(0, 285);
            this.reportViewer1.Margin = new System.Windows.Forms.Padding(4);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(741, 206);
            this.reportViewer1.TabIndex = 0;
            this.reportViewer1.Load += new System.EventHandler(this.reportViewer1_Load);
            // 
            // PODMasterLetterTypeTableAdapter
            // 
            this.PODMasterLetterTypeTableAdapter.ClearBeforeFill = true;
            // 
            // btGenerate
            // 
            this.btGenerate.Location = new System.Drawing.Point(329, 97);
            this.btGenerate.Margin = new System.Windows.Forms.Padding(4);
            this.btGenerate.Name = "btGenerate";
            this.btGenerate.Size = new System.Drawing.Size(100, 28);
            this.btGenerate.TabIndex = 1;
            this.btGenerate.Text = "Generate";
            this.btGenerate.UseVisualStyleBackColor = true;
            this.btGenerate.Click += new System.EventHandler(this.btGenerate_Click);
            // 
            // txtBatch
            // 
            this.txtBatch.Location = new System.Drawing.Point(113, 33);
            this.txtBatch.Margin = new System.Windows.Forms.Padding(4);
            this.txtBatch.Name = "txtBatch";
            this.txtBatch.Size = new System.Drawing.Size(132, 22);
            this.txtBatch.TabIndex = 2;
            // 
            // txtRun
            // 
            this.txtRun.Location = new System.Drawing.Point(113, 65);
            this.txtRun.Margin = new System.Windows.Forms.Padding(4);
            this.txtRun.Name = "txtRun";
            this.txtRun.Size = new System.Drawing.Size(132, 22);
            this.txtRun.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 37);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Batch";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 69);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Run";
            // 
            // btnMerge
            // 
            this.btnMerge.Location = new System.Drawing.Point(5, 133);
            this.btnMerge.Margin = new System.Windows.Forms.Padding(4);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(100, 28);
            this.btnMerge.TabIndex = 6;
            this.btnMerge.Text = "Merge";
            this.btnMerge.UseVisualStyleBackColor = true;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // btnJobTicket
            // 
            this.btnJobTicket.Location = new System.Drawing.Point(113, 133);
            this.btnJobTicket.Margin = new System.Windows.Forms.Padding(4);
            this.btnJobTicket.Name = "btnJobTicket";
            this.btnJobTicket.Size = new System.Drawing.Size(100, 28);
            this.btnJobTicket.TabIndex = 7;
            this.btnJobTicket.Text = "Job Ticket";
            this.btnJobTicket.UseVisualStyleBackColor = true;
            this.btnJobTicket.Click += new System.EventHandler(this.btnJobTicket_Click);
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(5, 97);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 8;
            this.button1.Text = "Download";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(437, 97);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(116, 28);
            this.button2.TabIndex = 9;
            this.button2.Text = "Generate WC";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnEmail
            // 
            this.btnEmail.Enabled = false;
            this.btnEmail.Location = new System.Drawing.Point(221, 133);
            this.btnEmail.Margin = new System.Windows.Forms.Padding(4);
            this.btnEmail.Name = "btnEmail";
            this.btnEmail.Size = new System.Drawing.Size(100, 28);
            this.btnEmail.TabIndex = 10;
            this.btnEmail.Text = "Email";
            this.btnEmail.UseVisualStyleBackColor = true;
            this.btnEmail.Click += new System.EventHandler(this.btnEmail_Click);
            // 
            // btnImport
            // 
            this.btnImport.Enabled = false;
            this.btnImport.Location = new System.Drawing.Point(113, 97);
            this.btnImport.Margin = new System.Windows.Forms.Padding(4);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(100, 28);
            this.btnImport.TabIndex = 11;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnSort
            // 
            this.btnSort.Enabled = false;
            this.btnSort.Location = new System.Drawing.Point(221, 97);
            this.btnSort.Margin = new System.Windows.Forms.Padding(4);
            this.btnSort.Name = "btnSort";
            this.btnSort.Size = new System.Drawing.Size(100, 28);
            this.btnSort.TabIndex = 12;
            this.btnSort.Text = "Sort";
            this.btnSort.UseVisualStyleBackColor = true;
            this.btnSort.Click += new System.EventHandler(this.btnSort_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Enabled = false;
            this.btnUpload.Location = new System.Drawing.Point(329, 133);
            this.btnUpload.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(100, 28);
            this.btnUpload.TabIndex = 13;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnMove
            // 
            this.btnMove.Location = new System.Drawing.Point(5, 169);
            this.btnMove.Margin = new System.Windows.Forms.Padding(4);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(424, 59);
            this.btnMove.TabIndex = 14;
            this.btnMove.Text = "Move to Production";
            this.btnMove.UseVisualStyleBackColor = true;
            this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // btnReport
            // 
            this.btnReport.Location = new System.Drawing.Point(437, 133);
            this.btnReport.Margin = new System.Windows.Forms.Padding(4);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(116, 28);
            this.btnReport.TabIndex = 15;
            this.btnReport.Text = "Email Report";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.button3_Click);
            // 
            // pODFODataSet1
            // 
            this.pODFODataSet1.DataSetName = "PODFODataSet1";
            this.pODFODataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uSPSelectBatchAddressToSortBindingSource
            // 
            this.uSPSelectBatchAddressToSortBindingSource.DataMember = "USP_Select_Batch_Address_To_Sort";
            this.uSPSelectBatchAddressToSortBindingSource.DataSource = this.pODFODataSet1;
            // 
            // uSP_Select_Batch_Address_To_SortTableAdapter
            // 
            this.uSP_Select_Batch_Address_To_SortTableAdapter.ClearBeforeFill = true;
            // 
            // uSPSelectENTBindingSource
            // 
            this.uSPSelectENTBindingSource.DataMember = "USP_SELECT_ENT_LETTERS_FOR_BATCH_RUN";
            // 
            // uSPSelectACOBindingSource
            // 
            this.uSPSelectACOBindingSource.DataMember = "USP_SELECT_ACO_LETTERS_FOR_BATCH_RUN";
            // 
            // uSPSelectCPCBindingSource
            // 
            this.uSPSelectCPCBindingSource.DataMember = "USP_SELECT_CPC_LETTERS_FOR_BATCH_RUN";
            // 
            // uSPSelectMBPBindingSource
            // 
            this.uSPSelectMBPBindingSource.DataMember = "USP_SELECT_MBP_LETTERS_FOR_BATCH_RUN";
            // 
            // uSPSelectDISBindingSource
            // 
            this.uSPSelectDISBindingSource.DataMember = "USP_SELECT_DIS_LETTERS_FOR_BATCH_RUN";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(437, 169);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(116, 59);
            this.button3.TabIndex = 16;
            this.button3.Text = "DO ALL";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // uSP_SELECT_Letter_CountBindingSource
            // 
            this.uSP_SELECT_Letter_CountBindingSource.DataMember = "USP_SELECT_Letter_Count";
            this.uSP_SELECT_Letter_CountBindingSource.DataSource = this.pODFODataSet1;
            // 
            // uSP_SELECT_Letter_CountTableAdapter
            // 
            this.uSP_SELECT_Letter_CountTableAdapter.ClearBeforeFill = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(741, 491);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.btnMove);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.btnSort);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnEmail);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnJobTicket);
            this.Controls.Add(this.btnMerge);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRun);
            this.Controls.Add(this.txtBatch);
            this.Controls.Add(this.btGenerate);
            this.Controls.Add(this.reportViewer1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PODMasterLetterTypeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PODFODataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pODFODataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uSPSelectBatchAddressToSortBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uSPSelectENTBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uSPSelectACOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uSPSelectCPCBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uSPSelectMBPBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uSPSelectDISBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uSP_SELECT_Letter_CountBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource PODMasterLetterTypeBindingSource;
        private PODFODataSet PODFODataSet;
        private PODFODataSetTableAdapters.PODMasterLetterTypeTableAdapter PODMasterLetterTypeTableAdapter;
        private PODFODataSet1 pODFODataSet1;
   
        private System.Windows.Forms.BindingSource uSPSelectBatchAddressToSortBindingSource;
        
        private PODFODataSet1TableAdapters.USP_Select_Batch_Address_To_SortTableAdapter uSP_Select_Batch_Address_To_SortTableAdapter;
        
        
        private ENT.PODFODataSetENTTableAdapters.USP_SELECT_ENT_LETTERS_FOR_BATCH_RUNTableAdapter USP_SELECT_ENT_LETTERS_FOR_BATCH_RUNTableAdapter;
        private ENT.PODFODataSetENT pODFODataSetENT;
        private System.Windows.Forms.BindingSource uSPSelectENTBindingSource;

        private ACO.PODFODataSetACOTableAdapters.USP_SELECT_ACO_LETTERS_FOR_BATCH_RUNTableAdapter USP_SELECT_ACO_LETTERS_FOR_BATCH_RUNTableAdapter;
        private ACO.PODFODataSetACO pODFODataSetACO;
        private System.Windows.Forms.BindingSource uSPSelectACOBindingSource;

        private CPC.PODFODataSetCPCTableAdapters.USP_SELECT_CPC_LETTERS_FOR_BATCH_RUNTableAdapter USP_SELECT_CPC_LETTERS_FOR_BATCH_RUNTableAdapter;
        private CPC.PODFODataSetCPC pODFODataSetCPC;
        private System.Windows.Forms.BindingSource uSPSelectCPCBindingSource;

        private MBP.PODFODataSetMBPTableAdapters.USP_SELECT_MBP_LETTERS_FOR_BATCH_RUNTableAdapter USP_SELECT_MBP_LETTERS_FOR_BATCH_RUNTableAdapter;
        private MBP.PODFODataSetMBP pODFODataSetMBP;
        private System.Windows.Forms.BindingSource uSPSelectMBPBindingSource;

        private DIS.PODFODataSetDISTableAdapters.USP_SELECT_DIS_LETTERS_FOR_BATCH_RUNTableAdapter USP_SELECT_DIS_LETTERS_FOR_BATCH_RUNTableAdapter;
        private DIS.PODFODataSetDIS pODFODataSetDIS;
        private System.Windows.Forms.BindingSource uSPSelectDISBindingSource;

        private System.Windows.Forms.Button btGenerate;
        private System.Windows.Forms.TextBox txtBatch;
        private System.Windows.Forms.TextBox txtRun;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnMerge;
        private System.Windows.Forms.Button btnJobTicket;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnEmail;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnSort;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnMove;
        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.BindingSource uSP_SELECT_Letter_CountBindingSource;
        private PODFODataSet1TableAdapters.USP_SELECT_Letter_CountTableAdapter uSP_SELECT_Letter_CountTableAdapter;
    }
}

