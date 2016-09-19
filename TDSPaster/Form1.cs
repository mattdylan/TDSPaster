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
        string location;
        string millName;
        string sectionName;
        string elevationName1;
        string elevationName2;
        string elevationName3;
        string boilerName;
        string inspectionYear;
        public Form1()
        {
            InitializeComponent();
            //In order to do some basic validation I require the user to select a file before the paste button is enabled
            PasteDataButton.Enabled = false;
            
        }
        //cleaning up the many types of comments that are not TDS compatable
        public string dataValidator(string readingValue)
        {   
            char[] charsToTrim = { '.', 'V' };//Place any characters that should not go into the TDS file here
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
            else
            {
                goodData = @"""    """;
                return goodData;
            }

        }

        //copys the data from the clipboard and other housekeeping things
        private void PasteDataButton_Click(object sender, EventArgs e)
        {
            
            //paste data from clipboard
            DataObject o = (DataObject)Clipboard.GetDataObject();
            if (o.GetDataPresent(DataFormats.Text))
            {
                
                string[] pastedRows = Regex.Split(o.GetData(DataFormats.Text).ToString().TrimEnd("\r\n".ToCharArray()), "\r\n");
                int j = 0;
                
                //Manually adding the tube columns to the datagrid view (this is required before data can be added
                for (int b = 1; b <= Global.GlobalVar; b++)
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
            if (Global.GlobalVar != dataGridView1.ColumnCount)
            {
                MessageBox.Show("Oh no! It seems that you have pasted an amount of tubes that does not match the tube count for the section! If you continue the program will crash!");
            }
        }

        //gathers the information for the elevation you want to write to
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
                StreamReader reader3 = new StreamReader(fileLocation);
                while ((line = reader3.ReadLine()) != null)
                {
                    counter++;
                }
                float finalTubeCount = ((counter - 18f) / 3f);
                Global.GlobalVar = finalTubeCount;
                reader3.Close();

                StreamReader reader2 = new StreamReader(fileLocation);
                //This could likly be made into a class, I just want to get it working first. Feel free to do it, or not :) haha
                counter = 0;
                //This is grabing the informaion about the elevation that has been selected to be overwritten
                //it should allow the user to clearly see at a glance what they are about to overwrite
                while ((line = reader2.ReadLine()) != null)
                {                    
                    previewListBox.Items.Add(line);
                                       
                    if (counter == 0)
                    {
                        millName = line;
                    }
                    if (counter == 1)
                    {
                        boilerName = line;
                    }
                    if (counter == 2)
                    {
                        sectionName = line;
                    }
                    if (counter == 3)
                    {
                        elevationName2 = line;
                    }
                    if (counter == 4)
                    {
                        elevationName1 = line;
                    }
                    if (counter == Global.GlobalVar +6)
                    {
                        location = line;
                    }
                    if (counter == Global.GlobalVar +7)
                    {
                        inspectionYear = line;
                    } 
                    if (counter == Global.GlobalVar +10)
                    {
                        elevationName3 = line;
                    }
                    counter++;
                }
                //outputting the elevation to the proper textboxes for the user to see,
                //I still need to run this data through a regex to clean it up. that will be on my todo
                millTextbox.Text = millName + location.ToString();
                inspectionYearTextBox.Text = inspectionYear.ToString();
                elevationTextBox.Text = elevationName1 + elevationName2 + elevationName3.ToString();
                SectionTextBox.Text = sectionName.ToString();
                
                reader2.Close();
            }
        }

        //Class that is being used to pass the tube count around the program
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
            //The following lines are for debugging only
            if (sublimeCheckBox.Checked)
            {
                
                ProcessStartInfo pi = new ProcessStartInfo(fileLocation);
                pi.Arguments = Path.GetFileName(fileLocation);
                pi.UseShellExecute = true;
                pi.WorkingDirectory = Path.GetDirectoryName(fileLocation);
                pi.FileName = "D:\\Apps\\Sublime\\Sublime Text 3\\sublime_text.exe";//you need to change your path to your sublime app
                pi.Verb = "OPEN";
                Process.Start(pi);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

