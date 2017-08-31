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
    public partial class FormMessage : Form
    {
        public FormMessage()
        {
            InitializeComponent();
        }
        

        private void FormMessage_Load(object sender, EventArgs e)
        {
            labelMessage.Text = FormMain.message[0];
            if (FormMain.message[0].Substring(0, 2) == "测试")
            {
                buttonOK.Text = "是";
                buttonCancel.Text = "否";
            }
            var type = FormMain.message[1];
            if (type == "OKorCancel")
            {
                buttonCancel.Visible = true;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            FormMain.message[2] = "OK";
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            FormMain.message[2] = "Cancel";
            this.Close();
        }
    }
}
