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
            else if (btn == buttonDateConfirm)
            {
                this.Close();
            }
        }

        private void FormKeyboard_Load(object sender, EventArgs e)
        {
            panelDate.Visible = false;
            if (FormMain.Rewrite[2] == "计算结果")
            {
                buttonDeleteSample.Visible = true;
            }
            else if (FormMain.Rewrite[2] == "开始日期" | FormMain.Rewrite[2] == "结束日期")
            {
                label1.Text = FormMain.Rewrite[2];
                Size = panelDate.Size;
                panelDate.Visible = true;
                textBoxTime.Text = FormMain.Rewrite[1];
            }
        }

        private void FormKeyboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (FormMain.Rewrite[2] == "开始日期" | FormMain.Rewrite[2] == "结束日期")
            {
                FormMain.Rewrite[0] = textBoxTime.Text;
            }
            else
            {
                FormMain.Rewrite[0] = textBoxWrite.Text;
            }
            FormMain.Rewrite[2] = "";
        }

        private void buttonTimeChange_Click(object sender, EventArgs e)//
        {
            Button btn = (Button)sender;
            DateTime dt = DateTime.Parse(textBoxTime.Text);
            switch (btn.Name)
            {
                case "button_Year_Down":
                    textBoxTime.Text = dt.AddYears(-1).ToString("yyyy/MM/dd");
                    break;

                case "button_Month_Down":
                    textBoxTime.Text = dt.AddMonths(-1).ToString("yyyy/MM/dd");
                    break;

                case "button_Day_Down":
                    textBoxTime.Text = dt.AddDays(-1).ToString("yyyy/MM/dd");
                    break;

                case "button_Year_Up":
                    textBoxTime.Text = dt.AddYears(+1).ToString("yyyy/MM/dd");
                    break;

                case "button_Month_Up":
                    textBoxTime.Text = dt.AddMonths(+1).ToString("yyyy/MM/dd");
                    break;

                case "button_Day_Up":
                    textBoxTime.Text = dt.AddDays(+1).ToString("yyyy/MM/dd");
                    break;
            }
        }

        private void textBoxTime_TextChanged(object sender, EventArgs e)
        {

        }

        private void panelDate_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
