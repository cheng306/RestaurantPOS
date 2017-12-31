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

        //This is the horizontal distance between the circle center and the mouse pointer
        Double xDistance;
        Double yDistance;

        //radius if Circle
        Double radius;

        //diameter of Circle
        Double diameter;

        //a list of all centers of circles. Used for Overlap detection
        List<Point> pointList;

        Circle selectedCircle;

        Object objectLock;
        
        public TablesLayout()
        {
            InitializeComponent();

            pointList = new List<Point>();
            
            previousPt = new Point();
          
            diameter = addButton.circleUI.Width;
            radius = diameter/2;

            addButton.Center = new Point(canvas.Width - radius, canvas.Height - radius);
            deleteButton.Center = new Point(radius, canvas.Height - radius);
            deleteButton.circleUI.Fill = new SolidColorBrush(Colors.Red);

            pointList.Add(addButton.Center);
            Console.WriteLine(pointList.Count);
            //pointList.Add(deleteButton.Center);
        }

        private void AddButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            newTable = new Circle();

            //how the new circle look
            xDistance = radius/2;
            yDistance = radius/2;
            newTable.SetValue(Canvas.RightProperty, xDistance);
            newTable.SetValue(Canvas.BottomProperty, yDistance);
            SolidColorBrush myBrush = new SolidColorBrush()
            {
                Color = Colors.Red
            };
            newTable.circleUI.Fill = myBrush;
            newTable.Opacity = 0.5;
            

            //logic of the new circle
            newTable.Added = false;
            canvas.Children.Add(newTable);
            ChangeZIndex(newTable, 3);
            newTable.CaptureMouse();
            ChangeSelectedCricle(newTable);

            //add listener
            newTable.MouseMove += Table_MouseMove;
            newTable.MouseUp += Table_MouseUp;
            newTable.MouseDown += Table_MouseDownAsync;
            newTable.MouseLeave += Table_MouseLeave;
        }

        private void Table_MouseMove(object sender, MouseEventArgs e)
        {
            Circle circle = (Circle)sender;
            if (circle.IsMouseCaptured && e.LeftButton == MouseButtonState.Pressed) 
            {
                Point point = e.GetPosition(canvas);
                //Circle newTable = (Circle)sender;
                Console.Write("MouseMove");
                circle.SetValue(Canvas.LeftProperty, point.X - (radius + xDistance));
                circle.SetValue(Canvas.TopProperty, point.Y - (radius + yDistance));

                Console.Write("x:"+circle.GetValue(Canvas.LeftProperty));
                Console.WriteLine(" y: "+circle.GetValue(Canvas.TopProperty));

                point.X = point.X - xDistance;
                point.Y = point.Y - yDistance;

                if (!AllowRelease(circle,point))
                {
                    ((SolidColorBrush)((Circle)sender).circleUI.Fill).Color = Colors.Green;
                    
                }
                else
                {
                    ((SolidColorBrush)((Circle)sender).circleUI.Fill).Color = Colors.Red;
                }
                //
            }

            e.Handled = true;

        }

        private void Table_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeSelectedCricle(null);
            objectLock = null;
        }

        private async void Table_MouseDownAsync(object sender, MouseButtonEventArgs e)
        {
            Circle circle = (Circle)sender;
            selectedCircle = circle;
            objectLock = new object();
            Object objectLock2 = objectLock;

            //hold for 1 second, if mouse le
            await HoldDelay();

            if (circle.IsMouseOver && e.LeftButton == MouseButtonState.Pressed && objectLock==objectLock2)
            {
                ChangeZIndex(circle, 3);
                circle.CaptureMouse();
                pointList.Remove(circle.Center);

                //Console.WriteLine(pointList.Count);
                previousPt.X = (Double)(circle.GetValue(Canvas.LeftProperty));
                previousPt.Y = (Double)(circle.GetValue(Canvas.TopProperty));
                

                xDistance = e.GetPosition(canvas).X - circle.Center.X;
                yDistance = e.GetPosition(canvas).Y - circle.Center.Y;
                
                circle.Opacity = 0.5;
                ((SolidColorBrush)circle.circleUI.Fill).Color = Colors.Green;

            }
            
            //Console.WriteLine("Table_MouseDown");
        }

        private void Table_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Circle circle = (Circle)sender;
            ChangeZIndex(circle, 2);
            if (!((Circle)sender).Added)
            {
                if (((SolidColorBrush)((Circle)sender).circleUI.Fill).Color == Colors.Red)
                {
                    canvas.Children.Remove(circle);
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
                    Console.WriteLine(pointList.Count);
                    SolidYellowCircle(circle);
                }
                else if (((SolidColorBrush)((Circle)sender).circleUI.Fill).Color == Colors.Green)
                {
                    AddNewCoordinate(circle);
                    SolidYellowCircle(circle);
              
                    //Console.WriteLine("MouseUp with successfully move a circle");
                }
            }
            
            circle.ReleaseMouseCapture();
            ChangeSelectedCricle(null);
            objectLock = null;
            //Console.WriteLine("Table_MouseUp");
        }

          
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ContentPresenter parent = (ContentPresenter)VisualTreeHelper.GetParent((Canvas)sender);
            var parent2 = VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(parent));
            //Console.WriteLine(parent2);
        }

        public bool AllowRelease(Circle circle, Point point)
        {
            if (!circle.Added)
            {
                pointList.Add(deleteButton.Center);
                //Console.WriteLine(pointList.Count);
            }

            foreach (Point otherPoint in pointList)
            {
                if ((Math.Abs(point.X - otherPoint.X) < diameter) 
                    && (Math.Abs(point.Y - otherPoint.Y) < diameter))
                {
                    pointList.Remove(deleteButton.Center);
                    //Console.WriteLine(pointList.Count);
                    return true;
                }
            }

            pointList.Remove(deleteButton.Center);
            //Console.WriteLine(pointList.Count);
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

        private void ChangeZIndex(Circle circle, int i)
        {
            circle.SetValue(Panel.ZIndexProperty, i);
        }

        private void HoldDelayOuter()
        {
            HoldDelay();
            Console.WriteLine(" HoldDelayOuter");

        }

        private async Task HoldDelay()
        {
            await Task.Delay(1000);
            Console.WriteLine("after 1 second");
        }

        private void ChangeSelectedCricle(Circle circle)
        {
            selectedCircle = circle;
        }
    }
}
