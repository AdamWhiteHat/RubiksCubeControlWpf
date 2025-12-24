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

        public List<Path> Paths_Ring_TopInner => new List<Path>()
        {
            Path_Inner_Top_A1,   Path_Inner_Top_A2,   Path_Inner_Top_A3,
            Path_Inner_Top_B1,   Path_Inner_Top_B2,   Path_Inner_Top_B3,
            Path_Inner_Top_C1,   Path_Inner_Top_C2,   Path_Inner_Top_C3,
            Path_Inner_Top_D1,   Path_Inner_Top_D2,   Path_Inner_Top_D3
        };

        public List<Path> Paths_Ring_LeftInner => new List<Path>()
        {
            Path_Inner_Left_A1,  Path_Inner_Left_A2,  Path_Inner_Left_A3,
            Path_Inner_Left_B1,  Path_Inner_Left_B2,  Path_Inner_Left_B3,
            Path_Inner_Left_C1,  Path_Inner_Left_C2,  Path_Inner_Left_C3,
            Path_Inner_Left_D1,  Path_Inner_Left_D2,  Path_Inner_Left_D3
        };

        public List<Path> Paths_Ring_RightInner => new List<Path>()
        {
            Path_Inner_Right_A1,     Path_Inner_Right_A2,     Path_Inner_Right_A3,
            Path_Inner_Right_B1,     Path_Inner_Right_B2,     Path_Inner_Right_B3,
            Path_Inner_Right_C1,     Path_Inner_Right_C2,     Path_Inner_Right_C3,
            Path_Inner_Right_D1,     Path_Inner_Right_D2,     Path_Inner_Right_D3
        };


        public List<Path> Paths_Ring_TopMiddle => new List<Path>()
        {
            Path_Middle_Top_A1,     Path_Middle_Top_A2,     Path_Middle_Top_A3,
            Path_Middle_Top_B1,     Path_Middle_Top_B2,     Path_Middle_Top_B3,
            Path_Middle_Top_C1,     Path_Middle_Top_C2,     Path_Middle_Top_C3,
            Path_Middle_Top_D1,     Path_Middle_Top_D2,     Path_Middle_Top_D3
        };

        public List<Path> Paths_Ring_LeftMiddle => new List<Path>()
        {
            Path_Middle_Left_A1,     Path_Middle_Left_A2,     Path_Middle_Left_A3,
            Path_Middle_Left_B1,     Path_Middle_Left_B2,     Path_Middle_Left_B3,
            Path_Middle_Left_C1,     Path_Middle_Left_C2,     Path_Middle_Left_C3,
            Path_Middle_Left_D1,     Path_Middle_Left_D2,     Path_Middle_Left_D3
        };

        public List<Path> Paths_Ring_RightMiddle => new List<Path>()
        {
            Path_Middle_Right_A1,     Path_Middle_Right_A2,     Path_Middle_Right_A3,
            Path_Middle_Right_B1,     Path_Middle_Right_B2,     Path_Middle_Right_B3,
            Path_Middle_Right_C1,     Path_Middle_Right_C2,     Path_Middle_Right_C3,
            Path_Middle_Right_D1,     Path_Middle_Right_D2,     Path_Middle_Right_D3
        };



        public List<Path> Paths_Ring_TopOuter => new List<Path>()
        {
            Path_Outer_Top_A1,     Path_Outer_Top_A2,     Path_Outer_Top_A3,
            Path_Outer_Top_B1,     Path_Outer_Top_B2,     Path_Outer_Top_B3,
            Path_Outer_Top_C1,     Path_Outer_Top_C2,     Path_Outer_Top_C3,
            Path_Outer_Top_D1,     Path_Outer_Top_D2,     Path_Outer_Top_D3
        };

        public List<Path> Paths_Ring_LeftOuter => new List<Path>()
        {

            Path_Outer_Left_A1,     Path_Outer_Left_A2,     Path_Outer_Left_A3,
            Path_Outer_Left_B1,     Path_Outer_Left_B2,     Path_Outer_Left_B3,
            Path_Outer_Left_C1,     Path_Outer_Left_C2,     Path_Outer_Left_C3,
            Path_Outer_Left_D1,     Path_Outer_Left_D2,     Path_Outer_Left_D3
        };

        public List<Path> Paths_Ring_RightOuter => new List<Path>()
        {
            Path_Outer_Right_A1,     Path_Outer_Right_A2,     Path_Outer_Right_A3,
            Path_Outer_Right_B1,     Path_Outer_Right_B2,     Path_Outer_Right_B3,
            Path_Outer_Right_C1,     Path_Outer_Right_C2,     Path_Outer_Right_C3,
            Path_Outer_Right_D1,     Path_Outer_Right_D2,     Path_Outer_Right_D3
        };

        private Storyboard _storyboard { get; set; }
        private static GameBoard<Circle> _gameboard { get; set; }
        private ConcurrentQueue<Tuple<RubiksCubeMoves, bool>> _moveQueue { get; set; }

        public RubiksCubeControl()
        {
            InitializeComponent();

            this.Loaded += RubiksCubeControl_Loaded;

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


        }

        private void RubiksCubeControl_Loaded(object sender, RoutedEventArgs e)
        {
            //ProcessKeyEvent(new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.X));
            //ProcessKeyEvent(new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Y));
            //ProcessKeyEvent(new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Z));
        }


        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseQuitClientRequested();
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

            switch (move)
            {
                // *** X, Y, Z *** //

                case RubiksCubeMoves.X:

                    MoveAnimationGroup aniOuterX = new MoveAnimationGroup(RubiksCubeMoves.Left, counterRotate);
                    aniOuterX.RingPeices = _gameboard.Right.Outer.GetItems();
                    aniOuterX.RingPaths = Paths_Ring_RightOuter;
                    aniOuterX.AddRingFinalizerAction();

                    aniOuterX.FacePeices = new List<Face<Circle>>() { _gameboard.Yellow };

                    MoveAnimationGroup aniMiddleX = new MoveAnimationGroup(RubiksCubeMoves.Middle, counterRotate);
                    aniMiddleX.RingPeices = _gameboard.Right.Middle.GetItems();
                    aniMiddleX.RingPaths = Paths_Ring_RightMiddle;
                    aniMiddleX.AddRingFinalizerAction();

                    MoveAnimationGroup aniInnerX = new MoveAnimationGroup(RubiksCubeMoves.Right, counterRotate);
                    aniInnerX.RingPeices = _gameboard.Right.Inner.GetItems();
                    aniInnerX.RingPaths = Paths_Ring_RightInner;
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
                    aniOuterY.AddRingFinalizerAction();

                    aniOuterY.FacePeices = new List<Face<Circle>>() { _gameboard.Blue };

                    MoveAnimationGroup aniMiddleY = new MoveAnimationGroup(RubiksCubeMoves.Equator, counterRotate);
                    aniMiddleY.RingPeices = _gameboard.Top.Middle.GetItems();
                    aniMiddleY.RingPaths = Paths_Ring_TopMiddle;
                    aniMiddleY.AddRingFinalizerAction();

                    MoveAnimationGroup aniInnerY = new MoveAnimationGroup(RubiksCubeMoves.Up, counterRotate);
                    aniInnerY.RingPeices = _gameboard.Top.Inner.GetItems();
                    aniInnerY.RingPaths = Paths_Ring_TopInner;
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
                    aniOuterZ.AddRingFinalizerAction();

                    aniOuterZ.FacePeices = new List<Face<Circle>>() { _gameboard.Orange };

                    MoveAnimationGroup aniMiddleZ = new MoveAnimationGroup(RubiksCubeMoves.Slice, counterRotate);
                    aniMiddleZ.RingPeices = _gameboard.Left.Middle.GetItems();
                    aniMiddleZ.RingPaths = Paths_Ring_LeftMiddle;
                    aniMiddleZ.AddRingFinalizerAction();

                    MoveAnimationGroup aniInnerZ = new MoveAnimationGroup(RubiksCubeMoves.Front, counterRotate);
                    aniInnerZ.RingPeices = _gameboard.Left.Inner.GetItems();
                    aniInnerZ.RingPaths = Paths_Ring_LeftInner;
                    aniInnerZ.AddRingFinalizerAction();

                    aniInnerZ.FacePeices = new List<Face<Circle>>() { _gameboard.Red };

                    moveAnimations.Add(aniOuterZ);
                    moveAnimations.Add(aniMiddleZ);
                    moveAnimations.Add(aniInnerZ);

                    break;

                // *** Inner *** //

                case RubiksCubeMoves.Up:
                    MoveAnimationGroup uAni = new MoveAnimationGroup(move, counterRotate);

                    uAni.RingPeices = _gameboard.Top.Inner.GetItems();
                    uAni.RingPaths = Paths_Ring_TopInner;
                    uAni.AddRingFinalizerAction();

                    uAni.FacePeices = new List<Face<Circle>>() { _gameboard.Green };

                    moveAnimations.Add(uAni);
                    break;

                case RubiksCubeMoves.Front:
                    MoveAnimationGroup fAni = new MoveAnimationGroup(move, counterRotate);

                    fAni.RingPeices = _gameboard.Left.Inner.GetItems();
                    fAni.RingPaths = Paths_Ring_LeftInner;
                    fAni.AddRingFinalizerAction();

                    fAni.FacePeices = new List<Face<Circle>>() { _gameboard.Red };

                    moveAnimations.Add(fAni);
                    break;

                case RubiksCubeMoves.Right:
                    MoveAnimationGroup rAni = new MoveAnimationGroup(move, counterRotate);

                    rAni.RingPeices = _gameboard.Right.Inner.GetItems();
                    rAni.RingPaths = Paths_Ring_RightInner;
                    rAni.AddRingFinalizerAction();

                    rAni.FacePeices = new List<Face<Circle>>() { _gameboard.White };

                    moveAnimations.Add(rAni);

                    break;

                // *** Middle *** //

                case RubiksCubeMoves.Equator:
                    MoveAnimationGroup eAni = new MoveAnimationGroup(move, counterRotate);

                    eAni.RingPeices = _gameboard.Top.Middle.GetItems();
                    eAni.RingPaths = Paths_Ring_TopMiddle;
                    eAni.AddRingFinalizerAction();

                    moveAnimations.Add(eAni);
                    break;

                case RubiksCubeMoves.Slice:
                    MoveAnimationGroup sAni = new MoveAnimationGroup(move, counterRotate);

                    sAni.RingPeices = _gameboard.Left.Middle.GetItems();
                    sAni.RingPaths = Paths_Ring_LeftMiddle;
                    sAni.AddRingFinalizerAction();

                    moveAnimations.Add(sAni);
                    break;

                case RubiksCubeMoves.Middle:
                    MoveAnimationGroup mAni = new MoveAnimationGroup(move, counterRotate);

                    mAni.RingPeices = _gameboard.Right.Middle.GetItems();
                    mAni.RingPaths = Paths_Ring_RightMiddle;
                    mAni.AddRingFinalizerAction();

                    moveAnimations.Add(mAni);
                    break;

                // *** Outer *** //

                case RubiksCubeMoves.Down:
                    MoveAnimationGroup dAni = new MoveAnimationGroup(move, counterRotate);

                    dAni.RingPeices = _gameboard.Top.Outer.GetItems();
                    dAni.RingPaths = Paths_Ring_TopOuter;
                    dAni.AddRingFinalizerAction();

                    dAni.FacePeices = new List<Face<Circle>>() { _gameboard.Blue };

                    moveAnimations.Add(dAni);
                    break;

                case RubiksCubeMoves.Back:
                    MoveAnimationGroup bAni = new MoveAnimationGroup(move, counterRotate);

                    bAni.RingPeices = _gameboard.Left.Outer.GetItems();
                    bAni.RingPaths = Paths_Ring_LeftOuter;
                    bAni.AddRingFinalizerAction();

                    bAni.FacePeices = new List<Face<Circle>>() { _gameboard.Orange };

                    moveAnimations.Add(bAni);
                    break;

                case RubiksCubeMoves.Left:
                    MoveAnimationGroup lAni = new MoveAnimationGroup(move, counterRotate);

                    lAni.RingPeices = _gameboard.Right.Outer.GetItems();
                    lAni.RingPaths = Paths_Ring_RightOuter;
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
                    faceAnimation.AddToStoryboard(_storyboard);
                }
            }

            _storyboard.Completed += MoveCompletedHandler;
            _storyboard.Begin();

            peicePositionsUpdates.ForEach(action => action.Invoke());

        }

        private void MoveCompletedHandler(object? source, EventArgs args)
        {
            _storyboard.Completed -= MoveCompletedHandler;
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

        private void helpButton_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow helpWindow = new HelpWindow()
            {
                Owner = Window.GetWindow(this)
            };

            helpWindow.Show();
        }
    }

}
