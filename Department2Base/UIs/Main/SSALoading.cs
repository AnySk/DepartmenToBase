using Department2Base.Properties;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Department2Base
{
    class SSALoading : TabPage
    {
        #region Контролы и глобальные переменные
        bool firstBind = false;

        ComboBox Спутник = new ComboBox()
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
        NumericUpDown Частота = new NumericUpDown()
        {
            Dock = DockStyle.Fill,
            BackColor = Supports.textGray,
            ForeColor = Supports.textWhite,
            BorderStyle = BorderStyle.FixedSingle,
            Maximum = 10000000000,

        };
        NumericUpDown Тактовая = new NumericUpDown()
        {
            Dock = DockStyle.Fill,
            BackColor = Supports.textGray,
            ForeColor = Supports.textWhite,
            BorderStyle = BorderStyle.FixedSingle,
            Maximum = 10000000000,
        };
        TextBox Примечание = new TextBox()
        {
            Multiline = true,
            Dock = DockStyle.Fill,
        };
        DateTimePicker dtp = new DateTimePicker()
        {
            Dock = DockStyle.Fill,
            Value = DateTime.Today
        };
        ComboBox Модуляция = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        ComboBox RПУК = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        ComboBox ПУК = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        ComboBox ВидДоступа = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        ComboBox СистемаСвязи = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        ComboBox Скремб = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        ComboBox ХарРаботы = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        ComboBox ХарактерИнфо = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        ComboBox ВозмСобытие = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        ComboBox Принадлежность = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        ComboBox СостДеятельности = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        ComboBox ВидИсточника = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        ComboBox ВидОбъекта = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        ComboBox Протоколы = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        TextBox Скорость = new TextBox() { Dock = DockStyle.Fill, };
        DateTimePicker ВремяВкл = new DateTimePicker()
        {
            Dock = DockStyle.Top,
        };
        SQLRequestFilter filter = null;
        DataGridView dgv = new DataGridView()
        {
            Dock = DockStyle.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            EditMode = DataGridViewEditMode.EditProgrammatically,
            AllowUserToOrderColumns = false,
            ScrollBars = ScrollBars.Both,
            EnableHeadersVisualStyles = false,
            AllowUserToAddRows = false,
            RowHeadersVisible = false,
            AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
            //AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader,
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
        };
        Button addButton = new Button()
        {
            Height = 46,
            Width = 46,
            Image = Resources.add,
            Dock = DockStyle.Bottom,
            Margin = new Padding(9, 0, 9, 6),
        };
        Button editButton = new Button()
        {
            Height = 46,
            Width = 46,
            Image = Resources.edit,
            Dock = DockStyle.Bottom,
            Margin = new Padding(0, 0, 0, 6),
        };
        Button addTimeButton = new Button()
        {
            Dock = DockStyle.Fill,
            Image = Resources.right,
            BackgroundImageLayout = ImageLayout.Zoom,
            ImageAlign = ContentAlignment.MiddleCenter,
        };
        Button subtractTimeButton = new Button()
        {
            Dock = DockStyle.Fill,
            Image = Resources.left,
            ImageAlign = ContentAlignment.MiddleCenter,
            BackgroundImageLayout = ImageLayout.Zoom,
        };

        #endregion

        public SSALoading()
        {
            Name = "tabPageSSALoading";
            Text = "ССА";

            SSALoadingControls();
            FillBottomControls();
            AddFreeSpase(Controls.Find("SSALoadingPanel0", false).FirstOrDefault());
            Supports.GangeGroup(this);

            dgv.DataBindingComplete += (s, e) =>
            {

                RefreshDGV();

                if (firstBind)
                    return;

                dgv.Columns["ID"].Visible = false;
                dgv.Columns["Состояние"].Visible = false;

                firstBind = true;

                filter = new SQLRequestFilter(dgv, new string[] { "Примечание" });
                filter.Dock = DockStyle.Left;
                filter.OnFilterChanged += (se, ar) => dataBase.ToDisplay("SSALoading WHERE [ВремяВкл] > '" + dtp.Value + "' AND [ВремяВкл] < '" + dtp.Value.AddDays(1) + "'" + filter.CurrentFilter, dataTableName: "SSALoading");
                Controls.Add(filter);
            };
            dgv.SelectionChanged += UpdateBottomControls;
            Enter += (s, e) => 
            {
                if(firstBind)
                    dataBase.ToDisplay("SSALoading WHERE [ВремяВкл] > '" + dtp.Value + "' AND [ВремяВкл] < '" + dtp.Value.AddDays(1) + "'" + filter.CurrentFilter, dataTableName: "SSALoading", onlyAdapter: true);
            };

            dgv.UserDeletedRow += (sender, e) => dataBase.sqlAdapter.Update(dataBase.dataset.Tables["SSALoading"]);
            dgv.CellMouseClick += (sender, e) =>
            {
                if (dgv != null)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        Supports.MenuPanel(dgv, e.RowIndex, e.ColumnIndex, typeof(SSALoading));
                    }
                    if (e.Button == MouseButtons.Left)
                    {
                    }
                }
            };
            dgv.MouseMove += (s, e) =>
            {
                if (dgv.ContextMenuStrip != null)
                    dgv.ContextMenuStrip.Dispose();
            };

            dgv.KeyUp += (se, e) =>
            {
                if (e.KeyData == Keys.Left)
                    dtp.Value = dtp.Value.AddDays(-1);
                if (e.KeyData == Keys.Right)
                    dtp.Value = dtp.Value.AddDays(1);
                if (e.KeyData == Keys.Tab || e.KeyData == Keys.Enter)
                    e.Handled = true;
            };
            subtractTimeButton.Click += (sender, e) => dtp.Value = dtp.Value.AddDays(-1);
            addTimeButton.Click += (sender, e) => dtp.Value = dtp.Value.AddDays(1);
            dtp.ValueChanged += (se, e) =>
            {
                dataBase.ToDisplay("SSALoading WHERE [ВремяВкл] > '" + dtp.Value + "' AND [ВремяВкл] < '" + dtp.Value.AddDays(1) + "'" + filter.CurrentFilter, dataTableName: "SSALoading");
                filter.RefreshNodes(dgv);
            };

            editButton.Click += (s, e) =>
            {
                if (MessageBoxTi.Show("Редактировать?", "Редактирование излучения") == MessageResult.Yes && dgv.RowCount != 0)
                {
                    bool red = false;

                    if (!dgv.SelectedRows[0].Cells["Спутник"].Value.Equals(Спутник.SelectedItem))
                    {
                        dataBase.dataset.Tables["SSALoading"].Select().Where(x => x["ID"].Equals(dgv.SelectedRows[0].Cells["ID"].Value)).FirstOrDefault()["Спутник"] = Спутник.SelectedItem;
                        red = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["Диапазон"].Value.Equals(Диапазон.SelectedItem))
                    {
                        dataBase.dataset.Tables["SSALoading"].Select().Where(x => x["ID"].Equals(dgv.SelectedRows[0].Cells["ID"].Value)).FirstOrDefault()["Диапазон"] = Диапазон.SelectedItem;
                        red = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["Поляризация"].Value.Equals(Поляризация.SelectedItem))
                    {
                        dataBase.dataset.Tables["SSALoading"].Select().Where(x => x["ID"].Equals(dgv.SelectedRows[0].Cells["ID"].Value)).FirstOrDefault()["Поляризация"] = Поляризация.SelectedItem;
                        red = true;
                    }

                    if (!Convert.ToDecimal(dgv.SelectedRows[0].Cells["Частота"].Value).Equals(Частота.Value))
                    {
                        dataBase.dataset.Tables["SSALoading"].Select().Where(x => x["ID"].Equals(dgv.SelectedRows[0].Cells["ID"].Value)).FirstOrDefault()["Частота"] = Частота.Value;
                        red = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["ВремяВкл"].Value.Equals(ВремяВкл.Value))
                    {
                        dataBase.dataset.Tables["SSALoading"].Select().Where(x => x["ID"].Equals(dgv.SelectedRows[0].Cells["ID"].Value)).FirstOrDefault()["ВремяВкл"] = ВремяВкл.Value;
                        red = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["ВидДоступа"].Value.Equals(ВидДоступа.SelectedItem))
                    {
                        dataBase.dataset.Tables["SSALoading"].Select().Where(x => x["ID"].Equals(dgv.SelectedRows[0].Cells["ID"].Value)).FirstOrDefault()["ВидДоступа"] = ВидДоступа.SelectedItem;
                        red = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["Модуляция"].Value.Equals(Модуляция.SelectedItem))
                    {
                        dataBase.dataset.Tables["SSALoading"].Select().Where(x => x["ID"].Equals(dgv.SelectedRows[0].Cells["ID"].Value)).FirstOrDefault()["Модуляция"] = Модуляция.SelectedItem;
                        red = true;
                    }

                    if (!Convert.ToDecimal(dgv.SelectedRows[0].Cells["Тактовая"].Value).Equals(Тактовая.Value))
                    {
                        dataBase.dataset.Tables["SSALoading"].Select().Where(x => x["ID"].Equals(dgv.SelectedRows[0].Cells["ID"].Value)).FirstOrDefault()["Тактовая"] = Тактовая.Value;
                        red = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["RПУК"].Value.Equals(RПУК.SelectedItem))
                    {
                        dataBase.dataset.Tables["SSALoading"].Select().Where(x => x["ID"].Equals(dgv.SelectedRows[0].Cells["ID"].Value)).FirstOrDefault()["RПУК"] = RПУК.SelectedItem;
                        red = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["ПУК"].Value.Equals(ПУК.SelectedItem))
                    {
                        dataBase.dataset.Tables["SSALoading"].Select().Where(x => x["ID"].Equals(dgv.SelectedRows[0].Cells["ID"].Value)).FirstOrDefault()["ПУК"] = ПУК.SelectedItem;
                        red = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["Скремб"].Value.Equals(Скремб.SelectedItem))
                    {
                        dataBase.dataset.Tables["SSALoading"].Select().Where(x => x["ID"].Equals(dgv.SelectedRows[0].Cells["ID"].Value)).FirstOrDefault()["Скремб"] = Скремб.SelectedItem;
                        red = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["СистемаСвязи"].Value.Equals(СистемаСвязи.SelectedItem))
                    {
                        dataBase.dataset.Tables["SSALoading"].Select().Where(x => x["ID"].Equals(dgv.SelectedRows[0].Cells["ID"].Value)).FirstOrDefault()["СистемаСвязи"] = СистемаСвязи.SelectedItem;
                        red = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["Протоколы"].Value.Equals(Протоколы.SelectedItem))
                    {
                        dataBase.dataset.Tables["SSALoading"].Select().Where(x => x["ID"].Equals(dgv.SelectedRows[0].Cells["ID"].Value)).FirstOrDefault()["Протоколы"] = Протоколы.SelectedItem;
                        red = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["ХарактерИнфо"].Value.Equals(ХарактерИнфо.SelectedItem))
                    {
                        dataBase.dataset.Tables["SSALoading"].Select().Where(x => x["ID"].Equals(dgv.SelectedRows[0].Cells["ID"].Value)).FirstOrDefault()["ХарактерИнфо"] = ХарактерИнфо.SelectedItem;
                        red = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["Принадлежность"].Value.Equals(Принадлежность.SelectedItem))
                    {
                        dataBase.dataset.Tables["SSALoading"].Select().Where(x => x["ID"].Equals(dgv.SelectedRows[0].Cells["ID"].Value)).FirstOrDefault()["Принадлежность"] = Принадлежность.SelectedItem;
                        red = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["ВидИсточника"].Value.Equals(ВидИсточника.SelectedItem))
                    {
                        dataBase.dataset.Tables["SSALoading"].Select().Where(x => x["ID"].Equals(dgv.SelectedRows[0].Cells["ID"].Value)).FirstOrDefault()["ВидИсточника"] = ВидИсточника.SelectedItem;
                        red = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["ВидОбъекта"].Value.Equals(ВидОбъекта.SelectedItem))
                    {
                        dataBase.dataset.Tables["SSALoading"].Select().Where(x => x["ID"].Equals(dgv.SelectedRows[0].Cells["ID"].Value)).FirstOrDefault()["ВидОбъекта"] = ВидОбъекта.SelectedItem;
                        red = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["ХарРаботы"].Value.Equals(ХарРаботы.SelectedItem))
                    {
                        dataBase.dataset.Tables["SSALoading"].Select().Where(x => x["ID"].Equals(dgv.SelectedRows[0].Cells["ID"].Value)).FirstOrDefault()["ХарРаботы"] = ХарРаботы.SelectedItem;
                        red = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["ВозмСобытие"].Value.Equals(ВозмСобытие.SelectedItem))
                    {
                        dataBase.dataset.Tables["SSALoading"].Select().Where(x => x["ID"].Equals(dgv.SelectedRows[0].Cells["ID"].Value)).FirstOrDefault()["ВозмСобытие"] = ВозмСобытие.SelectedItem;
                        red = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["СостДеятельности"].Value.Equals(СостДеятельности.SelectedItem))
                    {
                        dataBase.dataset.Tables["SSALoading"].Select().Where(x => x["ID"].Equals(dgv.SelectedRows[0].Cells["ID"].Value)).FirstOrDefault()["СостДеятельности"] = СостДеятельности.SelectedItem;
                        red = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["Примечание"].Value.ToString().Equals(Примечание.Text))
                    {
                        dataBase.dataset.Tables["SSALoading"].Select().Where(x => x["ID"].Equals(dgv.SelectedRows[0].Cells["ID"].Value)).FirstOrDefault()["Примечание"] = Примечание.Text;
                        red = true;
                    }




                    if (red == false)
                        return;

                    dataBase.dataset.Tables["SSALoading"].Select().Where(x => x["ID"].Equals(dgv.SelectedRows[0].Cells["ID"].Value)).FirstOrDefault()["Пользователь"] = Profile.userName;

                    dataBase.sqlAdapter.Update(dataBase.dataset.Tables["SSALoading"]);
                }
            };
            addButton.Click += (s, e) =>
            {
                if (MessageBoxTi.Show("Добавить?", "Добавление излучения") == MessageResult.Yes && dgv.RowCount != 0)
                {
                    DataRow p = (dgv.DataSource as DataTable).NewRow();

                    p["Спутник"] = Supports.NullOrObjectIfEmpty(Спутник.SelectedItem);
                    p["Диапазон"] = Supports.NullOrObjectIfEmpty(Диапазон.SelectedItem);
                    p["Поляризация"] = Supports.NullOrObjectIfEmpty(Поляризация.SelectedItem);                 
                    p["Частота"] = Частота.Value;
                    p["ВремяВкл"] = ВремяВкл.Value;
                    p["Состояние"] =  MessageBoxTi.Show("Состояние", "Излучение включилось или выключилось", new string[] { "Включилось", "Выключилось" }) == MessageResult.Btn0 ? true : false;
                    p["ВидДоступа"] = Supports.NullOrObjectIfEmpty(ВидДоступа.SelectedItem);
                    p["Модуляция"] = Supports.NullOrObjectIfEmpty(Модуляция.SelectedItem);
                    p["Тактовая"] = Тактовая.Value;
                    p["RПУК"] = Supports.NullOrObjectIfEmpty(RПУК.SelectedItem);
                    p["ПУК"] = Supports.NullOrObjectIfEmpty(ПУК.SelectedItem);
                    p["Скремб"] = Supports.NullOrObjectIfEmpty(Скремб.SelectedItem);
                    p["СистемаСвязи"] = Supports.NullOrObjectIfEmpty(СистемаСвязи.SelectedItem);
                    p["Протоколы"] = Supports.NullOrObjectIfEmpty(Протоколы.SelectedItem);
                    p["ХарактерИнфо"] = Supports.NullOrObjectIfEmpty(ХарактерИнфо.SelectedItem);
                    p["Принадлежность"] = Supports.NullOrObjectIfEmpty(Принадлежность.SelectedItem);
                    p["ВидИсточника"] = Supports.NullOrObjectIfEmpty(ВидИсточника.SelectedItem);
                    p["ВидОбъекта"] = Supports.NullOrObjectIfEmpty(ВидОбъекта.SelectedItem);
                    p["ХарРаботы"] = Supports.NullOrObjectIfEmpty(ХарРаботы.SelectedItem);
                    p["ВозмСобытие"] = Supports.NullOrObjectIfEmpty(ВозмСобытие.SelectedItem);
                    p["СостДеятельности"] = Supports.NullOrObjectIfEmpty(СостДеятельности.SelectedItem);
                    p["Примечание"] = Supports.NullOrObjectIfEmpty(Примечание.Text);
                    p["Пользователь"] = Profile.userName;

                    decimal fer = Частота.Value;
                    dataBase.dataset.Tables["SSALoading"].Rows.Add(p);
                    dataBase.sqlAdapter.Update(dataBase.dataset.Tables["SSALoading"]);
                    dataBase.ToDisplay("SSALoading WHERE [ВремяВкл] > '" + dtp.Value + "' AND [ВремяВкл] < '" + dtp.Value.AddDays(1) + "'" + filter.CurrentFilter, dataTableName: "SSALoading");

                    dgv.CurrentCell = dgv.Rows.Cast<DataGridViewRow>().Where(x => Convert.ToDecimal(x.Cells["Частота"].Value).Equals(fer) && x.Cells["Пользователь"].Value.ToString().Equals(Profile.userName)).FirstOrDefault().Cells["Частота"];
                }
            };

            dataBase.ToDisplay("SSALoading WHERE [ВремяВкл] > '" + dtp.Value + "' AND [ВремяВкл] < '" + dtp.Value.AddDays(1) + "'", dataTableName: "SSALoading");
            dgv.DataSource = dataBase.dataset.Tables["SSALoading"];
        }

        private void SSALoadingControls()
        {
            Controls.Add(new Panel()
            {
                Name = "SSALoadingPanel",
                Dock = DockStyle.Fill,
            });

            (Controls.Find("SSALoadingPanel", true).FirstOrDefault() as Panel).Controls.Add(dgv);

            Controls.Add(new Splitter()
            {
                Dock = DockStyle.Left,
                Name = "filterSplitter",
            });


            Controls.Add(new Panel()
            {
                Name = "SSALoadingPanel0",
                Height = 240,
                Width = 969,
                Dock = DockStyle.Bottom,
            });

            (Controls.Find("SSALoadingPanel0", true).FirstOrDefault() as Panel).Controls.Add(new TableLayoutPanel()
            {
                Name = "SSALoadingTableLayoutPanel1",
                Anchor = AnchorStyles.None,
                Height = 35,
                Width = 969,
                Location = new Point(((Controls.Find("SSALoadingPanel0", true).FirstOrDefault() as Panel).Width / 2) - (969 / 2), 0),
                BackColor = Supports.headGrey,
            });

            (Controls.Find("SSALoadingTableLayoutPanel1", true).FirstOrDefault() as TableLayoutPanel).RowStyles.Clear();
            (Controls.Find("SSALoadingTableLayoutPanel1", true).FirstOrDefault() as TableLayoutPanel).ColumnStyles.Clear();
            (Controls.Find("SSALoadingTableLayoutPanel1", true).FirstOrDefault() as TableLayoutPanel).ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
            (Controls.Find("SSALoadingTableLayoutPanel1", true).FirstOrDefault() as TableLayoutPanel).ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            (Controls.Find("SSALoadingTableLayoutPanel1", true).FirstOrDefault() as TableLayoutPanel).ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));

            (Controls.Find("SSALoadingTableLayoutPanel1", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(subtractTimeButton, 0, 0);

            (Controls.Find("SSALoadingTableLayoutPanel1", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(dtp, 1, 0);

            (Controls.Find("SSALoadingTableLayoutPanel1", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(addTimeButton, 2, 0);

            (Controls.Find("SSALoadingPanel0", true).FirstOrDefault() as Panel).Controls.Add(new FlowLayoutPanel()
            {
                Name = "SSALoadingFlowLayoutPanel1",
                Anchor = AnchorStyles.None,
                Height = 205,
                Width = 969,
                Location = new Point(((Controls.Find("SSALoadingPanel0", true).FirstOrDefault() as Panel).Width / 2) - (969 / 2), 35),
                BackColor = Supports.headGrey,
            });

            (Controls.Find("SSALoadingFlowLayoutPanel1", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new FlowLayoutPanel()
            {
                Name = "SSALoadingFlowLayoutPanel2",
                Height = 250,
                Width = 614,
                BackColor = Supports.headGrey,
                ForeColor = Supports.textWhite,


            });

            (Controls.Find("SSALoadingFlowLayoutPanel1", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new FlowLayoutPanel()
            {
                Name = "SSALoadingFlowLayoutPanel3",
                Height = 250,
                Width = 342,
            });

            (Controls.Find("SSALoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "SSALoadingNameGroupBox",
                Width = 130,
                Height = 40,
                Text = "Спутник:",
            });

            (Controls.Find("SSALoadingNameGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(Спутник);

            (Controls.Find("SSALoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "SSALoadingBeamGroupBox",
                Width = 70,
                Height = 40,
                Text = "Диапазон:",
            });

            (Controls.Find("SSALoadingBeamGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(Диапазон);

            (Controls.Find("SSALoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "SSALoadingPolarizationGroupBox",
                Width = 70,
                Height = 40,
                Text = "Поляризация:",
            });

            (Controls.Find("SSALoadingPolarizationGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(Поляризация);

            (Controls.Find("SSALoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "SSALoadingFrequencyGroupBox",
                Width = 110,
                Height = 40,
                Text = "Частота (Гц):",
            });

            (Controls.Find("SSALoadingFrequencyGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(Частота);

            (Controls.Find("SSALoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "SSALoadingBodGroupBox",
                Width = 97,
                Height = 40,
                Text = "Тактовая (кБод):",
            });

            (Controls.Find("SSALoadingBodGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(Тактовая);

            (Controls.Find("SSALoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "SSALoadingModulationGroupBox",
                Width = 100,
                Height = 40,
                Text = "Модуляция:",
            });

            (Controls.Find("SSALoadingModulationGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(Модуляция);

            (Controls.Find("SSALoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "SSALoadingAccessTypeGroupBox",
                Width = 80,
                Height = 40,
                Text = "Вид доступа:",
            });

            (Controls.Find("SSALoadingAccessTypeGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(ВидДоступа);

            (Controls.Find("SSALoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "SSALoadingConnectionSystemGroupBox",
                Width = 125,
                Height = 40,
                Text = "Система связи:",
            });

            (Controls.Find("SSALoadingConnectionSystemGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(СистемаСвязи);

            (Controls.Find("SSALoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "SSALoadingCodingGroupBox",
                Width = 100,
                Height = 40,
                Text = "Кодирование:",
            });

            (Controls.Find("SSALoadingCodingGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(ПУК);

            (Controls.Find("SSALoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "SSALoadingRGroupBox",
                Width = 60,
                Height = 40,
                Text = "R:",
            });

            (Controls.Find("SSALoadingRGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(RПУК);

            (Controls.Find("SSALoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "SSALoadingScrembler1GroupBox",
                Width = 130,
                Height = 40,
                Text = "Скремблер:",
            });

            (Controls.Find("SSALoadingScrembler1GroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(Скремб);

            (Controls.Find("SSALoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "SSALoadingVinfGroupBox",
                Width = 80,
                Height = 40,
                Text = "Vinf(кБит/с):",
            });

            (Controls.Find("SSALoadingVinfGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(Скорость);



            (Controls.Find("SSALoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "SSALoadingAdditionGroupBox",
                Width = 300,
                Height = 100,
                Text = "Примечание:",
                BackColor = Supports.headGrey,
                ForeColor = Supports.textWhite,
            });

            (Controls.Find("SSALoadingAdditionGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(Примечание);

            (Controls.Find("SSALoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new FlowLayoutPanel()
            {
                Name = "SSALoadingFlowLayoutPanel6",
                Width = 300,
                Height = 100,
            });


            (Controls.Find("SSALoadingFlowLayoutPanel6", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "SSALoadingWorkCharacterGroupBox",
                Width = 300,
                Height = 40,
                Text = "Характеристика работы:",
            });

            (Controls.Find("SSALoadingWorkCharacterGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(ХарРаботы);

            (Controls.Find("SSALoadingFlowLayoutPanel6", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new Panel()
            {
                Name = "SSALoadingPanel6",
                Height = 50,
                Width = 184,
                Dock = DockStyle.Bottom
            });


            (Controls.Find("SSALoadingPanel6", true).FirstOrDefault() as Panel).Controls.Add(ВремяВкл);

            ВремяВкл.Format = DateTimePickerFormat.Custom;
            ВремяВкл.CustomFormat = "dd.MM.yyyy hh:mm:ss";

            (Controls.Find("SSALoadingFlowLayoutPanel6", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(addButton);

            (Controls.Find("SSALoadingFlowLayoutPanel6", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(editButton);

            new ToolTip().SetToolTip(addButton, "Добавить излучение");
            new ToolTip().SetToolTip(editButton, "Редактировать излучение");


            (Controls.Find("SSALoadingFlowLayoutPanel3", true).FirstOrDefault() as Panel).Controls.Add(new GroupBox()
            {
                Name = "SSALoadingSSAInfoCharacterGroupBox",
                Width = 165,
                Height = 44,
                Text = "Характер информации:",
            });

            (Controls.Find("SSALoadingSSAInfoCharacterGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(ХарактерИнфо);

            (Controls.Find("SSALoadingFlowLayoutPanel3", true).FirstOrDefault() as Panel).Controls.Add(new GroupBox()
            {
                Name = "SSALoadingSSAPossibleEventGroupBox",
                Width = 165,
                Height = 44,
                Text = "Возможное событие(признак):",
            });

            (Controls.Find("SSALoadingSSAPossibleEventGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(ВозмСобытие);

            (Controls.Find("SSALoadingFlowLayoutPanel3", true).FirstOrDefault() as Panel).Controls.Add(new GroupBox()
            {
                Name = "SSALoadingSSAMembershipGroupBox",
                Width = 165,
                Height = 44,
                Text = "Принадлежность:",
            });

            (Controls.Find("SSALoadingSSAMembershipGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(Принадлежность);

            (Controls.Find("SSALoadingFlowLayoutPanel3", true).FirstOrDefault() as Panel).Controls.Add(new GroupBox()
            {
                Name = "SSALoadingSSAActionConditionGroupBox",
                Width = 165,
                Height = 44,
                Text = "Состояние деятельности:",
            });

            (Controls.Find("SSALoadingSSAActionConditionGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(СостДеятельности);

            (Controls.Find("SSALoadingFlowLayoutPanel3", true).FirstOrDefault() as Panel).Controls.Add(new GroupBox()
            {
                Name = "SSALoadingSSASourseViewGroupBox",
                Width = 165,
                Height = 45,
                Text = "Вид источника:",
            });

            (Controls.Find("SSALoadingSSASourseViewGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(ВидИсточника);

            (Controls.Find("SSALoadingFlowLayoutPanel3", true).FirstOrDefault() as Panel).Controls.Add(new GroupBox()
            {
                Name = "SSALoadingSSAObjectViewGroupBox",
                Width = 165,
                Height = 45,
                Text = "Вид объекта:",
            });

            (Controls.Find("SSALoadingSSAObjectViewGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(ВидОбъекта);

            (Controls.Find("SSALoadingFlowLayoutPanel3", true).FirstOrDefault() as Panel).Controls.Add(new GroupBox()
            {
                Name = "SSALoadingSSAProtocolStackGroupBox",
                Width = 336,
                Height = 44,
                Text = "Стек протоколов:",
            });

            (Controls.Find("SSALoadingSSAProtocolStackGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(Протоколы);
        }

        private void UpdateBottomControls(object sender, EventArgs e)
        {
            try
            {
                Спутник.Text = dgv.SelectedRows[0].Cells["Спутник"].Value.ToString();
                Диапазон.Text = dgv.SelectedRows[0].Cells["Диапазон"].Value.ToString();
                Поляризация.Text = dgv.SelectedRows[0].Cells["Поляризация"].Value.ToString();
                Частота.Value = Convert.ToDecimal(dgv.SelectedRows[0].Cells["Частота"].Value);
                Тактовая.Value = Convert.ToDecimal(dgv.SelectedRows[0].Cells["Тактовая"].Value);
                Примечание.Text = dgv.SelectedRows[0].Cells["Примечание"].Value.ToString();
                Модуляция.Text = dgv.SelectedRows[0].Cells["Модуляция"].Value.ToString();
                RПУК.Text = dgv.SelectedRows[0].Cells["RПУК"].Value.ToString();
                ПУК.Text = dgv.SelectedRows[0].Cells["ПУК"].Value.ToString();
                ВидДоступа.Text = dgv.SelectedRows[0].Cells["ВидДоступа"].Value.ToString();
                СистемаСвязи.Text = dgv.SelectedRows[0].Cells["СистемаСвязи"].Value.ToString();
                Скремб.Text = dgv.SelectedRows[0].Cells["Скремб"].Value.ToString();
                ХарРаботы.Text = dgv.SelectedRows[0].Cells["ХарРаботы"].Value.ToString();
                ХарактерИнфо.Text = dgv.SelectedRows[0].Cells["ХарактерИнфо"].Value.ToString();
                ВозмСобытие.Text = dgv.SelectedRows[0].Cells["ВозмСобытие"].Value.ToString();
                Принадлежность.Text = dgv.SelectedRows[0].Cells["Принадлежность"].Value.ToString();
                СостДеятельности.Text = dgv.SelectedRows[0].Cells["СостДеятельности"].Value.ToString();
                ВидИсточника.Text = dgv.SelectedRows[0].Cells["ВидИсточника"].Value.ToString();
                ВидОбъекта.Text = dgv.SelectedRows[0].Cells["ВидОбъекта"].Value.ToString();
                Протоколы.Text = dgv.SelectedRows[0].Cells["Протоколы"].Value.ToString();
                Скорость.Text = dgv.SelectedRows[0].Cells["Скорость"].Value.ToString();
                ВремяВкл.Value = (DateTime)dgv.SelectedRows[0].Cells["ВремяВкл"].Value;
            }
            catch (Exception)
            {
                return;
            }


        }

        private void RefreshDGV()
        {
            foreach (DataGridViewRow data in dgv.Rows)
            {
                if ((bool)data.Cells["Состояние"].Value == true)
                {
                    data.Cells["Частота"].Style.BackColor = Supports.Green;
                    data.Cells["Частота"].Style.ForeColor = Supports.textWhite;
                    data.Cells["Частота"].Style.SelectionBackColor = Color.DarkGreen;
                    data.Cells["Частота"].Style.SelectionForeColor = Supports.textWhite;

                    data.Cells["Спутник"].Style.BackColor = Supports.Green;
                    data.Cells["Спутник"].Style.ForeColor = Supports.textWhite;
                    data.Cells["Спутник"].Style.SelectionBackColor = Color.DarkGreen;
                    data.Cells["Спутник"].Style.SelectionForeColor = Supports.textWhite;

                    data.Cells["Диапазон"].Style.BackColor = Supports.Green;
                    data.Cells["Диапазон"].Style.ForeColor = Supports.textWhite;
                    data.Cells["Диапазон"].Style.SelectionBackColor = Color.DarkGreen;
                    data.Cells["Диапазон"].Style.SelectionForeColor = Supports.textWhite;

                    data.Cells["Поляризация"].Style.BackColor = Supports.Green;
                    data.Cells["Поляризация"].Style.ForeColor = Supports.textWhite;
                    data.Cells["Поляризация"].Style.SelectionBackColor = Color.DarkGreen;
                    data.Cells["Поляризация"].Style.SelectionForeColor = Supports.textWhite;
                }
                else
                {
                    data.Cells["Частота"].Style.BackColor = Supports.Red;
                    data.Cells["Частота"].Style.SelectionBackColor = Color.DarkRed;
                    data.Cells["Частота"].Style.SelectionForeColor = Supports.textWhite;
                    data.Cells["Частота"].Style.ForeColor = Supports.textWhite;

                    data.Cells["Спутник"].Style.BackColor = Supports.Red;
                    data.Cells["Спутник"].Style.SelectionBackColor = Color.DarkRed;
                    data.Cells["Спутник"].Style.SelectionForeColor = Supports.textWhite;
                    data.Cells["Спутник"].Style.ForeColor = Supports.textWhite;

                    data.Cells["Диапазон"].Style.BackColor = Supports.Red;
                    data.Cells["Диапазон"].Style.SelectionBackColor = Color.DarkRed;
                    data.Cells["Диапазон"].Style.SelectionForeColor = Supports.textWhite;
                    data.Cells["Диапазон"].Style.ForeColor = Supports.textWhite;

                    data.Cells["Поляризация"].Style.BackColor = Supports.Red;
                    data.Cells["Поляризация"].Style.SelectionBackColor = Color.DarkRed;
                    data.Cells["Поляризация"].Style.SelectionForeColor = Supports.textWhite;
                    data.Cells["Поляризация"].Style.ForeColor = Supports.textWhite;
                }
            }
        }

        private void FillBottomControls()
        {
            DataTable SSALoading = dataBase.SimpleData("SSALoading");

            Спутник.Items.Clear();
            foreach (String st in SSALoading.Rows.Cast<DataRow>().Distinct().Where(x => x["Спутник"].ToString() != "").Select(x => x["Спутник"].ToString()).OrderBy(x => x).Distinct().ToList())
                Спутник.Items.Add(st);

            Диапазон.Items.Clear();
            foreach (String st in SSALoading.Rows.Cast<DataRow>().Distinct().Where(x => x["Диапазон"].ToString() != "").Select(x => x["Диапазон"].ToString()).OrderBy(x => x).Distinct().ToList())
                Диапазон.Items.Add(st);

            Поляризация.Items.Clear();
            foreach (String st in SSALoading.Rows.Cast<DataRow>().Distinct().Where(x => x["Поляризация"].ToString() != "").Select(x => x["Поляризация"].ToString()).OrderBy(x => x).Distinct().ToList())
                Поляризация.Items.Add(st);

            Модуляция.Items.Clear();
            foreach (String st in SSALoading.Rows.Cast<DataRow>().Distinct().Where(x => x["Модуляция"].ToString() != "").Select(x => x["Модуляция"].ToString()).OrderBy(x => x).Distinct().ToList())
                Модуляция.Items.Add(st);

            ВидДоступа.Items.Clear();
            foreach (String st in SSALoading.Rows.Cast<DataRow>().Distinct().Where(x => x["ВидДоступа"].ToString() != "").Select(x => x["ВидДоступа"].ToString()).OrderBy(x => x).OrderBy(x => x).Distinct().ToList())
                ВидДоступа.Items.Add(st);

            СистемаСвязи.Items.Clear();
            foreach (String st in SSALoading.Rows.Cast<DataRow>().Distinct().Where(x => x["СистемаСвязи"].ToString() != "").Select(x => x["СистемаСвязи"].ToString()).OrderBy(x => x).Distinct().ToList())
                СистемаСвязи.Items.Add(st);

            ПУК.Items.Clear();
            foreach (String st in SSALoading.Rows.Cast<DataRow>().Distinct().Where(x => x["ПУК"].ToString() != "").Select(x => x["ПУК"].ToString()).OrderBy(x => x).Distinct().ToList())
                ПУК.Items.Add(st);

            RПУК.Items.Clear();
            foreach (String st in SSALoading.Rows.Cast<DataRow>().Distinct().Where(x => x["RПУК"].ToString() != "").Select(x => x["RПУК"].ToString()).OrderBy(x => x).Distinct().ToList())
                RПУК.Items.Add(st);

            Скремб.Items.Clear();
            foreach (String st in SSALoading.Rows.Cast<DataRow>().Distinct().Where(x => x["Скремб"].ToString() != "").Select(x => x["Скремб"].ToString()).OrderBy(x => x).Distinct().ToList())
                Скремб.Items.Add(st);

            ХарРаботы.Items.Clear();
            foreach (String st in SSALoading.Rows.Cast<DataRow>().Distinct().Where(x => x["ХарРаботы"].ToString() != "").Select(x => x["ХарРаботы"].ToString()).OrderBy(x => x).Distinct().ToList())
                ХарРаботы.Items.Add(st);

            ХарактерИнфо.Items.Clear();
            foreach (String st in SSALoading.Rows.Cast<DataRow>().Distinct().Where(x => x["ХарактерИнфо"].ToString() != "").Select(x => x["ХарактерИнфо"].ToString()).OrderBy(x => x).Distinct().ToList())
                ХарактерИнфо.Items.Add(st);

            ВозмСобытие.Items.Clear();
            foreach (String st in SSALoading.Rows.Cast<DataRow>().Distinct().Where(x => x["ВозмСобытие"].ToString() != "").Select(x => x["ВозмСобытие"].ToString()).OrderBy(x => x).Distinct().ToList())
                ВозмСобытие.Items.Add(st);

            Принадлежность.Items.Clear();
            foreach (String st in SSALoading.Rows.Cast<DataRow>().Distinct().Where(x => x["Принадлежность"].ToString() != "").Select(x => x["Принадлежность"].ToString()).OrderBy(x => x).Distinct().ToList())
                Принадлежность.Items.Add(st);

            СостДеятельности.Items.Clear();
            foreach (String st in SSALoading.Rows.Cast<DataRow>().Distinct().Where(x => x["СостДеятельности"].ToString() != "").Select(x => x["СостДеятельности"].ToString()).OrderBy(x => x).Distinct().ToList())
                СостДеятельности.Items.Add(st);

            ВидИсточника.Items.Clear();
            foreach (String st in SSALoading.Rows.Cast<DataRow>().Distinct().Where(x => x["ВидИсточника"].ToString() != "").Select(x => x["ВидИсточника"].ToString()).OrderBy(x => x).Distinct().ToList())
                ВидИсточника.Items.Add(st);

            ВидОбъекта.Items.Clear();
            foreach (String st in SSALoading.Rows.Cast<DataRow>().Distinct().Where(x => x["ВидОбъекта"].ToString() != "").Select(x => x["ВидОбъекта"].ToString()).OrderBy(x => x).Distinct().ToList())
                ВидОбъекта.Items.Add(st);

            Протоколы.Items.Clear();
            foreach (String st in SSALoading.Rows.Cast<DataRow>().Distinct().Where(x => x["Протоколы"].ToString() != "").Select(x => x["Протоколы"].ToString()).OrderBy(x => x).Distinct().ToList())
                Протоколы.Items.Add(st);

        }

        private void AddFreeSpase(Control control)
        {
            foreach (Control gro in control.Controls)
            {
                if (gro.GetType() == typeof(ComboBox))
                {
                    ((ComboBox)gro).Items.Add(DBNull.Value);
                    ((ComboBox)gro).MouseClick += (s, e) =>
                    {
                        if (e.Button == MouseButtons.Right)
                        {
                            ((ComboBox)gro).SelectedItem = DBNull.Value;
                        }
                    };
                }

                AddFreeSpase(gro);
            }
        }
    }
}