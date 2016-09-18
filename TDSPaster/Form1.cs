using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDSPaster
{
    public partial class Form1 : Form
    {       
        string fileLocation;
        public Form1()
        {
            InitializeComponent();
            PasteDataButton.Enabled = false;
        }
        //cleaning up the many types of comments that are not TDS compatable
        public string dataValidator(string readingValue)
        {
            char[] charsToTrim = { '.', 'V' };
            string goodData;
            string quote = "\"";
            string spaceQuote = " \"";
            if (Regex.IsMatch(readingValue, @"\d"))
            {
                readingValue = readingValue.Trim(charsToTrim);
                readingValue = quote + readingValue + spaceQuote;
                return readingValue;
                
            }
            else
            {
                goodData = @"""    """;
                return goodData;
            }

        }

        private void PasteDataButton_Click(object sender, EventArgs e)
        {
            //paste data from clipboard
            DataObject o = (DataObject)Clipboard.GetDataObject();
            if (o.GetDataPresent(DataFormats.Text))
            {
                if (dataGridView1.RowCount > 0)
                    dataGridView1.Rows.Clear();

                if (dataGridView1.ColumnCount > 0)
                    dataGridView1.Columns.Clear();

                bool columnsAdded = false;
                string[] pastedRows = Regex.Split(o.GetData(DataFormats.Text).ToString().TrimEnd("\r\n".ToCharArray()), "\r\n");
                int j = 0;
                foreach (string pastedRow in pastedRows)
                {
                    string[] pastedRowCells = pastedRow.Split(new char[] { '\t' });

                    if (!columnsAdded)
                    {
                        for (int i = 0; i < pastedRowCells.Length; i++)
                            dataGridView1.Columns.Add("col" + i, pastedRowCells[i]);

                        columnsAdded = true;
                        continue;
                    }

                    dataGridView1.Rows.Add();
                    int myRowIndex = dataGridView1.Rows.Count - 1;

                    using (DataGridViewRow dataGridView1Row = dataGridView1.Rows[j])
                    {
                        for (int i = 0; i < pastedRowCells.Length; i++)
                            dataGridView1Row.Cells[i].Value = pastedRowCells[i];
                    }
                    j++;
                }
            }
            if (Global.GlobalVar != dataGridView1.ColumnCount)
            {
                MessageBox.Show("Oh no! It seems that you have pasted an amount of tubes that does not match the tube count for the section! If you continue the program will crash!");
            }
        }

        private void SelectFileButton_Click(object sender, EventArgs e)
        {
            PasteDataButton.Enabled = true;
            // Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;

            openFileDialog1.Multiselect = true;

            // Call the ShowDialog method to show the dialog box.
            DialogResult userClickedOK = openFileDialog1.ShowDialog();


            // Process input if the user clicked OK.
            if (userClickedOK == DialogResult.OK)
            {
                // Open the selected file to read.
                Stream fileStream = openFileDialog1.OpenFile();

                using (StreamReader reader = new StreamReader(fileStream))
                {
                    string filename = openFileDialog1.FileName;

                    fileLocation = filename;                  
                }
                fileStream.Close();
                // Code that is going to fetch the tube count and other goodies!
                string line;
                int counter = 0;

                StreamReader reader2 = new StreamReader(fileLocation);


                while ((line = reader2.ReadLine()) != null)
                {
                    previewListBox.Items.Add(line);
                    counter++;
                }
                float finalTubeCount = ((counter - 18f) / 3f);
                Global.GlobalVar = finalTubeCount;
                Debug.WriteLine("Tube count:" + finalTubeCount);
                reader2.Close();
            }
        }

        //creating the class that will allow for global variable use.
        //this is for passing the tube count to the conversion routine
        static class Global
        {
            private static float _globalVar;
            private static int _globalCounter;

            public static float GlobalVar
            {
                get { return _globalVar; }
                set { _globalVar = value; }

            }
            public static int GlobalCounter
            {
                get { return _globalCounter; }
                set { _globalCounter = value; }
            }
        }
        
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            //creating the array, this is dynamic, the array will match the number of columns and rows
            string[,] readingValueArray = new string[dataGridView1.Rows.Count, dataGridView1.Columns.Count];
            //populating the array
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {                
                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {//crawling the datagridview and storing values in the array
                    readingValueArray[row.Index, col.Index] = dataGridView1.Rows[row.Index].Cells[col.Index].Value.ToString();
                }
            }
            //this is going to fetch the amount of lines in the document we are writing to
            string[] lines = File.ReadAllLines(fileLocation);
            //simple counters for each location on the tube
            int leftCounter = 0;
            int centerCounter = 0;
            int rightCounter = 0;

            
            //outputting the array values to the TDS file
            using (StreamWriter writer = new StreamWriter(fileLocation))
            {
                for (int currentLine = 1; currentLine <= lines.Length; ++currentLine)
                {    //printing left readings to file first             
                     if (currentLine >= 6 && currentLine <= (Global.GlobalVar +5))
                     {
                        string value = readingValueArray[0, leftCounter];
                        //passing the cleanup method the value of the cell in the array, it will return the TDS acceptable value
                        string returnedValue = dataValidator(value);
                        
                        writer.WriteLine(returnedValue);
                        leftCounter++;
                     }//printing center readings
                    else if (currentLine > (Global.GlobalVar + 11) && currentLine <= ((Global.GlobalVar * 2) + 11))
                    {
                        string value = readingValueArray[1, centerCounter];
                        //passing the cleanup method the value of the cell in the array, it will return the TDS acceptable value
                        string returnedValue = dataValidator(value);

                        writer.WriteLine(returnedValue);
                        centerCounter++;
                    }//printing right readings
                    else if (currentLine > ((Global.GlobalVar * 2) + 17) && currentLine <= ((Global.GlobalVar * 3) + 17))
                     {
                        string value = readingValueArray[2, rightCounter];
                        //passing the cleanup method the value of the cell in the array, it will return the TDS acceptable value
                        string returnedValue = dataValidator(value);

                        writer.WriteLine(returnedValue);
                            rightCounter++;
                     }
                        
                    else 
                    {//not 100% sure I think this closes the stream once end of file is reached and saves it also
                        writer.WriteLine(lines[currentLine - 1]);
                    }                    
                }
            }
            MessageBox.Show("If nothing caught fire the file was transfered!");
        }
    }
}

