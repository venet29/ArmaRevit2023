using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraficarPtos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public int MyProperty { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            PointCollection points_ = new PointCollection();
            //points_.Add(new Point(-10, -10));
            //points_.Add(new Point(-10, 10));
            //points_.Add(new Point(10, 10));
            //points_.Add(new Point(10, -10));

            Point p1 = new Point(-418.997404217, -7.955868944);
            Point p2 = new Point(-403.692481086, -23.260792075);
            Point p3 = new Point(-404.040466706, -24.072758523);
            Point p4 = new Point(-417.495910705, -10.617314524);

            points_.Add(p1);
            points_.Add(p2);
            points_.Add(p3);
            points_.Add(p4);

            Window_Loaded(points_);
        }
        public MainWindow(PointCollection points_)
        {
            InitializeComponent();
            //cargarPtos();

            Window_Loaded(points_);
        }


        private void Window_Loadedvo(PointCollection points)
        {
            int factor = 10;
            double minx = points.Min(c => c.X) * factor;
            double maxx = points.Max(c => c.X) * factor;
            double miny = points.Min(c => c.Y) * factor;
            double maxy = points.Max(c => c.Y) * factor;



           // canGraph.Width = (int)(maxx - minx) + 20;
            //canGraph.Height = (int)(maxy - miny) + 20;
            ScaleTransform act = new ScaleTransform();
            act.CenterX = (minx + maxx) / 2;
            act.CenterY = (miny + maxy) / 2;
            act.ScaleX = 3;
            act.ScaleY = 3;

            PointCollection pointsModifcado = new PointCollection();
            int cont = 1;
            foreach (var pt in points)
            {
                double centraXCanvas = (canGraph.Width / 2 - (Math.Abs(minx) + Math.Abs(maxx)) / 2);
                double centraYCanvas = (canGraph.Height / 2 - (Math.Abs(miny) + Math.Abs(maxy)) / 2);

                Point Point1_ = new Point((pt.X * factor - minx + centraXCanvas), Math.Abs(maxy - pt.Y * factor + centraYCanvas));
                pointsModifcado.Add(Point1_);
                //cuantias H
                Label SupleH = new Label();
                SupleH.Content = "P" + cont;
                SupleH.FontSize = 12;
                SupleH.Foreground = Brushes.Red;

                Canvas.SetLeft(SupleH, Point1_.X);
                Canvas.SetTop(SupleH, Point1_.Y);
                canGraph.Children.Add(SupleH);
                cont += 1;
            }

            //const double margin = 10;
            //double xmin = -margin;
            //double xmax = canGraph.Width - margin;
            //double ymin = -margin;
            //double ymax = canGraph.Height - margin;
            //const double step = 10;



            // Make some data sets.
            // Display ellipses at the points.
            const float width = 4;
            const float radius = width / 2;
            foreach (Point point in pointsModifcado)
            {
                Ellipse ellipse = new Ellipse();
                ellipse.SetValue(Canvas.LeftProperty, point.X - radius);
                ellipse.SetValue(Canvas.TopProperty, point.Y - radius);
                ellipse.Fill = Brushes.Red;
                ellipse.Stroke = Brushes.Red;
                ellipse.StrokeThickness = 1;
                ellipse.Width = width;
                ellipse.Height = width;
                canGraph.Children.Add(ellipse);
            }
        }

        private void Window_Loaded(PointCollection points)
        {
            int factor = 1;
            double minx = points.Min(c => c.X) * factor;
            double maxx = points.Max(c => c.X) * factor;
            double miny = points.Min(c => c.Y) * factor;
            double maxy = points.Max(c => c.Y) * factor;

            double deltaX = Math.Abs(maxx - minx);
            double deltaY = Math.Abs(maxy - miny);

            minx = minx - deltaX * 0.1;
            miny = miny - deltaY * 0.1;
            maxy = maxy + deltaY * 0.1;
            // deltaX =  deltaX * 0.8;
            // deltaY =  deltaY * 0.8;

            double factX = canGraph.Width / deltaX;
            double factY = canGraph.Height / deltaY;


            factX = factY = Math.Min(factX, factY) * 0.8;
            //factY = factY * 0.8;

            // canGraph.Width = (int)(maxx - minx) + 20;rev

            //canGraph.Height = (int)(maxy - miny) + 20;
            ScaleTransform act = new ScaleTransform();
            act.CenterX = (minx + maxx) / 2;
            act.CenterY = (miny + maxy) / 2;
            act.ScaleX = 3;
            act.ScaleY = 3;

            PointCollection pointsModifcado = new PointCollection();
            int cont = 1;
            foreach (var pt in points)
            {
                double centraXCanvas = (canGraph.Width / 2 - (Math.Abs(minx) + Math.Abs(maxx)) / 2);
                double centraYCanvas = (canGraph.Height / 2 - (Math.Abs(miny) + Math.Abs(maxy)) / 2);

                //Point Point1_ = new Point((pt.X * factor - minx + centraXCanvas), Math.Abs(maxy - pt.Y * factor + centraYCanvas));
                Point Point1_ = new Point((pt.X - minx) * factX, Math.Abs((maxy - pt.Y) * factY));


                pointsModifcado.Add(Point1_);
                //cuantias Hrev

                Label SupleH = new Label();
                SupleH.Content = "P" + cont + "Orig(" + Math.Round(pt.X, 1) + " , " + Math.Round(pt.Y, 1) + ")";//\r Mod(" + Math.Round(Point1_.X, 1) + " , " + Math.Round(Point1_.Y, 1) + ")";
                SupleH.FontSize = 12;
                SupleH.Foreground = Brushes.Red;

                Canvas.SetLeft(SupleH, Point1_.X);
                Canvas.SetTop(SupleH, Point1_.Y);
                canGraph.Children.Add(SupleH);

                cont += 1;
            }

            //const double margin = 10;
            //double xmin = -margin;
            //double xmax = canGraph.Width - margin;
            //double ymin = -margin;
            //double ymax = canGraph.Height - margin;
            //const double step = 10;



            // Make some data sets.
            // Display ellipses at the points.
            const float width = 4;
            const float radius = width / 2;
            foreach (Point point in pointsModifcado)
            {
                Ellipse ellipse = new Ellipse();
                ellipse.SetValue(Canvas.LeftProperty, point.X - radius);
                ellipse.SetValue(Canvas.TopProperty, point.Y - radius);
                ellipse.Fill = Brushes.Red;
                ellipse.Stroke = Brushes.Red;
                ellipse.StrokeThickness = 1;
                ellipse.Width = width;
                ellipse.Height = width;
                canGraph.Children.Add(ellipse);
            }
        }


    }
}
