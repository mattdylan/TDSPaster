using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitControls();

            //todo: disabling the transpose buttons for DART and DATUM. This can be deleted once we have the information needed to transpose these comments to TDS
            datumRadioButton.Enabled = false;
            dartRadioButton.Enabled = false;
        }

        //cleaning up the many types of comments that are not TDS compatable
        public string DataValidator(string readingValue)
        {   
            //check to make sure data already contains decimals. If not, continue on with validation. If so, checks and fixes decimal placement (ie data truncated from BEAM).
            if (readingValue.Contains("."))
            {
                //This code checks to see if the reading has been truncated and repairs if it has
                int count = readingValue.Length;
                decimal floatHolder;
                //executes if the reading has had just one zero truncated
                if (count == 4)
                {
                    //adds a zero to the end of the reading by multiplying by the decimal
                    floatHolder = decimal.Parse(readingValue);
                    floatHolder = floatHolder * 1.0m;
                    readingValue = floatHolder.ToString();
                }
                //executes if the reading has had two zeroes truncated
                if (count == 3)
                {
                    floatHolder = decimal.Parse(readingValue);
                    floatHolder = floatHolder * 1.00m;
                    readingValue = floatHolder.ToString();
                }
            }

            char[] charsToTrim = {'.'};//Place any characters that should not go into the TDS file here
            
            //array to store all possible comments. Add as needed.
            char[] comments =
            {
                '+', ')', '/', '=', '<', ';', '>', '$', '(', '[', '#', '*', '@', '?', '~', '!', '%', '^',
                '&', '{', '}', '\"', '\\'
            };
            //the following two strings encase the numeric value
            string quote = "\"";
            string spaceQuote = " \"";
            //if the data is numbers with a single character the if statement will execute.           
            if (Regex.IsMatch(readingValue, @"\d"))
            {
                //todo put the trimming functions in their own method
                readingValue = readingValue.TrimStart('0'); //getting rid of any leading zeros
                readingValue = readingValue.Trim(charsToTrim);
                readingValue = quote + readingValue + spaceQuote;
            }
            //todo started on a comment transposer, need to finish
            //check if the reading contains comments
            /*else if (readingValue.IndexOfAny(comments) != -1)
            {
                //todo put the trimming functions in their own method
                readingValue = TransposeComments(readingValue);
                readingValue = readingValue.TrimStart('0'); //getting rid of any leading zeros
                readingValue = readingValue.Trim(charsToTrim);
                readingValue = quote + readingValue + spaceQuote;
            }*/
            //if the reading is not a valid reading or comment
            else
            { 
                readingValue = @"""    """;
            }
            return readingValue;
        }

        private static string RemoveNonAlpha(string line)
        {
            //Replace all multiple spaces with just one space, then remove the quotes.
            var newLine = Regex.Replace(line, @"\s+", " ");
            newLine = newLine.Replace("\"", string.Empty);
            return newLine;
        }

        //todo finish the comment transposer
        /*private string TransposeComments(string readingValue)
        {
            char[] beamComments =
            {
                '~', '!', '%', '^', '&', '{', '}', '\\', '\"'
            };
            if (beamRadioButton.Checked)
            {
                if (readingValue.IndexOfAny(beamComments) != -1)
                {
                    StringBuilder sb = new StringBuilder(readingValue);

                    readingValue =
                        sb
                            .Replace("!", "@")
                            //.Replace("2", string.Empty)
                            //.Replace("3", string.Empty)
                            .ToString();
                }
            }
            else if (datumRadioButton.Checked)
            {
                //todo code for datum transpcription.
            }
            else
            {
                //todo code for dart transcription
            }
            return readingValue;
        }*/

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

        private void WriteTdsFile(string fileLocation, string [,] readingValueArray)
        {
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
                        if (currentLine >= 6 && currentLine <= (tubeCount + 5))
                        {
                            string readingValue = readingValueArray[0, leftCounter];
                            //passing the cleanup method the value of the cell in the array, it will return the TDS acceptable value
                            string returnedValue = DataValidator(readingValue);
                            writer.WriteLine(returnedValue);
                            leftCounter++;
                        }
                        //printing center readings
                        else if (currentLine > (tubeCount + 11) && currentLine <= ((tubeCount * 2) + 11))
                        {
                            string readingValue = readingValueArray[1, centerCounter];
                            //passing the cleanup method the value of the cell in the array, it will return the TDS acceptable value
                            string returnedValue = DataValidator(readingValue);

                            writer.WriteLine(returnedValue);
                            centerCounter++;
                        }
                        //printing right readings
                        else if (currentLine > tubeCount * 2 + 17 && currentLine <= ((tubeCount * 3) + 17))
                        {
                            string readingValue = readingValueArray[2, rightCounter];
                            //passing the cleanup method the value of the cell in the array, it will return the TDS acceptable value
                            string returnedValue = DataValidator(readingValue);

                            writer.WriteLine(returnedValue);
                            rightCounter++;
                        }
                        else
                        {//not 100% sure I think this closes the stream once end of file is reached and saves it also
                            writer.WriteLine(lines[currentLine - 1]);
                        }
                    }
                }
        }

        private void InitControls()
        {

            PasteDataButton.Enabled = false;
            SaveFileButton.Enabled = false;
            rowLeftLabel.Visible = false;
            rowRightLabel.Visible = false;
            rowCenterLabel.Visible = false;
            singleRowRadioButton.Checked = true;
            commentGroupBox.Enabled = false;
        }

        private void transposeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (transposeCheckBox.Checked)
            {
                commentGroupBox.Enabled = true;
                beamRadioButton.Checked = true;
            }
            else
            {
                commentGroupBox.Enabled = false;
            }
        }

        //gathers the information for the elevation you want to write to
        private void SelectFileButton_Click(object sender, EventArgs e)
        {
            try
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
                    //This is grabbing the informaion about the elevation that has been selected to be overwritten
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
            catch (Exception ex)
            {

                throw new ApplicationException("Something went wrong with the application:", ex);
            }
        }

        //copies the data from the clipboard and other housekeeping things
        private void PasteDataButton_Click(object sender, EventArgs e)
        {
            try
            {
                //paste data from clipboard
                DataObject o = (DataObject)Clipboard.GetDataObject();
                if (o.GetDataPresent(DataFormats.Text))
                {
                    string[] pastedRows = Regex.Split(o.GetData(DataFormats.Text).ToString().TrimEnd("\r\n".ToCharArray()), "\r\n");
                    int j = 0;

                    //Manually adding the tube columns to the datagrid view (this is required before data can be added
                    if (singleRowRadioButton.Checked)
                    {
                        for (int b = 1; b <= tubeCount * 3; b++)
                        {
                            string tubeColumn = "Tube";
                            dataGridView1.Columns.Add(tubeColumn + b, b.ToString());
                        }
                    }
                    if (tripleRowRadioButton.Checked)
                    {
                        for (int b = 1; b <= tubeCount; b++)
                        {
                            string tubeColumn = "Tube";
                            dataGridView1.Columns.Add(tubeColumn + b, b.ToString());
                        }
                    }
                    //This is the code that adds the copied data to the datagrid view, not 100% on how it works.
                    foreach (string pastedRow in pastedRows)
                    {
                        string[] pastedRowCells = pastedRow.Split(new char[] { '\t' });

                        dataGridView1.Rows.Add();

                        using (DataGridViewRow dataGridView1Row = dataGridView1.Rows[j])
                        {
                            for (int i = 0; i < pastedRowCells.Length; i++)
                                dataGridView1Row.Cells[i].Value = pastedRowCells[i];
                        }
                        j++;
                    }
                    //enable the save button once data has been pasted
                    SaveFileButton.Enabled = true;
                }
                //Simple data validation. If the amount of tubes pasted does not equal the amount of tubes for the actual section
                // the user is given an alert. Lots of room for improvement here.         
                if (tubeCount * 3 != dataGridView1.ColumnCount)
                {
                    MessageBox.Show("Oh no! It seems that you have pasted an amount of tubes that does not match the tube count for the section! If you continue the program will crash!");
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Something went wrong with the appliacation:", ex);
            }
        }

        //data from the datagridview is stored in an array and written to the TDS file the user has selected.
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            try
            {
                //This large block of code executes if the user selects the triple row radio button
                if (tripleRowRadioButton.Checked)
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

                    WriteTdsFile(fileLocation, readingValueArray);

                    MessageBox.Show("If nothing caught fire the file was transfered!");

                    //Reset the tube count
                    tubeCount = 0;

                    //The following lines are for debugging only. 
                    if (sublimeCheckBox.Checked)
                    {
                        Util.OpenInSublime(fileLocation);
                    }
                }
                //this code executes when the single row radio button has been selected.
                else if (singleRowRadioButton.Checked)
                {
                    int leftCounter = 0;
                    int centerCounter = 0;
                    int rightCounter = 0;
                    //creating the array, this is dynamic, the array will match the number of columns and rows
                    string[,] readingValueArray = new string[3, tubeCount];
                    //loops through the pasted data selecting and saving every 3rd reading to the first row of the array
                    for (int left = 0; left < tubeCount * 3; left = left + 3)
                    {
                        readingValueArray[0, leftCounter] = dataGridView1.Rows[0].Cells[left].Value.ToString();
                        leftCounter++;
                    }
                    //loops through the pasted data selecting and saving every 3rd reading to the second row of the array
                    for (int center = 1; center < tubeCount * 3; center = center + 3)
                    {
                        readingValueArray[1, centerCounter] = dataGridView1.Rows[0].Cells[center].Value.ToString();
                        centerCounter++;
                    }
                    //loops through the pasted data selecting and saving every 3rd reading to the third row of the array
                    for (int right = 2; right < tubeCount * 3; right = right + 3)
                    {
                        readingValueArray[2, rightCounter] = dataGridView1.Rows[0].Cells[right].Value.ToString();
                        rightCounter++;
                    }
                    //writes the file in the TDS format
                    WriteTdsFile(fileLocation, readingValueArray);

                    MessageBox.Show("If nothing caught fire the file was transfered!");

                    //Reset the tube count
                    tubeCount = 0;

                    //The following lines are for debugging only. 
                    if (sublimeCheckBox.Checked)
                    {
                        Util.OpenInSublime(fileLocation);
                    }
                }
                else
                {
                    MessageBox.Show("Please select which format the data you pasted is in.");
                }

                //Clear the controls and reset the save file and paste buttons to the default disabled state
                Util.ClearControls(this);
                InitControls();
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Something went wrong with the appliaction:", ex);
            }
        }

        private void tripleRowRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            rowLeftLabel.Visible = true;
            rowRightLabel.Visible = true;
            rowCenterLabel.Visible = true;

            colLeftLabel.Visible = false;
            colCenterLabel.Visible = false;
            colRightLabel.Visible = false;
        }

        private void singleRowRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            rowLeftLabel.Visible = false;
            rowRightLabel.Visible = false;
            rowCenterLabel.Visible = false;

            colLeftLabel.Visible = true;
            colCenterLabel.Visible = true;
            colRightLabel.Visible = true;
        }

        private void clearAllButton_Click(object sender, EventArgs e)
        {
            try
            {
                Util.ClearControls(this);
                InitControls();
            }
            catch (Exception ex)
            {
                //generic error message, could be improved
                throw new ApplicationException("Something went wrong with the application:", ex);
            }
        }
    }
}
