using Department2Base.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Department2Base
{
    class NS2 : TabPage
    {
        DataGridView Users;
        Panel CurrentUserCard;
        int checkSum1 = dataBase.CheckSum("(SELECT [Allowed], [Пользователь], (SELECT top 1 [What] FROM [MainLog] WHERE [Who] = [Login].[Login] AND ([What] = 'Вошел в систему' OR [What] = 'Отошел' OR [What] = 'Вышел из системы' OR [What] = 'Вернулся') ORDER BY [WhenItWas] DESC) AS [What] FROM [dbo].[Login]) AS [table]");

        public NS2()
        {
            BorderStyle = BorderStyle.None;
            Name = "tabPageNS2";
            Text = "Статистика пользования";
            BackColor = Supports.headGrey;
            ForeColor = Supports.headGrey;
            UseVisualStyleBackColor = true;
            bool painted = false;
            AutoScroll = true;


            Timer t = new Timer();
            t.Interval = 3000;
            t.Tick += (s, e) =>
            {
                UpdateUsersCondition();
            };

            Enter += (s, e) =>
            {
                Profile.DownSign = null;
            };

            Paint += (s, e) =>
            {
                if (painted) return;

                Supports.GangeGroup(this);

                (Controls.Find("logDataGridView", true).FirstOrDefault() as DataGridView).Columns["Пользователь"].Width = 220;
                (Controls.Find("logDataGridView", true).FirstOrDefault() as DataGridView).Columns["What"].Width = 720;
                Users.Columns["Пользователь"].Width = 220;
                Users.Columns["What"].Width = 220;
                Users.Columns["Allowed"].Visible = false;
                Users.Columns["Login"].Visible = false;
                Users.Columns["Face"].Visible = false;
                Users.Columns["Rank"].Visible = false;
                Users.Columns["Department"].Visible = false;
                Users.Columns["NSNote"].Visible = false;
                (Controls.Find("CurrentUserCardDataGridView", true).FirstOrDefault() as DataGridView).Columns["What"].Width = 470;
                RefreshUser();
                Users.SelectionChanged += (se, arg) =>
                RefreshUser();

                painted = true;
            };

            dataBase.ToDisplay("MainSettings");

            NS2Controls();

            Users.DataBindingComplete += (s, e) =>
            {
                foreach (DataGridViewRow row in Users.Rows)
                {
                    if (row.Cells["What"].Value.ToString().Equals("Вошел в систему") || row.Cells["What"].Value.ToString().Equals("Вернулся"))
                    {
                        row.Cells["What"].Style.BackColor = Supports.Green;
                        row.Cells["What"].Style.SelectionBackColor = System.Drawing.Color.DarkGreen;
                        row.Cells["LastSeen"].Style.BackColor = Supports.Green;
                        row.Cells["LastSeen"].Style.SelectionBackColor = System.Drawing.Color.DarkGreen;
                    }
                    else if (row.Cells["What"].Value.ToString().Equals("Вышел из системы") || row.Cells["What"].Value.ToString().Equals(""))
                    {
                        row.Cells["What"].Style.BackColor = Supports.Red;
                        row.Cells["What"].Style.SelectionBackColor = Color.DarkRed;
                        row.Cells["LastSeen"].Style.BackColor = Supports.Red;
                        row.Cells["LastSeen"].Style.SelectionBackColor = Color.DarkRed;
                    }
                    else if (row.Cells["What"].Value.ToString().Equals("Отошел"))
                    {
                        row.Cells["What"].Style.BackColor = Supports.Yellow;
                        row.Cells["What"].Style.SelectionBackColor = Color.DarkGoldenrod;
                        row.Cells["LastSeen"].Style.BackColor = Supports.Yellow;
                        row.Cells["LastSeen"].Style.SelectionBackColor = Color.DarkGoldenrod;
                    }
                    if (Convert.ToBoolean(row.Cells["Allowed"].Value).Equals(true))
                    {
                        row.Cells["Пользователь"].Style.BackColor = Color.Green;
                        row.Cells["Пользователь"].Style.SelectionBackColor = Color.Green;
                    }
                    else
                    {
                        row.Cells["Пользователь"].Style.BackColor = Color.Red;
                        row.Cells["Пользователь"].Style.SelectionBackColor = Color.Red;
                    }


                }
            };

            if (dataBase.dataset.Tables["MainSettings"].Select().Where(x => x["What"].ToString() == "NS2DocumentationTextBox").Where(x => x["Who"].ToString() == "SatelliteForDailyReport").Count() != 0)
                Controls.Find("NS2DocumentationTextBox", true).FirstOrDefault().Text = dataBase.dataset.Tables["MainSettings"].Select().Where(x => x["What"].ToString() == "NS2DocumentationTextBox").Where(x => x["Who"].ToString() == "SatelliteForDailyReport").FirstOrDefault()["Content"].ToString();
            if (dataBase.dataset.Tables["MainSettings"].Select().Where(x => x["What"].ToString() == "NS2DocumentationTextBox1").Where(x => x["Who"].ToString() == "SatelliteForDailyReport").Count() != 0)
                Controls.Find("NS2DocumentationTextBox1", true).FirstOrDefault().Text = dataBase.dataset.Tables["MainSettings"].Select().Where(x => x["What"].ToString() == "NS2DocumentationTextBox1").Where(x => x["Who"].ToString() == "SatelliteForDailyReport").FirstOrDefault()["Content"].ToString();
            if (dataBase.dataset.Tables["MainSettings"].Select().Where(x => x["What"].ToString() == "NS2DocumentationTextBox2").Where(x => x["Who"].ToString() == "SatelliteForDailyReport").Count() != 0)
                Controls.Find("NS2DocumentationTextBox2", true).FirstOrDefault().Text = dataBase.dataset.Tables["MainSettings"].Select().Where(x => x["What"].ToString() == "NS2DocumentationTextBox2").Where(x => x["Who"].ToString() == "SatelliteForDailyReport").FirstOrDefault()["Content"].ToString();

            CheckedListBox ch = (Controls.Find("NS2DocumentationCheckedListBox", true).FirstOrDefault() as CheckedListBox);
            foreach (DataRow st in dataBase.SimpleData("SatelliteList WHERE Принадлежность = 'MIL' OR Принадлежность IS NULL OR Принадлежность = 'TWO'").Rows)
            {
                ch.Items.Add(st["НаименованиеИСЗ"].ToString());
                if (dataBase.dataset.Tables["MainSettings"].Rows.Cast<DataRow>().Where(x => x["What"].ToString() == "NS2DocumentationCheckedListBox").Where(x => x["Content"].ToString() == st["НаименованиеИСЗ"].ToString()).Count() != 0)
                    ch.SetItemChecked(ch.Items.Count - 1, true);
            }

            ch = (Controls.Find("NS2DocumentationCheckedListBox1", true).FirstOrDefault() as CheckedListBox);
            foreach (DataRow st in dataBase.SimpleData("SatelliteList WHERE Принадлежность = 'MIL' OR Принадлежность IS NULL OR Принадлежность = 'TWO'").Rows)
            {
                ch.Items.Add(st["НаименованиеИСЗ"].ToString());
                if (dataBase.dataset.Tables["MainSettings"].Rows.Cast<DataRow>().Where(x => x["What"].ToString() == "NS2DocumentationCheckedListBox1").Where(x => x["Content"].ToString() == st["НаименованиеИСЗ"].ToString()).Count() != 0)
                    ch.SetItemChecked(ch.Items.Count - 1, true);
            }

            (Controls.Find("NS2DocumentationButton", true).FirstOrDefault() as Button).Click += (s, a) =>
            {
                ch = (Controls.Find("NS2DocumentationCheckedListBox", true).FirstOrDefault() as CheckedListBox);
                DataRow dr;
                if (dataBase.dataset.Tables["MainSettings"].Select().Where(x => x["What"].ToString() == "NS2DocumentationTextBox").Where(x => x["Who"].ToString() == "SatelliteForDailyReport").Count() != 0)
                    dataBase.dataset.Tables["MainSettings"].Select().Where(x => x["What"].ToString() == "NS2DocumentationTextBox").Where(x => x["Who"].ToString() == "SatelliteForDailyReport").FirstOrDefault()["Content"] = (Controls.Find("DeilyReport202", true).FirstOrDefault() as FlowLayoutPanel).Controls.Find("NS2DocumentationTextBox", false).FirstOrDefault().Text;
                else
                {
                    dr = dataBase.dataset.Tables["MainSettings"].NewRow();
                    dr["Who"] = "SatelliteForDailyReport";
                    dr["What"] = "NS2DocumentationTextBox";
                    dr["Content"] = (Controls.Find("DeilyReport202", true).FirstOrDefault() as FlowLayoutPanel).Controls.Find("NS2DocumentationTextBox", false).FirstOrDefault().Text;
                    dataBase.dataset.Tables["MainSettings"].Rows.Add(dr);
                }

                if (dataBase.dataset.Tables["MainSettings"].Select().Where(x => x["What"].ToString() == "NS2DocumentationTextBox1").Where(x => x["Who"].ToString() == "SatelliteForDailyReport").Count() != 0)
                    dataBase.dataset.Tables["MainSettings"].Select().Where(x => x["What"].ToString() == "NS2DocumentationTextBox1").Where(x => x["Who"].ToString() == "SatelliteForDailyReport").FirstOrDefault()["Content"] = (Controls.Find("DeilyReport202", true).FirstOrDefault() as FlowLayoutPanel).Controls.Find("NS2DocumentationTextBox1", false).FirstOrDefault().Text;
                else
                {
                    dr = dataBase.dataset.Tables["MainSettings"].NewRow();
                    dr["Who"] = "SatelliteForDailyReport";
                    dr["What"] = "NS2DocumentationTextBox1";
                    dr["Content"] = Controls.Find("NS2DocumentationTextBox1", false).FirstOrDefault().Text;
                    dataBase.dataset.Tables["MainSettings"].Rows.Add(dr);
                }
                if (dataBase.dataset.Tables["MainSettings"].Select().Where(x => x["What"].ToString() == "NS2DocumentationTextBox2").Where(x => x["Who"].ToString() == "SatelliteForDailyReport").Count() != 0)
                    dataBase.dataset.Tables["MainSettings"].Select().Where(x => x["What"].ToString() == "NS2DocumentationTextBox2").Where(x => x["Who"].ToString() == "SatelliteForDailyReport").FirstOrDefault()["Content"] = (Controls.Find("DeilyReport202", true).FirstOrDefault() as FlowLayoutPanel).Controls.Find("NS2DocumentationTextBox2", false).FirstOrDefault().Text;
                else
                {
                    dr = dataBase.dataset.Tables["MainSettings"].NewRow();
                    dr["Who"] = "SatelliteForDailyReport";
                    dr["What"] = "NS2DocumentationTextBox2";
                    dr["Content"] = (Controls.Find("DeilyReport202", true).FirstOrDefault() as FlowLayoutPanel).Controls.Find("NS2DocumentationTextBox2", false).FirstOrDefault().Text;
                    dataBase.dataset.Tables["MainSettings"].Rows.Add(dr);
                }

                List<string> deletedRows = new List<string>();

                for (var i = 0; i < ch.Items.Count; i++)
                {
                    if (dataBase.dataset.Tables["MainSettings"].Rows.Cast<DataRow>().Where(x => x["What"].ToString() == "NS2DocumentationCheckedListBox").Where(x => x["Content"].ToString() == ch.Items[i].ToString()).Count() != 0 && ch.GetItemChecked(i) == false)
                    {
                        deletedRows.Add(ch.Items[i].ToString());
                        continue;
                    }
                    if (dataBase.dataset.Tables["MainSettings"].Rows.Cast<DataRow>().Where(x => x["What"].ToString() == "NS2DocumentationCheckedListBox").Where(x => x["Content"].ToString() == ch.Items[i].ToString()).Count() == 0 && ch.GetItemChecked(i) == true)
                    {
                        dr = dataBase.dataset.Tables["MainSettings"].NewRow();
                        dr["Who"] = "SatelliteForDailyReport";
                        dr["What"] = "NS2DocumentationCheckedListBox";
                        dr["Content"] = ch.Items[i];
                        dataBase.dataset.Tables["MainSettings"].Rows.Add(dr);
                        continue;
                    }
                }

                foreach (string st in deletedRows)
                {
                    dataBase.dataset.Tables["MainSettings"].Rows.Cast<DataRow>().Where(x => x["What"].ToString() == "NS2DocumentationCheckedListBox").Where(x => x["Content"].ToString() == st).FirstOrDefault().Delete();
                    dataBase.sqlAdapter.Update(dataBase.dataset.Tables["MainSettings"]);
                    dataBase.dataset.Tables["MainSettings"].Clear();
                    dataBase.sqlAdapter.Fill(dataBase.dataset.Tables["MainSettings"]);
                }


                deletedRows.Clear();
                ch = ((Controls.Find("DeilyReport202", true).FirstOrDefault() as FlowLayoutPanel).Controls.Find("NS2DocumentationCheckedListBox1", false).FirstOrDefault() as CheckedListBox);

                for (var i = 0; i < ch.Items.Count; i++)
                {
                    if (dataBase.dataset.Tables["MainSettings"].Rows.Cast<DataRow>().Where(x => x["What"].ToString() == "NS2DocumentationCheckedListBox1").Where(x => x["Content"].ToString() == ch.Items[i].ToString()).Count() != 0 && ch.GetItemChecked(i) == false)
                    {
                        deletedRows.Add(ch.Items[i].ToString());
                        continue;
                    }
                    if (dataBase.dataset.Tables["MainSettings"].Rows.Cast<DataRow>().Where(x => x["What"].ToString() == "NS2DocumentationCheckedListBox1").Where(x => x["Content"].ToString() == ch.Items[i].ToString()).Count() == 0 && ch.GetItemChecked(i) == true)
                    {
                        dr = dataBase.dataset.Tables["MainSettings"].NewRow();
                        dr["Who"] = "SatelliteForDailyReport";
                        dr["What"] = "NS2DocumentationCheckedListBox1";
                        dr["Content"] = ch.Items[i];
                        dataBase.dataset.Tables["MainSettings"].Rows.Add(dr);
                        continue;
                    }
                }

                foreach (string st in deletedRows)
                {
                    dataBase.dataset.Tables["MainSettings"].Rows.Cast<DataRow>().Where(x => x["What"].ToString() == "NS2DocumentationCheckedListBox1").Where(x => x["Content"].ToString() == st).FirstOrDefault().Delete();
                    dataBase.sqlAdapter.Update(dataBase.dataset.Tables["MainSettings"]);
                    dataBase.dataset.Tables["MainSettings"].Clear();
                    dataBase.sqlAdapter.Fill(dataBase.dataset.Tables["MainSettings"]);
                }

                dataBase.ToDisplay("MainSettings", onlyAdapter: true);
                dataBase.sqlAdapter.Update(dataBase.dataset.Tables["MainSettings"]);
                dataBase.dataset.Tables["MainSettings"].Clear();
                dataBase.sqlAdapter.Fill(dataBase.dataset.Tables["MainSettings"]);
            };

            string filt = null;
            if (!Profile.userLogin.Equals("Kalter"))
                filt = "WHERE [Department] = " + Profile.userDepartment + " AND [Rank] < 9";
            foreach (DataRow st in dataBase.SimpleData("Login " + filt).Rows.Cast<DataRow>())
            {
                (Controls.Find("logHeadMenCheckedListBox", true).FirstOrDefault() as CheckedListBox).Items.Add(st["Пользователь"]);
            }

            (Controls.Find("logHeadWhatCheckedListBox", true).FirstOrDefault() as CheckedListBox).Items.Add("Удалил");
            (Controls.Find("logHeadWhatCheckedListBox", true).FirstOrDefault() as CheckedListBox).Items.Add("Вошел");
            (Controls.Find("logHeadWhatCheckedListBox", true).FirstOrDefault() as CheckedListBox).Items.Add("Отошел");
            (Controls.Find("logHeadWhatCheckedListBox", true).FirstOrDefault() as CheckedListBox).Items.Add("Добавил");
            (Controls.Find("logHeadWhatCheckedListBox", true).FirstOrDefault() as CheckedListBox).Items.Add("Изменил");

            (Controls.Find("logHeadMenCheckedListBox", true).FirstOrDefault() as CheckedListBox).ItemCheck += (s, e) =>
            {
                RefreshLog(2);
            };

            (Controls.Find("logHeadWhatCheckedListBox", true).FirstOrDefault() as CheckedListBox).ItemCheck += (s, e) =>
            {
                RefreshLog(2);
            };

            (Controls.Find("logHeadTime1DateTimePicker", true).FirstOrDefault() as DateTimePicker).ValueChanged += (s, e) =>
            {
                RefreshLog(2);
            };

            (Controls.Find("logHeadTime2DateTimePicker", true).FirstOrDefault() as DateTimePicker).ValueChanged += (s, e) =>
            {
                RefreshLog(2);
            };

            RefreshLog();

            Users.DataSource = dataBase.dataset.Tables["LoginNS"];

            (Controls.Find("logDataGridView", true).FirstOrDefault() as DataGridView).DataSource = dataBase.dataset.Tables["MainLog"];

            (Controls.Find("CurrentUserCardDataGridView", true).FirstOrDefault() as DataGridView).DataSource = dataBase.dataset.Tables["UserMainLog"];

            UpdateUsersCondition();
            t.Start();
        }

        private void RefreshLog(int which = -1)
        {
            string extraFilter1 = null;
            string extraFilter2 = null;
            string extraFilter3 = null;

            if (!Profile.userLogin.Equals("Kalter"))
            {
                if (Profile.userDepartment.Equals(2))
                {
                    extraFilter1 = "WHERE [Department] = " + Profile.userDepartment + " AND [Rank] < 9";
                    extraFilter2 = "AND [Department] = " + Profile.userDepartment + " AND [What] != 'Вернулся' AND [What] != 'Отошел'" + " AND [Rank] < 9";
                    extraFilter3 = " AND [What] != 'Вернулся' AND [What] != 'Отошел'";
                }
                else
                {
                    extraFilter1 = "WHERE [Department] = " + Profile.userDepartment + " AND [Rank] < 9";
                    extraFilter2 = "AND [Department] = " + Profile.userDepartment + " AND [Rank] < 9";
                }
            }

            if (which != 2 && which != 3)
            {
                if (dataBase.dataset.Tables.IndexOf("LoginNS") != -1)
                    dataBase.dataset.Tables["LoginNS"].Clear();
                //ыекштп 
                dataBase.ToDisplay("SELECT [Allowed], [Login],[Face],[Rank],[Department],[NSNote], [Пользователь], (SELECT top 1 [What] FROM [MainLog] WHERE [Who] = [Login].[Login] AND ([What] = 'Вошел в систему' OR [What] = 'Отошел' OR [What] = 'Вышел из системы' OR [What] = 'Вернулся') ORDER BY [WhenItWas] DESC) AS [What], [LastSeen] FROM [dbo].[Login] " + extraFilter1 + " ORDER BY [LastSeen] DESC", dataTableName: "LoginNS", requestJustByMyself: true, withoutComBuilder: true);

            }
            if (which != 1 && which != 3)
            {
                string from = (Controls.Find("logHeadTime1DateTimePicker", true).FirstOrDefault() as DateTimePicker).Value.ToString();
                string to = (Controls.Find("logHeadTime2DateTimePicker", true).FirstOrDefault() as DateTimePicker).Value.AddDays(1).ToString();

                if (dataBase.dataset.Tables.IndexOf("MainLog") != -1)
                    dataBase.dataset.Tables["MainLog"].Clear();
                dataBase.ToDisplay("SELECT[Пользователь],[What],[WhenItWas] FROM [dbo].[MainLog] left outer join [dbo].[Login] on[MainLog].[Who] = [Login].[Login] WHERE WhenItWas >= '" + from + "' AND WhenItWas <=  '" + to + "' " + extraFilter2, dataTableName: "MainLog", requestJustByMyself: true, withoutComBuilder: true);
            }
            if (which != 1 && which != 2)
            {
                string from = DateTime.Today.AddHours(9).ToString();
                string to = DateTime.Today.AddDays(1).AddHours(9).ToString();

                if (dataBase.dataset.Tables.IndexOf("UserMainLog") != -1)
                    dataBase.dataset.Tables["UserMainLog"].Clear();
                dataBase.ToDisplay("SELECT [What],[WhenItWas] FROM [dbo].[MainLog] WHERE  [Who] = '" + (CurrentUserCard.Controls.Find("LoginTextBox", true).FirstOrDefault() as TextBox).Text + "' AND WhenItWas >= '" + from + "' AND WhenItWas <=  '" + to + "' " + extraFilter3, dataTableName: "UserMainLog", requestJustByMyself: true, withoutComBuilder: true);
            }
        }

        private void NS2Controls()
        {
            Controls.Add(new Panel()
            {
                Name = "NS2DocumentationPanel",
                Dock = DockStyle.Top,
                Height = 500,
            });

            Controls.Add(new Panel()
            {
                Name = "logPanel",
                Dock = DockStyle.Top,
                Height = 600,
            });

            Controls.Add(new Panel()
            {
                Name = "NS2WorkPlacePanel",
                Dock = DockStyle.Top,
                Height = 700,
            });

            CurrentUserCard = new Panel() { Dock = DockStyle.Right, Width = 600 };
            Panel Report202 = new Panel() { Dock = DockStyle.Left, Width = 400 };
            Users = new DataGridView()
            {
                Dock = DockStyle.Fill,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                EditMode = DataGridViewEditMode.EditProgrammatically,
                AllowUserToOrderColumns = false,
                AllowUserToResizeRows = false,
                ScrollBars = ScrollBars.Both,
                EnableHeadersVisualStyles = false,
                RowHeadersWidth = 55,
                Height = 500,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                AllowUserToAddRows = false,
            };

            Users.CellMouseDoubleClick += (s, e) =>
            {
                if (MessageBoxTi.Show("Блокировка прав доступа", Convert.ToBoolean(Users["Allowed", e.RowIndex].Value).Equals(true) ? "Заблокировать пользователя?" : "Разблокировать пользователя?") == MessageResult.Yes)
                    dataBase.SimpleRequest("UPDATE [dbo].[Login] SET [Allowed] = " + Convert.ToInt32(!Convert.ToBoolean(Users["Allowed", e.RowIndex].Value)) + " WHERE [Login] = '" + Users["Login", e.RowIndex].Value + "'");
            };

            (Controls.Find("NS2WorkPlacePanel", true).FirstOrDefault() as Panel).Controls.Add(Users);
            (Controls.Find("NS2WorkPlacePanel", true).FirstOrDefault() as Panel).Controls.Add(CurrentUserCard);


            (Controls.Find("logPanel", true).FirstOrDefault() as Panel).Controls.Add(new DataGridView()
            {
                Name = "logDataGridView",
                Dock = DockStyle.Fill,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                EditMode = DataGridViewEditMode.EditProgrammatically,
                AllowUserToOrderColumns = false,
                AllowUserToResizeRows = false,
                ScrollBars = ScrollBars.Both,
                EnableHeadersVisualStyles = false,
                RowHeadersWidth = 55,
                Height = 500,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                AllowUserToAddRows = false,
            });

            (Controls.Find("logPanel", true).FirstOrDefault() as Panel).Controls.Add(new TableLayoutPanel()
            {
                Name = "logHeadTableLayoutPanel",
                Width = 600,
                Dock = DockStyle.Right,
            });

            (Controls.Find("logPanel", true).FirstOrDefault() as Panel).Controls.Add(new Label()
            {
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Text = "Хронология действий пользователей",
                Height = 50,
                Dock = DockStyle.Top,
            });

            (Controls.Find("NS2WorkPlacePanel", true).FirstOrDefault() as Panel).Controls.Add(new Label()
            {
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Text = "О пользователях",
                Height = 50,
                Dock = DockStyle.Top,
            });


            (Controls.Find("logHeadTableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).RowStyles.Clear();
            (Controls.Find("logHeadTableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).ColumnStyles.Clear();
            (Controls.Find("logHeadTableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            (Controls.Find("logHeadTableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            (Controls.Find("logHeadTableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).RowStyles.Add(new RowStyle(SizeType.Percent, 10f));
            (Controls.Find("logHeadTableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).RowStyles.Add(new RowStyle(SizeType.Percent, 90f));



            (Controls.Find("logHeadTableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "logHeadMenGroupBox",
                Text = "Для кого",
                Dock = DockStyle.Fill,
            }, 0, 1);

            (Controls.Find("logHeadTableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "logHeadWhatGroupBox",
                Text = "Для каких действий",
                Dock = DockStyle.Fill,
            }, 1, 1);

            (Controls.Find("logHeadTableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "logHeadTime1GroupBox",
                Text = "От",
                Dock = DockStyle.Fill,
            }, 0, 0);

            (Controls.Find("logHeadTableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "logHeadTime2GroupBox",
                Text = "До",
                Dock = DockStyle.Fill,
            }, 1, 0);

            (Controls.Find("logHeadMenGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(new CheckedListBox()
            {
                Name = "logHeadMenCheckedListBox",
                Dock = DockStyle.Fill,
            });

            (Controls.Find("logHeadTime1GroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(new DateTimePicker()
            {
                Name = "logHeadTime1DateTimePicker",
                Value = DateTime.Today.AddDays(-7),
                Dock = DockStyle.Fill,
            });

            (Controls.Find("logHeadTime2GroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(new DateTimePicker()
            {
                Name = "logHeadTime2DateTimePicker",
                Value = DateTime.Today,
                Dock = DockStyle.Fill,
            });

            (Controls.Find("logHeadWhatGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(new CheckedListBox()
            {
                Name = "logHeadWhatCheckedListBox",
                Dock = DockStyle.Fill,
            });

            (Controls.Find("NS2DocumentationPanel", true).FirstOrDefault() as Panel).Controls.Add(Report202);

            Report202.Controls.Add(new Button()
            {
                Name = "NS2DocumentationButton",
                Dock = DockStyle.Top,
                Width = 37,
                Text = "Сохранить изменения",
            });
            Report202.Controls.Add(new GroupBox()
            {
                Name = "NS2DocumentationGroupBox2",
                Dock = DockStyle.Top,
                Height = 52,
            });
            (Controls.Find("NS2DocumentationGroupBox2", true).FirstOrDefault() as GroupBox).Controls.Add(new TextBox()
            {
                Name = "NS2DocumentationTextBox2",
                Dock = DockStyle.Fill,
            });
            Report202.Controls.Add(new CheckedListBox()
            {
                Name = "NS2DocumentationCheckedListBox1",
                Dock = DockStyle.Top,
                Width = 150,
            });
            Report202.Controls.Add(new GroupBox()
            {
                Name = "NS2DocumentationGroupBox1",
                Dock = DockStyle.Top,
                Height = 52,
            });
            (Controls.Find("NS2DocumentationGroupBox1", true).FirstOrDefault() as GroupBox).Controls.Add(new TextBox()
            {
                Name = "NS2DocumentationTextBox1",
                Dock = DockStyle.Fill,
            });
            Report202.Controls.Add(new CheckedListBox()
            {
                Name = "NS2DocumentationCheckedListBox",
                Dock = DockStyle.Top,
                Width = 150,
            });
            Report202.Controls.Add(new GroupBox()
            {
                Name = "NS2DocumentationGroupBox",
                Dock = DockStyle.Top,
                Height = 52,
            });
            (Controls.Find("NS2DocumentationGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(new TextBox()
            {
                Name = "NS2DocumentationTextBox",
                Dock = DockStyle.Fill,
            });
            Report202.Controls.Add(new Label()
            {
                Name = "NS2DocumentationLabel",
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Text = "Отчет 202 поста",
                Dock = DockStyle.Top,
                Width = 37,
            });
            (Controls.Find("NS2DocumentationPanel", true).FirstOrDefault() as Panel).Controls.Add(new Label()
            {
                Name = "NS2DocumentationLabel",
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Text = "Отчеты",
                Dock = DockStyle.Top,
                Width = 50,
            });


            CurrentUserCard.Controls.Add(new TableLayoutPanel()
            {
                Name = "CurrentUserCardTableLayoutPanel",
                Dock = DockStyle.Fill,
            });

            (Controls.Find("CurrentUserCardTableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).RowStyles.Clear();
            (Controls.Find("CurrentUserCardTableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).ColumnStyles.Clear();
            (Controls.Find("CurrentUserCardTableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).RowStyles.Add(new RowStyle(SizeType.Percent, 25f));
            (Controls.Find("CurrentUserCardTableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
            (Controls.Find("CurrentUserCardTableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).RowStyles.Add(new RowStyle(SizeType.Percent, 55f));

            (Controls.Find("CurrentUserCardTableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(new TableLayoutPanel()
            {
                Name = "CurrentUserCardTableLayoutPanel1",
                Dock = DockStyle.Fill,
            }, 0, 0);
            (Controls.Find("CurrentUserCardTableLayoutPanel1", true).FirstOrDefault() as TableLayoutPanel).RowStyles.Clear();
            (Controls.Find("CurrentUserCardTableLayoutPanel1", true).FirstOrDefault() as TableLayoutPanel).ColumnStyles.Clear();
            (Controls.Find("CurrentUserCardTableLayoutPanel1", true).FirstOrDefault() as TableLayoutPanel).ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            (Controls.Find("CurrentUserCardTableLayoutPanel1", true).FirstOrDefault() as TableLayoutPanel).ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
            (Controls.Find("CurrentUserCardTableLayoutPanel1", true).FirstOrDefault() as TableLayoutPanel).ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));

            (Controls.Find("CurrentUserCardTableLayoutPanel1", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(new TableLayoutPanel()
            {
                Name = "CurrentUserCardTableLayoutPanel2",
                Dock = DockStyle.Fill,
            }, 1, 0);

            (Controls.Find("CurrentUserCardTableLayoutPanel2", true).FirstOrDefault() as TableLayoutPanel).RowStyles.Clear();
            (Controls.Find("CurrentUserCardTableLayoutPanel2", true).FirstOrDefault() as TableLayoutPanel).ColumnStyles.Clear();
            (Controls.Find("CurrentUserCardTableLayoutPanel2", true).FirstOrDefault() as TableLayoutPanel).RowStyles.Add(new RowStyle(SizeType.Percent, 33f));
            (Controls.Find("CurrentUserCardTableLayoutPanel2", true).FirstOrDefault() as TableLayoutPanel).RowStyles.Add(new RowStyle(SizeType.Percent, 33f));
            (Controls.Find("CurrentUserCardTableLayoutPanel2", true).FirstOrDefault() as TableLayoutPanel).RowStyles.Add(new RowStyle(SizeType.Percent, 33f));

            (Controls.Find("CurrentUserCardTableLayoutPanel2", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(new Panel()
            {
                Name = "CurrentUserCardPanel",
                Dock = DockStyle.Fill,
            }, 0, 2);

            (Controls.Find("CurrentUserCardTableLayoutPanel2", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "UserGroupBox",
                Text = "Пользователь",
                Dock = DockStyle.Fill,
            }, 0, 0);

            (Controls.Find("CurrentUserCardTableLayoutPanel2", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "LoginGroupBox",
                Text = "Логин",
                Dock = DockStyle.Fill,
            }, 0, 1);


            (Controls.Find("CurrentUserCardPanel", true).FirstOrDefault() as Panel).Controls.Add(new GroupBox()
            {
                Name = "RankGroupBox",
                Text = "Звание",
                Width = 150,
                Dock = DockStyle.Left,
            });

            (Controls.Find("CurrentUserCardPanel", true).FirstOrDefault() as Panel).Controls.Add(new GroupBox()
            {
                Name = "DepartmentGroupBox",
                Text = "Отдел",
                Width = 70,
                Dock = DockStyle.Right,
            });



            (Controls.Find("CurrentUserCardTableLayoutPanel1", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(new PictureBox()
            {
                Name = "CurrentUserCardPictureBox",
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
            }, 0, 0);

            (Controls.Find("RankGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(new TextBox()
            {
                Name = "RankTextBox",
                Dock = DockStyle.Fill,
                Enabled = false,
            });

            (Controls.Find("DepartmentGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(new TextBox()
            {
                Name = "DepartmentTextBox",
                Enabled = false,
                Dock = DockStyle.Fill,
            });

            (Controls.Find("LoginGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(new TextBox()
            {
                Name = "LoginTextBox",
                Enabled = false,
                Dock = DockStyle.Fill,
            });
            (Controls.Find("UserGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(new TextBox()
            {
                Name = "UserTextBox",
                Enabled = false,
                Dock = DockStyle.Fill,
            });


            (Controls.Find("CurrentUserCardTableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(new DataGridView()
            {
                Name = "CurrentUserCardDataGridView",
                Dock = DockStyle.Fill,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                EditMode = DataGridViewEditMode.EditProgrammatically,
                AllowUserToOrderColumns = false,
                AllowUserToResizeRows = false,
                ScrollBars = ScrollBars.Both,
                EnableHeadersVisualStyles = false,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                AllowUserToAddRows = false,
            }, 0, 2);

            (Controls.Find("CurrentUserCardTableLayoutPanel1", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "StatisticGroupBox",
                Text = "Статистика",
                Dock = DockStyle.Fill,
            }, 2, 0);

            (Controls.Find("CurrentUserCardTableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(new Panel()
            {
                Name = "CurrentUserCardPanel1",
                Dock = DockStyle.Fill,
            }, 0, 1);

            (Controls.Find("CurrentUserCardPanel1", true).FirstOrDefault() as Panel).Controls.Add(new GroupBox()
            {
                Name = "NoteGroupBox",
                Dock = DockStyle.Fill,
                Text = "Сообщение от НС",
            });
            (Controls.Find("NoteGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(new TextBox()
            {
                Name = "NoteTextBox",
                Multiline = true,
                Dock = DockStyle.Fill,
            });

            (Controls.Find("CurrentUserCardPanel1", true).FirstOrDefault() as Panel).Controls.Add(new Button()
            {
                Name = "NoteButton",
                Dock = DockStyle.Right,
                Width = 50,
                Image = Resources.add,
            });

            (Controls.Find("NoteButton", true).FirstOrDefault() as Button).Click += (s, e) =>
            {
                dataBase.SimpleRequest("UPDATE [dbo].[Login] SET [NSNote] = '" + (CurrentUserCard.Controls.Find("NoteTextBox", true).FirstOrDefault() as TextBox).Text + "' WHERE Login = '" + (CurrentUserCard.Controls.Find("LoginTextBox", true).FirstOrDefault() as TextBox).Text + "'");
                dataBase.dataset.Tables["LoginNS"].Rows.Cast<DataRow>().Where(x => x["Login"].ToString().Equals((CurrentUserCard.Controls.Find("LoginTextBox", true).FirstOrDefault() as TextBox).Text)).FirstOrDefault()["NSNote"] = (CurrentUserCard.Controls.Find("NoteTextBox", true).FirstOrDefault() as TextBox).Text;
            };
        }

        private void UpdateUsersCondition()
        {
            int check = dataBase.CheckSum("(SELECT [Allowed], [Пользователь], (SELECT top 1 [What] FROM [MainLog] WHERE [Who] = [Login].[Login] AND ([What] = 'Вошел в систему' OR [What] = 'Отошел' OR [What] = 'Вышел из системы' OR [What] = 'Вернулся') ORDER BY [WhenItWas] DESC) AS [What] FROM [dbo].[Login]) AS [table]");
            if (!checkSum1.Equals(check))
            {
                checkSum1 = check;
                RefreshLog(1);
            }
        }

        private void RefreshUser()
        {
            try
            {
                (CurrentUserCard.Controls.Find("CurrentUserCardPictureBox", true).FirstOrDefault() as PictureBox).Image = (Bitmap)Image.FromStream(new MemoryStream((byte[])Users.SelectedRows[0].Cells["Face"].Value));
                (CurrentUserCard.Controls.Find("UserTextBox", true).FirstOrDefault() as TextBox).Text = Users.SelectedRows[0].Cells["Пользователь"].Value.ToString();
                (CurrentUserCard.Controls.Find("LoginTextBox", true).FirstOrDefault() as TextBox).Text = Users.SelectedRows[0].Cells["Login"].Value.ToString();
                (CurrentUserCard.Controls.Find("RankTextBox", true).FirstOrDefault() as TextBox).Text = ((Supports.Ranks)Convert.ToInt32(Users.SelectedRows[0].Cells["Rank"].Value)).ToString();
                (CurrentUserCard.Controls.Find("DepartmentTextBox", true).FirstOrDefault() as TextBox).Text = Users.SelectedRows[0].Cells["Department"].Value.ToString();
                (CurrentUserCard.Controls.Find("NoteTextBox", true).FirstOrDefault() as TextBox).Text = Users.SelectedRows[0].Cells["NSNote"].Value.ToString();
                RefreshLog(3);

            }
            catch (Exception)
            {

            }

        }
    }
}