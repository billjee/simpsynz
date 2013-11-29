/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package Utils;

import java.io.*;

/**
 *
 * @author XPS 1645
 */
public class ConditionalDataReader extends FileManager
{
    BufferedReader myFileReader;

    @Override
    public void OpenFile(String fileName)
    {
        myFileReader = new BufferedReader(new StringReader(fileName));
    }

    @Override
    public void CloseFile() throws IOException
    {
        myFileReader.close();
    }

    public String GetNextRow() throws IOException
    {
        return myFileReader.readLine();
    }
}
