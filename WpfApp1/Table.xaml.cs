using System;
using System.Collections.Generic;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Table.xaml
    /// </summary>
    public partial class Table : UserControl
    {
        Circle newTable;
        List<Point> pointList;
        

        public Table()
        {
            InitializeComponent();
            pointList = new List<Point>();
        }

        private void AddButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            newTable = new Circle();
            newTable.SetValue(Canvas.RightProperty, 10.0);
            newTable.SetValue(Canvas.BottomProperty, 10.0);
            SolidColorBrush myBrush = new SolidColorBrush(Colors.Red);
            myBrush.Opacity = 0.5;
            newTable.circleUI.Fill = myBrush;
            newTable.Added = false;

            canvas.Children.Add(newTable);


            newTable.MouseMove += Table_MouseMove;
            newTable.MouseUp += Table_MouseUp;

            Console.WriteLine("Width: " + newTable.circleUI.Width);
        }

        

      

        private void Table_MouseMove(object sender, MouseEventArgs e)
        {
            //Console.WriteLine("Moving");
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point point = e.GetPosition(this.canvas);
                //Circle newTable = (Circle)sender;
                ((Circle)sender).SetValue(Canvas.LeftProperty, point.X - 35.0);
                ((Circle)sender).SetValue(Canvas.TopProperty, point.Y - 35.0);
                if (!Overlap(point))
                {
                    //SolidColorBrush sb = (SolidColorBrush)((Circle)sender).circleUI.Fill;// = "#FFFF0000";
                    ((SolidColorBrush)((Circle)sender).circleUI.Fill).Color = Colors.Green;
                }
                else
                {
                    ((SolidColorBrush)((Circle)sender).circleUI.Fill).Color = Colors.Red;
                }
                Console.WriteLine("Fill " + ((Circle)sender).circleUI.Fill);
                Console.WriteLine("Circle: "+ ((Circle)sender).GetValue(Canvas.LeftProperty) + " "+ ((Circle)sender).GetValue(Canvas.TopProperty));
                Console.WriteLine("pointer: "+e.GetPosition(canvas));
                Console.WriteLine("Width: " + newTable.circleUI.Width);
            }

        }

        private void Table_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Circle table = (Circle)sender;
            if (!table.Added && ((SolidColorBrush)((Circle)sender).circleUI.Fill).Color == Colors.Red)
            {
                canvas.
            }
        }

        

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ContentPresenter parent = (ContentPresenter)VisualTreeHelper.GetParent((Canvas)sender);
            var parent2 = VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(parent));
            Console.WriteLine(parent2);
        }

        public bool Overlap(Point point)
        {
            foreach (Point otherPoint in pointList)
            {
                if ((Math.Abs(point.X - otherPoint.X) < newTable.circleUI.Width/2) 
                    && (Math.Abs(point.Y - otherPoint.Y) < newTable.circleUI.Width / 2))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
