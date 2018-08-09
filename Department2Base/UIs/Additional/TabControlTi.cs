using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Department2Base
{
    public class TabControlTi : TabControl
    {
        Brush blue = new SolidBrush(Supports.headBlue);
        Brush liteBlue = new SolidBrush(Supports.LiteHeadBlue);
        Brush darkBlue = new SolidBrush(Supports.darkBlue);
        Brush liteLiteBlue = new SolidBrush(Supports.LiteTextBlue);

        public delegate void Closing(object sourse, ClosingEventArgs e);
        public event Closing OnPageClose;

        public class ClosingEventArgs : EventArgs
        {
            int index = -1;
            public ClosingEventArgs(int ClosingPageIndex)
            {
                index = ClosingPageIndex;
            }

            public int ClosingPageIndex
            {
                get
                {
                    return index;
                }
            }
        }

        private bool Closers = true;
        private int CurrentTab = -1;
        private int CurrentCloser = -1;
        private bool needToInvalidate = false;
        private bool mouseDown = false;

        public TabControlTi(bool withClosers = true)
        {
            Closers = withClosers;
            Dock = DockStyle.Fill;
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.Opaque | ControlStyles.UserMouse | ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (CurrentCloser != -1)
                    {
                        needToInvalidate = true;
                        mouseDown = true;
                        Invalidate();
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBoxTi.Show("TabControlTi.OnMouseDown " + ex.Message);
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            try
            {
                Point mouse = MousePosition;
                if (ClientRectangle.Contains(PointToClient(mouse)))
                    if (e.Button == MouseButtons.Left)
                    {
                        if (CurrentCloser != -1 && Closers)
                        {
                            if (SelectedIndex > 0 && SelectedIndex == CurrentTab)
                                SelectedIndex -= 1;

                            OnPageClose?.Invoke(this, new ClosingEventArgs(CurrentCloser));

                            TabPages.RemoveAt(CurrentCloser);


                            CurrentTab = SelectedIndex;

                            if (CurrentTab == -1) return;

                            if (CloserRect(CurrentTab).Contains(PointToClient(mouse)))
                                CurrentCloser = CurrentTab;
                            else if (CurrentCloser != -1) CurrentCloser = -1;

                            return;
                        }
                        else
                            SelectedIndex = CurrentTab;


                        mouseDown = false;
                    }
            }
            catch (Exception ex)
            {
                //MessageBoxTi.Show("TabControlTi.OnMouseUp " + ex.Message);
            }
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            try
            {
                if (CurrentTab != GetIndex())
                {
                    if (GetIndex() != -1)
                        Invalidate();

                    CurrentTab = GetIndex();
                    if (CurrentTab != -1 && CurrentTab != SelectedIndex)
                    {
                        needToInvalidate = true;
                        Invalidate();
                    }
                    else if (CurrentTab == -1)
                    {
                        Invalidate();
                    }
                }

                Point mouse = MousePosition;

                if (CurrentTab == -1) return;

                if (CloserRect(CurrentTab).Contains(PointToClient(mouse)) && CurrentCloser != CurrentTab)
                {
                    CurrentCloser = CurrentTab;
                    needToInvalidate = true;
                    Invalidate();
                }
                else if (!CloserRect(CurrentTab).Contains(PointToClient(mouse)) && CurrentCloser != -1)
                {
                    CurrentCloser = -1;
                    if (CurrentTab != SelectedIndex)
                    {
                        needToInvalidate = true;
                        Invalidate();
                    }
                    else
                    {
                        Invalidate();
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBoxTi.Show("TabControlTi.OnMouseMove " + ex.Message);
            }
            base.OnMouseMove(e);
        }

        int errDote = -1;

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                errDote = 1;
                Rectangle client = ClientRectangle;
                client.X += 2;
                client.Y += 22;
                client.Width -= 10;
                client.Height = 3;

                e.Graphics.Clear(Supports.headGrey);

                for (var i = 0; i < TabPages.Count; i++)
                {
                    if (i == SelectedIndex)
                    {
                        errDote = 2;
                        e.Graphics.FillRectangle(blue, GetTabRect(i));

                        if (Closers)
                        {
                            Rectangle close = CloserRect(i);
                            e.Graphics.FillRectangle(blue, close);
                            e.Graphics.DrawLine(Supports.StringPen, close.X + 3, close.Y + 3, close.X + 11, close.Y + 11);
                            e.Graphics.DrawLine(Supports.StringPen, close.X + 3, close.Y + 11, close.X + 11, close.Y + 3);
                        }
                    }
                    errDote = 3;
                    e.Graphics.DrawString(TabPages[i].Text, Font, Supports.StringBrush, GetTabRect(i).X + 2, GetTabRect(i).Y + 4);
                }

                e.Graphics.FillRectangle(blue, client);

                if (needToInvalidate)
                {
                    errDote = 4;
                    if (CurrentTab == -1)
                    {
                        needToInvalidate = false;
                        return;
                    }

                    if (CurrentTab != SelectedIndex)
                    {
                        errDote = 5;
                        Rectangle tab = GetTabRect(CurrentTab);
                        e.Graphics.FillRectangle(liteBlue, tab);
                        e.Graphics.DrawString(TabPages[CurrentTab].Text, Font, Supports.StringBrush, tab.X + 2, tab.Y + 4);

                        if (Closers)
                        {
                            errDote = 6;
                            Rectangle close = CloserRect(CurrentTab);
                            e.Graphics.DrawLine(Supports.StringPen, close.X + 3, close.Y + 3, close.X + 11, close.Y + 11);
                            e.Graphics.DrawLine(Supports.StringPen, close.X + 3, close.Y + 11, close.X + 11, close.Y + 3);
                        }
                    }

                    if (CurrentCloser != -1 && Closers)
                    {
                        Rectangle close = CloserRect(CurrentCloser);
                        if (mouseDown)
                            e.Graphics.FillRectangle(darkBlue, close);
                        else if (CurrentTab != SelectedIndex)
                            e.Graphics.FillRectangle(liteLiteBlue, close);
                        else
                            e.Graphics.FillRectangle(liteBlue, close);
                        e.Graphics.DrawLine(Supports.StringPen, close.X + 3, close.Y + 3, close.X + 11, close.Y + 11);
                        e.Graphics.DrawLine(Supports.StringPen, close.X + 3, close.Y + 11, close.X + 11, close.Y + 3);
                    }

                    e.Graphics.FillRectangle(blue, client);
                    needToInvalidate = false;
                    return;
                }

            }
            catch (Exception ex)
            {
                //MessageBoxTi.Show("TabControlTi.OnPaint !!!!!!!!!" + errDote + "!!!!!!!!" + ex.Message);
            }

            base.OnPaint(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            try
            {

                CurrentTab = GetIndex();
                needToInvalidate = true;
                Invalidate();


            }
            catch (Exception ex)
            {
                //MessageBoxTi.Show("TabControlTi.OnMouseEnter " + ex.Message);
            }
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            try
            {
                CurrentTab = -1;
                Invalidate();
            }
            catch (Exception ex)
            {
                //MessageBoxTi.Show("TabControlTi.OnMouseLeave " + ex.Message);
            }
            base.OnMouseLeave(e);
        }

        private int GetIndex()
        {
            try
            {
                Point mouse = MousePosition;
                var i = 0;

                while (i < TabCount)
                {

                    if (!GetTabRect(i).Contains(PointToClient(mouse)))
                        i++;
                    else break;

                    if (i == TabCount)
                        return i--;
                }

                return i;
            }
            catch (Exception ex)
            {
                //MessageBoxTi.Show("TabControlTi.GetIndex " + ex.Message);
                return CurrentTab;
            }
        }

        private Rectangle CloserRect(int index)
        {
            try
            {
                Rectangle close = GetTabRect(index);
                close.X += close.Width - 18;
                close.Y += 3;
                close.Width = 15;
                close.Height = 15;

                return close;
            }
            catch (Exception ex)
            {
                //MessageBoxTi.Show("TabControlTi.CloserRect " + ex.Message);
                return Rectangle.Empty;
            }
        }
    }
}
