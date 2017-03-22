using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Client
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if ((textBox3.Text != "" && textBox3.Text != " ") && (textBox4.Text != "" && textBox4.Text != " "))
            {
                try
                {
                    DirectoryInfo data = new DirectoryInfo("Client_info");
                    data.Create();

                    var sw = new StreamWriter(@"Client_info/data_info.txt");
                    sw.WriteLine(textBox3.Text + ":" + textBox4.Text);
                    sw.Close();

                    this.Hide();

                    Application.Restart();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }


    }
}
