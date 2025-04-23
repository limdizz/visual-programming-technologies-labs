using System;
using System.Windows.Forms;

namespace FacultyListCreator
{
    public partial class Form2 : Form
    {
        public Form2(string labelText)
        {
            InitializeComponent();
            label1.Text = "New " + labelText + ":";
        }

        public string getValue()
        {
            return textBox1.Text;
        }

        private void ok_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
