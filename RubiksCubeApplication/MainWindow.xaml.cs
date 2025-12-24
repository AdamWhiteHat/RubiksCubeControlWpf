using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RubiksCubeControl;

namespace RubiksCubeApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public double ScaleZoom
        {
            get { return (double)GetValue(ScaleZoomProperty); }
            set { SetValue(ScaleZoomProperty, value); }
        }

        public static readonly DependencyProperty ScaleZoomProperty = DependencyProperty.Register(nameof(ScaleZoom), typeof(double), typeof(MainWindow), new PropertyMetadata(1.0d, new PropertyChangedCallback(MainWindow.RaiseScaleZoomChanged)));
        public static readonly RoutedEvent ScaleZoomChangedEvent = EventManager.RegisterRoutedEvent(nameof(ScaleZoomChanged), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double>), typeof(MainWindow));

        public event RoutedPropertyChangedEventHandler<double> ScaleZoomChanged
        {
            add { base.AddHandler(ScaleZoomChangedEvent, value); }
            remove { base.RemoveHandler(ScaleZoomChangedEvent, value); }
        }

        protected virtual void RaiseScaleZoomChanged(double oldValue, double newValue)
        {
            RoutedPropertyChangedEventArgs<double> e = new RoutedPropertyChangedEventArgs<double>(oldValue, newValue);
            e.RoutedEvent = ScaleZoomChangedEvent;
            base.RaiseEvent(e);
        }

        private static void RaiseScaleZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MainWindow element = (MainWindow)d;
            element.RaiseScaleZoomChanged((double)e.OldValue, (double)e.NewValue);
        }

        public Point Center
        {
            get { return (Point)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register(nameof(Center), typeof(Point), typeof(MainWindow), new PropertyMetadata(default(Point), new PropertyChangedCallback(MainWindow.RaiseCenterChanged)));

        public static readonly RoutedEvent CenterChangedEvent = EventManager.RegisterRoutedEvent(nameof(CenterChanged), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Point>), typeof(MainWindow));
        public event RoutedPropertyChangedEventHandler<Point> CenterChanged
        {
            add { base.AddHandler(CenterChangedEvent, value); }
            remove { base.RemoveHandler(CenterChangedEvent, value); }
        }

        protected virtual void RaiseCenterChanged(Point oldValue, Point newValue)
        {
            RoutedPropertyChangedEventArgs<Point> e = new RoutedPropertyChangedEventArgs<Point>(oldValue, newValue);
            e.RoutedEvent = CenterChangedEvent;
            base.RaiseEvent(e);
        }

        private static void RaiseCenterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MainWindow element = (MainWindow)d;
            element.RaiseCenterChanged((Point)e.OldValue, (Point)e.NewValue);
        }

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;

            rubiksCubeControl.RegisterForInputEvents(this);
            rubiksCubeControl.MouseLeftButtonDown += RubiksCubeControl_MouseLeftButtonDown;
            rubiksCubeControl.QuitClientRequested += RubiksCubeControl_ClientCloseRequested;

            this.PreviewMouseWheel += MainWindow_PreviewMouseWheel;
        }

        private static double BaseWidth = -1;
        private static double BaseHeight = -1;

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= MainWindow_Loaded;

            BaseWidth = this.ActualWidth;
            BaseHeight = this.ActualHeight;

            CalculateUpdatedCenter();
        }

        private void MainWindow_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                double value = e.Delta / 120;
                double scaledValue = Math.Abs(value * 0.05d);
                int sign = Math.Sign(value);

                double oldScaleZoom = (double)GetValue(MainWindow.ScaleZoomProperty);
                Point oldCenter = (Point)GetValue(MainWindow.CenterProperty);
                double oldWidth = (double)GetValue(MainWindow.WidthProperty);
                double oldHeight = (double)GetValue(MainWindow.HeightProperty);

                double newScaleZoom = -1;
                if (sign == -1)
                {
                    newScaleZoom = oldScaleZoom - scaledValue;
                }
                else if (sign == 1)
                {
                    newScaleZoom = oldScaleZoom + scaledValue;
                }

                double newWidth = BaseWidth * newScaleZoom;
                double newHeight = BaseHeight * newScaleZoom;

                double newCenterX = newWidth / 2.0d;
                double newCenterY = newHeight / 2.0d;
                Point newCenter = new Point(newCenterX, newCenterY);

                TimeSpan animationsDuration = TimeSpan.FromSeconds(0.333);
                TimeSpan fastAnimationDuration = TimeSpan.FromSeconds(0.25);
                double accelerationRatio = 0.3;
                double decelerationRatio = 0.7;

                PointAnimation centerAnimation = new PointAnimation(oldCenter, newCenter, new Duration(fastAnimationDuration), FillBehavior.HoldEnd)
                {
                    AccelerationRatio = accelerationRatio,
                    DecelerationRatio = decelerationRatio
                };
                centerAnimation.Completed += (s, e) =>
                {
                    SetValue(MainWindow.CenterProperty, newCenter);
                    this.BeginAnimation(MainWindow.CenterProperty, null);
                };

                DoubleAnimation widthAnimation = new DoubleAnimation(oldWidth, newWidth, new Duration(fastAnimationDuration), FillBehavior.HoldEnd)
                {
                    AccelerationRatio = accelerationRatio,
                    DecelerationRatio = decelerationRatio
                };
                widthAnimation.Completed += (s, e) =>
                {
                    SetValue(MainWindow.WidthProperty, newWidth);
                    this.BeginAnimation(MainWindow.WidthProperty, null);
                };

                DoubleAnimation heightAnimation = new DoubleAnimation(oldHeight, newHeight, new Duration(fastAnimationDuration), FillBehavior.HoldEnd)
                {
                    AccelerationRatio = accelerationRatio,
                    DecelerationRatio = decelerationRatio
                };
                heightAnimation.Completed += (s, e) =>
                {
                    SetValue(MainWindow.HeightProperty, newHeight);
                    this.BeginAnimation(MainWindow.HeightProperty, null);
                };

                DoubleAnimation scaleZoomAnimation = new DoubleAnimation(oldScaleZoom, newScaleZoom, new Duration(animationsDuration), FillBehavior.HoldEnd)
                {
                    AccelerationRatio = accelerationRatio,
                    DecelerationRatio = decelerationRatio
                };
                scaleZoomAnimation.Completed += (s, e) =>
                {
                    SetValue(MainWindow.ScaleZoomProperty, newScaleZoom);
                    this.BeginAnimation(MainWindow.ScaleZoomProperty, null);
                };

                this.BeginAnimation(MainWindow.CenterProperty, centerAnimation);
                this.BeginAnimation(MainWindow.WidthProperty, widthAnimation);
                this.BeginAnimation(MainWindow.ScaleZoomProperty, scaleZoomAnimation);
                this.BeginAnimation(MainWindow.HeightProperty, heightAnimation);
            }
        }

        private void CalculateUpdatedCenter()
        {
            SetValue(CenterProperty, new Point(this.ActualWidth / 2, this.ActualHeight / 2));
        }

        private void RubiksCubeControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                QuitClientRequested();
                return;
            }
        }

        private void RubiksCubeControl_ClientCloseRequested(object sender, RoutedEventArgs e)
        {
            QuitClientRequested();
        }

        private void QuitClientRequested()
        {
            this.Close();
        }
    }
}