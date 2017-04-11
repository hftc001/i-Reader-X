namespace i_Reader_X
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.paneltime = new System.Windows.Forms.Panel();
            this.labelTimeSecond = new System.Windows.Forms.Label();
            this.labelTimeDay = new System.Windows.Forms.Label();
            this.timersystem = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonRewrite = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.labelbloodX = new System.Windows.Forms.Label();
            this.labelblood = new System.Windows.Forms.Label();
            this.buttonChange = new System.Windows.Forms.Button();
            this.textBoxItem = new System.Windows.Forms.TextBox();
            this.textBoxNum = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labeltestItem = new System.Windows.Forms.Label();
            this.dataGridViewMain = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.测试完成 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.创建时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labeltestnum = new System.Windows.Forms.Label();
            this.labelTesttype = new System.Windows.Forms.Label();
            this.buttonReaderQR = new System.Windows.Forms.Button();
            this.buttonPrint = new System.Windows.Forms.Button();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageMain = new System.Windows.Forms.TabPage();
            this.panelReadQR = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonQRCancel = new System.Windows.Forms.Button();
            this.buttonTest = new System.Windows.Forms.Button();
            this.tabPageResult = new System.Windows.Forms.TabPage();
            this.labelPages = new System.Windows.Forms.Label();
            this.buttonPageDown = new System.Windows.Forms.Button();
            this.buttonPageUp = new System.Windows.Forms.Button();
            this.dataGridViewResult = new System.Windows.Forms.DataGridView();
            this.tabPageSetting = new System.Windows.Forms.TabPage();
            this.groupBoxSetting = new System.Windows.Forms.GroupBox();
            this.labelAutoPrint = new System.Windows.Forms.Label();
            this.buttonAutoPrintSwitch = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.tabPageData = new System.Windows.Forms.TabPage();
            this.chartFluo = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxData = new System.Windows.Forms.TextBox();
            this.tabPageLoad = new System.Windows.Forms.TabPage();
            this.labelLoad = new System.Windows.Forms.Label();
            this.panelMenu = new System.Windows.Forms.Panel();
            this.buttonEngineer = new System.Windows.Forms.Button();
            this.buttonSetting = new System.Windows.Forms.Button();
            this.buttonMain = new System.Windows.Forms.Button();
            this.buttonData = new System.Windows.Forms.Button();
            this.serialPortFluo = new System.IO.Ports.SerialPort(this.components);
            this.serialPortMotor = new System.IO.Ports.SerialPort(this.components);
            this.serialPortQR = new System.IO.Ports.SerialPort(this.components);
            this.timerLoad = new System.Windows.Forms.Timer(this.components);
            this.buttonMin = new System.Windows.Forms.Button();
            this.paneltime.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMain)).BeginInit();
            this.tabControlMain.SuspendLayout();
            this.tabPageMain.SuspendLayout();
            this.panelReadQR.SuspendLayout();
            this.tabPageResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).BeginInit();
            this.tabPageSetting.SuspendLayout();
            this.groupBoxSetting.SuspendLayout();
            this.tabPageData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartFluo)).BeginInit();
            this.tabPageLoad.SuspendLayout();
            this.panelMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // paneltime
            // 
            this.paneltime.BackColor = System.Drawing.Color.SteelBlue;
            this.paneltime.Controls.Add(this.labelTimeSecond);
            this.paneltime.Controls.Add(this.labelTimeDay);
            this.paneltime.Location = new System.Drawing.Point(-1, 447);
            this.paneltime.Name = "paneltime";
            this.paneltime.Size = new System.Drawing.Size(802, 34);
            this.paneltime.TabIndex = 0;
            // 
            // labelTimeSecond
            // 
            this.labelTimeSecond.AutoSize = true;
            this.labelTimeSecond.Font = new System.Drawing.Font("黑体", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTimeSecond.ForeColor = System.Drawing.Color.White;
            this.labelTimeSecond.Location = new System.Drawing.Point(646, 0);
            this.labelTimeSecond.Name = "labelTimeSecond";
            this.labelTimeSecond.Size = new System.Drawing.Size(109, 29);
            this.labelTimeSecond.TabIndex = 1;
            this.labelTimeSecond.Text = "label2";
            // 
            // labelTimeDay
            // 
            this.labelTimeDay.AutoSize = true;
            this.labelTimeDay.Font = new System.Drawing.Font("黑体", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTimeDay.ForeColor = System.Drawing.Color.White;
            this.labelTimeDay.Location = new System.Drawing.Point(3, 0);
            this.labelTimeDay.Name = "labelTimeDay";
            this.labelTimeDay.Size = new System.Drawing.Size(109, 29);
            this.labelTimeDay.TabIndex = 0;
            this.labelTimeDay.Text = "label1";
            // 
            // timersystem
            // 
            this.timersystem.Enabled = true;
            this.timersystem.Interval = 1000;
            this.timersystem.Tick += new System.EventHandler(this.timersystem_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.groupBox1.Controls.Add(this.buttonRewrite);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.textBoxItem);
            this.groupBox1.Controls.Add(this.textBoxNum);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("黑体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(26, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(759, 115);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "项目设置";
            // 
            // buttonRewrite
            // 
            this.buttonRewrite.BackColor = System.Drawing.Color.Transparent;
            this.buttonRewrite.BackgroundImage = global::i_Reader_X.Properties.Resources.button_Black1;
            this.buttonRewrite.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonRewrite.FlatAppearance.BorderSize = 0;
            this.buttonRewrite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRewrite.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonRewrite.ForeColor = System.Drawing.Color.White;
            this.buttonRewrite.Location = new System.Drawing.Point(326, 32);
            this.buttonRewrite.Name = "buttonRewrite";
            this.buttonRewrite.Size = new System.Drawing.Size(120, 67);
            this.buttonRewrite.TabIndex = 10;
            this.buttonRewrite.Text = "修改项目";
            this.buttonRewrite.UseVisualStyleBackColor = false;
            this.buttonRewrite.Click += new System.EventHandler(this.button_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.labelbloodX);
            this.groupBox3.Controls.Add(this.labelblood);
            this.groupBox3.Controls.Add(this.buttonChange);
            this.groupBox3.Location = new System.Drawing.Point(519, 17);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(234, 89);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "样本类型";
            // 
            // labelbloodX
            // 
            this.labelbloodX.AutoSize = true;
            this.labelbloodX.BackColor = System.Drawing.Color.Transparent;
            this.labelbloodX.Font = new System.Drawing.Font("黑体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelbloodX.ForeColor = System.Drawing.Color.DarkGray;
            this.labelbloodX.Location = new System.Drawing.Point(170, 41);
            this.labelbloodX.Name = "labelbloodX";
            this.labelbloodX.Size = new System.Drawing.Size(51, 19);
            this.labelbloodX.TabIndex = 12;
            this.labelbloodX.Text = "血清";
            // 
            // labelblood
            // 
            this.labelblood.AutoSize = true;
            this.labelblood.BackColor = System.Drawing.Color.Transparent;
            this.labelblood.Font = new System.Drawing.Font("黑体", 14.25F, System.Drawing.FontStyle.Bold);
            this.labelblood.Location = new System.Drawing.Point(16, 41);
            this.labelblood.Name = "labelblood";
            this.labelblood.Size = new System.Drawing.Size(51, 19);
            this.labelblood.TabIndex = 11;
            this.labelblood.Text = "血浆";
            // 
            // buttonChange
            // 
            this.buttonChange.BackColor = System.Drawing.Color.Transparent;
            this.buttonChange.BackgroundImage = global::i_Reader_X.Properties.Resources.switch_left;
            this.buttonChange.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonChange.FlatAppearance.BorderSize = 0;
            this.buttonChange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonChange.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonChange.ForeColor = System.Drawing.Color.White;
            this.buttonChange.Location = new System.Drawing.Point(76, 12);
            this.buttonChange.Name = "buttonChange";
            this.buttonChange.Size = new System.Drawing.Size(85, 74);
            this.buttonChange.TabIndex = 11;
            this.buttonChange.UseVisualStyleBackColor = false;
            this.buttonChange.Click += new System.EventHandler(this.button_Click);
            // 
            // textBoxItem
            // 
            this.textBoxItem.Font = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxItem.Location = new System.Drawing.Point(128, 72);
            this.textBoxItem.Name = "textBoxItem";
            this.textBoxItem.Size = new System.Drawing.Size(192, 30);
            this.textBoxItem.TabIndex = 2;
            this.textBoxItem.Text = "BNP";
            // 
            // textBoxNum
            // 
            this.textBoxNum.Font = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxNum.Location = new System.Drawing.Point(128, 32);
            this.textBoxNum.Name = "textBoxNum";
            this.textBoxNum.Size = new System.Drawing.Size(192, 30);
            this.textBoxNum.TabIndex = 0;
            this.textBoxNum.Text = "001";
            this.textBoxNum.Click += new System.EventHandler(this.textBox_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("黑体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(8, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 19);
            this.label1.TabIndex = 3;
            this.label1.Text = "输入样本号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("黑体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(8, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 19);
            this.label2.TabIndex = 4;
            this.label2.Text = "测试项目：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labeltestItem);
            this.groupBox2.Controls.Add(this.dataGridViewMain);
            this.groupBox2.Controls.Add(this.labeltestnum);
            this.groupBox2.Controls.Add(this.labelTesttype);
            this.groupBox2.Font = new System.Drawing.Font("黑体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(26, 131);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(470, 197);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "测试结果";
            // 
            // labeltestItem
            // 
            this.labeltestItem.AutoSize = true;
            this.labeltestItem.BackColor = System.Drawing.Color.Transparent;
            this.labeltestItem.Font = new System.Drawing.Font("黑体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labeltestItem.Location = new System.Drawing.Point(6, 66);
            this.labeltestItem.Name = "labeltestItem";
            this.labeltestItem.Size = new System.Drawing.Size(114, 19);
            this.labeltestItem.TabIndex = 6;
            this.labeltestItem.Text = "测试项目：";
            // 
            // dataGridViewMain
            // 
            this.dataGridViewMain.AllowUserToAddRows = false;
            this.dataGridViewMain.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.dataGridViewMain.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridViewMain.ColumnHeadersHeight = 40;
            this.dataGridViewMain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.测试完成,
            this.创建时间});
            this.dataGridViewMain.Location = new System.Drawing.Point(6, 105);
            this.dataGridViewMain.MultiSelect = false;
            this.dataGridViewMain.Name = "dataGridViewMain";
            this.dataGridViewMain.ReadOnly = true;
            this.dataGridViewMain.RowHeadersVisible = false;
            this.dataGridViewMain.RowTemplate.Height = 40;
            this.dataGridViewMain.Size = new System.Drawing.Size(453, 80);
            this.dataGridViewMain.TabIndex = 5;
            this.dataGridViewMain.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "样本号";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 175;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "测试项目";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 125;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "测试状态";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 150;
            // 
            // 测试完成
            // 
            this.测试完成.HeaderText = "测试完成";
            this.测试完成.Name = "测试完成";
            this.测试完成.ReadOnly = true;
            this.测试完成.Visible = false;
            // 
            // 创建时间
            // 
            this.创建时间.HeaderText = "创建时间";
            this.创建时间.Name = "创建时间";
            this.创建时间.ReadOnly = true;
            this.创建时间.Visible = false;
            // 
            // labeltestnum
            // 
            this.labeltestnum.AutoSize = true;
            this.labeltestnum.BackColor = System.Drawing.Color.Transparent;
            this.labeltestnum.Font = new System.Drawing.Font("黑体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labeltestnum.Location = new System.Drawing.Point(6, 32);
            this.labeltestnum.Name = "labeltestnum";
            this.labeltestnum.Size = new System.Drawing.Size(115, 19);
            this.labeltestnum.TabIndex = 3;
            this.labeltestnum.Text = "样 本 号：";
            // 
            // labelTesttype
            // 
            this.labelTesttype.AutoSize = true;
            this.labelTesttype.BackColor = System.Drawing.Color.Transparent;
            this.labelTesttype.Font = new System.Drawing.Font("黑体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTesttype.Location = new System.Drawing.Point(223, 66);
            this.labelTesttype.Name = "labelTesttype";
            this.labelTesttype.Size = new System.Drawing.Size(114, 19);
            this.labelTesttype.TabIndex = 4;
            this.labelTesttype.Text = "样本类型：";
            // 
            // buttonReaderQR
            // 
            this.buttonReaderQR.BackColor = System.Drawing.Color.Transparent;
            this.buttonReaderQR.BackgroundImage = global::i_Reader_X.Properties.Resources.button_Black1;
            this.buttonReaderQR.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonReaderQR.FlatAppearance.BorderSize = 0;
            this.buttonReaderQR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonReaderQR.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonReaderQR.ForeColor = System.Drawing.Color.White;
            this.buttonReaderQR.Location = new System.Drawing.Point(627, 163);
            this.buttonReaderQR.Name = "buttonReaderQR";
            this.buttonReaderQR.Size = new System.Drawing.Size(164, 67);
            this.buttonReaderQR.TabIndex = 8;
            this.buttonReaderQR.Text = "读卡";
            this.buttonReaderQR.UseVisualStyleBackColor = false;
            this.buttonReaderQR.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonPrint
            // 
            this.buttonPrint.BackColor = System.Drawing.Color.Transparent;
            this.buttonPrint.BackgroundImage = global::i_Reader_X.Properties.Resources.button_Black1;
            this.buttonPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonPrint.FlatAppearance.BorderSize = 0;
            this.buttonPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPrint.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonPrint.ForeColor = System.Drawing.Color.White;
            this.buttonPrint.Location = new System.Drawing.Point(627, 309);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(164, 67);
            this.buttonPrint.TabIndex = 9;
            this.buttonPrint.Text = "打印";
            this.buttonPrint.UseVisualStyleBackColor = false;
            this.buttonPrint.Click += new System.EventHandler(this.button_Click);
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageMain);
            this.tabControlMain.Controls.Add(this.tabPageResult);
            this.tabControlMain.Controls.Add(this.tabPageSetting);
            this.tabControlMain.Controls.Add(this.tabPageData);
            this.tabControlMain.Controls.Add(this.tabPageLoad);
            this.tabControlMain.Location = new System.Drawing.Point(-7, 43);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(816, 413);
            this.tabControlMain.TabIndex = 10;
            // 
            // tabPageMain
            // 
            this.tabPageMain.BackColor = System.Drawing.Color.Lavender;
            this.tabPageMain.Controls.Add(this.panelReadQR);
            this.tabPageMain.Controls.Add(this.buttonTest);
            this.tabPageMain.Controls.Add(this.groupBox1);
            this.tabPageMain.Controls.Add(this.buttonPrint);
            this.tabPageMain.Controls.Add(this.groupBox2);
            this.tabPageMain.Controls.Add(this.buttonReaderQR);
            this.tabPageMain.Location = new System.Drawing.Point(4, 22);
            this.tabPageMain.Name = "tabPageMain";
            this.tabPageMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMain.Size = new System.Drawing.Size(808, 387);
            this.tabPageMain.TabIndex = 0;
            this.tabPageMain.Text = "tabPage1";
            // 
            // panelReadQR
            // 
            this.panelReadQR.Controls.Add(this.label3);
            this.panelReadQR.Controls.Add(this.buttonQRCancel);
            this.panelReadQR.Location = new System.Drawing.Point(503, 131);
            this.panelReadQR.Name = "panelReadQR";
            this.panelReadQR.Size = new System.Drawing.Size(299, 250);
            this.panelReadQR.TabIndex = 11;
            this.panelReadQR.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(15, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(164, 26);
            this.label3.TabIndex = 11;
            this.label3.Text = "请扫描二维码信息";
            // 
            // buttonQRCancel
            // 
            this.buttonQRCancel.BackColor = System.Drawing.Color.Transparent;
            this.buttonQRCancel.BackgroundImage = global::i_Reader_X.Properties.Resources.button_Black1;
            this.buttonQRCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonQRCancel.FlatAppearance.BorderSize = 0;
            this.buttonQRCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonQRCancel.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonQRCancel.ForeColor = System.Drawing.Color.White;
            this.buttonQRCancel.Location = new System.Drawing.Point(124, 178);
            this.buttonQRCancel.Name = "buttonQRCancel";
            this.buttonQRCancel.Size = new System.Drawing.Size(164, 67);
            this.buttonQRCancel.TabIndex = 10;
            this.buttonQRCancel.Text = "取消";
            this.buttonQRCancel.UseVisualStyleBackColor = false;
            this.buttonQRCancel.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonTest
            // 
            this.buttonTest.BackColor = System.Drawing.Color.Transparent;
            this.buttonTest.BackgroundImage = global::i_Reader_X.Properties.Resources.button_Black1;
            this.buttonTest.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonTest.FlatAppearance.BorderSize = 0;
            this.buttonTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTest.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonTest.ForeColor = System.Drawing.Color.White;
            this.buttonTest.Location = new System.Drawing.Point(627, 236);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(164, 67);
            this.buttonTest.TabIndex = 10;
            this.buttonTest.Text = "测试";
            this.buttonTest.UseVisualStyleBackColor = false;
            this.buttonTest.Click += new System.EventHandler(this.button_Click);
            // 
            // tabPageResult
            // 
            this.tabPageResult.BackColor = System.Drawing.Color.Lavender;
            this.tabPageResult.Controls.Add(this.labelPages);
            this.tabPageResult.Controls.Add(this.buttonPageDown);
            this.tabPageResult.Controls.Add(this.buttonPageUp);
            this.tabPageResult.Controls.Add(this.dataGridViewResult);
            this.tabPageResult.Location = new System.Drawing.Point(4, 22);
            this.tabPageResult.Name = "tabPageResult";
            this.tabPageResult.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageResult.Size = new System.Drawing.Size(808, 387);
            this.tabPageResult.TabIndex = 1;
            this.tabPageResult.Text = "tabPageResult";
            // 
            // labelPages
            // 
            this.labelPages.AutoSize = true;
            this.labelPages.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelPages.Location = new System.Drawing.Point(222, 333);
            this.labelPages.Name = "labelPages";
            this.labelPages.Size = new System.Drawing.Size(59, 22);
            this.labelPages.TabIndex = 12;
            this.labelPages.Text = "label3";
            // 
            // buttonPageDown
            // 
            this.buttonPageDown.BackColor = System.Drawing.Color.Transparent;
            this.buttonPageDown.BackgroundImage = global::i_Reader_X.Properties.Resources.button_Black1;
            this.buttonPageDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonPageDown.FlatAppearance.BorderSize = 0;
            this.buttonPageDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPageDown.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonPageDown.ForeColor = System.Drawing.Color.White;
            this.buttonPageDown.Location = new System.Drawing.Point(411, 317);
            this.buttonPageDown.Name = "buttonPageDown";
            this.buttonPageDown.Size = new System.Drawing.Size(99, 50);
            this.buttonPageDown.TabIndex = 11;
            this.buttonPageDown.Text = "下一页";
            this.buttonPageDown.UseVisualStyleBackColor = false;
            this.buttonPageDown.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonPageUp
            // 
            this.buttonPageUp.BackColor = System.Drawing.Color.Transparent;
            this.buttonPageUp.BackgroundImage = global::i_Reader_X.Properties.Resources.button_Black1;
            this.buttonPageUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonPageUp.FlatAppearance.BorderSize = 0;
            this.buttonPageUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPageUp.Font = new System.Drawing.Font("黑体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonPageUp.ForeColor = System.Drawing.Color.White;
            this.buttonPageUp.Location = new System.Drawing.Point(10, 317);
            this.buttonPageUp.Name = "buttonPageUp";
            this.buttonPageUp.Size = new System.Drawing.Size(99, 50);
            this.buttonPageUp.TabIndex = 10;
            this.buttonPageUp.Text = "上一页";
            this.buttonPageUp.UseVisualStyleBackColor = false;
            this.buttonPageUp.Click += new System.EventHandler(this.button_Click);
            // 
            // dataGridViewResult
            // 
            this.dataGridViewResult.AllowUserToAddRows = false;
            this.dataGridViewResult.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.dataGridViewResult.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewResult.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResult.Location = new System.Drawing.Point(10, 10);
            this.dataGridViewResult.MultiSelect = false;
            this.dataGridViewResult.Name = "dataGridViewResult";
            this.dataGridViewResult.ReadOnly = true;
            this.dataGridViewResult.RowHeadersVisible = false;
            this.dataGridViewResult.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dataGridViewResult.RowTemplate.Height = 30;
            this.dataGridViewResult.RowTemplate.ReadOnly = true;
            this.dataGridViewResult.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewResult.Size = new System.Drawing.Size(500, 300);
            this.dataGridViewResult.TabIndex = 0;
            // 
            // tabPageSetting
            // 
            this.tabPageSetting.BackColor = System.Drawing.Color.Lavender;
            this.tabPageSetting.Controls.Add(this.buttonMin);
            this.tabPageSetting.Controls.Add(this.groupBoxSetting);
            this.tabPageSetting.Controls.Add(this.buttonClose);
            this.tabPageSetting.Location = new System.Drawing.Point(4, 22);
            this.tabPageSetting.Name = "tabPageSetting";
            this.tabPageSetting.Size = new System.Drawing.Size(808, 387);
            this.tabPageSetting.TabIndex = 2;
            this.tabPageSetting.Text = "tabPage3";
            // 
            // groupBoxSetting
            // 
            this.groupBoxSetting.Controls.Add(this.labelAutoPrint);
            this.groupBoxSetting.Controls.Add(this.buttonAutoPrintSwitch);
            this.groupBoxSetting.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxSetting.Location = new System.Drawing.Point(30, 31);
            this.groupBoxSetting.Name = "groupBoxSetting";
            this.groupBoxSetting.Size = new System.Drawing.Size(316, 314);
            this.groupBoxSetting.TabIndex = 3;
            this.groupBoxSetting.TabStop = false;
            this.groupBoxSetting.Text = "基础设置";
            // 
            // labelAutoPrint
            // 
            this.labelAutoPrint.AutoSize = true;
            this.labelAutoPrint.BackColor = System.Drawing.Color.Transparent;
            this.labelAutoPrint.Font = new System.Drawing.Font("黑体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelAutoPrint.Location = new System.Drawing.Point(15, 55);
            this.labelAutoPrint.Name = "labelAutoPrint";
            this.labelAutoPrint.Size = new System.Drawing.Size(114, 19);
            this.labelAutoPrint.TabIndex = 5;
            this.labelAutoPrint.Text = "自动打印关";
            // 
            // buttonAutoPrintSwitch
            // 
            this.buttonAutoPrintSwitch.BackColor = System.Drawing.Color.Transparent;
            this.buttonAutoPrintSwitch.BackgroundImage = global::i_Reader_X.Properties.Resources.switch_left;
            this.buttonAutoPrintSwitch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonAutoPrintSwitch.FlatAppearance.BorderSize = 0;
            this.buttonAutoPrintSwitch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAutoPrintSwitch.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonAutoPrintSwitch.ForeColor = System.Drawing.Color.White;
            this.buttonAutoPrintSwitch.Location = new System.Drawing.Point(135, 32);
            this.buttonAutoPrintSwitch.Name = "buttonAutoPrintSwitch";
            this.buttonAutoPrintSwitch.Size = new System.Drawing.Size(76, 57);
            this.buttonAutoPrintSwitch.TabIndex = 4;
            this.buttonAutoPrintSwitch.UseVisualStyleBackColor = false;
            this.buttonAutoPrintSwitch.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.Color.Transparent;
            this.buttonClose.BackgroundImage = global::i_Reader_X.Properties.Resources.button_Black;
            this.buttonClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonClose.FlatAppearance.BorderSize = 0;
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonClose.ForeColor = System.Drawing.Color.White;
            this.buttonClose.Location = new System.Drawing.Point(688, 290);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(97, 90);
            this.buttonClose.TabIndex = 2;
            this.buttonClose.Text = "CLOSE";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.button_Click);
            // 
            // tabPageData
            // 
            this.tabPageData.BackColor = System.Drawing.Color.Lavender;
            this.tabPageData.Controls.Add(this.chartFluo);
            this.tabPageData.Controls.Add(this.button1);
            this.tabPageData.Controls.Add(this.textBoxData);
            this.tabPageData.Location = new System.Drawing.Point(4, 22);
            this.tabPageData.Name = "tabPageData";
            this.tabPageData.Size = new System.Drawing.Size(808, 387);
            this.tabPageData.TabIndex = 3;
            this.tabPageData.Text = "tabPage1";
            // 
            // chartFluo
            // 
            chartArea1.Name = "ChartArea1";
            this.chartFluo.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartFluo.Legends.Add(legend1);
            this.chartFluo.Location = new System.Drawing.Point(331, 139);
            this.chartFluo.Name = "chartFluo";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartFluo.Series.Add(series1);
            this.chartFluo.Size = new System.Drawing.Size(460, 237);
            this.chartFluo.TabIndex = 3;
            this.chartFluo.Text = "chart1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(331, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxData
            // 
            this.textBoxData.Location = new System.Drawing.Point(16, 11);
            this.textBoxData.Multiline = true;
            this.textBoxData.Name = "textBoxData";
            this.textBoxData.Size = new System.Drawing.Size(300, 365);
            this.textBoxData.TabIndex = 0;
            // 
            // tabPageLoad
            // 
            this.tabPageLoad.BackColor = System.Drawing.Color.Lavender;
            this.tabPageLoad.Controls.Add(this.labelLoad);
            this.tabPageLoad.Location = new System.Drawing.Point(4, 22);
            this.tabPageLoad.Name = "tabPageLoad";
            this.tabPageLoad.Size = new System.Drawing.Size(808, 387);
            this.tabPageLoad.TabIndex = 4;
            this.tabPageLoad.Text = "tabPageLoad";
            // 
            // labelLoad
            // 
            this.labelLoad.AutoSize = true;
            this.labelLoad.Font = new System.Drawing.Font("幼圆", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelLoad.Location = new System.Drawing.Point(294, 166);
            this.labelLoad.Name = "labelLoad";
            this.labelLoad.Size = new System.Drawing.Size(185, 33);
            this.labelLoad.TabIndex = 0;
            this.labelLoad.Text = "Loading...";
            // 
            // panelMenu
            // 
            this.panelMenu.BackgroundImage = global::i_Reader_X.Properties.Resources.topbg;
            this.panelMenu.Controls.Add(this.buttonEngineer);
            this.panelMenu.Controls.Add(this.buttonSetting);
            this.panelMenu.Controls.Add(this.buttonMain);
            this.panelMenu.Controls.Add(this.buttonData);
            this.panelMenu.Location = new System.Drawing.Point(-1, -1);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(803, 70);
            this.panelMenu.TabIndex = 11;
            // 
            // buttonEngineer
            // 
            this.buttonEngineer.BackColor = System.Drawing.Color.Transparent;
            this.buttonEngineer.BackgroundImage = global::i_Reader_X.Properties.Resources.topbg;
            this.buttonEngineer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonEngineer.FlatAppearance.BorderSize = 0;
            this.buttonEngineer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEngineer.Location = new System.Drawing.Point(0, 0);
            this.buttonEngineer.Name = "buttonEngineer";
            this.buttonEngineer.Size = new System.Drawing.Size(314, 70);
            this.buttonEngineer.TabIndex = 3;
            this.buttonEngineer.UseVisualStyleBackColor = false;
            this.buttonEngineer.Click += new System.EventHandler(this.buttonmenu_Click);
            // 
            // buttonSetting
            // 
            this.buttonSetting.BackgroundImage = global::i_Reader_X.Properties.Resources.Setting_Normal;
            this.buttonSetting.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonSetting.FlatAppearance.BorderSize = 0;
            this.buttonSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSetting.Location = new System.Drawing.Point(729, 5);
            this.buttonSetting.Name = "buttonSetting";
            this.buttonSetting.Size = new System.Drawing.Size(60, 60);
            this.buttonSetting.TabIndex = 2;
            this.buttonSetting.UseVisualStyleBackColor = true;
            this.buttonSetting.Visible = false;
            this.buttonSetting.Click += new System.EventHandler(this.buttonmenu_Click);
            // 
            // buttonMain
            // 
            this.buttonMain.BackgroundImage = global::i_Reader_X.Properties.Resources.Home_Normal;
            this.buttonMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonMain.FlatAppearance.BorderSize = 0;
            this.buttonMain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMain.Location = new System.Drawing.Point(597, 7);
            this.buttonMain.Name = "buttonMain";
            this.buttonMain.Size = new System.Drawing.Size(60, 60);
            this.buttonMain.TabIndex = 1;
            this.buttonMain.UseVisualStyleBackColor = true;
            this.buttonMain.Visible = false;
            this.buttonMain.Click += new System.EventHandler(this.buttonmenu_Click);
            // 
            // buttonData
            // 
            this.buttonData.BackgroundImage = global::i_Reader_X.Properties.Resources.Search_Normal;
            this.buttonData.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonData.FlatAppearance.BorderSize = 0;
            this.buttonData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonData.Location = new System.Drawing.Point(663, 5);
            this.buttonData.Name = "buttonData";
            this.buttonData.Size = new System.Drawing.Size(60, 60);
            this.buttonData.TabIndex = 0;
            this.buttonData.UseVisualStyleBackColor = true;
            this.buttonData.Visible = false;
            this.buttonData.Click += new System.EventHandler(this.buttonmenu_Click);
            // 
            // serialPortFluo
            // 
            this.serialPortFluo.BaudRate = 57600;
            this.serialPortFluo.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPortFluo_DataReceived);
            // 
            // serialPortMotor
            // 
            this.serialPortMotor.BaudRate = 38400;
            // 
            // serialPortQR
            // 
            this.serialPortQR.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPortQR_DataReceived);
            // 
            // timerLoad
            // 
            this.timerLoad.Tick += new System.EventHandler(this.timerLoad_Tick);
            // 
            // buttonMin
            // 
            this.buttonMin.BackColor = System.Drawing.Color.Transparent;
            this.buttonMin.BackgroundImage = global::i_Reader_X.Properties.Resources.button_Black;
            this.buttonMin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonMin.FlatAppearance.BorderSize = 0;
            this.buttonMin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMin.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonMin.ForeColor = System.Drawing.Color.White;
            this.buttonMin.Location = new System.Drawing.Point(688, 194);
            this.buttonMin.Name = "buttonMin";
            this.buttonMin.Size = new System.Drawing.Size(97, 90);
            this.buttonMin.TabIndex = 4;
            this.buttonMin.Text = "最小化";
            this.buttonMin.UseVisualStyleBackColor = false;
            this.buttonMin.Click += new System.EventHandler(this.button_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::i_Reader_X.Properties.Resources.Background4;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(800, 480);
            this.Controls.Add(this.panelMenu);
            this.Controls.Add(this.paneltime);
            this.Controls.Add(this.tabControlMain);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "i-Reader X";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.paneltime.ResumeLayout(false);
            this.paneltime.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMain)).EndInit();
            this.tabControlMain.ResumeLayout(false);
            this.tabPageMain.ResumeLayout(false);
            this.panelReadQR.ResumeLayout(false);
            this.panelReadQR.PerformLayout();
            this.tabPageResult.ResumeLayout(false);
            this.tabPageResult.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).EndInit();
            this.tabPageSetting.ResumeLayout(false);
            this.groupBoxSetting.ResumeLayout(false);
            this.groupBoxSetting.PerformLayout();
            this.tabPageData.ResumeLayout(false);
            this.tabPageData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartFluo)).EndInit();
            this.tabPageLoad.ResumeLayout(false);
            this.tabPageLoad.PerformLayout();
            this.panelMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel paneltime;
        private System.Windows.Forms.Label labelTimeSecond;
        private System.Windows.Forms.Label labelTimeDay;
        private System.Windows.Forms.Timer timersystem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxItem;
        private System.Windows.Forms.TextBox textBoxNum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labeltestnum;
        private System.Windows.Forms.Label labelTesttype;
        private System.Windows.Forms.DataGridView dataGridViewMain;
        private System.Windows.Forms.Button buttonReaderQR;
        private System.Windows.Forms.Button buttonPrint;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonRewrite;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageMain;
        private System.Windows.Forms.TabPage tabPageResult;
        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Button buttonData;
        private System.Windows.Forms.Button buttonMain;
        private System.Windows.Forms.Button buttonSetting;
        private System.Windows.Forms.TabPage tabPageSetting;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label labeltestItem;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.Label labelbloodX;
        private System.Windows.Forms.Label labelblood;
        private System.Windows.Forms.Button buttonChange;
        private System.Windows.Forms.DataGridView dataGridViewResult;
        private System.Windows.Forms.TabPage tabPageData;
        private System.Windows.Forms.Button buttonEngineer;
        private System.Windows.Forms.TextBox textBoxData;
        private System.IO.Ports.SerialPort serialPortFluo;
        private System.IO.Ports.SerialPort serialPortMotor;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartFluo;
        private System.Windows.Forms.GroupBox groupBoxSetting;
        private System.Windows.Forms.Button buttonPageDown;
        private System.Windows.Forms.Button buttonPageUp;
        private System.Windows.Forms.Label labelPages;
        private System.IO.Ports.SerialPort serialPortQR;
        private System.Windows.Forms.Panel panelReadQR;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonQRCancel;
        private System.Windows.Forms.Button buttonAutoPrintSwitch;
        private System.Windows.Forms.Label labelAutoPrint;
        private System.Windows.Forms.Timer timerLoad;
        private System.Windows.Forms.TabPage tabPageLoad;
        private System.Windows.Forms.Label labelLoad;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn 测试完成;
        private System.Windows.Forms.DataGridViewTextBoxColumn 创建时间;
        private System.Windows.Forms.Button buttonMin;
    }
}

