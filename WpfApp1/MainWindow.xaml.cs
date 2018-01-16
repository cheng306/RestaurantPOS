using System;
using System.Collections.Generic;
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
        //database directory
        DirectoryInfo dbDirectory;
        String itemListXmlPath;

        public MainWindow()
        {
            InitializeComponent();
            Console.WriteLine("================start==================");
            DirectoryInfo dbDirectory = new DirectoryInfo("db");
            itemListXmlPath = @"db\itemlist.xml";

            if (!dbDirectory.Exists)
            {
                dbDirectory.Create();
            }

            if (File.Exists(itemListXmlPath))
            {
                XmlSerializer reader = new XmlSerializer(typeof(List<Item>));

            }
            else
            {
                Console.WriteLine("itemList created");
                editPage.itemList = new List<Item>();
            }

            


            //DirectoryInfo di2 = di.Parent;


            //Console.WriteLine(di2.Name);
            //Console.WriteLine(di2.FullName);

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {




            XmlSerializer writer = new XmlSerializer(typeof(List<Item>));

            
            Console.WriteLine(itemListXmlPath);
            FileStream fs = File.Create(itemListXmlPath);
            List<Item> list = editPage.ItemList;

            writer.Serialize(fs, list);
            fs.Close();
        }
    }
}
