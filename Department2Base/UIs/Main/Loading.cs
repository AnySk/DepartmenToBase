using Department2Base.Properties;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Department2Base
{
    public class Loading : TabPage
    {
        #region Контролы и глобальные переменные
        public int checkSum = 0;
        public string keys = null;

        private bool firstBind = false;


        public string nameISZ = null;
        public string bandISZ = null;
        public string polarizationISZ = null;

        private SQLRequestFilter filter = null;
        public Panel mainPanel = new Panel()
        {
            Name = "LoadingPanel0",
            Height = 175,
            Dock = DockStyle.Bottom,
        };
        private DataGridView dgv = new DataGridView()
        {
            BackgroundColor = Supports.headGrey,
            Dock = DockStyle.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            EditMode = DataGridViewEditMode.EditProgrammatically,
            AllowUserToOrderColumns = false,
            ScrollBars = ScrollBars.Both,
            GridColor = Supports.headBlue,
            EnableHeadersVisualStyles = false,
            AllowUserToAddRows = false,
            RowHeadersVisible = false,
            AllowUserToResizeRows = false,
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
        };
        private NumericUpDown Частота = new NumericUpDown()
        {
            Dock = DockStyle.Fill,
            BackColor = Supports.textGray,
            ForeColor = Supports.textWhite,
            BorderStyle = BorderStyle.FixedSingle,
            Maximum = 10000000000,
            Increment = 100,
            ThousandsSeparator = true,
        };
        private NumericUpDown Тактовая = new NumericUpDown()
        {
            Dock = DockStyle.Fill,
            BackColor = Supports.textGray,
            ForeColor = Supports.textWhite,
            BorderStyle = BorderStyle.FixedSingle,
            Maximum = 10000000000,
            Increment = 10,
            ThousandsSeparator = true,
            DecimalPlaces = 2
        };
        private TextBox ОтношениеСШ = new TextBox() { Dock = DockStyle.Fill, };
        private TextBox Скорость = new TextBox()
        {
            Enabled = false,
            Dock = DockStyle.Fill,
        };
        private TextBox Примечание = new TextBox()
        {
            Multiline = true,
            Dock = DockStyle.Fill,
        };
        private TextBox ДлинаКадра = new TextBox() { Dock = DockStyle.Fill, };
        private TextBox СтекПрот = new TextBox() { Dock = DockStyle.Fill, };
        private ComboBox Загруженность = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        private ComboBox СостояниеАн = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        private ComboBox Наблюдение = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        private ComboBox Ценность = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        private ComboBox СистемаСвязи = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        private ComboBox ПУККаскад = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        private ComboBox СкрембВнутр = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        private ComboBox СкрембВнеш = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        private ComboBox RПУК = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        private ComboBox ПУК = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        private ComboBox ВидДоступа = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        private ComboBox Модуляция = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        private ComboBox ТипДанных = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        private ComboBox Оборудование = new ComboBox()
        {
            Dock = DockStyle.Fill,
        };
        private Button onOff = new Button()
        {
            Height = 55,
            Width = 46,
            Image = Resources.onoff,

        };
        private Button addButton = new Button()
        {
            Height = 55,
            Width = 46,
            Image = Resources.add1,
        };
        private Button redactButton = new Button()
        {
            Height = 55,
            Width = 46,
            Image = Resources.edit1,
        };
        #endregion

        public Loading(string name, string band, string polarization)
        {
            nameISZ = name;
            bandISZ = band;
            polarizationISZ = polarization;

            keys = "WHERE Спутник = \'" + nameISZ + "\' AND Диапазон = \'" + bandISZ + "\' AND Поляризация = \'" + polarizationISZ + "\'";
            checkSum = dataBase.CheckSum("Loading " + keys);
            Name = keys;
            Text = "Спутник:\"" + nameISZ + "\" Диапазон:\"" + bandISZ + "\" Поляризация:\"" + polarizationISZ + "\"";
            LoadingControls(keys);
            FillBottomControls();
            Supports.GangeGroup(this);
            AddFreeSpase(mainPanel);

            dataBase.ToDisplay("Loading " + keys + " ORDER BY [Частота]", dataTableName: "Loading " + keys);
            dgv.DataSource = dataBase.dataset.Tables["Loading " + keys];

            dgv.DataBindingComplete += (s, e) =>
            {
                RefreshDGV();
                Profile.DownSign = "Излучений: " + dgv.RowCount + " Вкл: " + dgv.Rows.Cast<DataGridViewRow>().Where(x => (bool)x.Cells["Состояние"].Value == true).Count() + " Выкл.: " + dgv.Rows.Cast<DataGridViewRow>().Where(x => (bool)x.Cells["Состояние"].Value == false || x.Cells["Состояние"].Value.ToString() == "").Count();

                if (firstBind)
                    return;
                firstBind = true;

                dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

                dgv.Columns["ОтношениеСШ"].HeaderText = "ОСШ";

                foreach (DataGridViewColumn t in dgv.Columns)
                    t.Width = t.HeaderText.Length * 10;

                dgv.Columns["Состояние"].Visible = false;
                dgv.Columns["Диапазон"].Visible = false;
                dgv.Columns["Поляризация"].Visible = false;
                dgv.Columns["Спутник"].Visible = false;
                dgv.Columns["ID"].Visible = false;
                foreach (string col in dataBase.SimpleData("[MainSettings] WHERE [Who] = 'LoadingColumnsVisibility' AND [What] = '" + Profile.userLogin + "'").Rows.Cast<DataRow>().Select(x => x["Content"].ToString()).ToList())
                    dgv.Columns[col].Visible = false;

                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    if (col.ValueType != typeof(string))
                       col.Width = col.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);
                }               

                if (dataBase.dataset.Tables["Loading " + keys].Rows.Count != 0)
                {
                    dgv.CurrentCell = dgv["Частота", 0];
                    UpdateBottomControls(this, new EventArgs());
                }
                dgv.SelectionChanged += UpdateBottomControls;

                filter = new SQLRequestFilter(dgv, new string[] { "Примечание", "СтекПрот" }) { Dock = DockStyle.Left };
                filter.OnFilterChanged += (se, a) => dataBase.ToDisplay("Loading " + keys + ' ' + filter.CurrentFilter + " ORDER BY [Частота]", dataTableName: "Loading " + keys);
                Controls.Add(filter);
                //Supports.GangeGroup(filter);
                filter.RefreshNodes(dgv);
            };

            Enter += (s, e) =>
            {
                if (firstBind)
                {
                    Profile.DownSign = "Излучений: " + dgv.RowCount + " Вкл: " + dgv.Rows.Cast<DataGridViewRow>().Where(x => (bool)x.Cells["Состояние"].Value == true).Count() + " Выкл.: " + dgv.Rows.Cast<DataGridViewRow>().Where(x => (bool)x.Cells["Состояние"].Value == false || x.Cells["Состояние"].Value.ToString() == "").Count();
                    dataBase.ToDisplay("Loading " + keys + ' ' + filter.CurrentFilter + " ORDER BY [Частота]", onlyAdapter: true, dataTableName: "Loading " + keys);
                }
            };

            dgv.UserDeletedRow += (s, e) =>
            {
                dataBase.sqlAdapter.Update(dataBase.dataset.Tables["Loading " + keys]);
                checkSum = dataBase.CheckSum("Loading " + keys);
            };
            dgv.UserDeletingRow += (sender, e) =>
            {
                if (MessageBoxTi.Show("Удаление излучения", "Удалить данное излучение?") == MessageResult.Yes)
                {
                    dataBase.SimpleRequest("INSERT INTO [dbo].[Deleted] ([Спутник],[Диапазон],[Поляризация],[Частота],[Состояние]," +
                    "[ОтношениеСШ],[ВидДоступа],[Модуляция],[Тактовая],[RПУК],[ПУК],[СкрембВнеш],[ПУККаскад],[СкрембВнутр]," +
                    "[Скорость],[СистемаСвязи],[Оборудование],[ДлинаКадра],[ТипДанных],[СтекПрот],[Примечание],[СостояниеАн],[Ценность]," +
                    "[Наблюдение],[ВремяДоб],[Пользователь],[Загруженность], [ВремяРедакт]) VALUES ('" + e.Row.Cells["Спутник"].Value + "', '" + e.Row.Cells["Диапазон"].Value + "', '" + e.Row.Cells["Поляризация"].Value + "', '" +
                    e.Row.Cells["Частота"].Value + "', '" + e.Row.Cells["Состояние"].Value + "', '" + e.Row.Cells["ОтношениеСШ"].Value + "', '" + e.Row.Cells["ВидДоступа"].Value + "', '" + e.Row.Cells["Модуляция"].Value + "', '" +
                    e.Row.Cells["Тактовая"].Value.ToString().Replace(',', '.') + "', '" + e.Row.Cells["RПУК"].Value + "', '" + e.Row.Cells["ПУК"].Value + "', '" + e.Row.Cells["СкрембВнеш"].Value + "', '" +
                    e.Row.Cells["ПУККаскад"].Value + "', '" + e.Row.Cells["СкрембВнутр"].Value + "', '" + e.Row.Cells["Скорость"].Value.ToString().Replace(',', '.') + "', '" + e.Row.Cells["СистемаСвязи"].Value + "', '" + e.Row.Cells["Оборудование"].Value + "', '" +
                    e.Row.Cells["ДлинаКадра"].Value + "', '" + e.Row.Cells["ТипДанных"].Value + "', '" + e.Row.Cells["СтекПрот"].Value + "', '" + e.Row.Cells["Примечание"].Value + "', '" + e.Row.Cells["СостояниеАн"].Value + "', '" +
                    e.Row.Cells["Ценность"].Value + "', '" + e.Row.Cells["Наблюдение"].Value + "', GETDATE(), '" + e.Row.Cells["Пользователь"].Value + "', '" + e.Row.Cells["Загруженность"].Value + "', '" + e.Row.Cells["ВремяРедакт"].Value + "')");

                    dataBase.ToUpdate(Profile.userLogin, "Удалил излучение спутника " + '"' + e.Row.Cells["Спутник"].Value.ToString() + '"' + " с частотой: \"" + e.Row.Cells["Частота"].Value.ToString() + "\". (запись перенесена в таблицу \"Удалённые\")");

                    e.Cancel = false;
                }
                else e.Cancel = true;
            };

            redactButton.Click += (sender, e) =>
            {
                if (dgv.Rows.Count == 0 || dgv.RowCount == 0)
                    return;

                RefreshIfOutsideCganges();

                DataRow dr1 = dataBase.SimpleData("FrequencyBand").Rows.Cast<DataRow>().Where(x => x["Наименование диапазона"].ToString() == bandISZ).FirstOrDefault();
                if ((int)dr1["Min"] > Частота.Value || (int)dr1["Max"] < Частота.Value)
                {
                    MessageBox.Show("Неверная частота. В данном диапазоне допускается частота от " + (int)dr1["Min"] + " до " + (int)dr1["Max"] + "!");
                    return;
                }
                if (MessageBoxTi.Show("Редактирование излучения", "Редактировать?") == MessageResult.Yes)
                {
                    long ID = (long)dgv.SelectedRows[0].Cells["ID"].Value;
                    bool somethingEdited = false;
                    if (!Convert.ToDecimal(dgv.SelectedRows[0].Cells["Частота"].Value).Equals(Частота.Value))
                    {
                        dataBase.ToUpdate(Profile.userLogin, "Изменил запись об излучении в спутнике \"" + dgv.SelectedRows[0].Cells["Спутник"].Value.ToString() + "\" с частотой " + dgv.SelectedRows[0].Cells["Частота"].Value.ToString() + " в графе \"Частота\" с \"" + dgv.SelectedRows[0].Cells["Частота"].Value.ToString() + "\" на \"" + Частота.Value.ToString() + "\"");
                        dgv.SelectedRows[0].Cells["Частота"].Value = Частота.Value;
                        somethingEdited = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["ОтношениеСШ"].Value.ToString().Equals(ОтношениеСШ.Text))
                    {
                        dataBase.ToUpdate(Profile.userLogin, "Изменил запись об излучении в спутнике \"" + dgv.SelectedRows[0].Cells["Спутник"].Value.ToString() + "\" с частотой " + dgv.SelectedRows[0].Cells["Частота"].Value.ToString() + " в графе \"ОтношениеСШ\" с \"" + dgv.SelectedRows[0].Cells["ОтношениеСШ"].Value.ToString() + "\" на \"" + ОтношениеСШ.Text + "\"");
                        //dgv.SelectedRows[0].Cells["ОтношениеСШ"].Value = Supports.NullOrObjectIfEmpty(ОтношениеСШ.Text);
                        dataBase.dataset.Tables["Loading " + keys].Select().Where(x => x["ID"].Equals(ID)).FirstOrDefault()["ОтношениеСШ"] = Supports.NullOrObjectIfEmpty(ОтношениеСШ.Text);
                        somethingEdited = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["Модуляция"].Value.Equals(Модуляция.SelectedItem))
                    {
                        dataBase.ToUpdate(Profile.userLogin, "Изменил запись об излучении в спутнике \"" + dgv.SelectedRows[0].Cells["Спутник"].Value.ToString() + "\" с частотой " + dgv.SelectedRows[0].Cells["Частота"].Value.ToString() + " в графе \"Модуляция\" с \"" + dgv.SelectedRows[0].Cells["Модуляция"].Value.ToString() + "\" на \"" + Модуляция.Text + "\"");
                        dataBase.dataset.Tables["Loading " + keys].Select().Where(x => x["ID"].Equals(ID)).FirstOrDefault()["Модуляция"] = Supports.NullOrObjectIfEmpty(Модуляция.SelectedItem);
                        somethingEdited = true;
                    }

                    if (!Convert.ToDecimal(dgv.SelectedRows[0].Cells["Тактовая"].Value).Equals(Тактовая.Value))
                    {
                        dataBase.ToUpdate(Profile.userLogin, "Изменил запись об излучении в спутнике \"" + dgv.SelectedRows[0].Cells["Спутник"].Value.ToString() + "\" с частотой " + dgv.SelectedRows[0].Cells["Частота"].Value.ToString() + " в графе \"Тактовая\" с \"" + dgv.SelectedRows[0].Cells["Тактовая"].Value.ToString() + "\" на \"" + Convert.ToDouble(Тактовая.Value.ToString()) + "\"");
                        dataBase.dataset.Tables["Loading " + keys].Select().Where(x => x["ID"].Equals(ID)).FirstOrDefault()["Тактовая"] = Supports.NullOrObjectIfEmpty(Тактовая.Value);
                        somethingEdited = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["ВидДоступа"].Value.Equals(ВидДоступа.SelectedItem))
                    {
                        dataBase.ToUpdate(Profile.userLogin, "Изменил запись об излучении в спутнике \"" + dgv.SelectedRows[0].Cells["Спутник"].Value.ToString() + "\" с частотой " + dgv.SelectedRows[0].Cells["Частота"].Value.ToString() + " в графе \"ВидДоступа\" с \"" + dgv.SelectedRows[0].Cells["ВидДоступа"].Value.ToString() + "\" на \"" + ВидДоступа.Text + "\"");
                        dataBase.dataset.Tables["Loading " + keys].Select().Where(x => x["ID"].Equals(ID)).FirstOrDefault()["ВидДоступа"] = Supports.NullOrObjectIfEmpty(ВидДоступа.SelectedItem);
                        somethingEdited = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["RПУК"].Value.Equals(RПУК.SelectedItem))
                    {
                        dataBase.ToUpdate(Profile.userLogin, "Изменил запись об излучении в спутнике \"" + dgv.SelectedRows[0].Cells["Спутник"].Value.ToString() + "\" с частотой " + dgv.SelectedRows[0].Cells["Частота"].Value.ToString() + " в графе \"RПУК\" с \"" + dgv.SelectedRows[0].Cells["RПУК"].Value.ToString() + "\" на \"" + RПУК.Text + "\"");
                        dataBase.dataset.Tables["Loading " + keys].Select().Where(x => x["ID"].Equals(ID)).FirstOrDefault()["RПУК"] = Supports.NullOrObjectIfEmpty(RПУК.SelectedItem);
                        somethingEdited = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["ПУК"].Value.Equals(ПУК.SelectedItem))
                    {
                        dataBase.ToUpdate(Profile.userLogin, "Изменил запись об излучении в спутнике \"" + dgv.SelectedRows[0].Cells["Спутник"].Value.ToString() + "\" с частотой " + dgv.SelectedRows[0].Cells["Частота"].Value.ToString() + " в графе \"ПУК\" с \"" + dgv.SelectedRows[0].Cells["ПУК"].Value.ToString() + "\" на \"" + ПУК.Text + "\"");
                        dataBase.dataset.Tables["Loading " + keys].Select().Where(x => x["ID"].Equals(ID)).FirstOrDefault()["ПУК"] = Supports.NullOrObjectIfEmpty(ПУК.SelectedItem);
                        somethingEdited = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["СкрембВнеш"].Value.Equals(СкрембВнеш.SelectedItem))
                    {
                        dataBase.ToUpdate(Profile.userLogin, "Изменил запись об излучении в спутнике \"" + dgv.SelectedRows[0].Cells["Спутник"].Value.ToString() + "\" с частотой " + dgv.SelectedRows[0].Cells["Частота"].Value.ToString() + " в графе \"СкрембВнеш\" с \"" + dgv.SelectedRows[0].Cells["СкрембВнеш"].Value.ToString() + "\" на \"" + СкрембВнеш.Text + "\"");
                        dataBase.dataset.Tables["Loading " + keys].Select().Where(x => x["ID"].Equals(ID)).FirstOrDefault()["СкрембВнеш"] = Supports.NullOrObjectIfEmpty(СкрембВнеш.SelectedItem);
                        somethingEdited = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["ПУККаскад"].Value.Equals(ПУККаскад.SelectedItem))
                    {
                        dataBase.ToUpdate(Profile.userLogin, "Изменил запись об излучении в спутнике \"" + dgv.SelectedRows[0].Cells["Спутник"].Value.ToString() + "\" с частотой " + dgv.SelectedRows[0].Cells["Частота"].Value.ToString() + " в графе \"ПУККаскад\" с \"" + dgv.SelectedRows[0].Cells["ПУККаскад"].Value.ToString() + "\" на \"" + ПУККаскад.Text + "\"");
                        dataBase.dataset.Tables["Loading " + keys].Select().Where(x => x["ID"].Equals(ID)).FirstOrDefault()["ПУККаскад"] = Supports.NullOrObjectIfEmpty(ПУККаскад.SelectedItem);
                        somethingEdited = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["СкрембВнутр"].Value.Equals(СкрембВнутр.SelectedItem))
                    {
                        dataBase.ToUpdate(Profile.userLogin, "Изменил запись об излучении в спутнике \"" + dgv.SelectedRows[0].Cells["Спутник"].Value.ToString() + "\" с частотой " + dgv.SelectedRows[0].Cells["Частота"].Value.ToString() + " в графе \"СкрембВнутр\" с \"" + dgv.SelectedRows[0].Cells["СкрембВнутр"].Value.ToString() + "\" на \"" + СкрембВнутр.Text + "\"");
                        dataBase.dataset.Tables["Loading " + keys].Select().Where(x => x["ID"].Equals(ID)).FirstOrDefault()["СкрембВнутр"] = Supports.NullOrObjectIfEmpty(СкрембВнутр.SelectedItem);
                        somethingEdited = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["СистемаСвязи"].Value.Equals(СистемаСвязи.SelectedItem))
                    {
                        dataBase.ToUpdate(Profile.userLogin, "Изменил запись об излучении в спутнике \"" + dgv.SelectedRows[0].Cells["Спутник"].Value.ToString() + "\" с частотой " + dgv.SelectedRows[0].Cells["Частота"].Value.ToString() + " в графе \"СистемаСвязи\" с \"" + dgv.SelectedRows[0].Cells["СистемаСвязи"].Value.ToString() + "\" на \"" + СистемаСвязи.Text + "\"");
                        dataBase.dataset.Tables["Loading " + keys].Select().Where(x => x["ID"].Equals(ID)).FirstOrDefault()["СистемаСвязи"] = Supports.NullOrObjectIfEmpty(СистемаСвязи.SelectedItem);
                        somethingEdited = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["Оборудование"].Value.Equals(Оборудование.SelectedItem))
                    {
                        dataBase.ToUpdate(Profile.userLogin, "Изменил запись об излучении в спутнике \"" + dgv.SelectedRows[0].Cells["Спутник"].Value.ToString() + "\" с частотой " + dgv.SelectedRows[0].Cells["Частота"].Value.ToString() + " в графе \"Оборудование\" с \"" + dgv.SelectedRows[0].Cells["Оборудование"].Value.ToString() + "\" на \"" + Оборудование.SelectedItem?.ToString() + "\"");
                        dataBase.dataset.Tables["Loading " + keys].Select().Where(x => x["ID"].Equals(ID)).FirstOrDefault()["Оборудование"] = Supports.NullOrObjectIfEmpty(Оборудование.SelectedItem?.ToString());
                        somethingEdited = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["ДлинаКадра"].Value.ToString().Equals(ДлинаКадра.Text))
                    {
                        dataBase.ToUpdate(Profile.userLogin, "Изменил запись об излучении в спутнике \"" + dgv.SelectedRows[0].Cells["Спутник"].Value.ToString() + "\" с частотой " + dgv.SelectedRows[0].Cells["Частота"].Value.ToString() + " в графе \"ДлинаКадра\" с \"" + dgv.SelectedRows[0].Cells["ДлинаКадра"].Value.ToString() + "\" на \"" + ДлинаКадра.Text + "\"");
                        dataBase.dataset.Tables["Loading " + keys].Select().Where(x => x["ID"].Equals(ID)).FirstOrDefault()["ДлинаКадра"] = Supports.NullOrObjectIfEmpty(ДлинаКадра.Text);
                        somethingEdited = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["ТипДанных"].Value.Equals(ТипДанных.SelectedItem))
                    {
                        dataBase.ToUpdate(Profile.userLogin, "Изменил запись об излучении в спутнике \"" + dgv.SelectedRows[0].Cells["Спутник"].Value.ToString() + "\" с частотой " + dgv.SelectedRows[0].Cells["Частота"].Value.ToString() + " в графе \"ТипДанных\" с \"" + dgv.SelectedRows[0].Cells["ТипДанных"].Value.ToString() + "\" на \"" + ТипДанных.SelectedItem?.ToString() + "\"");
                        dataBase.dataset.Tables["Loading " + keys].Select().Where(x => x["ID"].Equals(ID)).FirstOrDefault()["ТипДанных"] = Supports.NullOrObjectIfEmpty(ТипДанных.SelectedItem?.ToString());
                        somethingEdited = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["СтекПрот"].Value.ToString().Equals(СтекПрот.Text))
                    {
                        dataBase.ToUpdate(Profile.userLogin, "Изменил запись об излучении в спутнике \"" + dgv.SelectedRows[0].Cells["Спутник"].Value.ToString() + "\" с частотой " + dgv.SelectedRows[0].Cells["Частота"].Value.ToString() + " в графе \"СтекПрот\" с \"" + dgv.SelectedRows[0].Cells["СтекПрот"].Value.ToString() + "\" на \"" + СтекПрот.Text + "\"");
                        dataBase.dataset.Tables["Loading " + keys].Select().Where(x => x["ID"].Equals(ID)).FirstOrDefault()["СтекПрот"] = Supports.NullOrObjectIfEmpty(СтекПрот.Text);
                        somethingEdited = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["Примечание"].Value.ToString().Equals(Примечание.Text))
                    {
                        dataBase.ToUpdate(Profile.userLogin, "Изменил запись об излучении в спутнике \"" + dgv.SelectedRows[0].Cells["Спутник"].Value.ToString() + "\" с частотой " + dgv.SelectedRows[0].Cells["Частота"].Value.ToString() + " в графе \"Примечание\" с \"" + dgv.SelectedRows[0].Cells["Примечание"].Value.ToString() + "\" на \"" + Примечание.Text + "\"");
                        dataBase.dataset.Tables["Loading " + keys].Select().Where(x => x["ID"].Equals(ID)).FirstOrDefault()["Примечание"] = Supports.NullOrObjectIfEmpty(Примечание.Text);
                        somethingEdited = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["СостояниеАн"].Value.Equals(СостояниеАн.SelectedItem))
                    {
                        dataBase.ToUpdate(Profile.userLogin, "Изменил запись об излучении в спутнике \"" + dgv.SelectedRows[0].Cells["Спутник"].Value.ToString() + "\" с частотой " + dgv.SelectedRows[0].Cells["Частота"].Value.ToString() + " в графе \"СостояниеАн\" с \"" + dgv.SelectedRows[0].Cells["СостояниеАн"].Value.ToString() + "\" на \"" + СостояниеАн.Text + "\"");
                        dataBase.dataset.Tables["Loading " + keys].Select().Where(x => x["ID"].Equals(ID)).FirstOrDefault()["СостояниеАн"] = Supports.NullOrObjectIfEmpty(СостояниеАн.SelectedItem);
                        somethingEdited = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["Ценность"].Value.Equals(Ценность.SelectedItem))
                    {
                        dataBase.ToUpdate(Profile.userLogin, "Изменил запись об излучении в спутнике \"" + dgv.SelectedRows[0].Cells["Спутник"].Value.ToString() + "\" с частотой " + dgv.SelectedRows[0].Cells["Частота"].Value.ToString() + " в графе \"Ценность\" с \"" + dgv.SelectedRows[0].Cells["Ценность"].Value.ToString() + "\" на \"" + Ценность.Text + "\"");
                        dataBase.dataset.Tables["Loading " + keys].Select().Where(x => x["ID"].Equals(ID)).FirstOrDefault()["Ценность"] = Supports.NullOrObjectIfEmpty(Ценность.SelectedItem);
                        somethingEdited = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["Наблюдение"].Value.Equals(Наблюдение.SelectedItem))
                    {
                        dataBase.ToUpdate(Profile.userLogin, "Изменил запись об излучении в спутнике \"" + dgv.SelectedRows[0].Cells["Спутник"].Value.ToString() + "\" с частотой " + dgv.SelectedRows[0].Cells["Частота"].Value.ToString() + " в графе \"Наблюдение\" с \"" + dgv.SelectedRows[0].Cells["Наблюдение"].Value.ToString() + "\" на \"" + Наблюдение.Text + "\"");
                        dataBase.dataset.Tables["Loading " + keys].Select().Where(x => x["ID"].Equals(ID)).FirstOrDefault()["Наблюдение"] = Supports.NullOrObjectIfEmpty(Наблюдение.SelectedItem);
                        somethingEdited = true;
                    }

                    if (!dgv.SelectedRows[0].Cells["Загруженность"].Value.Equals(Загруженность.SelectedItem))
                    {
                        dataBase.ToUpdate(Profile.userLogin, "Изменил запись об излучении в спутнике \"" + dgv.SelectedRows[0].Cells["Спутник"].Value.ToString() + "\" с частотой " + dgv.SelectedRows[0].Cells["Частота"].Value.ToString() + " в графе \"Загруженность\" с \"" + dgv.SelectedRows[0].Cells["Загруженность"].Value.ToString() + "\" на \"" + Загруженность.Text + "\"");
                        dataBase.dataset.Tables["Loading " + keys].Select().Where(x => x["ID"].Equals(ID)).FirstOrDefault()["Загруженность"] = Supports.NullOrObjectIfEmpty(Загруженность.SelectedItem);
                        somethingEdited = true;
                    }

                    if (somethingEdited == false)
                        return;

                    dataBase.dataset.Tables["Loading " + keys].Select().Where(x => x["ID"].Equals(ID)).FirstOrDefault()["ВремяРедакт"] = dataBase.ToCount("SELECT GETDATE()");

                    dataBase.dataset.Tables["Loading " + keys].Select().Where(x => x["ID"].Equals(ID)).FirstOrDefault()["Пользователь"] = Profile.userName;

                    dataBase.dataset.Tables["Loading " + keys].Select().Where(x => x["ID"].Equals(ID)).FirstOrDefault()["Скорость"] = CalcSpeed(
                        Тактовая.Text,
                        RПУК.Text,
                        ПУККаскад.Text,
                        СистемаСвязи.Text,
                        Модуляция.Text
                        );

                    dataBase.sqlAdapter.Update(dataBase.dataset.Tables["Loading " + keys]);
                    checkSum = dataBase.CheckSum("Loading " + keys);
                }
            };
            addButton.Click += (sender, e) =>
            {
                MessageResult continueIfSameBand = MessageResult.None;
                DataRow dr1 = dataBase.SimpleData("FrequencyBand").Rows.Cast<DataRow>().Where(x => x["Наименование диапазона"].ToString() == bandISZ).FirstOrDefault();
                if ((int)dr1["Min"] > Частота.Value || (int)dr1["Max"] < Частота.Value)
                {
                    MessageBoxTi.Show("Неверная частота. В данном диапазоне допускается частота от " + (int)dr1["Min"] + " до " + (int)dr1["Max"] + "!");
                    return;
                }

                DataGridViewRow rowForCompareIfFerqExist = dgv.Rows.Cast<DataGridViewRow>().Where(x => (Convert.ToInt32(x.Cells["Частота"].Value) < Частота.Value + 50) && (Convert.ToInt32(x.Cells["Частота"].Value) > Частота.Value - 50)).FirstOrDefault();
                if (rowForCompareIfFerqExist != null)
                    continueIfSameBand = MessageBoxTi.Show("Добавление излучения", "Излучение с частотой " + Частота.Value.ToString() + "(~" + rowForCompareIfFerqExist.Cells["Частота"].Value.ToString() + ") уже есть. Всё равно добавить?");
                else
                    continueIfSameBand = MessageBoxTi.Show("Добавление излучения", "Добавить излучение?");


                if (continueIfSameBand == MessageResult.Yes)
                {
                    DataRow p = (dgv.DataSource as DataTable).NewRow();

                    p["Спутник"] = nameISZ;
                    p["Диапазон"] = bandISZ;
                    p["Поляризация"] = polarizationISZ;
                    p["Частота"] = Частота.Value;
                    p["Состояние"] = true;
                    p["ОтношениеСШ"] = Supports.NullOrObjectIfEmpty(ОтношениеСШ.Text);
                    p["Модуляция"] = Supports.NullOrObjectIfEmpty(Модуляция.SelectedItem);
                    p["Тактовая"] = Тактовая.Value;
                    p["ВидДоступа"] = Supports.NullOrObjectIfEmpty(ВидДоступа.SelectedItem);
                    p["RПУК"] = Supports.NullOrObjectIfEmpty(RПУК.SelectedItem);
                    p["ПУК"] = Supports.NullOrObjectIfEmpty(ПУК.SelectedItem);
                    p["СкрембВнеш"] = Supports.NullOrObjectIfEmpty(СкрембВнеш.SelectedItem);
                    p["ПУККаскад"] = Supports.NullOrObjectIfEmpty(ПУККаскад.SelectedItem);
                    p["СкрембВнутр"] = Supports.NullOrObjectIfEmpty(СкрембВнутр.SelectedItem);
                    p["Скорость"] = CalcSpeed(
                        Тактовая.Text,
                        RПУК.Text,
                        ПУККаскад.Text,
                        СистемаСвязи.Text,
                        Модуляция.Text
                        );
                    p["СистемаСвязи"] = Supports.NullOrObjectIfEmpty(СистемаСвязи.SelectedItem);
                    p["Оборудование"] = Supports.NullOrObjectIfEmpty(Оборудование.SelectedItem);
                    p["ДлинаКадра"] = Supports.NullOrObjectIfEmpty(ДлинаКадра.Text);
                    p["ТипДанных"] = Supports.NullOrObjectIfEmpty(ТипДанных.SelectedItem);
                    p["СтекПрот"] = Supports.NullOrObjectIfEmpty(СтекПрот.Text);
                    p["Примечание"] = Supports.NullOrObjectIfEmpty(Примечание.Text);
                    p["СостояниеАн"] = Supports.NullOrObjectIfEmpty(СостояниеАн.SelectedItem);
                    p["Ценность"] = Supports.NullOrObjectIfEmpty(Ценность.SelectedItem);
                    p["Наблюдение"] = Supports.NullOrObjectIfEmpty(Наблюдение.SelectedItem);
                    p["ВремяДоб"] = dataBase.ToCount("SELECT GETDATE()");
                    p["Пользователь"] = Profile.userName;
                    p["Загруженность"] = Supports.NullOrObjectIfEmpty(Загруженность.SelectedItem);
                    p["ВремяРедакт"] = dataBase.ToCount("SELECT GETDATE()");
                 

                    var fer = Частота.Value;

                    dataBase.ToUpdate(Profile.userLogin, "Добавил излучение спутника \"" + p["Спутник"].ToString() + "\" с частотой \"" + p["Частота"].ToString() + "\"");
                    dataBase.dataset.Tables["Loading " + keys].Rows.Add(p);
                    dataBase.sqlAdapter.Update(dataBase.dataset.Tables["Loading " + keys]);
                    dataBase.ToDisplay("Loading " + keys + ' ' + filter.CurrentFilter + " ORDER BY [Частота]", dataTableName: "Loading " + keys);
                    checkSum = dataBase.CheckSum("Loading " + keys);

                    dgv.CurrentCell = dgv.Rows.Cast<DataGridViewRow>().OrderByDescending(x => x.Cells["ID"].Value).First().Cells["Частота"];
                    int firstDisplayedScrollingRowIndex = dgv.CurrentCell.RowIndex - (dgv.DisplayedRowCount(false) / 2);
                    if (firstDisplayedScrollingRowIndex > 0)
                        dgv.FirstDisplayedScrollingRowIndex = firstDisplayedScrollingRowIndex;
                    else
                        dgv.FirstDisplayedScrollingRowIndex = 0;

                        dataBase.SimpleRequest("INSERT INTO [dbo].[SSALoading] ([Спутник], [Диапазон], [Поляризация], [Частота], [ВремяВкл], [Состояние], [ВидДоступа], [Модуляция], [Тактовая], [RПУК], [ПУК], [Скремб], [Скорость], [СистемаСвязи], [Протоколы], [Примечание], [Пользователь])" +
                                                " SELECT [Спутник], [Диапазон], [Поляризация], [Частота], GETDATE(), '1', [ВидДоступа], [Модуляция], [Тактовая], [RПУК], [ПУК], [СкрембВнеш], [Скорость], [СистемаСвязи], [СтекПрот], [Примечание], '" + Profile.userName +
                                                "' FROM[dbo].[Loading] WHERE [dbo].[Loading].[ID] = '" + dgv.SelectedRows[0].Cells["ID"].Value + "'");



                }
            };

            onOff.Click += (s, e) =>
            {
                if (dgv.SelectedCells.Count == 0)
                    return;

                RefreshIfOutsideCganges();

                if (MessageBoxTi.Show((bool)dgv.SelectedRows[0].Cells["Состояние"].Value == true ? "Отключить излучение?" : "Включить излучение?", "Состояние излучения") == MessageResult.Yes && dgv.RowCount != 0)
                {

                    dataBase.ToUpdate(Profile.userLogin, "Изменил запись об излучении в спутнике \"" + dgv.SelectedRows[0].Cells["Спутник"].Value.ToString() + "\" с частотой " + dgv.SelectedRows[0].Cells["Частота"].Value.ToString() + " в графе \"Состояние\" с \"" + dgv.SelectedRows[0].Cells["Состояние"].Value + "\" на \"" + !(bool)dgv.SelectedRows[0].Cells["Состояние"].Value + "\"");
                    dataBase.dataset.Tables["Loading " + keys].Select().Where(x => x["ID"].Equals(dgv.SelectedRows[0].Cells["ID"].Value)).FirstOrDefault()["Состояние"] = !(bool)dgv.SelectedRows[0].Cells["Состояние"].Value;
                    dataBase.dataset.Tables["Loading " + keys].Select().Where(x => x["ID"].Equals(dgv.SelectedRows[0].Cells["ID"].Value)).FirstOrDefault()["Пользователь"] = Profile.userName;
                    dataBase.dataset.Tables["Loading " + keys].Select().Where(x => x["ID"].Equals(dgv.SelectedRows[0].Cells["ID"].Value)).FirstOrDefault()["ВремяДоб"] = dataBase.ToCount("SELECT GETDATE()");

                    onOff.BackColor = (bool)dgv.SelectedRows[0].Cells["Состояние"].Value ? Supports.Green : Supports.Red;

                        dataBase.SimpleRequest("INSERT INTO [dbo].[SSALoading] ([Спутник], [Диапазон], [Поляризация], [Частота], [ВремяВкл], [Состояние], [ВидДоступа], [Модуляция], [Тактовая], [RПУК], [ПУК], [Скремб], [Скорость], [СистемаСвязи], [Протоколы], [Примечание], [Пользователь])" +
                                                " SELECT [Спутник], [Диапазон], [Поляризация], [Частота], GETDATE(), '" + (bool)dgv.SelectedRows[0].Cells["Состояние"].Value + "', [ВидДоступа], [Модуляция], [Тактовая], [RПУК], [ПУК], [СкрембВнеш], [Скорость], [СистемаСвязи], [СтекПрот], [Примечание], '" + Profile.userName +
                                                "' FROM[dbo].[Loading] WHERE [dbo].[Loading].[ID] = '" + dgv.SelectedRows[0].Cells["ID"].Value + "'");

                    dataBase.sqlAdapter.Update(dataBase.dataset.Tables["Loading " + keys]);
                    checkSum = dataBase.CheckSum("Loading " + keys);
                    RefreshDGV();
                }
            };
            dgv.CellMouseDoubleClick += (se, arg) =>
            {
                if (arg.Button == MouseButtons.Left && arg.RowIndex != -1)
                {
                    if (dgv.SelectedCells.Count == 0)
                        return;

                    RefreshIfOutsideCganges();


                    if (MessageBoxTi.Show((bool)dgv.SelectedRows[0].Cells["Состояние"].Value == true ? "Отключить излучение?" : "Включить излучение?", "Состояние излучения") == MessageResult.Yes && dgv.RowCount != 0)
                    {


                        dataBase.ToUpdate(Profile.userLogin, "Изменил запись об излучении в спутнике \"" + dgv.SelectedRows[0].Cells["Спутник"].Value.ToString() + "\" с частотой " + dgv.SelectedRows[0].Cells["Частота"].Value.ToString() + " в графе \"Состояние\" с \"" + !(bool)dgv.SelectedRows[0].Cells["Состояние"].Value + "\"");
                        dataBase.dataset.Tables["Loading " + keys].Select().Where(x => x["ID"].Equals(dgv.SelectedRows[0].Cells["ID"].Value)).FirstOrDefault()["Состояние"] = !(bool)dgv.SelectedRows[0].Cells["Состояние"].Value;
                        dataBase.dataset.Tables["Loading " + keys].Select().Where(x => x["ID"].Equals(dgv.SelectedRows[0].Cells["ID"].Value)).FirstOrDefault()["Пользователь"] = Profile.userName;
                        dataBase.dataset.Tables["Loading " + keys].Select().Where(x => x["ID"].Equals(dgv.SelectedRows[0].Cells["ID"].Value)).FirstOrDefault()["ВремяДоб"] = dataBase.ToCount("SELECT GETDATE()");


                        onOff.BackColor = (bool)dgv.SelectedRows[0].Cells["Состояние"].Value ? Supports.Green : Supports.Red;


                            dataBase.SimpleRequest("INSERT INTO [dbo].[SSALoading] ([Спутник], [Диапазон], [Поляризация], [Частота], [ВремяВкл], [Состояние], [ВидДоступа], [Модуляция], [Тактовая], [RПУК], [ПУК], [Скремб], [Скорость], [СистемаСвязи], [Протоколы], [Примечание], [Пользователь])" +
                                                    " SELECT [Спутник], [Диапазон], [Поляризация], [Частота], GETDATE(), '" + (bool)dgv.SelectedRows[0].Cells["Состояние"].Value + "', [ВидДоступа], [Модуляция], [Тактовая], [RПУК], [ПУК], [СкрембВнеш], [Скорость], [СистемаСвязи], [СтекПрот], [Примечание], '" + Profile.userName +
                                                    "' FROM[dbo].[Loading] WHERE [dbo].[Loading].[ID] = '" + dgv.SelectedRows[0].Cells["ID"].Value + "'");


                        dataBase.sqlAdapter.Update(dataBase.dataset.Tables["Loading " + keys]);
                        checkSum = dataBase.CheckSum("Loading " + keys);
                        RefreshDGV();
                    }
                }
            };

            dgv.CellMouseClick += (sender, e) =>
            {
                if (dgv != null)
                {
                    if (e.Button == MouseButtons.Right)
                        Supports.MenuPanel(dgv, e.RowIndex, e.ColumnIndex, typeof(Loading), this);
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
        }

        public void UpdateBottomControls(object sender, EventArgs e)
        {
            try
            {
                Частота.Value = Convert.ToDecimal(dgv.SelectedRows[0].Cells["Частота"].Value);
                Тактовая.Value = Convert.ToDecimal(dgv.SelectedRows[0].Cells["Тактовая"].Value);
                ОтношениеСШ.Text = dgv.SelectedRows[0].Cells["ОтношениеСШ"].Value.ToString();
                Скорость.Text = dgv.SelectedRows[0].Cells["Скорость"].Value.ToString();
                Примечание.Text = dgv.SelectedRows[0].Cells["Примечание"].Value.ToString();
                ДлинаКадра.Text = dgv.SelectedRows[0].Cells["ДлинаКадра"].Value.ToString();
                СтекПрот.Text = dgv.SelectedRows[0].Cells["СтекПрот"].Value.ToString();
                Загруженность.Text = dgv.SelectedRows[0].Cells["Загруженность"].Value.ToString();
                СостояниеАн.Text = dgv.SelectedRows[0].Cells["СостояниеАн"].Value.ToString();
                Наблюдение.Text = dgv.SelectedRows[0].Cells["Наблюдение"].Value.ToString();
                Ценность.Text = dgv.SelectedRows[0].Cells["Ценность"].Value.ToString();
                СистемаСвязи.Text = dgv.SelectedRows[0].Cells["СистемаСвязи"].Value.ToString();
                ПУККаскад.Text = dgv.SelectedRows[0].Cells["ПУККаскад"].Value.ToString();
                СкрембВнутр.Text = dgv.SelectedRows[0].Cells["СкрембВнутр"].Value.ToString();
                СкрембВнеш.Text = dgv.SelectedRows[0].Cells["СкрембВнеш"].Value.ToString();
                RПУК.Text = dgv.SelectedRows[0].Cells["RПУК"].Value.ToString();
                ПУК.Text = dgv.SelectedRows[0].Cells["ПУК"].Value.ToString();
                ВидДоступа.Text = dgv.SelectedRows[0].Cells["ВидДоступа"].Value.ToString();
                Модуляция.Text = dgv.SelectedRows[0].Cells["Модуляция"].Value.ToString();
                Оборудование.Text = dgv.SelectedRows[0].Cells["Оборудование"].Value.ToString();
                ТипДанных.Text = dgv.SelectedRows[0].Cells["ТипДанных"].Value.ToString();
                onOff.BackColor = (bool)dgv.SelectedRows[0].Cells["Состояние"].Value ? Supports.Green : Supports.Red;
            }
            catch (Exception)
            {
                return;
            }


        }

        private void LoadingControls(string keys = null)
        {
            Controls.Add(dgv);

            Controls.Add(mainPanel);

            Controls.Add(new Splitter()
            {
                Dock = DockStyle.Left,
                Name = "filterSplitter",
            });

            (mainPanel as Panel).Controls.Add(new TableLayoutPanel()
            {
                Name = "LoadingTableLayoutPanel1",
                Anchor = AnchorStyles.None,
                Height = 175,
                Width = 1050,
                Location = new Point(((mainPanel as Panel).Width / 2) - (1050 / 2), 0),
                BackColor = Supports.headGrey,
            });

            (Controls.Find("LoadingTableLayoutPanel1", true).FirstOrDefault() as TableLayoutPanel).RowStyles.Clear();
            (Controls.Find("LoadingTableLayoutPanel1", true).FirstOrDefault() as TableLayoutPanel).ColumnStyles.Clear();
            (Controls.Find("LoadingTableLayoutPanel1", true).FirstOrDefault() as TableLayoutPanel).ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 645));
            (Controls.Find("LoadingTableLayoutPanel1", true).FirstOrDefault() as TableLayoutPanel).ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 55));
            (Controls.Find("LoadingTableLayoutPanel1", true).FirstOrDefault() as TableLayoutPanel).ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 330));

            (Controls.Find("LoadingTableLayoutPanel1", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(new FlowLayoutPanel()
            {
                Name = "LoadingFlowLayoutPanel2",
                Dock = DockStyle.Fill,
                BackColor = Supports.headGrey,
                ForeColor = Supports.textWhite,
            }, 0, 0);

            (Controls.Find("LoadingTableLayoutPanel1", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(new FlowLayoutPanel()
            {
                Name = "LoadingFlowLayoutPanel3",
                Dock = DockStyle.Fill,
                BackColor = Supports.headGrey,
                ForeColor = Supports.textWhite,
            }, 2, 0);

            (Controls.Find("LoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "LoadingFrequencyGroupBox",
                Width = 110,
                Height = 37,
                Text = "Частота (Гц):",
            });

            (Controls.Find("LoadingFrequencyGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(Частота);

            (Controls.Find("LoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "LoadingBodGroupBox",
                Width = 114,
                Height = 37,
                Text = "Тактовая (кБод):",
            });

            (Controls.Find("LoadingBodGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(Тактовая);

            (Controls.Find("LoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "LoadingSShGroupBox",
                Width = 50,
                Height = 37,
                Text = "с/ш:",
            });

            (Controls.Find("LoadingSShGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(ОтношениеСШ);

            (Controls.Find("LoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "LoadingModulationGroupBox",
                Width = 80,
                Height = 37,
                Text = "Модуляция:",
            });

            (Controls.Find("LoadingModulationGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(Модуляция);

            (Controls.Find("LoadingFlowLayoutPanel2", true).FirstOrDefault() as Panel).Controls.Add(new GroupBox()
            {
                Name = "LoadingSpead1GroupBox",
                Dock = DockStyle.Right,
                Width = 100,
                Text = "Скорость (кБит/c):",
            });

            (Controls.Find("LoadingSpead1GroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(Скорость);

            (Controls.Find("LoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "LoadingCoderGroupBox",
                Width = 75,
                Height = 37,
                Text = "Вид доступа:",
            });

            (Controls.Find("LoadingCoderGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(ВидДоступа);

            (Controls.Find("LoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "LoadingRGroupBox",
                Width = 65,
                Height = 37,
                Text = "R:",
            });

            (Controls.Find("LoadingRGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(RПУК);

            (Controls.Find("LoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "LoadingCodingGroupBox",
                Width = 170,
                Height = 37,
                Text = "Помехоустойчивое кодирование:",
            });

            (Controls.Find("LoadingCodingGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(ПУК);

            (Controls.Find("LoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "LoadingEnvironmentGroupBox",
                Width = 100,
                Height = 37,
                Text = "Оборудование:",
                Dock = DockStyle.Fill,
            });

            (Controls.Find("LoadingEnvironmentGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(Оборудование);

            (Controls.Find("LoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "LoadingLoadingingGroupBox",
                Width = 95,
                Height = 37,
                Text = "Загруженность:",
            });

            (Controls.Find("LoadingLoadingingGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(Загруженность);

            (Controls.Find("LoadingFlowLayoutPanel2", true).FirstOrDefault() as Panel).Controls.Add(new GroupBox()
            {
                Name = "LoadingDataTypeGroupBox",
                Width = 110,
                Height = 37,
                Text = "Тип данных:",
            });

            (Controls.Find("LoadingDataTypeGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(ТипДанных);

            (Controls.Find("LoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "LoadingScrembler1GroupBox",
                Width = 132,
                Height = 37,
                Text = "Скремблер:",
            });

            (Controls.Find("LoadingScrembler1GroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(СкрембВнеш);

            (Controls.Find("LoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "LoadingCascadCodingGroupBox",
                Width = 170,
                Height = 37,
                Text = "Каскадное кодирование:",
            });

            (Controls.Find("LoadingCascadCodingGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(ПУККаскад);

            (Controls.Find("LoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "LoadingFrameGroupBox",
                Width = 61,
                Height = 37,
                Text = "Кадр:",
            });

            (Controls.Find("LoadingFrameGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(ДлинаКадра);

            (Controls.Find("LoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "LoadingAnConditionGroupBox",
                Width = 105,
                Height = 37,
                Text = "Сост. ан.:",
            });

            (Controls.Find("LoadingAnConditionGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(СостояниеАн);

            (Controls.Find("LoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "LoadingConnectSystemGroupBox",
                Width = 139,
                Height = 37,
                Text = "Система связи:",
            });

            (Controls.Find("LoadingConnectSystemGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(СистемаСвязи);

            (Controls.Find("LoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "LoadingScrembler2GroupBox",
                Width = 132,
                Height = 37,
                Text = "Скремблер:",
            });

            (Controls.Find("LoadingScrembler2GroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(СкрембВнутр);

            (Controls.Find("LoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "LoadingValueGroupBox",
                Width = 119,
                Height = 37,
                Text = "Ценность:",
            });

            (Controls.Find("LoadingValueGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(Ценность);

            (Controls.Find("LoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "LoadingSupervisionGroupBox",
                Width = 110,
                Height = 37,
                Text = "Наблюдение:",
            });

            (Controls.Find("LoadingFlowLayoutPanel2", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new GroupBox()
            {
                Name = "LoadingProtokolStackGroupBox",
                Width = 390,
                Height = 37,
                Text = "Стек протоколов:",
            });

            (Controls.Find("LoadingProtokolStackGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(СтекПрот);

            (Controls.Find("LoadingSupervisionGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(Наблюдение);

            (Controls.Find("LoadingTableLayoutPanel1", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(new TableLayoutPanel()
            {
                Name = "ButtonsTableLayoutPanel",
                Dock = DockStyle.Fill,
            }, 1, 0);

            (Controls.Find("ButtonsTableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).RowStyles.Clear();
            (Controls.Find("ButtonsTableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).ColumnStyles.Clear();
            (Controls.Find("ButtonsTableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).RowStyles.Add(new RowStyle(SizeType.Absolute, 55));
            (Controls.Find("ButtonsTableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).RowStyles.Add(new RowStyle(SizeType.Absolute, 55));
            (Controls.Find("ButtonsTableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).RowStyles.Add(new RowStyle(SizeType.Absolute, 55));

            (Controls.Find("ButtonsTableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(onOff, 0, 0);

            (Controls.Find("ButtonsTableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(addButton, 0, 2);

            (Controls.Find("ButtonsTableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(redactButton, 0, 1);

            new ToolTip().SetToolTip(onOff, "Переключить излучение");
            new ToolTip().SetToolTip(addButton, "Добавить излучение");
            new ToolTip().SetToolTip(redactButton, "Редактировать излучение");

            (Controls.Find("LoadingFlowLayoutPanel3", true).FirstOrDefault() as FlowLayoutPanel).Controls.Add(new Panel()
            {
                Name = "LoadingAboutPanel",
                Width = 322,
                Height = 165,
            });

            (Controls.Find("LoadingAboutPanel", true).FirstOrDefault() as Panel).Controls.Add(Примечание);

            ДлинаКадра.TextChanged += (s, args) =>
            {
                int p;
                if (!int.TryParse(ДлинаКадра.Text, out p))
                {
                    if (ДлинаКадра.TextLength != 0)
                        ДлинаКадра.Text = ДлинаКадра.Text.Remove(ДлинаКадра.TextLength - 1);
                    ДлинаКадра.Select(ДлинаКадра.TextLength, 0);
                }
            };

            Скорость.TextChanged += (s, args) =>
            {
                int p;
                if (!int.TryParse(Скорость.Text, out p))
                {
                    if (Скорость.TextLength != 0)
                        Скорость.Text = Скорость.Text.Remove(Скорость.TextLength - 1);
                    Скорость.Select(Скорость.TextLength, 0);
                }
            };

            ОтношениеСШ.TextChanged += (s, args) =>
            {
                int p;
                if (!int.TryParse(ОтношениеСШ.Text, out p))
                {
                    if (ОтношениеСШ.TextLength != 0)
                        ОтношениеСШ.Text = ОтношениеСШ.Text.Remove(ОтношениеСШ.TextLength - 1);
                    ОтношениеСШ.Select(ОтношениеСШ.TextLength, 0);
                }
            };
        }

        private double CalcSpeed(object Bod, object R, object Coding, object connectionSystem, object modulation)
        {
            double r1;
            double r2;
            double rMX;
            double rMOD;

            switch (R)
            {
                case "1|2":
                    r1 = 0.5;
                    break;
                case "2|3":
                    r1 = 0.6666667;
                    break;
                case "3|4":
                    r1 = 0.75;
                    break;
                case "3|5":
                    r1 = 0.6;
                    break;
                case "4|5":
                    r1 = 0.8;
                    break;
                case "5|6":
                    r1 = 0.83333333;
                    break;
                case "6|7":
                    r1 = 0.85714;
                    break;
                case "7|8":
                    r1 = 0.875;
                    break;
                case "8|9":
                    r1 = 0.8888889;
                    break;
                case "1|5":
                    r1 = 0.2;
                    break;
                case "1|4":
                    r1 = 0.25;
                    break;
                case "1|3":
                    r1 = 0.33333333;
                    break;
                case "7|10":
                    r1 = 0.7;
                    break;
                case "9|10":
                    r1 = 0.9;
                    break;
                case "19|20":
                    r1 = 0.95;
                    break;
                default:
                    r1 = 1;
                    break;
            }

            switch (Coding)
            {
                case "RS (126,112) (IBS)":
                    r2 = 0.88888;
                    break;
                case "RS (204,188) (DVB)":
                    r2 = 0.9215;
                    break;
                case "RS (194,178) (IDR)":
                    r2 = 0.9175;
                    break;
                case "RS (219,201) (IDR < 1M)":
                    r2 = 0.9178;
                    break;
                case "RS (219,201) (IDR > 1M)":
                    r2 = 0.9178;
                    break;
                case "RS (225,205) (IDR)":
                    r2 = 0.91111;
                    break;
                case "RS (208,192) (IDR)":
                    r2 = 0.923;
                    break;
                case "RS (448,384) (VSAT)":
                    r2 = 0.8571;
                    break;
                case "RS (160,146) (DVBN)":
                    r2 = 0.9125;
                    break;
                case "RS (255,139,4)":
                    r2 = 0.545;
                    break;
                case "RS (255,139,13)":
                    r2 = 0.545;
                    break;
                case "RS (146,130,13)":
                    r2 = 0.89;
                    break;
                case "RS (973,935) (Turbo Broadcom)":
                    r2 = 0.96;
                    break;
                default:
                    r2 = 1;
                    break;
            }

            switch (connectionSystem)
            {
                case "EDMAC":
                    rMX = 0.9524;
                    break;
                case "EDMAC-2":
                    rMX = 0.9836;
                    break;
                case "EDMAC D&I++":
                    rMX = 0.9783;
                    break;
                case "IBS":
                    rMX = 0.96875;
                    break;
                case "IDR":
                    rMX = 0.95522;
                    break;
                case "G.733":
                    rMX = 0.99481;
                    break;
                case "G.732":
                    rMX = 0.96875;
                    break;
                default:
                    rMX = 1;
                    break;
            }

            switch (modulation)
            {
                case "ФМ-2":
                    rMOD = 1;
                    break;
                case "ФМ-4":
                    rMOD = 2;
                    break;
                case "ФМ-8":
                    rMOD = 2;
                    break;
                case "КАМ-8":
                    rMOD = 3;
                    break;
                case "ФМ-16":
                    rMOD = 3;
                    break;
                case "КАМ-16":
                    rMOD = 3;
                    break;
                case "АФМ-16":
                    rMOD = 4;
                    break;
                case "АФМ-32":
                    rMOD = 4;
                    break;
                case "КАМ-32":
                    rMOD = 5;
                    break;
                case "КАМ-64":
                    rMOD = 6;
                    break;
                default:
                    rMOD = 2;
                    break;
            }
            return Math.Round(Convert.ToDouble(Bod) * rMOD * rMX * r1 * r2, 2);
        }

        public void FillBottomControls()
        {
            DataTable Loading = dataBase.SimpleData("Loading");

            Модуляция.Items.Clear();

            foreach (String st in Loading.Rows.Cast<DataRow>().Where(x => x["Модуляция"].ToString() != "").Select(x => x["Модуляция"].ToString()).OrderBy(x => x).Distinct().ToList())
            {
                Модуляция.Items.Add(st);
            }

            ВидДоступа.Items.Clear();
            ВидДоступа.Items.Add("МДВР");
            ВидДоступа.Items.Add("МДЧР");
            ВидДоступа.Items.Add("МДКР");



            ПУК.Items.Clear();
            foreach (String st in Loading.Rows.Cast<DataRow>().Where(x => x["ПУК"].ToString() != "").Select(x => x["ПУК"].ToString()).OrderBy(x => x).OrderBy(x => x).Distinct().ToList())
            {
                ПУК.Items.Add(st);
            }


            RПУК.Items.Clear();
            foreach (String st in Loading.Rows.Cast<DataRow>().Where(x => x["RПУК"].ToString() != "").Select(x => x["RПУК"].ToString()).OrderBy(x => x).Distinct().ToList())
            {
                RПУК.Items.Add(st);
            }


            СкрембВнеш.Items.Clear();
            foreach (String st in Loading.Rows.Cast<DataRow>().Where(x => x["СкрембВнеш"].ToString() != "").Select(x => x["СкрембВнеш"].ToString()).OrderBy(x => x).Distinct().ToList())
            {
                СкрембВнеш.Items.Add(st);
            }


            СкрембВнутр.Items.Clear();
            foreach (String st in Loading.Rows.Cast<DataRow>().Where(x => x["СкрембВнутр"].ToString() != "").Select(x => x["СкрембВнутр"].ToString()).OrderBy(x => x).Distinct().ToList())
            {
                СкрембВнутр.Items.Add(st);
            }



            ПУККаскад.Items.Clear();
            foreach (String st in Loading.Rows.Cast<DataRow>().Where(x => x["ПУККаскад"].ToString() != "").Select(x => x["ПУККаскад"].ToString()).OrderBy(x => x).Distinct().ToList())
            {
                ПУККаскад.Items.Add(st);
            }


            СистемаСвязи.Items.Clear();
            foreach (String st in Loading.Rows.Cast<DataRow>().Where(x => x["СистемаСвязи"].ToString() != "").Select(x => x["СистемаСвязи"].ToString()).OrderBy(x => x).Distinct().ToList())
            {
                СистемаСвязи.Items.Add(st);
            }

            Ценность.Items.Clear();
            Ценность.Items.Add("Отсутствует");
            Ценность.Items.Add("Оперативная");
            Ценность.Items.Add("Техническая");
            Ценность.Items.Add("Оперативно-техническая");
            Ценность.Items.Add("На задании");

            Наблюдение.Items.Clear();
            Наблюдение.Items.Add("Периодическое");
            Наблюдение.Items.Add("Контрольное");
            Наблюдение.Items.Add("Непрерывное");

            СостояниеАн.Items.Clear();
            СостояниеАн.Items.Add("Завершен");
            СостояниеАн.Items.Add("В стадии ДТА");
            СостояниеАн.Items.Add("Анализ срочно");

            Загруженность.Items.Clear();
            Загруженность.Items.Add("0-33 %");
            Загруженность.Items.Add("34-66 %");
            Загруженность.Items.Add("67-100 %");

            ТипДанных.Items.Clear();
            foreach (String st in Loading.Rows.Cast<DataRow>().Distinct().Where(x => x["ТипДанных"].ToString() != "").Select(x => x["ТипДанных"].ToString()).OrderBy(x => x).Distinct().ToList())
            {
                ТипДанных.Items.Add(st);
            }


            Оборудование.Items.Clear();

            foreach (String st in Loading.Rows.Cast<DataRow>().Distinct().Where(x => x["Оборудование"].ToString() != "").Select(x => x["Оборудование"].ToString()).OrderBy(x => x).Distinct().ToList())
            {
                Оборудование.Items.Add(st);
            }
        }

        private void RefreshDGV()
        {
            foreach (DataGridViewRow data in dgv.Rows)
            {
                if ((bool)data.Cells["Состояние"].Value == true)
                {
                    data.Cells["Частота"].Style.BackColor = Supports.Green;
                    data.Cells["Частота"].Style.ForeColor = Color.Black;
                    data.Cells["Частота"].Style.SelectionBackColor = Color.DarkGreen;
                }
                else
                {
                    data.Cells["Частота"].Style.BackColor = Supports.Red;
                    data.Cells["Частота"].Style.SelectionBackColor = Color.DarkRed;
                    data.Cells["Частота"].Style.ForeColor = Color.Black;
                }

                switch (data.Cells["ТипДанных"].Value.ToString())
                {
                    case "MPEG-2":
                        data.Cells["ТипДанных"].Style.BackColor = Color.Aquamarine;
                        data.Cells["ТипДанных"].Style.ForeColor = Color.Black;
                        data.Cells["ТипДанных"].Style.SelectionBackColor = Color.Aquamarine;
                        data.Cells["ТипДанных"].Style.SelectionForeColor = Color.Black;

                        data.Cells["СтекПрот"].Style.BackColor = Color.Aquamarine;
                        data.Cells["СтекПрот"].Style.ForeColor = Color.Black;
                        data.Cells["СтекПрот"].Style.BackColor = Color.Aquamarine;
                        data.Cells["СтекПрот"].Style.ForeColor = Color.Black;

                        data.Cells["Примечание"].Style.BackColor = Color.Aquamarine;
                        data.Cells["Примечание"].Style.ForeColor = Color.Black;
                        data.Cells["Примечание"].Style.BackColor = Color.Aquamarine;
                        data.Cells["Примечание"].Style.ForeColor = Color.Black;
                        break;
                    case "ОКС":
                        data.Cells["ТипДанных"].Style.BackColor = Color.LightYellow;
                        data.Cells["ТипДанных"].Style.ForeColor = Color.Black;

                        data.Cells["СтекПрот"].Style.BackColor = Color.LightYellow;
                        data.Cells["СтекПрот"].Style.ForeColor = Color.Black;

                        data.Cells["Примечание"].Style.BackColor = Color.LightYellow;
                        data.Cells["Примечание"].Style.ForeColor = Color.Black;

                        data.Cells["ТипДанных"].Style.SelectionBackColor = Color.LightYellow;
                        data.Cells["ТипДанных"].Style.SelectionForeColor = Color.Black;

                        data.Cells["СтекПрот"].Style.SelectionBackColor = Color.LightYellow;
                        data.Cells["СтекПрот"].Style.SelectionForeColor = Color.Black;

                        data.Cells["Примечание"].Style.SelectionBackColor = Color.LightYellow;
                        data.Cells["Примечание"].Style.SelectionForeColor = Color.Black;
                        break;
                    case "Шифрование":
                        data.Cells["ТипДанных"].Style.BackColor = Color.Orange;
                        data.Cells["ТипДанных"].Style.ForeColor = Color.Black;

                        data.Cells["СтекПрот"].Style.BackColor = Color.Orange;
                        data.Cells["СтекПрот"].Style.ForeColor = Color.Black;

                        data.Cells["Примечание"].Style.BackColor = Color.Orange;
                        data.Cells["Примечание"].Style.ForeColor = Color.Black;

                        data.Cells["ТипДанных"].Style.SelectionBackColor = Color.Orange;
                        data.Cells["ТипДанных"].Style.SelectionForeColor = Color.Black;

                        data.Cells["СтекПрот"].Style.SelectionBackColor = Color.Orange;
                        data.Cells["СтекПрот"].Style.SelectionForeColor = Color.Black;

                        data.Cells["Примечание"].Style.SelectionBackColor = Color.Orange;
                        data.Cells["Примечание"].Style.SelectionForeColor = Color.Black;
                        break;
                    case "ПДКП":
                        data.Cells["ТипДанных"].Style.BackColor = Color.LightGreen;
                        data.Cells["ТипДанных"].Style.ForeColor = Color.Black;

                        data.Cells["СтекПрот"].Style.BackColor = Color.LightGreen;
                        data.Cells["СтекПрот"].Style.ForeColor = Color.Black;

                        data.Cells["Примечание"].Style.BackColor = Color.LightGreen;
                        data.Cells["Примечание"].Style.ForeColor = Color.Black;

                        data.Cells["ТипДанных"].Style.SelectionBackColor = Color.LightGreen;
                        data.Cells["ТипДанных"].Style.SelectionForeColor = Color.Black;

                        data.Cells["СтекПрот"].Style.SelectionBackColor = Color.LightGreen;
                        data.Cells["СтекПрот"].Style.SelectionForeColor = Color.Black;

                        data.Cells["Примечание"].Style.SelectionBackColor = Color.LightGreen;
                        data.Cells["Примечание"].Style.SelectionForeColor = Color.Black;
                        break;
                    case "ADPCM":
                        data.Cells["ТипДанных"].Style.BackColor = Color.Gray;
                        data.Cells["ТипДанных"].Style.ForeColor = Color.Black;

                        data.Cells["СтекПрот"].Style.BackColor = Color.Gray;
                        data.Cells["СтекПрот"].Style.ForeColor = Color.Black;

                        data.Cells["Примечание"].Style.BackColor = Color.Gray;
                        data.Cells["Примечание"].Style.ForeColor = Color.Black;

                        data.Cells["ТипДанных"].Style.SelectionBackColor = Color.Gray;
                        data.Cells["ТипДанных"].Style.SelectionForeColor = Color.Black;

                        data.Cells["СтекПрот"].Style.SelectionBackColor = Color.Gray;
                        data.Cells["СтекПрот"].Style.SelectionForeColor = Color.Black;

                        data.Cells["Примечание"].Style.SelectionBackColor = Color.Gray;
                        data.Cells["Примечание"].Style.SelectionForeColor = Color.Black;
                        break;
                    case "MX":
                        data.Cells["ТипДанных"].Style.BackColor = Color.Pink;
                        data.Cells["ТипДанных"].Style.ForeColor = Color.Black;

                        data.Cells["СтекПрот"].Style.BackColor = Color.Pink;
                        data.Cells["СтекПрот"].Style.ForeColor = Color.Black;

                        data.Cells["Примечание"].Style.BackColor = Color.Pink;
                        data.Cells["Примечание"].Style.ForeColor = Color.Black;

                        data.Cells["ТипДанных"].Style.SelectionBackColor = Color.Pink;
                        data.Cells["ТипДанных"].Style.SelectionForeColor = Color.Black;

                        data.Cells["СтекПрот"].Style.SelectionBackColor = Color.Pink;
                        data.Cells["СтекПрот"].Style.SelectionForeColor = Color.Black;

                        data.Cells["Примечание"].Style.SelectionBackColor = Color.Pink;
                        data.Cells["Примечание"].Style.SelectionForeColor = Color.Black;
                        break;
                    case "Поле 1":
                        data.Cells["ТипДанных"].Style.BackColor = Color.Violet;
                        data.Cells["ТипДанных"].Style.ForeColor = Color.Black;

                        data.Cells["СтекПрот"].Style.BackColor = Color.Violet;
                        data.Cells["СтекПрот"].Style.ForeColor = Color.Black;

                        data.Cells["Примечание"].Style.BackColor = Color.Violet;
                        data.Cells["Примечание"].Style.ForeColor = Color.Black;

                        data.Cells["ТипДанных"].Style.SelectionBackColor = Color.Violet;
                        data.Cells["ТипДанных"].Style.SelectionForeColor = Color.Black;

                        data.Cells["СтекПрот"].Style.SelectionBackColor = Color.Violet;
                        data.Cells["СтекПрот"].Style.SelectionForeColor = Color.Black;

                        data.Cells["Примечание"].Style.SelectionBackColor = Color.Violet;
                        data.Cells["Примечание"].Style.SelectionForeColor = Color.Black;
                        break;
                    case "PCM":
                        data.Cells["ТипДанных"].Style.BackColor = Color.Blue;
                        data.Cells["ТипДанных"].Style.ForeColor = Color.Black;

                        data.Cells["СтекПрот"].Style.BackColor = Color.Blue;
                        data.Cells["СтекПрот"].Style.ForeColor = Color.Black;

                        data.Cells["Примечание"].Style.BackColor = Color.Blue;
                        data.Cells["Примечание"].Style.ForeColor = Color.Black;

                        data.Cells["ТипДанных"].Style.SelectionBackColor = Color.Blue;
                        data.Cells["ТипДанных"].Style.SelectionForeColor = Color.Black;

                        data.Cells["СтекПрот"].Style.SelectionBackColor = Color.Blue;
                        data.Cells["СтекПрот"].Style.SelectionForeColor = Color.Black;

                        data.Cells["Примечание"].Style.SelectionBackColor = Color.Blue;
                        data.Cells["Примечание"].Style.SelectionForeColor = Color.Black;
                        break;
                    case "GSM":
                        data.Cells["ТипДанных"].Style.BackColor = Color.Tomato;
                        data.Cells["ТипДанных"].Style.ForeColor = Color.Black;

                        data.Cells["СтекПрот"].Style.BackColor = Color.Tomato;
                        data.Cells["СтекПрот"].Style.ForeColor = Color.Black;

                        data.Cells["Примечание"].Style.BackColor = Color.Tomato;
                        data.Cells["Примечание"].Style.ForeColor = Color.Black;

                        data.Cells["ТипДанных"].Style.SelectionBackColor = Color.Tomato;
                        data.Cells["ТипДанных"].Style.SelectionForeColor = Color.Black;

                        data.Cells["СтекПрот"].Style.SelectionBackColor = Color.Tomato;
                        data.Cells["СтекПрот"].Style.SelectionForeColor = Color.Black;

                        data.Cells["Примечание"].Style.SelectionBackColor = Color.Tomato;
                        data.Cells["Примечание"].Style.SelectionForeColor = Color.Black;
                        break;
                    default:
                        data.Cells["ТипДанных"].Style.BackColor = Color.Gold;
                        data.Cells["ТипДанных"].Style.ForeColor = Color.Black;

                        data.Cells["СтекПрот"].Style.BackColor = Color.Gold;
                        data.Cells["СтекПрот"].Style.ForeColor = Color.Black;

                        data.Cells["Примечание"].Style.BackColor = Color.Gold;
                        data.Cells["Примечание"].Style.ForeColor = Color.Black;

                        data.Cells["ТипДанных"].Style.SelectionBackColor = Color.Gold;
                        data.Cells["ТипДанных"].Style.SelectionForeColor = Color.Black;

                        data.Cells["СтекПрот"].Style.SelectionBackColor = Color.Gold;
                        data.Cells["СтекПрот"].Style.SelectionForeColor = Color.Black;

                        data.Cells["Примечание"].Style.SelectionBackColor = Color.Gold;
                        data.Cells["Примечание"].Style.SelectionForeColor = Color.Black;
                        break;

                }
            }
        }

        public void AddFreeSpase(Control control)
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

        private void RefreshIfOutsideCganges()
        {
            if (checkSum != (checkSum = dataBase.CheckSum("Loading " + keys)))
            {
                long rowId = Convert.ToInt64(dgv.SelectedRows[0].Cells["ID"].Value);
                int firstCell = dgv.FirstDisplayedCell.RowIndex;
                dgv.SelectionChanged -= UpdateBottomControls;
                dataBase.ToDisplay("Loading " + keys + ' ' + filter.CurrentFilter + " ORDER BY [Частота]", dataTableName: "Loading " + keys);

                DataGridViewRow newRow = dgv.Rows.Cast<DataGridViewRow>().Where(x => x.Cells["ID"].Value.Equals(rowId)).FirstOrDefault();

                if (newRow == null)
                {
                    MessageBoxTi.Show("Кто-то удалил это излучение");
                    return;
                }
                else
                    dgv.CurrentCell = dgv[4, newRow.Index];

                dgv.SelectionChanged += UpdateBottomControls;
                dgv.FirstDisplayedScrollingRowIndex = firstCell;
            }
        }

        public void BandAndPolarizationNeedChange(string band, string polarization)
        {
            int index = Profile.tabControl1.TabPages.IndexOfKey(keys);
            string oldKeys = keys;
            bandISZ = band;
            polarizationISZ = polarization;
            keys = "WHERE Спутник = \'" + nameISZ + "\' AND Диапазон = \'" + bandISZ + "\' AND Поляризация = \'" + polarizationISZ + "\'";
            Text = "Спутник:\"" + nameISZ + "\" Диапазон:\"" + bandISZ + "\" Поляризация:\"" + polarizationISZ + "\"";
            Name = keys;
            dataBase.dataset.Tables["Loading " + oldKeys].TableName = "Loading " + keys;
            dataBase.ToDisplay("Loading " + keys + ' ' + filter.CurrentFilter + " ORDER BY [Частота]", dataTableName: "Loading " + keys);
        }
    }
}