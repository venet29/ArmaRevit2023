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

namespace DibujarPtos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //cargarPtos();

            PointCollection points_ = new PointCollection();
            points_.Add(new Point(-10, -10));
            points_.Add(new Point(-10,10));
            points_.Add(new Point(10, 10));
            points_.Add(new Point(10, -10));

            Window_Loaded(points_);
        }

        public MainWindow( PointCollection points_)
        {
            InitializeComponent();
            //cargarPtos();

            Window_Loaded(points_);
        }

        public void cargarPtos()
        {


            Polygon myPolygon = new Polygon();
            myPolygon.Stroke = System.Windows.Media.Brushes.Black;
            myPolygon.Fill = System.Windows.Media.Brushes.LightSeaGreen;
            myPolygon.StrokeThickness = 2;
            myPolygon.HorizontalAlignment = HorizontalAlignment.Left;
            myPolygon.VerticalAlignment = VerticalAlignment.Center;

            PointCollection points = new PointCollection();
            int x1 = 0;
            int x2 = 1000;
            int dx = 100;
            for (float x = x1; x < x2; x += dx)
            {
                Point p = new Point(x, x);
                points.Add(p);
            }

            myPolygon.Points = points;
            canGraph.Children.Add(myPolygon);

        }

        private void Window_Loaded()
        {


            const double margin = 10;
            double xmin = -margin;
            double xmax = canGraph.Width - margin;
            double ymin = -margin;
            double ymax = canGraph.Height - margin;
            const double step = 10;

            // Make the X axis.
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(new Point(xmin, ymax), new Point(canGraph.Width, ymax)));
       
            for (double x = xmin + step; x <= canGraph.Width - step; x += step)
            {
                // crea las linea cortitas en ele ejeX
             xaxis_geom.Children.Add(new LineGeometry(new Point(x, ymax - margin / 2), new Point(x, ymax + margin / 2)));
            }

            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            canGraph.Children.Add(xaxis_path);
    
            // Make the Y ayis.
            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(new Point(xmin, 0), new Point(xmin, canGraph.Height)));
            for (double y = step; y <= canGraph.Height - step; y += step)
            {
                yaxis_geom.Children.Add(new LineGeometry(new Point(xmin - margin / 2, y), new Point(xmin + margin / 2, y)));
            }

            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;

            canGraph.Children.Add(yaxis_path);
         
            // Make some data sets.
            Brush[] brushes = { Brushes.Red, Brushes.Green, Brushes.Blue };
            Random rand = new Random();
            for (int data_set = 0; data_set < 3; data_set++)
            {
                int last_y = rand.Next((int)ymin, (int)ymax);

                PointCollection points = new PointCollection();
                for (double x = xmin; x <= xmax; x += step)
                {
                    last_y = rand.Next(last_y - 10, last_y + 10);
                    if (last_y < ymin) last_y = (int)ymin;
                    if (last_y > ymax) last_y = (int)ymax;
                    points.Add(new Point(x, last_y));
                }

                Polyline polyline = new Polyline();
                polyline.StrokeThickness = 1;
                polyline.Stroke = brushes[data_set];
                polyline.Points = points;

                canGraph.Children.Add(polyline);
                
                // Display ellipses at the points.
                const float width = 4;
                const float radius = width / 2;
                foreach (Point point in points)
                {
                    Ellipse ellipse = new Ellipse();
                    ellipse.SetValue(Canvas.LeftProperty, point.X - radius);
                    ellipse.SetValue(Canvas.TopProperty, point.Y - radius);
                    ellipse.Fill = brushes[data_set];
                    ellipse.Stroke = brushes[data_set];
                    ellipse.StrokeThickness = 1;
                    ellipse.Width = width;
                    ellipse.Height = width;
                    canGraph.Children.Add(ellipse);
                }
            }
        }


        private void Window_Loaded(PointCollection points )
        {
            int factor = 10;
            double minx = points.Min(c => c.X)* factor;
            double maxx = points.Max(c => c.X) * factor;
            double miny = points.Min(c => c.Y) * factor;
            double maxy = points.Max(c => c.Y) * factor;

            

            canGraph.Width = (int)(maxx - minx) + 20;
            canGraph.Height = (int)(maxy - miny) + 20;
            ScaleTransform act = new ScaleTransform();
            act.CenterX = (minx + maxx) / 2;
            act.CenterY = (miny + maxy) / 2;
            act.ScaleX = 3;
            act.ScaleY = 3;

            PointCollection pointsModifcado = new PointCollection();
            int cont = 1;
            foreach (var pt in points)
            {
                Point Point1_ = new Point((pt.X * factor - minx), Math.Abs(maxy - pt.Y * factor) );
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


    }
}
