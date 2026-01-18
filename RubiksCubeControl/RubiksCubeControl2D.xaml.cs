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
using RubiksCubeControl;
using RubiksCubeControl.Shapes;
using static RubiksCubeControl.AnimationHelper;

namespace RubiksCubeControl
{
    /// <summary>
    /// Interaction logic for RubiksCubeControl2D.xaml
    /// </summary>
    public partial class RubiksCubeControl2D : UserControl
    {
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
        private static GamePuzzle_Stickers<Circle> _game { get; set; }

        public RubiksCubeControl2D()
        {
            InitializeComponent();

            this.Loaded += RubiksCubeControl_Loaded;

            _storyboard = new Storyboard();
            _storyboard.FillBehavior = FillBehavior.Stop;

            Face<Circle> yellow = new Face<Circle>(yNW, yNN, yNE, yWW, yCC, yEE, ySW, ySS, ySE);
            Face<Circle> green = new Face<Circle>(gNW, gNN, gNE, gWW, gCC, gEE, gSW, gSS, gSE);
            Face<Circle> orange = new Face<Circle>(oNW, oNN, oNE, oWW, oCC, oEE, oSW, oSS, oSE);
            Face<Circle> red = new Face<Circle>(rNW, rNN, rNE, rWW, rCC, rEE, rSW, rSS, rSE);
            Face<Circle> white = new Face<Circle>(wNW, wNN, wNE, wWW, wCC, wEE, wSW, wSS, wSE);
            Face<Circle> blue = new Face<Circle>(bNW, bNN, bNE, bWW, bCC, bEE, bSW, bSS, bSE);
            _game = new GamePuzzle_Stickers<Circle>(yellow, green, orange, red, white, blue);

        }

        private void RubiksCubeControl_Loaded(object sender, RoutedEventArgs e)
        {
            //ProcessKeyEvent(new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.X));
            //ProcessKeyEvent(new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Y));
            //ProcessKeyEvent(new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Z));
        }

        private void CubeNet_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.OnMouseLeftButtonDown(e);
        }

        public static void UpdatePositions_Rings(RubiksCubeMoves move, bool counterRotate)
        {
            Action<Ring<Circle>> rotateFunc = GamePuzzleExtensionMethods.Rotate<Circle>;
            if (counterRotate)
            {
                rotateFunc = GamePuzzleExtensionMethods.CounterRotate<Circle>;
            }

            switch (move)
            {
                case RubiksCubeMoves.Middle: rotateFunc.Invoke(_game.Right.Middle); break;
                case RubiksCubeMoves.Slice: rotateFunc.Invoke(_game.Left.Middle); break;
                case RubiksCubeMoves.Equator: rotateFunc.Invoke(_game.Top.Middle); break;

                case RubiksCubeMoves.Left:
                    rotateFunc.Invoke(_game.Right.Outer);
                    break;
                case RubiksCubeMoves.Right:
                    rotateFunc.Invoke(_game.Right.Inner);
                    break;
                case RubiksCubeMoves.Up:
                    rotateFunc.Invoke(_game.Top.Inner);
                    break;
                case RubiksCubeMoves.Down:
                    rotateFunc.Invoke(_game.Top.Outer);
                    break;
                case RubiksCubeMoves.Front:
                    rotateFunc.Invoke(_game.Left.Inner);
                    break;
                case RubiksCubeMoves.Back:
                    rotateFunc.Invoke(_game.Left.Outer);
                    break;

                case RubiksCubeMoves.X:
                    rotateFunc.Invoke(_game.Right.Inner);
                    rotateFunc.Invoke(_game.Right.Middle);
                    rotateFunc.Invoke(_game.Right.Outer);
                    break;
                case RubiksCubeMoves.Y:
                    rotateFunc.Invoke(_game.Top.Inner);
                    rotateFunc.Invoke(_game.Top.Middle);
                    rotateFunc.Invoke(_game.Top.Outer);
                    break;
                case RubiksCubeMoves.Z:
                    rotateFunc.Invoke(_game.Left.Inner);
                    rotateFunc.Invoke(_game.Left.Middle);
                    rotateFunc.Invoke(_game.Left.Outer);
                    break;
            }
        }


        public void AnimateMove(RubiksCubeMoves move, bool counterRotate)
        {
            List<MoveAnimationGroup> moveAnimations = new List<MoveAnimationGroup>();

            switch (move)
            {
                // *** X, Y, Z *** //

                case RubiksCubeMoves.X:

                    MoveAnimationGroup aniOuterX = new MoveAnimationGroup(RubiksCubeMoves.Left, counterRotate);
                    aniOuterX.RingPeices = _game.Right.Outer.GetItems();
                    aniOuterX.RingPaths = Paths_Ring_RightOuter;
                    aniOuterX.AddRingFinalizerAction();

                    aniOuterX.FacePeices = new List<Face<Circle>>() { _game.Yellow };

                    MoveAnimationGroup aniMiddleX = new MoveAnimationGroup(RubiksCubeMoves.Middle, counterRotate);
                    aniMiddleX.RingPeices = _game.Right.Middle.GetItems();
                    aniMiddleX.RingPaths = Paths_Ring_RightMiddle;
                    aniMiddleX.AddRingFinalizerAction();

                    MoveAnimationGroup aniInnerX = new MoveAnimationGroup(RubiksCubeMoves.Right, counterRotate);
                    aniInnerX.RingPeices = _game.Right.Inner.GetItems();
                    aniInnerX.RingPaths = Paths_Ring_RightInner;
                    aniInnerX.AddRingFinalizerAction();

                    aniInnerX.FacePeices = new List<Face<Circle>>() { _game.White };

                    moveAnimations.Add(aniOuterX);
                    moveAnimations.Add(aniMiddleX);
                    moveAnimations.Add(aniInnerX);

                    break;

                case RubiksCubeMoves.Y:

                    MoveAnimationGroup aniOuterY = new MoveAnimationGroup(RubiksCubeMoves.Down, counterRotate);
                    aniOuterY.RingPeices = _game.Top.Outer.GetItems();
                    aniOuterY.RingPaths = Paths_Ring_TopOuter;
                    aniOuterY.AddRingFinalizerAction();

                    aniOuterY.FacePeices = new List<Face<Circle>>() { _game.Blue };

                    MoveAnimationGroup aniMiddleY = new MoveAnimationGroup(RubiksCubeMoves.Equator, counterRotate);
                    aniMiddleY.RingPeices = _game.Top.Middle.GetItems();
                    aniMiddleY.RingPaths = Paths_Ring_TopMiddle;
                    aniMiddleY.AddRingFinalizerAction();

                    MoveAnimationGroup aniInnerY = new MoveAnimationGroup(RubiksCubeMoves.Up, counterRotate);
                    aniInnerY.RingPeices = _game.Top.Inner.GetItems();
                    aniInnerY.RingPaths = Paths_Ring_TopInner;
                    aniInnerY.AddRingFinalizerAction();

                    aniInnerY.FacePeices = new List<Face<Circle>>() { _game.Green };

                    moveAnimations.Add(aniOuterY);
                    moveAnimations.Add(aniMiddleY);
                    moveAnimations.Add(aniInnerY);

                    break;

                case RubiksCubeMoves.Z:

                    MoveAnimationGroup aniOuterZ = new MoveAnimationGroup(RubiksCubeMoves.Back, counterRotate);
                    aniOuterZ.RingPeices = _game.Left.Outer.GetItems();
                    aniOuterZ.RingPaths = Paths_Ring_LeftOuter;
                    aniOuterZ.AddRingFinalizerAction();

                    aniOuterZ.FacePeices = new List<Face<Circle>>() { _game.Orange };

                    MoveAnimationGroup aniMiddleZ = new MoveAnimationGroup(RubiksCubeMoves.Slice, counterRotate);
                    aniMiddleZ.RingPeices = _game.Left.Middle.GetItems();
                    aniMiddleZ.RingPaths = Paths_Ring_LeftMiddle;
                    aniMiddleZ.AddRingFinalizerAction();

                    MoveAnimationGroup aniInnerZ = new MoveAnimationGroup(RubiksCubeMoves.Front, counterRotate);
                    aniInnerZ.RingPeices = _game.Left.Inner.GetItems();
                    aniInnerZ.RingPaths = Paths_Ring_LeftInner;
                    aniInnerZ.AddRingFinalizerAction();

                    aniInnerZ.FacePeices = new List<Face<Circle>>() { _game.Red };

                    moveAnimations.Add(aniOuterZ);
                    moveAnimations.Add(aniMiddleZ);
                    moveAnimations.Add(aniInnerZ);

                    break;

                // *** Inner *** //

                case RubiksCubeMoves.Up:
                    MoveAnimationGroup uAni = new MoveAnimationGroup(move, counterRotate);

                    uAni.RingPeices = _game.Top.Inner.GetItems();
                    uAni.RingPaths = Paths_Ring_TopInner;
                    uAni.AddRingFinalizerAction();

                    uAni.FacePeices = new List<Face<Circle>>() { _game.Green };

                    moveAnimations.Add(uAni);
                    break;

                case RubiksCubeMoves.Front:
                    MoveAnimationGroup fAni = new MoveAnimationGroup(move, counterRotate);

                    fAni.RingPeices = _game.Left.Inner.GetItems();
                    fAni.RingPaths = Paths_Ring_LeftInner;
                    fAni.AddRingFinalizerAction();

                    fAni.FacePeices = new List<Face<Circle>>() { _game.Red };

                    moveAnimations.Add(fAni);
                    break;

                case RubiksCubeMoves.Right:
                    MoveAnimationGroup rAni = new MoveAnimationGroup(move, counterRotate);

                    rAni.RingPeices = _game.Right.Inner.GetItems();
                    rAni.RingPaths = Paths_Ring_RightInner;
                    rAni.AddRingFinalizerAction();

                    rAni.FacePeices = new List<Face<Circle>>() { _game.White };

                    moveAnimations.Add(rAni);

                    break;

                // *** Middle *** //

                case RubiksCubeMoves.Equator:
                    MoveAnimationGroup eAni = new MoveAnimationGroup(move, counterRotate);

                    eAni.RingPeices = _game.Top.Middle.GetItems();
                    eAni.RingPaths = Paths_Ring_TopMiddle;
                    eAni.AddRingFinalizerAction();

                    moveAnimations.Add(eAni);
                    break;

                case RubiksCubeMoves.Slice:
                    MoveAnimationGroup sAni = new MoveAnimationGroup(move, counterRotate);

                    sAni.RingPeices = _game.Left.Middle.GetItems();
                    sAni.RingPaths = Paths_Ring_LeftMiddle;
                    sAni.AddRingFinalizerAction();

                    moveAnimations.Add(sAni);
                    break;

                case RubiksCubeMoves.Middle:
                    MoveAnimationGroup mAni = new MoveAnimationGroup(move, counterRotate);

                    mAni.RingPeices = _game.Right.Middle.GetItems();
                    mAni.RingPaths = Paths_Ring_RightMiddle;
                    mAni.AddRingFinalizerAction();

                    moveAnimations.Add(mAni);
                    break;

                // *** Outer *** //

                case RubiksCubeMoves.Down:
                    MoveAnimationGroup dAni = new MoveAnimationGroup(move, counterRotate);

                    dAni.RingPeices = _game.Top.Outer.GetItems();
                    dAni.RingPaths = Paths_Ring_TopOuter;
                    dAni.AddRingFinalizerAction();

                    dAni.FacePeices = new List<Face<Circle>>() { _game.Blue };

                    moveAnimations.Add(dAni);
                    break;

                case RubiksCubeMoves.Back:
                    MoveAnimationGroup bAni = new MoveAnimationGroup(move, counterRotate);

                    bAni.RingPeices = _game.Left.Outer.GetItems();
                    bAni.RingPaths = Paths_Ring_LeftOuter;
                    bAni.AddRingFinalizerAction();

                    bAni.FacePeices = new List<Face<Circle>>() { _game.Orange };

                    moveAnimations.Add(bAni);
                    break;

                case RubiksCubeMoves.Left:
                    MoveAnimationGroup lAni = new MoveAnimationGroup(move, counterRotate);

                    lAni.RingPeices = _game.Right.Outer.GetItems();
                    lAni.RingPaths = Paths_Ring_RightOuter;
                    lAni.AddRingFinalizerAction();

                    lAni.FacePeices = new List<Face<Circle>>() { _game.Yellow };

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

            EventHandler finalUpdateAction = null;
            finalUpdateAction = new EventHandler((s, e) =>
            {
                _storyboard.Completed -= finalUpdateAction;
                peicePositionsUpdates.ForEach(action => action());
                RaiseAnimationCompleted();
            });

            _storyboard.Completed += finalUpdateAction;
            _storyboard.Begin();
        }
    }
}
