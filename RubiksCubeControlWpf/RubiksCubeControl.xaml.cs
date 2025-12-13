using System.Reflection;
using System.Text;
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
using RubiksCubeControlWpf;
using RubiksCubeControlWpf.Shapes;
using static RubiksCubeControlWpf.PathHelper;

namespace RubiksCubeControlWpf
{
    /// <summary>
    /// Interaction logic for RubiksCubeControl.xaml
    /// </summary>
    public partial class RubiksCubeControl : UserControl
    {
        public List<Circle> Face_Front_LeftFace => new List<Circle>() { rNW, rWW, rSW, rNN, rMM, rSS, rNE, rEE, rSE };
        public List<Circle> Face_Front_TopFace => new List<Circle>() { gNW, gWW, gSW, gNN, gMM, gSS, gNE, gEE, gSE };
        public List<Circle> Face_Front_RightFace => new List<Circle>() { wNW, wWW, wSW, wNN, wMM, wSS, wNE, wEE, wSE };


        public List<Circle> Face_Back_RightFace => new List<Circle>() { oNW, oWW, oSW, oNN, oMM, oSS, oNE, oEE, oSE };
        public List<Circle> Face_Back_LeftFace => new List<Circle>() { yNW, yWW, ySW, yNN, yMM, ySS, yNE, yEE, ySE };
        public List<Circle> Face_Back_BottomFace => new List<Circle>() { bNW, bWW, bSW, bNN, bMM, bSS, bNE, bEE, bSE };


        /* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */


        public List<Circle> Ring_Right_InnerRing => new List<Circle>() { bSE, bNE, bNN, rSE, rEE, rNE, gSS, gSE, gEE, oNW, oWW, oSW };
        public List<Circle> Ring_Right_MiddleRing => new List<Circle>() { bEE, bMM, bWW, oNN, oMM, oSS, gSW, gMM, gNE, rSS, rMM, rNN };
        public List<Circle> Ring_Right_OuterRing => new List<Circle>() { bSS, bSW, bNW, oNE, oEE, oSE, gWW, gNW, gNN, rSW, rWW, rNW };


        public List<Circle> Ring_Left_InnerRing => new List<Circle>() { bNW, bWW, bNN, wSW, wWW, wNW, gSS, gSW, gWW, yNE, yEE, ySE };
        public List<Circle> Ring_Left_MiddleRing => new List<Circle>() { bSW, bMM, bNE, wSS, wMM, wNN, gSE, gMM, gNW, yNN, yMM, ySS };
        public List<Circle> Ring_Left_OuterRing => new List<Circle>() { bSS, bEE, bSE, wSE, wEE, wNE, gEE, gNE, gNN, yNW, yWW, ySW };


        public List<Circle> Ring_Top_InnerRing => new List<Circle>() { wNW, wNN, wNE, oNW, oNN, oNE, yNW, yNN, yNE, rNW, rNN, rNE };
        public List<Circle> Ring_Top_MiddleRing => new List<Circle>() { wWW, wMM, wEE, oWW, oMM, oEE, yWW, yMM, yEE, rWW, rMM, rEE };
        public List<Circle> Ring_Top_OuterRing => new List<Circle>() { wSW, wSS, wSE, oSW, oSS, oSE, ySW, ySS, ySE, rSW, rSS, rSE };


        //public static class Faces
        //{
        //    public static List<Circle> Face_Front_LeftFace => new List<Circle>() { rNW, rWW, rSW, rNN, rMM, rSS, rNE, rEE, rSE };
        //}













        public RubiksCubeControl()
        {
            InitializeComponent();
        }

        public void RegisterForInputEvents(UIElement proxyParentControl)
        {
            proxyParentControl.KeyUp += UserControl_KeyUp;
        }

        public List<Circle> GetFace(RubiksCubeMoves face)
        {

            switch (face)
            {
                case RubiksCubeMoves.Left:
                    //Front_Left_Face.Children

                    break;
                case RubiksCubeMoves.Right:
                    break;
                case RubiksCubeMoves.Up:
                    break;
                case RubiksCubeMoves.Down:
                    break;
                case RubiksCubeMoves.Front:
                    break;
                case RubiksCubeMoves.Back:
                    break;
                default:
                    break;
            }



            throw new NotImplementedException();

        }

        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            ProcessKeyEvent(e);
        }

        private void ProcessKeyEvent(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.U:
                    AnimateMove(RubiksCubeMoves.Up);
                    break;
                case Key.D:
                    AnimateMove(RubiksCubeMoves.Down);
                    break;
                case Key.R:
                    AnimateMove(RubiksCubeMoves.Right);
                    break;
                case Key.L:
                    AnimateMove(RubiksCubeMoves.Left);
                    break;
                case Key.F:
                    AnimateMove(RubiksCubeMoves.Front);
                    break;
                case Key.B:
                    AnimateMove(RubiksCubeMoves.Back);
                    break;
                case Key.M:
                    AnimateMove(RubiksCubeMoves.Middle);
                    break;
                case Key.X:
                    AnimateMove(RubiksCubeMoves.X);
                    break;
                case Key.Y:
                    AnimateMove(RubiksCubeMoves.Y);
                    break;
                case Key.Z:
                    AnimateMove(RubiksCubeMoves.Z);
                    break;
            }
        }

        private void AnimateMove(RubiksCubeMoves move)
        {
            PathGeometry ringPath = null;
            PathGeometry facePath = null;
            List<Circle> ringPeices = new List<Circle>();
            List<Circle> facePeices = new List<Circle>();

            switch (move)
            {
                case RubiksCubeMoves.Left:
                    ringPeices = Ring_Right_OuterRing;
                    facePeices.AddRange(Face_Front_RightFace);

                    ringPath = PathHelper.Right.Inner(ringPeices[0].Location);

                    break;

                case RubiksCubeMoves.Right:
                    ringPeices = Ring_Right_InnerRing;

                    //facePeices.AddRange(Face_Back_LeftFace);

                    //ringPath = PathHelper.Right.Outer(ringPeices[0].Location);

                    List<Tuple<Circle, PointAnimationUsingPath>> animationPair =  IndividualPaths.Inner.Orbit(ringPeices);
                    animationPair.Animate();

                    return;

                case RubiksCubeMoves.Up:
                    ringPeices = Ring_Top_InnerRing;
                    facePeices.AddRange(Face_Front_TopFace);

                    ringPath = PathHelper.Top.Inner(ringPeices[0].Location);

                    break;

                case RubiksCubeMoves.Down:
                    ringPeices = Ring_Top_OuterRing;
                    facePeices.AddRange(Face_Back_BottomFace);

                    ringPath = PathHelper.Top.Outer(ringPeices[0].Location);

                    break;

                case RubiksCubeMoves.Front:
                    ringPeices = Ring_Left_InnerRing;
                    facePeices.AddRange(Face_Front_LeftFace);

                    ringPath = PathHelper.Left.Inner(ringPeices[0].Location);

                    break;

                case RubiksCubeMoves.Back:
                    ringPeices = Ring_Left_OuterRing;
                    facePeices.AddRange(Face_Back_RightFace);

                    ringPath = PathHelper.Left.Outer(ringPeices[0].Location);

                    break;

                case RubiksCubeMoves.Middle:
                    ringPeices = Ring_Right_MiddleRing;

                    ringPath = PathHelper.Right.Middle(ringPeices[0].Location);
                    break;

                case RubiksCubeMoves.Slice:
                    ringPeices = Ring_Left_MiddleRing;

                    ringPath = PathHelper.Left.Middle(ringPeices[0].Location);

                    break;

                case RubiksCubeMoves.Equator:
                    ringPeices = Ring_Top_MiddleRing;

                    ringPath = PathHelper.Top.Middle(ringPeices[0].Location);

                    break;
            }


            //if (!pathGeometry.IsFrozen) { pathGeometry.Freeze(); }

            //Circle c = ringPeices[0];


            List<Tuple<Circle, PointAnimationUsingPath>> circleAnimationTupleList = ringPeices.Select(c=> new Tuple<Circle, PointAnimationUsingPath>(c, PointAnimationHelper.Attempt2.WithoutAStoryboard(ringPath))).ToList();
            circleAnimationTupleList.Animate();

            //ParallelTimeline




            // storyBoard.Begin();





            /*

                                <Storyboard x:Key="RotateRightMiddle">

                                    <DoubleAnimationUsingPath BeginTime="00:00:02" Duration="00:00:02" 
                                                              Source="X"
                                                              Storyboard.TargetName="bMM" 
                                                              Storyboard.TargetProperty="(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.X)" 
                                                              PathGeometry="{DynamicResource MiddleRingPath}"
                                                              >

                                    </DoubleAnimationUsingPath>
                                    <DoubleAnimationUsingPath BeginTime="00:00:02" Duration="00:00:02" 
                                                              Source="Y"
                                                              Storyboard.TargetName="bMM" 
                                                              Storyboard.TargetProperty="(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.Y)"
                                                              PathGeometry="{DynamicResource MiddleRingPath}"
                                                              >

                                    </DoubleAnimationUsingPath>

                                </Storyboard>

            */



        }
    }

}
