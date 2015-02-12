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
    sealed class OutputFileWriter : IDisposable
    {
        TextWriter FileWriter;

        public OutputFileWriter(string fileName)
        {
            FileWriter = new StreamWriter(fileName);
        }

        public void WriteToFile(string currOutput)
        {
            FileWriter.WriteLine(currOutput);
        }

        ~OutputFileWriter()
        {
            Dispose(false);
        }

        private void Dispose(bool managed)
        {
            if(FileWriter != null)
            {
                FileWriter.Dispose();
                FileWriter = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
