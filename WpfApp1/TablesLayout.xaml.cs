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
            
        }

        private void AddButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            newTable = new Circle();

            //how the new circle look
            newTable.SetValue(Canvas.RightProperty, distance);
            newTable.SetValue(Canvas.BottomProperty, distance);
            SolidColorBrush myBrush = new SolidColorBrush()
            {
                Color = Colors.Red
            };
            newTable.circleUI.Fill = myBrush;
            newTable.Opacity = 0.5;
            newTable.Added = false;

            canvas.Children.Add(newTable);

            //logic of the new circle
            newTable.CaptureMouse();
            newTable.MouseMove += Table_MouseMove;
            newTable.MouseUp += Table_MouseUp;
            newTable.MouseDown += Table_MouseDownAsync;
            
        }

        

        private void Table_MouseMove(object sender, MouseEventArgs e)
        {
            Circle circle = (Circle)sender;
            if (circle.IsMouseCaptured && e.LeftButton == MouseButtonState.Pressed) 
            {
                Point point = e.GetPosition(canvas);
                //Circle newTable = (Circle)sender;
                circle.SetValue(Canvas.LeftProperty, point.X - (radius + distance));
                circle.SetValue(Canvas.TopProperty, point.Y - (radius + distance));

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
                Console.WriteLine("MouseMove");
            }

            e.Handled = true;

        }

        private async void Table_MouseDownAsync(object sender, MouseButtonEventArgs e)
        {
            Circle circle = (Circle)sender;
            Console.WriteLine("Table_MouseDown");
            await HoldDelay();
            if (circle.IsMouseOver && e.LeftButton == MouseButtonState.Pressed)
            {
                circle.CaptureMouse();
                pointList.Remove(circle.Center);
                previousPt.X = (Double)(circle.GetValue(Canvas.LeftProperty));
                previousPt.Y = (Double)(circle.GetValue(Canvas.TopProperty));
                circle.Opacity = 0.5;
                ((SolidColorBrush)circle.circleUI.Fill).Color = Colors.Green;

            }
            
            Console.WriteLine("Table_MouseDown2");
        }

        private void Table_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Circle circle = (Circle)sender;
            if (!((Circle)sender).Added)
            {
                if (((SolidColorBrush)((Circle)sender).circleUI.Fill).Color == Colors.Red)
                {
                    canvas.Children.Remove((Circle)sender);

                }
                else
                {
                    AddNewCoordinate(circle);
                    SolidYellowCircle(circle);

                    circle.Added = true;              
                }
                
            }
            else //((Circle)sender).Added) which mean move a existed table
            {
                if (((SolidColorBrush)((Circle)sender).circleUI.Fill).Color == Colors.Red)
                {
                    circle.SetValue(Canvas.LeftProperty, previousPt.X );
                    circle.SetValue(Canvas.TopProperty, previousPt.Y);
                    pointList.Add(circle.Center);

                    SolidYellowCircle(circle);
                }
                else
                {
                    AddNewCoordinate(circle);
                    SolidYellowCircle(circle);
              
                    //Console.WriteLine("MouseUp with successfully move a circle");
                }
            }
            
            circle.ReleaseMouseCapture();
            
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

        private void AddNewCoordinate(Circle circle)
        {
            Double x = (Double)circle.GetValue(Canvas.LeftProperty) + radius;
            Double y = (Double)circle.GetValue(Canvas.TopProperty) + radius;
            Point newPoint = new Point(x, y);
            circle.Center = newPoint;
            pointList.Add(newPoint);
        }

        private void SolidYellowCircle(Circle circle)
        {
            SolidColorBrush brush = (SolidColorBrush)circle.circleUI.Fill;
            brush.Color = Colors.Yellow;
            circle.Opacity = 1;
        }

        private async Task HoldDelay()
        {
            await Task.Delay(1000);   
        }
    }
}
