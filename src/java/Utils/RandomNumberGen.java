/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package Utils;

import java.util.ArrayList;
import java.util.Random;

/**
 *
 * @author XPS 1645
 */
public class RandomNumberGen
{
    private Random myRandGen;

    public RandomNumberGen()
    {
        myRandGen = new Random();
    }

    public RandomNumberGen(int seed)
    {
        myRandGen = new Random();
        myRandGen.setSeed(seed);
    }

    // Returns a value from 0 and 2,147,483,646
    public int Next()
    {
        return myRandGen.nextInt();
    }

    public int NextInRange(int strt, int end)
    {
        return (myRandGen.nextInt() % ((end - strt) + 1) + strt);
    }

    public double NextDouble()
    {
        return myRandGen.nextDouble();
    }

    public double NextDoubleInRange(double strt, double end)
    {    
        return (myRandGen.nextDouble() * (end - strt)) + strt;
    }

    public ArrayList GetNNumbersInRange(int strt, int end, int n)
    {
        ArrayList currList = new ArrayList();
        if (strt >= end)
        {
            return currList;
        }
        for (int i = 0; i < n; i++)
        {
            currList.add(NextInRange(strt, end));
        }
        return currList;
    }

    // Get normal (Gaussian) random sample with mean 0
    // and standard deviation 1
    public double GetNextStdNormal()
    {
        // Use Box-Muller algorithm
        double u1 = NextDouble();

        double u2 = NextDouble();

        double r = Math.sqrt(-2.0 * Math.log(u1));

        double theta = 2.0 * Math.PI * u2;

        return r * Math.sin(theta);

    }

    // Get normal (Gaussian) random sample with
    // specified mean and standard deviation
    public double GetNextNormal(double mean, double standardDeviation)
    {
        if (standardDeviation <= 0.0)
        {
            System.out.println(String.format("Shape must be positive. Received {0}.", standardDeviation));
            System.exit(-1);
        }

        return mean + standardDeviation * GetNextStdNormal();
    }
}
