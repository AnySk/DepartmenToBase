using Department2Base;
using Department2Base.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Department2Base
{
    class SatelliteList : TabPage
    {

        #region Контролы и глобальные переменные


        string addCount = "SELECT [ID], [НаименованиеИСЗ], [МеждНомер], [ПСТ], [Производитель], [Описание], (SELECT COUNT(*) FROM [Loading] WHERE [НаименованиеИСЗ] = [Loading].[Спутник] AND [Состояние] = 1 ) AS [Изл.], (SELECT COUNT(*) FROM [Loading] WHERE [НаименованиеИСЗ] = [Loading].[Спутник] AND [Состояние] = 1 AND [Диапазон] = 'P') AS [P], (SELECT COUNT(*) FROM [Loading] WHERE [НаименованиеИСЗ] = [Loading].[Спутник] AND [Состояние] = 1 AND [Диапазон] = 'L') AS [L]," +
                          "(SELECT COUNT(*) FROM [Loading] WHERE[НаименованиеИСЗ] = [Loading].[Спутник] AND [Состояние] = 1 AND [Диапазон] = 'S') AS [S], (SELECT COUNT(*) FROM [Loading] WHERE [НаименованиеИСЗ] = [Loading].[Спутник] AND [Состояние] = 1 AND [Диапазон] = 'C') AS [C], (SELECT COUNT(*) FROM [Loading] WHERE [НаименованиеИСЗ] = [Loading].[Спутник] AND [Состояние] = 1 AND [Диапазон] = 'X') AS [X]," +
                          "(SELECT COUNT(*) FROM [Loading] WHERE[НаименованиеИСЗ] = [Loading].[Спутник] AND [Состояние] = 1 AND [Диапазон] = 'K') AS [K], (SELECT COUNT(*) FROM [Loading] WHERE [НаименованиеИСЗ] = [Loading].[Спутник] AND [Состояние] = 1 AND [Диапазон] = 'Ku') AS [Ku], (SELECT COUNT(*) FROM [Loading] WHERE [НаименованиеИСЗ] = [Loading].[Спутник] AND [Состояние] = 1 AND [Диапазон] = 'Ka') AS [Ka], [Состояние], [Примечание], [Принадлежность], [Тредакт] FROM [dbo].[SatelliteList]";

        private static string membership = null;
        private bool binded = false;
        private int currentRow = 0;
        private static DataGridView dgv = new DataGridView()
        {
            BorderStyle = BorderStyle.None,
            Name = "dataGridViewSatelliteList",
            Dock = DockStyle.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            EditMode = DataGridViewEditMode.EditProgrammatically,
            AllowUserToOrderColumns = false,
            AllowUserToResizeRows = false,
            ScrollBars = ScrollBars.Both,
            EnableHeadersVisualStyles = false,
            RowHeadersWidth = 55,
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
            AllowUserToAddRows = false,
        };
        Panel mainPanel = new Panel()
        {
            Name = "SatelliteListPanel0",
            Height = 180,
            Width = 969,
            Dock = DockStyle.Bottom,
        };

        ComboBox НаименованиеИСЗ = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        TextBox МеждНомер = new TextBox() { Dock = DockStyle.Fill, };
        TextBox ПСТ = new TextBox() { Dock = DockStyle.Fill, };
        TextBox Производитель = new TextBox() { Dock = DockStyle.Fill, };
        TextBox Описание = new TextBox()
        {
            Multiline = true,
            Dock = DockStyle.Fill,
        };
        ComboBox Состояние = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        TextBox Примечание = new TextBox()
        {
            Multiline = true,
            Dock = DockStyle.Fill,
        };
        ComboBox Принадлежность = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        ComboBox Диапазон = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        ComboBox Поляризация = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        TextBox Излучения = new TextBox()
        {
            Modified = false,
            Dock = DockStyle.Fill,
        };
        Button editSatellite = new Button()
        {
            Name = "editButton",
            Text = "Редактировать ИСЗ",
            Width = 170,
            Height = 37,
        };
        Button addSatellite = new Button()
        {
            Name = "addButton",
            Text = "Добавить ИСЗ в БД",
            Width = 170,
            Height = 37,
        };
        Button goToBand = new Button()
        {
            Name = "passButton",
            Text = "Переход к ВЧ-загрузке",
            Width = 170,
            Height = 37,
        };

        #endregion

        public SatelliteList()
        {
            Name = "tabPageSatelliteList";
            Text = "ИСЗ";

            SatelliteListControls();
            LoadComboUp();
            Supports.GangeGroup(this);

            switch (dataBase.ToCount("SELECT [SatellitesType] FROM [Login] WHERE [Login].[Login] = '" + Profile.userLogin + "'").ToString())
            {
                case "CIV":
                    membership = " WHERE Принадлежность = 'CIV' OR Принадлежность IS NULL OR Принадлежность = 'TWO'";
                    Диапазон.SelectedIndex = 5;
                    Поляризация.SelectedIndex = 3;
                    break;
                case "MIL":
                    membership = " WHERE Принадлежность = 'MIL' OR Принадлежность IS NULL OR Принадлежность = 'TWO'";
                    Диапазон.SelectedIndex = 4;
                    Поляризация.SelectedIndex = 0;
                    break;
                default:
                    Диапазон.SelectedIndex = 3;
                    Поляризация.SelectedIndex = 3;
                    break;
            }



            Enter += (s, e) =>
            {
                if (binded)
                {
                    dataBase.ToDisplay(addCount + membership + " ORDER BY [НаименованиеИСЗ]", dataTableName: "SatelliteList", requestJustByMyself: true);
                    dgv.CurrentCell = dgv["НаименованиеИСЗ", currentRow];
                }
            };
            Leave += (s, e) =>
            {
                currentRow = dgv.CurrentCell.RowIndex;
            };

            dgv.DataBindingComplete += (s, args) =>
            {

                foreach (DataGridViewRow r in dgv.Rows)
                    r.HeaderCell.Value = String.Format("{0}", r.Index + 1);

                if (binded)
                    return;
                binded = true;

                dgv.Columns["ID"].Visible = false;
                dgv.Columns["Примечание"].Visible = false;
                dgv.Columns["Описание"].Visible = false;
                dgv.Columns["Принадлежность"].Visible = false;
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                dgv.SelectionChanged += DownControlsUpdate;

                Диапазон.SelectedValueChanged += (es, e) => DownSignUpdate();
                Поляризация.SelectedValueChanged += (se, e) => DownSignUpdate();
                
                    
            };

            dgv.UserDeletedRow += (s, e) => dataBase.sqlAdapter.Update(dataBase.dataset.Tables["SSALoading"]);
            dgv.UserDeletingRow += (sender, e) =>
            {
                dataBase.ToUpdate(Profile.userLogin, "Удалил запись о спутнике " + '"' + e.Row.Cells["НаименованиеИСЗ"].Value.ToString() + '"' + "\"");
                e.Cancel = false;
            };

            dgv.CellMouseClick += (sender, e) =>
            {
                if (dgv != null)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        Supports.MenuPanel(dgv, e.RowIndex, e.ColumnIndex, typeof(SatelliteList));
                    }
                    if (e.Button == MouseButtons.Left)
                    {

                    }
                }
            };
            dgv.MouseMove += (se, arg) =>
            {
                if (dgv.ContextMenuStrip != null)
                    dgv.ContextMenuStrip.Dispose();
            };
            dgv.KeyDown += (s, e) =>
            {
                if (e.KeyData == Keys.Delete)
                    e.Handled = true;
            };

            editSatellite.Click += (s, e) =>
            {
                if (dgv.SelectedCells.Count == 0)
                    return;

                bool red = false;

                dgv.CurrentCell = dgv.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["НаименованиеИСЗ"].Value.ToString() == НаименованиеИСЗ.Text).FirstOrDefault().Cells["НаименованиеИСЗ"];

                if (MessageBoxTi.Show("Вы действительно хотите редактировать данные этого спутника?", "Редактирование") == MessageResult.Yes)
                {
                    var ind = dgv.SelectedRows[0].Index;

                    if (!dgv.SelectedRows[0].Cells["МеждНомер"].Value.ToString().Equals(МеждНомер.Text))
                    {
                        dataBase.ToUpdate(Profile.userLogin, "Изменил запись о спутнике \"" + НаименованиеИСЗ.Text + "\" в графе \"МеждНомер\" с \"" + МеждНомер.Text + "\" на \"" + МеждНомер.Text + "\"");
                        dataBase.dataset.Tables["SatelliteList"].Select().Where(x => x["ID"].ToString().Equals(dgv["ID", ind].Value.ToString())).FirstOrDefault()["МеждНомер"] = МеждНомер.Text;
                        red = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["ПСТ"].Value.ToString().Equals(ПСТ.Text))
                    {
                        dataBase.ToUpdate(Profile.userLogin, "Изменил запись о спутнике \"" + НаименованиеИСЗ.Text + "\" в графе \"ПСТ\" с \"" + ПСТ.Text + "\" на \"" + ПСТ.Text + "\"");
                        dataBase.dataset.Tables["SatelliteList"].Select().Where(x => x["ID"].ToString().Equals(dgv["ID", ind].Value.ToString())).FirstOrDefault()["ПСТ"] = ПСТ.Text;
                        red = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["Производитель"].Value.ToString().Equals(Производитель.Text))
                    {
                        dataBase.ToUpdate(Profile.userLogin, "Изменил запись о спутнике \"" + НаименованиеИСЗ.Text + "\" в графе \"Производитель\" с \"" + Производитель.Text + "\" на \"" + Производитель.Text + "\"");
                        dataBase.dataset.Tables["SatelliteList"].Select().Where(x => x["ID"].ToString().Equals(dgv["ID", ind].Value.ToString())).FirstOrDefault()["Производитель"] = Производитель.Text;
                        red = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["Описание"].Value.ToString().Equals(Описание.Text))
                    {
                        dataBase.ToUpdate(Profile.userLogin, "Изменил запись о спутнике \"" + НаименованиеИСЗ.Text + "\" в графе \"Описание\" с \"" + Описание.Text + "\" на \"" + Описание.Text + "\"");
                        dataBase.dataset.Tables["SatelliteList"].Select().Where(x => x["ID"].ToString().Equals(dgv["ID", ind].Value.ToString())).FirstOrDefault()["Описание"] = Описание.Text;
                        red = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["Состояние"].Value.ToString().Equals(Состояние.Text))
                    {
                        dataBase.ToUpdate(Profile.userLogin, "Изменил запись о спутнике \"" + НаименованиеИСЗ.Text + "\" в графе \"Состояние\" с \"" + Состояние.Text + "\" на \"" + Состояние.Text + "\"");
                        dataBase.dataset.Tables["SatelliteList"].Select().Where(x => x["ID"].ToString().Equals(dgv["ID", ind].Value.ToString())).FirstOrDefault()["Состояние"] = Состояние.Text;
                        red = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["Примечание"].Value.ToString().Equals(Примечание.Text))
                    {
                        dataBase.ToUpdate(Profile.userLogin, "Изменил запись о спутнике \"" + НаименованиеИСЗ.Text + "\" в графе \"Примечание\" с \"" + Примечание.Text + "\" на \"" + Примечание.Text + "\"");
                        dataBase.dataset.Tables["SatelliteList"].Select().Where(x => x["ID"].ToString().Equals(dgv["ID", ind].Value.ToString())).FirstOrDefault()["Примечание"] = Примечание.Text;
                        red = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["Принадлежность"].Value.ToString().Equals(Принадлежность.Text))
                    {
                        dataBase.ToUpdate(Profile.userLogin, "Изменил запись о спутнике \"" + НаименованиеИСЗ.Text + "\" в графе \"Принадлежность\" с \"" + Принадлежность.Text + "\" на \"" + Принадлежность.Text + "\"");
                        dataBase.dataset.Tables["SatelliteList"].Select().Where(x => x["ID"].ToString().Equals(dgv["ID", ind].Value.ToString())).FirstOrDefault()["Принадлежность"] = Принадлежность.Text;
                        red = true;
                    }

                    if (red == false)
                        return;
                    dataBase.dataset.Tables["SatelliteList"].Select().Where(x => x["ID"].ToString().Equals(dgv["ID", ind].Value.ToString())).FirstOrDefault()["Тредакт"] = dataBase.ToCount("SELECT GETDATE()");
                    dataBase.sqlAdapter.Update(dataBase.dataset.Tables["SatelliteList"]);
                    LoadComboUp();
                }
            };
            addSatellite.Click += (s, e) =>
            {
                string h = MessageBoxTi.Show("Добавление спутника", "Введите название спутника", HorizontalAlignment.Left);
                while ((int)dataBase.ToCount("SELECT COUNT(*) FROM[dbo].[SatelliteList] WHERE[dbo].[SatelliteList].[НаименованиеИСЗ] = '" + h + "'") != 0)
                {
                    h = MessageBoxTi.Show("Добавление спутника", "Такой спутник уже есть! Введите другое название спутника", HorizontalAlignment.Left);
                }
                if (h == null || h == "")
                    return;
                DataRow p = dataBase.dataset.Tables["SatelliteList"].NewRow();
                p["НаименованиеИСЗ"] = h;
                p["МеждНомер"] = МеждНомер.Text;
                p["ПСТ"] = ПСТ.Text;
                p["Производитель"] = Производитель.Text;
                p["Описание"] = Описание.Text;
                p["Состояние"] = Состояние.Text;
                p["Примечание"] = Примечание.Text;
                p["Тредакт"] = dataBase.ToCount("SELECT GETDATE()");
                p["Принадлежность"] = Принадлежность.Text;

                if (p["НаименованиеИСЗ"].ToString() != "")
                {
                    if (dataBase.dataset.Tables["SatelliteList"].Select().Where(x => x["НаименованиеИСЗ"].Equals(p["НаименованиеИСЗ"])).Count() == 0)
                    {
                        dataBase.dataset.Tables["SatelliteList"].Rows.Add(p);
                        dataBase.ToUpdate(Profile.userLogin, "Добавил спутник \"" + p["НаименованиеИСЗ"] + "\"");
                        dataBase.sqlAdapter.Update(dataBase.dataset.Tables["SatelliteList"]);
                        dataBase.ToDisplay(addCount + membership + " ORDER BY [НаименованиеИСЗ]", dataTableName: "SatelliteList", requestJustByMyself: true);
                        dgv.CurrentCell = dgv.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["НаименованиеИСЗ"].Value.ToString() == h).FirstOrDefault().Cells[1];
                    }
                    else
                    {
                        MessageBoxTi.Show("Спутник с таким названием уже есть");
                    }

                }

            };
            goToBand.Click += (s, e) =>
            {
                if (Profile.tabControl1.TabPages.IndexOfKey("WHERE Спутник = \'" + НаименованиеИСЗ.Text + "\' AND Диапазон = \'" + Диапазон.Text + "\' AND Поляризация = \'" + Поляризация.Text + "\'") == -1)
                {
                    if (НаименованиеИСЗ.Text == "")
                    {
                        MessageBoxTi.Show("Введите название спутника");
                    }
                    else
                    {
                        if (dataBase.dataset.Tables["SatelliteList"].Select().Where(x => x["НаименованиеИСЗ"].Equals(НаименованиеИСЗ.Text)).Count() != 0)
                        {
                            if (Convert.ToInt32(Излучения.Text) != 0)
                            {
                                Profile.tabControl1.TabPages.Add(new Loading(НаименованиеИСЗ.Text, Диапазон.Text, Поляризация.Text));
                                Profile.tabControl1.SelectedTab = Profile.tabControl1.TabPages["WHERE Спутник = \'" + НаименованиеИСЗ.Text + "\' AND Диапазон = \'" + Диапазон.Text + "\' AND Поляризация = \'" + Поляризация.Text + "\'"];
                            }
                            else
                            {
                                if (MessageBoxTi.Show("Нет такого типа излучений", "Добавить новое излучение?") == MessageResult.Yes)
                                {
                                    Profile.tabControl1.TabPages.Add(new Loading(НаименованиеИСЗ.Text, Диапазон.Text, Поляризация.Text));
                                    Profile.tabControl1.SelectedTab = Profile.tabControl1.TabPages["WHERE Спутник = \'" + НаименованиеИСЗ.Text + "\' AND Диапазон = \'" + Диапазон.Text + "\' AND Поляризация = \'" + Поляризация.Text + "\'"];
                                }
                                else
                                {
                                    return;
                                }
                            }
                        }
                        else
                        {
                            MessageBoxTi.Show("Спутника с таким названием не существует");
                        }
                    }
                }
                else
                {
                    Profile.tabControl1.SelectedTab = Profile.tabControl1.TabPages["WHERE Спутник = \'" + НаименованиеИСЗ.Text + "\' AND Диапазон = \'" + Диапазон.Text + "\' AND Поляризация = \'" + Поляризация.Text + "\'"];
                    return;
                }
            };        

            dataBase.ToDisplay(addCount + membership + " ORDER BY [НаименованиеИСЗ]", dataTableName: "SatelliteList", requestJustByMyself: true);
            dgv.DataSource = dataBase.dataset.Tables["SatelliteList"];
        }

        private void LoadComboUp()
        {
            DataTable Satellite = dataBase.SimpleData("SatelliteList");
            НаименованиеИСЗ.Items.Clear();
            foreach (string st in Satellite.Rows.Cast<DataRow>().Select(x => x["НаименованиеИСЗ"].ToString()).OrderBy(x => x).ToList())
                НаименованиеИСЗ.Items.Add(st);

            Состояние.Items.Clear();
            foreach (string st in Satellite.Rows.Cast<DataRow>().Select(x => x["Состояние"].ToString()).OrderBy(x => x).Distinct().ToList())
                Состояние.Items.Add(st);

            foreach (string st in dataBase.SimpleData("FrequencyBand").Rows.Cast<DataRow>().Select(x => x["Наименование диапазона"].ToString()).ToList())
                Диапазон.Items.Add(st);

            Принадлежность.Items.Clear();
            Принадлежность.Items.Add("MIL");
            Принадлежность.Items.Add("CIV");
            Принадлежность.Items.Add("TWO");
            Принадлежность.Enabled = true;

            Поляризация.Items.Clear();
            Поляризация.Items.Add("L");
            Поляризация.Items.Add("R");
            Поляризация.Items.Add("V");
            Поляризация.Items.Add("H");
            Поляризация.Enabled = true;

            return;
        }

        private void SatelliteListControls()
        {
            Controls.Add(dgv);

            Controls.Add(mainPanel);

            (Controls.Find("SatelliteListPanel0", true).FirstOrDefault() as Panel).Controls.Add(new TableLayoutPanel()
            {
                Name = "SatellitePanel1",
                ColumnCount = 3,
                RowCount = 1,
                Location = new Point(((Controls.Find("SatelliteListPanel0", true).FirstOrDefault() as Panel).Width / 2) - (969 / 2), 0),
                Anchor = AnchorStyles.None,
                Height = 180,
                Width = 969,
                BackColor = Supports.headGrey,
            });

            (Controls.Find("SatellitePanel1", true).FirstOrDefault() as TableLayoutPanel).ColumnStyles.Clear();
            (Controls.Find("SatellitePanel1", true).FirstOrDefault() as TableLayoutPanel).ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22f));
            (Controls.Find("SatellitePanel1", true).FirstOrDefault() as TableLayoutPanel).ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 56f));
            (Controls.Find("SatellitePanel1", true).FirstOrDefault() as TableLayoutPanel).ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22f));

            (Controls.Find("SatellitePanel1", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "descriptionGroupBox",
                Text = "Описание ИСЗ:",
                Dock = DockStyle.Fill,
            }, 0, 0);
            (Controls.Find("SatellitePanel1", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "particularlyGroupBox",
                Text = "Примечание:",
                Dock = DockStyle.Fill,
            }, 2, 0);

            (Controls.Find("SatellitePanel1", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(new FlowLayoutPanel()
            {
                Name = "SatellitePanel4",
                Dock = DockStyle.Fill,
            }, 1, 0);

            (Controls.Find("SatellitePanel4", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "nameISZGroupBox",
                Text = "Название ИСЗ:",
                Width = 170,
                Height = 37,
            });

            (Controls.Find("SatellitePanel4", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "manufacturerGroupBox",
                Text = "Производитель ИСЗ:",
                Width = 170,
                Height = 37,
            });

            (Controls.Find("SatellitePanel4", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "membershipGroupBox",
                Text = "Принадлежность:",
                Width = 170,
                Height = 37,
            });

            (Controls.Find("SatellitePanel4", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "internationalNumberGroupBox",
                Text = "Межд. №:",
                Width = 170,
                Height = 37,
            });

            (Controls.Find("SatellitePanel4", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "PSTGroupBox",
                Text = "ПСТ:",
                Width = 170,
                Height = 37,
            });

            (Controls.Find("SatellitePanel4", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "analysisConditionGroupBox",
                Text = "Состояние анализа:",
                Width = 170,
                Height = 37,
            });

            (Controls.Find("SatellitePanel4", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(editSatellite);

            (Controls.Find("SatellitePanel4", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(addSatellite);

            (Controls.Find("SatellitePanel4", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(goToBand);

            (Controls.Find("SatellitePanel4", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "polarizationGroupBox",
                Text = "Поляризация:",
                Width = 170,
                Height = 37,
            });

            (Controls.Find("SatellitePanel4", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "bandGroupBox",
                Text = "Диапазон:",
                Height = 37,
                Width = 170,
            });

            (Controls.Find("SatellitePanel4", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "beamingGroupBox",
                Text = "Излучений:",
                Width = 170,
                Height = 37,
            });

            (Controls.Find("particularlyGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(Примечание);

            (Controls.Find("manufacturerGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(Производитель);

            (Controls.Find("membershipGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(Принадлежность);

            (Controls.Find("nameISZGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(НаименованиеИСЗ);

            (Controls.Find("beamingGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(Излучения);

            (Controls.Find("analysisConditionGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(Состояние);

            (Controls.Find("PSTGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(ПСТ);

            (Controls.Find("internationalNumberGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(МеждНомер);

            (Controls.Find("polarizationGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(Поляризация);

            (Controls.Find("bandGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(Диапазон);

            (Controls.Find("descriptionGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(Описание);
        }

        private void DownSignUpdate()
        {   
            List<object> pol = dataBase.SingleRow("SELECT  (SELECT COUNT(*) FROM [Loading] WHERE [НаименованиеИСЗ] = [Loading].[Спутник] AND [Состояние] = 1 AND [Диапазон] = '" + Диапазон.Text + "' AND [Поляризация] = 'L') AS [L]," +
                                                  "(SELECT COUNT(*) FROM[Loading] WHERE[НаименованиеИСЗ] = [Loading].[Спутник] AND[Состояние] = 1 AND[Диапазон] = '" + Диапазон.Text + "' AND[Поляризация] = 'R') AS[R]," +
                                                  "(SELECT COUNT(*) FROM[Loading] WHERE[НаименованиеИСЗ] = [Loading].[Спутник] AND[Состояние] = 1 AND[Диапазон] = '" + Диапазон.Text + "' AND[Поляризация] = 'V') AS[V], " +
                                                  "(SELECT COUNT(*) FROM[Loading] WHERE[НаименованиеИСЗ] = [Loading].[Спутник] AND[Состояние] = 1 AND[Диапазон] = '" + Диапазон.Text + "' AND[Поляризация] = 'H') AS[H]" +
                                                  "FROM[dbo].[SatelliteList] where[НаименованиеИСЗ] = '" + НаименованиеИСЗ.Text + "'");

            Profile.DownSign = Диапазон.Text + " диапазон: " + " L:" + pol[0] + " R:" + pol[1] + " V:" + pol[2] + " H:" + pol[3];

            Излучения.Text = dataBase.ToCount(
                "SELECT COUNT(*) FROM[dbo].[Loading] WHERE[dbo].[Loading].[Спутник] = '" +
                НаименованиеИСЗ.Text +
                "' AND [dbo].[Loading].[Диапазон] = '" +
                Диапазон.Text +
                "' AND [dbo].[Loading].[Поляризация] = '" +
                Поляризация.Text +
                "'").ToString();
        }

        private void DownControlsUpdate(object sender, EventArgs e)
        {
            try
            {
                НаименованиеИСЗ.Text = dgv.SelectedRows[0].Cells["НаименованиеИСЗ"].Value.ToString();
                Состояние.Text = dgv.SelectedRows[0].Cells["Состояние"].Value.ToString();
                Производитель.Text = dgv.SelectedRows[0].Cells["Производитель"].Value.ToString();
                Описание.Text = dgv.SelectedRows[0].Cells["Описание"].Value.ToString();
                ПСТ.Text = dgv.SelectedRows[0].Cells["ПСТ"].Value.ToString();
                МеждНомер.Text = dgv.SelectedRows[0].Cells["МеждНомер"].Value.ToString();
                Примечание.Text = dgv["Примечание", dgv.SelectedRows[0].Index].Value.ToString(); dgv.SelectedRows[0].Cells["Примечание"].Value.ToString();
                Принадлежность.Text = dgv.SelectedRows[0].Cells["Принадлежность"].Value.ToString();

                DownSignUpdate();
            }
            catch (Exception) { }

        }
    }
}