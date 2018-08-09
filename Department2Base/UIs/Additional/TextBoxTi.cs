using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Department2Base
{
    class TextBoxTi : TextBox
    {
        bool onluNum = false;

        public bool OnlyNumbers
        {
            get
            {
                return onluNum;
            }
            set
            {
                onluNum = value;
            }
        }

        

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (onluNum && !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
            base.OnKeyPress(e);
        }

        public TextBoxTi()
        {

        }
    }
}
