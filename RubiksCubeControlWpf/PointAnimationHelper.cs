using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using RubiksCubeControlWpf.Shapes;

namespace RubiksCubeControlWpf
{

    public static class PointAnimationHelper
    {
        public static class AnimationPairs
        {
        }




        /*
        public static void MoveAnimationFactory(RubiksCubeMoves move)
        {
            PathGeometry animationPath = null;

            switch (move)
            {
                case RubiksCubeMoves.Front:
                case RubiksCubeMoves.Up:
                    animationPath = ((PathGeometry)Resources.Instance["InnerRingPath"]).Clone();
                    break;

                case RubiksCubeMoves.Middle:
                    animationPath = ((PathGeometry)Resources.Instance["MiddleRingPath"]).Clone();
                    break;

                default:
                    animationPath = ((PathGeometry)Resources.Instance["OuterRingPath"]).Clone();
                    break;
            }

            // Freeze the PathGeometry for performance benefits.
            animationPath.Freeze();

            // Select the relevant Circles to animate, based on move enum
            List<Circle> circles = new List<Circle>();

        }
        */

        public static class Attempt2
        {
            public static PointAnimationUsingPath WithoutAStoryboard(PathGeometry path)
            {
                // Animate the Button's Width.
                PointAnimationUsingPath animatePointUsingPath = new PointAnimationUsingPath();
                animatePointUsingPath.PathGeometry = path;
                animatePointUsingPath.Duration = new Duration(TimeSpan.FromSeconds(3));
                animatePointUsingPath.AutoReverse = false;
                animatePointUsingPath.RepeatBehavior = new RepeatBehavior(1);
                animatePointUsingPath.AccelerationRatio = 0.9;
                animatePointUsingPath.DecelerationRatio = 0.1;
                animatePointUsingPath.FillBehavior = FillBehavior.HoldEnd;

                return animatePointUsingPath;
            }
        }
    }

    public static class Attempt1
    {

        public static Storyboard CircleAnimationFactory(Canvas canvas, List<Circle> circles, PathGeometry animationPath)
        {
            NameScope scope = new NameScope();
            NameScope.SetNameScope(canvas, scope);

            // Create a Storyboard to contain and apply the animation.
            Storyboard result = new Storyboard();
            result.RepeatBehavior = new RepeatBehavior(1);
            result.AutoReverse = false;

            int index = 0;
            foreach (Circle circle in circles)
            {
                PointAnimationUsingPath animationTimeline = BuildCircleAnimation(canvas, animationPath, circle, index);
                result.Children.Add(animationTimeline);
                index++;
            }

            return result;
        }

        private static PointAnimationUsingPath BuildCircleAnimation(Panel parent, PathGeometry animationPath, Circle circle, int index)
        {
            // Create the EllipseGeometry to animate.
            EllipseGeometry ellipseGeometry = new EllipseGeometry(circle.Location, circle.Radius, circle.Radius);

            //string ellipseName = $"Anim_EllipseGeometry_{circle.Name}";
            //string pathName = $"Anim_Path_{circle.Name}";

            // Register the EllipseGeometry's name with
            // the page so that it can be targeted by a
            // storyboard.
            //parent.RegisterName(circle.Name, ellipseGeometry);

            // Create a Path element to display the geometry.
            Path ellipsePath = new Path();
            ellipsePath.Data = ellipseGeometry;
            ellipsePath.Fill = circle.Stroke;
            ellipsePath.Margin = new Thickness(0);
            ellipsePath.Name = circle.Name;
            parent.RegisterName(circle.Name, ellipsePath);

            parent.Children.Add(ellipsePath);

            // Create a PointAnimationgUsingPath to move
            // the EllipseGeometry along the animation path.
            PointAnimationUsingPath anim = new PointAnimationUsingPath();
            anim.PathGeometry = animationPath;
            anim.Duration = TimeSpan.FromSeconds(3);
            anim.RepeatBehavior = new RepeatBehavior(1);
            anim.AutoReverse = false;
            anim.AccelerationRatio = 0.4;
            anim.DecelerationRatio = 0.4;
            anim.FillBehavior = FillBehavior.Stop;

            // Set the animation to target the Center property
            // of the EllipseGeometry named "AnimatedEllipseGeometry".
            Storyboard.SetTargetName(anim, circle.Name);
            Storyboard.SetTargetProperty(anim, new PropertyPath(EllipseGeometry.CenterProperty));

            return anim;
        }
    }
}
