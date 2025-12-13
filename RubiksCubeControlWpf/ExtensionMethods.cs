using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using RubiksCubeControlWpf.Shapes;

namespace RubiksCubeControlWpf
{
    public static class AnimationPairExtensionMethods
    {
        public static void Animate(this List<Tuple<Circle, PointAnimationUsingPath>> tuples)
        {
            tuples.ForEach(tup => tup.Item1.BeginAnimation(Circle.LocationProperty, tup.Item2));
        }
    }
}
