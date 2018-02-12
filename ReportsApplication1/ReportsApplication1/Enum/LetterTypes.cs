using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportsApplication1.Enums
{
    class LetterTypes
    {
        public enum ACO
        {
            ACOINAENG = 11,
            ACOINASPA = 12,
            ACOINRSAENG = 15,
            ACOINRSASPA = 16,
            ACOINUENG= 17,
            ACOINUSPA = 18,
            ACOOUTAENG = 19,
            ACOOUTASPA = 20,
            ACOOUTUENG = 21,
            ACOOUTUSPA = 22

        }

        public enum CPC
        {

            CPCOUTENG = 23,
            CPCOUTSPAN = 24,
            CPCOUTRENG = 25,
            CPCOUTRSPAN = 26

        }

        public enum MBP
        {
           MBP_AE_ENG= 35,
           MBP_DA_ENG= 29,
           MBP_NE_ENG= 37,
           MBP_PREP_ENG= 41,
           MBP_PRWE_ENG= 43,
           MBP_RAEP_ENG= 31, 
           MBP_RAWE_ENG= 33,
           MBP_RS_ENG= 39,
           MBP_RS_SPAN= 40

        }


        public enum DIS
        {

            DISDMA_MAENG = 1,
            DISDMA_MASPA = 2,
            DISDPD_PDAENG = 3,
            DISDPD_PDASPA = 4,
            DISOPT_PDPENG = 7,
            DISOPT_PDPSPA = 8

        }

        public enum ENT
        {

            ENTENMBP = 10,
            ENTENNGD = 9

        }

    }
}
