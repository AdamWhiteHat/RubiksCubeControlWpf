using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RubiksCubeControl.Animation;
using RubiksCubeControl.GameState;
using RubiksCubeControl.Shapes;
using RubiksCubeControl.Synchronization;

namespace RubiksCubeControl
{
    /// <summary>
    /// Interaction logic for RubiksCubeControl3D.xaml
    /// </summary>
    public partial class RubiksCubeControl3D : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event RoutedEventHandler AnimationCompleted;

        protected virtual void RaiseAnimationCompleted()
        {
            RoutedEventHandler routed = AnimationCompleted;
            if (routed != null)
            {
                RoutedEventArgs e = new RoutedEventArgs();
                routed.Invoke(this, e);
            }
        }

        #region Dependency Properties

        public RotateTransform3D CurrentTransform3D
        {
            get { return (RotateTransform3D)GetValue(CurrentTransform3DProperty); }
            set { SetValue(CurrentTransform3DProperty, value); }
        }

        public static readonly DependencyProperty CurrentTransform3DProperty = DependencyProperty.Register(
                                                                            nameof(CurrentTransform3D),
                                                                            typeof(RotateTransform3D),
                                                                            typeof(RubiksCubeControl3D),
                                                                            new PropertyMetadata(
                                                                                default(RotateTransform3D),
                                                                                new PropertyChangedCallback(
                                                                                    RubiksCubeControl3D.RaiseCurrentTransform3DChanged)
                                                                                )
                                                                            );


        public event RoutedPropertyChangedEventHandler<RotateTransform3D> CurrentTransform3DChanged
        {
            add { base.AddHandler(CurrentTransform3DChangedEvent, value); }
            remove { base.RemoveHandler(CurrentTransform3DChangedEvent, value); }
        }

        public static readonly RoutedEvent CurrentTransform3DChangedEvent = EventManager.RegisterRoutedEvent(
                                                                            nameof(CurrentTransform3DChanged),
                                                                            RoutingStrategy.Bubble,
                                                                            typeof(RoutedPropertyChangedEventHandler<RotateTransform3D>),
                                                                            typeof(RubiksCubeControl3D));

        protected virtual void RaiseCurrentTransform3DChanged(RotateTransform3D oldValue, RotateTransform3D newValue)
        {
            RoutedPropertyChangedEventArgs<RotateTransform3D> e = new RoutedPropertyChangedEventArgs<RotateTransform3D>(oldValue, newValue);
            e.RoutedEvent = CurrentTransform3DChangedEvent;
            base.RaiseEvent(e);
        }

        private static void RaiseCurrentTransform3DChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RubiksCubeControl3D element = (RubiksCubeControl3D)d;
            element.RaiseCurrentTransform3DChanged((RotateTransform3D)e.OldValue, (RotateTransform3D)e.NewValue);
        }

        public Point ViewportCenter
        {
            get
            {
                return _viewportCenter;
            }
            set
            {
                if (value != _viewportCenter)
                {
                    _viewportCenter = value;
                    RaisePropertyChanged(nameof(ViewportCenter));
                }
            }
        }

        #endregion 

        #region Private Members

        private RotateTransform3D all_slice_rotation_X;
        private RotateTransform3D all_slice_rotation_Y;
        private RotateTransform3D all_slice_rotation_Z;

        private Point _viewportCenter = new Point(0, 0);
        private Storyboard _storyboard { get; set; }
        private GamePuzzle_Cubies<Cubie> _game;
        private InterlockedCountdown _visualUpdateCountdown;
        private InterlockedCountdown _logicalUpdateCountdown;

        #endregion

        #region Constructor / Initialize

        public RubiksCubeControl3D()
        {
            InitializeComponent();

            this.Loaded += RubiksCubeControl3D_Loaded;

            all_slice_rotation_X = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 0), new Point3D(-0.31, -0.31, 0.31));
            all_slice_rotation_Y = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, -1), 0), new Point3D(0.31, -0.31, 0.31));
            all_slice_rotation_Z = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, -1, 0), 0), new Point3D(0.31, -0.31, 0.31));

            _storyboard = new Storyboard();
            _storyboard.FillBehavior = FillBehavior.HoldEnd;

            _logicalUpdateCountdown = new InterlockedCountdown("RubiksCubeControl3D.LogicalUpdateCountdown", 2);
            _logicalUpdateCountdown.CountdownComplete += logicalUpdateCountdown_CountdownComplete;

            viewport.DataContext = this;
            viewport.SizeChanged += Viewport_SizeChanged;
        }

        private void RubiksCubeControl3D_Loaded(object sender, RoutedEventArgs e)
        {
            CenterViewport();
            Reset();
        }

        public void Reset()
        {
            Cubie back_NW = new Cubie((GeometryModel3D)this.Resources["back_NW"], "back_NW");
            Cubie back_N = new Cubie((GeometryModel3D)this.Resources["back_N"], "back_N");
            Cubie back_NE = new Cubie((GeometryModel3D)this.Resources["back_NE"], "back_NE");
            Cubie back_W = new Cubie((GeometryModel3D)this.Resources["back_W"], "back_W");
            Cubie back_C = new Cubie((GeometryModel3D)this.Resources["back_C"], "back_C");
            Cubie back_E = new Cubie((GeometryModel3D)this.Resources["back_E"], "back_E");
            Cubie back_SW = new Cubie((GeometryModel3D)this.Resources["back_SW"], "back_SW");
            Cubie back_S = new Cubie((GeometryModel3D)this.Resources["back_S"], "back_S");
            Cubie back_SE = new Cubie((GeometryModel3D)this.Resources["back_SE"], "back_SE");
            Cubie middle_NW = new Cubie((GeometryModel3D)this.Resources["middle_NW"], "middle_NW");
            Cubie middle_N = new Cubie((GeometryModel3D)this.Resources["middle_N"], "middle_N");
            Cubie middle_NE = new Cubie((GeometryModel3D)this.Resources["middle_NE"], "middle_NE");
            Cubie middle_W = new Cubie((GeometryModel3D)this.Resources["middle_W"], "middle_W");
            Cubie middle_C = new Cubie((GeometryModel3D)this.Resources["middle_C"], "middle_C");
            Cubie middle_E = new Cubie((GeometryModel3D)this.Resources["middle_E"], "middle_E");
            Cubie middle_SW = new Cubie((GeometryModel3D)this.Resources["middle_SW"], "middle_SW");
            Cubie middle_S = new Cubie((GeometryModel3D)this.Resources["middle_S"], "middle_S");
            Cubie middle_SE = new Cubie((GeometryModel3D)this.Resources["middle_SE"], "middle_SE");
            Cubie front_NW = new Cubie((GeometryModel3D)this.Resources["front_NW"], "front_NW");
            Cubie front_N = new Cubie((GeometryModel3D)this.Resources["front_N"], "front_N");
            Cubie front_NE = new Cubie((GeometryModel3D)this.Resources["front_NE"], "front_NE");
            Cubie front_W = new Cubie((GeometryModel3D)this.Resources["front_W"], "front_W");
            Cubie front_C = new Cubie((GeometryModel3D)this.Resources["front_C"], "front_C");
            Cubie front_E = new Cubie((GeometryModel3D)this.Resources["front_E"], "front_E");
            Cubie front_SW = new Cubie((GeometryModel3D)this.Resources["front_SW"], "front_SW");
            Cubie front_S = new Cubie((GeometryModel3D)this.Resources["front_S"], "front_S");
            Cubie front_SE = new Cubie((GeometryModel3D)this.Resources["front_SE"], "front_SE");

            _game = new GamePuzzle_Cubies<Cubie>(
                back_NW,
                back_N,
                back_NE,
                back_W,
                back_C,
                back_E,
                back_SW,
                back_S,
                back_SE,
                middle_NW,
                middle_N,
                middle_NE,
                middle_W,
                middle_C,
                middle_E,
                middle_SW,
                middle_S,
                middle_SE,
                front_NW,
                front_N,
                front_NE,
                front_W,
                front_C,
                front_E,
                front_SW,
                front_S,
                front_SE
            );

            displayGroup.Children.Clear();
            displayGroup.PopulateFromSlice(_game.Z);
        }

        #endregion

        private void Viewport_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CenterViewport();
        }

        private void CenterViewport()
        {
            Point center = new Point(viewport.RenderSize.Width / 2, viewport.RenderSize.Height / 2);
            ViewportCenter = center;
        }

        #region INotifyPropertyChanged

        protected void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        public void RaisePropertyChanged(PropertyChangedEventArgs evenArgs)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, evenArgs);
            }
        }

        #endregion

        public void AnimateMove(RubiksCubeMoves move, bool counterRotate, AnimationSpeedParameters animationSpeed)
        {
            switch (move)
            {
                case RubiksCubeMoves.Front:
                    AnimateSliceRotation(_game.Front, counterRotate, front_slice_rotation, animationSpeed);
                    break;
                case RubiksCubeMoves.Slice:
                    AnimateSliceRotation(_game.Middle, counterRotate, middle_slice_rotation, animationSpeed);
                    break;
                case RubiksCubeMoves.Back:
                    AnimateSliceRotation(_game.Back, counterRotate, back_slice_rotation, animationSpeed);
                    break;

                case RubiksCubeMoves.Left:
                    AnimateSliceRotation(_game.Left, counterRotate, left_slice_rotation, animationSpeed);
                    break;
                case RubiksCubeMoves.Middle:
                    AnimateSliceRotation(_game.Center, counterRotate, center_slice_rotation, animationSpeed);
                    break;
                case RubiksCubeMoves.Right:
                    AnimateSliceRotation(_game.Right, counterRotate, right_slice_rotation, animationSpeed);
                    break;

                case RubiksCubeMoves.Up:
                    AnimateSliceRotation(_game.Up, counterRotate, up_slice_rotation, animationSpeed);
                    break;
                case RubiksCubeMoves.Equator:
                    AnimateSliceRotation(_game.Equator, counterRotate, equator_slice_rotation, animationSpeed);
                    break;
                case RubiksCubeMoves.Down:
                    AnimateSliceRotation(_game.Down, counterRotate, down_slice_rotation, animationSpeed);
                    break;

                case RubiksCubeMoves.X:
                    AnimateSliceRotation(_game.X, counterRotate, all_slice_rotation_X, animationSpeed);
                    break;
                case RubiksCubeMoves.Y:
                    AnimateSliceRotation(_game.Y, counterRotate, all_slice_rotation_Y, animationSpeed);
                    break;
                case RubiksCubeMoves.Z:
                    AnimateSliceRotation(_game.Z, counterRotate, all_slice_rotation_Z, animationSpeed);
                    break;
            }
        }

        private void AnimateSliceRotation(Slice<Cubie> slice, bool counterRotate, RotateTransform3D rotate3D, AnimationSpeedParameters animationSpeed)
        {
            if (!_logicalUpdateCountdown.IsCompleted())
            {
                throw new InvalidOperationException($"Logical control flow violation: {nameof(AnimateSliceRotation)} called before prior animation finished?");
            }

            Point3D center = new Point3D(rotate3D.CenterX, rotate3D.CenterY, rotate3D.CenterZ);
            AxisAngleRotation3D axisRotation = (AxisAngleRotation3D)rotate3D.Rotation;

            double angleFrom = (double)axisRotation.GetValue(AxisAngleRotation3D.AngleProperty);
            double angleTo = angleFrom + 90d;
            if (counterRotate)
            {
                angleTo = angleFrom - 90d;
            }

            _storyboard.Children.Clear();
            List<Cubie> cubes = slice.GetItems();

            _logicalUpdateCountdown.Reset();

            EventHandler visualUpdateCountdown_CountdownCompleteAction = null;
            visualUpdateCountdown_CountdownCompleteAction = new EventHandler((s, e) =>
            {
                _visualUpdateCountdown.CountdownComplete -= visualUpdateCountdown_CountdownCompleteAction;
                _logicalUpdateCountdown.Signal();
            });

            _visualUpdateCountdown = new InterlockedCountdown("RubiksCubeControl3D.VisualUpdateCountdown", cubes.Count());
            _visualUpdateCountdown.CountdownComplete += visualUpdateCountdown_CountdownCompleteAction;
            _visualUpdateCountdown.Reset();

            List<string> debug_Names = cubes.Select(c => c.Name).ToList();

            foreach (var cube in cubes)
            {
                Vector3D fromAxis = new Vector3D(axisRotation.Axis.X, axisRotation.Axis.Y, axisRotation.Axis.Z);
                Vector3D toAxis = new Vector3D(axisRotation.Axis.X, axisRotation.Axis.Y, axisRotation.Axis.Z);

                AxisAngleRotation3D fromRotation = new AxisAngleRotation3D(fromAxis, angleFrom);
                AxisAngleRotation3D toRotation = new AxisAngleRotation3D(toAxis, angleTo);

                cube.Center = center;
                cube.Rotation = fromRotation;

                Rotation3DAnimation animation = AnimationHelper.BuildRotation3DAnimation(animationSpeed);
                animation.From = fromRotation;
                animation.To = toRotation;

                EventHandler animationCompletedAction = null;
                animationCompletedAction = new EventHandler((s, e) =>
                {
                    cube.FinalizeRotationMovement(toRotation);
                    _visualUpdateCountdown.Signal();
                });
                animation.Completed += animationCompletedAction;

                this.RegisterName(cube.Name, cube);

                Storyboard.SetTarget(animation, cube);
                Storyboard.SetTargetName(animation, cube.Name);
                Storyboard.SetTargetProperty(animation, new PropertyPath(Cubie.RotationProperty));

                _storyboard.Children.Add(animation);
            }

            Slice<Cubie> copy = slice;

            EventHandler updateCubesLocationAction = null;
            if (counterRotate)
            {
                updateCubesLocationAction = new EventHandler((s, e) =>
                {
                    _storyboard.Completed -= updateCubesLocationAction;
                    copy.CounterRotate();
                    _logicalUpdateCountdown.Signal();
                });
            }
            else
            {
                updateCubesLocationAction = new EventHandler((s, e) =>
                {
                    _storyboard.Completed -= updateCubesLocationAction;
                    copy.Rotate();
                    _logicalUpdateCountdown.Signal();
                });
            }

            _storyboard.Completed += updateCubesLocationAction;
            _storyboard.Begin();
        }

        private void logicalUpdateCountdown_CountdownComplete(object? sender, EventArgs e)
        {
            RaiseAnimationCompleted();
        }
    }
}
