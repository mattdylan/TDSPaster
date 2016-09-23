using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TDSPaster
{
    public partial class MainForm : Form
    {
        private string _fileLocation;
        private string _location;
        private string _millName;
        private string _sectionName;
        private string _elevationName1;
        private string _elevationName2;
        private string _elevationName3;
        private string _boilerName;
        private string _inspectionYear;

        private int _tubeCount;
        private int _pastedRowLength;

        private bool _isSingle = true;

        public MainForm()
        {
            InitializeComponent();
            //open the form on center screen
            CenterToScreen();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitControls();
            elevationOverwriteLabel.ForeColor = Color.Red;
        }
        
        //error message method for tube count error
        private string ErrMsgTubeCount(int tubeCompare)
        {
            float pastedTubeCount = (float)_pastedRowLength;
            string invalidTubeCountSingleRowMsg = ("The pasted data tube count does not match the selected file tube count." +
                            "\n" + "Execution will not continue." +
                            "\n" + "Selected File Tube Count:" + _tubeCount +
                            "\n" + "Pasted Data Tube Count:" + pastedTubeCount / 3f);
            string invalidTubeCountTripleRowMsg = ("The pasted data tube count does not match the selected file tube count." +
                            "\n" + "Execution will not continue." +
                            "\n" + "Selected File Tube Count:" + _tubeCount +
                            "\n" + "Pasted Data Tube Count:" + pastedTubeCount);
            if (tubeCompare == 1)
            {
                return invalidTubeCountSingleRowMsg;
            }
            return invalidTubeCountTripleRowMsg;
        }

        //checking to see if the pasted tube count matches selected file tubecount
        private int TubeCountComparison()
        {    
            //casting variables so division works properly        
            float tubeCountFloat = (float)_tubeCount;
            float pastedTubeCount = (float)_pastedRowLength;
            pastedTubeCount = pastedTubeCount / 3.00f;
            //executes if single row pasted data does not match selected file
            if (singleRowRadioButton.Checked && (pastedTubeCount != tubeCountFloat))
            {
                int failSingleRow = 1;
                return failSingleRow;
            }
            //executes if tripple row pasted data does not match selected file
            else if (tripleRowRadioButton.Checked && (_tubeCount != _pastedRowLength))
            {
                int failTripleRow = 2;
                return failTripleRow;
            }
            int passedTest = 3;
            return passedTest;
        }

        //this method sets the inspection info into variables for display
        private void SetInspectionInfo(string line, int counter)
        {
            string newLine;

            if (counter == 0)
            {
                newLine = DataValidation.RemoveNonAlpha(line);
                _millName = newLine;
            }
            if (counter == 1)
            {
                newLine = DataValidation.RemoveNonAlpha(line);
                _boilerName = newLine;
            }
            if (counter == 2)
            {
                newLine = DataValidation.RemoveNonAlpha(line);
                _sectionName = newLine;
            }
            if (counter == 3)
            {
                newLine = DataValidation.RemoveNonAlpha(line);
                _elevationName2 = newLine;
            }
            if (counter == 4)
            {
                newLine = DataValidation.RemoveNonAlpha(line);
                _elevationName1 = newLine;
            }
            if (counter == _tubeCount +6)
            {
                newLine = DataValidation.RemoveNonAlpha(line);
                _location = newLine;
            }
            if (counter == _tubeCount +7)
            {
                newLine = DataValidation.RemoveNonAlpha(line);
                _inspectionYear = newLine;
            } 
            if (counter == _tubeCount +10)
            {
                newLine = DataValidation.RemoveNonAlpha(line);
                _elevationName3 = newLine;
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
                    if (currentLine >= 6 && currentLine <= (_tubeCount + 5))
                    {
                        string readingValue = readingValueArray[0, leftCounter];
                        //passing the cleanup method the value of the cell in the array, it will return the TDS acceptable value
                        string returnedValue = DataValidation.DataValidator(readingValue);
                        writer.WriteLine(returnedValue);
                        leftCounter++;
                    }
                    //printing center readings
                    else if (currentLine > (_tubeCount + 11) && currentLine <= ((_tubeCount * 2) + 11))
                    {
                        string readingValue = readingValueArray[1, centerCounter];
                        //passing the cleanup method the value of the cell in the array, it will return the TDS acceptable value
                        string returnedValue = DataValidation.DataValidator(readingValue);

                        writer.WriteLine(returnedValue);
                        centerCounter++;
                    }
                    //printing right readings
                    else if (currentLine > _tubeCount * 2 + 17 && currentLine <= ((_tubeCount * 3) + 17))
                    {
                        string readingValue = readingValueArray[2, rightCounter];
                        //passing the cleanup method the value of the cell in the array, it will return the TDS acceptable value
                        string returnedValue = DataValidation.DataValidator(readingValue);

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

        //Initiates controls enabled / visible state upon start and for re-run
        private void InitControls()
        {

            PasteDataButton.Enabled = false;
            SaveFileButton.Enabled = false;
            rowLeftLabel.Visible = false;
            rowRightLabel.Visible = false;
            rowCenterLabel.Visible = false;
            singleRowRadioButton.Checked = true;
        }

        //clears data pasted in the gridview
        private void ClearPastedData()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
        }

        //check to make sure the file is a valid TDS file
        private bool IsTdsFile(string fileLocation)
        {
            //get the file name without the extension
            string fileName = Path.GetFileNameWithoutExtension(fileLocation);
            //get the file path and remove the . in front
            string path = Path.GetExtension(fileLocation).Replace(".", "");
            //try to parse the string to see if it is an int
            int num;
            bool isNumeric = int.TryParse(path, out num);

            bool isTdsFormat = fileName.Trim().Length == 6;
            //has numeric file extension and 6 letter file-name
            if (isNumeric && isTdsFormat)
            {
                return true;
            }
            //not a TDS file
            return false;
        }

        private void PasteData()
        {
            try
            {
                //clear any previously pasted data
                ClearPastedData();
                //check which button is checked and set the columns accordingly. Must be done in order for pasted data to work.
                if (singleRowRadioButton.Checked)
                {
                    for (int b = 1; b <= _tubeCount * 3; b++)
                    {
                        string tubeColumn = "Tube";
                        dataGridView1.Columns.Add(tubeColumn + b, b.ToString());
                    }
                }

                if (tripleRowRadioButton.Checked)
                {
                    for (int b = 1; b <= _tubeCount; b++)
                    {
                        string tubeColumn = "Tube";
                        dataGridView1.Columns.Add(tubeColumn + b, b.ToString());
                    }
                }

                //paste data from clipboard
                DataObject o = (DataObject)Clipboard.GetDataObject();
                if (o != null && o.GetDataPresent(DataFormats.Text))
                {
                    string[] pastedRows =
                        Regex.Split(o.GetData(DataFormats.Text).ToString().TrimEnd("\r\n".ToCharArray()), "\r\n");
                    int j = 0;

                    //This is the code that adds the copied data to the datagrid view, not 100% on how it works.
                    foreach (string pastedRow in pastedRows)
                    {
                        string[] pastedRowCells = pastedRow.Split(new char[] { '\t' });
                        _pastedRowLength = pastedRowCells.Length;
                        //this is needed to verify that the data pasted matches the selected file
                        dataGridView1.Rows.Add();

                        using (DataGridViewRow dataGridView1Row = dataGridView1.Rows[j])
                        {
                            for (int i = 0; i < pastedRowCells.Length; i++)
                                dataGridView1Row.Cells[i].Value = pastedRowCells[i];
                        }
                        j++;
                    }
                }
                //checks to see if the the singleColumn radio button is checked, and if so, sees if there is more than 1 row of data that was pasted. if so, it clears the data and throws an error message. this is inefficient (the check should be BEFORE the data is pasted, but it works for now. 
                if (_isSingle && dataGridView1.Rows.Count > 1)
                {
                    ClearPastedData();
                    MessageBox.Show(
                        "The data does not seem to be in single row format. Please make sure that your data is in one single row like this:\n\nLCRLCRLCR");
                }
                else
                {
                    //enable the save button once data has been pasted
                    SaveFileButton.Enabled = true;

                    //resize the columns to fit the data entered
                    dataGridView1.AutoResizeColumns();
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show("The data does not seem to be in triple row format. Please make sure that your data is in three rows like this:\n\nLCR\nLCR\nLCR\n\n" + ex.Message);
                ClearPastedData();
            }
            catch (Exception ex)
            {
                //generic error message, could be improved
                MessageBox.Show(ex.Message);
            }
        }

        //overiding the key press event for ctrl+v to allow user to paste using the shortcut. Only works when the paste button has been enabled
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control && PasteDataButton.Enabled)
            {
                PasteData();
            }
            base.OnKeyDown(e);
        }

        private void superSecret()
        {
            SoundPlayer simpleSound = new SoundPlayer(Properties.Resources.superSecret);
            simpleSound.Play();
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
                    // clear the list view items before writing so that subsequent file selection doesn't keep populating the listbox. Also clear pasted data if any exists.
                    elevationInfoListBox.Items.Clear();
                    previewListBox.Items.Clear();
                    ClearPastedData();

                    // Open the selected file to read.
                    Stream fileStream = openFileDialog1.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        string filename = openFileDialog1.FileName;
                        _fileLocation = filename;
                    }
                    fileStream.Close();

                    //check to make sure that the file path is not null and that the file is a valid TDS file
                    if (_fileLocation != null && IsTdsFile(_fileLocation))
                    {
                        PasteDataButton.Enabled = true;

                        // Code that is going to fetch the tube count and other goodies!
                        string line;
                        int counter = 0;
                        StreamReader reader3 = new StreamReader(_fileLocation);
                        while (reader3.ReadLine() != null)
                        {
                            counter++;
                        }
                        int finalTubeCount = (counter - 18) / 3;
                        _tubeCount = finalTubeCount;
                        reader3.Close();

                        StreamReader reader2 = new StreamReader(_fileLocation);
                        counter = 0;

                        //This is grabbing the informaion about the elevation that has been selected to be overwritten
                        //it should allow the user to clearly see at a glance what they are about to overwrite
                        while ((line = reader2.ReadLine()) != null)
                        {
                            previewListBox.Items.Add(line);
                            SetInspectionInfo(line, counter);
                            counter++;
                        }

                        //outputting the elevation to the listview for the user to see,
                        elevationInfoListBox.Items.Add("Mill:\t" + _millName + _location);
                        elevationInfoListBox.Items.Add("");
                        elevationInfoListBox.Items.Add("Boiler:\t" + _boilerName);
                        elevationInfoListBox.Items.Add("");
                        elevationInfoListBox.Items.Add("Year:\t" + _inspectionYear);
                        elevationInfoListBox.Items.Add("");
                        elevationInfoListBox.Items.Add("Section:\t" + _sectionName);
                        elevationInfoListBox.Items.Add("");
                        elevationInfoListBox.Items.Add("Elevation: " + _elevationName1 + _elevationName2 + _elevationName3);
                        elevationInfoListBox.Items.Add("");
                        elevationInfoListBox.Items.Add("# Tubes:\t" + _tubeCount);//hastag tubes!

                        reader2.Close(); 
                    }
                    else
                    {
                        //could prolly make these message boxes more appealing
                        MessageBox.Show("Please select a valid TDS file. TDS data files should look like this:\n\nXXXXXX.1 OR XXXXX1.1");
                    }
                }
            }
            catch (Exception ex)
            {
                //generic error message, could be improved
                MessageBox.Show(ex.Message);
            }
        }
        
        //pastes the data from the clipboard and other housekeeping things
        private void PasteDataButton_Click(object sender, EventArgs e)
        {
            PasteData();
        }

        //data from the datagridview is stored in an array and written to the TDS file the user has selected.
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (superSecretCheckbox.Checked)
                {
                    superSecret();
                }

                //message if data is successfully transferred
                string successMessage = "Successfully pasted data into:\n\n" + _millName + _location + "\n" + _boilerName + "\n" +_elevationName1 + _elevationName2 + _elevationName3 + "\n" + _inspectionYear;

                //This large block of code executes if the user selects the triple row radio button
                if (tripleRowRadioButton.Checked)
                {
                    //validating that the selected file tube count matches the pasted data, for more info check methods above
                    int tubeCompare = TubeCountComparison();
                    if (tubeCompare == 2) { string errMSG = ErrMsgTubeCount(tubeCompare); MessageBox.Show(errMSG); return; }
                    
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

                    WriteTdsFile(_fileLocation, readingValueArray);

                    MessageBox.Show(successMessage);

                    //Reset the tube count
                    _tubeCount = 0;

                    //The following lines are for debugging only. 
                    if (sublimeCheckBox.Checked)
                    {
                        Util.OpenInSublime(_fileLocation);
                    }
                }
                //this code executes when the single row radio button has been selected.
                else if (singleRowRadioButton.Checked)
                {
                    //validating that the selected file tube count matches the pasted data, for more info check methods above
                    int tubeCompare = TubeCountComparison();
                    if (tubeCompare == 1) { string errMsg = ErrMsgTubeCount(tubeCompare); MessageBox.Show(errMsg); return; }

                    int leftCounter = 0;
                    int centerCounter = 0;
                    int rightCounter = 0;
                    //creating the array, this is dynamic, the array will match the number of columns and rows
                    string[,] readingValueArray = new string[3, _tubeCount];
                    //loops through the pasted data selecting and saving every 3rd reading to the first row of the array
                    for (int left = 0; left < _tubeCount * 3; left = left + 3)
                    {
                        readingValueArray[0, leftCounter] = dataGridView1.Rows[0].Cells[left].Value.ToString();
                        leftCounter++;
                    }
                    //loops through the pasted data selecting and saving every 3rd reading to the second row of the array
                    for (int center = 1; center < _tubeCount * 3; center = center + 3)
                    {
                        readingValueArray[1, centerCounter] = dataGridView1.Rows[0].Cells[center].Value.ToString();
                        centerCounter++;
                    }
                    //loops through the pasted data selecting and saving every 3rd reading to the third row of the array
                    for (int right = 2; right < _tubeCount * 3; right = right + 3)
                    {
                        readingValueArray[2, rightCounter] = dataGridView1.Rows[0].Cells[right].Value.ToString();
                        rightCounter++;
                    }
                    //writes the file in the TDS format
                    WriteTdsFile(_fileLocation, readingValueArray);

                    MessageBox.Show(successMessage);

                    //Reset the tube count
                    _tubeCount = 0;

                    //The following lines are for debugging only. 
                    if (sublimeCheckBox.Checked)
                    {
                        Util.OpenInSublime(_fileLocation);
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
                //generic error message, could be improved
                MessageBox.Show(ex.Message);
            }
        }

        //shows/hides the column / row labels based on radio selection
        private void tripleRowRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            rowLeftLabel.Visible = true;
            rowRightLabel.Visible = true;
            rowCenterLabel.Visible = true;

            colLeftLabel.Visible = false;
            colCenterLabel.Visible = false;
            colRightLabel.Visible = false;

            //when triple radio button is selected, set this bool to false. used to make sure user doesn't paste single row data when triple row is expected.
            _isSingle = false;
        }

        //shows/hides the column / row labels based on radio selection
        private void singleRowRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            rowLeftLabel.Visible = false;
            rowRightLabel.Visible = false;
            rowCenterLabel.Visible = false;

            colLeftLabel.Visible = true;
            colCenterLabel.Visible = true;
            colRightLabel.Visible = true;

            //when single radio button is selected, set this bool to false. used to make sure user doesn't paste triple row data when single row is expected.
            _isSingle = true;
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
                MessageBox.Show(ex.Message);
            }
        }

        private void clearPastedButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count != 0)
                {
                    ClearPastedData();
                }
                else
                {
                    MessageBox.Show("There is no pasted data.");
                }
            }
            catch (Exception ex)
            {
                //generic error message, could be improved
                MessageBox.Show(ex.Message);
            }
        }
    }
}
