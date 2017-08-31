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
using i_Reader_X.Properties;
using System.Runtime.InteropServices;
using System.Globalization;

namespace i_Reader_X
{
    public partial class FormMain : Form
    {

        [DllImport("user32.dll", EntryPoint = "ShowCursor", CharSet = CharSet.Auto)]
        public extern static void ShowCursor(int status);
        //结果显示栏页数：当前页，总页数，每页个数
        public static int[] page = { 1, 0, 5 };
        //自己定义的全局变量
        public static string  qulot = "";
        //电池电量初始值
        public int powerbefore = 100;
        public bool isfirst = true;
        //小键盘数据传输：新数据，原来数据，按键由来
        public static string[] Rewrite = { "", "", "" };
        //命令数据存储：荧光数据，荧光电机,定标二维码，样本号
        private readonly string[] commandStr = { "", "", "","" };
        //荧光的数据需要8个数一组自行读取，为间断式数据，用于存储荧光数据
        private readonly List<double> _fluoData = new List<double>();
        //信息弹窗
        private FormMessage MessageShow;
        //小键盘
        private FormKeyboard Keyboard;
        //样本类型设定,0为全血 ，1为血清
        public string[] TestType = { "0", "全血" };
        //弹窗信息,弹窗形式,返回信息,打印信息
        public static string[] message = { "", "", "", "" };
        //荧光测试的seq，打印信息，CCDSeq
        private readonly string[] _otherStr = { "", "", "" };
        //用于确定弹窗形式：0.确认是普通形式还是OkCancel形式的弹窗 1.右上角弹窗时间 2.记录返回信息
        public static string[] MessageType = { "", "3", "" };
        private static string time = "[yyyy-MM-dd HH:mm:ss.fff]";
        //searchcondition 查询列表条件
        private string _searchcondition = string.Format(" createtime between '{0:yyyy-MM-dd 00:00:00}' and '{1:yyyy-MM-dd 23:59:59}' ", DateTime.Today.AddDays(-7), DateTime.Now);
        //鼠标位置{[X],[Y]}
        private int[] Cursor_Point = { 0, 0 };
        //用于触摸键盘传值
        public static string CounterText = "";
        //用于读卡判断
        private int QRstate = 1;
        //[0]温湿度命令判断,[1]温湿度命令计数，[2]休眠剩余时间
        private int[] CommandCheck = { 0, 0, 0 };
        //[0]barcodemode,[1]CCD测试次数,[2]状态名称,[3]混匀模式，[4]打印模式，[5]自动测试,[6]是否光源校准,[7]是否中心校准,[8]是否锁定弹窗,[9]取片失败次数,[10]休眠状态
        private readonly int[] _otherInt = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        //bool变量，休眠设定
        public bool b_sleep = false;
        //当前仪器测试参数 检测头，片仓,*反应时间
        private int[] _MParam = { 0, 1, 300 };
        //资源文件 根据信息编号读取信息内容
        //private ResourceManager _rm;
        //警告弹窗
        private FormAlert _myAlert;
        private string searchcondition = "";       
        //读取鼠标在屏幕上的位置
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out POINT pt);

        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        //初始化
        public FormMain()
        {
            InitializeComponent();
        }


        private void FormMain_Load(object sender, EventArgs e)
        {
            tabControlMain.SelectedTab = tabPageLoad;
            ShowCursor(1);
            timerLoad.Start();
            //timerSleep.Start();
        }

        //向数据库中写入配置文件数据
        private void UpdateAppConfig(string newKey, string newValue)
        {
            try
            {
                SqlData.UpdateAppSetting(newKey, newValue);
            }
            catch (Exception ee)
            {
                LogAdd("Data", "[E]", "写入配置文件数据出错" + ee);
            }

        }

        //数据校验
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



        //写入日志，形式为"Data/Result"+时间+加字符串
        private void LogAdd(string project, string SorR, string str)
        {
            Invoke(new Action(() =>
            {
                if (project == "Data")
                {
                    textBoxData.AppendText(SorR + DateTime.Now.ToString(time) + str);
                    textBoxData.AppendText("\r\n");
                }
                if (project == "Result")
                {
                    textBoxResult.AppendText(SorR + DateTime.Now.ToString(time) + str);
                    textBoxResult.AppendText("\r\n");
                }
            }));
        }

        //系统计时器,测试若还在进行，表格显示倒计时，若完成，显示浓度，并存入数据库
        private void timersystem_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            labelTimeDay.Text = dt.ToShortDateString().ToString();
            labelTimeSecond.Text = dt.ToLongTimeString().ToString();

            for (int i = 0; i < dataGridViewMain.RowCount; i++)//遍历表格每一行
            {
                if (dataGridViewMain.Rows[i].Cells[3].Value.ToString() == "1")//第四列，测试完成的值等于1，表示测试未完成
                {
                    var str = dataGridViewMain.Rows[i].Cells[2].Value.ToString();//第三列测试状态
                    str = str.Substring(str.IndexOf(":", StringComparison.Ordinal) + 1);//从冒号开始后面所有字符，即测试倒计时时间
                    try
                    {
                        if (int.Parse(str) >= 1)
                        {
                            dataGridViewMain.Rows[i].Cells[2].Value = "正在测试........:" + (int.Parse(str) - 1);//显示测试中：倒计时间                   
                        /*
                        if (int.Parse(str) == 5)
                        {                            
                            LogAdd("Data", "[M]", "荧光电机串口打开");
                        }*/
                        }
                        else
                        {
                            dataGridViewMain.Rows[i].Cells[3].Value = "2";//测试完成
                        //dataGridViewMain.Rows[i].Cells[2].Value = CalResult().ToString("f2") + "pg/mL";//第三列显示浓度
                        //var SampleNo = dataGridViewMain.Rows[0].Cells[0].Value.ToString();
                        //var TestItemID = "1";//测试项目还没定，先暂用1
                        //var TestItem = dataGridViewMain.Rows[0].Cells[1].Value.ToString();
                        //var Result = dataGridViewMain.Rows[0].Cells[2].Value.ToString().Replace("pg/mL", "");
                        //SqlData.InsertNewResult(SampleNo, DateTime.Now.ToString(), TestItemID, Result, "pg/mL", "", "", "1|0", "1", "", "");
                            if (ConfigRead("AutoPrint") == "1")
                                PrintReadResult();
                                PagePrint();
                        }
                    }
                    catch(Exception ee)
                    {
                        LogAdd("Result", "[E]", "0015" + ee);
                    }
                }
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                Button btn = (Button)sender;
                    if (btn == buttonClose)
                    {
                        try
                        {
                            serialPort_DataSend(serialPortFluoMotor, "010629");

                        //LogAdd("Data", "[S]", "关机010629");

                            this.Dispose();
                            this.Close();

                        //this.Enabled = false;

                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show(ee.ToString());
                        }
                    }
                    else if (btn == buttonTest)
                    {
                        if (dataGridViewMain.Rows.Count != 0)
                        {
                            if (dataGridViewMain.Rows[0].Cells[2].Value.ToString().Substring(0, 1) == "正")
                            {
                                MessageBox_Show("测试尚未结束，是否等待", true);
                                if (message[2] == "OK")
                                {
                                    return;
                                }
                                else if (message[2] == "Cancel")
                                {
                                    dataGridViewMain.Rows.Clear();
                                }
                            }

                        }
                        var result = CalResult();
                        dataGridViewMain.Rows.Clear();
                        dataGridViewMain.Rows.Add(textBoxNum.Text, textBoxItem.Text, "正在测试......:" + labelTime.Text.ToString(), "1", DateTime.Now.ToString());
                        labeltestnum.Text = "样 本 号：" + textBoxNum.Text;
                        labeltestItem.Text = "测试项目：" + textBoxItem.Text;
                        labelTesttype.Text = "样本类型：" + TestType[1];
                        if (QRstate == 0)
                        {
                            textBoxNum.Text = (textBoxNum.Text).ToString();
                            QRstate = 1;
                        }
                        else
                        {
                            textBoxNum.Text = (long.Parse(textBoxNum.Text) + 1).ToString().PadLeft(textBoxNum.Text.Length, '0');
                        }
                        LogAdd("Result", "[M]", "测试开始");
                        FluoNewTest("0");

                    }
                    else if (btn == button_BNP)
                    {
                        UpdataLotNum(1);
                    }
                    else if (btn == buttonItemConfirm)
                    {
                        tabControlMain.SelectedTab = tabPageMain;
                        labelLotNo.Text = labelLotNoChoose.Text;
                        UpdateAppConfig("LotNo", labelLotNoChoose.Text.Substring(5));
                    }
                    else if (btn == button_QRM)
                    {
                        panelReadQR.Visible = true;
                        QRstate = 0;
                    }
                    else if (btn == buttonReaderQR)
                    {
                        serialPort_DataSend(serialPortQR, "#TRGON");
                        LogAdd("Result", "[S]", "点读卡，开启内置扫码器");
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
                            TestType[1] = "全血";
                            buttonChange.BackgroundImage = Resource.switch_left;
                            labelbloodX.ForeColor = Color.DarkGray;
                            labelblood.ForeColor = Color.Black;
                        }
                    }
                    else if (btn == buttonPrint)
                    {
                        message[3] = "";
                        try
                        {
                            PrintReadResult();
                            PagePrint();
                        }
                        catch
                        {
                            MessageBox_Show("没有结果，无法打印", false);
                        }
                    }
                    else if (btn == buttonPageDown)
                    {
                        if (page[0] != page[1])
                        {
                            page[0] = page[0] + 1;
                            UpdataSearchResult(page[0], page[2], searchcondition);
                            labelPages.Text = page[0] + "/" + page[1];
                        }

                    }
                    else if (btn == buttonPageUp)
                    {
                        if (page[0] != 1)
                        {
                            page[0] = page[0] - 1;
                            UpdataSearchResult(page[0], page[2], searchcondition);
                            labelPages.Text = page[0] + "/" + page[1];
                        }
                    }
                    else if (btn == buttonAutoPrintSwitch)
                    {
                        if (ConfigRead("AutoPrint") == "0")
                        {
                            buttonAutoPrintSwitch.BackgroundImage = Resource.switch_right;
                            labelAutoPrint.Text = Resources.AutoPrintOpen;
                            UpdateAppConfig("AutoPrint", "1");
                        }
                        else if (ConfigRead("AutoPrint") == "1")
                        {
                            buttonAutoPrintSwitch.BackgroundImage = Resource.switch_left;
                            labelAutoPrint.Text = Resources.AutoPrintClose;
                            UpdateAppConfig("AutoPrint", "0");
                        }
                    }
                    else if (btn == buttonChangeItems)
                    {
                        tabControlMain.SelectedTab = tabPageItem;
                        dataGridViewProductID.DataSource = null;
                        buttonItemConfirm.Visible = false;
                        buttonLotNoDelect.Visible = false;
                    }
                    else if (btn == buttonMin)
                    {
                        this.WindowState = FormWindowState.Minimized;
                    }
                    else if (btn == buttonPrintResult)
                    {
                        PagePrint();
                    }
                    else if (btn == buttonChangeResultNo)
                    {
                        textBox_Click(textBoxResultNum, null);
                    }
                    else if (btn == buttonDateTime)
                    {
                        Rewrite[2] = "开始日期";
                        textBox_Click(textBoxStartTime, null);
                        Rewrite[2] = "结束日期";
                        textBox_Click(textBoxEndTime, null);
                    }
                    else if (btn == buttonSearch)
                    {
                        if (textBoxResultNum.Text == "")
                        {
                            page[0] = 1;
                            var starttime = textBoxStartTime.Text.Split('/');
                            var endtime = textBoxEndTime.Text.Split('/');
                            var starttime1 = starttime[0] + "-" + starttime[1] + "-" + starttime[2];
                            var endtime1 = endtime[0] + "-" + endtime[1] + "-" + endtime[2];
                            searchcondition = string.Format(" createtime between '{0:yyyy-MM-dd 00:00:00}' and '{1:yyyy-MM-dd 23:59:59}' ", starttime1, endtime1);
                            UpdataSearchResult(page[0], page[2], searchcondition);
                            var resultCount = int.Parse(SqlData.SelectResultCount(searchcondition).Rows[0][0].ToString());
                            if (resultCount < page[2])
                            {
                                page[1] = 1 + resultCount / page[2] + (resultCount % page[2] == 0 ? 0 : 1);//0719
                                labelPages.Text = page[0] + "/" + page[1];
                            }
                            else
                            {
                                page[1] = resultCount / page[2] + (resultCount % page[2] == 0 ? 0 : 1);//0719
                                labelPages.Text = page[0] + "/" + page[1];
                            }

                        }
                        else
                        {
                            page[0] = 1;
                            var starttime = textBoxStartTime.Text.Split('/');
                            var endtime = textBoxEndTime.Text.Split('/');
                            var starttime1 = starttime[0] + "-" + starttime[1] + "-" + starttime[2];
                            var endtime1 = endtime[0] + "-" + endtime[1] + "-" + endtime[2];
                            searchcondition = string.Format(" createtime between '{0:yyyy-MM-dd 00:00:00}' and '{1:yyyy-MM-dd 23:59:59}' ", starttime1.ToString() + " 00:00:00", endtime1.ToString() + " 23:59:59");
                            searchcondition = searchcondition + "and sampleNo = '" + textBoxResultNum.Text.ToString() + "'";
                            UpdataSearchResult(page[0], page[2], searchcondition);
                            var resultCount = int.Parse(SqlData.SelectResultCount(searchcondition).Rows[0][0].ToString());
                            if (resultCount < page[2])
                            {
                                page[1] = 1 + resultCount / page[2] + (resultCount % page[2] == 0 ? 0 : 1);//0719
                                labelPages.Text = page[0] + "/" + page[1];
                            }
                            else
                            {
                                page[1] = resultCount / page[2] + (resultCount % page[2] == 0 ? 0 : 1);//0719
                                labelPages.Text = page[0] + "/" + page[1];
                            }
                        }
                    }
             }));
        }

        //计算结果,临时
        private double CalResult()
        {
            Random ran = new Random();
            double result = ran.Next(50, 6000);
            result = Math.Pow((result * 13), 2) / 950000;
            if (result < 1)
            {
                result = 1;
            }
            else if (result > 6400)
            {
                result = 6400;
            }
            return result;
        }


        private void buttonmenu_Click(object sender, EventArgs e)
        //页面切换
        {
            Invoke(new Action(() =>
            {
                Button btn = (Button)sender;
                switch (btn.Name)
                {
                    case "buttonData":
                        tabControlMain.SelectedTab = tabPageResult;
                        page[0] = 1;
                        searchcondition = _searchcondition;
                        UpdataSearchResult(page[0], page[2], searchcondition);
                        var resultCount = int.Parse(SqlData.SelectResultCount(searchcondition).Rows[0][0].ToString());
                        if (resultCount < page[2])
                        {
                            page[1] = 1 + resultCount / page[2] + (resultCount % page[2] == 0 ? 0 : 1);//0719
                            labelPages.Text = page[0] + "/" + page[1];
                        }
                        else
                        {
                            page[1] = resultCount / page[2] + (resultCount % page[2] == 0 ? 0 : 1);//0719
                            labelPages.Text = page[0] + "/" + page[1];
                        }
                        //页数显示
                        buttonPrintResult.Visible = false;
                        //打印按键不可见
                        textBoxStartTime.Text = DateTime.Today.AddDays(-7).ToString("yyyy/MM/dd");
                        textBoxEndTime.Text = DateTime.Now.ToString("yyyy/MM/dd");
                        //选择日期
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
            }));

        }
        //打印机输出值
        private void PrintReadResult()
        {
            message[3] += dataGridViewMain.Rows[0].Cells[4].Value.ToString() + "\r\n";
            message[3] += ("TestResult: ").ToString() + dataGridViewMain.Rows[0].Cells[2].Value.ToString() + "\r\n";
            message[3] += ("TestProject:").ToString() + dataGridViewMain.Rows[0].Cells[1].Value.ToString() + "\r\n";
            message[3] += ("SampleID:   ").ToString() + dataGridViewMain.Rows[0].Cells[0].Value.ToString() + "\r\n";
                     
        }
        private void PagePrint()
        //打印机输出
        {
            serialPort_DataSend(serialPortPrint, message[3]);
            message[3] = "";
        }

        //主控端口校验
        private string MainPortCheck(string strcmd)
        {
            var check = 0;
            for (int i = 0; i < strcmd.Length; i++)
            {
                check += strcmd[i];
            }
            var a = (char)((check & 0x0F) + 'A');
            var b = (char)(((check & 0xF0) >> 4) + 'A');
            return a + b.ToString();
        }

        //串口数据发送
        private void serialPort_DataSend(SerialPort sr, string strCmd)
        {
            var frontstr = "";
            var backstr = "";
            if (sr == serialPortFluoMotor)//主控板
            {
                if (strCmd == "") return;
                frontstr = "";
                backstr = Environment.NewLine;
                //cmdlist.Add(strCmd);               
            }
            if (sr == serialPortFluo)//荧光头
            {                               
                backstr = Environment.NewLine;
            }
            else if (sr == serialPortQR)//内置扫码器
            {
                frontstr = "";
                backstr = "\r";
                //MessageBox.Show("111");
            }
            else if (sr == serialPortQRM)//外置扫码器
            {
                frontstr = "";
                backstr = Environment.NewLine;
            }
            else if (sr == serialPortPrint)//打印机
            {
                frontstr = "";
                backstr = Environment.NewLine;
            }
            try
            {
                //荧光发送数据的规则是:0003....\r\n  :0006....\r\n
                var writeBuffer = Encoding.ASCII.GetBytes(frontstr + strCmd + backstr);
                sr.Write(writeBuffer, 0, writeBuffer.Length);
                LogAdd("Data", "[S]", strCmd);
                Thread.Sleep(50);
            }
            catch (Exception ee)
            {
                LogAdd("Data", "[E]", "0004" + ee.ToString());
            }
        }

        //荧光头握手
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
                        LogAdd("Data", "[M]", "FluoConnectOK");
                        break;
                    // ReSharper disable once LocalizableElement
                    case 1:
                        LogAdd("Data", "[M]", "FluoReConnectOK");
                        break;
                    // ReSharper disable once LocalizableElement
                    case 2:
                        LogAdd("Data", "[M]", "FluoMotorReinitOK");
                        break;
                }
            }
            catch (Exception ee)
            {
                LogAdd("Data", "[E]", "0003" + ee.ToString());
            }
        }

        //启动荧光测试
        private void FluoNewTest(string seq)
        {
            var fluoEnable = ConfigRead("FluoEnable");
            if (fluoEnable == "0")
            {
                ShowMyAlert("荧光端口未开启");
                return;
            }
            _otherStr[0] = seq;
            var fluoParam = "180|0|0|8|1|1|0|0|1|1|0|1|0|0|300|300|300|300|66|51|100|76|160|126|10|51|100";
            //var fluoParam = ConfigRead("FluoParam");
            var fluoparam = fluoParam.Split('|');
            var vD = new string[33];
            //反应参数共33个
            //Invoke(new Action(() =>
                vD[0] = int.Parse(fluoparam[0]).ToString("X4");
                vD[1] = int.Parse(fluoparam[1]).ToString("X4");
                vD[7] = int.Parse(fluoparam[7]).ToString("X4");
                vD[20] = int.Parse(fluoparam[14]).ToString("X4");
                vD[21] = int.Parse(fluoparam[15]).ToString("X4");
                vD[22] = int.Parse(fluoparam[16]).ToString("X4");
                vD[23] = int.Parse(fluoparam[17]).ToString("X4");
                vD[32] = int.Parse(fluoparam[26]).ToString("X4");
                vD[2] = int.Parse(fluoparam[2]).ToString("X2") + "00";
                vD[3] = int.Parse(fluoparam[3]).ToString("X2") + "00";
                vD[4] = int.Parse(fluoparam[4]).ToString("X2") + "00";
                vD[5] = int.Parse(fluoparam[5]).ToString("X2") + "00";
                vD[6] = int.Parse(fluoparam[6]).ToString("X2") + "00";
                vD[24] = int.Parse(fluoparam[18]).ToString("X2") + "00";
                vD[25] = int.Parse(fluoparam[19]).ToString("X2") + "00";
                vD[26] = int.Parse(fluoparam[20]).ToString("X2") + "00";
                vD[27] = int.Parse(fluoparam[21]).ToString("X2") + "00";
                vD[28] = int.Parse(fluoparam[22]).ToString("X2") + "00";
                vD[29] = int.Parse(fluoparam[23]).ToString("X2") + "00";
                vD[30] = int.Parse(fluoparam[24]).ToString("X2") + "00";
                vD[31] = int.Parse(fluoparam[25]).ToString("X2") + "00";
           // }));

            for (var i = 8; i < 14; i++)
            {
                var b = BitConverter.GetBytes(float.Parse(fluoparam[i]));
                var a = b[3].ToString("X2") + b[2].ToString("X2") + b[1].ToString("X2") + b[0].ToString("X2");
                vD[i * 2 - 8] = a.Substring(0, 4);
                vD[i * 2 - 7] = a.Substring(4, 4);
            }
            int x = int.Parse(fluoparam[18]);           
            for (var i = 0; i < 33; i++)
            {
                var strcmd = string.Format("0006{0:X4}{1}{2}", i, vD[i], LeftCheck1(string.Format("0006{0:X4}{1}", i, vD[i])));
                serialPort_DataSend(serialPortFluo, ":"+strcmd);
            }
            serialPort_DataSend(serialPortFluoMotor, "010620");            
        }

        /*
        private void FluoParam(int index, string param)
        {
            var fluop = ConfigRead("FluoParam").Split('|');
            var param1 = "";
            for (var i = 0; i < index; i++)
            {
                param1 += fluop[i] + "|";
            }
            param1 += param + "|";
            for (var i = index + 1; i < fluop.Length; i++)
            {
                param1 += fluop[i] + "|";
            }
            param1 = param1.Substring(0, param1.Length - 1);
            UpdateAppConfig("FluoParam", param1);
        }*/


        //荧光电机数据接收
        private void serialPortFluoMotor_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                var currentline = new StringBuilder();
                while (serialPortFluoMotor.BytesToRead > 0)
                {
                    var ch = (char)serialPortFluoMotor.ReadByte();
                    currentline.Append(ch);
                }
                if (currentline.Length == 0)
                {
                    return;
                }
                else
                {
                    commandStr[1] += currentline.ToString();
                    //LogAdd("Result", "[R]", currentline.ToString());
                    //对荧光电机数据进行整理
                    serialPort_DataMakeUp(serialPortFluoMotor);
                }               
            }
            catch (Exception ee)
            {
                //Log_Add("3158" + ee.ToString(), false);
                LogAdd("Data", "[E]", "0004" + ee.ToString());
            }
        }

        //荧光头数据接收
        private void serialPortFluo_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                var currentline = new StringBuilder();
                while (serialPortFluo.BytesToRead > 0)
                {
                    var ch = (char)serialPortFluo.ReadByte();
                    currentline.Append(ch);                    
                }
                if (currentline.Length == 0)
                {
                    return;
                }               
                else//将采集的数据放入荧光数据字符串
                {
                    commandStr[0] += currentline.ToString();                   
                    LogAdd("Result", "[R]", currentline.ToString());                                                           
                    serialPort_DataMakeUp(serialPortFluo);
                }
            }
            catch (Exception ee)
            {
                LogAdd("Data", "[E]", "0002" + ee.ToString());
            }
        }


        //内置二维码数据接收
        private void serialPortQR_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                var currentline = new StringBuilder();
                while (serialPortQR.BytesToRead > 0)
                {
                    var ch = (char)serialPortQR.ReadByte();
                    currentline.Append(ch);
                }
                if (currentline.Length == 0)
                {
                    return;
                }
                else
                {
                    commandStr[2] += currentline.ToString();
                    LogAdd("Result", "[R]", currentline.ToString());
                    var strTemp = commandStr[2];
                    //Invoke(new Action(() => PortLog("QR", "R", strTemp)));
                    commandStr[2] = "";
                   //对一条二维码数据进行处理
                    serialPort_DataDeal(strTemp, "QR");                   
                }                                   
            }
            catch (Exception ee)
            {
                LogAdd("Data", "[E]", "0011" + ee.ToString());
            }
        }

        //外置二维码数据接收
        private void serialPortQRM_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                //Console.WriteLine("进入数据接收");
                var currentline = new StringBuilder();
                
                while (serialPortQRM.BytesToRead > 0)
                {
                    //Console.WriteLine("进入while");
                    var ch = (char)serialPortQRM.ReadByte();
                    commandStr[3] += ch.ToString();                                  
                    if (ch == '\r')
                    {
                        var strTemp = commandStr[3];
                        LogAdd("Result", "[R]", strTemp.ToString());
                        commandStr[3] = "";
                        //对一条二维码数据进行处理
                        Invoke(new Action(() =>
                        {
                            labelQRM.Text = strTemp;
                            textBoxNum.Text = labelQRM.Text;
                        }));
                    }
                }
            }
            catch (Exception ee)
            {
                LogAdd("Data", "[E]", "0011" + ee.ToString());
            }
        }

        private int Fluo2500 = 0;
        private void serialPort_DataMakeUp(SerialPort sr)//数据处理
        {
            if (sr == serialPortFluo)
            {
                //荧光需要采集的点数从配置文件中读取
                //var fluoPa = "180|0|0|8|1|1|0|0|1|1|0|1|0|0|300|300|300|300|32|51|100|76|160|126|10|51|100";
                //var fluoPointCount = int.Parse(ConfigRead("FluoParam").Split('|')[0]);
                //Console.WriteLine(commandStr[0]);
                var fluoPointCount = 180;              
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
                //Console.WriteLine("_fluodata个数");
                //Console.WriteLine(_fluoData.Count);
                
                if (_fluoData.Count == fluoPointCount)
                {
                    //Console.WriteLine("执行 ");
                    foreach (double a in _fluoData)//显示到日志里
                    {
                        LogAdd("Result", "[D]", a.ToString());
                    }

                    List<double> list = new List<double>();
                    if (_fluoData.Take(70).Max() > 400)
                    {
                        list = _fluoData.Take(180).ToList();
                        _fluoData.Clear();
                        for (int i = 0; i < 180; i++)
                        {
                            _fluoData.Add(list[i]);
                        }
                    }
                    else
                    {
                        list = _fluoData.Take(_fluoData.Count).Reverse().Take(180).Reverse().ToList();
                        _fluoData.Clear();
                        for (int i = 0; i < 180; i++)
                        {
                            _fluoData.Add(list[i]);
                        }
                    }
                    //定标曲线图
                    Invoke(new Action(() => { chartFluo.Series[0].Points.Clear(); }));
                    var str = "";
                    for (var i = 0; i < _fluoData.Count; i++)
                    {                        
                        var i1 = i;
                        Invoke(new Action(() => 
                        {
                            chartFluo.Series[0].Points.AddXY(i1, _fluoData[i1]);
                            chartFluo.Series[0].BorderWidth = 2;
                            chartFluo.ChartAreas[0].AxisX.Interval = 10;
                            chartFluo.ChartAreas[0].AxisX.LineColor = Color.Black;//X轴颜色   
                            chartFluo.ChartAreas[0].AxisX.LineWidth = 1;
                            chartFluo.ChartAreas[0].AxisY.LineColor = Color.Black;//Y轴颜色 
                            chartFluo.ChartAreas[0].AxisY.LineWidth = 1;                           
                        }));
                        str += _fluoData[i] + Environment.NewLine;
                    }

                    //var dt = SqlData.Selectresultinfo(_otherStr[0]);
                    var dt = dataGridViewMain.Rows[0].Cells[0].Value;
                    string sampleno = dt.ToString();
                    //var sampleno = dt.Rows.Count == 0 ? _otherStr[0] : dt.Rows[0][0].ToString();
                    str = str.Substring(0, str.Length - 2);
                    var path = string.Format("No-{0}-{1:yyyyMMddHHmmss}.csv", sampleno, DateTime.Now);
                    var path2 = string.Format("No-{0}-{1:yyyyMMddHHmmss}.jpg", sampleno, DateTime.Now);
                    //保存数据
                    Invoke(new Action(() => 
                    {
                        try
                        {
                            chartFluo.SaveImage(string.Format("{0}/FluoData/{1}.jpg", Application.StartupPath, path2), ChartImageFormat.Jpeg);
                            //var sw = new StreamWriter(string.Format("{0}/FluoData/{1}", Application.StartupPath, DateTime.Now.ToString(time) + ".csv"), true, Encoding.ASCII);
                            //sw.Write(str);
                            //sw.Flush();
                            //sw.Close();
                        }
                        catch (Exception ee)
                        {
                            LogAdd("Data", "[E]", "0017" + ee);
                        }
                    }));
                    

                    //提取完毕对数据进行运算，得到荧光OD详细数据
                    try
                    {
                        var odData = CalMethod.CalFluo(_fluoData);
                        LogAdd("Result", "[D]", odData);

                        _fluoData.Clear();
                        //初始化荧光字符串
                        //commandStr[0] = "-1";
                        if (odData.IndexOf("Error", StringComparison.Ordinal) > -1)
                        {
                            Invoke(new Action(() => LogAdd("Result", "[E]", odData)));
                            //DrawResult("-10", _otherStr[0], odData, "", "");
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
                        LogAdd("Result", "[D]", "(cx,cy)=" + "(" + cx + "," + cy + ");\r\n" + "(tx,ty)=" + "(" + tx + "," + ty + ");\r\n" + "(sumCBase)=" + sumCBase + ";\r\n" + "(sumTBase)=" + sumTBase + ";\r\n");

                        if (double.Parse(ty) >= 2500.0)
                        {
                            Fluo2500 = 1;
                            FluoNewTest(_otherStr[0]);
                            return;
                        }                  
                        var Tap = double.Parse(sumTBase) / (double.Parse(sumTBase) + double.Parse(sumCBase)) * 5000.0;
                        LogAdd("Result", "[D]", "Tap"+Tap.ToString());
                        //Console.WriteLine(Tap);
                        //进行结果计算与存储
                        DrawFluoResult(cy, sumCBase, sumTBase, _otherStr[0], odData, path, path2);
                    }
                    catch (Exception ee)
                    {
                        LogAdd("Data", "[E]", "0014" + ee.ToString());
                    }

                }
            }
            else if (sr == serialPortFluoMotor)
            {
                try
                {
                    //命令以字符串的形式传过来,每种命令单独发送
                    var str = commandStr[1];
                    commandStr[1] = "";
                    //LogAdd("Result", "[E]", "str"+str);
                    var indexm = str.IndexOf("$");
                    var indexp = str.IndexOf("*");
                    var indext = str.IndexOf("#");
                    //commandStr[1] = commandStr[1].Substring(commandStr[1].IndexOf(Environment.NewLine) + 2); 
                    if (indexm > -1)
                    {
                        var SArray = str.Substring(str.IndexOf("$"), str.IndexOf(Environment.NewLine) + 2);
                        LogAdd("Result", "[D]", "sarray:" + SArray);
                        SArray = SArray.Replace("\r\n", "");
                        //SArray = SArray.Substring(0, SArray.Length - 2);
                        serialPort_DataDeal(SArray, "Main");
                        SArray = "";
                    }
                    else if (indexp > -1)
                    {
                        var power = str.Substring(str.IndexOf("*"), str.IndexOf(Environment.NewLine));
                        var powerafter = int.Parse(power.Substring(2, power.Length - 4));
                        //LogAdd("Result", "[D]", "powerafter:" + powerafter);
                        if (!isfirst)
                        {
                            if (power.Substring(0, 2) == "*-")
                            {
                                if (powerafter > powerbefore)
                                {
                                    powerafter = powerbefore;
                                }
                            }
                            else if (power.Substring(0, 2) == "*+")
                            {                                
                                if (powerafter < powerbefore)
                                {
                                    powerafter = powerbefore;
                                }
                            }
                        }
                        isfirst = false;
                        var Powershow = powerafter.ToString() + "%";
                        Invoke(new Action(() => label_power.Text = Powershow));
                        powerbefore = powerafter;
                        power = "";
                    }
                    else if (indext > -1)
                    {
                        var temperature = str.Substring(str.IndexOf("#"), str.IndexOf(Environment.NewLine) + 2);
                        //LogAdd("Result", "[D]", "temperature" + temperature);
                        temperature = temperature.Replace("#", "");                        
                        temperature = temperature.Replace("\r\n", "");
                        //LogAdd("Result", "[D]", "temperature" + temperature);
                        //var Temperature = int.Parse(temperature);
                        //int temint = temperature.Length;
                        
                        //if (temint > 6)
                        //    temint = 6;
                        Invoke(new Action(() => label_temperature.Text = temperature.Substring(2,temperature.Length - 2).ToString() + "℃"));
                        if (temperature.Substring(0, 1) == "+")
                        {
                            float temperatureup = float.Parse(temperature.Substring(1, temperature.Length - 1));
                            if (temperatureup > 90.0)
                            {
                                MessageBox.Show("仪器温度过高");
                            }
                        }
                        else if (temperature.Substring(0, 1) == "-")
                        {
                            float temperaturedown = float.Parse(temperature.Substring(1, temperature.Length - 1));
                            if (temperaturedown > 0.0)
                            {
                                MessageBox.Show("仪器温度低于0℃");
                            }
                        }
                        temperature = "";
                    }               
                    
                }
                catch (Exception ee)
                {
                    LogAdd("Data", "[E]", "0001" + ee.ToString());
                }
            }
        }

        //提取荧光结果信息
        private void DrawFluoResult(string cy, string sumcbase, string sumtbase,
            string seq, string odData, string path1, string path2)
        {
            try
            {                
                var sampleNo = dataGridViewMain.Rows[0].Cells[0].Value.ToString();               
                var createtime = DateTime.Now.ToString();                
                var testItemId = "1";    /////////////////////////////////////////////////////
                var calibDataId = SqlData.getCaid(qulot.ToString());
                var unit = "pg/ml";
                var ratio = "1";
                var unitRatio = "1";
                var minValue = "0.01";
                var maxValue = "5000";
                var tyFixStr = "1|0";//统一性，ax+b，a=1,b=0
                var accurancy = "2";              
                var dtparam = SqlData.SelectFluoCalMethod(qulot.ToString());//计算参数提取  
                //var value = 0.00;
                var parama = double.Parse(tyFixStr.Split('|')[0]);
                var paramb = double.Parse(tyFixStr.Split('|')[1]);                  
                var value = (parama * 5000 * double.Parse(sumtbase) + paramb) / (parama * double.Parse(sumtbase) + paramb + parama * double.Parse(sumcbase) + paramb);                
                var result = 0.0;               
                result = value > Math.Max(double.Parse(dtparam.Rows[0][2].ToString()), double.Parse(dtparam.Rows[0][6].ToString())) ?
                        double.Parse(value > double.Parse(dtparam.Rows[0][3].ToString()) ? maxValue : (Math.Pow((double.Parse(dtparam.Rows[0][3].ToString()) - value) / (value - double.Parse(dtparam.Rows[0][6].ToString())), 1 / double.Parse(dtparam.Rows[0][4].ToString())) * double.Parse(dtparam.Rows[0][5].ToString())).ToString("F" + accurancy)) : double.Parse(((value - double.Parse(dtparam.Rows[0][1].ToString())) / double.Parse(dtparam.Rows[0][0].ToString())).ToString("F" + accurancy));
                //Console.WriteLine("result:" + result);
                result = Math.Min(Math.Max(result, double.Parse(minValue)), double.Parse(maxValue) / double.Parse(ratio));
                var flag = "";
                if (result == double.Parse(minValue)) flag = "<";
                else if (result == double.Parse(maxValue)) flag = ">";
                result = result * double.Parse(ratio) * double.Parse(unitRatio);
                dataGridViewMain.Rows[0].Cells[2].Value = result.ToString() + "pg/mL";//第三列显示浓度
                // ReSharper disable once ResourceItemNotResolved
                var msglog = Resources.M0001;
                msglog = msglog?.Replace("[1]", sampleNo);
                msglog += result.ToString(CultureInfo.InvariantCulture) + unit;
                LogAdd("Result", "[D]", msglog);               
                if (Fluo2500 == 1)
                {
                    Fluo2500 = 0;
                    flag = "$";
                }
                SqlData.InsertNewResult(sampleNo, DateTime.Now.ToString(), testItemId, result.ToString(CultureInfo.InvariantCulture), unit,
                    path1, path2, tyFixStr, calibDataId, odData, flag);
                //SqlData.InsertIntoNewResult(sampleNo, createtime, testItemId, result.ToString(CultureInfo.InvariantCulture), unit,
                //   path1, path2, tyFixStr, calibDataId, "", "", "", odData, flag);
            }
            catch (Exception ee)
            {
                LogAdd("Data", "[E]", "0005" + ee.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FluoConnect(1);
        }

        private void exportResult(string sampleno, string testitem, string result, string createtime)
        {
            if (sampleno.IndexOf("位置") > -1)
            {
                return;
            }
            if (!File.Exists(Application.StartupPath + "/iReader.txt"))
            {
                StreamWriter sw1 = new StreamWriter(Application.StartupPath + "/iReader.txt");
                sw1.Write("");
                sw1.Close();
            }
            StreamReader sr = new StreamReader(Application.StartupPath + "/iReader.txt");
            var str = sr.ReadToEnd();
            str += string.Format("{0},{1},{2},{3}\r\n", sampleno, testitem, result, createtime);
            sr.Close();
            StreamWriter sw = new StreamWriter(Application.StartupPath + "/iReader.txt");
            sw.Write(str);
            sw.Close();
        }

        private void textBox_Click(object sender, EventArgs e)
        {
            TextBox txb = (TextBox)sender;          
            Rewrite[1] = txb.Text;
            var str = UseKeyBoard();
            txb.Text = str;
            panelMenu.Focus();
            
            if (txb.Name == textBoxSleepTime.Name)
            {
                if (str == "")
                {                   
                    _otherInt[0] = 0;
                    ShowMyAlert("休眠时间不能为空");
                    return;
                }
                var time = int.Parse(str) * 60;
                UpdateAppConfig("SleepTime", str);
                serialPort_DataSend(serialPortFluoMotor, "010627" + time);
            }

            txb.Text = str;
            _otherInt[0] = 0;
        }
        
        private void ShowMyAlert(string alertstr)
        {
            if (_myAlert != null)
            {
                if (!_myAlert.IsDisposed)
                    return;
            }

            try
            {
                CounterText = alertstr;
                _myAlert = new FormAlert { Owner = this };               
                _myAlert.Show();
            }
            catch (Exception ee)
            {
                LogAdd("Data", "[E]", "0006" + ee.ToString());
            }
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

        private void UpdataSearchResult(int pages, int resultnum, string condition)
        //显示结果
        {
            DataTable tb = new DataTable();
            tb = SqlData.SelectResult(pages, resultnum, condition);
            dataGridViewResult.DataSource = tb;
            dataGridViewResult.Columns[0].Width = 200;
            dataGridViewResult.Columns[0].HeaderCell.Value = "样本号";
            dataGridViewResult.Columns[1].Width = 149;
            dataGridViewResult.Columns[1].HeaderCell.Value = "测试项目";
            dataGridViewResult.Columns[2].Width = 148;
            dataGridViewResult.Columns[2].HeaderCell.Value = "测试结果";
            dataGridViewResult.Columns[3].Visible = false;
        }

        private void UpdataLotNum(int ProductID)
        //显示结果
        {
            DataTable tb = new DataTable();
            tb = SqlData.SelectLotNo(ProductID);
            dataGridViewProductID.DataSource = tb;
            dataGridViewProductID.Columns[0].Width = 200;
            dataGridViewProductID.Columns[0].HeaderCell.Value = "定标批号";
        }


        //端口数据处理
        //double InnerTemprature = 0;
        private void serialPort_DataDeal(string strCmd, string type)
        {            
            if (type == "QR")//定标二维码数据
            {
                try
                {
                    //去掉多余字符，二维码数据以<CR>(回车)结尾
                    strCmd = strCmd.Replace("\r", "");
                    var strcmdlenth = strCmd.Length;
                    LogAdd("Result", "[M]", strcmdlenth.ToString());
                    LogAdd("Result", "[D]", "strcmd"+strCmd);
                    Invoke(new Action(() =>
                    {
                        //添加新项目
                        if (CounterText == "AddNewItem")//点击了添加新项目按钮后才执行
                        {
                            var detail = strCmd.Split(',');
                            if (detail.Length == 17)
                            {
                                try
                                {
                                    SqlData.SelectTestItemNameById(detail[0]);
                                }
                                catch (Exception)
                                {
                                    SqlData.InsertNewProductInfo(detail);
                                }
                                try
                                {
                                    SqlData.InsertNewTestItem(detail);
                                }
                                catch (Exception)
                                {
                                    //Log_Add("该项目已经存在", true);
                                    LogAdd("Data", "[E]", "该项目已存在");
                                }
                             CounterText = "";
                            }
                        }
                        else if (strCmd.Length == 84)
                        {
                            //MessageBox.Show("1");
                            //对定标信息进行解码
                            Base64Decode(strCmd);
                        }
                        else
                        {
                            LogAdd("Data", "[E]", "二维码长度出错");
                        }
                    }));
                }
                catch (Exception ee)
                {
                    //Log_Add("3569" + ee.ToString(), false);
                    LogAdd("Data", "[E]", "0008" + ee.ToString());
                }
            }
            else if (type == "Main")
            {

                var msgInfo = strCmd;
                try
                {
                    //var power = msgInfo.Substring('*', msgInfo.IndexOf(Environment.NewLine));
                    //var temperatrue = msgInfo.Substring('#', msgInfo.IndexOf(Environment.NewLine));
                    //LogAdd("Result", "[D]", power);
                    //LogAdd("Result", "[D]", temperatrue);

                    //信息编号及信息
                    var msgType = msgInfo.Substring(0, 6);                  
                    //var msgLog = "";
                    //命令、错误,警告和信息
                    string[] msgnoparam =
                    {
                        "$10621", "$10622", "$10623", "$10624", "$10625", "$10626", "$10627",
                        "$20120", "$20121", "$20122", "$20123",
                        "$20220", "$20221", "$20222", "$20225",
                        "$20320", "$20321"
                    };                    
                    if (msgnoparam.Contains(msgType))
                    {                        
                        switch (msgType)
                        {
                            case "$10621":
                                {
                                    serialPort_DataSend(serialPortQR, "#TRGON");
                                    //serialPort_DataSend(serialPortFluoMotor, "010621");
                                    LogAdd("Result", "[M]", "电机准备好扫码，开启扫码器");
                                };
                                break;
                            case "$10622":
                                {
                                    LogAdd("Result", "[M]", "电机回零位");
                                };
                                break;
                            case "$10623"://电机减速，开始扫描,打开荧光检测头
                                
                                //FluoNewTest("0");
                                serialPort_DataSend(serialPortFluo, ":000602000001F7");//开启荧光头 
                                //timer1.Start();
                                LogAdd("Result", "[M]", "荧光扫描开启");                                                         
                                break;
                            case "$10624"://电机加速，结束扫描，存入数据库
                                {
                                    LogAdd("Result", "[M]", "电机加速，荧光扫描关闭");
                                    serialPort_DataSend(serialPortFluo, ":000301000002FA");//读取地址256里的值，直到满足180个点，考虑300ms发送一次，读三次
                                    serialPort_DataSend(serialPortFluo, ":000302000001FA");//读取地址512里采集的点数，设定值为180个点                                                                                          
                                    serialPort_DataSend(serialPortFluo, ":000302010010EA");
                                    serialPort_DataSend(serialPortFluo, ":000302110010DA");
                                    serialPort_DataSend(serialPortFluo, ":000302210010CA");
                                    serialPort_DataSend(serialPortFluo, ":000302310010BA");
                                    serialPort_DataSend(serialPortFluo, ":000302410010AA");
                                    serialPort_DataSend(serialPortFluo, ":0003025100109A");
                                    serialPort_DataSend(serialPortFluo, ":0003026100108A");
                                    serialPort_DataSend(serialPortFluo, ":0003027100107A");
                                    serialPort_DataSend(serialPortFluo, ":0003028100106A");
                                    serialPort_DataSend(serialPortFluo, ":0003029100105A");
                                    serialPort_DataSend(serialPortFluo, ":000302A100104A");
                                    serialPort_DataSend(serialPortFluo, ":000302B100103A");
                                    serialPort_DataSend(serialPortFluo, ":000302C100102A");
                                    serialPort_DataSend(serialPortFluo, ":000302D100101A");
                                    serialPort_DataSend(serialPortFluo, ":000302E100100A");
                                    serialPort_DataSend(serialPortFluo, ":000302F10010FA");
                                    serialPort_DataSend(serialPortFluo, ":000303010010E9");
                                    serialPort_DataSend(serialPortFluo, ":000303110010D9");
                                    serialPort_DataSend(serialPortFluo, ":000303210010C9");
                                    serialPort_DataSend(serialPortFluo, ":000303310010B9");
                                    serialPort_DataSend(serialPortFluo, ":000303410010A9");
                                    serialPort_DataSend(serialPortFluo, ":00030351001099");
                                    serialPort_DataSend(serialPortFluo, ":00030361000891");                                  
                                };
                                break;
                            case "$10625"://电机到达最里端，可以开启新的荧光检测
                                LogAdd("Result", "[M]", "电机到达最里端");
                                break;
                            //case "$10627"://电机到达最外端
                            //    {
                            //        try
                            //        {
                            //            Thread.Sleep(100);
                            //            Invoke(new Action(() => { this.Close(); }));
                            //        }
                            //        catch
                            //        {
                            //            MessageBox.Show("关机失败");
                            //        }
                            //    };
                            //    break;
                            case "$10626"://电机到达最外端
                                LogAdd("Result", "[M]", "电机到达最外端");                                
                                break;
                            case "$20120":
                                LogAdd("Result", "[M]", "电源电压信号异常");
                                break;
                            case "$20121":
                                LogAdd("Result", "[M]", "温度信号异常");
                                break;
                            case "$20122":
                                LogAdd("Result", "[M]", "左光耦无信号");
                                break;
                            case "$20123":
                                LogAdd("Result", "[M]", "右光耦无信号");
                                break;
                            case "$20220":
                                ShowMyAlert("温度过高");
                                LogAdd("Result", "[M]", "温度过高");
                                break;
                            case "$20221":
                                ShowMyAlert("温度过低");
                                LogAdd("Result", "[M]", "温度过低");
                                break;
                            case "$20222":
                                ShowMyAlert("电量过低");
                                LogAdd("Result", "[M]", "电量过低");
                                break;
                            case "$20225"://一个测试结束电机未回最外端                                 
                                ShowMyAlert("电机未回最外端");
                                LogAdd("Result", "[M]", "电机未回最外端");
                                break;
                            case "$20320":
                                LogAdd("Result", "[M]", "电机未准备好");
                                break;
                            case "$20321":
                                LogAdd("Result", "[M]", "休眠电机未回最里端");
                                break;
                        }
                    }                                                                                                
                }
                catch (Exception ee)
                {
                    LogAdd("Data", "[E]", "0010" + ee);
                }
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
                qulot = lot;
                Console.WriteLine(qulot);
                a = Convert.ToString(outputb[5], 2).PadLeft(8, '0') + Convert.ToString(outputb[6], 2).PadLeft(8, '0');
                //第6第7字节表示生产日期，前7位为年份+2000，接着4位为月份，最后5位为日
                var year = Convert.ToInt32(a.Substring(0, 7), 2);
                var month = Convert.ToInt32(a.Substring(7, 4), 2);
                var day = Convert.ToInt32(a.Substring(11), 2);

                LogAdd("Result", "[D]", "byte数组;" + outputb + "产品编号:" + cpmc + "保质期:" + bzs + "公式类型:" + funtype+"批号信息:"+lot+"生产日期:"+year+month+day);
                //第8字节开始后的56个字节，每8个字节对应一个double数字，共7个参数
                var param = new double[7];
                for (var i = 0; i < 7; i++)
                {
                    var aa = new byte[8];
                    Array.Copy(outputb, 7 + i * 8, aa, 0, 8);
                    param[i] = BitConverter.ToDouble(aa, 0);
                    LogAdd("Result", "[D]", param[i].ToString());
                }

                var dtPdate = DateTime.Parse(string.Format("{0}-{1}-{2}", year, month, day));
                var epdate = dtPdate.AddMonths(3 * int.Parse(bzs));
                //2016年1月11之前规则：+15月的前一月最后一天。  2016年1月11号改为+15月-1天
                epdate = dtPdate < DateTime.Parse("2016-01-11 00:00:00") ? epdate.AddDays(-epdate.Day) : epdate.AddDays(-1);
                //将定标信息插入到定标数据表中
                var calibdataid = SqlData.InsertIntoCalibdata(cpmc, lot, dtPdate, epdate, funtype, param);
                //???
                var strCalibdataid = calibdataid == 0
                    ? "(select max(CalibDataID)  from CalibData)"
                    : calibdataid.ToString();
                
                if (calibdataid > -1)
                {
                    var labelQrText = Resources.labelQRText;
                    labelQrText = labelQrText?.Replace("[m]", lot);
                    //var productName = SqlData.SelectTestItemNameById(cpmc);
                   // labelQrText = labelQrText?.Replace("[n]", productName);                   
                    Invoke(new Action(() =>
                    {                        
                        labelLotNo.Text = labelQrText;
                        //labelQR.Text = labelQrText;                        
                    }));
                    if (labelLotNo.Text.ToString().Substring(0, 1) == "识")
                    {
                        serialPort_DataSend(serialPortFluoMotor, "010622");
                        LogAdd("Result", "[M]", "扫码成功告知电机");
                    }
                    else 
                    {
                        serialPort_DataSend(serialPortFluoMotor, "010623");
                        MessageBox.Show("请重新放片");
                    }
                }
                else
                    Invoke(new Action(() => labelQRM.Text = Resources.QRError));
            }
            catch (Exception)
            {
                Invoke(new Action(() => labelQRM.Text = Resources.QRError));
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
                    else if (dgv == dataGridViewResult)
                    {
                        buttonPrintResult.Visible = true;
                        message[3] = "";               
                        message[3] += dataGridViewResult.Rows[e.RowIndex].Cells[3].Value.ToString() + "\r\n";
                        message[3] += ("SampleID:   ").ToString() + dataGridViewResult.Rows[e.RowIndex].Cells[0].Value.ToString() + "\r\n";
                        message[3] += ("TestProject:").ToString() + dataGridViewResult.Rows[e.RowIndex].Cells[1].Value.ToString() + "\r\n";
                        message[3] += ("TestResult: ").ToString() + dataGridViewResult.Rows[e.RowIndex].Cells[2].Value.ToString() + "\r\n";
                    }
                    else if (dgv == dataGridViewProductID)
                    {
                        buttonItemConfirm.Visible = true;
                        buttonLotNoDelect.Visible = true;
                        labelLotNoChoose.Text = Resources.LotNo + dataGridViewProductID.Rows[e.RowIndex].Cells[0].Value.ToString();
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
            timerSleep.Start();
            var _sleeptime = ConfigRead("SleepTime");
            textBoxSleepTime.Text = _sleeptime;
            var fluoEnable = ConfigRead("FluoEnable");
            var QREnable = ConfigRead("QREnable");
            var PrintEnable = ConfigRead("PrintEnable");
            if (fluoEnable == "1")
            {
                //荧光端口
                try
                {
                    serialPortFluo.PortName = ConfigRead("FluoPort");
                    serialPortFluo.Open();
                    FluoConnect(0);
                    LogAdd("Data", "[M]", "荧光串口打开");
                }
                catch (Exception ee)
                {
                    LogAdd("Data", "[E]", ee.ToString());
                }
                //荧光电机端口（由主控板控制，所以其实是主控板端口）
                try
                {
                    serialPortFluoMotor.PortName = ConfigRead("FluoMotorPort");
                    serialPortFluoMotor.Open();
                    serialPort_DataSend(serialPortFluoMotor, "010626");
                    LogAdd("Data", "[M]", "荧光电机串口打开");
                }
                catch (Exception ee)
                {
                    LogAdd("Data", "[E]", ee.ToString());
                }
                //定标信息扫码端口
                try
                {
                    serialPortQR.PortName = ConfigRead("QRport");
                    serialPortQR.Open();
                    //serialPort_DataSend(serialPortQR, "");
                    LogAdd("Data", "[M]", "内置扫码器串口打开");
                }
                catch (Exception ee)
                {
                    LogAdd("Data", "[E]", ee.ToString());
                }
                //样本信息扫码端口
                try
                {
                    serialPortQRM.PortName = ConfigRead("QRMPort");
                    serialPortQRM.Open();
                    //serialPort_DataSend(serialPortQRM, "1111111");
                    LogAdd("Data", "[M]", "外置扫码器串口打开");
                }
                catch (Exception ee)
                {
                    LogAdd("Data", "[E]", ee.ToString());
                }
                //打印机端口
                try
                {
                    serialPortPrint.PortName = ConfigRead("PrintPort");
                    serialPortPrint.Open();
                    LogAdd("Data", "[M]", "打印机串口打开");
                }
                catch (Exception ee)
                {
                    LogAdd("Data", "[E]", ee.ToString());
                }
            }
            var AutoPrint = ConfigRead("AutoPrint");
            if (AutoPrint == "1")
            {
                buttonAutoPrintSwitch.BackgroundImage = Resource.switch_left;
                labelAutoPrint.Text = Resources.AutoPrintOpen;
            }

            labelLotNo.Text = Resources.LotNo + ConfigRead("LotNo");
            labelReactionTime.Text = Resources.ReactionTime + ConfigRead("ReactionTime");
            buttonMain.Visible = true;
            buttonSetting.Visible = true;
            buttonData.Visible = true;
            tabControlMain.SelectedTab = tabPageMain;            
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            labelTime.Text = btn.Text;
           // labelReactionTime.Text = UpdateAppConfig("ReactionTime", labelTime.Text);
            labelReactionTime.Text = Resources.ReactionTime + btn.Text;

        }

        private void button7_Click(object sender, EventArgs e)
        {
            tabControlMain.SelectedTab = tabPageSecret;
        }

        private void textBoxUseless_Click(object sender, EventArgs e)
        {
            panelMenu.Focus();
        }

        
        //从数据库中读取配置数据
        private string ConfigRead(string str)
        {
            try
            {
                var key = str;
                str = SqlData.SelectAppSetting(str).Rows[0][0].ToString();
                if (str == "True")
                {
                    str = "1";
                }
                else if (str == "False")
                {
                    str = "0";
                }
                return str;
            }
            catch (Exception ee)
            {
                //Log_Add("2334" + ee.ToString(), false);
                LogAdd("Data", "[E]", ee.ToString());
                return "";
            }
        }

        //休眠设定
        private void timerSleep_Tick(object sender, EventArgs e)
        {           
            if (textBoxSleepTime.Text == "0")
            {
                ShowMyAlert("休眠倒计时模式关闭");                
                return;
            }
            else
            {           
                CommandCheck[2]--;
                //label45.Text = CommandCheck[4].ToString();
                if (CommandCheck[2] <= 0)
                {
                    timerSleep.Stop();
                    b_sleep = true;
                    serialPort_DataSend(serialPortFluoMotor, "010627");
                    tabControlMain.SelectedTab = tabPageSleep;//5秒进入休眠界面
                }
                /* else if (CommandCheck[4] == 300)
                    {
                        MessageboxShow("休眠警告|还有5分钟进入休眠");
                    }*/
            }
        }

        //捕捉鼠标
        private void timerCursor_Tick(object sender, EventArgs e)
        {
            POINT currentPosition = new POINT();
            GetCursorPos(out currentPosition);
            var cursor_x = currentPosition.X;
            var cursor_y = currentPosition.Y;
            if (cursor_x != Cursor_Point[0] | cursor_y != Cursor_Point[1])
            {
                var b = ConfigRead("SleepTime");
                var a = int.Parse(ConfigRead("SleepTime"));
                CommandCheck[2] = int.Parse(ConfigRead("SleepTime")) * 60;
                Cursor_Point[0] = cursor_x;
                Cursor_Point[1] = cursor_y;                      
                if (b_sleep)
                {
                    serialPort_DataSend(serialPortFluoMotor,"010628");
                    tabControlMain.SelectedTab = tabPageMain;
                    b_sleep = false;
                    timerSleep.Start();
                }
            }
        }
   

        private void label9_Click(object sender, EventArgs e)
        {

        }


        private void buttonFluoFix_Click(object sender, EventArgs e)
        {            
            FluoNewTest("0");
            serialPort_DataSend(serialPortFluo, ":000602020000F5");//LED灯开
            serialPort_DataSend(serialPortFluo, ":000602000001F7");//开启荧光头  
            timer1.Start();
            LogAdd("Result", "[M]", "荧光扫描开启");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //timer1.Stop();
            serialPort_DataSend(serialPortFluo, ":000301000002FA");//读取地址256里的值，直到满足180个点，考虑300ms发送一次，读三次
            serialPort_DataSend(serialPortFluo, ":000302000001FA");//读取地址512里采集的点数，设定值为180个点
            //var fluoPointCount = 180;
            /*
            for (var i = 0; i < (fluoPointCount - 1) / 8 + 1; i++)
            {
                 var strcmd = "0003" + (513 + i * 16).ToString("X4");//00030201
                 strcmd += i == (fluoPointCount - 1) / 8
                 ? ((fluoPointCount - 8 * i) * 2).ToString("X4")
                  : 16.ToString("X4");
                  //加上校验符号
                strcmd += LeftCheck1(strcmd);
                  //发送读取命令
                 serialPort_DataSend(serialPortFluo, strcmd);
                
            }
            for (int i = 0; i < 3; i++)
            {
                var strcmd = "0003" + (513 + i * 16).ToString("X4") + "0010";
                strcmd = strcmd + (234 - i * 16).ToString("X2");
                serialPort_DataSend(serialPortFluo, strcmd);
            }*/
            serialPort_DataSend(serialPortFluo, ":000302010010EA");
            serialPort_DataSend(serialPortFluo, ":000302110010DA");
            serialPort_DataSend(serialPortFluo, ":000302210010CA");
            serialPort_DataSend(serialPortFluo, ":000302310010BA");
            serialPort_DataSend(serialPortFluo, ":000302410010AA");
            serialPort_DataSend(serialPortFluo, ":0003025100109A");
            serialPort_DataSend(serialPortFluo, ":0003026100108A");
            serialPort_DataSend(serialPortFluo, ":0003027100107A");
            serialPort_DataSend(serialPortFluo, ":0003028100106A");
            serialPort_DataSend(serialPortFluo, ":0003029100105A");
            serialPort_DataSend(serialPortFluo, ":000302A100104A");
            serialPort_DataSend(serialPortFluo, ":000302B100103A");
            serialPort_DataSend(serialPortFluo, ":000302C100102A");
            serialPort_DataSend(serialPortFluo, ":000302D100101A");
            serialPort_DataSend(serialPortFluo, ":000302E100100A");
            serialPort_DataSend(serialPortFluo, ":000302F10010FA");
            serialPort_DataSend(serialPortFluo, ":000303010010E9");
            serialPort_DataSend(serialPortFluo, ":000303110010D9");
            serialPort_DataSend(serialPortFluo, ":000303210010C9");
            serialPort_DataSend(serialPortFluo, ":000303310010B9");
            serialPort_DataSend(serialPortFluo, ":000303410010A9");
            serialPort_DataSend(serialPortFluo, ":00030351001099");
            serialPort_DataSend(serialPortFluo, ":00030361000891");
            timer1.Stop();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBoxResult.Clear();
        }    
    }

    
}
