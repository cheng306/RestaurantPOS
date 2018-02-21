using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class Item : INotifyPropertyChanged
    {
        private List<InventoryConsumption> inventoryConsumptionList;

        public int Id { get; set; }

        public string Name{ get;set;}

        public string Category { get; set; }

        public  double Price { get; set; }

        public DateTime AddTime { get; set; }

        public List<InventoryConsumption> InventoryConsumptionList {
            get { return this.inventoryConsumptionList; }
            set
            {
                if (value != this.inventoryConsumptionList)
                {
                    this.inventoryConsumptionList = value;
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

    public class InventoryConsumption
    {
        public string InventoryName
        {
            get; set;
        }
        public double ConsumptionQUantity
        {
            get; set;
        }

    }
}
