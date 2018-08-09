using Department2Base.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Linq;
using System.Diagnostics;

namespace Department2Base
{
    public partial class Profile : Form
    {
        private bool loadMode = false;
        public static string DownSign
        {
            get
            {
                return label1.Text;
            }
            set
            {
                label1.Text = value;
            }
        }

        public bool SwitchLoadMode
        {
            get
            {
                return loadMode;
            }
            set
            {
                ChangeState();
                loadMode = value;
            }
        }

        public static string userName = null, userLogin = null;
        public static int userDepartment = -1;

        Supports sup = new Supports();
        Bitmap x1;
        Bitmap x2;
        Bitmap x3;
        Bitmap x4;

        public void ChangeState(bool off = false)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool>(ChangeState), new object[] { off });
                return;
            }

            if (BackColor == Supports.headOrange || off)
            {
                BackColor = Supports.headBlue;
                panel2.BackColor = Supports.headBlue;
                pictureBox5.Visible = false;
                видToolStripMenuItem.Enabled = true;
            }
            else
            {
                BackColor = Supports.headOrange;
                panel2.BackColor = Supports.headOrange;
                pictureBox5.Visible = true;
                видToolStripMenuItem.Enabled = false;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            Padding = new Padding(0);
            panel5.Padding = new Padding(0);
            base.OnShown(e);
        }

        private void сменитьПрофильToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void светлаяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Supports.textWhite == Supports.ColorSetLite.textWhite)
                return;

            dataBase.SimpleRequest("UPDATE [MainSettings] SET [Content] = 'Lite'  WHERE [Who] = 'ColorStyle' AND [What] = '" + Profile.userLogin + "'");

            Supports.headGrey = Supports.ColorSetLite.headGrey;
            Supports.headliteGrey = Supports.ColorSetLite.headliteGrey;
            Supports.textBlack = Supports.ColorSetLite.textBlack;
            Supports.darkBlue = Supports.ColorSetLite.darkBlue;
            Supports.backBlack = Supports.ColorSetLite.backBlack;
            Supports.textGray = Supports.ColorSetLite.textGray;
            Supports.liteTextGray = Supports.ColorSetLite.liteTextGray;
            Supports.headBlue = Supports.ColorSetLite.headBlue;
            Supports.headOrange = Supports.ColorSetLite.headOrange;
            Supports.textWhite = Supports.ColorSetLite.textWhite;
            Supports.groupGrey = Supports.ColorSetLite.groupGrey;
            Supports.Red = Supports.ColorSetLite.Red;
            Supports.Green = Supports.ColorSetLite.Green;
            Supports.LiteHeadBlue = Supports.ColorSetLite.LiteHeadBlue;
            Supports.LiteTextBlue = Supports.ColorSetLite.LiteTextBlue;
            Supports.StringBrush = Supports.ColorSetLite.StringBrush;
            Supports.StringPen = Supports.ColorSetLite.StringPen;


            Supports.GangeGroup(this);
            panel2.BackColor = Supports.headBlue;

            if (panel4.BackColor.R < 122)
            {
                x1 = Resources.x1tw;
                x2 = Resources.x2tw;
                x3 = Resources.x3tw;
                x4 = Resources.x7tw;
            }
            else
            {
                x1 = Resources.x1tb;
                x2 = Resources.x2tb;
                x3 = Resources.x3tb;
                x4 = Resources.x7tb;
            }

            pictureBox1.Image = x1;
            pictureBox2.Image = x2;
            pictureBox3.Image = x3;

            tabControl1.Invalidate();

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void тёмнаяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Supports.textWhite == Supports.ColorSetDark.textWhite)
                return;

            dataBase.SimpleRequest("UPDATE [MainSettings] SET [Content] = 'Dark'  WHERE [Who] = 'ColorStyle' AND [What] = '" + Profile.userLogin + "'");

            Supports.headGrey = Supports.ColorSetDark.headGrey;
            Supports.headliteGrey = Supports.ColorSetDark.headliteGrey;
            Supports.textBlack = Supports.ColorSetDark.textBlack;
            Supports.darkBlue = Supports.ColorSetDark.darkBlue;
            Supports.backBlack = Supports.ColorSetDark.backBlack;
            Supports.textGray = Supports.ColorSetDark.textGray;
            Supports.liteTextGray = Supports.ColorSetDark.liteTextGray;
            Supports.headBlue = Supports.ColorSetDark.headBlue;
            Supports.headOrange = Supports.ColorSetDark.headOrange;
            Supports.textWhite = Supports.ColorSetDark.textWhite;
            Supports.groupGrey = Supports.ColorSetDark.groupGrey;
            Supports.Red = Supports.ColorSetDark.Red;
            Supports.Green = Supports.ColorSetDark.Green;
            Supports.LiteHeadBlue = Supports.ColorSetDark.LiteHeadBlue;
            Supports.LiteTextBlue = Supports.ColorSetDark.LiteTextBlue;
            Supports.StringBrush = Supports.ColorSetDark.StringBrush;
            Supports.StringPen = Supports.ColorSetDark.StringPen;

            Supports.GangeGroup(this);
            panel2.BackColor = Supports.headBlue;

            if (panel4.BackColor.R < 122)
            {
                x1 = Resources.x1tw;
                x2 = Resources.x2tw;
                x3 = Resources.x3tw;
                x4 = Resources.x7tw;
            }
            else
            {
                x1 = Resources.x1tb;
                x2 = Resources.x2tb;
                x3 = Resources.x3tb;
                x4 = Resources.x7tb;
            }

            pictureBox1.Image = x1;
            pictureBox2.Image = x2;
            pictureBox3.Image = x3;

            tabControl1.Invalidate();

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }


        int RESIZE_HANDLE_SIZE = 0;
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0xA3:
                    {

                    }

                    break;
                case 0x84:
                    Point clientPoint = PointToClient(new Point(m.LParam.ToInt32()));
                    if (clientPoint.Y <= RESIZE_HANDLE_SIZE)
                    {
                        if (clientPoint.X <= RESIZE_HANDLE_SIZE)
                            m.Result = (IntPtr)13;
                        else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
                            if (WindowState == FormWindowState.Maximized)
                                m.Result = (IntPtr)0;
                            else m.Result = (IntPtr)12;
                        else m.Result = (IntPtr)14;
                    }
                    else if (clientPoint.Y <= (Size.Height - RESIZE_HANDLE_SIZE))
                    {
                        if (clientPoint.X <= RESIZE_HANDLE_SIZE)
                            m.Result = (IntPtr)10;
                        else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
                            base.WndProc(ref m);
                        else m.Result = (IntPtr)11;
                    }
                    else
                    {
                        if (clientPoint.X <= RESIZE_HANDLE_SIZE)
                            m.Result = (IntPtr)16;
                        else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
                            m.Result = (IntPtr)15;
                        else m.Result = (IntPtr)17;

                    }
                    if ((int)m.Result == 0x1)
                        m.Result = (IntPtr)0x2;

                    return;
            }
            base.WndProc(ref m);
        }

        private void prepare()
        {
            try
            {
                MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;

                if (Environment.OSVersion.Version.Build < 4000)
                    BackColor = Supports.headBlue;

                bool lite = false;

                if (panel4.BackColor.R < 122)
                    lite = true;
                if (lite)
                {
                    x1 = Resources.x1tw;
                    x2 = Resources.x2tw;
                    x3 = Resources.x3tw;
                    x4 = Resources.x7tw;
                }
                else
                {
                    x1 = Resources.x1tb;
                    x2 = Resources.x2tb;
                    x3 = Resources.x3tb;
                    x4 = Resources.x7tb;
                }

                pictureBox1.Image = x1;
                pictureBox2.Image = x2;
                pictureBox3.Image = x3;

                FormWindowState prevState = WindowState;
                MouseUp += (s, arg) =>
                {
                    if (Location.Y < 0 && Cursor.Position.Y == 0)
                    {
                        WindowState = FormWindowState.Maximized;
                        pictureBox2.Image = x2;
                        ChangePadd();
                        return;
                    }
                    else if (Location.X < 0 && Cursor.Position.X == 0)
                    {
                        Height = Screen.FromControl(this).Bounds.Height;
                        Width = Screen.FromControl(this).Bounds.Width / 2;
                    }

                    if (Location.Y < 0)
                    {
                        Location = new Point(Location.X, 0);
                        return;
                    }
                };
                Deactivate += (s, arg) =>
                {
                    BackColor = Color.Gray;
                };
                Activated += (s, arg) =>
                {
                    BackColor = Supports.headBlue;
                };
                Resize += (s, arg) =>
                {
                    if (prevState != WindowState)
                    {
                        prevState = WindowState;
                        ChangePadd();
                        if (WindowState == FormWindowState.Maximized)
                            pictureBox2.Image = x2;
                        else pictureBox2.Image = x4;
                    }
                };

                pictureBox1.MouseEnter += (s, arg) => pictureBox1.BackColor = Supports.liteTextGray;
                pictureBox1.MouseUp += (s, arg) => pictureBox1.BackColor = Supports.headGrey;
                pictureBox1.MouseDown += (s, arg) => pictureBox1.BackColor = Supports.headBlue;
                pictureBox1.MouseLeave += (s, arg) => pictureBox1.BackColor = Supports.headGrey;
                pictureBox1.MouseClick += (s, arg) =>
                {
                    if (MessageBoxTi.Show("Завершение работы", "Точно выйти?") != MessageResult.Yes)
                        return;
                    Supports.WorkActivity.Stop();
                    Close();
                };
                pictureBox2.MouseEnter += (s, arg) => pictureBox2.BackColor = Supports.liteTextGray;
                pictureBox2.MouseLeave += (s, arg) => pictureBox2.BackColor = Supports.headGrey;
                pictureBox2.MouseUp += (s, arg) => pictureBox2.BackColor = Supports.headGrey;
                pictureBox2.MouseDown += (s, arg) => pictureBox2.BackColor = Supports.headBlue;

                pictureBox2.MouseClick += (s, arg) =>
                {
                    if (WindowState == FormWindowState.Maximized)
                        WindowState = FormWindowState.Normal;
                    else WindowState = FormWindowState.Maximized;
                };

                pictureBox3.MouseEnter += (s, arg) => pictureBox3.BackColor = Supports.liteTextGray;
                pictureBox3.MouseLeave += (s, arg) => pictureBox3.BackColor = Supports.headGrey;
                pictureBox3.MouseUp += (s, arg) => pictureBox3.BackColor = Supports.headGrey;
                pictureBox3.MouseDown += (s, arg) => pictureBox3.BackColor = Supports.headBlue;
                pictureBox3.MouseClick += (s, arg) => WindowState = FormWindowState.Minimized;

                splitContainer1.Visible = (splitContainer1.Panel1.Controls.Count == 0 && splitContainer1.Panel2.Controls.Count == 0 ? false : true);
                splitContainer2.Visible = (splitContainer2.Panel1.Controls.Count == 0 && splitContainer2.Panel2.Controls.Count == 0 ? false : true);
                splitter1.Visible = (splitContainer1.Panel1.Controls.Count == 0 && splitContainer1.Panel2.Controls.Count == 0 ? false : true);
                splitter2.Visible = (splitContainer2.Panel1.Controls.Count == 0 && splitContainer2.Panel2.Controls.Count == 0 ? false : true);

                splitContainer1.Panel1.ControlAdded += (s, arg) =>
                {
                    splitContainer1.Panel1Collapsed = false;
                    if (!splitContainer1.Visible)
                    {
                        splitContainer1.Visible = true;
                        splitter1.Visible = true;
                    }

                };
                splitContainer1.Panel2.ControlAdded += (s, arg) =>
                {
                    splitContainer1.Panel2Collapsed = false;
                    if (!splitContainer1.Visible)
                    {
                        splitContainer1.Visible = true;
                        splitter1.Visible = true;
                    }
                };
                splitContainer2.Panel1.ControlAdded += (s, arg) =>
                {
                    splitContainer2.Panel1Collapsed = false;
                    if (!splitContainer2.Visible)
                    {
                        splitContainer2.Visible = true;
                        splitter2.Visible = true;
                    }
                };
                splitContainer2.Panel2.ControlAdded += (s, arg) =>
                {
                    splitContainer2.Panel2Collapsed = false;
                    if (!splitContainer2.Visible)
                    {
                        splitContainer2.Visible = true;
                        splitter2.Visible = true;
                    }
                };

                splitContainer1.Panel1.ControlRemoved += (s, arg) =>
                {
                    splitContainer1.Panel1Collapsed = (splitContainer1.Panel1.Controls.Count == 0 ? true : false);
                    splitContainer1.Visible = (splitContainer1.Panel1.Controls.Count == 0 && splitContainer1.Panel2.Controls.Count == 0 ? false : true);
                    splitter1.Visible = (splitContainer1.Panel1.Controls.Count == 0 && splitContainer1.Panel2.Controls.Count == 0 ? false : true);
                };
                splitContainer1.Panel2.ControlRemoved += (s, arg) =>
                {
                    splitContainer1.Panel2Collapsed = (splitContainer1.Panel2.Controls.Count == 0 ? true : false);
                    splitContainer1.Visible = (splitContainer1.Panel1.Controls.Count == 0 && splitContainer1.Panel2.Controls.Count == 0 ? false : true);
                    splitter1.Visible = (splitContainer1.Panel1.Controls.Count == 0 && splitContainer1.Panel2.Controls.Count == 0 ? false : true);
                };
                splitContainer2.Panel1.ControlRemoved += (s, arg) =>
                {
                    splitContainer2.Panel1Collapsed = (splitContainer2.Panel1.Controls.Count == 0 ? true : false);
                    splitContainer2.Visible = (splitContainer2.Panel1.Controls.Count == 0 && splitContainer2.Panel2.Controls.Count == 0 ? false : true);
                    splitter2.Visible = (splitContainer2.Panel1.Controls.Count == 0 && splitContainer2.Panel2.Controls.Count == 0 ? false : true);
                };
                splitContainer2.Panel2.ControlRemoved += (s, arg) =>
                {
                    splitContainer2.Panel2Collapsed = (splitContainer2.Panel2.Controls.Count == 0 ? true : false);
                    splitContainer2.Visible = (splitContainer2.Panel1.Controls.Count == 0 && splitContainer2.Panel2.Controls.Count == 0 ? false : true);
                    splitter2.Visible = (splitContainer2.Panel1.Controls.Count == 0 && splitContainer2.Panel2.Controls.Count == 0 ? false : true);
                };

                FormClosed += (s, args) => dataBase.ToUpdate(userLogin, "Вышел из системы");

                foreach (DataRow st in dataBase.SimpleData("FrequencyBand").Rows)
                    comboBox2.Items.Add(st["Наименование диапазона"]);

                comboBox1.Items.Add("L");
                comboBox1.Items.Add("R");
                comboBox1.Items.Add("V");
                comboBox1.Items.Add("H");

                bool indexChange = false;
                comboBox1.SelectedValueChanged += (s, e) =>
                {
                    if (tabControl1.SelectedTab.GetType() == typeof(Loading) && !indexChange)
                        if (tabControl1.TabPages.IndexOfKey("WHERE Спутник = \'" + (tabControl1.SelectedTab as Loading).nameISZ + "\' AND Диапазон = \'" + comboBox2.Text + "\' AND Поляризация = \'" + comboBox1.Text + "\'") == -1)
                            (tabControl1.SelectedTab as Loading).BandAndPolarizationNeedChange(comboBox2.Text, comboBox1.Text);
                        else
                            tabControl1.SelectedTab = tabControl1.TabPages["WHERE Спутник = \'" + (tabControl1.SelectedTab as Loading).nameISZ + "\' AND Диапазон = \'" + comboBox2.Text + "\' AND Поляризация = \'" + comboBox1.Text + "\'"];
                };
                comboBox2.SelectedValueChanged += (s, e) =>
                {
                    if (tabControl1.SelectedTab.GetType() == typeof(Loading) && !indexChange)
                        if (tabControl1.TabPages.IndexOfKey("WHERE Спутник = \'" + (tabControl1.SelectedTab as Loading).nameISZ + "\' AND Диапазон = \'" + comboBox2.Text + "\' AND Поляризация = \'" + comboBox1.Text + "\'") == -1)
                            (tabControl1.SelectedTab as Loading).BandAndPolarizationNeedChange(comboBox2.Text, comboBox1.Text);
                        else
                            tabControl1.SelectedTab = tabControl1.TabPages["WHERE Спутник = \'" + (tabControl1.SelectedTab as Loading).nameISZ + "\' AND Диапазон = \'" + comboBox2.Text + "\' AND Поляризация = \'" + comboBox1.Text + "\'"];

                };

                tabControl1.SelectedIndexChanged += (s, e) =>
                {
                    if (tabControl1.TabPages.Count == 0)
                        return;

                    if (tabControl1.SelectedTab.GetType() == typeof(SatelliteList))
                    {
                        comboBox1.Visible = false;
                        comboBox2.Visible = false;
                    }
                    else if (tabControl1.SelectedTab.GetType() == typeof(Loading))
                    {
                        indexChange = true;
                        comboBox1.Visible = true;
                        comboBox2.Visible = true;
                        comboBox1.Text = (tabControl1.SelectedTab as Loading).polarizationISZ;
                        comboBox2.Text = (tabControl1.SelectedTab as Loading).bandISZ;
                        indexChange = false;
                    }
                    else
                    {
                        DownSign = null;
                    }
                };
                tabControl1.OnPageClose += (s, e) =>
                {
                    if (tabControl1.TabPages[e.ClosingPageIndex].GetType() == typeof(Loading) && dataBase.dataset.Tables.Contains("Loading " + (tabControl1.TabPages[e.ClosingPageIndex] as Loading).keys))
                        dataBase.dataset.Tables["Loading " + (tabControl1.TabPages[e.ClosingPageIndex] as Loading).keys].Clear();
                };

                калькуляторToolStripMenuItem.Click += (s, e) => Process.Start(@"C:\WINDOWS\system32\calc.exe");
                графикToolStripMenuItem.Click += (s, e) =>
                {
                    tabControl1.TabPages.Add(new Charts("Chart"));

                    tabControl1.SelectedIndex = tabControl1.TabCount - 1;
                };
                headLabel.TextAlign = ContentAlignment.MiddleCenter;
                headLabel.Dock = DockStyle.Fill;

                if (DateTime.Today.Month == 1 && DateTime.Today.Day < 7 && userDepartment == 2)
                {
                    headLabel.Text = Supports.DeCezarus(dataBase.ToCount("SELECT Content FROM [MainSettings] WHERE [Who] = 'NYCongratulation'").ToString());
                }                             

                if (DateTime.Today.Month == 5 && DateTime.Today.Day == 9 && userDepartment == 2)
                {
                    headLabel.Text = Supports.DeCezarus(dataBase.ToCount("SELECT Content FROM [MainSettings] WHERE [Who] = '9Congratulation'").ToString());
                }                             

                if (DateTime.Today.Month == 11 && DateTime.Today.Day == 5 && userDepartment == 2)
                {
                    headLabel.Text = Supports.DeCezarus(dataBase.ToCount("SELECT Content FROM [MainSettings] WHERE [Who] = 'ExpCongratulation'").ToString());
                }                    
            }
            catch (Exception e)
            {
                MessageBoxTi.Show("Profile.prepare " + e.Message);
            }
        }

        private void ChangePadd()
        {
            if (WindowState != FormWindowState.Maximized)
            {
                RESIZE_HANDLE_SIZE = 2;
                pictureBox2.Image = x4;
                Padding = new Padding(1);
            }
            else
            {
                RESIZE_HANDLE_SIZE = 0;
                Padding = new Padding(0);
                pictureBox2.Image = x2;
            }
        }

        public Profile(string login, Image face = null)
        {
            try
            {
                string name = dataBase.ToCount("SELECT [Пользователь] FROM [Login] WHERE [Login].[Login] = '" + login + "'").ToString();
                userDepartment = Convert.ToInt32(dataBase.ToCount("SELECT [Department] FROM [Login] WHERE [Login].[Login] = '" + login + "'"));
                InitializeComponent();
                Name = "Department2Base";
                Text = name;
                userName = name;
                userLogin = login;
                pictureBox4.Image = face;
                prepare();
                Supports.GangeGroup(this);

                dataBase.ToUpdate(login, "Вошел в систему");

                new ToolTip().SetToolTip(pictureBox4, userName);

                Supports.GangeGroup(this);
                panel2.BackColor = Supports.headBlue;

                tabControl1.TabPages.Add(new SatelliteList());
                tabControl1.TabPages["tabPageSatelliteList"].Select();

                foreach (string appointment in dataBase.SimpleData("[Appointment] WHERE Login = '" + login + "'").Rows.Cast<DataRow>().Select(x => x["Occupation"].ToString()))
                {
                    switch (appointment)
                    {
                        case "ССА":
                            tabControl1.TabPages.Add(new SSALoading());
                            break;
                        case "Администратор":
                            tabControl1.TabPages.Add(new Admin());
                            break;
                        case "Графики":
                            графикToolStripMenuItem.Enabled = true;
                            break;
                        case "НС":
                            tabControl1.TabPages.Add(new NS2());
                            break;
                        case "Документация":
                            splitContainer1.Panel1.Controls.Add(new SidePanel("Документация") { Name = "Report" });
                            splitContainer1.Panel1.Controls.Find("Report", false).FirstOrDefault().Controls.Find("Body", false).FirstOrDefault().Controls.Add(new RaportsForOperators());
                            break;
                    }
                }

                splitContainer1.Panel2.Controls.Add(new SidePanel("Документация") { Name = "Chat" });
                splitContainer1.Panel2.Controls.Find("Chat", false).FirstOrDefault().Controls.Find("Body", false).FirstOrDefault().Controls.Add(new NoteForOperators());


                foreach (TabPage tp in tabControl1.TabPages)
                {
                    окнаToolStripMenuItem.DropDownItems.Add(new ToolStripMenuItem { Name = tp.Name, Text = tp.Text });
                }
                foreach (ToolStripItem tp in окнаToolStripMenuItem.DropDownItems)
                {
                    tp.Click += (s, e) =>
                    {
                        if (tabControl1.TabPages.IndexOfKey(tp.Name) != -1)
                            tabControl1.SelectTab(tp.Name);
                        else
                        {
                            switch (tp.Name)
                            {
                                case "tabPageSatelliteList":
                                    tabControl1.TabPages.Add(new SatelliteList());
                                    tabControl1.TabPages["tabPageSatelliteList"].Select();
                                    break;

                                case "tabPageNS2":
                                    tabControl1.TabPages.Add(new NS2());
                                    tabControl1.TabPages["tabPageNS2"].Select();

                                    break;

                                case "tabPageSSALoading":
                                    tabControl1.TabPages.Add(new SSALoading());
                                    tabControl1.TabPages["tabPageSSALoading"].Select();
                                    break;
                            }
                        }

                    };

                }

                пенелиToolStripMenuItem.DropDownItems.Add(new ToolStripMenuItem { Name = "Report", Text = "Отчёты" });
                пенелиToolStripMenuItem.DropDownItems.Add(new ToolStripMenuItem { Name = "Chat", Text = "Записи пользователя" });

                foreach (ToolStripItem tp in пенелиToolStripMenuItem.DropDownItems)
                {
                    tp.Click += (s, e) =>
                    {
                        switch (tp.Name)
                        {
                            case "Report":
                                if(splitContainer1.Panel1.Controls.Count == 0)
                                {
                                    if (!splitContainer1.Panel1.Controls.IndexOfKey("Report").Equals(-1) || !splitContainer1.Panel2.Controls.IndexOfKey("Report").Equals(-1) || !splitContainer2.Panel1.Controls.IndexOfKey("Report").Equals(-1) || !splitContainer2.Panel2.Controls.IndexOfKey("Report").Equals(-1))
                                        return;
                                    splitContainer1.Panel1.Controls.Add(new SidePanel("Документация") { Name = "Report" });
                                    splitContainer1.Panel1.Controls.Find("Report", false).FirstOrDefault().Controls.Find("Body", false).FirstOrDefault().Controls.Add(new RaportsForOperators());
                                }                   
                                else if (splitContainer1.Panel2.Controls.Count == 0)
                                {
                                    if (!splitContainer1.Panel1.Controls.IndexOfKey("Report").Equals(-1) || !splitContainer1.Panel2.Controls.IndexOfKey("Report").Equals(-1) || !splitContainer2.Panel1.Controls.IndexOfKey("Report").Equals(-1) || !splitContainer2.Panel2.Controls.IndexOfKey("Report").Equals(-1))
                                        return;
                                    splitContainer1.Panel2.Controls.Add(new SidePanel("Документация") { Name = "Report" });
                                    splitContainer1.Panel2.Controls.Find("Report", false).FirstOrDefault().Controls.Find("Body", false).FirstOrDefault().Controls.Add(new RaportsForOperators());
                                }
                                else if (splitContainer2.Panel1.Controls.Count == 0)
                                {
                                    if (!splitContainer1.Panel1.Controls.IndexOfKey("Report").Equals(-1) || !splitContainer1.Panel2.Controls.IndexOfKey("Report").Equals(-1) || !splitContainer2.Panel1.Controls.IndexOfKey("Report").Equals(-1) || !splitContainer2.Panel2.Controls.IndexOfKey("Report").Equals(-1))
                                        return;
                                    splitContainer2.Panel1.Controls.Add(new SidePanel("Документация") { Name = "Report" });
                                    splitContainer2.Panel1.Controls.Find("Report", false).FirstOrDefault().Controls.Find("Body", false).FirstOrDefault().Controls.Add(new RaportsForOperators());
                                }
                                else if (splitContainer2.Panel2.Controls.Count == 0)
                                {
                                    if (!splitContainer1.Panel1.Controls.IndexOfKey("Report").Equals(-1) || !splitContainer1.Panel2.Controls.IndexOfKey("Report").Equals(-1) || !splitContainer2.Panel1.Controls.IndexOfKey("Report").Equals(-1) || !splitContainer2.Panel2.Controls.IndexOfKey("Report").Equals(-1))
                                        return;
                                    splitContainer2.Panel2.Controls.Add(new SidePanel("Документация") { Name = "Report" });
                                    splitContainer2.Panel2.Controls.Find("Report", false).FirstOrDefault().Controls.Find("Body", false).FirstOrDefault().Controls.Add(new RaportsForOperators());
                                }
                                break;

                            case "Chat":

                                if (splitContainer1.Panel1.Controls.Count == 0)
                                {
                                    if (!splitContainer1.Panel1.Controls.IndexOfKey("Chat").Equals(-1) || !splitContainer1.Panel2.Controls.IndexOfKey("Chat").Equals(-1) || !splitContainer2.Panel1.Controls.IndexOfKey("Chat").Equals(-1) || !splitContainer2.Panel2.Controls.IndexOfKey("Chat").Equals(-1))
                                        return;
                                    splitContainer1.Panel1.Controls.Add(new SidePanel("Документация") { Name = "Chat" });
                                    splitContainer1.Panel1.Controls.Find("Chat", false).FirstOrDefault().Controls.Find("Body", false).FirstOrDefault().Controls.Add(new NoteForOperators());
                                }
                                else if (splitContainer1.Panel2.Controls.Count == 0)
                                {
                                    if (!splitContainer1.Panel1.Controls.IndexOfKey("Chat").Equals(-1) || !splitContainer1.Panel2.Controls.IndexOfKey("Chat").Equals(-1) || !splitContainer2.Panel1.Controls.IndexOfKey("Chat").Equals(-1) || !splitContainer2.Panel2.Controls.IndexOfKey("Chat").Equals(-1))
                                        return;
                                    splitContainer1.Panel2.Controls.Add(new SidePanel("Документация") { Name = "Chat" });
                                    splitContainer1.Panel2.Controls.Find("Chat", false).FirstOrDefault().Controls.Find("Body", false).FirstOrDefault().Controls.Add(new NoteForOperators());
                                }
                                else if (splitContainer2.Panel1.Controls.Count == 0)
                                {
                                    if (!splitContainer1.Panel1.Controls.IndexOfKey("Chat").Equals(-1) || !splitContainer1.Panel2.Controls.IndexOfKey("Chat").Equals(-1) || !splitContainer2.Panel1.Controls.IndexOfKey("Chat").Equals(-1) || !splitContainer2.Panel2.Controls.IndexOfKey("Chat").Equals(-1))
                                        return;
                                    splitContainer2.Panel1.Controls.Add(new SidePanel("Документация") { Name = "Chat" });
                                    splitContainer2.Panel1.Controls.Find("Chat", false).FirstOrDefault().Controls.Find("Body", false).FirstOrDefault().Controls.Add(new NoteForOperators());
                                }
                                else if (splitContainer2.Panel2.Controls.Count == 0)
                                {
                                    if (!splitContainer1.Panel1.Controls.IndexOfKey("Chat").Equals(-1) || !splitContainer1.Panel2.Controls.IndexOfKey("Chat").Equals(-1) || !splitContainer2.Panel1.Controls.IndexOfKey("Chat").Equals(-1) || !splitContainer2.Panel2.Controls.IndexOfKey("Chat").Equals(-1))
                                        return;
                                    splitContainer2.Panel2.Controls.Add(new SidePanel("Документация") { Name = "Chat" });
                                    splitContainer2.Panel2.Controls.Find("Chat", false).FirstOrDefault().Controls.Find("Body", false).FirstOrDefault().Controls.Add(new NoteForOperators());
                                }
                                break;
                        }
                    };

                }

                Supports.WorkActivity.Start();
            }
            catch (Exception e)
            {
                MessageBoxTi.Show("Profile " + e.Message);
            }
        }
    }
}