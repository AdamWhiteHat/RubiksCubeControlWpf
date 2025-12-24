using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using RubiksCubeControl.Shapes;

namespace RubiksCubeControl
{
    public static class AnimationHelper
    {

        public static double AnimationDuration = 0.75;
        public static double AnimationAccelerationRatio = 0.333;
        public static double AnimationDecelerationRatio = 0.667;

        public static Tuple<Circle, PointAnimationUsingPath>[] BuildRingAnimation(MoveAnimationGroup moveAnimationGroup)
        {
            int index = 0;
            int max = moveAnimationGroup.RingPeices.Count;

            if (moveAnimationGroup.RingPaths.Count != moveAnimationGroup.RingPeices.Count)
            {
                throw new Exception("Paths.Count != Peices.Count");
            }

            List<Tuple<Circle, PointAnimationUsingPath>> results = new List<Tuple<Circle, PointAnimationUsingPath>>();
            while (index < max)
            {
                Circle circle = moveAnimationGroup.RingPeices[index];
                Path path = moveAnimationGroup.RingPaths[index];

                PathGeometry pathGeometry = path.Data.GetFlattenedPathGeometry();
                Point finalLocation = ExtractEndPoint(path);

                PointAnimationUsingPath animation = BuildPathAnimation(pathGeometry);
                animation.Completed += (object? sender, EventArgs e) =>
                {
                    circle.SetValue(Circle.LocationProperty, finalLocation);
                };

                results.Add(new Tuple<Circle, PointAnimationUsingPath>(circle, animation));

                index++;
            }

            return results.ToArray();
        }

        public static List<Tuple<Circle, PointAnimation>[]> BuildFaceAnimation(MoveAnimationGroup moveAnimationGroup)
        {
            List<Tuple<Circle, PointAnimation>[]> results = new List<Tuple<Circle, PointAnimation>[]>();

            foreach (var face in moveAnimationGroup.FacePeices)
            {
                List<Tuple<Circle, PointAnimation>> animationGroup = new List<Tuple<Circle, PointAnimation>>();
                Func<Circle, Point, Tuple<Circle, PointAnimation>> buildAnimation = new Func<Circle, Point, Tuple<Circle, PointAnimation>>((circle, toLocation) =>
                {
                    PointAnimation animation = new PointAnimation(circle.Location, toLocation, new Duration(System.TimeSpan.FromSeconds(AnimationHelper.AnimationDuration)), FillBehavior.Stop)
                    {
                        RepeatBehavior = new RepeatBehavior(1),
                        AutoReverse = false,
                        AccelerationRatio = AnimationHelper.AnimationAccelerationRatio,
                        DecelerationRatio = AnimationHelper.AnimationDecelerationRatio
                    };
                    animation.Completed += (object? sender, EventArgs e) =>
                    {
                        circle.SetValue(Circle.LocationProperty, toLocation);
                    };
                    return new Tuple<Circle, PointAnimation>(circle, animation);
                });

                if (moveAnimationGroup.CounterRotate)
                {
                    animationGroup.Add(buildAnimation(face.NW.Item, face.SW.Item.Location));
                    animationGroup.Add(buildAnimation(face.NE.Item, face.NW.Item.Location));
                    animationGroup.Add(buildAnimation(face.SE.Item, face.NE.Item.Location));
                    animationGroup.Add(buildAnimation(face.SW.Item, face.SE.Item.Location));

                    animationGroup.Add(buildAnimation(face.W.Item, face.S.Item.Location));
                    animationGroup.Add(buildAnimation(face.N.Item, face.W.Item.Location));
                    animationGroup.Add(buildAnimation(face.E.Item, face.N.Item.Location));
                    animationGroup.Add(buildAnimation(face.S.Item, face.E.Item.Location));
                }
                else
                {
                    animationGroup.Add(buildAnimation(face.SW.Item, face.NW.Item.Location));
                    animationGroup.Add(buildAnimation(face.NW.Item, face.NE.Item.Location));
                    animationGroup.Add(buildAnimation(face.NE.Item, face.SE.Item.Location));
                    animationGroup.Add(buildAnimation(face.SE.Item, face.SW.Item.Location));

                    animationGroup.Add(buildAnimation(face.S.Item, face.W.Item.Location));
                    animationGroup.Add(buildAnimation(face.W.Item, face.N.Item.Location));
                    animationGroup.Add(buildAnimation(face.N.Item, face.E.Item.Location));
                    animationGroup.Add(buildAnimation(face.E.Item, face.S.Item.Location));
                }

                moveAnimationGroup.AddFaceFinalizerAction(face);

                results.Add(animationGroup.ToArray());
            }

            return results;
        }

        public static Point ExtractEndPoint(Path path)
        {
            string pathString = path.Data.ToString();
            string[] pathParts = pathString.Split(',');
            int lastIndex = pathParts.Length - 1;

            double x = double.Parse(pathParts[lastIndex - 1]);
            double y = double.Parse(pathParts[lastIndex]);

            return new Point(x, y);
        }

        public static PointAnimationUsingPath BuildPathAnimation(PathGeometry path)
        {
            // Animate the Button's Width.
            PointAnimationUsingPath animatePointUsingPath = new PointAnimationUsingPath();
            animatePointUsingPath.PathGeometry = path;
            animatePointUsingPath.Duration = new Duration(TimeSpan.FromSeconds(AnimationDuration));
            animatePointUsingPath.AutoReverse = false;
            animatePointUsingPath.RepeatBehavior = new RepeatBehavior(1);
            animatePointUsingPath.AccelerationRatio = AnimationAccelerationRatio;
            animatePointUsingPath.DecelerationRatio = AnimationDecelerationRatio;
            animatePointUsingPath.FillBehavior = FillBehavior.Stop;
            return animatePointUsingPath;
        }
    }
}
