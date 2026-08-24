using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCubeControl.Animation
{
    public class AnimationSpeedParameters
    {
        public static AnimationSpeedParameters Default = new AnimationSpeedParameters()
        {
            Duration = 0.75,
            AccelerationRatio = 0.333,
            DecelerationRatio = 0.667
        };

        public static AnimationSpeedParameters Quick = new AnimationSpeedParameters()
        {
            Duration = 0.25,
            AccelerationRatio = 0.2,
            DecelerationRatio = 0.8
        };

        public static AnimationSpeedParameters Immediate = new AnimationSpeedParameters()
        {
            Duration = 0.15,
            AccelerationRatio = 0.1,
            DecelerationRatio = 0.9
        };

        public double Duration { get; set; }
        public double AccelerationRatio { get; set; }
        public double DecelerationRatio { get; set; }
    }

}
