using System;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Data;
using System.Threading;

namespace Department2Base
{
    public class Supports
    {
        public static bool theme = false;

        public static Color headGrey = ColorSetLite.headGrey;
        public static Color headliteGrey = ColorSetLite.headliteGrey;
        public static Color textBlack = ColorSetLite.textBlack;
        public static Color darkBlue = ColorSetLite.darkBlue;
        public static Color backBlack = ColorSetLite.backBlack;
        public static Color textGray = ColorSetLite.textGray;
        public static Color liteTextGray = ColorSetLite.liteTextGray;
        public static Color headBlue = ColorSetLite.headBlue;
        public static Color headOrange = ColorSetLite.headOrange;
        public static Color textWhite = ColorSetLite.textWhite;
        public static Color groupGrey = ColorSetLite.groupGrey;
        public static Color Red = ColorSetLite.Red;
        public static Color Green = ColorSetLite.Green;
        public static Color Yellow = ColorSetLite.Yellow;
        public static Color LiteHeadBlue = ColorSetLite.LiteHeadBlue;
        public static Color LiteTextBlue = ColorSetLite.LiteTextBlue;
        public static Pen StringPen = Pens.Black;
        public static Brush StringBrush = Brushes.Black;

        public enum Departments
        {
            Первый_отдел = 1,
            Второй_отдел = 2,
            Третий_отдел = 3,
            Четвертый_отдел = 4,
            Пятый_отдел = 5,
            Шестое_отделение = 6,
            Седьмой_отдел = 7,
            Узел_связи = 8,
        }

        public enum Ranks
        {
            Рядовой = 1,
            Ефрейтор = 2,
            Мл_Сержант = 3,
            Сержант = 4,
            Ст_Сержант = 5,
            Старшина = 6,
            Прапорщик = 7,
            Ст_Прапорщик = 8,
            Мл_Лейтенант = 9,
            Лейтенант = 10,
            Ст_Лейтенант = 11,
            Капитан = 12,
            Майор = 13,
            Подполковник = 14,
            Полковник = 15,
        }

        /// <summary>
        /// Цветовой набор для светлой темы
        /// </summary>
        public struct ColorSetLite
        {
            public static Color headGrey = Color.FromArgb(214, 219, 233);
            public static Color headliteGrey = Color.FromArgb(214, 219, 233);
            public static Color textBlack = Color.FromArgb(230, 231, 232);
            public static Color backBlack = Color.FromArgb(255, 255, 255);
            public static Color textGray = Color.FromArgb(255, 255, 255);
            public static Color liteTextGray = Color.FromArgb(255, 255, 255);
            public static Color textWhite = Color.Black;
            public static Color groupGrey = Color.FromArgb(255, 255, 255);
            public static Color Red = Color.FromArgb(243, 139, 118);
            public static Color Green = Color.FromArgb(141, 210, 138);
            public static Color Yellow = Color.FromArgb(239, 242, 132);
            public static Color headBlue = Color.FromArgb(0, 122, 204);
            public static Color headOrange = Color.FromArgb(202, 81, 0);
            public static Color LiteHeadBlue = Color.FromArgb(28, 151, 234);
            public static Color LiteTextBlue = Color.FromArgb(82, 176, 239);
            public static Color darkBlue = Color.FromArgb(14, 97, 152);
            public static Pen StringPen = Pens.Black;
            public static Brush StringBrush = Brushes.Black;
        }

        /// <summary>
        /// Цветовой набор для темной темы
        /// </summary>
        public struct ColorSetDark
        {
            public static Color headGrey = Color.FromArgb(45, 45, 48);
            public static Color headliteGrey = Color.FromArgb(63, 63, 65);
            public static Color textBlack = Color.FromArgb(40, 40, 40);
            public static Color darkBlue = Color.FromArgb(14, 97, 152);
            public static Color backBlack = Color.FromArgb(28, 28, 28);
            public static Color textGray = Color.FromArgb(51, 51, 55);
            public static Color liteTextGray = Color.FromArgb(63, 63, 70);
            public static Color headBlue = Color.FromArgb(0, 122, 204);
            public static Color headOrange = Color.FromArgb(202, 81, 0);
            public static Color textWhite = Color.White;
            public static Color groupGrey = Color.FromArgb(45, 45, 48);
            public static Color Red = Color.FromArgb(243, 139, 118);
            public static Color Green = Color.FromArgb(141, 210, 138);
            public static Color Yellow = Color.FromArgb(239, 242, 132);
            public static Color LiteHeadBlue = Color.FromArgb(28, 151, 234);
            public static Color LiteTextBlue = Color.FromArgb(82, 176, 239);
            public static Pen StringPen = Pens.White;
            public static Brush StringBrush = Brushes.White;
        }

        /// <summary>
        /// Преобразование картинки в массив байтов, например, чтобы поместить в базу данных
        /// </summary>
        /// <param name="im">
        /// Картинка, которую нужно преобразовать
        /// </param>
        /// <returns></returns>
        public byte[] ImToBy(Image im)
        {
            using (var ms = new MemoryStream())
            {
                im.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Меню при щелчке правой клавишей на DGV
        /// </summary>
        /// <param name="dgv">
        /// Тот DGV, где нужно показать менюшку
        /// </param>
        /// <param name="rowIndex">
        /// Индекс строки
        /// </param>
        /// <param name="columnIndex">
        /// Индекс столбца
        /// </param>
        /// <param name="mode">
        /// Тип DGV
        /// </param>
        public static void MenuPanel(DataGridView dgv, int rowIndex, int columnIndex, Type mode, Loading sender = null)
        {
            try
            {
                ContextMenuStrip cm = new ContextMenuStrip();

                if (rowIndex != -1 && mode == typeof(SatelliteList))
                {
                    ToolStripMenuItem[] ToolStripMenuItem1 = { new ToolStripMenuItem("&Удалить"), new ToolStripMenuItem("&Переименовать"), new ToolStripMenuItem("&Скопировать текст") };

                    cm.Items.Add(ToolStripMenuItem1[0]);
                    cm.Items.Add(ToolStripMenuItem1[1]);
                    cm.Items.Add(ToolStripMenuItem1[2]);

                    dgv.ContextMenuStrip = cm;
                    ToolStripMenuItem1[0].Click += (s1, eArg) =>
                    {
                        dgv.CurrentCell = dgv[columnIndex, rowIndex];
                        string n = dgv.Rows[rowIndex].Cells["НаименованиеИСЗ"].Value.ToString();
                        dgv.Rows.RemoveAt(rowIndex);
                        dataBase.ToDisplay("SatelliteList", onlyAdapter: true);
                        dataBase.sqlAdapter.Update(dataBase.dataset.Tables["SatelliteList"]);
                        dataBase.ToUpdate(Profile.userLogin, "Удалил запись о спутнике \"" + n + "\"");
                    };
                    ToolStripMenuItem1[1].Click += (s1, eArg) =>
                    {
                        dgv.CurrentCell = dgv[columnIndex, rowIndex];
                        string log = MessageBoxTi.Show("Переименование спутника", "Введите новое название спутника", HorizontalAlignment.Left);
                        if (log == "" || log == null)
                            return;
                        while (dataBase.dataset.Tables["SatelliteList"].Rows.Cast<DataRow>().Where(x => x["НаименованиеИСЗ"].Equals(log)).Count() != 0)
                        {
                            log = MessageBoxTi.Show("Переименование спутника", "Такой спутник уже есть. Введите другое название спутника!", HorizontalAlignment.Left);
                            if (log == "" || log == null)
                                return;
                        }

                        if (log != "" || log == null)
                        {
                            dataBase.ToUpdate(Profile.userLogin, "Переименовал спутник " + dgv.SelectedRows[0].Cells["НаименованиеИСЗ"].Value.ToString() + " в " + log);
                            dataBase.SimpleRequest("UPDATE Loading SET Спутник = '" + log + "' WHERE Спутник = '" + dgv.SelectedRows[0].Cells["НаименованиеИСЗ"].Value.ToString() + "' UPDATE SatelliteList SET [НаименованиеИСЗ] = '" + log + "' WHERE ID = '" + dgv.SelectedRows[0].Cells["ID"].Value.ToString() + "'" + " UPDATE SSALoading SET [Спутник] = '" + log + "' WHERE [Спутник] = '" + dgv.SelectedRows[0].Cells["НаименованиеИСЗ"].Value.ToString() + "'");
                            dataBase.dataset.Tables["SatelliteList"].Clear();
                            dataBase.ToDisplay("SatelliteList", onlyAdapter: true);
                            dataBase.sqlAdapter.Fill(dataBase.dataset.Tables["SatelliteList"]);
                        }
                    };
                    ToolStripMenuItem1[2].Click += (s1, eArg) =>
                    {
                        string per = dgv[columnIndex, rowIndex].Value.ToString();
                        if (per != "")
                            Clipboard.SetText(per);
                    };
                    dgv.ContextMenuStrip.Show(dgv, new Point(Cursor.Position.X - dgv.PointToScreen(new Point(0, 0)).X, Cursor.Position.Y - dgv.PointToScreen(new Point(0, 0)).Y));
                }
                else if (rowIndex != -1 && mode == typeof(Loading))
                {
                    ToolStripMenuItem[] ToolStripMenuItem1 = { new ToolStripMenuItem("&Скопировать текст"), new ToolStripMenuItem("&Добавить новое значение"), new ToolStripMenuItem("&Удалить строку") };

                    cm.Items.Add(ToolStripMenuItem1[0]);
                    cm.Items.Add(ToolStripMenuItem1[1]);
                    cm.Items.Add(ToolStripMenuItem1[2]);

                    dgv.ContextMenuStrip = cm;

                    ToolStripMenuItem1[0].Click += (s1, eArg) =>
                    {
                        string per = dgv[columnIndex, rowIndex].Value.ToString();
                        if (per != "")
                            Clipboard.SetText(per);
                    };

                    ToolStripMenuItem1[1].Click += (s1, eArg) =>
                    {
                        if (MessageBoxTi.Show(textAlign: HorizontalAlignment.Center))
                        {
                            string newVal = MessageBoxTi.Show("Новое значение", "Введите новое значение", textAlign: HorizontalAlignment.Center);
                            if (newVal == null || newVal == "")
                                return;
                            else
                            {
                                dataBase.dataset.Tables["Loading " + sender.keys].Select().Where(x => x["ID"].Equals(dgv["ID", rowIndex].Value)).FirstOrDefault()[dgv.Columns[columnIndex].Name] = newVal;
                                dataBase.sqlAdapter.Update(dataBase.dataset.Tables["Loading " + sender.keys]);
                                sender.FillBottomControls();                         
                                sender.AddFreeSpase(sender.mainPanel);
                                sender.UpdateBottomControls(sender, new EventArgs());
                                dgv.CurrentCell = dgv[columnIndex, rowIndex];
                            }                             
                        }                         
                    };

                    ToolStripMenuItem1[2].Click += (s1, eArg) =>
                    {
                        if (MessageBoxTi.Show("Удаление излучения", "Удалить данное излучение?") == MessageResult.Yes)
                        {
                            DataRow dr = dataBase.dataset.Tables["Loading " + sender.keys].Select().Where(x => x["ID"].Equals(dgv["ID", rowIndex].Value)).FirstOrDefault();

                            dataBase.SimpleRequest("INSERT INTO [dbo].[Deleted] ([Спутник],[Диапазон],[Поляризация],[Частота],[Состояние]," +
                                                   "[ОтношениеСШ],[ВидДоступа],[Модуляция],[Тактовая],[RПУК],[ПУК],[СкрембВнеш],[ПУККаскад],[СкрембВнутр]," +
                                                   "[Скорость],[СистемаСвязи],[Оборудование],[ДлинаКадра],[ТипДанных],[СтекПрот],[Примечание],[СостояниеАн],[Ценность]," +
                                                   "[Наблюдение],[ВремяДоб],[Пользователь],[Загруженность], [ВремяРедакт]) VALUES ('" + dr["Спутник"] + "', '" + dr["Диапазон"] + "', '" + dr["Поляризация"] + "', '" +
                                                   dr["Частота"] + "', '" + dr["Состояние"] + "', '" + dr["ОтношениеСШ"] + "', '" + dr["ВидДоступа"] + "', '" + dr["Модуляция"] + "', '" +
                                                   dr["Тактовая"].ToString().Replace(',', '.') + "', '" + dr["RПУК"] + "', '" + dr["ПУК"] + "', '" + dr["СкрембВнеш"] + "', '" +
                                                   dr["ПУККаскад"] + "', '" + dr["СкрембВнутр"] + "', '" + dr["Скорость"].ToString().Replace(',', '.') + "', '" + dr["СистемаСвязи"] + "', '" + dr["Оборудование"] + "', '" +
                                                   dr["ДлинаКадра"] + "', '" + dr["ТипДанных"] + "', '" + dr["СтекПрот"] + "', '" + dr["Примечание"] + "', '" + dr["СостояниеАн"] + "', '" +
                                                   dr["Ценность"] + "', '" + dr["Наблюдение"] + "', GETDATE(), '" + dr["Пользователь"] + "', '" + dr["Загруженность"] + "', '" + dr["ВремяРедакт"] + "')");

                            dataBase.ToUpdate(Profile.userLogin, "Удалил излучение спутника " + '"' + dr["Спутник"].ToString() + '"' + " с частотой: \"" + dr["Частота"].ToString() + "\". (запись перенесена в таблицу \"Удалённые\")");

                            dr.Delete();
                            dataBase.sqlAdapter.Update(dataBase.dataset.Tables["Loading " + sender.keys]);
                            sender.checkSum = dataBase.CheckSum("Loading " + sender.keys);
                        }
                    };

                    dgv.ContextMenuStrip.Show(dgv, new Point(Cursor.Position.X - dgv.PointToScreen(new Point(0, 0)).X, Cursor.Position.Y - dgv.PointToScreen(new Point(0, 0)).Y));
                }
                else if (rowIndex != -1 && mode == typeof(SSALoading))
                {

                }
                else
                {
                    ToolStripMenuItem columns = new ToolStripMenuItem("&Столбцы");
                    columns.DropDown.AutoClose = false;
                    columns.DropDown.MouseLeave += (s, e) => columns.DropDown.Close();

                    columns.DropDown.MouseLeave += (s, e) =>
                    {
                        columns.DropDown.Close();
                    };
                    ToolStripMenuItem[] ToolStripMenuItem2 = { new ToolStripMenuItem("&Свойства"), new ToolStripMenuItem("&Скрыть"), columns };
                    foreach (DataGridViewColumn col in dgv.Columns)
                    {
                        if (col.Name == "Частота") continue;
                        columns.DropDownItems.Add("&" + col.Name);
                    }
                    foreach (ToolStripMenuItem it in columns.DropDownItems)
                    {
                        it.Click += (s, args) =>
                        {
                            if ((s as ToolStripMenuItem).Checked == true)
                            {
                                (s as ToolStripMenuItem).Checked = false;
                                dgv.Columns[(s as ToolStripMenuItem).Text.Replace("&", string.Empty)].Visible = false;
                                dataBase.SimpleRequest("INSERT INTO [MainSettings] ([Who], [What], [Content]) VALUES ('LoadingColumnsVisibility' , '" + Profile.userLogin + "', '" + (s as ToolStripMenuItem).Text.Replace("&", string.Empty) + "')");
                            }
                            else
                            {
                                (s as ToolStripMenuItem).Checked = true;
                                dgv.Columns[(s as ToolStripMenuItem).Text.Replace("&", string.Empty)].Visible = true;
                                dataBase.SimpleRequest("DELETE FROM [MainSettings] WHERE [Who] = 'LoadingColumnsVisibility' AND [What] = '" + Profile.userLogin + "' AND [Content] = '" + (s as ToolStripMenuItem).Text.Replace("&", string.Empty) + "'");
                            }
                        };
                        if (dgv.Columns[it.Text.Replace("&", string.Empty)].Visible == true)
                        {
                            it.Checked = true;
                        }
                    }

                    cm.Items.Add(ToolStripMenuItem2[0]);
                    cm.Items.Add(ToolStripMenuItem2[1]);
                    cm.Items.Add(ToolStripMenuItem2[2]);
                    ToolStripMenuItem2[1].Click += (s, e) =>
                    {
                        if (dgv.Columns[columnIndex].Name != "Частота")
                        {
                            dgv.Columns[columnIndex].Visible = false;
                            if (mode == typeof(Loading))
                                dataBase.SimpleRequest("INSERT INTO [MainSettings] ([Who], [What], [Content]) VALUES ('LoadingColumnsVisibility' , '" + Profile.userLogin + "', '" + dgv.Columns[columnIndex].Name + "')");
                        }
                    };

                    dgv.ContextMenuStrip = cm;
                    dgv.ContextMenuStrip.Show(dgv, new Point(Cursor.Position.X - dgv.PointToScreen(new Point(0, 0)).X, Cursor.Position.Y - dgv.PointToScreen(new Point(0, 0)).Y));
                }
            }
            catch (Exception e)
            {
                MessageBoxTi.Show("MenuPanel " + e.Message);
            }
        }

        /// <summary>
        /// Преобразование Всех дочерних элементов в соответствии с выбранной цветовой темой.
        /// </summary>
        /// <param name="parent">
        /// Верхний контрол, все дочерние элементы будут преобразованны
        /// </param>
        public static void GangeGroup(Control parent)
        {
            try
            {
                foreach (Control gro in parent.Controls)
                {
                    if (!gro.Name.Contains("NotDraw"))
                    {
                        if (gro.GetType() == typeof(GroupBox))
                        {
                            ((GroupBox)gro).BackColor = textGray;
                            ((GroupBox)gro).ForeColor = textWhite;
                            gro.Paint += (s, args) =>
                            {
                                GroupBox box = (GroupBox)s;
                                args.Graphics.Clear(headGrey);
                                args.Graphics.DrawString(box.Text, box.Font, new SolidBrush(textWhite), 0, 0);
                            };
                        }
                        else if (gro.GetType() == typeof(ListBox))
                        {
                            ((ListBox)gro).Dock = DockStyle.Fill;
                            ((ListBox)gro).BackColor = textGray;
                            ((ListBox)gro).BorderStyle = BorderStyle.FixedSingle;
                            ((ListBox)gro).ForeColor = textWhite;
                            ((ListBox)gro).Enter += (s, args) =>
                            {
                                ((ListBox)gro).BackColor = liteTextGray;
                            };
                            ((ListBox)gro).Leave += (s, args) =>
                            {
                                ((ListBox)gro).BackColor = textGray;
                            };
                            ((ListBox)gro).MouseEnter += (s, args) =>
                            {
                                ((ListBox)gro).BackColor = liteTextGray;
                            };
                            ((ListBox)gro).MouseLeave += (s, args) =>
                            {
                                ((ListBox)gro).BackColor = textGray;
                            };
                        }
                        else if (gro.GetType() == typeof(TextBox))
                        {
                            ((TextBox)gro).ScrollBars = ScrollBars.Vertical;
                            ((TextBox)gro).BackColor = textGray;
                            ((TextBox)gro).BorderStyle = BorderStyle.FixedSingle;
                            ((TextBox)gro).ForeColor = textWhite;
                            ((TextBox)gro).Enter += (s, args) =>
                            {
                                ((TextBox)gro).BackColor = liteTextGray;
                            };
                            ((TextBox)gro).Leave += (s, args) =>
                            {
                                ((TextBox)gro).BackColor = textGray;
                            };
                            ((TextBox)gro).MouseEnter += (s, args) =>
                            {
                                ((TextBox)gro).BackColor = liteTextGray;
                            };
                            ((TextBox)gro).MouseLeave += (s, args) =>
                            {
                                ((TextBox)gro).BackColor = textGray;
                            };
                        }
                        else if (gro.GetType() == typeof(TextBoxTi))
                        {
                            ((TextBoxTi)gro).ScrollBars = ScrollBars.Vertical;
                            ((TextBoxTi)gro).BackColor = textGray;
                            ((TextBoxTi)gro).BorderStyle = BorderStyle.FixedSingle;
                            ((TextBoxTi)gro).ForeColor = textWhite;
                            ((TextBoxTi)gro).Enter += (s, args) =>
                            {
                                ((TextBoxTi)gro).BackColor = liteTextGray;
                            };
                            ((TextBoxTi)gro).Leave += (s, args) =>
                            {
                                ((TextBoxTi)gro).BackColor = textGray;
                            };
                            ((TextBoxTi)gro).MouseEnter += (s, args) =>
                            {
                                ((TextBoxTi)gro).BackColor = liteTextGray;
                            };
                            ((TextBoxTi)gro).MouseLeave += (s, args) =>
                            {
                                ((TextBoxTi)gro).BackColor = textGray;
                            };
                        }
                        else if (gro.GetType() == typeof(ComboBox))
                        {
                            ((ComboBox)gro).BackColor = textGray;
                            ((ComboBox)gro).ForeColor = textWhite;
                            ((ComboBox)gro).FlatStyle = FlatStyle.Flat;
                            ((ComboBox)gro).DropDownStyle = ComboBoxStyle.DropDownList;
                            ((ComboBox)gro).Enter += (s, args) =>
                            {
                                ((ComboBox)gro).BackColor = liteTextGray;
                            };
                            ((ComboBox)gro).Leave += (s, args) =>
                            {
                                ((ComboBox)gro).BackColor = textGray;
                            };
                            ((ComboBox)gro).MouseEnter += (s, args) =>
                            {
                                ((ComboBox)gro).BackColor = liteTextGray;
                            };
                            ((ComboBox)gro).MouseLeave += (s, args) =>
                            {
                                ((ComboBox)gro).BackColor = textGray;
                            };

                        }
                        else if (gro.GetType() == typeof(Button))
                        {
                            ((Button)gro).ForeColor = textWhite;
                            ((Button)gro).BackColor = headGrey;
                            ((Button)gro).FlatStyle = FlatStyle.Flat;
                        }
                        else if (gro.GetType() == typeof(NumericUpDown))
                        {
                            ((NumericUpDown)gro).ForeColor = textWhite;
                            ((NumericUpDown)gro).BackColor = textBlack;
                        }
                        else if (gro.GetType() == typeof(DataGridView))
                        {
                            ((DataGridView)gro).AlternatingRowsDefaultCellStyle.BackColor = textBlack;
                            ((DataGridView)gro).AlternatingRowsDefaultCellStyle.ForeColor = textWhite;
                            ((DataGridView)gro).RowHeadersDefaultCellStyle.BackColor = textBlack;
                            ((DataGridView)gro).RowHeadersDefaultCellStyle.ForeColor = textWhite;
                            ((DataGridView)gro).DefaultCellStyle.BackColor = textGray;
                            ((DataGridView)gro).DefaultCellStyle.ForeColor = textWhite;
                            ((DataGridView)gro).GridColor = headBlue;
                            ((DataGridView)gro).ColumnHeadersDefaultCellStyle.BackColor = textBlack;
                            ((DataGridView)gro).ColumnHeadersDefaultCellStyle.ForeColor = textWhite;
                            ((DataGridView)gro).BackgroundColor = textBlack;
                            ((DataGridView)gro).MouseEnter += (s, e) =>
                            {
                                ((DataGridView)gro).Focus();
                            };
                        }
                        else if (gro.GetType() == typeof(FlowLayoutPanel) || gro.GetType() == typeof(TableLayoutPanel) || gro.GetType() == typeof(Panel) || gro.GetType() == typeof(TransPanel))
                        {
                            gro.BackColor = headGrey;
                        }
                        else if (gro.GetType() == typeof(Label))
                        {
                            gro.ForeColor = textWhite;
                        }
                        else if (gro.GetType() == typeof(MenuStrip))
                        {
                            gro.ForeColor = textWhite;
                            gro.BackColor = headGrey;
                            //foreach(MenuItem mi in ((MenuStrip)gro).Items)
                            // {

                            // }
                        }
                        else if (gro.GetType() == typeof(CheckedListBox))
                        {
                            gro.ForeColor = textWhite;
                            gro.BackColor = headGrey;
                        }
                        else if (gro.GetType() == typeof(SplitContainer))
                        {
                            ((SplitContainer)gro).BackColor = headGrey;
                            ((SplitContainer)gro).Panel1.BackColor = headGrey;
                            ((SplitContainer)gro).Panel2.BackColor = headGrey;
                        }
                        else if (gro.GetType() == typeof(CheckBox))
                        {
                            gro.BackColor = headGrey;
                            gro.ForeColor = textWhite;
                        }
                        else if (gro.GetType() == typeof(PictureBox))
                        {
                            gro.BackColor = headGrey;
                        }
                        else if (gro.GetType() == typeof(ToolStrip))
                        {
                            ((ToolStrip)gro).BackColor = headGrey;
                            ((ToolStrip)gro).RenderMode = ToolStripRenderMode.System;
                            ((ToolStrip)gro).ForeColor = textWhite;
                        }
                        else if (gro.GetType() == typeof(TreeView))
                        {
                            ((TreeView)gro).BackColor = headGrey;
                            ((TreeView)gro).ForeColor = textWhite;
                        }
                        else if (gro.GetType() == typeof(Splitter))
                        {
                            ((Splitter)gro).BackColor = headliteGrey;
                            ((Splitter)gro).Width = 5;
                        }
                        else if (gro.GetType() == typeof(ChartTi))
                        {
                            ((ChartTi)gro).BackColor = headGrey;
                            ((ChartTi)gro).ForeColor = textBlack;
                            ((ChartTi)gro).chartArea.BackColor = headGrey;
                            ((ChartTi)gro).chartArea.AxisX.LineColor = textBlack;
                            ((ChartTi)gro).chartArea.AxisY.LineColor = textBlack;
                            ((ChartTi)gro).chartArea.AxisX.MajorGrid.LineColor = textBlack;
                            ((ChartTi)gro).chartArea.AxisY.MajorGrid.LineColor = textBlack;
                            ((ChartTi)gro).chartArea.AxisX.LabelStyle.ForeColor = textWhite;
                            ((ChartTi)gro).chartArea.AxisY.LabelStyle.ForeColor = textWhite;
                            ((ChartTi)gro).chartArea.AxisY.MajorGrid.LineColor = textBlack;
                            ((ChartTi)gro).chartArea.AxisX.InterlacedColor = textBlack;
                            ((ChartTi)gro).chartArea.AxisY.InterlacedColor = textBlack;
                            ((ChartTi)gro).chartArea.AxisX.TitleForeColor = textWhite;
                            ((ChartTi)gro).chartArea.AxisY.TitleForeColor = textWhite;
                            ((ChartTi)gro).legend.BackColor = headGrey;
                            ((ChartTi)gro).legend.ForeColor = textWhite;
                            ((ChartTi)gro).legend.TitleForeColor = textWhite;
                        }
                    }
                    else
                    {

                    }
                    GangeGroup(gro);

                }
            }
            catch (Exception e)
            {
                MessageBoxTi.Show("GangeGroup " + e.Message);
            }
        }

        /// <summary>
        /// Набор методов для определения присутствия пользователя за рабочим местом
        /// </summary>
        public class WorkActivity
        {
            private static Thread backWork = null;
            private static Point mouseLastStatement = Cursor.Position;
            private static int minutesForWaitForOperator = 15;
            private static double currentWaiting = 0;
            private static bool alreadyOut = false;

            public static void Start()
            {
                backWork = new Thread(delegate ()
                {
                    System.Timers.Timer eachMinute = new System.Timers.Timer();

                    eachMinute.Interval = 1000;

                    eachMinute.Elapsed += (s, e) =>
                    {
                        dataBase.SimpleRequest("UPDATE [dbo].[Login] SET [LastSeen] =  GETDATE() WHERE [Login] = '" + Profile.userLogin + "'");

                        if (!Convert.ToBoolean(dataBase.ToCount("SELECT [Allowed] FROM [dbo].[Login] WHERE [Login] = '" + Profile.userLogin + "'")))
                        {
                            Application.Exit();
                            eachMinute.Stop();
                        }


                        if (mouseLastStatement.X == Cursor.Position.X && mouseLastStatement.Y == Cursor.Position.Y)
                        {
                            mouseLastStatement = Cursor.Position;
                            currentWaiting += eachMinute.Interval;

                            if (((minutesForWaitForOperator * 60000) < currentWaiting) && alreadyOut == false)
                            {
                                alreadyOut = true;
                                dataBase.ToUpdate(Profile.userLogin, "Отошел", -minutesForWaitForOperator * 60);
                            }
                        }
                        else
                        {
                            if (currentWaiting != 0)
                                currentWaiting = 0;
                            if (mouseLastStatement != Cursor.Position)
                                mouseLastStatement = Cursor.Position;
                            if (alreadyOut != false)
                            {
                                alreadyOut = false;
                                dataBase.ToUpdate(Profile.userLogin, "Вернулся");
                            }
                        }
                    };

                    dataBase.SimpleRequest("UPDATE [dbo].[Login] SET [LastSeen] = GETDATE() WHERE [Login] = '" + Profile.userLogin + "'");
                    eachMinute.Start();
                });
                backWork.Start();

            }

            public static void Stop()
            {
                if (backWork != null)
                    backWork.Abort();
            }
        }

        /// <summary>
        /// Возвращает null, если поданый объект имеет нулевую длину
        /// </summary>
        /// <param name="param"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object NullOrObjectIfEmpty(object param, Type type = null)
        {
            try
            {
                object t = DBNull.Value;
                if (param != null)
                    t = param.ToString().Equals("") ? DBNull.Value : param;
                if (t != null && type != null)
                    t = Convert.ChangeType(t, type);
                return t;
            }
            catch (Exception e)
            {
                MessageBoxTi.Show("NullOrObjectIfEmpty " + e.Message);
                return DBNull.Value;
            }
        }

        public static Profile GetProfileForm()
        {
            foreach(Form fo in Application.OpenForms)
            {
                if (fo.GetType() == typeof(Profile))
                    return fo as Profile;
            }
            return null;
        }

        public static string DeCezarus(string encriptingString)
        {
            string decriptredString = null;

            for (var i = 0; i < encriptingString.Length; i++)          
                decriptredString += Convert.ToChar(Convert.ToInt32(encriptingString[i]) - 1);


               

            return decriptredString;
        }
    }
}