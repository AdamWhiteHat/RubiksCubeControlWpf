using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using RubiksCubeControl.Shapes;
using System.Windows.Media.Media3D;
using System.Windows.Controls;

namespace RubiksCubeControl
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

    public static class Model3DGroupExtensionMethods
    {
        public static void PopulateFromSlice(this Model3DGroup source, Slice<Cube> slice)
        {
            source.Children = new Model3DCollection(slice.GetItems().Select(c => c.Model));
        }

        public static void AddSlice(this Model3DGroup source, Slice<GeometryModel3D> slice)
        {
            var items = slice.GetItems();
            foreach (var item in items)
            {
                source.Children.Add(item);
            }
        }

        public static void RemoveSlice(this Model3DGroup source, Slice<GeometryModel3D> slice)
        {
            var items = slice.GetItems();

            int index = 0;
            int max = items.Count;

            while (index < max)
            {
                GeometryModel3D item = items[index];

                source.Children.Remove(item);

                index++;
            }
        }
    }

    public static class GamePuzzleExtensionMethods
    {
        public static List<T> GetItems<T>(this Ring<T> source) where T : class
        {
            return source.All.GetItems();
        }

        /// <summary>
        /// Clockwise ring rotation
        /// </summary>
        public static void Rotate<T>(this Ring<T> source) where T : class
        {
            List<T> slots = source.All.GetItems();

            RotationHelper.Rotate(slots);

            source.All.SetItems(slots);
        }

        /// <summary>
        /// Counter-clockwise ring rotation
        /// </summary>
        public static void CounterRotate<T>(this Ring<T> source) where T : class
        {
            List<T> slots = source.All.GetItems();

            RotationHelper.CounterRotate(slots);

            source.All.SetItems(slots);
        }

        /// <summary>
        /// Clockwise face rotation
        /// </summary>
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

        /// <summary>
        /// Counter-clockwise face rotation
        /// </summary>
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

        public static void Rotate<T>(this Slice<T> source) where T : Cube
        {
            T[] items = source.GetItems().ToArray();

            List<string> start_names = items.Select(i => i.Name).ToList();

            T NW = items[0];
            T N  = items[1];
            T NE = items[2];
            T W  = items[3];
            T C  = items[4];
            T E  = items[5];
            T SW = items[6];
            T S  = items[7];
            T SE = items[8];

            List<T> result = new List<T>()
            {
                SW,   // NW
                W,    // N
                NW,   // NE
                S,    // W
                C,    // C
                N,    // E
                SE,   // SW
                E,    // S
                NE    // SE
            };

            List<string> result_names = result.Select(i => i.Name).ToList();

            source.SetItems(result);
        }

        public static void CounterRotate<T>(this Slice<T> source) where T : Cube
        {
            T[] items = source.GetItems().ToArray();
            T NW = items[0];
            T N  = items[1];
            T NE = items[2];
            T W  = items[3];
            T C  = items[4];
            T E  = items[5];
            T SW = items[6];
            T S  = items[7];
            T SE = items[8];

            List<T> result = new List<T>()
            {
                NE,  // NW
                E,   // N
                SE,  // NE
                N,   // W
                C,   // C
                S,   // E
                NW,  // SW
                W,   // S
                SW   // SE
            };

            source.SetItems(result);
        }
    }
}
