using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FacultyListCreator
{
    public partial class Form1 : Form
    {
        private string facultyName;
        private string deansName;
        private string abbreviation;

        public Form1()
        {
            InitializeComponent();
        }

        private void facultyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 editForm = new Form2("faculty name");
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                facultyName = editForm.getValue();
                textBox1.Text = facultyName;
            }
        }

        private void deansNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 editForm = new Form2("dean's name");
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                deansName = editForm.getValue();
                textBox2.Text = deansName;
            }
        }

        private void abbreviationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 editForm = new Form2("abbreviation");
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                abbreviation = editForm.getValue();
                textBox3.Text = abbreviation;
            }
        }

        private bool formsNotNullOrEmpty()
        {
            string outputFacultyName = textBox1.Text;
            string outputDeansName = textBox2.Text;
            string outputAbbreviation = textBox3.Text;

            if (!string.IsNullOrEmpty(outputFacultyName) && !string.IsNullOrEmpty(outputDeansName) && !string.IsNullOrEmpty(outputAbbreviation))
            {
                return true;
            }
            MessageBox.Show("Одно или несколько полей не заполнены. Повторите попытку.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        private void appendToFile_Click(object sender, EventArgs e)
        {
            string outputFileName = textBox4.Text;
            string[] extensionsArray = { ".txt", ".doc", ".rtf"};

            if (!string.IsNullOrEmpty(outputFileName) && formsNotNullOrEmpty())
            {
                if (extensionsArray.Any(ext => outputFileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
                {
                    using (StreamWriter writer = new StreamWriter(outputFileName, true, Encoding.Unicode))
                    {
                        writer.WriteLine("Факультет: " + facultyName);
                        writer.WriteLine("Декан: " + deansName);
                        writer.WriteLine("Аббревиатура: " + abbreviation);
                        writer.WriteLine("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");
                    }
                    MessageBox.Show("Информация добавлена в файл. Файл находится по пути 'Faculty List Creator/bin/Debug' ", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Неподдерживаемое расширение файла. Пожалуйста, используйте одно из следующих: " + string.Join(", ", extensionsArray), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (string.IsNullOrEmpty(outputFileName) && !formsNotNullOrEmpty())
            {
                MessageBox.Show("Введите имя выходного файла.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (string.IsNullOrEmpty(outputFileName))
            {
                MessageBox.Show("Введите имя выходного файла.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
