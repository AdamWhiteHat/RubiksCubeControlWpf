using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using RubiksCubeControlWpf.Shapes;
using PG = System.Windows.Media.PathGeometry;

namespace RubiksCubeControlWpf
{
    public static class PathHelper
    {
        public static class IndividualPaths
        {
            public static class Inner
            {

                public static List<Tuple<Circle, PointAnimationUsingPath>> Orbit(List<Circle> circles)
                {
                    int index = 0;
                    int max = _orbit.Length;

                    if (circles.Count < max)
                    {
                        throw new Exception();
                    }

                    List<Tuple<Circle, PointAnimationUsingPath>> result = new List<Tuple<Circle, PointAnimationUsingPath>>();
                    while (index < max)
                    {
                        Circle c = circles[index];

                        Point location = c.Location;

                        var animation =    PointAnimationHelper.Attempt2.WithoutAStoryboard(((PG)PG.Parse(string.Format(_orbit[index], location.X, location.Y)).GetFlattenedPathGeometry()));
                        //animation.Completed += (object? sender, EventArgs e) =>
                        //{
                        //
                        //};

                        result.Add(
                            new Tuple<Circle, PointAnimationUsingPath>(c, animation)
                        );

                        index++;
                    }

                    return result;
                }

                private static string[] _orbit = new string[]
                    {
                        "m {0},{1} c -17.989,-0.924 -30.4617,-12.24 -36.1374,-23.385",
                        "m {0},{1} c -18.1243,-7.856 -24.6638,-23.579 -26.0524,-34.278",
                        "m {0},{1} c -14.1305,-12.669 -16.4201,-28.395 -14.1741,-40.652",
                        "m {0},{1} c -8.5308,-16.714 -5.8849,-38.045 10.1845,-52.138",
                        "m {0},{1} c -2.1932,-16.966 5.889,-36.169 26.0331,-45.061",
                        "m {0},{1} c 1.936,-12.478 13.9597,-32.985 40.0857,-34.88",
                        "m {0},{1} c 12.082,-10.151 27.911,-13.137 42.2,-8.056",
                        "m {0},{1} c 12.185,-5.007 28.283,-5.371 42.598,5.383",
                        "m {0},{1} c 13.215,-0.964 29.275,5.499 38.221,19.548",
                        "m {0},{1} c 63.417,29.602 11.933,112.302 -42.177,73.178",
                        "m {0},{1} c 40.033,34.595 2.923,90.998 -42.639,73.962",
                        "m {0},{1} c 19.724,32.055 -5.476,68.127 -38.227,66.345"
                    };


            }

            public static class Middle
            {
                public static PathGeometry M_1_1(Point start) => (PG)PG.Parse(string.Format("m {0},{1} c -25.7579,-6.738 -36.8601,-26.566 -40.2485,-38.982", start.X, start.Y)).GetFlattenedPathGeometry();
            }
            public static class Outer
            {
                public static PathGeometry O_1_1(Point start) => (PG)PG.Parse(string.Format("m {0},{1} c -31.496,-14.105 -39.4236,-43.298 -40.5474,-55.847", start.X, start.Y)).GetFlattenedPathGeometry();
            }
        }


















        public static class Top
        {
            public static PathGeometry Inner(Point start)
            {
                if (_inner == null)
                {
                    _inner = (PathGeometry)PathGeometry.Parse(string.Format(Template.Inner, start.X, start.Y)).GetFlattenedPathGeometry();
                    _inner.Freeze();
                }
                return _inner;
            }
            private static PathGeometry _inner = null;

            public static PathGeometry Middle(Point start)
            {
                if (_middle == null)
                {
                    _middle = (PathGeometry)PathGeometry.Parse(string.Format(Template.Middle, start.X, start.Y)).GetFlattenedPathGeometry();
                    _middle.Freeze();
                }
                return _middle;
            }
            private static PathGeometry _middle = null;

            public static PathGeometry Outer(Point start)
            {
                if (_outer == null)
                {
                    _outer = (PathGeometry)PathGeometry.Parse(string.Format(Template.Outer, start.X, start.Y)).GetFlattenedPathGeometry();
                    _outer.Freeze();
                }
                return _outer;
            }
            private static PathGeometry _outer = null;

            private static class Template
            {
                public static string Inner = "m {0},{1} c 7.359,10.847 11.731,21.937 14.49,33.432 c 2.572,9.913 2.695,20.515 1.632,32.07 c -0.737,8.352 -3.152,19.627 -6.779,28.748 c -4.913,11.516 -12.58,21.59 -20.975,30.62 c -7.951,8.703 -17.102,14.401 -26.597,19.187 c -20.563,11.121 -53.534,14.643 -80.569,3.246 c -9.636,-3.956 -18.666,-9.206 -27.442,-16.796 c -7.151,-5.815 -15.846,-14.74 -22.834,-27.753 c -4.306,-8.838 -8.893,-19.973 -11.174,-33.208 c -1.201,-11.703 -1.328,-23.004 0.713,-33.468 c 2.583,-14.547 9.366,-27.124 14.649,-35.017 c 47.257,-67.9393 133.694,-49.4858 164.886,-1.061 z";

                public static string Middle = "m {0},{1} c 4.107,12.403 6.042,24.068 6.855,36.864 c 0.811,12.797 -0.947,23.448 -2.572,35.234 c -7.297,21.664 -17.283,42.405 -25.364,50.381 c -8.747,9.787 -15.681,17.669 -28.817,26.542 c -7.956,5.505 -20.276,12.306 -32.191,15.656 c -24.548,8.036 -54.225,7.729 -77.958,0.305 c -9.683,-1.807 -20.351,-7.649 -31.349,-13.844 c -10.274,-7.084 -20.023,-15.08 -28.681,-24.972 c -14.68,-18.207 -23.406,-39.016 -27.188,-53.369 c -3.289,-12.469 -3.868,-24.338 -3.932,-35.892 c 0.974,-13.983 3.627,-26.521 7.235,-38.247 c 40.681,-108.6557 197.521,-119.6659 243.962,1.342 z";

                public static string Outer = "m {0},{1} c 1.258,12.373 1.158,27.92 -0.688,41.281 c -1.465,10.633 -4.156,18.883 -10.268,38.121 c -6.113,19.239 -29.784,49.87 -47.536,63.837 c -15.785,12.426 -25.828,18.029 -36.558,22.472 c -10.731,4.441 -24.609,8.508 -39.3,10.968 c -14.691,2.459 -33.879,1.068 -49.061,-0.454 c -14.448,-1.452 -27.255,-6.433 -38.469,-11.254 c -11.223,-4.816 -21.012,-10.897 -35.12,-20.963 c -18.311,-13.071 -41.605,-44.668 -48.636,-65.508 c -5.758,-17.046 -8.508,-26.158 -10.111,-39.163 c -1.602,-13.005 -1.403,-28.538 0.342,-42.324 c 34.126,-187.8066 287.938,-175.93783 315.405,2.987 z";
            }
        }

        public static class Left
        {
            public static PathGeometry Inner(Point start)
            {
                if (_inner == null)
                {
                    _inner = (PathGeometry)PathGeometry.Parse(string.Format(Template.Inner, start.X, start.Y)).GetFlattenedPathGeometry();
                    _inner.Freeze();
                }
                return _inner;
            }
            private static PathGeometry _inner = null;

            public static PathGeometry Middle(Point start)
            {
                if (_middle == null)
                {
                    _middle = (PathGeometry)PathGeometry.Parse(string.Format(Template.Middle, start.X, start.Y)).GetFlattenedPathGeometry();
                    _middle.Freeze();
                }
                return _middle;
            }
            private static PathGeometry _middle = null;

            public static PathGeometry Outer(Point start)
            {
                if (_outer == null)
                {
                    _outer = (PathGeometry)PathGeometry.Parse(string.Format(Template.Outer, start.X, start.Y)).GetFlattenedPathGeometry();
                    _outer.Freeze();
                }
                return _outer;
            }
            private static PathGeometry _outer = null;

            private static class Template
            {
                public static string Inner = "m {0},{1} c 6.285,-11.5 14.152,-20.452 23.122,-28.152 c 7.641,-6.821 17.039,-11.764 27.796,-16.074 c 7.781,-3.178 18.912,-6.166 28.697,-7.105 c 12.469,-0.891 24.933,1.353 36.806,4.68 c 11.372,3.099 20.617,8.654 29.217,14.919 c 19.261,13.21 37.449,40.946 39.662,70.209 c 0.877,10.382 0.332,20.8 -2.41,32.095 c -1.925,8.992 -5.891,20.808 -14.291,32.976 c -5.889,7.858 -13.697,17.021 -24.443,25.086 c -9.856,6.421 -19.859,11.686 -30.098,14.66 c -14.119,4.331 -28.42,4.041 -37.841,2.931 c -82.035,-11.036 -105.064,-96.359 -76.217,-146.225 z";

                public static string Middle = "m {0},{1} c 8.922,-9.544 18.233,-16.834 29.076,-23.677 c 10.843,-6.842 21.035,-10.402 32.159,-14.621 c 22.513,-3.973 45.502,-5.143 56.374,-1.87 c 12.782,2.989 23.023,5.3 37.104,12.58 c 8.643,4.347 20.514,11.903 29.163,20.756 c 18.815,17.699 32.762,43.897 37.613,68.286 c 3.057,9.367 3.039,21.53 2.869,34.151 c -1.296,12.412 -3.645,24.8 -8.18,37.139 c -8.949,21.608 -23.035,39.236 -33.822,49.432 c -9.37,8.86 -19.511,15.054 -29.622,20.645 c -12.741,5.844 -25.021,9.519 -37.043,11.969 c -114.865,16.341 -199.663,-116.056 -115.691,-214.79 z";

                public static string Outer = "m {0},{1} c 9.91,-7.515 23.236,-15.522 35.605,-20.901 c 9.842,-4.285 18.286,-6.281 37.894,-11.077 c 19.608,-4.795 58.084,-0.528 79.251,7.358 c 18.827,7.009 28.839,12.667 38.217,19.517 c 9.378,6.85 20.075,16.582 29.823,27.846 c 9.747,11.264 18.547,28.37 25.154,42.127 c 6.281,13.093 8.695,26.62 10.417,38.705 c 1.73,12.089 1.633,23.613 0.382,40.899 c -1.629,22.438 -16.481,58.776 -30.615,75.627 c -11.558,13.789 -17.905,20.881 -28.175,29.019 c -10.27,8.138 -23.637,16.053 -36.316,21.74 c -178.123,68.622 -300.1111,-154.268 -161.637,-270.86 z";
            }
        }

        public static class Right
        {
            public static PathGeometry Inner(Point start)
            {
                if (_inner == null)
                {
                    _inner = (PathGeometry)PathGeometry.Parse(string.Format(Template.Inner, start.X, start.Y)).GetFlattenedPathGeometry();
                    _inner.Freeze();
                }
                return _inner;
            }
            private static PathGeometry _inner = null;

            public static PathGeometry Middle(Point start)
            {
                if (_middle == null)
                {
                    _middle = (PathGeometry)PathGeometry.Parse(string.Format(Template.Middle, start.X, start.Y)).GetFlattenedPathGeometry();
                    _middle.Freeze();
                }
                return _middle;
            }
            private static PathGeometry _middle = null;

            public static PathGeometry Outer(Point start)
            {
                if (_outer == null)
                {
                    _outer = (PathGeometry)PathGeometry.Parse(string.Format(Template.Outer, start.X, start.Y)).GetFlattenedPathGeometry();
                    _outer.Freeze();
                }
                return _outer;
            }
            private static PathGeometry _outer = null;

            private static class Template
            {
                public static string Inner = "m {0},{1} c -13.09,0.67 -24.84,-1.34 -36.1,-4.94 c -9.81,-2.94 -18.94,-8.33 -28.27,-15.23 c -6.76,-4.96 -15.15,-12.87 -21.07,-20.7 c -7.3,-10.17 -11.94,-21.95 -15.31,-33.81 c -3.32,-11.31 -3.45,-22.09 -2.62,-32.69 c 1.15,-23.35 15.23,-53.37 38.99,-70.58 c 8.38,-6.19 17.55,-11.19 28.59,-14.76 c 8.68,-3.1 20.82,-5.91 35.57,-5.14 c 9.79,0.9 21.69,2.75 34.19,7.66 c 10.63,5.04 20.36,10.79 28.25,17.96 c 11.1,9.75 18.34,22.07 22.35,30.68 c 33.6,75.63 -26.99,139.98 -84.57,141.55 z";

                public static string Middle = "m {0},{1} c -12.8,-2.62 -23.88,-6.75 -35.38,-12.42 c -11.5,-5.67 -19.86,-12.5 -29.27,-19.78 c -15.15,-17.12 -28.16,-36.11 -31.05,-47.09 c -4.13,-12.46 -7.51,-22.4 -8.66,-38.21 c -0.81,-9.64 -0.57,-23.71 2.46,-35.71 c 5.26,-25.29 20.31,-50.87 38.57,-67.75 c 6.39,-7.5 16.77,-13.84 27.62,-20.29 c 11.26,-5.38 23.05,-9.85 35.94,-12.43 c 23.1,-3.66 45.49,-0.86 59.82,3.01 c 12.45,3.36 23.03,8.77 33.08,14.47 c 11.64,7.81 21.19,16.36 29.56,25.33 c 73.95,89.4 5.37,230.88 -122.69,210.87 z";

                public static string Outer = "m {0},{1} c -11.4,-4.97 -24.9,-12.68 -35.64,-20.84 c -8.55,-6.49 -14.42,-12.88 -28.19,-27.64 c -13.77,-14.76 -28.86,-50.41 -32.33,-72.73 c -3.09,-19.85 -3.05,-31.35 -1.66,-42.88 c 1.39,-11.53 4.65,-25.62 9.71,-39.63 c 5.06,-14.01 15.68,-30.05 24.45,-42.54 c 8.35,-11.88 18.97,-20.6 28.67,-28.01 c 9.7,-7.42 19.8,-12.97 35.49,-20.33 c 20.37,-9.55 59.33,-14.36 80.94,-10.27 c 17.68,3.34 26.97,5.41 39.09,10.39 c 12.12,4.98 25.56,12.77 36.72,21.05 c 146.95,121.83 12.16,337.22 -157.25,273.43 z";
            }
        }
    }
}
