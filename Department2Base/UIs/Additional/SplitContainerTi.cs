using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Department2Base
{
    class SelectablePanelTi : Panel
    {
        public SelectablePanelTi()
        {
            SetStyle(ControlStyles.Selectable, true);
        }
    }
}
