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
        private StreamWriter Writer;
        private MemoryStream MemoryBackend;

        public OutputFileWriter(string fileName)
        {
            Writer = new StreamWriter(fileName);
        }

        public OutputFileWriter()
        {
            MemoryBackend = new MemoryStream();
            Writer = new StreamWriter(MemoryBackend);
        }

        public void WriteToFile(string currOutput)
        {
            Writer.WriteLine(currOutput);
        }

        ~OutputFileWriter()
        {
            Dispose(false);
        }

        private void Dispose(bool managed)
        {
            if(MemoryBackend != null)
            {
                MemoryBackend.Dispose();
                MemoryBackend = null;
            }
            if(Writer != null)
            {
                Writer.Dispose();
                Writer = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void CopyTo(OutputFileWriter destinationWriter)
        {
            lock (destinationWriter)
            {
                if(MemoryBackend != null)
                {
                    Writer.Flush();
                    destinationWriter.Writer.Flush();
                    MemoryBackend.WriteTo(destinationWriter.Writer.BaseStream);
                }
            }
        }
    }
}
