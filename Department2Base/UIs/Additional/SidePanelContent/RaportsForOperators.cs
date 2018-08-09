using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Department2Base
{
    public class RaportsForOperators : TableLayoutPanel
    {
        public RaportsForOperators()
        {
            try
            {
                RowStyles.Clear();
                ColumnStyles.Clear();
                RowStyles.Add(new RowStyle(SizeType.Absolute, 120f));
                RowStyles.Add(new RowStyle(SizeType.Absolute, 120f));
                RowStyles.Add(new RowStyle(SizeType.Absolute, 200f));
                RowStyles.Add(new RowStyle(SizeType.Absolute, 150f));

                Dock = DockStyle.Fill;

                Controls.Add(new GroupBox()
                {
                    Name = "202ReportGroupBox",
                    Dock = DockStyle.Fill,
                    Text = "Отчёт для 202",
                }, 0, 0);

                Controls.Add(new GroupBox()
                {
                    Name = "203ReportGroupBox",
                    Dock = DockStyle.Fill,
                    Text = "Отчёт для 203",
                }, 0, 1);

                Controls.Add(new GroupBox()
                {
                    Name = "HistoryGroupBox",
                    Dock = DockStyle.Fill,
                    Text = "История включений",
                }, 0, 2);

                Controls.Add(new Panel()
                {
                    Dock = DockStyle.Fill,
                }, 0, 3);

                Controls.Find("HistoryGroupBox", true).FirstOrDefault().Controls.Add(new Panel()
                {
                    Name = "HistoryPanel",
                    Dock = DockStyle.Top,
                    Text = "Получить историю",
                    Height = 25,
                });

                Controls.Find("HistoryPanel", true).FirstOrDefault().Controls.Add(new Button()
                {
                    Name = "HistoryButton",
                    Dock = DockStyle.Fill,
                    Text = "Получить историю",
                });

                Controls.Find("HistoryPanel", true).FirstOrDefault().Controls.Add(new Button()
                {
                    Name = "HistoryButton1",
                    Dock = DockStyle.Right,
                    Text = "Получить min/max",
                    Width = 120,
                });

                Controls.Find("HistoryGroupBox", true).FirstOrDefault().Controls.Add(new DateTimePicker()
                {           
                    Name = "HistoryDateTimePicker2",
                    Dock = DockStyle.Top,                    
                });

                Controls.Find("HistoryGroupBox", true).FirstOrDefault().Controls.Add(new DateTimePicker()
                {
                    Top = 5,
                    Name = "HistoryDateTimePicker1",
                    Dock = DockStyle.Top,
                });

                Controls.Find("HistoryGroupBox", true).FirstOrDefault().Controls.Add(new GroupBox()
                {
                    Name = "HistoryPolarizationGroupBox",
                    Dock = DockStyle.Top,
                    Text = "Поляризация",
                    Height = 37,
                });

                Controls.Find("HistoryGroupBox", true).FirstOrDefault().Controls.Add(new GroupBox()
                {
                    Name = "HistoryBeamGroupBox",
                    Dock = DockStyle.Top,
                    Text = "Диапазон",
                    Height = 37,
                });

                Controls.Find("HistoryGroupBox", true).FirstOrDefault().Controls.Add(new GroupBox()
                {
                    Name = "HistorySatelliteGroupBox",
                    Dock = DockStyle.Top,
                    Text = "Спутник",
                    Height = 37,
                });

                Controls.Find("HistoryPolarizationGroupBox", true).FirstOrDefault().Controls.Add(new ComboBox()
                {
                    Name = "HistoryPolarizationComboBox",
                    Dock = DockStyle.Fill,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                });
                Controls.Find("HistoryBeamGroupBox", true).FirstOrDefault().Controls.Add(new ComboBox()
                {
                    Name = "HistoryBeamComboBox",
                    Dock = DockStyle.Fill,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                });
                Controls.Find("HistorySatelliteGroupBox", true).FirstOrDefault().Controls.Add(new ComboBox()
                {
                    Name = "HistorySatelliteComboBox",
                    Dock = DockStyle.Fill,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                });


                (Controls.Find("HistoryPolarizationComboBox", true).FirstOrDefault() as ComboBox).Items.Add("L");
                (Controls.Find("HistoryPolarizationComboBox", true).FirstOrDefault() as ComboBox).Items.Add("R");
                (Controls.Find("HistoryPolarizationComboBox", true).FirstOrDefault() as ComboBox).Items.Add("V");
                (Controls.Find("HistoryPolarizationComboBox", true).FirstOrDefault() as ComboBox).Items.Add("H");

                foreach(string be in dataBase.SimpleData("FrequencyBand").Rows.Cast<DataRow>().Select(x => x["Наименование диапазона"].ToString()).OrderBy(x => x).ToList())
                    (Controls.Find("HistoryBeamComboBox", true).FirstOrDefault() as ComboBox).Items.Add(be);

                foreach (string be in dataBase.SimpleData("SatelliteList").Rows.Cast<DataRow>().Select(x => x["НаименованиеИСЗ"].ToString()).OrderBy(x => x).ToList())
                    (Controls.Find("HistorySatelliteComboBox", true).FirstOrDefault() as ComboBox).Items.Add(be);


                (Controls.Find("HistoryButton", true).FirstOrDefault() as Button).Click += (s, e) =>
                {
                    Documentation.WorkingHistory((Controls.Find("HistorySatelliteComboBox", true).FirstOrDefault() as ComboBox).SelectedItem.ToString(),
                        (Controls.Find("HistoryBeamComboBox", true).FirstOrDefault() as ComboBox).SelectedItem.ToString(),
                        (Controls.Find("HistoryPolarizationComboBox", true).FirstOrDefault() as ComboBox).SelectedItem.ToString(),
                        (Controls.Find("HistoryDateTimePicker1", true).FirstOrDefault() as DateTimePicker).Value,
                        Convert.ToInt32(((Controls.Find("HistoryDateTimePicker2", true).FirstOrDefault() as DateTimePicker).Value -
                        (Controls.Find("HistoryDateTimePicker1", true).FirstOrDefault() as DateTimePicker).Value).TotalDays));          
                };

                (Controls.Find("HistoryButton1", true).FirstOrDefault() as Button).Click += (s, e) =>
                {
                    
                    Documentation.WorkingHistoryMinMax((Controls.Find("HistorySatelliteComboBox", true).FirstOrDefault() as ComboBox).SelectedItem.ToString(),
                        (Controls.Find("HistoryBeamComboBox", true).FirstOrDefault() as ComboBox).SelectedItem.ToString(),
                        (Controls.Find("HistoryPolarizationComboBox", true).FirstOrDefault() as ComboBox).SelectedItem.ToString(),
                        (Controls.Find("HistoryDateTimePicker1", true).FirstOrDefault() as DateTimePicker).Value,
                        (Controls.Find("HistoryDateTimePicker2", true).FirstOrDefault() as DateTimePicker).Value);
                };

                Controls.Find("202ReportGroupBox", true).FirstOrDefault().Controls.Add(new FlowLayoutPanel()
                {
                    Name = "202ReportGroupBoxFlowLayoutPanel",
                    Dock = DockStyle.Fill,
                });

                Controls.Find("202ReportGroupBoxFlowLayoutPanel", true).FirstOrDefault().Controls.Add(new TableLayoutPanel()
                {
                    Name = "DailyRaport202TableLayoutPanel",
                    Height = 30,
                });

                (Controls.Find("DailyRaport202TableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).ColumnStyles.Clear();
                (Controls.Find("DailyRaport202TableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).RowStyles.Clear();
                (Controls.Find("DailyRaport202TableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 90f));
                (Controls.Find("DailyRaport202TableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10f));

                (Controls.Find("DailyRaport202TableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(new DateTimePicker()
                {
                    Name = "DailyRaport202DateTimePicker1",
                }, 0, 0);

                (Controls.Find("DailyRaport202TableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(new CheckBox()
                {
                    Name = "DailyRaport202CheckBox",
                }, 1, 0);

                Controls.Find("202ReportGroupBoxFlowLayoutPanel", true).FirstOrDefault().Controls.Add(new DateTimePicker()
                {
                    Name = "DailyRaport202DateTimePicker2",
                    Enabled = false,
                });

                (Controls.Find("DailyRaport202CheckBox", true).FirstOrDefault() as CheckBox).CheckedChanged += (s, e) =>
                {
                    try
                    {
                        if ((Controls.Find("DailyRaport202CheckBox", true).FirstOrDefault() as CheckBox).Checked)
                        {
                            (Controls.Find("DailyRaport202DateTimePicker2", true).FirstOrDefault() as DateTimePicker).Enabled = true;
                            (Controls.Find("TodayReport", true).FirstOrDefault() as Button).Text = "Отчёт за период";
                        }
                        else
                        {
                            (Controls.Find("DailyRaport202DateTimePicker2", true).FirstOrDefault() as DateTimePicker).Enabled = false;
                            (Controls.Find("TodayReport", true).FirstOrDefault() as Button).Text = "Ежедневный отчёт";
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBoxTi.Show("RaportsForOperators.DailyRaport202CheckBox " + ex.Message);
                    }
                };

                Controls.Find("202ReportGroupBoxFlowLayoutPanel", true).FirstOrDefault().Controls.Add(new Button()
                {
                    Name = "TodayReport",
                    Text = "Ежедневный отчёт",
                    AutoSize = true,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Supports.headGrey,
                    Width = 200,
                });

                (Controls.Find("TodayReport", true).FirstOrDefault() as Button).Click += (s, e) =>
                {


                    try
                    {
                        if (!(Controls.Find("DailyRaport202CheckBox", true).FirstOrDefault() as CheckBox).Checked)
                            Documentation.Daily202Report((Controls.Find("DailyRaport202DateTimePicker1", true).FirstOrDefault() as DateTimePicker).Value.Date);
                        else
                        {
                            int days = Convert.ToInt32(((Controls.Find("DailyRaport202DateTimePicker2", true).FirstOrDefault() as DateTimePicker).Value.Date - (Controls.Find("DailyRaport202DateTimePicker1", true).FirstOrDefault() as DateTimePicker).Value.Date).TotalDays);
                            if (days < 0)
                            {
                                MessageBoxTi.Show("Неверно введён период");
                                return;
                            }
                            Documentation.Daily202Report((Controls.Find("DailyRaport202DateTimePicker1", true).FirstOrDefault() as DateTimePicker).Value.Date, days);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBoxTi.Show("RaportsForOperators.TodayReport " + ex.Message);
                    }
                };

                Controls.Find("203ReportGroupBox", true).FirstOrDefault().Controls.Add(new FlowLayoutPanel()
                {
                    Name = "203ReportGroupBoxFlowLayoutPanel",
                    Dock = DockStyle.Fill,
                });

                Controls.Find("203ReportGroupBoxFlowLayoutPanel", true).FirstOrDefault().Controls.Add(new TableLayoutPanel()
                {
                    Name = "Raport203TableLayoutPanel",
                    Height = 30,
                });

                (Controls.Find("Raport203TableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).ColumnStyles.Clear();
                (Controls.Find("Raport203TableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).RowStyles.Clear();
                (Controls.Find("Raport203TableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 90f));
                (Controls.Find("Raport203TableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10f));

                (Controls.Find("Raport203TableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(new DateTimePicker()
                {
                    Name = "Raport203DateTimePicker1",
                }, 0, 0);

                (Controls.Find("Raport203TableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel).Controls.Add(new CheckBox()
                {
                    Name = "Raport203CheckBox",
                }, 1, 0);

                Controls.Find("203ReportGroupBoxFlowLayoutPanel", true).FirstOrDefault().Controls.Add(new DateTimePicker()
                {
                    Name = "Raport203DateTimePicker2",
                    Enabled = false,
                });

                (Controls.Find("Raport203CheckBox", true).FirstOrDefault() as CheckBox).CheckedChanged += (s, e) =>
                {
                    try
                    {
                        if ((Controls.Find("Raport203CheckBox", true).FirstOrDefault() as CheckBox).Checked)
                        {
                            (Controls.Find("Raport203DateTimePicker2", true).FirstOrDefault() as DateTimePicker).Enabled = true;
                            (Controls.Find("Report203", true).FirstOrDefault() as Button).Text = "Отчёт за период";
                        }
                        else
                        {
                            (Controls.Find("Raport203DateTimePicker2", true).FirstOrDefault() as DateTimePicker).Enabled = false;
                            (Controls.Find("Report203", true).FirstOrDefault() as Button).Text = "Отчёт";
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBoxTi.Show("RaportsForOperators.Raport203CheckBox " + ex.Message);
                    }
                };

                Controls.Find("203ReportGroupBoxFlowLayoutPanel", true).FirstOrDefault().Controls.Add(new Button()
                {
                    Name = "Report203",
                    Text = "Отчёт",
                    AutoSize = true,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Supports.headGrey,
                    Width = 200,
                });

                (Controls.Find("Report203", true).FirstOrDefault() as Button).Click += (s, e) =>
                {
                    try
                    {
                        if (!(Controls.Find("Raport203CheckBox", true).FirstOrDefault() as CheckBox).Checked)
                            Documentation.TillCalledFor203Report((Controls.Find("Raport203DateTimePicker1", true).FirstOrDefault() as DateTimePicker).Value.Date);
                        else
                        {
                            int days = Convert.ToInt32(((Controls.Find("Raport203DateTimePicker2", true).FirstOrDefault() as DateTimePicker).Value.Date - (Controls.Find("Raport203DateTimePicker1", true).FirstOrDefault() as DateTimePicker).Value.Date).TotalDays);
                            if (days < 0)
                            {
                                MessageBoxTi.Show("Неверно введён период");
                                return;
                            }
                            Documentation.TillCalledFor203Report((Controls.Find("Raport203DateTimePicker1", true).FirstOrDefault() as DateTimePicker).Value.Date, days);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBoxTi.Show("RaportsForOperators.Report203Button " + ex.Message);
                    }
                };
            }
            catch (Exception e)
            {
                MessageBoxTi.Show("RaportsForOperators " + e.Message);
            }
        }
    }
}