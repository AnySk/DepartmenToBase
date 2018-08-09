using Department2Base.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Department2Base
{
    class Admin : TabPage
    {
        Supports sup = new Supports();
        FlowLayoutPanel foreignPeople = new FlowLayoutPanel() { Dock = DockStyle.Fill, BorderStyle = BorderStyle.None, AutoScroll = true };

        public Admin()
        {
            try
            {
                BorderStyle = BorderStyle.None;
                Name = "tabPageAdmin";
                Text = "Админ";
                BackColor = Supports.headGrey;
                ForeColor = Supports.headGrey;

                Controls.Add(new FlowLayoutPanel()
                {
                    BorderStyle = BorderStyle.None,
                    Name = "FlowLayoutPanelAdmin",
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                });

                Controls.Add(new Panel()
                {
                    Name = "foreignPanelAdmin",
                    Dock = DockStyle.Right,
                    Width = 440,
                });

                (Controls.Find("foreignPanelAdmin", true).FirstOrDefault() as Panel).Controls.Add(foreignPeople);

                (Controls.Find("foreignPanelAdmin", true).FirstOrDefault() as Panel).Controls.Add(new Label()
                {
                    Font = new Font("Times New Roman", 12, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Top,
                    Text = "Грингос",

                });

                Controls.Add(new FlowLayoutPanel()
                {
                    BorderStyle = BorderStyle.None,
                    Name = "FlowLayoutPanelData",
                    Dock = DockStyle.Top,
                    Height = 30,
                    Padding = new Padding(3),
                    BackColor = Supports.backBlack,
                });

                bool adminFirst = false;

                Enter += (se, e) =>
                {
                    try
                    {
                        dataBase.ToDisplay("SELECT [ID], [Department], [Login], [Password], [Face], [Пользователь], [Rank], [Allowed], [SatellitesType] FROM Login", requestJustByMyself: true, dataTableName: "Login", onlyAdapter: true);

                        if (adminFirst == true) return;

                        foreach (DataRow dr in dataBase.dataset.Tables["Login"].Rows.Cast<DataRow>().Where(x => Convert.ToInt32(x["Department"]) == 2))
                        {
                            (Controls.Find("FlowLayoutPanelAdmin", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(UserCard(dr["Login"].ToString()));
                        }


                        (Controls.Find("FlowLayoutPanelAdmin", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new Panel()
                        {
                            Name = "AddPanel",
                            Width = 200,
                            Height = 410,
                            BackColor = Supports.backBlack,
                            Padding = new Padding(3),
                        });


                        (Controls.Find("FlowLayoutPanelData", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new Label()
                        {
                            Name = "DataLabel",
                            AutoSize = true,
                            Text = "Текущая версия программы:",
                            Padding = new Padding(0, 9, 0, 0),
                        });

                        (Controls.Find("FlowLayoutPanelData", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new Label()
                        {
                            Name = "DataLabel1",
                            Text = (string)dataBase.ToCount("SELECT TOP 1 [Version] FROM [dbo].[ProgVer] ORDER BY [dbo].[ProgVer].[Version] DESC"),
                            AutoSize = true,
                            Padding = new Padding(0, 9, 0, 0),
                        });



                        (Controls.Find("FlowLayoutPanelData", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new Button()
                        {
                            Name = "DataButton",
                            Text = "Добавить новую версию",
                            Width = 160,
                            FlatStyle = FlatStyle.Flat,
                        });

                        (Controls.Find("DataButton", true).FirstOrDefault() as Button).Click += (s, arg) =>
                        {
                            OpenFileDialog openFileDialog1 = new OpenFileDialog();
                            openFileDialog1.Filter = "EXECUTE files|*.exe;";
                            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                            {
                                dataBase.ToUpdate(openFileDialog1.OpenFile(), Application.ProductVersion);
                            }

                            (Controls.Find("DataLabel1", true).FirstOrDefault() as Label).Text = (string)dataBase.ToCount("SELECT TOP 1 [Version] FROM [dbo].[ProgVer] ORDER BY [dbo].[ProgVer].[Version] DESC");
                        };

                        (Controls.Find("AddPanel", true).FirstOrDefault() as Panel).Controls.Add(new Button()
                        {
                            Name = "AddPanelButton",
                            Image = Resources.plus,
                            Dock = DockStyle.Fill,
                            FlatStyle = FlatStyle.Flat,
                            ForeColor = Supports.textBlack,
                            BackColor = Supports.headGrey
                        });

                        (Controls.Find("AddPanelButton", true).FirstOrDefault() as Button).Click += (s, arg) =>
                        {
                            string log = MessageBoxTi.Show("Логин", "Придумайте логин", HorizontalAlignment.Left);
                            while (log.Length > 15)
                            {
                                log = MessageBoxTi.Show("Логин", "Пользователь с таким ологином уже существует. Придумайте другой логин!", HorizontalAlignment.Left);
                                if (log == null || log == "")
                                    return;
                            }

                            if (dataBase.dataset.Tables["Login"].Select().Where(x => x["Login"].ToString().Equals(log)).Count() == 0 && log != "")
                            {
                                (Controls.Find("FlowLayoutPanelAdmin", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(UserCard(log, true));
                                (Controls.Find("FlowLayoutPanelAdmin", true).FirstOrDefault() as FlowLayoutPanel).Controls.SetChildIndex((Controls.Find("FlowLayoutPanelAdmin", true).FirstOrDefault() as FlowLayoutPanel).Controls.Find("AddPanel", false).FirstOrDefault(), (Controls.Find("FlowLayoutPanelAdmin", true).FirstOrDefault() as FlowLayoutPanel).Controls.Count);
                                return;
                            }
                            else if (log == null || log == "")
                            {
                                return;
                            }
                            else
                            {
                                MessageBoxTi.Show("Такой логин уже существует");
                                return;
                            }
                        };
                        Controls.Find("AddPanelButton", true).FirstOrDefault().Focus();
                        adminFirst = true;



                        foreach (DataRow dr in dataBase.dataset.Tables["Login"].Rows.Cast<DataRow>().Where(x => Convert.ToInt32(x["Department"]) != 2))
                        {
                            foreignPeople.Controls.Add(UserCard(dr["Login"].ToString()));
                        }

                        foreignPeople.Controls.Add(new Panel()
                        {
                            Name = "AddPanel",
                            Width = 200,
                            Height = 410,
                            BackColor = Supports.backBlack,
                            Padding = new Padding(3),
                        });

                        (foreignPeople.Controls.Find("AddPanel", true).FirstOrDefault() as Panel).Controls.Add(new Button()
                        {
                            Name = "AddPanelButton",
                            Image = Resources.plus,
                            Dock = DockStyle.Fill,
                            FlatStyle = FlatStyle.Flat,
                            ForeColor = Supports.textBlack,
                            BackColor = Supports.headGrey
                        });

                        (foreignPeople.Controls.Find("AddPanelButton", true).FirstOrDefault() as Button).Click += (s, arg) =>
                        {
                            string log = MessageBoxTi.Show("Логин", "Придумайте логин", HorizontalAlignment.Left);
                            while (log.Length > 15)
                            {
                                log = MessageBoxTi.Show("Логин", "Пользователь с таким ологином уже существует. Придумайте другой логин!", HorizontalAlignment.Left);
                                if (log == null || log == "")
                                    return;
                            }

                            if (dataBase.dataset.Tables["Login"].Select().Where(x => x["Login"].ToString().Equals(log)).Count() == 0 && log != "")
                            {
                                foreignPeople.Controls.Add(UserCard(log, true));
                                foreignPeople.Controls.SetChildIndex(foreignPeople.Controls.Find("AddPanel", false).FirstOrDefault(), foreignPeople.Controls.Count);
                                return;
                            }
                            else if (log == null || log == "")
                            {
                                return;
                            }
                            else
                            {
                                MessageBoxTi.Show("Такой логин уже существует");
                                return;
                            }
                        };


                        Supports.GangeGroup(this);

                    }
                    catch (Exception ex)
                    {
                        MessageBoxTi.Show("Admin.Enter " + ex.Message);
                    }
                };

                dataBase.ToDisplay("SELECT [ID], [Department], [Login], [Password], [Face], [Пользователь], [Rank], [Allowed], [SatellitesType] FROM Login", requestJustByMyself: true, dataTableName: "Login");
                dataBase.ToDisplay("Appointment");
            }
            catch (Exception e)
            {
                MessageBoxTi.Show("Admin " + e.Message);
            }
        }

        public Panel UserCard(string Login, bool add = false)
        {
            try
            {
                Bitmap im = Resources.person;
                string satellitesType = null;
                string password = null;
                string name = null;
                string department = null;
                int rank = -1;
                DataTable occupation = null;

                if (!add)
                {
                    DataTable person = dataBase.SimpleData("[Login] WHERE [Login].[Login] = '" + Login + "'");
                    occupation = dataBase.SimpleData("[Appointment] WHERE [Appointment].[Login] = '" + Login + "'");
                    satellitesType = person.Rows[0]["SatellitesType"].ToString();
                    password = person.Rows[0]["Password"].ToString();
                    name = person.Rows[0]["Пользователь"].ToString();
                    department = person.Rows[0]["Department"].ToString();
                    im = (Bitmap)Image.FromStream(new MemoryStream((byte[])person.Rows[0]["Face"]));
                    rank = Convert.ToInt32(person.Rows[0]["Rank"]);
                }

                Panel myCard = new Panel()
                {
                    Name = Login,
                    Width = 200,
                    Height = 500,
                    BackColor = Supports.backBlack,
                    Padding = new Padding(3),
                };

                myCard.Controls.Add(new FlowLayoutPanel()
                {
                    Dock = DockStyle.Fill,

                    BorderStyle = BorderStyle.None,
                    Name = "FlowLayoutPanel",
                    Width = 200,
                    Height = 300,
                    BackColor = Supports.headGrey,
                });                 

                (myCard.Controls.Find("FlowLayoutPanel", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new PictureBox()
                {
                    Name = "Picture",
                    Image = im,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Width = 200,
                    Height = 100,
                });

                (myCard.Controls.Find("Picture", true).FirstOrDefault() as PictureBox).Click += (s, args) =>
                {
                    OpenFileDialog openFileDialog1 = new OpenFileDialog();
                    openFileDialog1.Filter = "Image files|*.jpeg;*.jpg;*.png;*.gif;*.bmp;*.tiff;*.tif;*.jfif;*.jpe;*.dib;";
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        (myCard.Controls.Find("Picture", true).FirstOrDefault() as PictureBox).Image = Image.FromStream(openFileDialog1.OpenFile());
                    }
                };

                (myCard.Controls.Find("FlowLayoutPanel", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
                {
                    Name = "GroupBoxAppointment",
                    Text = "Тип спутника",
                    Width = 188,
                    Height = 40,
                });

                (myCard.Controls.Find("GroupBoxAppointment", true).FirstOrDefault() as GroupBox).Controls.Add(new ComboBox()
                {
                    Name = "SatellitesTypeComboBox",
                    Dock = DockStyle.Fill,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                });

                (myCard.Controls.Find("FlowLayoutPanel", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new Panel()
                {
                    Name = "PanelRankAndDep",
                    Width = 188,
                    Height = 40,
                });

                (myCard.Controls.Find("PanelRankAndDep", true).FirstOrDefault() as Panel).Controls.Add(new GroupBox()
                {
                    Name = "GroupBoxDepartment",
                    Text = "Отдел",
                    Width = 60,
                    Dock = DockStyle.Left
                });

                (myCard.Controls.Find("PanelRankAndDep", true).FirstOrDefault() as Panel).Controls.Add(new GroupBox()
                {
                    Name = "GroupBoxRank",
                    Text = "Звание",
                    Width = 126,
                    Dock = DockStyle.Right
                });

                (myCard.Controls.Find("GroupBoxDepartment", true).FirstOrDefault() as GroupBox).Controls.Add(new ComboBox()
                {
                    Name = "DepartmentComboBox",
                    Dock = DockStyle.Fill,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                });

                (myCard.Controls.Find("GroupBoxRank", true).FirstOrDefault() as GroupBox).Controls.Add(new ComboBox()
                {
                    Name = "RankComboBox",
                    Dock = DockStyle.Fill,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                });

                for (var i = 1; i < 9; i++)
                    (myCard.Controls.Find("DepartmentComboBox", true).FirstOrDefault() as ComboBox).Items.Add(i.ToString());

                foreach(var ranks in Enum.GetValues(typeof(Supports.Ranks)))
                    (myCard.Controls.Find("RankComboBox", true).FirstOrDefault() as ComboBox).Items.Add(ranks.ToString());

                if (rank != -1)
                    (myCard.Controls.Find("RankComboBox", true).FirstOrDefault() as ComboBox).SelectedItem = ((Supports.Ranks)rank).ToString();

                if (department != null)
                    (myCard.Controls.Find("DepartmentComboBox", true).FirstOrDefault() as ComboBox).SelectedItem = department;
                else
                    (myCard.Controls.Find("DepartmentComboBox", true).FirstOrDefault() as ComboBox).SelectedItem = 2;

                (myCard.Controls.Find("SatellitesTypeComboBox", true).FirstOrDefault() as ComboBox).Items.Add("MIL");
                (myCard.Controls.Find("SatellitesTypeComboBox", true).FirstOrDefault() as ComboBox).Items.Add("CIV");
                (myCard.Controls.Find("SatellitesTypeComboBox", true).FirstOrDefault() as ComboBox).Items.Add("TWO");
                 
                if (satellitesType != null)
                    (myCard.Controls.Find("SatellitesTypeComboBox", true).FirstOrDefault() as ComboBox).SelectedItem = satellitesType;
                else
                    (myCard.Controls.Find("SatellitesTypeComboBox", true).FirstOrDefault() as ComboBox).SelectedIndex = 1;

                (myCard.Controls.Find("FlowLayoutPanel", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
                {
                    Name = "GroupBoxName",
                    Text = "Пользователь",
                    Width = 188,
                    Height = 40,
                });

                (myCard.Controls.Find("GroupBoxName", true).FirstOrDefault() as GroupBox).Controls.Add(new TextBox()
                {
                    Name = "Name",
                    Text = name ?? "",
                    Dock = DockStyle.Fill,
                    MaxLength = 50,
                });

                (myCard.Controls.Find("FlowLayoutPanel", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
                {
                    Name = "GroupBoxLogin",
                    Text = "Логин",
                    Width = 188,
                    Height = 40,
                });

                (myCard.Controls.Find("GroupBoxLogin", true).FirstOrDefault() as GroupBox).Controls.Add(new TextBox()
                {
                    Name = "Login",
                    Text = Login ?? "",
                    Dock = DockStyle.Fill,
                    Enabled = false,
                    MaxLength = 15
                });

                (myCard.Controls.Find("FlowLayoutPanel", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
                {
                    Name = "GroupBoxPassword",
                    Text = "Пароль",
                    Width = 188,
                    Height = 40,
                });

                (myCard.Controls.Find("GroupBoxPassword", true).FirstOrDefault() as GroupBox).Controls.Add(new TextBox()
                {
                    Name = "Password",
                    Text = password ?? "",
                    Dock = DockStyle.Fill,
                    PasswordChar = '*',
                    MaxLength = 15,
                });

                (myCard.Controls.Find("Password", true).FirstOrDefault() as TextBox).Click += (s, a) =>
                {
                    (myCard.Controls.Find("Password", true).FirstOrDefault() as TextBox).PasswordChar = '\0';
                };

                (myCard.Controls.Find("Password", true).FirstOrDefault() as TextBox).LostFocus += (s, a) =>
                {
                    (myCard.Controls.Find("Password", true).FirstOrDefault() as TextBox).PasswordChar = '*';
                };

                (myCard.Controls.Find("FlowLayoutPanel", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
                {
                    Name = "GroupBoxOccupation",
                    Text = "Права",
                    Width = 188,
                    Height = 120,
                });

                (myCard.Controls.Find("GroupBoxOccupation", true).FirstOrDefault() as GroupBox).Controls.Add(new CheckedListBox()
                {
                    Name = "Occupation",
                    Dock = DockStyle.Fill,
                    BorderStyle = BorderStyle.None,
                    BackColor = Supports.headGrey,
                    ForeColor = Supports.textWhite,

                });

                CheckedListBox OccupationCheckedListBox = (myCard.Controls.Find("Occupation", true).FirstOrDefault() as CheckedListBox);

                OccupationCheckedListBox.MouseLeave += (sen, arg) =>
                {
                    (sen as CheckedListBox).ClearSelected();
                };

                (myCard.Controls.Find("FlowLayoutPanel", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new Panel()
                {
                    Name = "Panel1",
                    Width = 188,
                    Height = 20
                });

                (myCard.Controls.Find("Panel1", true).FirstOrDefault() as Panel).Controls.Add(new Button()
                {
                    Name = "editButton",
                    Image = add == false ? Resources.edit : Resources.add,
                    Dock = DockStyle.Left,
                    Width = 90,
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Supports.textBlack,
                });

                (myCard.Controls.Find("Panel1", true).FirstOrDefault() as Panel).Controls.Add(new Button()
                {
                    Name = "eraseButton",
                    Image = Resources.erase,
                    Dock = DockStyle.Right,
                    Width = 90,
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Supports.textBlack,
                });

                new ToolTip().SetToolTip((myCard.Controls.Find("editButton", true).FirstOrDefault() as Button), "Внести изменения");
                new ToolTip().SetToolTip((myCard.Controls.Find("eraseButton", true).FirstOrDefault() as Button), "Удалить пользователя");

                (myCard.Controls.Find("editButton", true).FirstOrDefault() as Button).Click += (sen, arg) =>
                {
                    if (MessageBoxTi.Show("Вы действительно хотите изменить данные пользователя?", "Редактирование") == MessageResult.Yes)
                    {
                        if (dataBase.dataset.Tables["Login"].Select().Where(x => x["Login"].ToString().Equals(Login)).Count() != 0)
                        {
                            List<object> delID = new List<object>();
                            dataBase.dataset.Tables["Login"].Select().Where(x => x["Login"].ToString().Equals(Login)).FirstOrDefault()["Password"] = ((Controls.Find(Login, true).FirstOrDefault() as Panel).Controls.Find("Password", true).FirstOrDefault() as TextBox).Text;
                            dataBase.dataset.Tables["Login"].Select().Where(x => x["Login"].ToString().Equals(Login)).FirstOrDefault()["Face"] = sup.ImToBy(((Controls.Find(Login, true).FirstOrDefault() as Panel).Controls.Find("Picture", true).FirstOrDefault() as PictureBox).Image);
                            dataBase.dataset.Tables["Login"].Select().Where(x => x["Login"].ToString().Equals(Login)).FirstOrDefault()["Пользователь"] = ((Controls.Find(Login, true).FirstOrDefault() as Panel).Controls.Find("Name", true).FirstOrDefault() as TextBox).Text;
                            dataBase.dataset.Tables["Login"].Select().Where(x => x["Login"].ToString().Equals(Login)).FirstOrDefault()["Login"] = ((Controls.Find(Login, true).FirstOrDefault() as Panel).Controls.Find("Login", true).FirstOrDefault() as TextBox).Text;
                            dataBase.dataset.Tables["Login"].Select().Where(x => x["Login"].ToString().Equals(Login)).FirstOrDefault()["SatellitesType"] = (myCard.Controls.Find("SatellitesTypeComboBox", true).FirstOrDefault() as ComboBox).SelectedItem;
                            dataBase.dataset.Tables["Login"].Select().Where(x => x["Login"].ToString().Equals(Login)).FirstOrDefault()["Department"] = (myCard.Controls.Find("DepartmentComboBox", true).FirstOrDefault() as ComboBox).SelectedItem;
                            dataBase.dataset.Tables["Login"].Select().Where(x => x["Login"].ToString().Equals(Login)).FirstOrDefault()["Allowed"] = 1;
                            dataBase.dataset.Tables["Login"].Select().Where(x => x["Login"].ToString().Equals(Login)).FirstOrDefault()["Rank"] = (int)Enum.Parse(typeof(Supports.Ranks), (myCard.Controls.Find("RankComboBox", true).FirstOrDefault() as ComboBox).SelectedItem.ToString());


                            for (int j = 0; j < OccupationCheckedListBox.Items.Count; j++)
                            {
                                if (OccupationCheckedListBox.GetItemChecked(j) && dataBase.dataset.Tables["Appointment"].Rows.Cast<DataRow>().Where(x => x["Login"].ToString().Equals(Login)).Where(x => x["Occupation"].ToString().Equals(OccupationCheckedListBox.Items[j].ToString())).Count() == 0)
                                {
                                    DataRow dr = dataBase.dataset.Tables["Appointment"].NewRow();
                                    dr["Login"] = Login;
                                    dr["Occupation"] = OccupationCheckedListBox.Items[j].ToString();
                                    dataBase.dataset.Tables["Appointment"].Rows.Add(dr);
                                }

                                if (!OccupationCheckedListBox.GetItemChecked(j) && dataBase.dataset.Tables["Appointment"].Rows.Cast<DataRow>().Where(x => x["Login"].ToString().Equals(Login)).Where(x => x["Occupation"].ToString().Equals(OccupationCheckedListBox.Items[j].ToString())).Count() != 0)
                                {
                                    delID.Add(dataBase.dataset.Tables["Appointment"].Rows.Cast<DataRow>().Where(x => x["Login"].ToString().Equals(Login)).Where(x => x["Occupation"].ToString().Equals(OccupationCheckedListBox.Items[j].ToString())).FirstOrDefault()[0]);
                                }
                            }

                            if (delID.Count != 0)
                                dataBase.ToDisplay("Appointment", onlyAdapter: true);

                            foreach (var del in delID)
                            {
                                string k = dataBase.dataset.Tables["Appointment"].Select().Where(x => x["Occupation"].ToString().Equals("Admin")).FirstOrDefault()[0].ToString();

                                if (dataBase.dataset.Tables["Appointment"].Select().Where(x => x["Occupation"].ToString().Equals("Admin")).Count() == 1 && dataBase.dataset.Tables["Appointment"].Select().Where(x => x["Occupation"].ToString().Equals("Admin")).FirstOrDefault()[0].ToString().Equals(del.ToString()))
                                {
                                    OccupationCheckedListBox.SetItemChecked(3, true);
                                    continue;
                                }
                                dataBase.dataset.Tables["Appointment"].Rows.Cast<DataRow>().Where(x => x["Login"].ToString().Equals(Login)).Where(x => x["ID"].Equals(del)).FirstOrDefault().Delete();
                                dataBase.sqlAdapter.Update(dataBase.dataset.Tables["Appointment"]);
                                dataBase.dataset.Tables["Appointment"].Clear();
                                dataBase.sqlAdapter.Fill(dataBase.dataset.Tables["Appointment"]);
                            }

                            dataBase.ToDisplay("SELECT [ID], [Department], [Login], [Password], [Face], [Пользователь], [Rank], [Allowed], [SatellitesType] FROM Login", requestJustByMyself: true, dataTableName: "Login", onlyAdapter: true);
                            dataBase.sqlAdapter.Update(dataBase.dataset.Tables["Login"]);
                            dataBase.ToDisplay("SELECT [ID], [Department], [Login], [Password], [Face], [Пользователь], [Rank], [Allowed], [SatellitesType] FROM Login", requestJustByMyself: true, dataTableName: "Login");
                            dataBase.ToDisplay("Appointment", onlyAdapter: true);
                            dataBase.sqlAdapter.Update(dataBase.dataset.Tables["Appointment"]);
                            dataBase.ToDisplay("Appointment");

                            return;

                        }
                        else
                        {
                            DataRow dr1 = dataBase.dataset.Tables["Login"].NewRow();
                            dr1["Password"] = ((Controls.Find(Login, true).FirstOrDefault() as Panel).Controls.Find("Password", true).FirstOrDefault() as TextBox).Text;
                            dr1["Face"] = sup.ImToBy(((Controls.Find(Login, true).FirstOrDefault() as Panel).Controls.Find("Picture", true).FirstOrDefault() as PictureBox).Image);
                            dr1["Пользователь"] = ((Controls.Find(Login, true).FirstOrDefault() as Panel).Controls.Find("Name", true).FirstOrDefault() as TextBox).Text;
                            dr1["Login"] = ((Controls.Find(Login, true).FirstOrDefault() as Panel).Controls.Find("Login", true).FirstOrDefault() as TextBox).Text;
                            dr1["SatellitesType"] = (myCard.Controls.Find("SatellitesTypeComboBox", true).FirstOrDefault() as ComboBox).SelectedItem;
                            dr1["Department"] = (myCard.Controls.Find("DepartmentComboBox", true).FirstOrDefault() as ComboBox).SelectedItem;
                            dr1["Allowed"] = 1;
                            dr1["Rank"] = (int)Enum.Parse(typeof(Supports.Ranks), (myCard.Controls.Find("RankComboBox", true).FirstOrDefault() as ComboBox).SelectedItem.ToString());

                            dataBase.dataset.Tables["Login"].Rows.Add(dr1);

                            for (int j = 0; j < OccupationCheckedListBox.Items.Count; j++)
                            {
                                if (OccupationCheckedListBox.GetItemChecked(j) && dataBase.dataset.Tables["Appointment"].Rows.Cast<DataRow>().Where(x => x["Login"].ToString().Equals(Login) && x["Occupation"].ToString().Equals(OccupationCheckedListBox.Items[j].ToString())).Count() == 0)
                                {
                                    DataRow dr = dataBase.dataset.Tables["Appointment"].NewRow();
                                    dr["Login"] = ((Controls.Find(Login, true).FirstOrDefault() as Panel).Controls.Find("Login", true).FirstOrDefault() as TextBox).Text;
                                    dr["Occupation"] = OccupationCheckedListBox.Items[j].ToString();
                                    dataBase.dataset.Tables["Appointment"].Rows.Add(dr);
                                }
                            }

                            dataBase.ToDisplay("SELECT [ID], [Department], [Login], [Password], [Face], [Пользователь], [Rank], [Allowed], [SatellitesType] FROM Login", requestJustByMyself: true, dataTableName: "Login", onlyAdapter: true);
                            dataBase.sqlAdapter.Update(dataBase.dataset.Tables["Login"]);
                            dataBase.ToDisplay("SELECT [ID], [Department], [Login], [Password], [Face], [Пользователь], [Rank], [Allowed], [SatellitesType] FROM Login", requestJustByMyself: true, dataTableName: "Login");
                            dataBase.ToDisplay("Appointment", onlyAdapter: true);
                            dataBase.sqlAdapter.Update(dataBase.dataset.Tables["Appointment"]);
                            dataBase.ToDisplay("Appointment");


                            (myCard.Controls.Find("editButton", true).FirstOrDefault() as Button).Image = Resources.edit;
                            return;
                        }
                    }
                };

                (myCard.Controls.Find("eraseButton", true).FirstOrDefault() as Button).Click += (sen, arg) =>
                {
                    if (dataBase.dataset.Tables["Appointment"].Select().Where(x => x["Occupation"].ToString().Equals("Admin")).Count() == 1 && OccupationCheckedListBox.GetItemChecked(3))
                    {
                        MessageBoxTi.Show("Невозможно удалить последнего администратора");
                        return;
                    }
                    if (dataBase.dataset.Tables["Login"].Select().Where(x => x["Login"].ToString().Equals(Login)).Count() != 0)
                    {
                        dataBase.dataset.Tables["Login"].Select().Where(x => x["Login"].ToString().Equals(Login)).FirstOrDefault().Delete();
                        foreach (DataRow dr in dataBase.dataset.Tables["Appointment"].Select().Where(x => x["Login"].ToString().Equals(Login)))
                        {
                            dr.Delete();
                        }
                        dataBase.ToDisplay("SELECT [ID], [Department], [Login], [Password], [Face], [Пользователь], [Rank], [Allowed], [SatellitesType] FROM Login", requestJustByMyself: true, dataTableName: "Login", onlyAdapter: true);
                        dataBase.sqlAdapter.Update(dataBase.dataset.Tables["Login"]);
                        dataBase.ToDisplay("SELECT [ID], [Department], [Login], [Password], [Face], [Пользователь], [Rank], [Allowed], [SatellitesType] FROM Login", requestJustByMyself: true, dataTableName: "Login");
                        dataBase.ToDisplay("Appointment", onlyAdapter: true);
                        dataBase.sqlAdapter.Update(dataBase.dataset.Tables["Appointment"]);
                        dataBase.ToDisplay("Appointment");
                        Controls.Remove(Controls.Find(Login, true).FirstOrDefault());
                    }
                    else
                    {
                        Controls.Remove(Controls.Find(Login, true).FirstOrDefault());
                    }
                };

                OccupationCheckedListBox.Items.Add("Администратор");
                OccupationCheckedListBox.Items.Add("ССА");
                OccupationCheckedListBox.Items.Add("Графики");
                OccupationCheckedListBox.Items.Add("НС");
                OccupationCheckedListBox.Items.Add("Документация");

                if (occupation != null)
                for (int j = 0; j < OccupationCheckedListBox.Items.Count; j++)
                {
                    if ((occupation.Rows.Cast<DataRow>().Where(x => x["Login"].ToString().Equals(Login)).Where(x => x["Occupation"].ToString().Equals(OccupationCheckedListBox.Items[j].ToString()))).ToList().Count != 0)
                        OccupationCheckedListBox.SetItemChecked(j, true);
                }

                Supports.GangeGroup(myCard);
                return myCard;
            }
            catch (Exception e)
            {
                MessageBoxTi.Show("Admin.UserCard " + e.Message);
                return null;
            }
        }
    }
}