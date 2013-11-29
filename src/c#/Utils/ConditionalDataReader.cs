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
	class ConditionalDataReader : FileManager
	{

		TextReader myFileReader;

        public override void OpenFile(string fileName)
        {
            myFileReader = new StreamReader(fileName);
        }
        public override void CloseFile()
        {
            myFileReader.Close();
        }
		public string GetNextRow()
		{
			return myFileReader.ReadLine();
		}
	}
}

