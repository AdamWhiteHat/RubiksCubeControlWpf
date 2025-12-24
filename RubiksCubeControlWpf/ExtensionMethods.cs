using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using RubiksCubeControlWpf.Shapes;

namespace RubiksCubeControlWpf
{
    public static class AnimationPairExtensionMethods
    {
        public static void AddToStoryboard(this Tuple<Circle, PointAnimationUsingPath>[] tuples, Storyboard parallelTimeline)
        {
            foreach (var tup in tuples)
            {
                Storyboard.SetTarget(tup.Item2, tup.Item1);
                Storyboard.SetTargetName(tup.Item2, tup.Item1.Name);
                Storyboard.SetTargetProperty(tup.Item2, new PropertyPath(Circle.LocationProperty));
                parallelTimeline.Children.Add(tup.Item2);
            }
        }


        public static void AddToStoryboard(this Tuple<Circle, PointAnimation>[] tuples, Storyboard parallelTimeline)
        {
            foreach (var tup in tuples)
            {
                Storyboard.SetTarget(tup.Item2, tup.Item1);
                Storyboard.SetTargetName(tup.Item2, tup.Item1.Name);
                Storyboard.SetTargetProperty(tup.Item2, new PropertyPath(Circle.LocationProperty));
                parallelTimeline.Children.Add(tup.Item2);
            }
        }
    }

    public static class PathExtensionMethods
    {
        public static Path ReverseDirection(this Path source)
        {
            string pathString = source.Data.ToString();

            if (!pathString.StartsWith('M'))
            {
                throw new Exception(ReversePathExceptionMessage);
            }

            int cIndex = pathString.IndexOf('C');
            if (cIndex == -1)
            {
                throw new Exception(ReversePathExceptionMessage);
            }


            string mString = pathString.Substring(1, cIndex - 1);
            cIndex += 1;
            string cString = pathString.Substring(cIndex, pathString.Length - cIndex);

            string[] points = cString.Split(new char[] { ' ', ',' });
            if (points.Length != 6)
            {
                throw new Exception(ReversePathExceptionMessage);
            }

            List<string> pointPairs = points.Chunk(2).Select(arr => string.Join(",", arr)).ToList();

            string m = mString;
            string c1 = pointPairs[0];
            string c2 = pointPairs[1];
            string c3 = pointPairs[2];

            string result = $"M{c3}C{c2},{c1},{m}";

            return new Path()
            {
                Data = PathGeometry.Parse(result)
            };
        }
        private static string ReversePathExceptionMessage = "Can only reverse simple paths of the form Mx0,y0Cx1,y1,x2,y2,x3,y3";
    }

    public static class GameBoardExtensionMethods
    {
        public static List<T> GetItems<T>(this Ring<T> source) where T : class
        {
            return source.All.GetItems();
        }

        public static void Rotate<T>(this Ring<T> source) where T : class
        {
            List<T> slots = source.All.GetItems();

            RotationHelper.Rotate(slots);

            source.All.SetItems(slots);
        }

        public static void CounterRotate<T>(this Ring<T> source) where T : class
        {
            List<T> slots = source.All.GetItems();

            RotationHelper.CounterRotate(slots);

            source.All.SetItems(slots);
        }

        public static void Rotate<T>(this Face<T> source) where T : class
        {
            T temp = source.SW.Item;
            source.SW.Item = source.SE.Item;
            source.SE.Item = source.NE.Item;
            source.NE.Item = source.NW.Item;
            source.NW.Item = temp;

            T temp2 = source.S.Item;
            source.S.Item = source.E.Item;
            source.E.Item = source.N.Item;
            source.N.Item = source.W.Item;
            source.W.Item = temp2;
        }

        public static void CounterRotate<T>(this Face<T> source) where T : class
        {
            T temp = source.SE.Item;
            source.SE.Item = source.SW.Item;
            source.SW.Item = source.NW.Item;
            source.NW.Item = source.NE.Item;
            source.NE.Item = temp;

            T temp2 = source.E.Item;
            source.E.Item = source.S.Item;
            source.S.Item = source.W.Item;
            source.W.Item = source.N.Item;
            source.N.Item = temp2;
        }
    }
}
