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
        }

        private void PasteDataButton_Click(object sender, EventArgs e)
        {//paste data from clipboard
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
        }

        private void SelectFileButton_Click(object sender, EventArgs e)
        {
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
                System.IO.Stream fileStream = openFileDialog1.OpenFile();

                using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
                {
                    // Read the first line from the file and write it the textbox.
                    //previewListBox.Items.Add(reader.ReadLine());


                    string filename = openFileDialog1.FileName;

                    fileLocation = filename;

                    //textBox2.Text = filename;

                    //folderLocation = Path.GetDirectoryName(filename);
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
            StreamWriter sW = new StreamWriter(fileLocation);

            //string lines = "";
            //string data = "";

            /*for(int row = 2; row <=4; row++ )
            {
                for (int col = 2; col <= (int)Global.GlobalVar; col++)
            {

                    data += (string.IsNullOrEmpty(data) ? " ": ", ") + dataGridView1.Rows[row].Cells[col].Value.ToString();
                    sW.WriteLine(dataGridView1.Rows[row].Cells[col].Value.ToString());

                }
                sW.WriteLine("will this even fucking work?");
            }*/


        sW.WriteLine("will this even fucking work?");
            sW.WriteLine(dataGridView1.Rows[3].Cells[3].Value.ToString());

            sW.Close();
        }
    }
}

