using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Department2Base
{
    class ChartTi : Chart
    {
        public ChartArea chartArea = new ChartArea();
        public Legend legend = new Legend();
        public ChartTi()
        {
            BackColor = Supports.headGrey;
            ForeColor = Supports.textWhite;

            chartArea.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chartArea.AxisX2.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chartArea.AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chartArea.AxisY2.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chartArea.Name = "chartArea";
            ChartAreas.Add(chartArea);

            chartArea.BackColor = Supports.headGrey;
            chartArea.AxisX.LineColor = Supports.textBlack;
            chartArea.AxisY.LineColor = Supports.textBlack;
            chartArea.AxisX.MajorGrid.LineColor = Supports.textBlack;
            chartArea.AxisY.MajorGrid.LineColor = Supports.textBlack;
            chartArea.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Days;
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.ScaleView.Zoomable = true;
            chartArea.CursorX.AutoScroll = true;
            chartArea.CursorX.IsUserSelectionEnabled = true;
            chartArea.AxisX.LabelStyle.ForeColor = Supports.textWhite;
            chartArea.AxisY.LabelStyle.ForeColor = Supports.textWhite;
            chartArea.AxisY.MajorGrid.LineColor = Supports.textBlack;
            chartArea.AxisX.InterlacedColor = Supports.textBlack;
            chartArea.AxisY.InterlacedColor = Supports.textBlack;
            chartArea.AxisX.TitleForeColor = Supports.textWhite;
            chartArea.AxisY.TitleForeColor = Supports.textWhite;
            chartArea.AxisX.LabelStyle.Format = "dd.MM.yyyy";
            chartArea.AxisX.TitleFont = new Font("Times New Roman", 15);
            chartArea.AxisY.TitleFont = new Font("Times New Roman", 15);
            chartArea.AxisY.TextOrientation = TextOrientation.Rotated270;

            Point? prevPosition = null;
            ToolTip toolTip = new ToolTip();

            MouseMove += (s, e) =>
            {
                var pos = e.Location;
                if (prevPosition.HasValue && pos == prevPosition.Value)
                    return;
                toolTip.RemoveAll();
                prevPosition = pos;
                var results = HitTest(pos.X, pos.Y, false, ChartElementType.DataPoint);

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
                                toolTip.Show("Время - " + DateTime.FromOADate(prop.XValue), this, pos.X, pos.Y);
                        }
                    }
                }
            };

            legend.BackColor = Supports.headGrey;
            legend.ForeColor = Supports.textWhite;
            legend.TitleForeColor = Supports.textWhite;

            legend.DockedToChartArea = "chartArea";
            legend.Docking = Docking.Left;
            legend.IsDockedInsideChartArea = false;
            legend.LegendStyle = LegendStyle.Column;
            legend.Name = "Legend";
            legend.TableStyle = LegendTableStyle.Tall;
            legend.Title = "Обозначения";
            Legends.Add(legend);
        }
    }
}
