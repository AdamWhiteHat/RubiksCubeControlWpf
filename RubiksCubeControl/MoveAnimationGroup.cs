using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using RubiksCubeControl.Shapes;

namespace RubiksCubeControl
{
    public class MoveAnimationGroup
    {
        public bool CounterRotate { get; set; }
        public RubiksCubeMoves Move { get; set; }
        public List<Circle> RingPeices { get; set; }
        public List<Path> RingPaths { get; set; }
        public List<Face<Circle>> FacePeices { get; set; }
        public List<Action> FinalizerActions { get; set; }

        public MoveAnimationGroup(RubiksCubeMoves move, bool counterRotate)
        {
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
            FinalizerActions.Add(new Action(() => RubiksCubeCtrl.UpdatePositions_Rings(move, counterRotate)));
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
