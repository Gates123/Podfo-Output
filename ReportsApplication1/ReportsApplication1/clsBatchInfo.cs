using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.IO;
using System.Data.SqlClient;

namespace ReportsApplication1
{
  public class clsBatchInfo
    {
        private string batch1;
        private string run1;
        private int images1;
        private int records1;
        private PODFODataSet1TableAdapters.USP_Select_Batch_Address_To_SortTableAdapter SelectBatchTA = new PODFODataSet1TableAdapters.USP_Select_Batch_Address_To_SortTableAdapter();
        private PODFODataSet1TableAdapters.USP_SELECT_Letter_CountTableAdapter LetterCountTA = new PODFODataSet1TableAdapters.USP_SELECT_Letter_CountTableAdapter();
        private PODFODataSet1 DS = new PODFODataSet1();
      //private BindingSource  BatchAddsSort
          //uSPSelectBatchAddressToSortBindingSource
        private BindingSource BSCount = new BindingSource(new PODFODataSet1(), "USP_SELECT_Letter_Count");
        private BindingSource BSsortAddress = new BindingSource(new PODFODataSet1(), "USP_Select_Batch_Address_To_Sort");

        public string Batch
        {
            get
            {
                return this.batch1;
            }
            set
            {
                this.batch1 = value;
            }
        }
        public string Run
        {
            get
            {
                return this.run1;
            }
            set
            {
                this.run1 = value;
            }
        }

        public int Images
        {
            get
            {
                return images1;
            }
            set
            {
                images1 = value;
            }
        }

        public int Records
        {
            get
            {
                return records1;
            }
            set
            {
                records1 = value;
            }
        }
      
      //public PODFODataSet1TableAdapters.USP_SELECT_Letter_CountTableAdapter LetterCount
      //{
      //    get
      //    {
      //        return LetterCountTA;
      //    }
      //    set
      //    {
      //        LetterCountTA = value;
      //    }
      //}

      //public PODFODataSet1TableAdapters.USP_Select_Batch_Address_To_SortTableAdapter SortBatchAddress
      //{
      //    get
      //    {
      //        return SelectBatchTA;
      //    }
      //    set
      //    {
      //        SelectBatchTA = value;
      //    }
      //}
      //public PODFODataSet1 PODds
      //{
      //    get
      //    {
      //        return DS;
      //    }
      //    set
      //    {
      //        DS = value;
      //    }
      //}
      //public BindingSource BindsCount           
      //{
      //    get
      //    {
      //        return BSCount;
      //    }
      //    set
      //    {
      //        BSCount = value;
      //    }
      //}
      //public BindingSource BindsLetters
      //{
      //    get
      //    {
      //        return BSsortAddress;
      //    }
      //    set
      //    {
      //        BSsortAddress = value;
      //    }
      //}

    }
}
