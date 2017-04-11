using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;
using System.Drawing.Printing;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;
using System.Configuration;

namespace i_Reader_X
{
    public partial class FormMain : Form
    {
        //结果显示栏页数：当前页，总页数，每页个数
        public static int[] page = { 1, 0, 9 };
        //小键盘数据传输：新数据，原来数据，按键由来
        public static string[] Rewrite = { "", "" ,"" };
        //命令数据存储：荧光数据，荧光电机,二维码
        private readonly string[] commandStr = { "-1", "" ,"" };
        //荧光的数据需要8个数一组自行读取，为间断式数据，用于存储荧光数据
        private readonly List<double> _fluoData = new List<double>();
        //信息弹窗
        private FormMessage MessageShow;
        //小键盘
        private FormKeyboard Keyboard;
        //样本类型设定,0为血浆 ，1为血清
        public string[] TestType = { "0", "血浆" };
        //弹窗信息,弹窗形式,返回信息,打印信息
        public static string[] message = { "", "", "" ,""};

        private static string time = "[yyyy-MM-dd HH:mm:ss.fff]";

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            timerLoad.Start();
        }

        private static void UpdateAppConfig(string newKey, string newValue)
            //写入配置文件
        {
            var isModified = ConfigurationManager.AppSettings.Cast<string>().Any(key => key == newKey);
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (isModified)
            {
                config.AppSettings.Settings.Remove(newKey);
            }
            config.AppSettings.Settings.Add(newKey, newValue);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
        private string ConfigRead(string str)
            //读取配置文件
        {
            try
            {
                try
                {
                    str = ConfigurationManager.AppSettings[str];
                    return str;
                }
                catch (Exception)
                {

                    if (File.Exists(Application.StartupPath + @"/i-Reader S.exe.config") & File.Exists(Application.StartupPath + @"/configbackup/i-Reader S.exe.config"))
                    {
                        File.Delete(Application.StartupPath + @"/i-Reader S.exe.config");
                        File.Move(Application.StartupPath + @"/configbackup/i-Reader S.exe.config", Application.StartupPath + @"/i-Reader S.exe.config");

                        MessageBox.Show("配置文件已损坏，软件将复制备份文件到安装目录，请稍后手动启动软件或重启电脑");
                        Close();
                    }
                    else if (File.Exists(Application.StartupPath + @"/configbackup/i-Reader S.exe.config"))
                    {
                        File.Move(Application.StartupPath + @"/configbackup/i-Reader S.exe.config", Application.StartupPath + @"/i-Reader S.exe.config");
                        MessageBox.Show("配置文件不存在，软件将复制备份文件到安装目录，请稍后手动启动软件或重启电脑");
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("配置文件已损坏，请检查");
                        Close();
                    }
                    MessageBox.Show("4");

                    return "";
                }
            }
            catch (Exception ee)
            {

            }
            return "";
        }

        private string LeftCheck1(string strcmd)
        {
            var checkNum = 0;
            for (var i = 0; i < strcmd.Length; i = i + 2)
            {
                checkNum += Convert.ToInt32(strcmd.Substring(i, 2), 16);
            }
            checkNum = 256 - checkNum & 0x00FF;
            return checkNum.ToString("X2");
        }

        private void LogAdd(string project, string SorR, string str )
        {
            if (project == "Data")
            {
                textBoxData.AppendText(SorR + DateTime.Now.ToString(time) + str);
                textBoxData.AppendText("\r\n");
            }
        }

        private void timersystem_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            labelTimeDay.Text = dt.ToShortDateString().ToString();
            labelTimeSecond.Text = dt.ToLongTimeString().ToString();

            for (int i = 0; i < dataGridViewMain.RowCount; i++)
            {
                if (dataGridViewMain.Rows[i].Cells[3].Value.ToString() == "1")
                {
                    var str = dataGridViewMain.Rows[i].Cells[2].Value.ToString();
                    str = str.Substring(str.IndexOf("：", StringComparison.Ordinal) + 1);
                    if (int.Parse(str) >= 1)
                    {
                        dataGridViewMain.Rows[i].Cells[2].Value = "测试中：" + (int.Parse(str) - 1);
                    }
                    else
                    {
                        dataGridViewMain.Rows[i].Cells[3].Value = "2";
                        dataGridViewMain.Rows[i].Cells[2].Value = CalResult().ToString("f2") + "mg/L";
                        if(ConfigRead("AutoPrint") == "1")
                            PagePrint();
                    }
                }
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn == buttonClose)
            {
                this.Close();
            }
            else if (btn == buttonTest)
            {
                if (dataGridViewMain.Rows.Count != 0)
                {
                    var SampleNo = dataGridViewMain.Rows[0].Cells[0].Value.ToString();
                    var TestItemID = "1";
                    //var TestItem = dataGridViewMain.Rows[0].Cells[1].Value.ToString();
                    var Result = dataGridViewMain.Rows[0].Cells[2].Value.ToString().Replace("mg/L", "");
                    SqlData.InsertNewResult(SampleNo, DateTime.Now.ToString(), TestItemID, Result, "mg/L", "", "", "1|0", "1", "", "");
                }
                var result = CalResult();
                dataGridViewMain.Rows.Clear();
                dataGridViewMain.Rows.Add(textBoxNum.Text, textBoxItem.Text, "测试中：0", "1", DateTime.Now.ToString());
                labeltestnum.Text = "样 本 号：" + textBoxNum.Text;
                labeltestItem.Text = "测试项目：" + textBoxItem.Text;
                labelTesttype.Text = "样本类型：" + TestType[1];
                textBoxNum.Text = (long.Parse(textBoxNum.Text) + 1).ToString().PadLeft(textBoxNum.Text.Length, '0');
            }
            else if (btn == buttonReaderQR)
            {
                panelReadQR.Visible = true;
            }
            else if (btn == buttonQRCancel)
            {
                panelReadQR.Visible = false;
            }
            else if (btn == buttonChange)
            {
                if (TestType[0] == "0")
                {
                    TestType[0] = "1";
                    TestType[1] = "血清";
                    buttonChange.BackgroundImage = Resource.switch_right;
                    labelbloodX.ForeColor = Color.Black;
                    labelblood.ForeColor = Color.DarkGray;
                }
                else
                {
                    TestType[0] = "0";
                    TestType[1] = "血浆";
                    buttonChange.BackgroundImage = Resource.switch_left;
                    labelbloodX.ForeColor = Color.DarkGray;
                    labelblood.ForeColor = Color.Black;
                }
            }
            else if (btn == buttonPrint)
            {
                message[3] = "";
                PrintReadResult();
                PagePrint();
            }
            else if (btn == buttonPageDown)
            {
                if (page[0] != page[1])
                {
                    page[0] = page[0] + 1;
                    UpdataSearchResult(page[0], page[2]);
                    labelPages.Text = page[0] + "/" + page[1];
                }
            }
            else if (btn == buttonPageUp)
            {
                if (page[0] != 1)
                {
                    page[0] = page[0] - 1;
                    UpdataSearchResult(page[0], page[2]);
                    labelPages.Text = page[0] + "/" + page[1];
                }
            }
            else if (btn == buttonAutoPrintSwitch)
            {
                if (ConfigRead("AutoPrint") == "0")
                {
                    buttonAutoPrintSwitch.BackgroundImage = Resource.switch_right;
                    labelAutoPrint.Text = "自动打印开";
                    UpdateAppConfig("AutoPrint", "1");
                }
                else if (ConfigRead("AutoPrint") == "1")
                {
                    buttonAutoPrintSwitch.BackgroundImage = Resource.switch_left;
                    labelAutoPrint.Text = "自动打印关";
                    UpdateAppConfig("AutoPrint", "0");
                }
            }
            else if (btn == buttonRewrite)
            {
                this.Close();
            }
            else if (btn == buttonMin)
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }


        private double CalResult()
        {
            Random ran = new Random();
            double result = ran.Next(50, 300);
            result = Math.Pow((result * 13), 2) / 70000;
            return result;
        }


        private void buttonmenu_Click(object sender, EventArgs e)
            //页面切换
        {
            Button btn =(Button)sender;
            switch (btn.Name)
            {
                case "buttonData":
                    tabControlMain.SelectedTab = tabPageResult;
                    page[0] = 1;
                    page[1] = SqlData.SelectAllResult().Rows.Count / page[2] + 1;
                    UpdataSearchResult(page[0], page[2]);
                    labelPages.Text = page[0] + "/" + page[1];
                    break;
                case "buttonMain":
                    tabControlMain.SelectedTab = tabPageMain;
                    break;
                case "buttonSetting":
                    tabControlMain.SelectedTab = tabPageSetting;
                    break;
                case "buttonEngineer":
                    tabControlMain.SelectedTab = tabPageData;
                    break;
            }

        }

        private void PrintReadResult()
        {

            message[3] += dataGridViewMain.Rows[0].Cells[4].Value.ToString() + "|";
            message[3] += "样本号  ：" + dataGridViewMain.Rows[0].Cells[0].Value.ToString() + "|";
            message[3] += "测试项目：" + dataGridViewMain.Rows[0].Cells[1].Value.ToString() + "|";
            message[3] += "测试结果：" + dataGridViewMain.Rows[0].Cells[2].Value.ToString() + "|";
        }
        private void PagePrint()
            //打印机输出
        {
            var printDocument = new PrintDocument();
            printDocument.PrintPage += printDocument_PrintPage;
            printDocument.Print();
        }

        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
            //打印机格式
        {
            var x = 0;
            var y = 0;
            var rowGap = 10;
            var font = new Font("新宋体", 10);
            Brush brush = new SolidBrush(Color.Black);
            if (message[3] != "")
            {
                x = 10;
                var printInfos = message[3].Split('|');
                e.Graphics.DrawString("------------------------", font, brush, x, y);
                y += rowGap;
                foreach (var t in printInfos)
                {
                    e.Graphics.DrawString(t, font, brush, x, y);
                    y += 18;
                }
            }
            y -= 8;
            e.Graphics.DrawString("------------------------", font, brush, x, y);
        }
        

        private void serialPort_DataSend(SerialPort sr, string strCmd)
        {
            var frontstr = "";
            var backstr = "";
            if (sr == serialPortFluo)
            {
                frontstr = ":";
                backstr = Environment.NewLine;
            }
            else if (sr == serialPortMotor)
            {
                backstr = Environment.NewLine;
            }
            try
            {
                //荧光发送数据的规则是:0003....\r\n  :0006....\r\n
                var writeBuffer = Encoding.ASCII.GetBytes(frontstr + strCmd + backstr);
                sr.Write(writeBuffer, 0, writeBuffer.Length);
                Thread.Sleep(50);
            }
            catch (Exception ee)
            {
                LogAdd("Data", "[E]", ee.ToString());
            }
        }

        private void FluoConnect(int type)
        {
            try
            {
                //初始化需要发送的代码
                string[] fluoConnetData2 =
                {
                        ":000300000001FC", ":000300010001FB", ":000300020001FA", ":000300030001F9",
                        ":000300040001F8", ":000300050001F7", ":000300060001F6", ":000300070001F5",
                        ":000300080002F3", ":000300090001F3", ":0003000A0002F1", ":0003000B0001F1",
                        ":0003000C0002EF", ":0003000D0001EF", ":0003000E0002ED", ":0003000F0001ED",
                        ":000300100002EB", ":000300110001EB", ":000300120002E9", ":000300130001E9",
                        ":000300140001E8", ":000300150001E7", ":000300160001E6", ":000300170001E5",
                        ":000300180001E4", ":000300190001E3", ":0003001A0001E2", ":0003001B0001E1",
                        ":0003001C0001E0", ":0003001D0001DF", ":0003001E0001DE", ":0003001F0001DD",
                        ":000300200001DC"
                };
                foreach (var t in fluoConnetData2)
                {
                    serialPort_DataSend(serialPortFluo, t);
                }
                switch (type)
                {
                    // ReSharper disable once LocalizableElement
                    case 0:
                        LogAdd("Data", "[E]", "FluoConnectOK");
                        break;
                    // ReSharper disable once LocalizableElement
                    case 1:
                        LogAdd("Data", "[E]", "FluoReConnectOK");
                        break;
                    // ReSharper disable once LocalizableElement
                    case 2:
                        LogAdd("Data", "[E]", "FluoMotorReinitOK");
                        break;
                }
            }
            catch (Exception ee)
            {
                LogAdd("Data", "[E]", ee.ToString());
            }
        }
        private void serialPortFluo_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                //为-1时，不对采集的数据进行处理
                var currentline = new StringBuilder();
                while (serialPortFluo.BytesToRead > 0)
                {
                    var ch = (char)serialPortFluo.ReadByte();
                    currentline.Append(ch);
                }
                if (currentline.Length == 0) return;
                if (commandStr[0] == "-1")
                {

                }
                else
                //将采集的数据放入荧光数据字符串
                {
                    commandStr[0] += currentline.ToString();
                    serialPort_DataMakeUp(serialPortFluo);
                }
            }
            catch (Exception ee)
            {
                LogAdd("Data", "[E]", ee.ToString());
            }
        }

        private void serialPort_DataMakeUp(SerialPort sr)
        {
            if (sr == serialPortFluo)
            {
                //荧光需要采集的点数从配置文件中读取
                var fluoPointCount = int.Parse(ConfigRead("FluoParam").Split('|')[0]);
                while (commandStr[0].IndexOf(Environment.NewLine, StringComparison.Ordinal) > -1)
                {
                    var str = commandStr[0].Substring(0, commandStr[0].IndexOf(Environment.NewLine, StringComparison.Ordinal));
                    commandStr[0] = commandStr[0].Substring(commandStr[0].IndexOf(Environment.NewLine, StringComparison.Ordinal) + 2);
                    if (str.Length >= 17 & (str.Substring(0, 7) == ":000320" | str.Substring(0, 7) == ":000310") & LeftCheck1(str.Substring(1, str.Length - 3)) == str.Substring(str.Length - 2))
                    {
                        str = str.Substring(7);
                        str = str.Substring(0, str.Length - 2);
                        //对数据字符每8个提取一次作为一组数
                        while (str.Length > 0)
                        {
                            //一个数据的字符
                            var str1 = str.Substring(0, 8);
                            str = str.Substring(8);
                            //将字符依照给定规则转换成数字
                            var dFactor = ((double)Convert.ToInt32(str1, 16)) * 2500 / 0x7fffff;
                            //添加到荧光数据中
                            _fluoData.Add(dFactor);
                        }
                    }
                }
                if (_fluoData.Count == fluoPointCount)
                {
                    Invoke(new Action(() => { chartFluo.Series[0].Points.Clear(); }));
                    var str = "";
                    for (var i = 0; i < _fluoData.Count; i++)
                    {
                        var i1 = i;
                        Invoke(new Action(() => { chartFluo.Series[0].Points.AddXY(i1, _fluoData[i1]); }));
                        str += _fluoData[i] + Environment.NewLine;
                    }
                    //Invoke(new Action(() => { chartFluo.SaveImage(string.Format("{0}/FluoData/{1}.jpg", Application.StartupPath, path2), ChartImageFormat.Jpeg); }));
                    //保存数据
                    var sw = new StreamWriter(string.Format("{0}/FluoData/{1}", Application.StartupPath, DateTime.Now.ToString(time) + ".csv"), true, Encoding.ASCII);
                    sw.Write(str);
                    sw.Flush();
                    sw.Close();

                    //提取完毕对数据进行运算，得到荧光OD详细数据
                    //var odData = CalMethods.CalFluo(_fluoData);
                    //清空荧光数据
                    _fluoData.Clear();
                    //初始化荧光字符串
                    /*commandStr[0] = "-1";
                    if (odData.IndexOf("Error", StringComparison.Ordinal) > -1)
                    {
                        Invoke(new Action(() => Log_Add(odData, true)));

                        DrawResult("-10", _otherStr[0], odData, "", "");
                        return;
                    }
                    //提取cx cy tx ty sumtbase sumcbase用于计算
                    var cy = odData.Substring(odData.IndexOf("C(", StringComparison.Ordinal) + 2);
                    var ty = odData.Substring(odData.IndexOf("T(", StringComparison.Ordinal) + 2);
                    var cx = cy.Substring(0, cy.IndexOf(",", StringComparison.Ordinal));
                    var tx = ty.Substring(0, ty.IndexOf(",", StringComparison.Ordinal));
                    var sumCBase =
                        odData.Substring(odData.IndexOf("SumCBase(", StringComparison.Ordinal) + 9);
                    var sumTBase =
                        odData.Substring(odData.IndexOf("SumTBase(", StringComparison.Ordinal) + 9);
                    cy = cy.Substring(cy.IndexOf(",", StringComparison.Ordinal) + 1);
                    ty = ty.Substring(ty.IndexOf(",", StringComparison.Ordinal) + 1);
                    cy = cy.Substring(0, cy.IndexOf(")", StringComparison.Ordinal));
                    ty = ty.Substring(0, ty.IndexOf(")", StringComparison.Ordinal));
                    sumCBase = sumCBase.Substring(0, sumCBase.IndexOf(")", StringComparison.Ordinal));
                    sumTBase = sumTBase.Substring(0, sumTBase.IndexOf(")", StringComparison.Ordinal));
                    Invoke(new Action(() =>
                    {
                        // ReSharper disable once LocalizableElement
                        Log_Add(string.Format(@"{0}^{1}", odData, _otherStr[0]), false);
                        labelResult.Text = string.Format(@"C({0},{1}),T({2},{3});TX-CX={4}", cx, cy, tx, ty, int.Parse(tx) - int.Parse(cx));
                        if (buttonFluoFix.BackColor == Color.LightGray)
                        {
                            if (Math.Abs(int.Parse(cy.Substring(0, cy.Length - 3)) - int.Parse(textBoxFluoRef.Text)) > 20)
                            {
                                //#4167:LED1Current
                                var param = ((int.Parse(textBoxFluoRef.Text) - int.Parse(cy.Substring(0, cy.Length - 3))) / 12 + int.Parse(ConfigRead("FluoParam").Split('|')[18]));
                                param = Math.Min(214, Math.Max(param, 24));
                                FluoCmd("#4167:LED1Current$" + param);
                                Thread.Sleep(100);
                                FluoNewTest("0");
                            }
                            else
                            {
                                buttonFluoFix.BackColor = Color.Transparent;
                            }
                        }
                    }));
                    //进行结果计算与存储
                    DrawFluoResult(cy, sumCBase, sumTBase, _otherStr[0], odData, path, path2);
                    */
                }
            }
            else if (sr == serialPortMotor)
            {
                try
                {
                    //荧光电机数据以\r\n结尾
                    while (commandStr[1].IndexOf(Environment.NewLine, StringComparison.Ordinal) > -1)
                    {
                        var str = commandStr[1].Substring(0, commandStr[1].IndexOf(Environment.NewLine, StringComparison.Ordinal));
                        commandStr[1] = commandStr[1].Substring(commandStr[1].IndexOf(Environment.NewLine, StringComparison.Ordinal) + 2);
                        //采集数据完毕时需执行读取数据操作
                        if (str == "FLUOMotorRunOK")
                        {
                            //此时需将荧光数据字符串有-1改成空以便执行读取操作
                            commandStr[0] = "";
                            var fluoPointCount = int.Parse(ConfigRead("FluoParam").Split('|')[0]);
                            //读取的荧光以8个点为一组，命令开头为513，513+16后跟上点数*2 都以x4格式
                            for (var i = 0; i < (fluoPointCount - 1) / 8 + 1; i++)
                            {
                                var strcmd = "0003" + (513 + i * 16).ToString("X4");
                                strcmd += i == (fluoPointCount - 1) / 8
                                    ? ((fluoPointCount - 8 * i) * 2).ToString("X4")
                                    : 16.ToString("X4");
                                //加上校验符号
                                strcmd += LeftCheck1(strcmd);
                                //发送读取命令
                                serialPort_DataSend(serialPortFluo, strcmd);
                            }
                        }
                        else if (str == "FLUOMotorRunRx")
                        {
                            serialPort_DataSend(serialPortFluo, "000602000001F7");
                            serialPort_DataSend(serialPortFluo, "000301000002FA");
                            serialPort_DataSend(serialPortFluo, "000301000002FA");
                            serialPort_DataSend(serialPortFluo, "000301000002FA");
                            serialPort_DataSend(serialPortFluo, "000302000001FA");
                        }
                    }
                }
                catch (Exception ee)
                {
                    LogAdd("Data", "[E]", ee.ToString());
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FluoConnect(1);
        }

        
        private void textBox_Click(object sender, EventArgs e)
        {
            TextBox txb = (TextBox)sender;
            Rewrite[1] = txb.Text;
            var str = UseKeyBoard();
            txb.Text = str;
            label1.Focus();
        }

        private string UseKeyBoard()
            //打开小键盘
        {
            Keyboard = new FormKeyboard { Owner = this };
            Keyboard.ShowDialog();
            return Rewrite[0];
        }

        private string MessageBox_Show(string Message, bool Choice)
            //信息弹窗
        {
            message[0] = Message;
            if (Choice == true)
            {
                message[1] = "OKorCancel";
            }
            MessageShow = new FormMessage { Owner = this };
            MessageShow.StartPosition = FormStartPosition.CenterScreen;
            MessageShow.ShowDialog();
            message[0] = "";
            
            return message[2];
        }

        private void UpdataSearchResult(int page, int resultnum)
            //显示结果
        {
            DataTable tb = new DataTable();
            tb = SqlData.SelectResult(page, resultnum);
            dataGridViewResult.DataSource = tb;
            dataGridViewResult.Columns[0].Width = 200;
            dataGridViewResult.Columns[0].HeaderCell.Value = "样本号";
            dataGridViewResult.Columns[1].Width = 149;
            dataGridViewResult.Columns[1].HeaderCell.Value = "测试项目";
            dataGridViewResult.Columns[2].Width = 148;
            dataGridViewResult.Columns[2].HeaderCell.Value = "测试结果";
            dataGridViewResult.Columns[3].Visible = false;
        }

        private void serialPortQR_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            while (serialPortQR.BytesToRead > 0)
            {
                var ch = (char)serialPortQR.ReadByte();
                commandStr[2] += ch.ToString();
                if (ch == '\r')
                {
                    var strTemp = commandStr[2];
                    //Invoke(new Action(() => PortLog("QR", "R", strTemp)));
                    commandStr[2] = "";
                    //对一条二维码数据进行处理
                    serialPort_DataDeal(strTemp, "QR");
                }
            }
        }


        private void serialPort_DataDeal(string strCmd, string type)
        {
            if (strCmd.Length == 84)
            {
                Base64Decode(strCmd);
            }
            else
            {
                LogAdd("Data", "[R]", "二维码长度出错");
            }
        }

        private void Base64Decode(string str)
        {
            try
            {
                //将二维码字符串转为可用的byte数组
                var outputb = Convert.FromBase64String(str);
                //第一个字节保留
                //第二个字节的前5位表示0-31的产品编号
                var a = Convert.ToString(outputb[1], 2).PadLeft(8, '0') + Convert.ToString(outputb[2], 2).PadLeft(8, '0');
                var cpmc = Convert.ToInt32(a.Substring(0, 5), 2).ToString();
                //第三字节的第2到第5位表示0-15*3月的保质期
                var bzs = Convert.ToInt32(a.Substring(9, 4), 2).ToString();
                //第二字节的第6-8位表示公式类型0-7
                var funtype = Convert.ToInt32(a.Substring(13), 2).ToString();
                //第四第五字节表示批号新系
                var lot = (Convert.ToInt32(outputb[3]) * 256 + Convert.ToInt32(outputb[4])).ToString();
                a = Convert.ToString(outputb[5], 2).PadLeft(8, '0') + Convert.ToString(outputb[6], 2).PadLeft(8, '0');
                //第6第7字节表示生产日期，前7位为年份+2000，接着4位为月份，最后5位为日
                var year = Convert.ToInt32(a.Substring(0, 7), 2);
                var month = Convert.ToInt32(a.Substring(7, 4), 2);
                var day = Convert.ToInt32(a.Substring(11), 2);
                //第8字节开始后的56个字节，没8个字节对应一个double数字，共7个参数
                var param = new double[7];
                for (var i = 0; i < 7; i++)
                {
                    var aa = new byte[8];
                    Array.Copy(outputb, 7 + i * 8, aa, 0, 8);
                    param[i] = BitConverter.ToDouble(aa, 0);
                }

                var dtPdate = DateTime.Parse(string.Format("{0}-{1}-{2}", year, month, day));
                var epdate = dtPdate.AddMonths(3 * int.Parse(bzs));
                //2016年1月11之前规则：+15月的前一月最后一天。  2016年1月11号改为+15月-1天
                epdate = dtPdate < DateTime.Parse("2016-01-11 00:00:00") ? epdate.AddDays(-epdate.Day) : epdate.AddDays(-1);
                //将定标信息插入到定标数据表中
                var calibdataid = SqlData.InsertIntoCalibdata(cpmc, lot, dtPdate, epdate, funtype, param);
                //
                var strCalibdataid = calibdataid == 0
                    ? "(select max(CalibDataID)  from CalibData)"
                    : calibdataid.ToString();
                /*
                if (calibdataid > -1)
                {
                    var labelQrText = Resources.labelQRText;
                    labelQrText = labelQrText?.Replace("[m]", lot);
                    var productName = SqlData.SelectTestItemNameById(cpmc);
                    labelQrText = labelQrText?.Replace("[n]", productName);
                    var reagentStoreId = "";
                    Invoke(new Action(() =>
                    {
                        labelQR.Text = labelQrText;
                        labelQR2.Text = labelQrText;
                        reagentStoreId = labelReagentOperation.Text.Substring(labelReagentOperation.Text.IndexOf("#", StringComparison.Ordinal) + 1, 1);
                    }));
                    SqlData.UpdateReagentCalibData(strCalibdataid, reagentStoreId);
                    UpdateReagentStore();
                }
                else
                    Invoke(new Action(() => labelQR.Text = Resources.QRError));*/
            }
            catch (Exception)
            {
                //Invoke(new Action(() => labelQR.Text = Resources.QRError));
            }
        }
        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    DataGridView dgv = (DataGridView)sender;
                    if (dgv == dataGridViewMain)
                    {
                        Rewrite[1] = dataGridViewMain.Rows[0].Cells[0].Value.ToString();
                        Rewrite[2] = "计算结果";
                        var str = UseKeyBoard();
                        if (str == "Delete")
                        {
                            MessageBox_Show("是否删除测试样本", true);
                            if (message[2] == "OK")
                            {
                                dataGridViewMain.Rows.Clear();
                            }
                        }
                        else
                        {
                            dataGridViewMain.Rows[0].Cells[0].Value = Rewrite[0];
                        }
                    }
                }
                catch
                {

                }
            }
        }

        private void timerLoad_Tick(object sender, EventArgs e)
        {
            timerLoad.Stop();
            var fluoEnable = ConfigRead("FluoEnable");
            if (fluoEnable == "1")
            {
                //荧光端口
                try
                {
                    serialPortFluo.PortName = ConfigRead("FluoPort");
                    serialPortFluo.Open();
                    FluoConnect(0);
                    LogAdd("Data", "[s]", "荧光串口打开");
                }
                catch (Exception ee)
                {
                    // ReSharper disable once ResourceItemNotResolved
                    //initStr += Resources.D0003_4 + Environment.NewLine;
                    LogAdd("Data", "[E]", ee.ToString());
                }
                //荧光电机端口
                try
                {
                    serialPortMotor.PortName = ConfigRead("FluoMotorPort");
                    serialPortMotor.Open();
                    serialPort_DataSend(serialPortMotor, "010611");
                    LogAdd("Data", "[s]", "荧光电机串口打开");
                }
                catch (Exception ee)
                {
                    // ReSharper disable once ResourceItemNotResolved
                    // initStr += Resources.D0003_5 + Environment.NewLine;
                    LogAdd("Data", "[E]", ee.ToString());
                }
            }
            var AutoPrint = ConfigRead("AutoPrint");
            if (AutoPrint == "1")
            {
                buttonAutoPrintSwitch.BackgroundImage = Resource.switch_left;
                labelAutoPrint.Text = "自动打印开";
            }

            buttonMain.Visible = true;
            buttonSetting.Visible = true;
            buttonData.Visible = true;
        }
    }
}
