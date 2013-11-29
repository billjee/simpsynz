/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package Utils;

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 *
 * @author XPS 1645
 */
public class OutputFileWritter extends FileManager
{
    BufferedWriter myFileWritter;

    @Override
    public void OpenFile(String fileName)
    {
        FileWriter fstream = null;
        try {
            fstream = new FileWriter(fileName);
            myFileWritter = new BufferedWriter(fstream);
        } catch (IOException ex) {
            Logger.getLogger(OutputFileWritter.class.getName()).log(Level.SEVERE, null, ex);
        }
    }

    @Override
    public void CloseFile() throws IOException
    {
        myFileWritter.close();
    }

    public void WriteToFile(String currOutput) throws IOException
    {
        myFileWritter.write(currOutput);
        myFileWritter.newLine();
    }
}
