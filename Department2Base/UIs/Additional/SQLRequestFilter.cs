using Department2Base.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Department2Base
{
    public class SQLRequestFilter : Panel
    {
        private string checkBoxesFilter = null;
        private string textBoxesFilter = null;
        private string rangeTextBoxesFilter = null;
        private string rangeDateTimePickersFilter = null;
        private bool resetOn = false;
        private TreeView tree = null;
        private List<string> rangeTextBoxes = new List<string>();
        private List<string> rangeDateTimePickers = new List<string>();
        public delegate void Filter(object sourse, EventArgs e);
        public event Filter OnFilterChanged;
        string[] columnsFilter;
        int lastWidth = 0;

        public string CurrentFilter
        {
            get
            {
                string finText = textBoxesFilter;
                string finCheck = checkBoxesFilter;
                string finRangeText = rangeTextBoxesFilter;
                string finDateTime = rangeDateTimePickersFilter;
                if (textBoxesFilter != null && textBoxesFilter != "")
                {
                    finText = " AND " + textBoxesFilter;
                }
                if (checkBoxesFilter != null && checkBoxesFilter != "")
                {
                    finCheck = " AND " + checkBoxesFilter;
                }
                if (rangeTextBoxesFilter != null && rangeTextBoxesFilter != "")
                {
                    finRangeText = " AND " + rangeTextBoxesFilter;
                }
                if (rangeDateTimePickersFilter != null && rangeDateTimePickersFilter != "")
                {
                    finDateTime = " AND " + rangeDateTimePickersFilter;
                }
                return finText + finCheck + finRangeText + finDateTime;
            }
        }

        public SQLRequestFilter(DataGridView dataGridView, string[] columnsToFilter)
        {
            try
            {
                if (dataGridView == null)
                    return;

                columnsFilter = columnsToFilter;
                Width = 200;
                lastWidth = Width;
                Name = "filterPanel";
                Padding = new Padding(0, 0, 5, 0);

                Controls.Add(new TreeView()
                {
                    CheckBoxes = true,
                    Dock = DockStyle.Fill,
                    Scrollable = true,
                    Name = "filterTreeView",
                });

                Controls.Add(new Button()
                {
                    Dock = DockStyle.Top,
                    Width = 37,
                    Name = "filterButton",
                    Text = "Сбросить все фильтры",
                });

                Controls.Add(new Panel()
                {
                    Dock = DockStyle.Bottom,
                    Height = 600,
                    Name = "Panel",
                    AutoScroll = true,
                });

                Controls.Add(new Button()
                {
                    Dock = DockStyle.Right,
                    Name = "hideFilterButton",
                    Width = 15,
                    Image = Resources.left,

                });

                (Controls.Find("hideFilterButton", false).FirstOrDefault() as Button).Click += (s, e) =>
                {
                    if (Controls.Find("filterTreeView", false).FirstOrDefault().Visible)
                    {
                        Controls.Find("filterTreeView", false).FirstOrDefault().Visible = false;
                        Controls.Find("filterButton", false).FirstOrDefault().Visible = false;
                        Controls.Find("Panel", false).FirstOrDefault().Visible = false;
                        lastWidth = Width;
                        Width = 20;
                        (Controls.Find("hideFilterButton", false).FirstOrDefault() as Button).Image = Resources.right;
                        if (Parent.Controls.Find("filterSplitter", false).Count() != 0)
                            (Parent.Controls.Find("filterSplitter", false).FirstOrDefault() as Splitter).Visible = false;
                    }
                    else
                    {
                        Controls.Find("filterTreeView", false).FirstOrDefault().Visible = true;
                        Controls.Find("filterButton", false).FirstOrDefault().Visible = true;
                        Controls.Find("Panel", false).FirstOrDefault().Visible = true;
                        Width = lastWidth;
                        (Controls.Find("hideFilterButton", false).FirstOrDefault() as Button).Image = Resources.left;
                        if (Parent.Controls.Find("filterSplitter", false).Count() != 0)
                            (Parent.Controls.Find("filterSplitter", false).FirstOrDefault() as Splitter).Visible = true;
                    }

                };

                (Controls.Find("filterButton", false).FirstOrDefault() as Button).Click += (s, e) =>
                {
                    resetOn = true;
                    ResetAllNodes();
                    ResetAllText(this);
                    textBoxesFilter = null;
                    checkBoxesFilter = null;
                    rangeTextBoxesFilter = null;
                    rangeDateTimePickersFilter = null;
                    OnFilterChanged?.Invoke(this, new EventArgs());
                    resetOn = false;
                };

                tree = (Controls.Find("filterTreeView", false).FirstOrDefault() as TreeView);

                tree.BeforeCheck += (s, e) =>
                {
                    if (e.Node.Parent == null)
                        e.Cancel = true;
                };

                tree.AfterCheck += (s, e) =>
                {
                    if (resetOn)
                        return;

                    if (e.Node.Parent != null)
                    {
                        bool checkedCol = false;
                        checkBoxesFilter = null;
                        foreach (TreeNode node in tree.Nodes)
                        {

                            foreach (TreeNode nod in node.Nodes)
                                if (nod.Checked)
                                {
                                    checkBoxesFilter += '(';
                                    checkedCol = true;
                                    break;
                                }
                            if (checkedCol)
                            {

                                foreach (TreeNode nod in node.Nodes)
                                    if (nod.Checked)
                                    {
                                        checkBoxesFilter += "[" + node.Name + "] = '" + nod.Name + "' OR ";
                                    }
                                checkedCol = false;
                                checkBoxesFilter = checkBoxesFilter.Remove(checkBoxesFilter.Length - 4, 4);
                                checkBoxesFilter += ") AND ";
                            }
                        }
                        if (checkBoxesFilter != null)
                        {
                            checkBoxesFilter = checkBoxesFilter.Remove(checkBoxesFilter.Length - 6, 6);
                            checkBoxesFilter += ')';
                        }


                        OnFilterChanged?.Invoke(this, new EventArgs());
                    }
                };
                 

                foreach (DataGridViewColumn col in dataGridView.Columns)
                {
                    if (col.Name == "ID")
                        continue;
                    if ((col.ValueType == typeof(string) || col.ValueType == typeof(bool)) && !columnsFilter.Contains(col.Name))
                    {
                        List<string> distinctConditions = dataGridView.Rows.Cast<DataGridViewRow>().Select(x => x.Cells[col.Name].Value.ToString()).Where(x => x != "").OrderBy(x => x).Distinct().ToList();

                        Type k = col.ValueType;

                        if (distinctConditions.Count < 2)
                            continue;

                        tree.Nodes.Add(col.Name, col.HeaderText);

                        foreach (string str in distinctConditions)
                        {
                            if (col.ValueType != typeof(bool))
                                tree.Nodes.Find(col.Name, false).FirstOrDefault().Nodes.Add(str.ToString(), str.ToString());
                            else
                            {
                                if (str == "True")
                                    tree.Nodes.Find(col.Name, false).FirstOrDefault().Nodes.Add(str.ToString(), "Вкл.");
                                else
                                    tree.Nodes.Find(col.Name, false).FirstOrDefault().Nodes.Add(str.ToString(), "Выкл.");
                            }
                        }
                    }
                    else
                    {
                        if (col.ValueType == typeof(string))
                        {
                            Controls.Find("Panel", true).FirstOrDefault().Controls.Add(new GroupBox()
                            {
                                Text = col.Name,
                                Name = col.Name + "GroupBox",
                                Dock = DockStyle.Top,
                                Height = 37,
                            });

                            Controls.Find(col.Name + "GroupBox", true).FirstOrDefault().Controls.Add(new TextBox()
                            {
                                Dock = DockStyle.Fill,
                                Name = col.Name + "TextBox",
                            });
                            (Controls.Find(col.Name + "TextBox", true).FirstOrDefault() as TextBox).TextChanged += (s, e) =>
                            {
                                if (resetOn)
                                    return;


                                textBoxesFilter = null;
                                foreach (string text in columnsToFilter)
                                {
                                    if ((Controls.Find(text + "TextBox", true).FirstOrDefault() as TextBox).Text == "")
                                        continue;
                                    textBoxesFilter += '[' + text + "] LIKE '%" + (Controls.Find(text + "TextBox", true).FirstOrDefault() as TextBox).Text + "%' AND ";
                                }
                                if (textBoxesFilter != null)
                                    textBoxesFilter = textBoxesFilter.Remove(textBoxesFilter.Length - 4, 4);

                                OnFilterChanged?.Invoke(this, new EventArgs());
                            };
                        }
                        else if (col.ValueType == typeof(DateTime))
                        {
                            rangeDateTimePickers.Add(col.Name);
                            Controls.Find("Panel", true).FirstOrDefault().Controls.Add(new GroupBox()
                            {
                                Text = col.Name,
                                Name = col.Name + "GroupBox",
                                Dock = DockStyle.Top,
                                Height = 65,
                            });

                            Controls.Find(col.Name + "GroupBox", true).FirstOrDefault().Controls.Add(new DateTimePicker()
                            {
                                Name = col.Name + "1",
                                Dock = DockStyle.Top,                          
                            });

                            Controls.Find(col.Name + "GroupBox", true).FirstOrDefault().Controls.Add(new DateTimePicker()
                            {
                                Name = col.Name + "2",
                                Dock = DockStyle.Bottom,                  
                            });

                            Controls.Find(col.Name + "1", true).FirstOrDefault().TextChanged += (s, e) =>
                            {
                                if (resetOn)
                                    return;

                                string old = rangeDateTimePickersFilter;
                                rangeDateTimePickersFilter = null;




                                foreach (string text in rangeDateTimePickers)
                                {
                                    if ((Controls.Find(text + "1", true).FirstOrDefault() as DateTimePicker).Value >= (Controls.Find(text + "2", true).FirstOrDefault() as DateTimePicker).Value)
                                        continue;

                                    rangeDateTimePickersFilter += "([" + text + "] > '" + (Controls.Find(text + "1", true).FirstOrDefault() as DateTimePicker).Value.Date + "' AND [" + text + "] < '" + (Controls.Find(text + "2", true).FirstOrDefault() as DateTimePicker).Value.Date + "') AND ";
                                }
                                if (rangeDateTimePickersFilter != null)
                                    rangeDateTimePickersFilter = rangeDateTimePickersFilter.Remove(rangeDateTimePickersFilter.Length - 4, 4);

                                if (rangeDateTimePickersFilter != null)
                                    if (!rangeDateTimePickersFilter.Equals(old))
                                        OnFilterChanged?.Invoke(this, new EventArgs());
                            };

                            Controls.Find(col.Name + "2", true).FirstOrDefault().TextChanged += (s, e) =>
                            {
                                if (resetOn)
                                    return;

                                string old = rangeDateTimePickersFilter;
                                rangeDateTimePickersFilter = null;

                                foreach (string text in rangeDateTimePickers)
                                {
                                    if ((Controls.Find(text + "1", true).FirstOrDefault() as DateTimePicker).Value >= (Controls.Find(text + "2", true).FirstOrDefault() as DateTimePicker).Value)
                                        continue;

                                    rangeDateTimePickersFilter += "([" + text + "] > '" + (Controls.Find(text + "1", true).FirstOrDefault() as DateTimePicker).Value + "' AND [" + text + "] < '" + (Controls.Find(text + "2", true).FirstOrDefault() as DateTimePicker).Value + "') AND ";
                                }
                                if (rangeDateTimePickersFilter != null)
                                    rangeDateTimePickersFilter = rangeDateTimePickersFilter.Remove(rangeDateTimePickersFilter.Length - 4, 4);

                                if (rangeDateTimePickersFilter != null)
                                    if (!rangeDateTimePickersFilter.Equals(old))
                                        OnFilterChanged?.Invoke(this, new EventArgs());
                            };
                        }
                        else
                        {

                            rangeTextBoxes.Add(col.Name);
                            Controls.Find("Panel", true).FirstOrDefault().Controls.Add(new GroupBox()
                            {
                                Text = col.Name,
                                Name = col.Name + "GroupBox",
                                Dock = DockStyle.Top,
                                Height = 65,
                            });

                            Controls.Find(col.Name + "GroupBox", true).FirstOrDefault().Controls.Add(new TextBoxTi()
                            {
                                Name = col.Name + "1",
                                Dock = DockStyle.Top,
                                OnlyNumbers = true,
                                MaxLength = 9,
                            });

                            Controls.Find(col.Name + "GroupBox", true).FirstOrDefault().Controls.Add(new TextBoxTi()
                            {
                                Name = col.Name + "2",
                                Dock = DockStyle.Bottom,
                                OnlyNumbers = true,
                                MaxLength = 9,
                            });

                            Controls.Find(col.Name + "1", true).FirstOrDefault().TextChanged += (s, e) =>
                            {
                                if (resetOn)
                                    return;

                                string old = rangeTextBoxesFilter;
                                rangeTextBoxesFilter = null;

                                foreach (string text in rangeTextBoxes)
                                {
                                    if (Controls.Find(text + "1", true).FirstOrDefault().Text == "" || Controls.Find(text + "2", true).FirstOrDefault().Text == "")
                                        continue;
                                    if (Convert.ToInt32(Controls.Find(text + "1", true).FirstOrDefault().Text) - Convert.ToInt32(Controls.Find(text + "2", true).FirstOrDefault().Text) > 0)
                                        continue;

                                    rangeTextBoxesFilter += "([" + text + "] > '" + Controls.Find(text + "1", true).FirstOrDefault().Text + "' AND [" + text + "] < '" + Controls.Find(text + "2", true).FirstOrDefault().Text + "') AND ";
                                }
                                if (rangeTextBoxesFilter != null)
                                    rangeTextBoxesFilter = rangeTextBoxesFilter.Remove(rangeTextBoxesFilter.Length - 4, 4);

                                if(rangeTextBoxesFilter != null)
                                    if(!rangeTextBoxesFilter.Equals(old))
                                        OnFilterChanged?.Invoke(this, new EventArgs());
                            };

                            Controls.Find(col.Name + "2", true).FirstOrDefault().TextChanged += (s, e) =>
                            {
                                if (resetOn)
                                    return;

                                string old = rangeTextBoxesFilter;
                                rangeTextBoxesFilter = null;

                                foreach (string text in rangeTextBoxes)
                                {
                                    if (Controls.Find(text + "1", true).FirstOrDefault().Text == "" || Controls.Find(text + "2", true).FirstOrDefault().Text == "")
                                        continue;
                                    if (Convert.ToInt32(Controls.Find(text + "1", true).FirstOrDefault().Text) - Convert.ToInt32(Controls.Find(text + "2", true).FirstOrDefault().Text) > 0)
                                        continue;

                                    rangeTextBoxesFilter += "([" + text + "] > '" + Controls.Find(text + "1", true).FirstOrDefault().Text + "' AND [" + text + "] < '" + Controls.Find(text + "2", true).FirstOrDefault().Text + "') AND ";
                                }
                                if (rangeTextBoxesFilter != null)
                                    rangeTextBoxesFilter = rangeTextBoxesFilter.Remove(rangeTextBoxesFilter.Length - 4, 4);

                                if (rangeTextBoxesFilter != null)
                                    if (!rangeTextBoxesFilter.Equals(old))
                                        OnFilterChanged?.Invoke(this, new EventArgs());
                            };

                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBoxTi.Show("SQLRequestFilter " + e.Message);
            }

            Supports.GangeGroup(this);

        }

        private void ResetAllNodes()
        {
            foreach (TreeNode nod in tree.Nodes)
            {
                foreach (TreeNode no in nod.Nodes)
                    no.Checked = false;
                nod.Checked = false;
            }
        }

        private void ResetAllText(Control control)
        {
            foreach (Control con in control.Controls)
            {
                if (con.GetType() == typeof(TextBox))
                    con.Text = null;
                if (con.GetType() == typeof(DateTimePicker))
                    ((DateTimePicker)con).Value = DateTime.Now;
                ResetAllText(con);
            }
        }

        public void RefreshNodes(DataGridView dataGridView)
        {
            if (dataGridView == null)
                return;

            bool doNotRemove = false;
            List<string> indexes = new List<string>();
            foreach (TreeNode tn in tree.Nodes)
            {
                foreach (TreeNode tnod in tn.Nodes)
                {
                    if (tnod.Checked == true)
                    {
                        doNotRemove = true;
                        continue;
                    }
                }
                if (doNotRemove)
                {
                    doNotRemove = false;
                    continue;
                }
                else
                    indexes.Add(tn.Name);
            }

            foreach (string tnod in indexes)
                tree.Nodes.RemoveByKey(tnod);





            foreach (DataGridViewColumn col in dataGridView.Columns)
            {
                List<string> distinctConditions = dataGridView.Rows.Cast<DataGridViewRow>().Select(x => x.Cells[col.Name].Value.ToString()).Where(x => x != "").OrderBy(x => x).Distinct().ToList();

                if (columnsFilter.Contains(col.Name) || col.Name == "ID" || distinctConditions.Count < 2 || (col.ValueType != typeof(string) && col.ValueType != typeof(bool)))
                    continue;

                tree.Nodes.Add(col.Name, col.HeaderText);

                foreach (string str in distinctConditions)
                {
                    if (col.ValueType != typeof(bool))
                        tree.Nodes.Find(col.Name, false).FirstOrDefault().Nodes.Add(str.ToString(), str.ToString());
                    else
                    {
                        if (str == "True")
                            tree.Nodes.Find(col.Name, false).FirstOrDefault().Nodes.Add(str.ToString(), "Вкл.");
                        else
                            tree.Nodes.Find(col.Name, false).FirstOrDefault().Nodes.Add(str.ToString(), "Выкл.");
                    }
                }
            }
        }
    }
}