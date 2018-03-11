using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RestaurantPOS.Models
{
    public class Table
    {
        public Table()
        {

        }

        public Table(Point point)
        {
            this.UpperLeftPoint = point;
        }

        public Point UpperLeftPoint { get; set; }
    }
}
