using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Xaml.Behaviors.Media;
using RubiksCubeControlWpf;
using RubiksCubeControlWpf.Shapes;
using static RubiksCubeControlWpf.AnimationHelper;

namespace RubiksCubeControlWpf
{
    /// <summary>
    /// Interaction logic for RubiksCubeControl.xaml
    /// </summary>
    public partial class RubiksCubeControl : UserControl
    {

        public event RoutedEventHandler QuitClientRequested;

        protected virtual void RaiseQuitClientRequested()
        {
            RoutedEventHandler routed = QuitClientRequested;
            if (routed != null)
            {
                RoutedEventArgs e = new RoutedEventArgs();
                routed.Invoke(this, e);
            }
        }

        public double ScaleZoom
        {
            get { return (double)GetValue(ScaleZoomProperty); }
            set { SetValue(ScaleZoomProperty, value); }
        }
        public static readonly DependencyProperty ScaleZoomProperty = DependencyProperty.Register(nameof(ScaleZoom), typeof(double), typeof(RubiksCubeControl), new PropertyMetadata(1.0d, new PropertyChangedCallback(RubiksCubeControl.RaiseScaleZoomChanged)));
        public static readonly RoutedEvent ScaleZoomChangedEvent = EventManager.RegisterRoutedEvent(nameof(ScaleZoomChanged), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double>), typeof(RubiksCubeControl));

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
            RubiksCubeControl element = (RubiksCubeControl)d;
            element.RaiseScaleZoomChanged((double)e.OldValue, (double)e.NewValue);
        }

        public Point Center
        {
            get { return (Point)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register(nameof(Center), typeof(Point), typeof(RubiksCubeControl), new PropertyMetadata(default(Point), new PropertyChangedCallback(RubiksCubeControl.RaiseCenterChanged)));

        public static readonly RoutedEvent CenterChangedEvent = EventManager.RegisterRoutedEvent(nameof(CenterChanged), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Point>), typeof(RubiksCubeControl));
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
            RubiksCubeControl element = (RubiksCubeControl)d;
            element.RaiseCenterChanged((Point)e.OldValue, (Point)e.NewValue);
        }

        public List<Path> Paths_Ring_TopInner => new List<Path>()
        {
            Path_TopInner_A1,   Path_TopInner_A2,   Path_TopInner_A3,
            Path_TopInner_B1,   Path_TopInner_B2,   Path_TopInner_B3,
            Path_TopInner_C1,   Path_TopInner_C2,   Path_TopInner_C3,
            Path_TopInner_D1,   Path_TopInner_D2,   Path_TopInner_D3
        };

        public List<Path> Paths_Ring_LeftInner => new List<Path>()
        {
            Path_LeftInner_A1,  Path_LeftInner_A2,  Path_LeftInner_A3,
            Path_LeftInner_B1,  Path_LeftInner_B2,  Path_LeftInner_B3,
            Path_LeftInner_C1,  Path_LeftInner_C2,  Path_LeftInner_C3,
            Path_LeftInner_D1,  Path_LeftInner_D2,  Path_LeftInner_D3
        };

        public List<Path> Paths_Ring_RightInner => new List<Path>()
        {
            Path_RightInner_A1,     Path_RightInner_A2,     Path_RightInner_A3,
            Path_RightInner_B1,     Path_RightInner_B2,     Path_RightInner_B3,
            Path_RightInner_C1,     Path_RightInner_C2,     Path_RightInner_C3,
            Path_RightInner_D1,     Path_RightInner_D2,     Path_RightInner_D3
        };


        public List<Path> Paths_Ring_TopMiddle => new List<Path>()
        {
            Path_TopMiddle_A1,     Path_TopMiddle_A2,     Path_TopMiddle_A3,
            Path_TopMiddle_B1,     Path_TopMiddle_B2,     Path_TopMiddle_B3,
            Path_TopMiddle_C1,     Path_TopMiddle_C2,     Path_TopMiddle_C3,
            Path_TopMiddle_D1,     Path_TopMiddle_D2,     Path_TopMiddle_D3
        };

        public List<Path> Paths_Ring_LeftMiddle => new List<Path>()
        {
            Path_LeftMiddle_A1,     Path_LeftMiddle_A2,     Path_LeftMiddle_A3,
            Path_LeftMiddle_B1,     Path_LeftMiddle_B2,     Path_LeftMiddle_B3,
            Path_LeftMiddle_C1,     Path_LeftMiddle_C2,     Path_LeftMiddle_C3,
            Path_LeftMiddle_D1,     Path_LeftMiddle_D2,     Path_LeftMiddle_D3
        };

        public List<Path> Paths_Ring_RightMiddle => new List<Path>()
        {
            Path_RightMiddle_A1,     Path_RightMiddle_A2,     Path_RightMiddle_A3,
            Path_RightMiddle_B1,     Path_RightMiddle_B2,     Path_RightMiddle_B3,
            Path_RightMiddle_C1,     Path_RightMiddle_C2,     Path_RightMiddle_C3,
            Path_RightMiddle_D1,     Path_RightMiddle_D2,     Path_RightMiddle_D3
        };



        public List<Path> Paths_Ring_TopOuter => new List<Path>()
        {
            Path_TopOuter_A1,     Path_TopOuter_A2,     Path_TopOuter_A3,
            Path_TopOuter_B1,     Path_TopOuter_B2,     Path_TopOuter_B3,
            Path_TopOuter_C1,     Path_TopOuter_C2,     Path_TopOuter_C3,
            Path_TopOuter_D1,     Path_TopOuter_D2,     Path_TopOuter_D3
        };

        public List<Path> Paths_Ring_LeftOuter => new List<Path>()
        {

            Path_LeftOuter_A1,     Path_LeftOuter_A2,     Path_LeftOuter_A3,
            Path_LeftOuter_B1,     Path_LeftOuter_B2,     Path_LeftOuter_B3,
            Path_LeftOuter_C1,     Path_LeftOuter_C2,     Path_LeftOuter_C3,
            Path_LeftOuter_D1,     Path_LeftOuter_D2,     Path_LeftOuter_D3
        };

        public List<Path> Paths_Ring_RightOuter => new List<Path>()
        {
            Path_RightOuter_A1,     Path_RightOuter_A2,     Path_RightOuter_A3,
            Path_RightOuter_B1,     Path_RightOuter_B2,     Path_RightOuter_B3,
            Path_RightOuter_C1,     Path_RightOuter_C2,     Path_RightOuter_C3,
            Path_RightOuter_D1,     Path_RightOuter_D2,     Path_RightOuter_D3
        };

        private Storyboard _storyboard { get; set; }
        private static GameBoard<Circle> _gameboard { get; set; }
        private ConcurrentQueue<Tuple<RubiksCubeMoves, bool>> _moveQueue { get; set; }

        public RubiksCubeControl()
        {
            InitializeComponent();

            _moveQueue = new ConcurrentQueue<Tuple<RubiksCubeMoves, bool>>();

            _storyboard = new Storyboard();
            _storyboard.FillBehavior = FillBehavior.Stop;

            Face<Circle> yellow = new Face<Circle>(yNW, yNN, yNE, yWW, yCC, yEE, ySW, ySS, ySE);
            Face<Circle> green = new Face<Circle>(gNW, gNN, gNE, gWW, gCC, gEE, gSW, gSS, gSE);
            Face<Circle> orange = new Face<Circle>(oNW, oNN, oNE, oWW, oCC, oEE, oSW, oSS, oSE);
            Face<Circle> red = new Face<Circle>(rNW, rNN, rNE, rWW, rCC, rEE, rSW, rSS, rSE);
            Face<Circle> white = new Face<Circle>(wNW, wNN, wNE, wWW, wCC, wEE, wSW, wSS, wSE);
            Face<Circle> blue = new Face<Circle>(bNW, bNN, bNE, bWW, bCC, bEE, bSW, bSS, bSE);
            _gameboard = new GameBoard<Circle>(yellow, green, orange, red, white, blue);

            this.Loaded += (s, e) => SetValue(CenterProperty, new Point(this.ActualWidth / 23, this.ActualHeight / 2));
            //this.SizeChanged += RubiksCubeControl_SizeChanged;


        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseQuitClientRequested();
        }

        private void ScaleTransform_Changed(object sender, EventArgs e)
        {
            var width = this.ActualWidth * ScaleZoom;
            double height = this.ActualHeight *  ScaleZoom;

            //RaiseScaleZoomChanged(ScaleZoom, ScaleZoom);

            int k = 0;
        }

        private void rubiksCubeUserControl_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                double value = e.Delta / 120;
                double scaledValue = Math.Abs(value * 0.05d);
                int sign = Math.Sign(value);

                double scaleZoom = (double)GetValue(RubiksCubeControl.ScaleZoomProperty);

                if (sign == -1)
                {
                    SetValue(RubiksCubeControl.ScaleZoomProperty, scaleZoom - scaledValue);
                }
                else if (sign == 1)
                {
                    SetValue(RubiksCubeControl.ScaleZoomProperty, scaleZoom + scaledValue);
                }
            }
        }

        public static void UpdatePositions_Rings(RubiksCubeMoves move, bool counterRotate)
        {
            Action<Ring<Circle>> rotateFunc = GameBoardExtensionMethods.Rotate<Circle>;
            if (counterRotate)
            {
                rotateFunc = GameBoardExtensionMethods.CounterRotate<Circle>;
            }

            switch (move)
            {
                case RubiksCubeMoves.Middle: rotateFunc.Invoke(_gameboard.Right.Middle); break;
                case RubiksCubeMoves.Slice: rotateFunc.Invoke(_gameboard.Left.Middle); break;
                case RubiksCubeMoves.Equator: rotateFunc.Invoke(_gameboard.Top.Middle); break;

                case RubiksCubeMoves.Left:
                    rotateFunc.Invoke(_gameboard.Right.Outer);
                    break;
                case RubiksCubeMoves.Right:
                    rotateFunc.Invoke(_gameboard.Right.Inner);
                    break;
                case RubiksCubeMoves.Up:
                    rotateFunc.Invoke(_gameboard.Top.Inner);
                    break;
                case RubiksCubeMoves.Down:
                    rotateFunc.Invoke(_gameboard.Top.Outer);
                    break;
                case RubiksCubeMoves.Front:
                    rotateFunc.Invoke(_gameboard.Left.Inner);
                    break;
                case RubiksCubeMoves.Back:
                    rotateFunc.Invoke(_gameboard.Left.Outer);
                    break;

                case RubiksCubeMoves.X:
                    rotateFunc.Invoke(_gameboard.Right.Inner);
                    rotateFunc.Invoke(_gameboard.Right.Middle);
                    rotateFunc.Invoke(_gameboard.Right.Outer);
                    break;
                case RubiksCubeMoves.Y:
                    rotateFunc.Invoke(_gameboard.Top.Inner);
                    rotateFunc.Invoke(_gameboard.Top.Middle);
                    rotateFunc.Invoke(_gameboard.Top.Outer);
                    break;
                case RubiksCubeMoves.Z:
                    rotateFunc.Invoke(_gameboard.Left.Inner);
                    rotateFunc.Invoke(_gameboard.Left.Middle);
                    rotateFunc.Invoke(_gameboard.Left.Outer);
                    break;
            }
        }

        public void RegisterForInputEvents(UIElement proxyParentControl)
        {
            proxyParentControl.KeyUp += UserControl_KeyUp;
        }

        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            ProcessKeyEvent(e);
        }

        private void ProcessKeyEvent(KeyEventArgs e)
        {
            RubiksCubeMoves move;

            switch (e.Key)
            {
                case Key.U: move = RubiksCubeMoves.Up; break;
                case Key.D: move = RubiksCubeMoves.Down; break;
                case Key.L: move = RubiksCubeMoves.Left; break;
                case Key.R: move = RubiksCubeMoves.Right; break;
                case Key.F: move = RubiksCubeMoves.Front; break;
                case Key.B: move = RubiksCubeMoves.Back; break;
                case Key.M: move = RubiksCubeMoves.Middle; break;
                case Key.S: move = RubiksCubeMoves.Slice; break;
                case Key.E: move = RubiksCubeMoves.Equator; break;
                case Key.X: move = RubiksCubeMoves.X; break;
                case Key.Y: move = RubiksCubeMoves.Y; break;
                case Key.Z: move = RubiksCubeMoves.Z; break;
                default: return;
            }

            bool counterRotate = (e.KeyboardDevice.Modifiers == ModifierKeys.Shift || e.KeyboardDevice.Modifiers == ModifierKeys.Control);

            _moveQueue.Enqueue(new Tuple<RubiksCubeMoves, bool>(move, counterRotate));

            if (!ExclusiveAccess.TryObtainLock())
            {
                return;
            }

            ProcessNextCommand();
        }

        private void ProcessNextCommand()
        {
            if (_moveQueue.TryDequeue(out Tuple<RubiksCubeMoves, bool> parameters))
            {
                AnimateMove(parameters.Item1, parameters.Item2);
            }
            else
            {
                ExclusiveAccess.ReleaseLock();
            }
        }

        private void AnimateMove(RubiksCubeMoves move, bool counterRotate)
        {
            List<MoveAnimationGroup> moveAnimations = new List<MoveAnimationGroup>();

            /* 
            List<Tuple<List<Circle>, List<Path>>> peicesAndPathsTuple = GetFacesPeicesAndPaths(move);
            foreach (new Tuple<List<Circle>, List<Path>> tup in peicesAndPathsTuple)
            {
                MoveAnimationGroup animationGroup = new MoveAnimationGroup(move, counterRotate);
                animationGroup.Peices = tup.Item1;
                animationGroup.Paths = tup.Item2;
                // Actions?
            }            */

            switch (move)
            {
                /*** X, Y, Z ***/

                case RubiksCubeMoves.X:

                    MoveAnimationGroup aniOuterX = new MoveAnimationGroup(RubiksCubeMoves.Left, counterRotate);
                    aniOuterX.RingPeices = _gameboard.Right.Outer.GetItems();
                    aniOuterX.RingPaths = Paths_Ring_RightOuter;
                    aniOuterX.RingParentCanvas = Track_Right_Outer;
                    aniOuterX.AddRingFinalizerAction();

                    aniOuterX.FacePeices = new List<Face<Circle>>() { _gameboard.Yellow };

                    MoveAnimationGroup aniMiddleX = new MoveAnimationGroup(RubiksCubeMoves.Middle, counterRotate);
                    aniMiddleX.RingPeices = _gameboard.Right.Middle.GetItems();
                    aniMiddleX.RingPaths = Paths_Ring_RightMiddle;
                    aniMiddleX.RingParentCanvas = Track_Right_Middle;
                    aniMiddleX.AddRingFinalizerAction();

                    MoveAnimationGroup aniInnerX = new MoveAnimationGroup(RubiksCubeMoves.Right, counterRotate);
                    aniInnerX.RingPeices = _gameboard.Right.Inner.GetItems();
                    aniInnerX.RingPaths = Paths_Ring_RightInner;
                    aniInnerX.RingParentCanvas = Track_Right_Inner;
                    aniInnerX.AddRingFinalizerAction();

                    aniInnerX.FacePeices = new List<Face<Circle>>() { _gameboard.White };

                    moveAnimations.Add(aniOuterX);
                    moveAnimations.Add(aniMiddleX);
                    moveAnimations.Add(aniInnerX);

                    break;

                case RubiksCubeMoves.Y:

                    MoveAnimationGroup aniOuterY = new MoveAnimationGroup(RubiksCubeMoves.Down, counterRotate);
                    aniOuterY.RingPeices = _gameboard.Top.Outer.GetItems();
                    aniOuterY.RingPaths = Paths_Ring_TopOuter;
                    aniOuterY.RingParentCanvas = Track_Top_Outer;
                    aniOuterY.AddRingFinalizerAction();

                    aniOuterY.FacePeices = new List<Face<Circle>>() { _gameboard.Blue };

                    MoveAnimationGroup aniMiddleY = new MoveAnimationGroup(RubiksCubeMoves.Equator, counterRotate);
                    aniMiddleY.RingPeices = _gameboard.Top.Middle.GetItems();
                    aniMiddleY.RingPaths = Paths_Ring_TopMiddle;
                    aniMiddleY.RingParentCanvas = Track_Top_Middle;
                    aniMiddleY.AddRingFinalizerAction();

                    MoveAnimationGroup aniInnerY = new MoveAnimationGroup(RubiksCubeMoves.Up, counterRotate);
                    aniInnerY.RingPeices = _gameboard.Top.Inner.GetItems();
                    aniInnerY.RingPaths = Paths_Ring_TopInner;
                    aniInnerY.RingParentCanvas = Track_Top_Inner;
                    aniInnerY.AddRingFinalizerAction();

                    aniInnerY.FacePeices = new List<Face<Circle>>() { _gameboard.Green };

                    moveAnimations.Add(aniOuterY);
                    moveAnimations.Add(aniMiddleY);
                    moveAnimations.Add(aniInnerY);

                    break;

                case RubiksCubeMoves.Z:

                    MoveAnimationGroup aniOuterZ = new MoveAnimationGroup(RubiksCubeMoves.Back, counterRotate);
                    aniOuterZ.RingPeices = _gameboard.Left.Outer.GetItems();
                    aniOuterZ.RingPaths = Paths_Ring_LeftOuter;
                    aniOuterZ.RingParentCanvas = Track_Left_Outer;
                    aniOuterZ.AddRingFinalizerAction();

                    aniOuterZ.FacePeices = new List<Face<Circle>>() { _gameboard.Orange };

                    MoveAnimationGroup aniMiddleZ = new MoveAnimationGroup(RubiksCubeMoves.Slice, counterRotate);
                    aniMiddleZ.RingPeices = _gameboard.Left.Middle.GetItems();
                    aniMiddleZ.RingPaths = Paths_Ring_LeftMiddle;
                    aniMiddleZ.RingParentCanvas = Track_Left_Middle;
                    aniMiddleZ.AddRingFinalizerAction();

                    MoveAnimationGroup aniInnerZ = new MoveAnimationGroup(RubiksCubeMoves.Front, counterRotate);
                    aniInnerZ.RingPeices = _gameboard.Left.Inner.GetItems();
                    aniInnerZ.RingPaths = Paths_Ring_LeftInner;
                    aniInnerZ.RingParentCanvas = Track_Left_Inner;
                    aniInnerZ.AddRingFinalizerAction();

                    aniInnerZ.FacePeices = new List<Face<Circle>>() { _gameboard.Red };

                    moveAnimations.Add(aniOuterZ);
                    moveAnimations.Add(aniMiddleZ);
                    moveAnimations.Add(aniInnerZ);

                    break;

                /*** Inner ***/

                case RubiksCubeMoves.Up:
                    MoveAnimationGroup uAni = new MoveAnimationGroup(move, counterRotate);

                    uAni.RingPeices = _gameboard.Top.Inner.GetItems();
                    uAni.RingPaths = Paths_Ring_TopInner;
                    uAni.RingParentCanvas = Track_Top_Inner;
                    uAni.AddRingFinalizerAction();

                    uAni.FacePeices = new List<Face<Circle>>() { _gameboard.Green };

                    moveAnimations.Add(uAni);
                    break;

                case RubiksCubeMoves.Front:
                    MoveAnimationGroup fAni = new MoveAnimationGroup(move, counterRotate);

                    fAni.RingPeices = _gameboard.Left.Inner.GetItems();
                    fAni.RingPaths = Paths_Ring_LeftInner;
                    fAni.RingParentCanvas = Track_Left_Inner;
                    fAni.AddRingFinalizerAction();

                    fAni.FacePeices = new List<Face<Circle>>() { _gameboard.Red };

                    moveAnimations.Add(fAni);
                    break;

                case RubiksCubeMoves.Right:
                    MoveAnimationGroup rAni = new MoveAnimationGroup(move, counterRotate);

                    rAni.RingPeices = _gameboard.Right.Inner.GetItems();
                    rAni.RingPaths = Paths_Ring_RightInner;
                    rAni.RingParentCanvas = Track_Right_Inner;
                    rAni.AddRingFinalizerAction();

                    rAni.FacePeices = new List<Face<Circle>>() { _gameboard.White };

                    moveAnimations.Add(rAni);

                    break;

                /*** Middle ***/

                case RubiksCubeMoves.Equator:
                    MoveAnimationGroup eAni = new MoveAnimationGroup(move, counterRotate);

                    eAni.RingPeices = _gameboard.Top.Middle.GetItems();
                    eAni.RingPaths = Paths_Ring_TopMiddle;
                    eAni.RingParentCanvas = Track_Top_Middle;
                    eAni.AddRingFinalizerAction();

                    moveAnimations.Add(eAni);
                    break;

                case RubiksCubeMoves.Slice:
                    MoveAnimationGroup sAni = new MoveAnimationGroup(move, counterRotate);

                    sAni.RingPeices = _gameboard.Left.Middle.GetItems();
                    sAni.RingPaths = Paths_Ring_LeftMiddle;
                    sAni.RingParentCanvas = Track_Left_Middle;
                    sAni.AddRingFinalizerAction();

                    moveAnimations.Add(sAni);
                    break;

                case RubiksCubeMoves.Middle:
                    MoveAnimationGroup mAni = new MoveAnimationGroup(move, counterRotate);

                    mAni.RingPeices = _gameboard.Right.Middle.GetItems();
                    mAni.RingPaths = Paths_Ring_RightMiddle;
                    mAni.RingParentCanvas = Track_Right_Middle;
                    mAni.AddRingFinalizerAction();

                    moveAnimations.Add(mAni);
                    break;

                /*** Outer ***/

                case RubiksCubeMoves.Down:
                    MoveAnimationGroup dAni = new MoveAnimationGroup(move, counterRotate);

                    dAni.RingPeices = _gameboard.Top.Outer.GetItems();
                    dAni.RingPaths = Paths_Ring_TopOuter;
                    dAni.RingParentCanvas = Track_Top_Outer;
                    dAni.AddRingFinalizerAction();

                    dAni.FacePeices = new List<Face<Circle>>() { _gameboard.Blue };

                    moveAnimations.Add(dAni);
                    break;

                case RubiksCubeMoves.Back:
                    MoveAnimationGroup bAni = new MoveAnimationGroup(move, counterRotate);

                    bAni.RingPeices = _gameboard.Left.Outer.GetItems();
                    bAni.RingPaths = Paths_Ring_LeftOuter;
                    bAni.RingParentCanvas = Track_Left_Outer;
                    bAni.AddRingFinalizerAction();

                    bAni.FacePeices = new List<Face<Circle>>() { _gameboard.Orange };

                    moveAnimations.Add(bAni);
                    break;

                case RubiksCubeMoves.Left:
                    MoveAnimationGroup lAni = new MoveAnimationGroup(move, counterRotate);

                    lAni.RingPeices = _gameboard.Right.Outer.GetItems();
                    lAni.RingPaths = Paths_Ring_RightOuter;
                    lAni.RingParentCanvas = Track_Right_Outer;
                    lAni.AddRingFinalizerAction();

                    lAni.FacePeices = new List<Face<Circle>>() { _gameboard.Yellow };

                    moveAnimations.Add(lAni);
                    break;

                default:
                    return;

            }

            List<Action> peicePositionsUpdates = new List<Action>();
            List<Tuple<Circle, PointAnimationUsingPath>[]> ringAnimationGroups = new List<Tuple<Circle, PointAnimationUsingPath>[]>();
            List<List<Tuple<Circle, PointAnimation>[]>> faceAnimations = new List<List<Tuple<Circle, PointAnimation>[]>>();

            foreach (MoveAnimationGroup movesToAnimate in moveAnimations)
            {
                if (counterRotate)
                {
                    List<Path> directionReversedPaths = movesToAnimate.RingPaths.Select(p => p.ReverseDirection()).ToList();
                    RotationHelper.Rotate(directionReversedPaths);
                    movesToAnimate.RingPaths = directionReversedPaths.ToList();
                }

                if (movesToAnimate.RingParentCanvas != null)
                {
                    double top = (double)movesToAnimate.RingParentCanvas.GetValue(Canvas.TopProperty);
                    double left = (double)movesToAnimate.RingParentCanvas.GetValue(Canvas.LeftProperty);

                    movesToAnimate.Transforms = new TransformGroup();
                    movesToAnimate.Transforms.Children.Add(new TranslateTransform(left, top));


                    TransformGroup transformGroup = movesToAnimate.RingParentCanvas.RenderTransform as TransformGroup;
                    if (transformGroup != null)
                    {
                        foreach (Transform child in transformGroup.Children)
                        {
                            RotateTransform rotate = child as RotateTransform;
                            if (rotate != null)
                            {
                                rotate.CenterX = left;
                                rotate.CenterY = top;
                                movesToAnimate.Transforms.Children.Add(rotate);
                            }
                            else
                            {
                                movesToAnimate.Transforms.Children.Add(child);
                            }
                        }
                    }
                }

                if (movesToAnimate.FacePeices != null && movesToAnimate.FacePeices.Any())
                {
                    faceAnimations.Add(AnimationHelper.BuildFaceAnimation(movesToAnimate));
                }

                peicePositionsUpdates.AddRange(movesToAnimate.FinalizerActions);
                ringAnimationGroups.Add(AnimationHelper.BuildRingAnimation(movesToAnimate));
            }

            _storyboard.Children.Clear();

            foreach (var ringAnimation in ringAnimationGroups)
            {
                ringAnimation.AddToStoryboard(_storyboard);
            }

            foreach (var faceAnimationGroups in faceAnimations)
            {
                foreach (var faceAnimation in faceAnimationGroups)
                {
                    //faceAnimation.Item1.BeginAnimation(System.Windows.Media.RotateTransform.AngleProperty, faceAnimation.Item2);
                    faceAnimation.AddToStoryboard(_storyboard);
                }
            }

            _storyboard.Completed += StoryboardCompletedHandler;
            _storyboard.Begin();

            peicePositionsUpdates.ForEach(action => action.Invoke());
        }

        private void StoryboardCompletedHandler(object? source, EventArgs args)
        {
            _storyboard.Completed -= StoryboardCompletedHandler;
            ProcessNextCommand();
        }

        private static class ExclusiveAccess
        {
            private static UInt64 _lockObject = 0;

            public static bool TryObtainLock()
            {
                return (0 == Interlocked.CompareExchange(ref _lockObject, 1, 0));
            }

            public static void ReleaseLock()
            {
                Interlocked.Exchange(ref _lockObject, 0);
            }
        }

        private void CubeNet_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.OnMouseLeftButtonDown(e);
        }


    }

    public class MoveAnimationGroup
    {
        public bool CounterRotate { get; set; }
        public RubiksCubeMoves Move { get; set; }
        public List<Circle> RingPeices { get; set; }
        public List<Path> RingPaths { get; set; }
        public Canvas RingParentCanvas { get; set; }

        public List<Face<Circle>> FacePeices { get; set; }

        public TransformGroup Transforms { get; set; }

        public List<Action> FinalizerActions { get; set; }

        public MoveAnimationGroup(RubiksCubeMoves move, bool counterRotate)
        {
            RingParentCanvas = null;
            Transforms = null;
            FacePeices = null;
            FinalizerActions = new List<Action>();

            Move = move;
            CounterRotate = counterRotate;
        }

        public void AddRingFinalizerAction()
        {
            AddRingFinalizerAction(Move, CounterRotate);
        }

        public void AddRingFinalizerAction(RubiksCubeMoves move, bool counterRotate)
        {
            FinalizerActions.Add(new Action(() => RubiksCubeControl.UpdatePositions_Rings(move, counterRotate)));
        }

        public void AddFaceFinalizerAction(Face<Circle> face)
        {
            Face<Circle> copy = face;

            if (CounterRotate)
            {
                FinalizerActions.Add(new Action(() =>
                {
                    copy.CounterRotate();
                }));
            }
            else
            {
                FinalizerActions.Add(new Action(() =>
                {
                    copy.Rotate();
                }));
            }
        }
    }
}
