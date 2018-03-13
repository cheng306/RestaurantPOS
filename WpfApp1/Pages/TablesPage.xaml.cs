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
using RestaurantPOS.Models;
using System.Diagnostics;

namespace RestaurantPOS.Pages
{
  /// <summary>
  /// Interaction logic for Table.xaml
  /// </summary>
  public partial class TablesPage : UserControl
  {
    Circle newTable;

    Point previousPt;

    Point addButtonAbsolutePoint;

    Point deleteButtonAbsolutePoint;

    //This is the horizontal distance between the circle UpperLeft and the mouse or touch pointer
    double xDistance;
    double yDistance;

    //radius if Circle
    double radius;

    //diameter of Circle
    double diameter;

    //a list of all centers of circles. Used for Overlap detection
    List<Point> pointList;

    //a list of circles 
    internal List<Models.Table> tablesList;

    Object auxObject;

    bool goToSelectionPage;

    internal List<bool> tableNumberBooleanList;

    double canvasDimension;

    Circle movingCircle;

    public TablesPage()
    {
      InitializeComponent();

      Console.WriteLine("=================tablesPage Started");

      OtherInitialSetup();
    }

    private void OtherInitialSetup()
    {
      pointList = new List<Point>();
      previousPt = new Point();
      movingCircle = null;

      double minDimension = Math.Min(SystemParameters.FullPrimaryScreenWidth, SystemParameters.FullPrimaryScreenHeight);
      canvasDimension = minDimension * 0.85;
      canvas.Width = canvasDimension;
      canvas.Height = canvasDimension;

      diameter = canvasDimension / 10;
      radius = diameter / 2;

      addButtonAbsolutePoint = new Point(canvasDimension - diameter, canvasDimension - diameter);
      deleteButtonAbsolutePoint = new Point(0, canvasDimension - diameter);

      addButton.numberTextBlock.Text = "+";
      addButton.numberTextBlock.Foreground = new SolidColorBrush(Colors.White);

      pointList.Add(addButtonAbsolutePoint);
      addButton.circleUI.Width = diameter;
      addButton.circleUI.Height = diameter;

      deleteButton.circleUI.Fill = new SolidColorBrush(Colors.Red);
      deleteButton.numberTextBlock.Text = "-";
      deleteButton.circleUI.Width = diameter;
      deleteButton.circleUI.Height = diameter;
      deleteButton.numberTextBlock.Foreground = new SolidColorBrush(Colors.White);
    }

    private void AddButton_MouseDown(object sender, MouseButtonEventArgs e)
    {
      if (e.LeftButton == MouseButtonState.Pressed && movingCircle == null)
      {
        newTable = new Circle();
        movingCircle = newTable;

        //how the new circle look
        xDistance = diameter * 0.75;
        yDistance = diameter * 0.75;
        newTable.SetValue(Canvas.LeftProperty, e.GetPosition(canvas).X - xDistance);
        newTable.SetValue(Canvas.TopProperty, e.GetPosition(canvas).Y - yDistance);       
        SolidColorBrush myBrush = new SolidColorBrush()
        {
          Color = Colors.Red
        };
        newTable.circleUI.Fill = myBrush;
        newTable.Opacity = 0.5;
        newTable.circleUI.Width = diameter;
        newTable.circleUI.Height = diameter;
        ChangeZIndex(newTable, 3);


        //logic of the new circle
        newTable.Added = false;
        canvas.Children.Add(newTable);
        
        newTable.CaptureMouse();

        //add listener
        AddMouseListener(newTable);
      }

    }

    private void AddButton_TouchDown(object sender, TouchEventArgs e)
    {
      if (movingCircle == null)
      {
        newTable = new Circle();
        movingCircle = newTable;

        //how the new circle look
        xDistance = diameter * 0.75;
        yDistance = diameter * 0.75;
        newTable.SetValue(Canvas.LeftProperty, e.GetTouchPoint(canvas).Position.X - xDistance);
        newTable.SetValue(Canvas.TopProperty, e.GetTouchPoint(canvas).Position.Y - yDistance);
        SolidColorBrush myBrush = new SolidColorBrush()
        {
          Color = Colors.Red
        };
        newTable.circleUI.Fill = myBrush;
        newTable.Opacity = 0.5;
        newTable.circleUI.Width = diameter;
        newTable.circleUI.Height = diameter;


        //logic of the new circle
        newTable.Added = false;
        canvas.Children.Add(newTable);
        ChangeZIndex(newTable, 3);
        newTable.CaptureTouch(e.TouchDevice);

        //add listener
        AddTouchListener(newTable);
      }

    }

    private async void Table_MouseDownAsync(object sender, MouseButtonEventArgs e)
    {
      if (e.LeftButton == MouseButtonState.Pressed && movingCircle == null )
      {       
        Circle circle = (Circle)sender;
        RemoveTouchListeners(circle);
        movingCircle = circle;
        auxObject = new object();
        Object auxObject2 = auxObject;
        goToSelectionPage = true;
        //hold for 1 second, if mouse leave within a second, it fail to gain mousecapture
        await HoldDelay();

        if (auxObject == auxObject2)
        {

          goToSelectionPage = false;

          ChangeZIndex(circle, 3);

          previousPt.X = (Double)(circle.GetValue(Canvas.LeftProperty));
          previousPt.Y = (Double)(circle.GetValue(Canvas.TopProperty));

          pointList.Remove(new Point(previousPt.X, previousPt.Y));

          xDistance = e.GetPosition(canvas).X - previousPt.X;
          yDistance = e.GetPosition(canvas).Y - previousPt.Y;

          circle.Opacity = 0.5;
          ((SolidColorBrush)circle.circleUI.Fill).Color = Colors.Green;

          circle.CaptureMouse();
        }
      }
    }

    private void Table_MouseLeave(object sender, MouseEventArgs e)
    {
      Console.WriteLine("MouseLeave");
      Circle circle = (Circle)sender;
      if (circle == movingCircle)
      {
        auxObject = null;
        goToSelectionPage = false;
        movingCircle = null;

        AddTouchListener(circle);
        Console.WriteLine("add touch listener");
      }
  
    }

    private void Table_MouseMove(object sender, MouseEventArgs e)
    {
      Circle circle = (Circle)sender;
      if (circle.IsMouseCaptured)//&& e.LeftButton == MouseButtonState.Pressed
      {

        Point point = e.GetPosition(canvas);

        circle.SetValue(Canvas.LeftProperty, point.X - xDistance);//Canvas.LeftProperty
        circle.SetValue(Canvas.TopProperty, point.Y - yDistance);

        point.X = point.X - xDistance;
        point.Y = point.Y - yDistance;

        if (!CoordinateConflict(circle, point))
        {
          ((SolidColorBrush)((Circle)sender).circleUI.Fill).Color = Colors.Green;
        }
        else
        {
          ((SolidColorBrush)((Circle)sender).circleUI.Fill).Color = Colors.Red;
        }
      }
    }


    //fist condition check if mouseup after moving a table or go to selectionPage
    //if after moving a table, checked if table is added
    private void Table_MouseUp(object sender, MouseButtonEventArgs e)
    {
      Console.WriteLine("MouseUp");
      Circle circle = (Circle)sender;
      if (movingCircle == circle)
      {
        if (goToSelectionPage)
        {
          GoToSelectionPage();
        }
        else
        {
          ChangeZIndex(circle, 2);
          if (!((Circle)sender).Added)
          {
            if (((SolidColorBrush)((Circle)sender).circleUI.Fill).Color == Colors.Red)
            {
              canvas.Children.Remove(circle);
            }
            else //green color, then add the circle
            {
              //add the table affliated to this circle
              AddTableToCircle(circle);
            }
          }
          else //((Circle)sender).Added) which mean move a existed table
          {
            if (((SolidColorBrush)((Circle)sender).circleUI.Fill).Color == Colors.Red)
            {
              RestoreOriginalCoordinate(circle);
              SolidYellowCircle(circle);
            }
            else if (((SolidColorBrush)((Circle)sender).circleUI.Fill).Color == Colors.Green)
            {
              DeleteOrRelocateTable(circle);
            }
            else //if yellow color
            {
              //do nothing
            }
          }
        }
        circle.ReleaseMouseCapture();
        auxObject = null;
        movingCircle = null;
        AddTouchListener(circle);
        Console.WriteLine("add touch listener");
      }
        
      
      
    }

    private async void TableUI_TouchDownAsync(object sender, TouchEventArgs e)
    {
      Console.WriteLine("TouchDOwn");
      if (movingCircle == null)
      {
        Circle circle = (Circle)sender;
        RemoveMouseListener(circle);
        movingCircle = circle;
        auxObject = new object();
        Object auxObject2 = auxObject;
        goToSelectionPage = true;

        //hold for 1 second, if mouse le
        await HoldDelay();

        if (auxObject == auxObject2)//circle.AreAnyTouchesDirectlyOver &&
        {
          goToSelectionPage = false;

          ChangeZIndex(circle, 3);
          previousPt.X = (Double)(circle.GetValue(Canvas.LeftProperty));
          previousPt.Y = (Double)(circle.GetValue(Canvas.TopProperty));

          pointList.Remove(new Point(previousPt.X, previousPt.Y));

          xDistance = e.GetTouchPoint(canvas).Position.X - previousPt.X;
          yDistance = e.GetTouchPoint(canvas).Position.Y - previousPt.Y;

          circle.Opacity = 0.5;
          ((SolidColorBrush)circle.circleUI.Fill).Color = Colors.Green;
          circle.CaptureTouch(e.TouchDevice);
        }
      }
        
      
    }

    private void TableUI_TouchLeave(object sender, TouchEventArgs e)
    {
      Console.WriteLine("touchleave");
      Circle circle = (Circle)sender;
      if (movingCircle == circle)
      {
        auxObject = null;
        goToSelectionPage = false;
        movingCircle = null;
        AddMouseListener(circle);
      }    
    }

    private void TableUI_TouchMove(object sender, TouchEventArgs e)
    {

        Circle circle = (Circle)sender;
        if (circle.AreAnyTouchesCaptured)
        {
          Point point = e.GetTouchPoint(canvas).Position;

          circle.SetValue(Canvas.LeftProperty, point.X - xDistance);//Canvas.LeftProperty
          circle.SetValue(Canvas.TopProperty, point.Y - yDistance);

          point.X = point.X - xDistance;
          point.Y = point.Y - yDistance;

          if (!CoordinateConflict(circle, point))
          {
            ((SolidColorBrush)((Circle)sender).circleUI.Fill).Color = Colors.Green;
          }
          else
          {
            ((SolidColorBrush)((Circle)sender).circleUI.Fill).Color = Colors.Red;
          }
        }
      
      
    }

    private void TableUI_TouchUp(object sender, TouchEventArgs e)
    {
      Console.WriteLine("touch_up");
      Circle circle = (Circle)sender;
      if (movingCircle == circle)
      {
        if (goToSelectionPage)
        {
          GoToSelectionPage();
        }
        else
        {
          ChangeZIndex(circle, 2);
          if (!((Circle)sender).Added)
          {
            if (((SolidColorBrush)((Circle)sender).circleUI.Fill).Color == Colors.Red)
            {
              canvas.Children.Remove(circle);
            }
            else //green color, then add the circle
            {
              //add the table affliated to this circle
              AddTableToCircle(circle);
            }

          }
          else //((Circle)sender).Added) which mean move a existed table
          {
            if (((SolidColorBrush)((Circle)sender).circleUI.Fill).Color == Colors.Red)
            {
              RestoreOriginalCoordinate(circle);
              SolidYellowCircle(circle);
            }
            else if (((SolidColorBrush)((Circle)sender).circleUI.Fill).Color == Colors.Green)
            {
              DeleteOrRelocateTable(circle);
            }
            else //if yellow color
            {
              Console.WriteLine("touchup at sth else?");
              //do nothing
            }
          }
        }

        circle.ReleaseMouseCapture();
        auxObject = null;
        movingCircle = null;
        AddMouseListener(circle);
      }
      
    }

    /*
     * circle is the moving circle object
     * point is the upper left point of moving cirlce
     * 
     * AllowRelease composed of 2 parts
     * Part1) check if circle move out of boundry
     * Part2) check if overlap with other circles
     */
    public bool CoordinateConflict(Circle circle, Point upperLeftPoint)
    {

      if (upperLeftPoint.X < 0 || upperLeftPoint.X + diameter > canvas.Width || upperLeftPoint.Y < 0 || upperLeftPoint.Y + diameter > canvas.Height)
      {
        return true;
      }

      if (!circle.Added)
      {
        pointList.Add(deleteButtonAbsolutePoint); 
      }

      foreach (Point otherPoint in pointList)
      {
        if (Math.Sqrt(Math.Pow(upperLeftPoint.X - otherPoint.X,2) + Math.Pow(upperLeftPoint.Y - otherPoint.Y, 2)) < diameter)
        {
          pointList.Remove(deleteButtonAbsolutePoint);
          return true;
        }
      }

      // will remove deleteButton center if exist. Will do nothing if deleteButton not exist
      pointList.Remove(deleteButtonAbsolutePoint);
      return false;
    }

    private Point AbsoluteToRelative(Point point)
    {
      return new Point(point.X/canvasDimension, point.Y/canvasDimension);
    }

    private Point RelativeToAbsolute(Point point)
    {
      return new Point(point.X * canvasDimension, point.Y * canvasDimension);
    }

    private void AddNewCoordinate(Circle circle)
    {
      Double x = (Double)circle.GetValue(Canvas.LeftProperty);
      Double y = (Double)circle.GetValue(Canvas.TopProperty);

      Point newAbsolutePoint = new Point(x, y);
      Point newRelativePoint = AbsoluteToRelative(newAbsolutePoint);

      pointList.Add(newAbsolutePoint);
      circle.Table.UpperLeftPoint = newRelativePoint;
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
    private void RestoreOriginalCoordinate(Circle circle)
    {
      circle.SetValue(Canvas.LeftProperty, previousPt.X);
      circle.SetValue(Canvas.TopProperty, previousPt.Y);
      Point newAbsolutePoint = new Point((double)circle.GetValue(Canvas.LeftProperty), (double)circle.GetValue(Canvas.TopProperty));
      pointList.Add(newAbsolutePoint);
    }

    private void AddListener(Circle tableUI)
    {
      tableUI.MouseMove += Table_MouseMove;
      tableUI.MouseUp += Table_MouseUp;
      tableUI.MouseDown += Table_MouseDownAsync;
      tableUI.MouseLeave += Table_MouseLeave;

      tableUI.TouchDown += TableUI_TouchDownAsync;
      tableUI.TouchMove += TableUI_TouchMove;
      tableUI.TouchLeave += TableUI_TouchLeave;
      tableUI.TouchUp += TableUI_TouchUp;
    }

    private void RemoveListeners(Circle tableUI)
    {
      tableUI.MouseMove += Table_MouseMove;
      tableUI.MouseUp += Table_MouseUp;
      tableUI.MouseDown += Table_MouseDownAsync;
      tableUI.MouseLeave += Table_MouseLeave;

      tableUI.TouchDown -= TableUI_TouchDownAsync;
      tableUI.TouchMove -= TableUI_TouchMove;
      tableUI.TouchLeave -= TableUI_TouchLeave;
      tableUI.TouchUp -= TableUI_TouchUp;
    }

    private void AddMouseListener(Circle tableUI)
    {
      tableUI.MouseMove += Table_MouseMove;
      tableUI.MouseUp += Table_MouseUp;
      tableUI.MouseDown += Table_MouseDownAsync;
      tableUI.MouseLeave += Table_MouseLeave;
    }

    private void AddTouchListener(Circle tableUI)
    {
      tableUI.TouchDown += TableUI_TouchDownAsync;
      tableUI.TouchMove += TableUI_TouchMove;
      tableUI.TouchLeave += TableUI_TouchLeave;
      tableUI.TouchUp += TableUI_TouchUp;
    }

    private void RemoveMouseListener(Circle tableUI)
    {
      tableUI.MouseMove -= Table_MouseMove;
      tableUI.MouseUp -= Table_MouseUp;
      tableUI.MouseDown -= Table_MouseDownAsync;
      tableUI.MouseLeave -= Table_MouseLeave;
    }

    private void RemoveTouchListeners(Circle tableUI)
    {
      tableUI.TouchDown -= TableUI_TouchDownAsync;
      tableUI.TouchMove -= TableUI_TouchMove;
      tableUI.TouchLeave -= TableUI_TouchLeave;
      tableUI.TouchUp -= TableUI_TouchUp;
    }

    

    private void GoToSelectionPage()
    {
      MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
      mainWindow.tabControl.SelectedItem = mainWindow.selectionPageTab;
    }

    private void AddTableToCircle(Circle circle)
    {
      Models.Table table = new Models.Table();
      circle.Table = table;

      FillCircleTableNumber(circle);

      tablesList.Add(table);

      AddNewCoordinate(circle);
      SolidYellowCircle(circle);
      circle.Added = true;
    }

    private void DeleteOrRelocateTable(Circle circle)
    {
      Double x = (Double)circle.GetValue(Canvas.LeftProperty);
      Double y = (Double)circle.GetValue(Canvas.TopProperty);

      if (Math.Sqrt(Math.Pow(x - deleteButtonAbsolutePoint.X, 2) + Math.Pow(y - deleteButtonAbsolutePoint.Y, 2)) < diameter)
      {
        MessageBoxResult mResult = MessageBox.Show("Do You want to DELETE this Table", "Delete the table", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.No);
        switch (mResult)
        {
          case MessageBoxResult.Yes:
            RemoveTable(circle);
            break;
          case MessageBoxResult.No:
            RestoreOriginalCoordinate(circle);
            SolidYellowCircle(circle);
            break;
          case MessageBoxResult.Cancel:
            RestoreOriginalCoordinate(circle);
            SolidYellowCircle(circle);
            break;
        }
      }
      else //green successfuly move the circle
      {
        AddNewCoordinate(circle);
        SolidYellowCircle(circle);
      }
    }

    private void RemoveTable(Circle circle)
    {
      canvas.Children.Remove(circle);
      tableNumberBooleanList[circle.Table.TableNumber-1] = false;
      tablesList.Remove(circle.Table);
    }

    private void FillCircleTableNumber(Circle circle)
    {
      bool fillNumberGap = false;
      for (int i = 0; i < tableNumberBooleanList.Count; i++)
      {
        if (!tableNumberBooleanList[i])
        {
          tableNumberBooleanList[i] = true;
          fillNumberGap = true;
          circle.Table.TableNumber = i + 1;
          circle.NumberTextBlockText = (i + 1).ToString();
          break;
        }
      }
      if (!fillNumberGap)
      {
        tableNumberBooleanList.Add(true);
        circle.Table.TableNumber = tableNumberBooleanList.Count;
        circle.NumberTextBlockText = tableNumberBooleanList.Count.ToString();
      }
    }

    internal void LoadTables()
    {
      foreach (Models.Table table in tablesList)
      {
        //logic of tableUI
        Circle tableUI = new Circle();
        tableUI.Table = table;
        tableUI.Added = true;

        //apperance of the tableUI
        tableUI.circleUI.Width = diameter;
        tableUI.circleUI.Height = diameter;
        SolidColorBrush myBrush = new SolidColorBrush()
        {
          Color = Colors.Yellow
        };
        tableUI.circleUI.Fill = myBrush;
        tableUI.Opacity = 1;
        tableUI.NumberTextBlockText = tableUI.Table.TableNumber.ToString();

        //add listeners to tableUI
        AddListener(tableUI);

        //show on canvas
        Point absolutePoint = RelativeToAbsolute(table.UpperLeftPoint);
        tableUI.SetValue(Canvas.LeftProperty, absolutePoint.X);
        tableUI.SetValue(Canvas.TopProperty, absolutePoint.Y );
        canvas.Children.Add(tableUI);

        pointList.Add(absolutePoint);

      }
    }
  }
}
