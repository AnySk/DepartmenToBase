using Department2Base.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Department2Base
{
    public partial class LoginForm : Form
    {
        private Timer t = null;
        Point imageLoca;
        int imageWidth = -1;
        private bool gotIn = false;
        int UsersColorChoise = -1;
        string login;

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
        public LoginForm()
        {
            try
            {
                Opacity = 0;
                InitializeComponent();
                imageLoca = pictureBox2.Location;
                imageWidth = pictureBox2.Width;
                label2.Text = Application.ProductVersion;

                t = new Timer();
                t.Tick += (s, e) =>
                {
                    if ((Opacity += 0.05d) >= 1)
                        t.Stop();
                };
                t.Interval = 16;
                t.Start();


                ThisControl(this);

                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;

                pictureBox1.BackColor = Color.Transparent;
                pictureBox1.Image = button1.BackColor.R > 122 ? Resources.x1tb : Resources.x1tw;


                textBox1.TextChanged += (s, e) =>
                {
                    login = textBox1.Text;
                };

                textBox2.Enter += (s, e) =>
                {
                    if (login == textBox1.Text)
                    {
                        login = null;
                    }
                    else
                        return;


                    DataRow da = dataBase.SimpleData("Login").Select().Where(x => x["Login"].Equals(textBox1.Text)).FirstOrDefault();
                    if (da != null)
                    {
                        pictureBox2.Location = imageLoca;
                        pictureBox2.Width = imageWidth;
                        pictureBox2.Height = imageWidth;

                        if (da["Face"] != null)
                            pictureBox2.Image = (Bitmap)Image.FromStream(new MemoryStream((byte[])da["Face"]));
                        int picX = pictureBox2.Location.X;

                        int picWidth = pictureBox2.Width;
                        Point po = new Point(pictureBox2.Location.X, pictureBox2.Location.Y);

                        t = new Timer();
                        bool bigOrSmall = false;
                        int i = 0;


                        t.Tick += (se, a) =>
                        {
                            if (pictureBox2.Height >= 135 && pictureBox2.Width >= 135)
                            {
                                if (pictureBox2.Height > pictureBox2.Width)
                                {
                                    pictureBox2.Width += 4;
                                    po.X -= 2;
                                    pictureBox2.Location = po;
                                    return;
                                }


                                t.Stop();
                                button1.Enabled = true;
                                textBox1.Enabled = true;
                                textBox2.Enabled = true;
                                return;
                            }

                            picWidth += 2 + i;
                            pictureBox2.Height += 2 + i;

                            if (t.Interval > 1)
                                t.Interval -= 1;



                            if (!bigOrSmall)
                            {
                                pictureBox2.Width -= 4 + i;
                                po.X += 2 + i / 2;
                                pictureBox2.Location = po;
                                if (pictureBox2.Width == 0)
                                {
                                    bigOrSmall = true;
                                    pictureBox2.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                    i += 2;
                                }
                            }
                            if (bigOrSmall)
                            {
                                pictureBox2.Width += 4 + i;
                                po.X -= 2 + i / 2;
                                pictureBox2.Location = po;
                                if (picWidth <= pictureBox2.Width)
                                {
                                    bigOrSmall = false;
                                }
                            }

                        };
                        t.Interval = 50;
                        t.Start();

                    }
                    else
                        pictureBox2.Image = null;
                };

                button1.Click += (s, e) =>
                {
                    t.Stop();
                    pictureBox2.Width = 156;
                    pictureBox2.Height = 156;
                    pictureBox2.Location = new Point(153, 133);

                    GetIn();


                };

                textBox2.KeyUp += (s, e) =>
                {
                    if (e.KeyCode == Keys.Enter && textBox2.Focused)
                    {
                        button1.PerformClick();
                    }
                };

                textBox1.KeyUp += (s, e) =>
                {
                    if (e.KeyCode == Keys.Enter && textBox1.Focused)
                    {
                        button1.PerformClick();
                        Focus();
                    }
                };

                pictureBox1.MouseEnter += (s, e) =>
                {
                    pictureBox1.BackColor = Supports.liteTextGray;
                };

                pictureBox1.MouseLeave += (s, e) =>
                {
                    pictureBox1.BackColor = Color.Transparent;
                };

                pictureBox1.Click += (s, e) =>
                {
                    Application.Exit();
                };

                pictureBox1.MouseDown += (s, e) =>
                {
                    pictureBox1.BackColor = Supports.headBlue;
                };

                pictureBox1.MouseUp += (s, e) =>
                {
                    pictureBox1.BackColor = Color.Transparent;
                };
            }
            catch (Exception e)
            {
                MessageBoxTi.Show("LoginForm " + e.Message);
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            UsersColorChoise = 2;

            pictureBox4.Image = Resources.darkch;
            pictureBox3.Image = Resources.liteunch;
            Supports.theme = true;

            Supports.headGrey = Supports.ColorSetDark.headGrey;
            Supports.headliteGrey = Supports.ColorSetDark.headliteGrey;
            Supports.textBlack = Supports.ColorSetDark.textBlack;
            Supports.darkBlue = Supports.ColorSetDark.darkBlue;
            Supports.backBlack = Supports.ColorSetDark.backBlack;
            Supports.textGray = Supports.ColorSetDark.textGray;
            Supports.liteTextGray = Supports.ColorSetDark.liteTextGray;
            Supports.headBlue = Supports.ColorSetDark.headBlue;
            Supports.headOrange = Supports.ColorSetDark.headOrange;
            Supports.textWhite = Supports.ColorSetDark.textWhite;
            Supports.groupGrey = Supports.ColorSetDark.groupGrey;
            Supports.Red = Supports.ColorSetDark.Red;
            Supports.Green = Supports.ColorSetDark.Green;
            Supports.LiteHeadBlue = Supports.ColorSetDark.LiteHeadBlue;
            Supports.LiteTextBlue = Supports.ColorSetDark.LiteTextBlue;
            Supports.StringBrush = Supports.ColorSetDark.StringBrush;
            Supports.StringPen = Supports.ColorSetDark.StringPen;


            ThisControl(this);

            pictureBox1.Image = button1.BackColor.R > 122 ? Resources.x1tb : Resources.x1tw;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            UsersColorChoise = 1;

            pictureBox3.Image = Resources.litech;
            pictureBox4.Image = Resources.darkunch;
            Supports.theme = false;

            Supports.headGrey = Supports.ColorSetLite.headGrey;
            Supports.headliteGrey = Supports.ColorSetLite.headliteGrey;
            Supports.textBlack = Supports.ColorSetLite.textBlack;
            Supports.darkBlue = Supports.ColorSetLite.darkBlue;
            Supports.backBlack = Supports.ColorSetLite.backBlack;
            Supports.textGray = Supports.ColorSetLite.textGray;
            Supports.liteTextGray = Supports.ColorSetLite.liteTextGray;
            Supports.headBlue = Supports.ColorSetLite.headBlue;
            Supports.headOrange = Supports.ColorSetLite.headOrange;
            Supports.textWhite = Supports.ColorSetLite.textWhite;
            Supports.groupGrey = Supports.ColorSetLite.groupGrey;
            Supports.Red = Supports.ColorSetLite.Red;
            Supports.Green = Supports.ColorSetLite.Green;
            Supports.LiteHeadBlue = Supports.ColorSetLite.LiteHeadBlue;
            Supports.LiteTextBlue = Supports.ColorSetLite.LiteTextBlue;
            Supports.StringBrush = Supports.ColorSetLite.StringBrush;
            Supports.StringPen = Supports.ColorSetLite.StringPen;

            ThisControl(this);
            pictureBox1.Image = button1.BackColor.R > 122 ? Resources.x1tb : Resources.x1tw;
        }

        private void ThisControl(Control cont)
        {
            foreach (Control gro in cont.Controls)
            {
                if (gro.GetType() == typeof(Button))
                {
                    ((Button)gro).FlatStyle = FlatStyle.Flat;
                    ((Button)gro).ForeColor = Supports.textWhite;
                    ((Button)gro).BackColor = Supports.textGray;
                }
                if (gro.GetType() == typeof(TextBox))
                {
                    ((TextBox)gro).ForeColor = Supports.textWhite;
                    ((TextBox)gro).BackColor = Supports.textGray;
                }
                ThisControl(gro);
            }
        }

        private void GetIn()
        {
            try {
                if(Convert.ToBoolean(dataBase.SimpleData("[Login] WHERE Login='" + textBox1.Text + "'").Rows[0]["Allowed"]).Equals(false))
                {
                    MessageBoxTi.Show("К сожалению вас заблокировали, обратитесь к администратору");
                    return;
                }
                if (gotIn)
                    return;
                if (textBox1.Text == "")
                {
                    toolTip1.Show("Введите логин!", textBox1, textBox1.Width, 0, 2000);
                    return;
                }
                else if (textBox2.Text == "")
                {
                    toolTip1.Show("Введите пароль!", textBox2, textBox1.Width, 0, 2000);
                    return;
                }
                else
                {
                    var Login = dataBase.SimpleData("[Login] WHERE Login='" + textBox1.Text + "'");

                    if (Login.Rows.Count == 0)
                    {
                        toolTip1.Show("Неверный логин или пароль", textBox1, textBox1.Width, 0, 2000);
                        return;
                    }


                    if (Login.Rows[0]["Login"].ToString() == textBox1.Text
                            && Login.Rows[0]["Password"].ToString() == textBox2.Text)
                    {
                        gotIn = true;

                        dataBase.SimpleRequest("INSERT INTO [dbo].[MainLog] ([Who] ,[What] ,[WhenItWas])" +
                            "SELECT[Login], 'Вышел из системы', [LastSeen] FROM(SELECT[Login], (SELECT top 1[What] FROM[MainLog] WHERE[Who] = [Login].[Login] ORDER BY[WhenItWas] DESC) AS[What], [LastSeen]" +
                            "FROM[dbo].[Login] WHERE[LastSeen] is not null AND[LastSeen] < GETDATE() - '00:00:02.000') A WHERE[What] != 'Вышел из системы'");

                        if ((int)dataBase.ToCount("SELECT count(*) FROM [MainSettings] WHERE [Who] = 'ColorStyle' AND  [What] = '" + Login.Rows[0]["Login"].ToString() + "'") == 0)
                        {
                            if (Supports.textWhite == Supports.ColorSetDark.textWhite)
                                dataBase.SimpleRequest("INSERT INTO [MainSettings] ([Who], [What], [Content]) VALUES ('ColorStyle' , '" + Login.Rows[0]["Login"].ToString() + "', 'Dark')");
                            else
                                dataBase.SimpleRequest("INSERT INTO [MainSettings] ([Who], [What], [Content]) VALUES ('ColorStyle' , '" + Login.Rows[0]["Login"].ToString() + "', 'Lite')");
                        }
                        else
                        {
                            if (UsersColorChoise != -1)
                            {
                                if (UsersColorChoise == 1)
                                {
                                    dataBase.SimpleRequest("UPDATE [MainSettings] SET [Content] = 'Lite'  WHERE [Who] = 'ColorStyle' AND [What] = '" + Login.Rows[0]["Пользователь"].ToString() + "'");
                                }
                                else
                                {
                                    dataBase.SimpleRequest("UPDATE [MainSettings] SET [Content] = 'Dark'  WHERE [Who] = 'ColorStyle' AND [What] = '" + Login.Rows[0]["Пользователь"].ToString() + "'");
                                }
                            }
                            else if ((string)dataBase.ToCount("SELECT [Content] FROM [MainSettings] WHERE [Who] = 'ColorStyle' AND  [What] = '" + Login.Rows[0]["Login"].ToString() + "'") == "Dark")
                                pictureBox4_Click(this, new EventArgs());


                        }

                            object exist = dataBase.ToCount("SELECT [LastSeen] FROM [dbo].[Login] WHERE [dbo].[Login].[Login] = '" + textBox1.Text + "'");
                            if (exist != null)
                                if (((DateTime)exist).AddSeconds(2) > Convert.ToDateTime(dataBase.ToCount("SELECT GETDATE()")))
                                {
                                    MessageBoxTi.Show("Другой пользователь уже вошёл в в базу под тиким логином");
                                    gotIn = false;
                                    return;
                                }


                            Hide();
                            Profile form = null;

                                if (!Convert.IsDBNull(Login.Rows[0]["Face"]))
                                    form = new Profile(Login.Rows[0]["Login"].ToString(), face: (Bitmap)Image.FromStream(new MemoryStream((byte[])Login.Rows[0]["Face"])));
                                else
                                    form = new Profile(Login.Rows[0]["Login"].ToString());
                          
                            form.FormClosed += (se, args) => Close();
                            form.Show();
                            return;
                    }
                    else
                    {
                        toolTip1.Show("Неверный логин или пароль", textBox1, textBox1.Width, 0, 2000);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBoxTi.Show("LoginForm.GetIn " + e.Message);
            }
        }
    }
}