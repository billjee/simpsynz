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
using System.IO;

namespace PopulationSynthesis.Utils
{
    class OutputFileWritter : FileManager
    {
        TextWriter myFileWritter;
        public override void OpenFile(string fileName)
        {
            myFileWritter = new StreamWriter(fileName);
        }
        public override void CloseFile()
        {
            myFileWritter.Close();
        }
        public void WriteToFile(string currOutput)
        {
            myFileWritter.WriteLine(currOutput);
        }
    }
}
