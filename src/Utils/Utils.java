/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package Utils;

/**
 *
 * @author XPS 1645
 */
public class Utils
{
    //KeyValPair
    public String category;
    public double value;

    //Constants
    public static double INVALID_VAL = -1000.00;
    public static int INVALID_UINT_VAL = 0;
    public static String CATEGORY_DELIMITER = "|";
    public static String CONDITIONAL_DELIMITER = "-";
    public static String COLUMN_DELIMETER = ",";
    public static String CONDITIONAL_GENERIC = "...";

    public static int WARMUP_ITERATIONS = 20000;
    public static int SKIP_ITERATIONS = 10;
    public static int POOL_COUNT = 2500000;

    public static double BFRANC_TO_EURO = 40.3399;

    public static String DATA_DIR = "C:\\Bilal\\PopulationSynthesis\\InputData\\";
}
