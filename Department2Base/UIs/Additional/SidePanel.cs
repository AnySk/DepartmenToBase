using Department2Base.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Department2Base
{
    public class SidePanel : Panel
    {

        public SidePanel(string name)
        {
            try
            {
                BackColor = Supports.backBlack;

                Dock = DockStyle.Fill;

                Controls.Add(new Panel()
                {
                    Name = "Body",
                    Dock = DockStyle.Fill,
                });

                (Controls.Find("Body", false).FirstOrDefault() as Panel).ControlAdded += (s, e) =>
                {
                    Supports.GangeGroup(this);
                };


                Controls.Add(new Panel()
                {
                    Name = "Head",

                    Dock = DockStyle.Top,
                    Height = 20,
                    BackColor = Supports.groupGrey,
                });

                (Controls.Find("Head", false).FirstOrDefault() as Panel).Controls.Add(new Label
                {
                    TextAlign = System.Drawing.ContentAlignment.BottomLeft,
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                    Text = name,
                });

                (Controls.Find("Head", false).FirstOrDefault() as Panel).Controls.Add(new PictureBox()
                {
                    Name = "HeadCloser",
                    Dock = DockStyle.Right,
                    Width = 20,
                    Image = Resources.ex,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Padding = new Padding(2),
                });

                (Controls.Find("HeadCloser", true).FirstOrDefault() as PictureBox).MouseEnter += (s, e) => (s as PictureBox).BackColor = Supports.LiteHeadBlue;

                (Controls.Find("HeadCloser", true).FirstOrDefault() as PictureBox).MouseLeave += (s, e) => (s as PictureBox).BackColor = Color.Transparent;

                (Controls.Find("HeadCloser", true).FirstOrDefault() as PictureBox).MouseDown += (s, e) =>
                {
                    if (MouseButtons.Left == e.Button)
                        (Controls.Find("HeadCloser", true).FirstOrDefault() as PictureBox).BackColor = Supports.darkBlue;
                };

                (Controls.Find("HeadCloser", true).FirstOrDefault() as PictureBox).MouseUp += (s, e) =>
                {
                    if (MouseButtons.Left == e.Button && (s as PictureBox).ClientRectangle.Contains((s as PictureBox).PointToClient(MousePosition)))
                        Dispose();
                };
            }
            catch (Exception e)
            {
                MessageBoxTi.Show("SidePanel " + e.Message);
            }
        }
    }
}
