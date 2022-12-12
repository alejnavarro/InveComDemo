using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InveComv1.RCS.Templates
{
    public class TabContentTemplate
    {
        private string header;
        private FrameworkElement content;

        public string Header { get => header; set => header = value; }
        public FrameworkElement TabContent { get => content; set => content = value; }
    }
}
