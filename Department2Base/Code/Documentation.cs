using System;
using System.Data;
using System.Linq;
using word = Microsoft.Office.Interop.Word;
using excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;

namespace Department2Base
{
    class Documentation
    {
        /// <summary>
        /// Ежедневный отчёт для 202 поста, выгружается в Word
        /// </summary>
        /// <param name="date">
        /// Дата, для которой нужно расчитать
        /// </param>
        /// <param name="days">
        /// Если не 0, то расчитывается для периода с той даты которая указана + это количество дней
        /// </param>
        public static void Daily202Report(DateTime date, int days = 0)
        {
            string path = null;
            try
            {               
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Word files (*.docx)|*.docx";
                saveFileDialog1.Title = days == 0 ? "Отчёт за " + date.ToShortDateString() : "Отчёт за период от " + date.ToShortDateString() + " до " + date.AddDays(days).ToShortDateString();
                saveFileDialog1.FileName = days == 0 ? date.ToShortDateString() + ".docx" : date.ToShortDateString() + " - " + date.AddDays(days).ToShortDateString() + ".docx";

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    path = saveFileDialog1.FileName;
                else
                    return;
            }
            catch (Exception e)
            {
                MessageBoxTi.Show("Documentation.Daily202Report " + e.Message);
                return;
            }

            Thread t = new Thread(delegate ()
            {
                try
                {
                    Supports.GetProfileForm().ChangeState();

                    object miss = System.Reflection.Missing.Value;
                    word.Application app = null;

                    System.Data.DataTable dt = dataBase.SimpleData("[dbo].[MainSettings] left join [dbo].[SatelliteList] ON [dbo].[MainSettings].[Content] = [dbo].[SatelliteList].[НаименованиеИСЗ] WHERE [dbo].[MainSettings].[What] = 'NS2DocumentationCheckedListBox' OR [dbo].[MainSettings].[What] = 'NS2DocumentationCheckedListBox1'");

                    if (dt.Rows.Cast<DataRow>().Where(x => x["What"].ToString() == "NS2DocumentationCheckedListBox").Count() == 0)
                    {
                        MessageBoxTi.Show("Упс, не выбран ни один спутник, сообщите НС о данной ошибке");
                        return;
                    }

                    app = new word.Application();

                    Thread.Sleep(1000);
                    var doc = app.Documents.Add();

                    app.Visible = false;
                    Thread.Sleep(1000);
                    doc.PageSetup.Orientation = word.WdOrientation.wdOrientLandscape;
                    doc.PageSetup.TopMargin = 80;
                    doc.PageSetup.BottomMargin = 50;
                    doc.PageSetup.LeftMargin = 60;
                    doc.PageSetup.RightMargin = 60;
                    var par = doc.Paragraphs.Add();
                    par.Range.Text = "Доклад 202 БП";
                    par.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter;
                    par.Range.Font.Name = "Times New Roman";
                    par.Range.Font.Size = 12;
                    par = doc.Paragraphs.Add();
                    par = doc.Paragraphs.Add();
                    System.Data.DataTable MainSettings = dataBase.SimpleData("MainSettings");
                    if (MainSettings.Select().Where(x => x["What"].ToString() == "NS2DocumentationTextBox").Where(x => x["Who"].ToString() == "SatelliteForDailyReport").Count() != 0)
                        par.Range.Text = MainSettings.Select().Where(x => x["What"].ToString() == "NS2DocumentationTextBox").Where(x => x["Who"].ToString() == "SatelliteForDailyReport").FirstOrDefault()["Content"].ToString();
                    else
                        par.Range.Text = "1.    Изменения в ВЧ загрузке ССС МО США ДСЦС и ВГС, ССС МО Великобритании СКАЙНЕТ:";
                    par.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft;
                    par.Range.Font.Bold = 1;
                    par = doc.Paragraphs.Add();

                    word.Table fTable = par.Range.Tables.Add(doc.Range(doc.Content.End - 1, doc.Content.End), 4, 13, DefaultTableBehavior: miss, AutoFitBehavior: miss);
                    fTable.Borders.InsideLineStyle = word.WdLineStyle.wdLineStyleSingle;
                    fTable.Borders.OutsideLineStyle = word.WdLineStyle.wdLineStyleSingle;

                    fTable.Cell(1, 1).Merge(fTable.Cell(3, 1));
                    fTable.Cell(1, 2).Merge(fTable.Cell(3, 2));
                    fTable.Cell(1, 3).Merge(fTable.Cell(3, 3));
                    fTable.Cell(1, 4).Merge(fTable.Cell(1, 8));
                    fTable.Cell(1, 5).Merge(fTable.Cell(1, 9));
                    fTable.Cell(2, 4).Merge(fTable.Cell(3, 4));
                    fTable.Cell(2, 5).Merge(fTable.Cell(3, 5));
                    fTable.Cell(2, 6).Merge(fTable.Cell(2, 8));
                    fTable.Cell(2, 7).Merge(fTable.Cell(2, 8));
                    fTable.Cell(2, 8).Merge(fTable.Cell(2, 9));
                    fTable.Cell(2, 9).Merge(fTable.Cell(3, 13));

                    fTable.Cell(1, 1).Range.Text = "Наименование ИСЗ";
                    fTable.Cell(1, 2).Range.Text = "Межд. Номер";
                    fTable.Cell(1, 3).Range.Text = "ПСТ";
                    fTable.Cell(1, 4).Range.Text = "Излучений";
                    fTable.Cell(1, 5).Range.Text = "Излучений по системам";
                    fTable.Cell(2, 4).Range.Text = "вкл.";
                    fTable.Cell(2, 5).Range.Text = "выкл.";
                    fTable.Cell(2, 6).Range.Text = "Всего";
                    fTable.Cell(2, 7).Range.Text = "ОКС";
                    fTable.Cell(2, 8).Range.Text = "Вин-Т";
                    fTable.Cell(2, 9).Range.Text = "ГБС";
                    fTable.Cell(3, 6).Range.Text = "X";
                    fTable.Cell(3, 7).Range.Text = "Ka";
                    fTable.Cell(3, 8).Range.Text = "∑";
                    fTable.Cell(3, 9).Range.Text = "ПДКП";
                    fTable.Cell(3, 10).Range.Text = "ДТС";
                    fTable.Cell(3, 11).Range.Text = "МДЧР";
                    fTable.Cell(3, 12).Range.Text = "МДВР";
                    fTable.Range.Font.Size = 10;

                    par = doc.Paragraphs.Add();
                    par = doc.Paragraphs.Add();
                    if (MainSettings.Select().Where(x => x["What"].ToString() == "NS2DocumentationTextBox1").Where(x => x["Who"].ToString() == "SatelliteForDailyReport").Count() != 0)
                        par.Range.Text = MainSettings.Select().Where(x => x["What"].ToString() == "NS2DocumentationTextBox1").Where(x => x["Who"].ToString() == "SatelliteForDailyReport").FirstOrDefault()["Content"].ToString();
                    else
                        par.Range.Text = "2.    Изменения в ВЧ загрузке геостационарных ИСЗ связи:";
                    par = doc.Paragraphs.Add();
                    word.Table sTable = par.Range.Tables.Add(doc.Range(doc.Content.End - 1, doc.Content.End), 3, 6, DefaultTableBehavior: miss, AutoFitBehavior: miss);

                    sTable.Cell(1, 1).Merge(sTable.Cell(2, 1));
                    sTable.Cell(1, 2).Merge(sTable.Cell(2, 2));
                    sTable.Cell(1, 3).Merge(sTable.Cell(2, 3));
                    sTable.Cell(1, 4).Merge(sTable.Cell(1, 6));

                    sTable.Cell(1, 1).Range.Text = "Наименование ИСЗ";
                    sTable.Cell(1, 2).Range.Text = "Межд. Номер";
                    sTable.Cell(1, 3).Range.Text = "ПСТ";
                    sTable.Cell(1, 4).Range.Text = "Излучений";
                    sTable.Cell(2, 4).Range.Text = "вкл.";
                    sTable.Cell(2, 5).Range.Text = "выкл.";
                    sTable.Cell(2, 6).Range.Text = "Всего";
                    sTable.Range.Font.Size = 10;

                    sTable.Borders.InsideLineStyle = word.WdLineStyle.wdLineStyleSingle;
                    sTable.Borders.OutsideLineStyle = word.WdLineStyle.wdLineStyleSingle;
                    par = doc.Paragraphs.Add();
                    par = doc.Paragraphs.Add();
                    if (MainSettings.Select().Where(x => x["What"].ToString() == "NS2DocumentationTextBox2").Where(x => x["Who"].ToString() == "SatelliteForDailyReport").Count() != 0)
                        par.Range.Text = MainSettings.Select().Where(x => x["What"].ToString() == "NS2DocumentationTextBox2").Where(x => x["Who"].ToString() == "SatelliteForDailyReport").FirstOrDefault()["Content"].ToString();
                    else
                        par.Range.Text = "3.    Причины изменений в ВЧ загрузке ССС МО США ВГС  не отмечено";
                    par = doc.Paragraphs.Add();
                    par = doc.Paragraphs.Add();
                    par.Range.Text = "Оператор:  " + Profile.userName;
                    par.Range.Font.Bold = 0;
                    par = doc.Paragraphs.Add();
                    par = doc.Paragraphs.Add();
                    par.Range.Text = date.ToString("dd/MM/yyyy");
                    par.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter;

                    var j = 0;
                    string firDate = days == 0 ? (date.AddDays(-1) + new TimeSpan(9, 0, 0)).ToString() : (date + new TimeSpan(9, 0, 0)).ToString();
                    string secDate = days == 0 ? (date + new TimeSpan(9, 0, 0)).ToString() : (date + new TimeSpan(9, 0, 0)).AddDays(days).ToString();
                    fTable.Range.Bold = 0;
                    var n = dt.Rows.Cast<DataRow>().Where(x => x["What"].ToString() == "NS2DocumentationCheckedListBox").Count();
                    foreach (DataRow dr in dt.Rows.Cast<DataRow>().Where(x => x["What"].ToString() == "NS2DocumentationCheckedListBox").OrderBy(x => x["НаименованиеИСЗ"]))
                    {
                        fTable.Cell(fTable.Rows.Count, 1).Range.Text = dr["НаименованиеИСЗ"].ToString();
                        fTable.Cell(fTable.Rows.Count, 2).Range.Text = dr["МеждНомер"].ToString();
                        fTable.Cell(fTable.Rows.Count, 3).Range.Text = dr["ПСТ"].ToString();
                        fTable.Cell(fTable.Rows.Count, 4).Range.Text = dataBase.ToCount("SELECT COUNT(*) FROM SSALoading WHERE [Спутник] = '" + dr["НаименованиеИСЗ"].ToString() + "' AND [ВремяВкл] > '" + firDate + "' AND [ВремяВкл] < '" + secDate + "' AND [Состояние] = '1'").ToString();
                        fTable.Cell(fTable.Rows.Count, 5).Range.Text = dataBase.ToCount("SELECT COUNT(*) FROM SSALoading WHERE [Спутник] = '" + dr["НаименованиеИСЗ"].ToString() + "' AND [ВремяВкл] > '" + firDate + "' AND [ВремяВкл] < '" + secDate + "' AND [Состояние] = '0'").ToString();
                        fTable.Cell(fTable.Rows.Count, 6).Range.Text = dataBase.ToCount("SELECT COUNT(*) FROM Loading WHERE [Спутник] = '" + dr["НаименованиеИСЗ"].ToString() + "' AND [Диапазон] = 'X' AND [Состояние] = 'true'").ToString();
                        fTable.Cell(fTable.Rows.Count, 7).Range.Text = dataBase.ToCount("SELECT COUNT(*) FROM Loading WHERE [Спутник] = '" + dr["НаименованиеИСЗ"].ToString() + "' AND [Диапазон] = 'Ka' AND [Состояние] = 'true'").ToString();
                        fTable.Cell(fTable.Rows.Count, 8).Range.Text = (Convert.ToInt32(fTable.Cell(fTable.Rows.Count, 6).Range.Text.Replace("\r\a", string.Empty)) + Convert.ToInt32(fTable.Cell(fTable.Rows.Count, 7).Range.Text.Replace("\r\a", string.Empty))).ToString();
                        fTable.Cell(fTable.Rows.Count, 9).Range.Text = dataBase.ToCount("SELECT COUNT(*) FROM Loading WHERE [Спутник] = '" + dr["НаименованиеИСЗ"].ToString() + "' AND ([Диапазон] = 'Ka' OR [Диапазон] = 'X') AND [СистемаСвязи] != 'WIN-T' AND [ТипДанных] = 'ПДКП' AND [Состояние] = 1").ToString();
                        fTable.Cell(fTable.Rows.Count, 10).Range.Text = dataBase.ToCount("SELECT COUNT(*) FROM Loading WHERE [Спутник] = '" + dr["НаименованиеИСЗ"].ToString() + "' AND ([Диапазон] = 'Ka' OR [Диапазон] = 'X') AND [СистемаСвязи] = 'DTS' AND [Состояние] = 1").ToString();
                        fTable.Cell(fTable.Rows.Count, 11).Range.Text = dataBase.ToCount("SELECT COUNT(*) FROM Loading WHERE [Спутник] = '" + dr["НаименованиеИСЗ"].ToString() + "' AND [Диапазон] = 'Ka' AND [Состояние] = 'true' AND (([Тактовая] > '2729' AND [Тактовая] < '2731') OR ([Тактовая] > '3071' AND [Тактовая] < '3073'))").ToString();
                        fTable.Cell(fTable.Rows.Count, 12).Range.Text = dataBase.ToCount("SELECT COUNT(*) FROM Loading WHERE [Спутник] = '" + dr["НаименованиеИСЗ"].ToString() + "' AND [Диапазон] = 'Ka' AND [Состояние] = 'true' AND[ВидДоступа] = 'МДВР'").ToString();
                        fTable.Cell(fTable.Rows.Count, 13).Range.Text = dataBase.ToCount("SELECT COUNT(*) FROM Loading WHERE [Спутник] = '" + dr["НаименованиеИСЗ"].ToString() + "' AND ([Диапазон] = 'Ka' OR [Диапазон] = 'X') AND [СистемаСвязи] = 'GBS'").ToString();
                        j++;

                        if (j < n)
                            fTable.Rows.Add();
                    }

                    j = 0;
                    n = dt.Rows.Cast<DataRow>().Where(x => x["What"].ToString() == "NS2DocumentationCheckedListBox1").Count();
                    if (dt.Rows.Cast<DataRow>().Where(x => x["What"].ToString() == "NS2DocumentationCheckedListBox1").Count() != 0)
                    {
                        foreach (DataRow dr in dt.Rows.Cast<DataRow>().Where(x => x["What"].ToString() == "NS2DocumentationCheckedListBox1").OrderBy(x => x["НаименованиеИСЗ"]))
                        {
                            sTable.Cell(sTable.Rows.Count, 1).Range.Text = dr["НаименованиеИСЗ"].ToString();
                            sTable.Cell(sTable.Rows.Count, 2).Range.Text = dr["МеждНомер"].ToString();
                            sTable.Cell(sTable.Rows.Count, 3).Range.Text = dr["ПСТ"].ToString();
                            sTable.Cell(sTable.Rows.Count, 4).Range.Text = dataBase.ToCount("SELECT COUNT(*) FROM SSALoading WHERE [Спутник] = '" + dr["НаименованиеИСЗ"].ToString() + "' AND [ВремяВкл] > '" + firDate + "' AND [ВремяВкл] < '" + secDate + "' AND [Состояние] = '1' AND ([Диапазон] = 'Ka' OR [Диапазон] = 'X')").ToString();
                            sTable.Cell(sTable.Rows.Count, 5).Range.Text = dataBase.ToCount("SELECT COUNT(*) FROM SSALoading WHERE [Спутник] = '" + dr["НаименованиеИСЗ"].ToString() + "' AND [ВремяВкл] > '" + firDate + "' AND [ВремяВкл] < '" + secDate + "' AND [Состояние] = '0' AND ([Диапазон] = 'Ka' OR [Диапазон] = 'X')").ToString();
                            sTable.Cell(sTable.Rows.Count, 6).Range.Text = dataBase.ToCount("SELECT COUNT(*) FROM Loading WHERE [Спутник] = '" + dr["НаименованиеИСЗ"].ToString() + "' AND [Состояние] = 'true' AND ([Диапазон] = 'X' OR [Диапазон] = 'Ka')").ToString();
                            j++;
                            if (j < n)
                                sTable.Rows.Add();
                        }
                    }

                    fTable.Range.Paragraphs.SpaceAfter = 0;
                    sTable.Range.Paragraphs.SpaceAfter = 0;

                    fTable.Cell(1, 3).Width = 60;

                    for (var i = 4; i <= fTable.Rows.Count; i++)
                    {
                        fTable.Cell(i, 3).Width = 60;
                    }

                    fTable.Cell(1, 2).Width = 50;

                    for (var i = 4; i <= fTable.Rows.Count; i++)
                    {
                        fTable.Cell(i, 2).Width = 50;
                    }

                    fTable.AutoFitBehavior(word.WdAutoFitBehavior.wdAutoFitWindow);
                    sTable.AutoFitBehavior(word.WdAutoFitBehavior.wdAutoFitWindow);

                    fTable.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter;
                    sTable.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter;

                    app.ActiveDocument.SaveAs(path, word.WdSaveFormat.wdFormatDocumentDefault);

                    doc.Close();

                    MessageBoxTi.Show("Отчёт готов");

                    if (app != null)
                    {
                        app.Quit();
                        Marshal.FinalReleaseComObject(app);
                        Thread.CurrentThread.Abort();
                    }
                }
                catch (Exception e)
                {
                    if(e.GetType() != typeof(ThreadAbortException))
                        MessageBoxTi.Show("Documentation.Daily202Report.Thread " + e.Message);
                    return;
                }
                finally
                {
                    Supports.GetProfileForm().ChangeState(true);
                }
            });
            t.Start();
        }

        /// <summary>
        /// Отчёт для 203 поста
        /// </summary>
        /// <param name="date">
        /// Дата, для которой нужно расчитать
        /// </param>
        /// <param name="days">
        /// Если не 0, то расчитывается для периода с той даты которая указана + это количество дней
        /// </param>
        public static void TillCalledFor203Report(DateTime date, int days = 0)
        {
            string path = null;
            System.Data.DataTable dt = dataBase.SimpleData("Loading WHERE [Спутник] = 'Кореасат-5' AND [Состояние] = 'true' AND [Диапазон] = 'Ku' AND [Примечание] LIKE '%AIS%'");
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Word files (*.docx)|*.docx";
                saveFileDialog1.Title = days == 0 ? "Отчёт за " + date.ToShortDateString() : "Отчёт за период от " + date.ToShortDateString() + " до " + date.AddDays(days).ToShortDateString();
                saveFileDialog1.FileName = days == 0 ? date.ToShortDateString() + ".docx" : date.ToShortDateString() + " - " + date.AddDays(days).ToShortDateString() + ".docx";

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    path = saveFileDialog1.FileName;
                else
                    return;
            }
            catch (Exception e)
            {
                MessageBoxTi.Show("Documentation.TillCalledFor203Report " + e.Message);
                return;
            }

            Thread t = new Thread(delegate ()
            {
                try
                {
                    Supports.GetProfileForm().ChangeState();
                    object miss = System.Reflection.Missing.Value;
                    word.Application app = null;

                    app = new word.Application();

                    Thread.Sleep(1000);
                    var doc = app.Documents.Add();

                    app.Visible = false;
                    Thread.Sleep(1000);
                    doc.PageSetup.TopMargin = 80;
                    doc.PageSetup.BottomMargin = 50;
                    doc.PageSetup.LeftMargin = 60;
                    doc.PageSetup.RightMargin = 60;
                    var par = doc.Paragraphs.Add();
                    par.Range.Text = "Доклад 203 БП";
                    par.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter;
                    par.Range.Font.Name = "Times New Roman";
                    par.Range.Font.Size = 12;
                    par = doc.Paragraphs.Add();
                    par = doc.Paragraphs.Add();

                    par.Range.Text = "1.    Статистика:";

                    par = doc.Paragraphs.Add();
                    par = doc.Paragraphs.Add();

                    string firDate = days == 0 ? (date.AddDays(-1) + new TimeSpan(9, 0, 0)).ToString() : (date + new TimeSpan(9, 0, 0)).ToString();
                    string secDate = days == 0 ? (date + new TimeSpan(9, 0, 0)).ToString() : (date + new TimeSpan(9, 0, 0)).AddDays(days).ToString();

                    par.Range.Text = "Включилось:" + dataBase.ToCount("SELECT COUNT(*) FROM SSALoading WHERE [Спутник] = 'Кореасат-5' AND [ВремяВкл] > '" + firDate + "' AND [ВремяВкл] < '" + secDate + "' AND [Состояние] = '1' AND [Диапазон] = 'Ku'").ToString();
                    par.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft;
                    par = doc.Paragraphs.Add();
                    par = doc.Paragraphs.Add();

                    par.Range.Text = "Выключилось:" + dataBase.ToCount("SELECT COUNT(*) FROM SSALoading WHERE [Спутник] = 'Кореасат-5' AND [ВремяВкл] > '" + firDate + "' AND [ВремяВкл] < '" + secDate + "' AND [Состояние] = '0' AND [Диапазон] = 'Ku'").ToString();
                    par.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft;
                    par = doc.Paragraphs.Add();
                    par = doc.Paragraphs.Add();

                    par.Range.Text = "2.    Номиналы включённых излучений:";
                    par = doc.Paragraphs.Add();

                    word.Table fTable = par.Range.Tables.Add(doc.Range(doc.Content.End - 1, doc.Content.End), 2, 4, DefaultTableBehavior: miss, AutoFitBehavior: miss);
                    fTable.Borders.InsideLineStyle = word.WdLineStyle.wdLineStyleSingle;
                    fTable.Borders.OutsideLineStyle = word.WdLineStyle.wdLineStyleSingle;

                    fTable.Cell(1, 1).Range.Text = "Частота";
                    fTable.Cell(1, 2).Range.Text = "Стек протоков";
                    fTable.Cell(1, 3).Range.Text = "Время";
                    fTable.Cell(1, 4).Range.Text = "Примечание";

                    fTable.Range.Font.Size = 10;

                    par = doc.Paragraphs.Add();
                    par = doc.Paragraphs.Add();

                    fTable.Range.Bold = 0;

                    par.Range.Font.Bold = 0;
                    par = doc.Paragraphs.Add();
                    par = doc.Paragraphs.Add();
                    par.Range.Text = date.ToString("dd/MM/yyyy");
                    par.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter;

                    foreach (DataRow dr in dt.Rows.Cast<DataRow>())
                    {
                        fTable.Cell(fTable.Rows.Count, 1).Range.Text = dr["Частота"].ToString();
                        fTable.Cell(fTable.Rows.Count, 2).Range.Text = dr["СтекПрот"].ToString();
                        fTable.Cell(fTable.Rows.Count, 3).Range.Text = dr["ВремяДоб"].ToString();
                        fTable.Cell(fTable.Rows.Count, 4).Range.Text = dr["Примечание"].ToString();
                        fTable.Range.Rows.Add();
                    }

                    fTable.Range.Rows[fTable.Range.Rows.Count].Delete();

                    fTable.Range.Paragraphs.SpaceAfter = 0;

                    fTable.Columns[1].Width = 60;
                    fTable.Columns[1].Width = 60;
                    fTable.Columns[1].Width = 60;
                    fTable.Columns[1].Width = 60;

                    fTable.AutoFitBehavior(word.WdAutoFitBehavior.wdAutoFitWindow);
                    fTable.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter;

                    app.ActiveDocument.SaveAs(path, word.WdSaveFormat.wdFormatDocumentDefault);

                    doc.Close();

                    MessageBoxTi.Show("Отчёт готов");

                    if (app != null)
                    {
                        app.Quit();
                        Marshal.FinalReleaseComObject(app);

                        Thread.CurrentThread.Abort();
                    }
                    Thread.CurrentThread.Abort();
                }
                catch (Exception e)
                {
                    if (e.GetType() != typeof(ThreadAbortException))
                        MessageBoxTi.Show("Documentation.TillCalledFor203Report.Thread " + e.Message);
                        return;
                }
                finally
                {
                    Supports.GetProfileForm().ChangeState(true);
                }
            });
            t.Start();
        }

        public static void Graphics(List<DataPoint> Values, string chartName = null, string legend = null, excel.XlChartType type = excel.XlChartType.xlColumnClustered)
        {
            if (Values.Count == 0)
                return;

            try
            {
                Supports.GetProfileForm().ChangeState();
                word.Application app = new word.Application();
                app.Visible = true;
                Thread.Sleep(500);
                var doc = app.Documents.Add();

               
                word.InlineShape chartShape = doc.InlineShapes.AddOLEObject((Microsoft.Office.Core.XlChartType)type);
                chartShape.AlternativeText = "Chart1";
                word.Chart chart = chartShape.OLEFormat.Object as word.Chart;
                dynamic book = chart.ChartData.Workbook;
                //dynamic bookTable = book.Sheets[1].ListObjects("Table1");
                //bookTable.DataBodyRange.ClearContents();
                //Thread.Sleep(500);
                //excel.Chart chart =  exApp.Charts[0];
                //chart.SetSourceData(exApp.Cells.Range["A2", "E15"]);
                //exApp.Cells.Range["A2", "E5"].Clear();
                //sheert.ListObjects.Item[1].Resize(sheert.Range["A1", "B" + (Values.Count + 1).ToString()]);
                //sheert.Cells[1, 2] = legend;

                for (var i = 2; i < Values.Count + 2; i++)
                {
                    //exApp.Cells[i, 1] = DateTime.FromOADate(Values[i - 2].XValue);
                    //exApp.Cells[i, 2] = Values[i - 2].YValues[0];
                }
            }
            catch (Exception e)
            {
                MessageBoxTi.Show("Documentation.Graphics " + e.Message);
                return;
            }
            finally
            {
                Supports.GetProfileForm().ChangeState(true);
            }

        }

        public static void Graphics1(List<DataPoint> Values, string chartName = null, string legend = null, excel.XlChartType type = excel.XlChartType.xlColumnClustered)
        {
            if (Values.Count == 0)
                return;

            try
            {
                Supports.GetProfileForm().ChangeState();
                word.Application app = new word.Application();
                app.Visible = true;
                Thread.Sleep(500);
                var doc = app.Documents.Add();
                word.InlineShape chartShape = doc.InlineShapes.AddChart2(212);
                chartShape.Chart.ChartTitle.Text = chartName;
                chartShape.Chart.ChartType = (Microsoft.Office.Core.XlChartType)type;
                excel.Workbook workbook = chartShape.Chart.ChartData.Workbook;
                excel.Worksheet sheert = workbook.Worksheets[1];
                sheert.Cells.Range["A2", "E5"].Clear();
                sheert.ListObjects.Item[1].Resize(sheert.Range["A1", "B" + (Values.Count + 1).ToString()]);
                sheert.Cells[1, 2] = legend;

                for (var i = 2; i < Values.Count + 2; i++)
                {
                    sheert.Cells[i, 1] = DateTime.FromOADate(Values[i - 2].XValue);
                    sheert.Cells[i, 2] = Values[i - 2].YValues[0];
                }
            }
            catch (Exception e)
            {
                MessageBoxTi.Show("Documentation.Graphics " + e.Message);
                return;
            }
            finally
            {
                Supports.GetProfileForm().ChangeState(true);
            }
        }

        public static void WorkingHistory(string satellite, string beam, string polarization, DateTime firstDate, int days = 0)
        {
            string path = null;
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Word files (*.docx)|*.docx";
                saveFileDialog1.Title = days == 0 ? "Отчёт за " + firstDate.ToShortDateString() : "Отчёт за период от " + firstDate.ToShortDateString() + " до " + firstDate.AddDays(days).ToShortDateString();
                saveFileDialog1.FileName = days == 0 ? firstDate.ToShortDateString() + ".docx" : firstDate.ToShortDateString() + " - " + firstDate.AddDays(days).ToShortDateString() + ".docx";

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    path = saveFileDialog1.FileName;
                else
                    return;
            }
            catch (Exception e)
            {
                MessageBoxTi.Show("Documentation.WorkingHistory " + e.Message);
                return;
            }

            Thread t = new Thread(delegate ()
            {
                try
                {
                    Supports.GetProfileForm().ChangeState();
                    object miss = System.Reflection.Missing.Value;
                    word.Application app = null;

                    app = new word.Application();

                    Thread.Sleep(1000);
                    var doc = app.Documents.Add();

                    app.Visible = false;
                    Thread.Sleep(1000);
                    doc.PageSetup.TopMargin = 80;
                    doc.PageSetup.BottomMargin = 50;
                    doc.PageSetup.LeftMargin = 60;
                    doc.PageSetup.RightMargin = 60;
                    var par = doc.Paragraphs.Add();
                    par = doc.Paragraphs.Add();
                    par.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter;
                    par.Range.Font.Name = "Times New Roman";
                    par.Range.Font.Size = 12;


                    par.Range.Text = "История включений Спутника " + satellite + ", " + beam + " - диапазона, " + polarization + " - поляризации";
                    par = doc.Paragraphs.Add();

                    word.Table fTable = par.Range.Tables.Add(doc.Range(doc.Content.End - 1, doc.Content.End), 2, 4, DefaultTableBehavior: miss, AutoFitBehavior: miss);
                    fTable.Borders.InsideLineStyle = word.WdLineStyle.wdLineStyleSingle;
                    fTable.Borders.OutsideLineStyle = word.WdLineStyle.wdLineStyleSingle;

                    fTable.Cell(1, 1).Range.Text = "Дата";
                    fTable.Cell(1, 2).Range.Text = "Включилось";
                    fTable.Cell(1, 3).Range.Text = "Выключилось";
                    fTable.Cell(1, 4).Range.Text = "Всего";

                    fTable.Range.Font.Size = 10;

                    par = doc.Paragraphs.Add();
                    par = doc.Paragraphs.Add();

                    fTable.Range.Bold = 0;

                    par.Range.Font.Bold = 0;
                    par = doc.Paragraphs.Add();
                    par = doc.Paragraphs.Add();
                    par.Range.Text = firstDate.ToString("dd/MM/yyyy");
                    par.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter;

                    for(var i = 0; i < days; i++)
                    {
                        List<object> counts = dataBase.SingleRow("SELECT (SELECT COUNT(*) FROM SSALoading WHERE [Спутник] = '" + satellite +"' AND [Диапазон] = '" + beam +"' AND [Поляризация] = '" + polarization + "' AND [Состояние] = 1 AND [ВремяВкл] > '" + firstDate.Date.AddDays(i) + "' AND [ВремяВкл] < '" + firstDate.Date.AddDays(i + 1) + "') as [ON], " +
                                                                 "(SELECT COUNT(*) FROM SSALoading WHERE [Спутник] = '" + satellite + "' AND [Диапазон] = '" + beam + "' AND [Поляризация] = '" + polarization + "' AND [Состояние] = 0 AND[ВремяВкл] > '" + firstDate.Date.AddDays(i) + "' AND [ВремяВкл] < '" + firstDate.Date.AddDays(i + 1) + "') as [OFF]," +
                                                                 "((SELECT COUNT(*) FROM SSALoading WHERE [Спутник] = '" + satellite + "' AND [Диапазон] = '" + beam + "' AND [Поляризация] = '" + polarization + "' AND [Состояние] = 0 AND[ВремяВкл] > '" + firstDate.Date.AddDays(i) + "' AND[ВремяВкл] < GETDATE()) - " +
                                                                 "(SELECT COUNT(*) FROM SSALoading WHERE [Спутник] = '" + satellite + "' AND [Диапазон] = '" + beam + "' AND [Поляризация] = '" + polarization + "' AND [Состояние] = 1 AND[ВремяВкл] > '" + firstDate.Date.AddDays(i) + "' AND[ВремяВкл] < GETDATE()) + " +
                                                                 "(SELECT COUNT(*) FROM Loading WHERE [Спутник] = '" + satellite + "' AND [Диапазон] = '" + beam + "' AND [Поляризация] = '" + polarization + "' AND [Состояние] = 1)) as [ALL]");
                        fTable.Cell(fTable.Rows.Count, 1).Range.Text = firstDate.AddDays(i).ToShortDateString();
                        fTable.Cell(fTable.Rows.Count, 2).Range.Text = counts[0].ToString();
                        fTable.Cell(fTable.Rows.Count, 3).Range.Text = counts[1].ToString();
                        fTable.Cell(fTable.Rows.Count, 4).Range.Text = counts[2].ToString();
                        fTable.Range.Rows.Add();
                    }

                    fTable.Range.Rows[fTable.Range.Rows.Count].Delete();

                    fTable.Range.Paragraphs.SpaceAfter = 0;

                    fTable.Columns[1].Width = 60;
                    fTable.Columns[1].Width = 60;
                    fTable.Columns[1].Width = 60;
                    fTable.Columns[1].Width = 60;

                    fTable.AutoFitBehavior(word.WdAutoFitBehavior.wdAutoFitWindow);
                    fTable.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter;

                    app.ActiveDocument.SaveAs(path, word.WdSaveFormat.wdFormatDocumentDefault);

                    doc.Close();

                    MessageBoxTi.Show("Отчёт готов");

                    if (app != null)
                    {
                        app.Quit();
                        Marshal.FinalReleaseComObject(app);

                        Thread.CurrentThread.Abort();
                    }
                    Thread.CurrentThread.Abort();
                }
                catch (Exception e)
                {
                    if (e.GetType() != typeof(ThreadAbortException))
                        MessageBoxTi.Show("Documentation.WorkingHistory.Thread " + e.Message);
                        return;
                }
                finally
                {
                    Supports.GetProfileForm().ChangeState(true);
                }
            });
            t.Start();
        }

        public static void WorkingHistoryMinMax(string satellite, string beam, string polarization, DateTime firstDateSet, DateTime secondDateSet)
        {
            string path = null;
            DateTime firstDate = new DateTime(firstDateSet.Year, firstDateSet.Month, 1);
            DateTime secondDate = new DateTime(secondDateSet.Year, secondDateSet.Month, 1);
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Word files (*.docx)|*.docx";
                saveFileDialog1.Title = "Отчёт за период от " + firstDate.ToShortDateString() + " до " + secondDate.ToShortDateString();
                saveFileDialog1.FileName = firstDate.ToShortDateString() + " - " + secondDate.ToShortDateString() + ".docx";

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    path = saveFileDialog1.FileName;
                else
                    return;
            }
            catch (Exception e)
            {
                MessageBoxTi.Show("Documentation.WorkingHistory " + e.Message);
                return;
            }

            Thread t = new Thread(delegate ()
            {
                try
                {
                    Supports.GetProfileForm().ChangeState();
                    object miss = System.Reflection.Missing.Value;
                    word.Application app = null;

                    app = new word.Application();

                    Thread.Sleep(1000);
                    var doc = app.Documents.Add();

                    app.Visible = false;
                    Thread.Sleep(1000);
                    doc.PageSetup.TopMargin = 80;
                    doc.PageSetup.BottomMargin = 50;
                    doc.PageSetup.LeftMargin = 60;
                    doc.PageSetup.RightMargin = 60;
                    var par = doc.Paragraphs.Add();
                    par = doc.Paragraphs.Add();
                    par.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter;
                    par.Range.Font.Name = "Times New Roman";
                    par.Range.Font.Size = 12;


                    par.Range.Text = "История включений Спутника " + satellite + ", " + beam + " - диапазона, " + polarization + " - поляризации";
                    par = doc.Paragraphs.Add();

                    word.Table fTable = par.Range.Tables.Add(doc.Range(doc.Content.End - 1, doc.Content.End), 2, 3, DefaultTableBehavior: miss, AutoFitBehavior: miss);
                    fTable.Borders.InsideLineStyle = word.WdLineStyle.wdLineStyleSingle;
                    fTable.Borders.OutsideLineStyle = word.WdLineStyle.wdLineStyleSingle;

                    fTable.Cell(1, 1).Range.Text = "Минимум";
                    fTable.Cell(1, 2).Range.Text = "Максимум";
                    fTable.Cell(1, 3).Range.Text = "Месяц";

                    fTable.Range.Font.Size = 10;

                    par = doc.Paragraphs.Add();
                    par = doc.Paragraphs.Add();

                    fTable.Range.Bold = 0;

                    par.Range.Font.Bold = 0;
                    par = doc.Paragraphs.Add();
                    par = doc.Paragraphs.Add();
                    par.Range.Text = firstDate.ToString("dd/MM/yyyy");
                    par.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter;

                    List<int> oneMonth = new List<int>();
                    int prevMonth = firstDate.Month;

                    for (var i = 0; i < (secondDate.AddMonths(1) - firstDate).TotalDays; i++)
                    {
                        if(prevMonth == firstDate.AddDays(i).Month)
                        {
                            oneMonth.Add(Convert.ToInt32(dataBase.ToCount("SELECT ((SELECT COUNT(*) FROM SSALoading WHERE [Спутник] = '" + satellite + "' AND [Диапазон] = '" + beam + "' AND [Поляризация] = '" + polarization + "' AND [Состояние] = 0 AND[ВремяВкл] > '" + firstDate.Date.AddDays(i) + "' AND[ВремяВкл] < GETDATE()) - " +
                                                                 "(SELECT COUNT(*) FROM SSALoading WHERE [Спутник] = '" + satellite + "' AND [Диапазон] = '" + beam + "' AND [Поляризация] = '" + polarization + "' AND [Состояние] = 1 AND[ВремяВкл] > '" + firstDate.Date.AddDays(i) + "' AND[ВремяВкл] < GETDATE()) + " +
                                                                 "(SELECT COUNT(*) FROM Loading WHERE [Спутник] = '" + satellite + "' AND [Диапазон] = '" + beam + "' AND [Поляризация] = '" + polarization + "' AND [Состояние] = 1)) as [ALL]")));
                        }
                        else
                        {
                            fTable.Cell(fTable.Rows.Count, 1).Range.Text = oneMonth.Min().ToString();
                            fTable.Cell(fTable.Rows.Count, 2).Range.Text = oneMonth.Max().ToString();
                            fTable.Cell(fTable.Rows.Count, 3).Range.Text = firstDate.AddDays(i).ToString("MM/yyyy");
                            fTable.Range.Rows.Add();
                            oneMonth.Clear();
                            oneMonth.Add(Convert.ToInt32(dataBase.ToCount("SELECT ((SELECT COUNT(*) FROM SSALoading WHERE [Спутник] = '" + satellite + "' AND [Диапазон] = '" + beam + "' AND [Поляризация] = '" + polarization + "' AND [Состояние] = 0 AND[ВремяВкл] > '" + firstDate.Date.AddDays(i) + "' AND[ВремяВкл] < GETDATE()) - " +
                                                                 "(SELECT COUNT(*) FROM SSALoading WHERE [Спутник] = '" + satellite + "' AND [Диапазон] = '" + beam + "' AND [Поляризация] = '" + polarization + "' AND [Состояние] = 1 AND[ВремяВкл] > '" + firstDate.Date.AddDays(i) + "' AND[ВремяВкл] < GETDATE()) + " +
                                                                 "(SELECT COUNT(*) FROM Loading WHERE [Спутник] = '" + satellite + "' AND [Диапазон] = '" + beam + "' AND [Поляризация] = '" + polarization + "' AND [Состояние] = 1)) as [ALL]")));
                        }
                        prevMonth = firstDate.AddDays(i).Month;             
                    }

                    fTable.Range.Rows[fTable.Range.Rows.Count].Delete();

                    fTable.Range.Paragraphs.SpaceAfter = 0;

                    fTable.Columns[1].Width = 60;
                    fTable.Columns[1].Width = 60;
                    fTable.Columns[1].Width = 60;
                    fTable.Columns[1].Width = 60;

                    fTable.AutoFitBehavior(word.WdAutoFitBehavior.wdAutoFitWindow);
                    fTable.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter;

                    app.ActiveDocument.SaveAs(path, word.WdSaveFormat.wdFormatDocumentDefault);

                    doc.Close();

                    MessageBoxTi.Show("Отчёт готов");

                    if (app != null)
                    {
                        app.Quit();
                        Marshal.FinalReleaseComObject(app);

                        Thread.CurrentThread.Abort();
                    }
                    Thread.CurrentThread.Abort();
                }
                catch (Exception e)
                {
                    if (e.GetType() != typeof(ThreadAbortException))
                        MessageBoxTi.Show("Documentation.WorkingHistory.Thread " + e.Message);
                    return;
                }
                finally
                {
                    Supports.GetProfileForm().ChangeState(true);
                }
            });
            t.Start();
        }
    }
}