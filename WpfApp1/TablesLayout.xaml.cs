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
    public partial class TablesLayout: UserControl
    {
        Circle newTable;  
        Point previousPt;

        //This is the vertical or horizontal distance between the circle center and the mouse pinter
        Double distance;

        //radius if CircleUI;
        Double radius;

        Double diameter;

        List<Point> pointList;
        List<Circle> circleList;

        public TablesLayout()
        {
            InitializeComponent();

            addButton.Center = new Point(canvas.Width - radius, canvas.Height - radius);
            pointList = new List<Point>();
            circleList = new List<Circle>();

            previousPt = new Point();
            distance = 10.0;
            diameter = addButton.circleUI.Width;
            radius = diameter/2;
            pointList.Add(new Point(canvas.Width - radius, canvas.Height - radius));
            
            //Console.WriteLine(pointList[0]);
        }

        private void AddButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            newTable = new Circle();
            newTable.SetValue(Canvas.RightProperty, distance);
            newTable.SetValue(Canvas.BottomProperty, distance);
            SolidColorBrush myBrush = new SolidColorBrush()
            {
                Opacity = 0.5
            };
            newTable.circleUI.Fill = myBrush;
            newTable.Added = false;

            canvas.Children.Add(newTable);


            newTable.CaptureMouse();
            newTable.MouseMove += Table_MouseMove;
            newTable.MouseUp += Table_MouseUp;
            newTable.MouseDown += Table_MouseDown;
            

            //Console.WriteLine("Width: " + newTable.circleUI.Width);
        }

        

        private void Table_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && ((UIElement)sender).IsMouseCaptured)
            {
                Point point = e.GetPosition(canvas);
                //Circle newTable = (Circle)sender;
                ((Circle)sender).SetValue(Canvas.LeftProperty, point.X - (radius + distance));
                ((Circle)sender).SetValue(Canvas.TopProperty, point.Y - (radius + distance));

                point.X = point.X - distance;
                point.Y = point.Y - distance;

                if (!Overlap(point))
                {
                    ((SolidColorBrush)((Circle)sender).circleUI.Fill).Color = Colors.Green;
                }
                else
                {
                    ((SolidColorBrush)((Circle)sender).circleUI.Fill).Color = Colors.Red;
                }
                //Console.WriteLine("Fill " + ((Circle)sender).circleUI.Fill);
                //Console.WriteLine("Circle: "+ ((Circle)sender).GetValue(Canvas.LeftProperty) + " "+ ((Circle)sender).GetValue(Canvas.TopProperty));
                //Console.WriteLine("pointer: "+e.GetPosition(canvas));
                //Console.WriteLine("Width: " + newTable.circleUI.Width);
            }

            e.Handled = true;

        }

        private void Table_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pointList.Remove(((Circle)sender).Center);
            ((UIElement)sender).CaptureMouse(); 
            previousPt.X = (Double)((Circle)sender).GetValue(Canvas.LeftProperty);
            previousPt.Y = (Double)((Circle)sender).GetValue(Canvas.TopProperty);


            Console.WriteLine("Table_MouseDown");

        }

        private void Table_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!((Circle)sender).Added)
            {
                if (((SolidColorBrush)((Circle)sender).circleUI.Fill).Color == Colors.Red)
                {
                    canvas.Children.Remove((Circle)sender);
                }
                else
                {
                    Double x = (Double)((DependencyObject)sender).GetValue(Canvas.LeftProperty)+radius;
                    Double y = (Double)((DependencyObject)sender).GetValue(Canvas.TopProperty) + radius;
                    Point newPoint = new Point(x,y);
                    ((Circle)sender).Center = newPoint;
                    pointList.Add(newPoint);
                    ((Circle)sender).Added = true;
                    //Console.WriteLine(e.GetPosition(canvas));
                    //Console.WriteLine(((DependencyObject)sender).GetValue(Canvas.LeftProperty) + " " + ((DependencyObject)sender).GetValue(Canvas.TopProperty));
                }
                
            }
            else //Table has been added before which mean move a existed table
            {
                if (((SolidColorBrush)((Circle)sender).circleUI.Fill).Color == Colors.Red)
                {
                    ((Circle)sender).SetValue(Canvas.LeftProperty, previousPt.X );
                    ((Circle)sender).SetValue(Canvas.TopProperty, previousPt.Y);
                }
                else
                {
                    Double x = (Double)((DependencyObject)sender).GetValue(Canvas.LeftProperty) + radius;
                    Double y = (Double)((DependencyObject)sender).GetValue(Canvas.TopProperty) + radius;
                    Point newPoint = new Point(x, y);
                    ((Circle)sender).Center = newPoint;
                    pointList.Add(newPoint);
                }
            }
            ((UIElement)sender).ReleaseMouseCapture();

            Console.WriteLine("Table_MouseUp");
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
                if ((Math.Abs(point.X - otherPoint.X) < diameter) 
                    && (Math.Abs(point.Y - otherPoint.Y) < diameter))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
