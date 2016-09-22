using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace TDSPaster
{
    class Util
    {
        //Opens the file in sublime text. Path to your sublime text is hardcoded. 
        public static void OpenInSublime(string fileLocation)
        {
            ProcessStartInfo pi = new ProcessStartInfo(fileLocation)
            {
                Arguments = Path.GetFileName(fileLocation),
                UseShellExecute = true,
                WorkingDirectory = Path.GetDirectoryName(fileLocation),
                //FileName = "D:\\Apps\\Sublime\\Sublime Text 3\\sublime_text.exe";//Dylan Path
                FileName = "D:\\Program Files\\Sublime Text 3\\sublime_text.exe",//Tyler path
                Verb = "OPEN"
            };
            Process.Start(pi);
        }

        //clears all controls on the sending form
        public static void ClearControls(Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox) c).Clear();
                }

                if (c.HasChildren)
                {
                    ClearControls(c);
                }

                if (c is CheckBox)
                {
                    ((CheckBox) c).Checked = false;
                }

                if (c is RadioButton)
                {
                    ((RadioButton) c).Checked = false;
                }

                if (c is ListBox)
                {
                    ((ListBox) c).Items.Clear();
                }

                if (c is DataGridView)
                {
                    ((DataGridView) c).DataSource = null;
                    ((DataGridView) c).Rows.Clear();
                    ((DataGridView) c).Columns.Clear();
                }
            }
        }

        public static void CloseConfirmation(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Exit Application", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }
    }
}
