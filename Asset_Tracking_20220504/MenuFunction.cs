using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_Tracking_20220504
{
    internal class MenuFunction
    {
        public MenuFunction(string description, Action action)
        {
            Description = description;
            Action = action;
        }
        public string Description { get; set; }
        public Action Action { get; set; }
    }

}
