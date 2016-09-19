using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TDSPaster
{
    public partial class MainForm : Form
    {       
        string fileLocation;
        string location;
        string millName;
        string sectionName;
        string elevationName1;
        string elevationName2;
        string elevationName3;
        string boilerName;
        string inspectionYear;
        int tubeCount;

        public MainForm()
        {
            InitializeComponent();
            //In order to do some basic validation I require the user to select a file before the paste button is enabled
            PasteDataButton.Enabled = false; 
        }

        //cleaning up the many types of comments that are not TDS compatable
        public string DataValidator(string readingValue)
        {   
            char[] charsToTrim = { '.' };//Place any characters that should not go into the TDS file here
            string goodData;
            //the following two strings encase the numeric value
            string quote = "\"";
            string spaceQuote = " \"";
            //if the data is numbers with a single character the if statement will execute.           
            if (Regex.IsMatch(readingValue, @"\d"))
            {
                readingValue = readingValue.Trim(charsToTrim);
                readingValue = quote + readingValue + spaceQuote;
                return readingValue;  
            }
            //if there is only text or more than 1 non integer character the else statement will execute and return 4 spaces to be written to the TDS file
            goodData = @"""    """;
            return goodData;
        }

        private static string RemoveNonAlpha(string line)
        {
            //Replace all multiple spaces with just one space, then remove the quotes.
            var newLine = Regex.Replace(line, @"\s+", " ");
            newLine = newLine.Replace("\"", string.Empty);
            return newLine;
        }

        private void SetInspectionInfo(string line, int counter)
        {
            string newLine;

            if (counter == 0)
            {
                newLine = RemoveNonAlpha(line);
                millName = newLine;
            }
            if (counter == 1)
            {
                newLine = RemoveNonAlpha(line);
                boilerName = newLine;
            }
            if (counter == 2)
            {
                newLine = RemoveNonAlpha(line);
                sectionName = newLine;
            }
            if (counter == 3)
            {
                newLine = RemoveNonAlpha(line);
                elevationName2 = newLine;
            }
            if (counter == 4)
            {
                newLine = RemoveNonAlpha(line);
                elevationName1 = newLine;
            }
            if (counter == tubeCount +6)
            {
                newLine = RemoveNonAlpha(line);
                location = newLine;
            }
            if (counter == tubeCount +7)
            {
                newLine = RemoveNonAlpha(line);
                inspectionYear = newLine;
            } 
            if (counter == tubeCount +10)
            {
                newLine = RemoveNonAlpha(line);
                elevationName3 = newLine;
            }
        }

        //copies the data from the clipboard and other housekeeping things
        private void PasteDataButton_Click(object sender, EventArgs e)
        {
            //paste data from clipboard
            DataObject o = (DataObject)Clipboard.GetDataObject();
            if (o.GetDataPresent(DataFormats.Text))
            {
                string[] pastedRows = Regex.Split(o.GetData(DataFormats.Text).ToString().TrimEnd("\r\n".ToCharArray()), "\r\n");
                int j = 0;
                
                //Manually adding the tube columns to the datagrid view (this is required before data can be added
                for (int b = 1; b <= tubeCount; b++)
                {
                    string tubeColumn = "Tube";
                    dataGridView1.Columns.Add(tubeColumn + b, b.ToString());
                }
                //This is the code that adds the copied data to the datagrid view, not 100% on how it works.
                foreach (string pastedRow in pastedRows)
                {                   
                    string[] pastedRowCells = pastedRow.Split(new char[] { '\t' });
                                        
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
            //Simple data validation. If the amount of tubes pasted does not equal the amount of tubes for the actual section
            // the user is given an alert. Lots of room for improvement here.         
            if (tubeCount != dataGridView1.ColumnCount)
            {
                MessageBox.Show("Oh no! It seems that you have pasted an amount of tubes that does not match the tube count for the section! If you continue the program will crash!");
            }
        }

        //gathers the information for the elevation you want to write to
        private void SelectFileButton_Click(object sender, EventArgs e)
        {
            
            // Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                // Set filter options and filter index.
                Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*",
                FilterIndex = 2,
                Multiselect = true
            };

            // Call the ShowDialog method to show the dialog box.
            DialogResult userClickedOk = openFileDialog1.ShowDialog();

            // Process input if the user clicked OK.
            if (userClickedOk == DialogResult.OK)
            {
                PasteDataButton.Enabled = true;

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
                StreamReader reader3 = new StreamReader(fileLocation);
                while (reader3.ReadLine() != null)
                {
                    counter++;
                }
                int finalTubeCount = (counter - 18) / 3;
                tubeCount = finalTubeCount;
                reader3.Close();

                StreamReader reader2 = new StreamReader(fileLocation);
                counter = 0;
                //This is grabing the informaion about the elevation that has been selected to be overwritten
                //it should allow the user to clearly see at a glance what they are about to overwrite
                while ((line = reader2.ReadLine()) != null)
                {                    
                    previewListBox.Items.Add(line);
                    SetInspectionInfo(line, counter);
                    counter++; 
                }
                //outputting the elevation to the proper textboxes for the user to see,
                //I still need to run this data through a regex to clean it up. that will be on my todo
                millTextbox.Text = millName + location;
                inspectionYearTextBox.Text = inspectionYear;
                elevationTextBox.Text = elevationName1 + elevationName2 + elevationName3;
                SectionTextBox.Text = sectionName;
                
                reader2.Close();
            }
        }
        
        //data from the datagridview is stored in an array and written to the TDS file the user has selected.
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
                     if (currentLine >= 6 && currentLine <= (tubeCount +5))
                     {
                        string value = readingValueArray[0, leftCounter];
                        //passing the cleanup method the value of the cell in the array, it will return the TDS acceptable value
                        string returnedValue = DataValidator(value);
                        
                        writer.WriteLine(returnedValue);
                        leftCounter++;
                     }//printing center readings
                    else if (currentLine > (tubeCount + 11) && currentLine <= ((tubeCount * 2) + 11))
                    {
                        string value = readingValueArray[1, centerCounter];
                        //passing the cleanup method the value of the cell in the array, it will return the TDS acceptable value
                        string returnedValue = DataValidator(value);

                        writer.WriteLine(returnedValue);
                        centerCounter++;
                    }//printing right readings
                    else if (currentLine > tubeCount * 2 + 17 && currentLine <= ((tubeCount * 3) + 17))
                     {
                        string value = readingValueArray[2, rightCounter];
                        //passing the cleanup method the value of the cell in the array, it will return the TDS acceptable value
                        string returnedValue = DataValidator(value);

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
            
            //Reset the tube count
            tubeCount = 0;

            //The following lines are for debugging only. 
            if (sublimeCheckBox.Checked)
            {
                Util.OpenInSublime(fileLocation);
            }
        }
    }
}