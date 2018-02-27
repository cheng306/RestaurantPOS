using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1.Models
{
    public class Table
    {
        public Table()
        {

        }

        public Table(Point point)
        {
            this.Center = point;
        }

        public Point Center { get; set; }
    }
}
