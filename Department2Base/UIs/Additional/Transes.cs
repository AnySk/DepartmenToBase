using System;
using System.Windows.Forms;

namespace Department2Base
{
    public class TransStrip : MenuStrip
    {
        const int test = 0x84;
        const int trans = -1;

        protected override void WndProc(ref Message m)
        {
            if (!DesignMode && m.Msg == test) m.Result = new IntPtr(trans);
            else base.WndProc(ref m);
        }
    }

    public class TransPanel : Panel
    {
        const int test = 0x84;
        const int trans = -1;

        protected override void WndProc(ref Message m)
        {
            if (!DesignMode && m.Msg == test) m.Result = new IntPtr(trans);
            else base.WndProc(ref m);
        }
    }

    public class TransPictureBox : PictureBox
    {
        const int test = 0x84;
        const int trans = -1;

        protected override void WndProc(ref Message m)
        {
            if (!DesignMode && m.Msg == test) m.Result = new IntPtr(trans);
            else base.WndProc(ref m);
        }
    }

    public class TransLabel : Label
    {
        const int test = 0x84;
        const int trans = -1;

        protected override void WndProc(ref Message m)
        {
            if (!DesignMode && m.Msg == test) m.Result = new IntPtr(trans);
            else base.WndProc(ref m);
        }
    }
}
