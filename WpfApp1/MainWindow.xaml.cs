using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
  
        //path of the itemlist.xml
        String itemListXmlPath;



        XmlSerializer listSerializer;

        public MainWindow()
        {
            InitializeComponent();

            Console.WriteLine("================Window start==================");
            

            DirectoryInfo dbDirectory = new DirectoryInfo("db");
            itemListXmlPath = @"db\itemlist.xml";

            listSerializer = new XmlSerializer(typeof(ObservableCollection<Item>));

            if (!dbDirectory.Exists)
            {
                dbDirectory.Create();
            }

            if (File.Exists(itemListXmlPath))
            {
                
                StreamReader streamReader = new StreamReader(itemListXmlPath);
                editPage.itemList2 = (ObservableCollection<Item>)listSerializer.Deserialize(streamReader);
                streamReader.Close();

                Console.WriteLine("itemList loaded");
            }
            else
            {
                
                editPage.itemList2 = new ObservableCollection<Item>();

                Console.WriteLine("itemList created");
            }



            editPage.itemList2.Add(new Item { Name = "asd", Category="asds", Price = 12.6 });
            editPage.itemListView.ItemsSource = editPage.itemList2;

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {




            //XmlSerializer writer = new XmlSerializer(typeof(List<Item>));

            
            
            FileStream fs = File.Create(itemListXmlPath);
            ObservableCollection<Item> list = editPage.itemList2;

            listSerializer.Serialize(fs, list);
            fs.Close();
        }
    }
}
