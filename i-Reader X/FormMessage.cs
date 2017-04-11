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
