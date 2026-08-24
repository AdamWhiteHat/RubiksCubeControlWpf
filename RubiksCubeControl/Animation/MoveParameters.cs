using RubiksCubeControl.GameState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCubeControl.Animation
{
    public class MoveParameters
    {
        public RubiksCubeMoves Move { get; set; }
        public bool CounterRotate { get; set; }
        public AnimationSpeedParameters AnimationSpeed { get; set; }

        public MoveParameters(RubiksCubeMoves move)
            : this(move, false, AnimationSpeedParameters.Default)
        { }

        public MoveParameters(RubiksCubeMoves move, bool counterRotate)
            : this(move, counterRotate, AnimationSpeedParameters.Default)
        { }

        public MoveParameters(RubiksCubeMoves move, bool counterRotate, AnimationSpeedParameters animationSpeed)
        {
            Move = move;
            CounterRotate = counterRotate;
            AnimationSpeed = animationSpeed;
        }
    }
}
