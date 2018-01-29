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
using WpfApp1.Models;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Interaction logic for Table.xaml
    /// </summary>
    public partial class TablesPage: UserControl
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

        Object auxObject;
        
        public TablesPage()
        {
            InitializeComponent();

            pointList = new List<Point>();
            
            previousPt = new Point();
          
            diameter = addButton.circleUI.Width;
            radius = diameter/2;

            addButton.Center = new Point(canvas.Width - radius, canvas.Height - radius);
            pointList.Add(addButton.Center);

            deleteButton.Center = new Point(radius, canvas.Height - radius);
            deleteButton.circleUI.Fill = new SolidColorBrush(Colors.Red);

            deleteButton.MouseEnter += DeleteButton_MouseEnter;

           


            var a = LogicalTreeHelper.GetParent(canvas);
            var b = LogicalTreeHelper.GetParent(a);
            Console.WriteLine(a);
            Console.WriteLine(b);
        }

        private void DeleteButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("DeleteButton_up");
        }

        private void DeleteButton_MouseEnter(object sender, MouseEventArgs e)
        {
            Console.WriteLine("DeleteButton_enter");
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
 
                circle.SetValue(Canvas.LeftProperty, point.X - (radius + xDistance));
                circle.SetValue(Canvas.TopProperty, point.Y - (radius + yDistance));

              
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
        
            }

            e.Handled = true;

        }

        private void Table_MouseLeave(object sender, MouseEventArgs e)
        {
            auxObject = null;
        }

        private async void Table_MouseDownAsync(object sender, MouseButtonEventArgs e)
        {
            Circle circle = (Circle)sender;
            auxObject = new object();
            Object auxObject2 = auxObject;

            //hold for 1 second, if mouse le
            await HoldDelay();

            if (circle.IsMouseOver && e.LeftButton == MouseButtonState.Pressed && auxObject == auxObject2)
            {
                ChangeZIndex(circle, 3);

                previousPt.X = (Double)(circle.GetValue(Canvas.LeftProperty));
                previousPt.Y = (Double)(circle.GetValue(Canvas.TopProperty));

                
                pointList.Remove(circle.Center);


                xDistance = e.GetPosition(canvas).X - circle.Center.X;
                yDistance = e.GetPosition(canvas).Y - circle.Center.Y;

                Console.WriteLine("left x: " + previousPt.X + " center x: " + circle.Center.X);
                Console.WriteLine("left y: " + previousPt.Y + " center y: " + circle.Center.Y);

                circle.Opacity = 0.5;
                ((SolidColorBrush)circle.circleUI.Fill).Color = Colors.Green;

                circle.CaptureMouse();  

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
                    restoreOriginalCoordinate(circle);
                    SolidYellowCircle(circle);
                }
                else if (((SolidColorBrush)((Circle)sender).circleUI.Fill).Color == Colors.Green)
                {
                    Double x = (Double)circle.GetValue(Canvas.LeftProperty) + radius;
                    Double y = (Double)circle.GetValue(Canvas.TopProperty) + radius;

                    if (x - deleteButton.Center.X < diameter && y - deleteButton.Center.Y < diameter)
                    {
                        MessageBoxResult mResult = MessageBox.Show("Do You want to DELETE this Table", "Delete the table", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.No);
                        switch (mResult)
                        {
                            case MessageBoxResult.Yes:
                                canvas.Children.Remove(circle);
                                break;
                            case MessageBoxResult.No:
                                restoreOriginalCoordinate(circle);
                                SolidYellowCircle(circle);
                                break;
                            case MessageBoxResult.Cancel:
                                restoreOriginalCoordinate(circle);
                                SolidYellowCircle(circle);
                                break;

                        }
                    }
                    else
                    {
                        AddNewCoordinate(circle);
                        SolidYellowCircle(circle);
                    }
                }
                else //if yellow color
                {
                   //do nothing
                }
            }
            
            circle.ReleaseMouseCapture();
            auxObject = null;
            Console.WriteLine("Table_MouseUp");

        }

          
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ContentPresenter parent = (ContentPresenter)VisualTreeHelper.GetParent((Canvas)sender);
            var parent2 = VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(parent));
            //Console.WriteLine(parent2);
        }

        /*
         * circle is the moving circle object
         * point is the center point of moving cirlce
         * 
         * AllowRelease composed of 2 parts
         * Part1) check if circle move out of boundry
         * Part2) check if overlap with other circles
         */
        public bool AllowRelease(Circle circle, Point point)
        {
            if (point.X < radius || point.X > canvas.Width - radius || point.Y < radius || point.Y > canvas.Height - radius)
            {
                return true;
            }

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
                    
                    return true;
                }
            }

            // will remove deleteButton center if exist. Will do nothing if deleteButton not exist
            pointList.Remove(deleteButton.Center);
           
            return false;
        }

        private void AddNewCoordinate(Circle circle)
        {
            Double x = (Double)circle.GetValue(Canvas.LeftProperty) + radius;
            Double y = (Double)circle.GetValue(Canvas.TopProperty) + radius;
            Point newPoint = new Point(x, y);
            circle.Center = newPoint;

            Console.WriteLine("In add: left x: " + (Double)circle.GetValue(Canvas.LeftProperty) + " center x: " + circle.Center.X);
            Console.WriteLine("In add: left y: " + (Double)circle.GetValue(Canvas.TopProperty) + " center y: " + circle.Center.Y);

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

        private async Task HoldDelay()
        {
            await Task.Delay(1000);         
        }

        /*
         * 1) restore circle to its original coordinate
         * 2) add the coordinate of the circle to pointList
         */
        private void restoreOriginalCoordinate(Circle circle)
        {
            circle.SetValue(Canvas.LeftProperty, previousPt.X);
            circle.SetValue(Canvas.TopProperty, previousPt.Y);
            pointList.Add(circle.Center);
        }


    }
}
