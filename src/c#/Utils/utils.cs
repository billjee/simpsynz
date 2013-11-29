/*
 * created by: b farooq, poly montreal
 * on: 22 october, 2013
 * last edited by: b farooq, poly montreal
 * on: 22 october, 2013
 * summary: 
 * comments:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopulationSynthesis.Utils
{
    public struct KeyValPair
    {
        public string category;
        public double value;
    }
    struct Constants
    {
        public static double INVALID_VAL = -1000.00;
        public static uint INVALID_UINT_VAL = 0;
        public static string CATEGORY_DELIMITER = "|";
        public static string CONDITIONAL_DELIMITER = "-";
        public static string COLUMN_DELIMETER = ",";
        public static string CONDITIONAL_GENERIC = "...";

        public static int WARMUP_ITERATIONS = 20000;
        public static int SKIP_ITERATIONS = 10;
        public static int POOL_COUNT = 2500000;

        public static double BFRANC_TO_EURO = 40.3399;
        
        public static string DATA_DIR = 
                "C:\\Bilal\\PopulationSynthesis\\InputData\\";
    }

}
