using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantPOS.Models
{
    public class Inventory : INotifyPropertyChanged
    {
        private string name;
        private double quantity;
        private string unit;

        public string Name {
            get { return this.name; }
            set
            {
                if (value != this.name)
                {
                    this.name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public double Quantity
        {
            get { return this.quantity; }
            set {
                if (value != this.quantity)
                {
                    this.quantity = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Unit
        {
            get { return this.unit; }
            set
            {
                if (value != this.unit)
                {
                    this.unit = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
