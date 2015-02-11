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
    sealed class OutputFileWritter : IDisposable
    {
        TextWriter FileWriter;

        public OutputFileWritter(string fileName)
        {
            FileWriter = new StreamWriter(fileName);
        }

        public void WriteToFile(string currOutput)
        {
            FileWriter.WriteLine(currOutput);
        }

        ~OutputFileWritter()
        {
            Dispose(false);
        }

        private void Dispose(bool managed)
        {
            if(managed)
            {
                GC.SuppressFinalize(this);
            }
            if(FileWriter != null)
            {
                FileWriter.Dispose();
                FileWriter = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
