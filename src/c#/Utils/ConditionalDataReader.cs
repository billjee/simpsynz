/*
 * created by: b farooq, poly montreal
 * on: 22 october, 2013
 * last edited by: b farooq, poly montreal
 * on: 22 october, 2013
 * summary: 
 * comments:
 */

using System;
using System.Collections;
using System.IO;

namespace PopulationSynthesis.Utils
{
    sealed class ConditionalDataReader : IDisposable
    {

        TextReader FileReader;

        public ConditionalDataReader(string fileName)
        {
            FileReader = new StreamReader(fileName);
        }

        public string GetNextRow()
        {
            return FileReader.ReadLine();
        }

        ~ConditionalDataReader()
        {
            Dispose(false);
        }

        private void Dispose(bool managed)
        {
            if(FileReader != null)
            {
                FileReader.Dispose();
                FileReader = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

