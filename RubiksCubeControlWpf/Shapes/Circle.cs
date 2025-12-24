using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RubiksCubeControlWpf.Shapes
{
    public class Circle : System.Windows.Shapes.Shape
    {
        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public Point Location
        {
            get { return (Point)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }

        public new double Width
        {
            get { return (double)GetValue(RadiusProperty) * 2; }
        }

        public new double Height
        {
            get { return (double)GetValue(RadiusProperty) * 2; }
        }

        public new double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        #region Routed Property Events

        public event RoutedPropertyChangedEventHandler<double> RadiusChanged
        {
            add { base.AddHandler(RadiusChangedEvent, value); }
            remove { base.RemoveHandler(RadiusChangedEvent, value); }
        }

        public event RoutedPropertyChangedEventHandler<Point> LocationChanged
        {
            add { base.AddHandler(LocationChangedEvent, value); }
            remove { base.RemoveHandler(LocationChangedEvent, value); }
        }

        public event RoutedPropertyChangedEventHandler<double> LeftChanged
        {
            add { base.AddHandler(LeftChangedEvent, value); }
            remove { base.RemoveHandler(LeftChangedEvent, value); }
        }

        public event RoutedPropertyChangedEventHandler<double> TopChanged
        {
            add { base.AddHandler(TopChangedEvent, value); }
            remove { base.RemoveHandler(TopChangedEvent, value); }
        }

        public event RoutedPropertyChangedEventHandler<double> WidthChanged
        {
            add { base.AddHandler(WidthChangedEvent, value); }
            remove { base.RemoveHandler(WidthChangedEvent, value); }
        }

        public event RoutedPropertyChangedEventHandler<double> HeightChanged
        {
            add { base.AddHandler(HeightChangedEvent, value); }
            remove { base.RemoveHandler(HeightChangedEvent, value); }
        }

        public event RoutedPropertyChangedEventHandler<double> StrokeThicknessChanged
        {
            add { base.AddHandler(StrokeThicknessChangedEvent, value); }
            remove { base.RemoveHandler(StrokeThicknessChangedEvent, value); }
        }

        #endregion

        #region Routed Events

        public static readonly RoutedEvent RadiusChangedEvent = EventManager.RegisterRoutedEvent(nameof(RadiusChanged), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double>), typeof(Circle));

        public static readonly RoutedEvent LocationChangedEvent = EventManager.RegisterRoutedEvent(nameof(LocationChanged), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Point>), typeof(Circle));

        public static readonly RoutedEvent LeftChangedEvent = EventManager.RegisterRoutedEvent(nameof(LeftChanged), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double>), typeof(Circle));

        public static readonly RoutedEvent TopChangedEvent = EventManager.RegisterRoutedEvent(nameof(TopChanged), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double>), typeof(Circle));

        public static readonly RoutedEvent WidthChangedEvent = EventManager.RegisterRoutedEvent(nameof(WidthChanged), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double>), typeof(Circle));

        public static readonly RoutedEvent HeightChangedEvent = EventManager.RegisterRoutedEvent(nameof(HeightChanged), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double>), typeof(Circle));

        public static readonly RoutedEvent StrokeThicknessChangedEvent = EventManager.RegisterRoutedEvent(nameof(StrokeThicknessChanged), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double>), typeof(Circle));

        #endregion

        #region Raise Event Methods

        protected virtual void RaiseRadiusChanged(double oldValue, double newValue) => base.RaiseEvent(new RoutedPropertyChangedEventArgs<double>(oldValue, newValue) { RoutedEvent = RadiusChangedEvent });
        private static void RaiseRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((Circle)d).RaiseRadiusChanged((double)e.OldValue, (double)e.NewValue);

        protected virtual void RaiseLocationChanged(Point oldValue, Point newValue) => base.RaiseEvent(new RoutedPropertyChangedEventArgs<Point>(oldValue, newValue) { RoutedEvent = LocationChangedEvent });
        private static void RaiseLocationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((Circle)d).RaiseLocationChanged((Point)e.OldValue, (Point)e.NewValue);

        protected virtual void RaiseLeftChanged(double oldValue, double newValue) => base.RaiseEvent(new RoutedPropertyChangedEventArgs<double>(oldValue, newValue) { RoutedEvent = LeftChangedEvent });
        private static void RaiseLeftChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((Circle)d).RaiseLeftChanged((double)e.OldValue, (double)e.NewValue);

        protected virtual void RaiseTopChanged(double oldValue, double newValue) => base.RaiseEvent(new RoutedPropertyChangedEventArgs<double>(oldValue, newValue) { RoutedEvent = TopChangedEvent });
        private static void RaiseTopChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((Circle)d).RaiseTopChanged((double)e.OldValue, (double)e.NewValue);

        protected virtual void RaiseWidthChanged(double oldValue, double newValue) => base.RaiseEvent(new RoutedPropertyChangedEventArgs<double>(oldValue, newValue) { RoutedEvent = WidthChangedEvent });
        private static void RaiseWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((Circle)d).RaiseWidthChanged((double)e.OldValue, (double)e.NewValue);

        protected virtual void RaiseHeightChanged(double oldValue, double newValue) => base.RaiseEvent(new RoutedPropertyChangedEventArgs<double>(oldValue, newValue) { RoutedEvent = HeightChangedEvent });
        private static void RaiseHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((Circle)d).RaiseHeightChanged((double)e.OldValue, (double)e.NewValue);

        protected virtual void RaiseStrokeThicknessChanged(double oldValue, double newValue) => base.RaiseEvent(new RoutedPropertyChangedEventArgs<double>(oldValue, newValue) { RoutedEvent = StrokeThicknessChangedEvent });
        private static void RaiseStrokeThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((Circle)d).RaiseStrokeThicknessChanged((double)e.OldValue, (double)e.NewValue);

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(nameof(Radius), typeof(double), typeof(Circle), new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Circle.RaiseRadiusChanged), new CoerceValueCallback(CoerceRadiusProperty)));

        public static readonly DependencyProperty LocationProperty = DependencyProperty.Register(nameof(Location), typeof(Point), typeof(Circle), new FrameworkPropertyMetadata(default(Point), new PropertyChangedCallback(Circle.RaiseLocationChanged)));

        public static readonly DependencyProperty LeftProperty = Canvas.LeftProperty.AddOwner(typeof(Circle), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(Circle.RaiseLeftChanged)));
        public static readonly DependencyProperty TopProperty = Canvas.TopProperty.AddOwner(typeof(Circle), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(Circle.RaiseTopChanged)));

        #endregion

        #region Coerce Value Callbacks

        private static object CoerceRadiusProperty(DependencyObject dp, object baseValue)
        {
            return Math.Max(0, (double)baseValue);
        }

        #endregion

        static Circle()
        {
            StretchProperty.OverrideMetadata(typeof(Circle), new FrameworkPropertyMetadata(Stretch.Fill));

            WidthProperty.OverrideMetadata(typeof(Circle),
                new FrameworkPropertyMetadata(18.0d, FrameworkPropertyMetadataOptions.AffectsMeasure,
                    new PropertyChangedCallback(Circle.RaiseWidthChanged)));

            HeightProperty.OverrideMetadata(typeof(Circle),
                new FrameworkPropertyMetadata(18.0d, FrameworkPropertyMetadataOptions.AffectsMeasure,
                    new PropertyChangedCallback(Circle.RaiseHeightChanged)));

            StrokeThicknessProperty.OverrideMetadata(typeof(Circle),
                new FrameworkPropertyMetadata(9.0d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                    new PropertyChangedCallback(Circle.RaiseStrokeThicknessChanged)));

            MarginProperty.OverrideMetadata(typeof(Circle), new FrameworkPropertyMetadata(new Thickness(-9, -9, 0, 0)));
        }

        public Circle()
        {
            this.TopChanged += Circle_TopChanged;
            this.LeftChanged += Circle_LeftChanged;
            this.LocationChanged += Circle_LocationChanged;
            this.RadiusChanged += Circle_RadiusChanged;
        }

        private void Circle_TopChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double newY = e.NewValue; // + Radius;
            if (!double.IsSubnormal(newY) && ((Point)GetValue(LocationProperty)).Y != newY)
            {
                SetValue(LocationProperty, new Point(Location.X, newY));
            }
        }

        private void Circle_LeftChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double newX = e.NewValue; // + Radius;
            if (!double.IsSubnormal(newX) && ((Point)GetValue(LocationProperty)).X != newX)
            {
                SetValue(LocationProperty, new Point(newX, Location.Y));
            }
        }

        private void Circle_LocationChanged(object sender, RoutedPropertyChangedEventArgs<Point> e)
        {
            double radius = (double)GetValue(RadiusProperty);
            double newLeft = e.NewValue.X;// - radius;
            if (!double.IsSubnormal(newLeft) && (double)GetValue(LeftProperty) != newLeft)
            {
                SetValue(LeftProperty, newLeft);
            }
            double newTop = e.NewValue.Y;// - radius;
            if (!double.IsSubnormal(newTop) && (double)GetValue(TopProperty) != newTop)
            {
                SetValue(TopProperty, newTop);
            }
        }

        private void Circle_RadiusChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!double.IsSubnormal(e.NewValue))
            {
                double newDimensions = 2 * e.NewValue;

                if ((double)GetValue(WidthProperty) != newDimensions)
                {
                    SetValue(WidthProperty, newDimensions);
                }
                if ((double)GetValue(HeightProperty) != newDimensions)
                {
                    SetValue(HeightProperty, newDimensions);
                }

                Thickness newMargin = new Thickness(-e.NewValue, -e.NewValue, 0, 0);
                Thickness oldMargin = (Thickness)GetValue(MarginProperty);

                if (oldMargin.Top != newMargin.Top || oldMargin.Left != newMargin.Left)
                {
                    SetValue(MarginProperty, newMargin);
                }
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            double radius = Radius;
            return new Size(radius * 2, radius * 2);
        }

        public override Geometry RenderedGeometry
        {
            get
            {
                return DefiningGeometry;
            }
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                return new EllipseGeometry(Location, Radius, Radius);
            }
        }

        public override string ToString()
        {
            string drawString = "";
            if (this.Fill != null && this.Fill != Brushes.Transparent)
            {
                drawString += $"fill=\"{this.Fill}\" ";
            }
            if (this.Stroke != null)
            {
                drawString += $"stroke=\"{this.Stroke}\" stroke-width=\"{this.StrokeThickness}\" ";
            }
            return $"<circle id=\"{Name}\" cx=\"{Math.Round(Location.X, 6)}\" cy=\"{Math.Round(Location.Y, 6)}\" r=\"{Math.Round(Radius, 6)}\" {drawString}/>";
        }
    }
}
