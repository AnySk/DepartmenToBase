using Department2Base.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Department2Base
{
    /// <summary>
    /// Результаты для MessageBoxTi.
    /// </summary>
    public enum MessageResult
    {
        Btn0 = 0,
        Btn1 = 1,
        Btn2 = 2,
        Btn3 = 3,
        Btn4 = 4,
        Btn5 = 5,
        None = 6,
        OK = 7,
        Cancel = 8,
        Abort = 9,
        Ignore = 10,
        Yes = 11,
        No = 12,
    }
    public static class MessageBoxTi
    {

        /// <summary>
        /// Каркас для MessageBoxTi 
        /// </summary>
        private class MessageBox : Form
        {
            protected override void WndProc(ref Message m)
            {
                switch (m.Msg)
                {
                    case 0xA3:
                        m.Result = (IntPtr)0x1;
                        return;
                    case 0x84:
                        base.WndProc(ref m);
                        if ((int)m.Result == 0x1)
                            m.Result = (IntPtr)0x2;
                        return;
                }
                base.WndProc(ref m);
            }

            public MessageBox(string headText = null, int height = 150, int width = 500, bool headVasible = true)
            {
                try
                {
                    Padding = new Padding(2);
                    BackColor = Supports.headBlue;
                    TopMost = true;
                    Width = width;
                    Height = height;
                    ShowIcon = false;
                    StartPosition = FormStartPosition.CenterScreen;
                    FormBorderStyle = FormBorderStyle.None;

                    Controls.Add(new Panel()
                    {
                        Name = "Body",
                        Dock = DockStyle.Fill,
                        BackColor = Supports.liteTextGray,
                    });

                    Controls.Add(new TransPanel()
                    {
                        Name = "Head",
                        Visible = headVasible,
                        Dock = DockStyle.Top,
                        Height = 27,
                        BackColor = Supports.headBlue,
                    });

                    Controls.Find("Head", false).FirstOrDefault().Controls.Add(new PictureBox()
                    {
                        Name = "Closer",
                        Dock = DockStyle.Right,
                        Width = 35,
                        BackColor = Supports.Red,
                        Image = Properties.Resources.x1tw,
                    });

                    Controls.Find("Head", false).FirstOrDefault().Controls.Add(new TransLabel()
                    {
                        Text = headText,
                        Name = "HeadTransLabel",
                        Dock = DockStyle.Fill,
                        Padding = new Padding(4, 6, 0, 0),
                        ForeColor = Supports.textWhite,
                    });

                    Controls.Find("Closer", true).FirstOrDefault().MouseUp += (s, e) =>
                    {
                        if (Controls.Find("Closer", true).FirstOrDefault().ClientRectangle.Contains(Controls.Find("Closer", true).FirstOrDefault().PointToClient(MousePosition)))
                            Dispose();
                        else
                            Controls.Find("Closer", true).FirstOrDefault().BackColor = Supports.Red;
                    };


                    Controls.Find("Closer", true).FirstOrDefault().MouseDown += (s, e) =>
                        Controls.Find("Closer", true).FirstOrDefault().BackColor = Color.OrangeRed;

                    Controls.Find("Closer", true).FirstOrDefault().MouseEnter += (s, e) =>
                        Controls.Find("Closer", true).FirstOrDefault().BackColor = Color.IndianRed;

                    Controls.Find("Closer", true).FirstOrDefault().MouseLeave += (s, e) =>
                        Controls.Find("Closer", true).FirstOrDefault().BackColor = Supports.Red;
                }
                catch (Exception e)
                {
                    MessageBoxTi.Show("MessageBoxTi " + e.Message);
                }
            }
        }

        /// <summary>
        /// Выводит диалоговое окно для оповещения
        /// </summary>
        /// <param name="mainText">
        /// Текст, отобржаемый в окне
        /// </param>
        /// <returns>
        /// MessageResult в соответствии с нажатой кнопкой
        /// </returns>
        public static MessageResult Show(string mainText)
        {
            string trueMainText = mainText;
            if (trueMainText.Length > 150)
                trueMainText = trueMainText.Remove(150);

            MessageResult result = MessageResult.None;

            using (var form = new MessageBox(height: 150, width: 500))
            {
                form.Controls.Find("Body", false).FirstOrDefault().Controls.Add(new TableLayoutPanel()
                {
                    Name = "TableLayoutPanel",
                    Dock = DockStyle.Bottom,
                    Height = 40,
                });

                form.Controls.Find("Body", false).FirstOrDefault().Controls.Add(new Label()
                {
                    Font = new Font("Times New Roman", 10),
                    Text = trueMainText,
                    Name = "BodyTransLabel",
                    Dock = DockStyle.Top,
                    Padding = new Padding(4, 6, 0, 0),
                    MaximumSize = new Size(500, 100),
                    Height = 80,
                    BorderStyle = BorderStyle.None,
                    AutoSize = true,
                    BackColor = Supports.liteTextGray,
                    ForeColor = Supports.textWhite,
                });

                TableLayoutPanel body = (form.Controls.Find("TableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel);
                body.RowStyles.Clear();
                body.ColumnStyles.Clear();
                body.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
                body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
                body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
                body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));

                body.Controls.Add(new Button()
                {
                    Name = "OK",
                    Text = "OK",
                    Dock = DockStyle.Fill,
                }, 1, 0);
                body.Controls.Find("OK", false).FirstOrDefault().Click += (s, e) =>
                {
                    result = MessageResult.OK;
                    form.Dispose();
                };

                Supports.GangeGroup(body);

                form.ShowDialog();
            }


            return result;
        }

        /// <summary>
        /// Выводит окно с тремя кнопками. "Да", "Нет", "Отмена"
        /// </summary>
        /// <param name="headText">
        /// Текст в заголовке
        /// </param>
        /// <param name="mainText">
        /// Уточняющий текст
        /// </param>
        /// <returns>
        /// MessageResult в соответствии с нажатой кнопкой
        /// </returns>
        public static MessageResult Show(string headText, string mainText)
        {
            string trueHeadText = headText;
            string trueMainText = mainText;
            if (trueHeadText.Length > 75)
                trueHeadText = trueHeadText.Remove(75);
            if (trueMainText.Length > 200)
                trueMainText = trueMainText.Remove(200);

            MessageResult result = MessageResult.None;

            using (var form = new MessageBox(trueHeadText))
            {
                form.Controls.Find("Body", false).FirstOrDefault().Controls.Add(new TableLayoutPanel()
                {
                    Name = "TableLayoutPanel",
                    Dock = DockStyle.Bottom,
                    Height = 40,
                });

                form.Controls.Find("Body", false).FirstOrDefault().Controls.Add(new Label()
                {
                    Font = new Font("Times New Roman", 10),
                    Text = trueMainText,
                    Name = "BodyTransLabel",
                    Dock = DockStyle.Top,
                    Padding = new Padding(4, 6, 0, 0),
                    MaximumSize = new Size(500, 100),
                    Height = 80,
                    BorderStyle = BorderStyle.None,
                    AutoSize = true,
                    BackColor = Supports.liteTextGray,
                    ForeColor = Supports.textWhite,
                });

                TableLayoutPanel body = (form.Controls.Find("TableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel);
                body.RowStyles.Clear();
                body.ColumnStyles.Clear();
                body.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
                body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
                body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
                body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));

                body.Controls.Add(new Button()
                {
                    Name = "Yes",
                    Text = "Да",
                    Dock = DockStyle.Fill,
                }, 0, 0);
                body.Controls.Find("Yes", false).FirstOrDefault().Click += (s, e) =>
                {
                    result = MessageResult.Yes;
                    form.Dispose();
                };

                body.Controls.Add(new Button()
                {
                    Name = "No",
                    Text = "Нет",
                    Dock = DockStyle.Fill,
                }, 1, 0);
                body.Controls.Find("No", false).FirstOrDefault().Click += (s, e) =>
                {
                    result = MessageResult.No;
                    form.Dispose();
                };

                body.Controls.Add(new Button()
                {
                    Name = "Cancel",
                    Text = "Отменить",
                    Dock = DockStyle.Fill,
                }, 2, 0);
                body.Controls.Find("Cancel", false).FirstOrDefault().Click += (s, e) =>
                {
                    result = MessageResult.Cancel;
                    form.Dispose();
                };

                Supports.GangeGroup(body);

                form.ShowDialog();
            }


            return result;
        }

        /// <summary>
        /// Выводит диалоговое окно с кастомными кнопками, при нажатии на любую возвращается соответствующий результат Btn0, Btn1?
        /// </summary>
        /// <param name="headText"></param>
        /// <param name="mainText"></param>
        /// <param name="buttonNames"></param>
        /// <returns></returns>
        public static MessageResult Show(string headText, string mainText, string[] buttonNames)
        {
            string trueHeadText = headText;
            string trueMainText = mainText;
            if (trueHeadText.Length > 75)
                trueHeadText = trueHeadText.Remove(75);
            if (trueMainText.Length > 200)
                trueMainText = trueMainText.Remove(200);

            MessageResult result = MessageResult.None;

            using (var form = new MessageBox(trueHeadText))
            {
                form.Controls.Find("Body", false).FirstOrDefault().Controls.Add(new TableLayoutPanel()
                {
                    Name = "TableLayoutPanel",
                    Dock = DockStyle.Bottom,
                    Height = 80,
                });

                form.Controls.Find("Body", false).FirstOrDefault().Controls.Add(new Label()
                {
                    Font = new Font("Times New Roman", 10),
                    Text = trueMainText,
                    Name = "BodyTransLabel",
                    Dock = DockStyle.Top,
                    Padding = new Padding(4, 6, 0, 0),
                    MaximumSize = new Size(500, 100),
                    Height = 80,
                    BorderStyle = BorderStyle.None,
                    AutoSize = true,
                    BackColor = Supports.liteTextGray,
                    ForeColor = Supports.textWhite,
                });

                TableLayoutPanel body = (form.Controls.Find("TableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel);
                body.RowStyles.Clear();
                body.ColumnStyles.Clear();
                body.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
                body.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
                body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
                body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
                body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));

                if (buttonNames.Length > 3)
                {
                    int k = 0;
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {

                            if (k >= buttonNames.Length)
                                break;

                            body.Controls.Add(new Button()
                            {
                                Name = buttonNames[k],
                                Text = buttonNames[k],
                                Dock = DockStyle.Fill,
                            }, j, i);

                            body.Controls.Find(buttonNames[k], false).FirstOrDefault().Click += (s, e) =>
                            {
                                result = (MessageResult)buttonNames.Cast<string>().ToList().IndexOf((s as Button).Name);
                                form.Dispose();
                            };
                            k++;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (j >= buttonNames.Length)
                            break;

                        body.Controls.Add(new Button()
                        {
                            Name = buttonNames[j],
                            Text = buttonNames[j],
                            Dock = DockStyle.Fill,
                        }, j, 1);

                        body.Controls.Find(buttonNames[j], false).FirstOrDefault().Click += (s, e) =>
                        {
                            result = (MessageResult)buttonNames.Cast<string>().ToList().IndexOf((s as Button).Name);
                            form.Dispose();
                        };
                    }
                }

                Supports.GangeGroup(body);
                form.ShowDialog();
            }
            return result;
        }

        public static string Show(string headText, string mainText, HorizontalAlignment textAlign)
        {
            string trueHeadText = headText;
            string trueMainText = mainText;
            if (trueHeadText.Length > 75)
                trueHeadText = trueHeadText.Remove(75);
            if (trueMainText.Length > 200)
                trueMainText = trueMainText.Remove(200);

            string result = null;

            using (var form = new MessageBox(trueHeadText))
            {
                form.Controls.Find("Body", false).FirstOrDefault().Controls.Add(new TableLayoutPanel()
                {
                    Name = "TableLayoutPanel",
                    Dock = DockStyle.Bottom,
                    Height = 40,
                });

                form.Controls.Find("Body", false).FirstOrDefault().Controls.Add(new TextBox()
                {
                    Font = new Font("Times New Roman", 12),
                    Name = "BodyTextBox",
                    Dock = DockStyle.Top,
                    TextAlign = textAlign,
                });

                form.Controls.Find("Body", false).FirstOrDefault().Controls.Add(new Label()
                {
                    Font = new Font("Times New Roman", 10),
                    Text = trueMainText,
                    Name = "BodyTextBox",
                    Dock = DockStyle.Top,
                    BorderStyle = BorderStyle.None,
                });

                TableLayoutPanel body = (form.Controls.Find("TableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel);
                body.RowStyles.Clear();
                body.ColumnStyles.Clear();
                body.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
                body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
                body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
                body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));

                body.Controls.Add(new Button()
                {
                    Name = "OK",
                    Text = "OK",
                    Dock = DockStyle.Fill,
                }, 1, 0);
                body.Controls.Find("OK", false).FirstOrDefault().Click += (s, e) =>
                {
                    result = form.Controls.Find("BodyTextBox", true).FirstOrDefault().Text;
                    form.Dispose();
                };

                body.Controls.Add(new Button()
                {
                    Name = "Cancel",
                    Text = "Отменить",
                    Dock = DockStyle.Fill,
                }, 2, 0);
                body.Controls.Find("Cancel", false).FirstOrDefault().Click += (s, e) =>
                {
                    result = "";
                    form.Dispose();
                };

                Supports.GangeGroup(body);
                form.ShowDialog();

                return result;
            }
        }

        /// <summary>
        /// Выводит диалоговое окно для оповещения
        /// </summary>
        /// <param name="mainText">
        /// Текст, отобржаемый в окне
        /// </param>
        /// <returns>
        /// MessageResult в соответствии с нажатой кнопкой
        /// </returns>
        public static void Show()
        {
            using (var form = new MessageBox(height: 126, width: 126, headVasible: false))
            {
                form.Controls.Find("Body", false).FirstOrDefault().Controls.Add(new PictureBox()
                {
                    BackColor = Supports.headBlue,
                    Dock = DockStyle.Fill,
                    Image = Resources.sandWatch,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                });

                form.ShowDialog();
            }
        }

        public static bool Show(HorizontalAlignment textAlign)
        { 
            bool result = false;

            using (var form = new MessageBox("Проверка доступа", 180, 300))
            {
                form.Controls.Find("Body", false).FirstOrDefault().Controls.Add(new TableLayoutPanel()
                {
                    Name = "TableLayoutPanel",
                    Dock = DockStyle.Bottom,
                    Height = 40,
                });


                form.Controls.Find("Body", false).FirstOrDefault().Controls.Add(new GroupBox()
                {
                    Font = new Font("Times New Roman", 10),
                    Text = "Пароль",
                    Name = "PasswordGroupBox",
                    Dock = DockStyle.Top,
                    
                });

                form.Controls.Find("PasswordGroupBox", true).FirstOrDefault().Controls.Add(new TextBox()
                {
                    Font = new Font("Times New Roman", 12),
                    Name = "PasswordTextBox",
                    Dock = DockStyle.Fill,
                    TextAlign = textAlign,
                });


                form.Controls.Find("Body", false).FirstOrDefault().Controls.Add(new GroupBox()
                {
                    Font = new Font("Times New Roman", 10),
                    Text = "Логин",
                    Name = "LoginGroupBox",
                    Dock = DockStyle.Top,
                    Height = 40,
                });

                form.Controls.Find("LoginGroupBox", true).FirstOrDefault().Controls.Add(new TextBox()
                {
                    Font = new Font("Times New Roman", 12),
                    Name = "LoginTextBox",
                    Dock = DockStyle.Fill,
                    TextAlign = textAlign,
                });

                TableLayoutPanel body = (form.Controls.Find("TableLayoutPanel", true).FirstOrDefault() as TableLayoutPanel);
                body.RowStyles.Clear();
                body.ColumnStyles.Clear();
                body.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
                body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
                body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
                body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));

                body.Controls.Add(new Button()
                {
                    Name = "OK",
                    Text = "OK",
                    Dock = DockStyle.Fill,
                }, 1, 0);
                body.Controls.Find("OK", false).FirstOrDefault().Click += (s, e) =>
                {
                    if(Convert.ToInt32(dataBase.ToCount("SELECT COUNT(*) FROM [Login] WHERE [Login] = '" + form.Controls.Find("LoginTextBox", true).FirstOrDefault().Text + "' AND [Password] = '" + form.Controls.Find("PasswordTextBox", true).FirstOrDefault().Text + "' AND [Rank] > 9")) == 0)
                    {
                        result = false;
                    }
                    else
                        result = true;

                    form.Dispose();
                };

                body.Controls.Add(new Button()
                {
                    Name = "Cancel",
                    Text = "Отменить",
                    Dock = DockStyle.Fill,
                }, 2, 0);
                body.Controls.Find("Cancel", false).FirstOrDefault().Click += (s, e) =>
                {
                    result = false;
                    form.Dispose();
                };

                Supports.GangeGroup(body);
                form.ShowDialog();

                return result;
            }
        }
    }
}
