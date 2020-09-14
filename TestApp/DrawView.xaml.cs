using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using static System.Math;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for DrawView.xaml
    /// </summary>
    public partial class DrawView : UserControl
    {
        private List<LineCoord> LineCoords { get; set; } = new List<LineCoord>();

        public double MinX { get => (double)GetValue(MinXProperty); set => SetValue(MinXProperty, value); }
        public static readonly DependencyProperty MinXProperty =
            DependencyProperty.Register("MinX", typeof(double), typeof(DrawView), new PropertyMetadata(0d));

        public double MaxX { get => (double)GetValue(MaxXProperty); set => SetValue(MaxXProperty, value); }
        public static readonly DependencyProperty MaxXProperty =
            DependencyProperty.Register("MaxX", typeof(double), typeof(DrawView), new PropertyMetadata(0d));

        public double MinY { get => (double)GetValue(MinYProperty); set => SetValue(MinYProperty, value); }
        public static readonly DependencyProperty MinYProperty =
            DependencyProperty.Register("MinY", typeof(double), typeof(DrawView), new PropertyMetadata(0d));

        public double MaxY { get => (double)GetValue(MaxYProperty); set => SetValue(MaxYProperty, value); }
        public static readonly DependencyProperty MaxYProperty =
            DependencyProperty.Register("MaxY", typeof(double), typeof(DrawView), new PropertyMetadata(0d));

        public double XSpan { get => (double)GetValue(XSpanProperty); set => SetValue(XSpanProperty, value); }
        public static readonly DependencyProperty XSpanProperty =
            DependencyProperty.Register("XSpan", typeof(double), typeof(DrawView), new PropertyMetadata(0d));

        public double YSpan { get => (double)GetValue(YSpanProperty); set => SetValue(YSpanProperty, value); }
        public static readonly DependencyProperty YSpanProperty =
            DependencyProperty.Register("YSpan", typeof(double), typeof(DrawView), new PropertyMetadata(0d));

        public ObservableCollection<string> LineStrings { get => (ObservableCollection<string>)GetValue(LineStringsProperty); set => SetValue(LineStringsProperty, value); }
        public static readonly DependencyProperty LineStringsProperty =
            DependencyProperty.Register("LineStrings", typeof(ObservableCollection<string>), typeof(DrawView), new PropertyMetadata(null));



        public double LineThickness { get => (double)GetValue(LineThicknessProperty); set => SetValue(LineThicknessProperty, value); }
        public static readonly DependencyProperty LineThicknessProperty =
            DependencyProperty.Register("LineThickness", typeof(double), typeof(DrawView), new PropertyMetadata(0d));


        public DrawView()
        {
            InitializeComponent();
            //LineCoords.Add(new LineCoord(20, 10, 50, 10, "Black"));
            //LineCoords.Add(new LineCoord(50, 10, 50, 70, "Gray"));
            //LineCoords.Add(new LineCoord(50, 70, 30, 70, "Blue"));
            //LineCoords.Add(new LineCoord(30, 70, 30, 60, "Green"));
            LineCoords.Add(new LineCoord(-2, -1, 5, -1, "Black"));
            LineCoords.Add(new LineCoord(5, -1, 5, 7, "Gray"));
            LineCoords.Add(new LineCoord(5, 7, 3, 7, "Blue"));
            LineCoords.Add(new LineCoord(3, 7, 3, 6, "Green"));

            LineStrings = new ObservableCollection<string>();
        }

        private void DrawView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

            MinX = Min(LineCoords[0].X1, LineCoords[0].X2);
            MinY = Min(LineCoords[0].Y1, LineCoords[0].Y2);
            MaxX = Max(LineCoords[0].X1, LineCoords[0].X2);
            MaxY = Max(LineCoords[0].Y1, LineCoords[0].Y2);

            foreach (LineCoord line in LineCoords)
            {
                LineStrings.Add($"X1:{line.X1} Y1:{line.Y1} X2:{line.X2} Y2:{line.Y2} Color:{line.Color}");
                MinX = Min(MinX, Min(line.X1, line.X2));
                MaxX = Max(MaxX, Max(line.X1, line.X2));

                MinY = Min(MinY, Max(line.Y1, line.Y2));
                MaxY = Max(MaxY, Max(line.Y1, line.Y2));
            }
            XSpan = MaxX - MinX;
            YSpan = MaxY - MinY;

            canvas.Width = XSpan;
            canvas.Height = YSpan;


            TransformGroup transformGroup = new TransformGroup();
            TranslateTransform translateTransform = new TranslateTransform(-MinX, -MinY);
            transformGroup.Children.Add(translateTransform);
            ScaleTransform scaleTransform = new ScaleTransform(1, -1, 0, YSpan / 2);
            transformGroup.Children.Add(scaleTransform);
            canvas.RenderTransform = transformGroup;

            double  zoomFact = VBox.ActualWidth / canvas.Width;
            LineThickness = 1/zoomFact;

            foreach (LineCoord line in LineCoords)
            {
                Line newLine = new Line()
                {
                    X1 = line.X1,
                    Y1 = line.Y1,
                    X2 = line.X2,
                    Y2 = line.Y2,
                    Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString(line.Color)),
                    StrokeThickness = LineThickness
                };
                canvas.Children.Add(newLine);
            }
        }


        private void VBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
    }
}
