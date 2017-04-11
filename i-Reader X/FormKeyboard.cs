using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace i_Reader_X
{
    public partial class FormKeyboard : Form
    {
        public FormKeyboard()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            textBoxWrite.AppendText(btn.Name.ToString().Replace("buttonNo", ""));
        }

        private void buttonMain_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn == buttonCanel)
            {
                textBoxWrite.Clear();
                textBoxWrite.Text = FormMain.Rewrite[1];
                this.Close();
            }
            else if (btn == buttonConfirm)
            {
                if (textBoxWrite.Text == "")
                {
                    textBoxWrite.Text = FormMain.Rewrite[1];
                }
                this.Close();
            }
            else if (btn == buttonDelete)
            {
                if (textBoxWrite.Text != "")
                    textBoxWrite.Text = textBoxWrite.Text.Substring(0, textBoxWrite.Text.Length - 1);
            }
            else if (btn == buttonClear)
            {
                textBoxWrite.Clear();
            }
            else if (btn == buttonDeleteSample)
            {
                textBoxWrite.Text = "Delete";
                this.Close();
            }
        }

        private void FormKeyboard_Load(object sender, EventArgs e)
        {
            if (FormMain.Rewrite[2] == "计算结果")
            {
                buttonDeleteSample.Visible = true;
            }
        }

        private void FormKeyboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormMain.Rewrite[0] = textBoxWrite.Text;
            FormMain.Rewrite[2] = "";
        }
    }
}
