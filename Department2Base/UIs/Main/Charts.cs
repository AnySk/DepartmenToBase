using Department2Base.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Department2Base
{
    public class Charts : TabPage
    {
        #region controls and globals
        TabControlTi chartTabs = new TabControlTi(false);
        TabPage satellitesStatistics = new TabPage("По спутникам");
        TabPage beamsStatistics = new TabPage("По излучениям");
        ChartTi satellitesStatisticsChart = new ChartTi() { Dock = DockStyle.Fill };
        ChartTi beamsStatisticsChart = new ChartTi() { Dock = DockStyle.Fill };
        Panel satellitesStatisticsPanel = new Panel() { Dock = DockStyle.Left, Padding = new Padding(0, 0, 4, 0) };
        Panel beamsStatisticsPanel = new Panel() { Dock = DockStyle.Left, Padding = new Padding(0, 0, 4, 0) };
        CheckedListBox beamsStatisticsCheckedListBox = new CheckedListBox() { Dock = DockStyle.Fill };
        ComboBox beamsStatisticsSatelliteComboBox = new ComboBox() { Dock = DockStyle.Fill };
        ComboBox beamsStatisticsBandComboBox = new ComboBox() { Dock = DockStyle.Fill };
        ComboBox beamsStatisticsPolarizationComboBox = new ComboBox() { Dock = DockStyle.Fill };
        Panel beamsCurrentSeriesPanel = new Panel()
        {
            Dock = DockStyle.Bottom,
            Height = 200,
            AutoScroll = true,
        };
        Panel satellitesCurrentSeriesPanel = new Panel()
        {
            Dock = DockStyle.Bottom,
            Height = 200,
            AutoScroll = true,
        };
        Button beamsApply = new Button()
        {
            Dock = DockStyle.Bottom,
            Height = 37,
            Text = "Добавить",
        };
        Button beamsExportExel = new Button()
        {
            Dock = DockStyle.Bottom,
            Height = 37,
            Text = "Выгрузить в Excel",
        };
        Button satellitesApply = new Button()
        {
            Dock = DockStyle.Bottom,
            Height = 37,
            Text = "Добавить",
        };
        Button satellitesExportExel = new Button()
        {
            Dock = DockStyle.Bottom,
            Height = 37,
            Text = "Выгрузить в Excel",
        };
        #endregion

        string satelliteCheckBoxesFilter = null;
        string beamsCheckBoxesFilter = null;

        public Charts(string name)
        {
            Width = 350;
            Text = "Графики";
            LoadControls();
            Name = name;
        }

        private void LoadControls()
        {
            DataTable load = dataBase.SimpleData("SSALoading");

            Controls.Add(chartTabs);
            chartTabs.TabPages.Add(satellitesStatistics);
            chartTabs.TabPages.Add(beamsStatistics);



            satellitesStatistics.Controls.Add(satellitesStatisticsChart);
            beamsStatistics.Controls.Add(beamsStatisticsChart);

            satellitesStatistics.Controls.Add(satellitesStatisticsPanel);
            beamsStatistics.Controls.Add(beamsStatisticsPanel);


            #region satellitesStatistics

            satellitesStatisticsPanel.Controls.Add(new TreeView()
            {
                Name = "FilterTreeView",
                Dock = DockStyle.Fill,
                CheckBoxes = true,
            });

            foreach (string data in load.Columns.Cast<DataColumn>().Select(x => x.ColumnName))
            {
                switch (data)
                {
                    case "ID":
                        break;
                    case "Частота":
                        break;
                    case "ВремяВкл":
                        break;
                    case "Тактовая":
                        break;
                    case "RПУК":
                        break;
                    case "ПУК":
                        break;
                    case "Скорость":
                        break;
                    case "Протоколы":
                        break;
                    case "ХарактерИнфо":
                        break;
                    case "Принадлежность":
                        break;
                    case "ВидИсточника":
                        break;
                    case "ВидОбъекта":
                        break;
                    case "ХарРаботы":
                        break;
                    case "ВозмСобытие":
                        break;
                    case "СостДеятельности":
                        break;
                    case "Примечание":
                        break;
                    default:
                        (satellitesStatisticsPanel.Controls.Find("FilterTreeView", true).FirstOrDefault() as TreeView).Nodes.Add(data, data);

                        if (data == "Состояние")
                            (satellitesStatisticsPanel.Controls.Find("FilterTreeView", true).FirstOrDefault() as TreeView).Nodes["Состояние"].BackColor = Supports.darkBlue;

                        List<string> distinctConditions = load.Rows.Cast<DataRow>().Select(x => x[data].ToString()).OrderBy(x => x).Distinct().ToList();

                        if (distinctConditions.Count < 2)
                            continue;

                        foreach (string str in distinctConditions)
                        {
                            if (str == "True")
                                (satellitesStatisticsPanel.Controls.Find("FilterTreeView", true).FirstOrDefault() as TreeView).Nodes.Find(data, false).FirstOrDefault().Nodes.Add(str.ToString(), "Вкл.");
                            else if (str == "False")
                                (satellitesStatisticsPanel.Controls.Find("FilterTreeView", true).FirstOrDefault() as TreeView).Nodes.Find(data, false).FirstOrDefault().Nodes.Add(str.ToString(), "Выкл.");
                            else
                                (satellitesStatisticsPanel.Controls.Find("FilterTreeView", true).FirstOrDefault() as TreeView).Nodes.Find(data, false).FirstOrDefault().Nodes.Add(str.ToString(), str.ToString());
                        }
                        break;

                }
            }

            (satellitesStatisticsPanel.Controls.Find("FilterTreeView", true).FirstOrDefault() as TreeView).BeforeCheck += (s, e) =>
            {
                if (e.Node.Parent == null && e.Node.Name != "Состояние")
                    e.Cancel = true;
            };

            (satellitesStatisticsPanel.Controls.Find("FilterTreeView", true).FirstOrDefault() as TreeView).AfterCheck += (s, e) =>
            {
                if (e.Node.Parent != null)
                {
                    if (e.Node.Parent.Name == "Состояние" && e.Node.Checked)
                        e.Node.Parent.Checked = false;

                    bool checkedCol = false;
                    satelliteCheckBoxesFilter = null;
                    foreach (TreeNode node in (satellitesStatisticsPanel.Controls.Find("FilterTreeView", true).FirstOrDefault() as TreeView).Nodes)
                    {
                        foreach (TreeNode nod in node.Nodes)
                            if (nod.Checked)
                            {
                                satelliteCheckBoxesFilter += '(';
                                checkedCol = true;
                                break;
                            }
                        if (checkedCol)
                        {

                            foreach (TreeNode nod in node.Nodes)
                                if (nod.Checked)
                                {
                                    satelliteCheckBoxesFilter += "[" + node.Name + "] = '" + nod.Name + "' OR ";
                                }
                            checkedCol = false;
                            satelliteCheckBoxesFilter = satelliteCheckBoxesFilter.Remove(satelliteCheckBoxesFilter.Length - 4, 4);
                            satelliteCheckBoxesFilter += ") AND ";
                        }
                    }
                    if (satelliteCheckBoxesFilter != null)
                    {
                        satelliteCheckBoxesFilter = satelliteCheckBoxesFilter.Remove(satelliteCheckBoxesFilter.Length - 6, 6);
                        satelliteCheckBoxesFilter += ')';
                    }


                }
                else if (e.Node.Name == "Состояние" && e.Node.Checked)
                {
                    foreach (TreeNode tn in e.Node.Nodes)
                        tn.Checked = false;
                }
            };

            satellitesStatisticsPanel.Controls.Add(new GroupBox()
            {
                Name = "TimeFirstTabControlGroupBox",
                Dock = DockStyle.Top,
                Text = "Время",
                Height = 60,
            });

            (satellitesStatisticsPanel.Controls.Find("TimeFirstTabControlGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(new DateTimePicker()
            {
                Name = "maxDateTimePicker",
                Dock = DockStyle.Bottom,
            });

            (satellitesStatisticsPanel.Controls.Find("TimeFirstTabControlGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(new DateTimePicker()
            {
                Name = "minDateTimePicker",
                Dock = DockStyle.Top,
            });

            satellitesStatisticsPanel.Controls.Add(satellitesCurrentSeriesPanel);

            satellitesStatisticsPanel.Controls.Add(satellitesApply);

            satellitesStatisticsPanel.Controls.Add(satellitesExportExel);

            Point? satellitesPrevPosition = null;
            ToolTip satellitesToolTip = new ToolTip();

            satellitesStatisticsChart.MouseMove += (s, e) =>
            {


                var pos = e.Location;
                if (satellitesPrevPosition.HasValue && pos == satellitesPrevPosition.Value)
                    return;
                satellitesToolTip.RemoveAll();
                satellitesPrevPosition = pos;
                var results = satellitesStatisticsChart.HitTest(pos.X, pos.Y, false, ChartElementType.DataPoint);

                foreach (var result in results)
                {
                    if (result.ChartElementType == ChartElementType.DataPoint)
                    {
                        var prop = result.Object as DataPoint;

                        if (prop != null)
                        {
                            var pointXPixel = result.ChartArea.AxisX.ValueToPixelPosition(prop.XValue);
                            var pointYPixel = result.ChartArea.AxisY.ValueToPixelPosition(prop.YValues[0]);

                            if (Math.Abs(pos.X - pointXPixel) < 4)
                                satellitesToolTip.Show("Время - " + DateTime.FromOADate(prop.XValue), satellitesStatisticsChart, pos.X, pos.Y);
                        }
                    }
                }
            };

            #endregion

            #region beamsStatistics

            beamsStatisticsPanel.Controls.Add(new TreeView()
            {
                Name = "FilterTreeView",
                Dock = DockStyle.Fill,
                CheckBoxes = true,
            });

            foreach (string data in load.Columns.Cast<DataColumn>().Select(x => x.ColumnName))
            {
                switch (data)
                {
                    case "ID":
                        break;
                    case "Спутник":
                        break;
                    case "Диапазон":
                        break;
                    case "Поляризация":
                        break;
                    case "Частота":
                        break;
                    case "ВремяВкл":
                        break;
                    case "Тактовая":
                        break;
                    case "RПУК":
                        break;
                    case "ПУК":
                        break;
                    case "Скорость":
                        break;
                    case "Протоколы":
                        break;
                    case "ХарактерИнфо":
                        break;
                    case "Принадлежность":
                        break;
                    case "ВидИсточника":
                        break;
                    case "ВидОбъекта":
                        break;
                    case "ХарРаботы":
                        break;
                    case "ВозмСобытие":
                        break;
                    case "СостДеятельности":
                        break;
                    case "Примечание":
                        break;
                    default:
                        (beamsStatisticsPanel.Controls.Find("FilterTreeView", true).FirstOrDefault() as TreeView).Nodes.Add(data, data);

                        if (data == "Состояние")
                            (beamsStatisticsPanel.Controls.Find("FilterTreeView", true).FirstOrDefault() as TreeView).Nodes["Состояние"].BackColor = Supports.darkBlue;

                        List<string> distinctConditions = load.Rows.Cast<DataRow>().Select(x => x[data].ToString()).OrderBy(x => x).Distinct().ToList();

                        if (distinctConditions.Count < 2)
                            continue;

                        foreach (string str in distinctConditions)
                        {
                            if (str == "True")
                                (beamsStatisticsPanel.Controls.Find("FilterTreeView", true).FirstOrDefault() as TreeView).Nodes.Find(data, false).FirstOrDefault().Nodes.Add(str.ToString(), "Вкл.");
                            else if (str == "False")
                                (beamsStatisticsPanel.Controls.Find("FilterTreeView", true).FirstOrDefault() as TreeView).Nodes.Find(data, false).FirstOrDefault().Nodes.Add(str.ToString(), "Выкл.");
                            else
                                (beamsStatisticsPanel.Controls.Find("FilterTreeView", true).FirstOrDefault() as TreeView).Nodes.Find(data, false).FirstOrDefault().Nodes.Add(str.ToString(), str.ToString());
                        }
                        break;

                }
            }

            (beamsStatisticsPanel.Controls.Find("FilterTreeView", true).FirstOrDefault() as TreeView).BeforeCheck += (s, e) =>
            {
                if (e.Node.Parent == null)
                    e.Cancel = true;
            };

            (beamsStatisticsPanel.Controls.Find("FilterTreeView", true).FirstOrDefault() as TreeView).AfterCheck += (s, e) =>
            {
                if (e.Node.Parent != null)
                {
                    bool checkedCol = false;
                    beamsCheckBoxesFilter = null;
                    foreach (TreeNode node in (beamsStatisticsPanel.Controls.Find("FilterTreeView", true).FirstOrDefault() as TreeView).Nodes)
                    {
                        foreach (TreeNode nod in node.Nodes)
                            if (nod.Checked)
                            {
                                beamsCheckBoxesFilter += '(';
                                checkedCol = true;
                                break;
                            }
                        if (checkedCol)
                        {

                            foreach (TreeNode nod in node.Nodes)
                                if (nod.Checked)
                                {
                                    beamsCheckBoxesFilter += "[" + node.Name + "] = '" + nod.Name + "' OR ";
                                }
                            checkedCol = false;
                            beamsCheckBoxesFilter = beamsCheckBoxesFilter.Remove(beamsCheckBoxesFilter.Length - 4, 4);
                            beamsCheckBoxesFilter += ") AND ";
                        }
                    }
                    if (beamsCheckBoxesFilter != null)
                    {
                        beamsCheckBoxesFilter = beamsCheckBoxesFilter.Remove(beamsCheckBoxesFilter.Length - 6, 6);
                        beamsCheckBoxesFilter += ')';
                    }


                }
                else if (e.Node.Name == "Состояние" && e.Node.Checked)
                {
                    foreach (TreeNode tn in e.Node.Nodes)
                        tn.Checked = false;
                }

                RefreshBeams();
            };

            beamsStatisticsPanel.Controls.Add(new GroupBox()
            {
                Name = "PolarSecondTabControlGroupBox",
                Dock = DockStyle.Top,
                Height = 37,
                Text = "Поляризация",
            });

            (beamsStatisticsPanel.Controls.Find("PolarSecondTabControlGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(beamsStatisticsPolarizationComboBox);

            beamsStatisticsPanel.Controls.Add(new GroupBox()
            {
                Name = "BandsSecondTabControlGroupBox",
                Dock = DockStyle.Top,
                Height = 37,
                Text = "Диапазон",
            });

            (beamsStatisticsPanel.Controls.Find("BandsSecondTabControlGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(beamsStatisticsBandComboBox);

            beamsStatisticsPanel.Controls.Add(new GroupBox()
            {
                Name = "SatelliteSecondTabControlGroupBox",
                Dock = DockStyle.Top,
                Height = 37,
                Text = "Спутники",
            });

            (beamsStatisticsPanel.Controls.Find("SatelliteSecondTabControlGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(beamsStatisticsSatelliteComboBox);

            foreach (string sat in dataBase.SimpleData("SatelliteList").Rows.Cast<DataRow>().Select(x => x["НаименованиеИСЗ"].ToString()).OrderBy(x => x).Distinct().ToList())
            {
                beamsStatisticsSatelliteComboBox.Items.Add(sat);
            }

            foreach (string sat in dataBase.SimpleData("FrequencyBand").Rows.Cast<DataRow>().Select(x => x["Наименование диапазона"].ToString()).ToList())
            {
                beamsStatisticsBandComboBox.Items.Add(sat);
            }

            beamsStatisticsPolarizationComboBox.Items.Add("L");
            beamsStatisticsPolarizationComboBox.Items.Add("R");
            beamsStatisticsPolarizationComboBox.Items.Add("V");
            beamsStatisticsPolarizationComboBox.Items.Add("H");

            beamsStatisticsPolarizationComboBox.SelectedIndex = 0;
            beamsStatisticsBandComboBox.SelectedIndex = 0;
            beamsStatisticsSatelliteComboBox.SelectedIndex = 0;

            beamsStatisticsSatelliteComboBox.SelectedIndexChanged += (s, e) => RefreshBeams();
            beamsStatisticsPolarizationComboBox.SelectedIndexChanged += (s, e) => RefreshBeams();
            beamsStatisticsBandComboBox.SelectedIndexChanged += (s, e) => RefreshBeams();

            beamsStatisticsPanel.Controls.Add(new GroupBox()
            {
                Name = "TimeFirstTabControlGroupBox",
                Dock = DockStyle.Top,
                Text = "Время",
                Height = 60,
            });

            (beamsStatisticsPanel.Controls.Find("TimeFirstTabControlGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(new DateTimePicker()
            {
                Name = "maxDateTimePicker",
                Dock = DockStyle.Bottom,
            });

            (beamsStatisticsPanel.Controls.Find("TimeFirstTabControlGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(new DateTimePicker()
            {
                Name = "minDateTimePicker",
                Dock = DockStyle.Top,
            });

            beamsStatisticsPanel.Controls.Add(new GroupBox()
            {
                Name = "BeamsSecondTabControlGroupBox",
                Dock = DockStyle.Bottom,
                Height = 200,
                Text = "Излучения",
            });

            (beamsStatisticsPanel.Controls.Find("BeamsSecondTabControlGroupBox", true).FirstOrDefault() as GroupBox).Controls.Add(beamsStatisticsCheckedListBox);

            beamsStatisticsPanel.Controls.Add(beamsCurrentSeriesPanel);

            beamsStatisticsPanel.Controls.Add(beamsApply);

            beamsStatisticsPanel.Controls.Add(beamsExportExel);



            Point? beamsPrevPosition = null;
            ToolTip beamsToolTip = new ToolTip();

            beamsStatisticsChart.MouseMove += (s, e) =>
            {
                var pos = e.Location;
                if (beamsPrevPosition.HasValue && pos == beamsPrevPosition.Value)
                    return;
                beamsToolTip.RemoveAll();
                beamsPrevPosition = pos;
                var results = beamsStatisticsChart.HitTest(pos.X, pos.Y, false, ChartElementType.DataPoint);

                foreach (var result in results)
                {
                    if (result.ChartElementType == ChartElementType.DataPoint)
                    {
                        var prop = result.Object as DataPoint;

                        if (prop != null)
                        {
                            var pointXPixel = result.ChartArea.AxisX.ValueToPixelPosition(prop.XValue);
                            var pointYPixel = result.ChartArea.AxisY.ValueToPixelPosition(prop.YValues[0]);

                            if (Math.Abs(pos.X - pointXPixel) < 4)
                                beamsToolTip.Show("Время - " + DateTime.FromOADate(prop.XValue), beamsStatisticsChart, pos.X, pos.Y);
                        }
                    }
                }
            };

            #endregion

            beamsApply.Click += (s, e) =>
            {
                beamsStatisticsChart.chartArea.AxisX.Title = "Дата";
                beamsStatisticsChart.chartArea.AxisY.Title = "";
                beamsStatisticsChart.chartArea.AxisY.LabelStyle.ForeColor = Supports.headGrey;

                foreach (string beam in beamsStatisticsCheckedListBox.CheckedItems)
                {
                    int i = 0;
                    int j = 0;
                    foreach (Panel pan in beamsCurrentSeriesPanel.Controls)
                    {
                        if (pan.Controls.Find("Series" + i + "Label", true).FirstOrDefault().Text.Contains("График Состояний для"))
                            j++;

                        if (pan.Name == "Series" + i)
                            i++;

                    }


                    beamsCurrentSeriesPanel.Controls.Add(new Panel()
                    {
                        Name = "Series" + i,
                        Dock = DockStyle.Top,
                        Height = 20,
                        BackColor = Supports.headGrey,
                    });
                    (beamsStatisticsPanel.Controls.Find("Series" + i, true).FirstOrDefault() as Panel).MouseClick += (se, a) =>
                    {
                        foreach (Panel pa in beamsCurrentSeriesPanel.Controls)
                            pa.BackColor = Supports.headGrey;

                        (beamsStatisticsPanel.Controls.Find("Series" + i, true).FirstOrDefault() as Panel).BackColor = Supports.headBlue;
                    };
                    (beamsStatisticsPanel.Controls.Find("Series" + i, true).FirstOrDefault() as Panel).Controls.Add(new TransLabel()
                    {
                        Name = "Series" + i + "Label",
                        Text = "График Состояний для " + beam,
                        Dock = DockStyle.Fill,
                        TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                        ForeColor = Supports.textWhite,

                    });
                    (beamsStatisticsPanel.Controls.Find("Series" + i, true).FirstOrDefault() as Panel).Controls.Add(new Button()
                    {
                        Name = "Series" + i + "Button",
                        Dock = DockStyle.Right,
                        Width = 20,
                        Image = Resources.x1tb,
                        FlatStyle = FlatStyle.Flat,
                    });
                    (beamsStatisticsPanel.Controls.Find("Series" + i + "Button", true).FirstOrDefault() as Button).Click += (se, ar) =>
                    {
                        beamsCurrentSeriesPanel.Controls.RemoveByKey("Series" + i);
                        beamsStatisticsChart.Series.Remove(beamsStatisticsChart.Series["Series" + i]);
                    };
                    beamsStatisticsChart.Series.Add("Series" + i);
                    beamsStatisticsChart.Series["Series" + i].BorderWidth = 3;
                    beamsStatisticsChart.Series["Series" + i].ChartType = SeriesChartType.StepLine;
                    beamsStatisticsChart.Series["Series" + i].XValueType = ChartValueType.DateTime;
                    beamsStatisticsChart.Series["Series" + i].LegendText = " Частота " + beam;
                    beamsStatisticsChart.Series["Series" + i].Points.AddXY((beamsStatisticsPanel.Controls.Find("minDateTimePicker", true).FirstOrDefault() as DateTimePicker).Value.Date, j * 2);
                    int lastState = j * 2;
                    foreach (DataRow state in dataBase.SimpleData("SSALoading WHERE [ВремяВкл] < '" + (beamsStatisticsPanel.Controls.Find("maxDateTimePicker", true).FirstOrDefault() as DateTimePicker).Value.Date + "' AND [ВремяВкл] > '" +
                       (beamsStatisticsPanel.Controls.Find("minDateTimePicker", true).FirstOrDefault() as DateTimePicker).Value.Date + "' AND [Спутник] = '" +
                       beamsStatisticsSatelliteComboBox.Text + "' AND [Диапазон] = '" +
                       beamsStatisticsBandComboBox.Text + "' AND [Поляризация] = '" +
                       beamsStatisticsPolarizationComboBox.Text + "' AND [Частота] > '" + (Convert.ToInt32(beam) - 50).ToString()
                       + "' AND [Частота] < '" + (Convert.ToInt32(beam) + 50).ToString() + "'").Rows)
                    {
                        DateTime gf = (DateTime)state["ВремяВкл"];
                        beamsStatisticsChart.Series["Series" + i].Points.AddXY(state["ВремяВкл"], Convert.ToInt32(state["Состояние"]) + j * 2);
                        lastState = Convert.ToInt32(state["Состояние"]) + j * 2;
                    }

                    beamsStatisticsChart.Series["Series" + i].Points.AddXY((beamsStatisticsPanel.Controls.Find("maxDateTimePicker", true).FirstOrDefault() as DateTimePicker).Value.Date, lastState);

                }
            };

            beamsExportExel.Click += (s, e) =>
            {
                if (beamsStatisticsChart.Series.Count == 0)
                    return;
                Documentation.Graphics1(beamsStatisticsChart.Series[0].Points.ToList<DataPoint>(), "Этот график", "Спутник1", Microsoft.Office.Interop.Excel.XlChartType.xlXYScatterLines);
            };

            satellitesApply.Click += (s, e) =>
            {

                satellitesStatisticsChart.chartArea.AxisX.Title = "Дата";
                satellitesStatisticsChart.chartArea.AxisY.Title = "Количество излучений";
                satellitesStatisticsChart.chartArea.AxisY.LabelStyle.ForeColor = Supports.textBlack;

                int i = 0;
                foreach (Panel pan in satellitesCurrentSeriesPanel.Controls)
                    if (pan.Name == "Series" + i)
                        i++;


                satellitesCurrentSeriesPanel.Controls.Add(new Panel()
                {
                    Name = "Series" + i,
                    Dock = DockStyle.Top,
                    Height = 20,
                    BackColor = Supports.headGrey,
                });

                (satellitesStatisticsPanel.Controls.Find("Series" + i, true).FirstOrDefault() as Panel).MouseClick += (se, a) =>
                {
                    foreach (Panel pa in satellitesCurrentSeriesPanel.Controls)
                        pa.BackColor = Supports.headGrey;

                    (satellitesStatisticsPanel.Controls.Find("Series" + i, true).FirstOrDefault() as Panel).BackColor = Supports.headBlue;
                };

                (satellitesStatisticsPanel.Controls.Find("Series" + i, true).FirstOrDefault() as Panel).Controls.Add(new TransLabel()
                {
                    Name = "Series" + i + "Label",
                    Text = "График " + i,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Supports.textWhite,

                });

                (satellitesStatisticsPanel.Controls.Find("Series" + i, true).FirstOrDefault() as Panel).Controls.Add(new Button()
                {
                    Name = "Series" + i + "Button",
                    Dock = DockStyle.Right,
                    Width = 20,
                    Image = Resources.x1tb,
                    FlatStyle = FlatStyle.Flat,
                });

                (satellitesStatisticsPanel.Controls.Find("Series" + i + "Button", true).FirstOrDefault() as Button).Click += (se, ar) =>
                {
                    satellitesCurrentSeriesPanel.Controls.RemoveByKey("Series" + i);
                    satellitesStatisticsChart.Series.Remove(satellitesStatisticsChart.Series["Series" + i]);
                };

                satellitesStatisticsChart.Series.Add("Series" + i);

                satellitesStatisticsChart.Series["Series" + i].XValueType = ChartValueType.Date;
                satellitesStatisticsChart.Series["Series" + i].LegendText = satelliteCheckBoxesFilter.Replace("(", string.Empty).Replace(")", string.Empty).Replace("AND", ",").Replace("=", ":").Replace("OR", ",").Replace("]", string.Empty).Replace("[", string.Empty).Replace("'", string.Empty);
                //  satellitesStatisticsChart.Series["123"].ChartType = SeriesChartType.Spline;
                //   satellitesStatisticsChart.Series[0].Points.Clear();

                if ((satellitesStatisticsPanel.Controls.Find("FilterTreeView", true).FirstOrDefault() as TreeView).Nodes["Состояние"].Checked)

                    for (int j = 0; j < Convert.ToInt32(((satellitesStatisticsPanel.Controls.Find("maxDateTimePicker", true).FirstOrDefault() as DateTimePicker).Value.Date - (satellitesStatisticsPanel.Controls.Find("minDateTimePicker", true).FirstOrDefault() as DateTimePicker).Value.Date).TotalDays); j++)
                    {
                        satellitesStatisticsChart.Series["Series" + i].Points.AddXY((satellitesStatisticsPanel.Controls.Find("minDateTimePicker", true).FirstOrDefault() as DateTimePicker).Value.Date.AddDays(j), dataBase.ToCount("SELECT " +
                                                                                   "(SELECT COUNT(*) FROM SSALoading WHERE " + satelliteCheckBoxesFilter + " AND [Состояние] = 0 AND [ВремяВкл] > '" + (satellitesStatisticsPanel.Controls.Find("minDateTimePicker", true).FirstOrDefault() as DateTimePicker).Value.Date.AddDays(j).AddHours(9) + "' AND [ВремяВкл] < GETDATE()) - "
                                                                                 + "(SELECT COUNT(*) FROM SSALoading WHERE " + satelliteCheckBoxesFilter + " AND [Состояние] = 1 AND [ВремяВкл] > '" + (satellitesStatisticsPanel.Controls.Find("minDateTimePicker", true).FirstOrDefault() as DateTimePicker).Value.Date.AddDays(j).AddHours(9) + "' AND [ВремяВкл] < GETDATE()) + "
                                                                                 + "(SELECT COUNT(*) FROM Loading WHERE " + satelliteCheckBoxesFilter + " AND [Состояние] = 1)"));
                    }
                else
                    for (int j = 0; j < Convert.ToInt32(((satellitesStatisticsPanel.Controls.Find("maxDateTimePicker", true).FirstOrDefault() as DateTimePicker).Value.Date - (satellitesStatisticsPanel.Controls.Find("minDateTimePicker", true).FirstOrDefault() as DateTimePicker).Value.Date).TotalDays); j++)
                    {
                        satellitesStatisticsChart.Series["Series" + i].Points.AddXY((satellitesStatisticsPanel.Controls.Find("minDateTimePicker", true).FirstOrDefault() as DateTimePicker).Value.Date.AddDays(j), dataBase.ToCount("SELECT COUNT(*) FROM SSALoading WHERE " + satelliteCheckBoxesFilter + " AND [ВремяВкл] > '" + (satellitesStatisticsPanel.Controls.Find("minDateTimePicker", true).FirstOrDefault() as DateTimePicker).Value.Date.AddDays(j) + "' AND [ВремяВкл] < '" + (satellitesStatisticsPanel.Controls.Find("minDateTimePicker", true).FirstOrDefault() as DateTimePicker).Value.Date.AddDays(j + 1) + "'"));
                    }
            };

            satellitesExportExel.Click += (s, e) =>
            {
                if (satellitesStatisticsChart.Series.Count == 0)
                    return;
                Documentation.Graphics1(satellitesStatisticsChart.Series[0].Points.ToList<DataPoint>(), "Этот график", "Спутник1");
            };

            Supports.GangeGroup(this);
        }

        private void RefreshBeams()
        {
            beamsStatisticsCheckedListBox.Items.Clear();
            if (beamsCheckBoxesFilter == null)
            {
                foreach (string sat in dataBase.SimpleData(
                "Loading WHERE [Спутник] = '" + beamsStatisticsSatelliteComboBox.Text +
                "' AND [Диапазон] = '" + beamsStatisticsBandComboBox.Text +
                "' AND [Поляризация] = '" + beamsStatisticsPolarizationComboBox.Text + "' ").Rows.Cast<DataRow>().Select(x => x["Частота"].ToString()).Distinct().OrderBy(x => x).ToList())
                {
                    beamsStatisticsCheckedListBox.Items.Add(sat, false);
                };
            }
            else
                foreach (string sat in dataBase.SimpleData(
                "Loading WHERE [Спутник] = '" + beamsStatisticsSatelliteComboBox.Text +
                "' AND [Диапазон] = '" + beamsStatisticsBandComboBox.Text +
                "' AND [Поляризация] = '" + beamsStatisticsPolarizationComboBox.Text + "' AND " + beamsCheckBoxesFilter).Rows.Cast<DataRow>().Select(x => x["Частота"].ToString()).Distinct().OrderBy(x => x).ToList())
                {
                    beamsStatisticsCheckedListBox.Items.Add(sat, false);
                };
        }
    }
}