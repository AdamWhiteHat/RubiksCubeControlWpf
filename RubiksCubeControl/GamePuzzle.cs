using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Xml.Linq;
using RubiksCubeControl.Shapes;

namespace RubiksCubeControl
{
    /// <summary>
    /// Slots are containers. They contain an Item.     
    /// A collection of containers are created as a logical grouping of game pieces, e.g. the front face, the middle slice, etc.
    /// These containers are created once per lifetime of the application, and they do not change their logical groupings.
    /// The items they contain, however, do shuffle around. The item change with every move or rotation. The items in this case are the colored face stickers of the Rubik's cube.
    /// The logical groupings, the front face, for example, never moves. The front face always stays the front face.
    /// Whether the front face contains a blue sticker or a red sticker depends on the particular permutation of the cube at the time, and changes any time you, say, rotate some right-face pieces to be in the front-face position.
    /// The game pieces are distinct. There are only 9 red stickers that exists on a Rubik's cube, for example, and each of them have a unique identity. There is only 1 NW corner (as seen from the unscrambled starting state) red sticker. 
    /// Multiple containers may have Item members that point to the same game piece instance. 
    /// This is a very useful abstraction, because it lets us address game pieces by these logical groupings: We can gather the items of the front-face containers, and rotate them. If the rotation method update the containers' items appropriately, next time you grab the items from the front-face group, they will reflect the result of that prior rotation.
    /// </summary>
    public class Slot<T> where T : class
    {
        public T Item;
        public Slot()
            : this(null)
        { }
        public Slot(T item)
        {
            Item = item;
        }
    }

    public class Slice<T> where T : class
    {
        public int Count
        {
            get { return _elements.Length; }
        }

        public Slot<T> this[int index]
        {
            get
            {
                if (index < 0 || index >= _elements.Length) { throw new IndexOutOfRangeException(); }
                return _elements[index];
            }
            set
            {
                if (index < 0 || index >= _elements.Length) { throw new IndexOutOfRangeException(); }
                _elements[index] = value;
            }
        }

        private Slot<T>[] _elements;

        private Slice() { }
        public Slice(params Slot<T>[] slots)
        {
            _elements = slots;
        }

        public static Slice<T> Factory(params T[] items)
        {
            var slots = items.Select(itm => new Slot<T>(itm)).ToArray();
            return new Slice<T>(slots);
        }

        public List<T> GetItems()
        {
            return _elements.Select(sl => sl.Item).ToList();
        }

        public List<Slot<T>> GetSlots()
        {
            return _elements.ToList();
        }

        public Slice<T> Reverse()
        {
            return new Slice<T>(_elements.Reverse().ToArray());
        }

        public void SetItems(List<T> items)
        {
            T[] array = items.ToArray();
            if (_elements.Length != array.Length)
            {
                throw new ArgumentException($"Array length mismatch. Was expecting {_elements.Length} containers, but got {array.Length} instead.");
            }

            int index = 0;
            int max = _elements.Length;

            while (index < max)
            {
                _elements[index].Item = array[index];
                index++;
            }
        }
    }

    public class Face<T> where T : class
    {
        public Slot<T> NW; public Slot<T> N; public Slot<T> NE;
        public Slot<T> W; public Slot<T> C; public Slot<T> E;
        public Slot<T> SW; public Slot<T> S; public Slot<T> SE;

        public Slice<T> Top;
        public Slice<T> Middle;
        public Slice<T> Bottom;

        public Slice<T> Left; public Slice<T> Center; public Slice<T> Right;

        public Slice<T> DiagonalForwardslash;
        public Slice<T> DiagonalBackslash;

        public Slice<T> CornerNW;
        public Slice<T> CornerNE;
        public Slice<T> CornerSE;
        public Slice<T> CornerSW;

        public Slice<T> All;
        public Slice<T> Reverse;

        public Face()
        {
            NW = new Slot<T>();
            N = new Slot<T>();
            NE = new Slot<T>();

            W = new Slot<T>();
            C = new Slot<T>();
            E = new Slot<T>();

            SW = new Slot<T>();
            S = new Slot<T>();
            SE = new Slot<T>();

            InitSlices();
        }

        public Face(T nw, T n, T ne, T w, T c, T e, T sw, T s, T se)
        {
            NW = new Slot<T>(nw);
            N = new Slot<T>(n);
            NE = new Slot<T>(ne);

            W = new Slot<T>(w);
            C = new Slot<T>(c);
            E = new Slot<T>(e);

            SW = new Slot<T>(sw);
            S = new Slot<T>(s);
            SE = new Slot<T>(se);

            InitSlices();
        }

        private void InitSlices()
        {
            Top = new Slice<T>(NW, N, NE);
            Middle = new Slice<T>(W, C, E);
            Bottom = new Slice<T>(SW, S, SE);

            Left = new Slice<T>(NW, W, SW);
            Center = new Slice<T>(N, C, S);
            Right = new Slice<T>(NE, E, SE);

            DiagonalForwardslash = new Slice<T>(NE, C, SW);
            DiagonalBackslash = new Slice<T>(NW, C, SE);

            CornerNW = new Slice<T>(W, NW, N);
            CornerNE = new Slice<T>(N, NE, E);
            CornerSE = new Slice<T>(E, SE, S);
            CornerSW = new Slice<T>(S, SW, W);

            All = new Slice<T>(NW, N, NE, W, C, E, SW, S, SE);
            Reverse = All.Reverse();
        }

        public List<T> GetItems()
        {
            return All.GetItems();
        }
    }

    public class Ring<T> where T : class
    {
        public Slot<T> A1;
        public Slot<T> A2;
        public Slot<T> A3;

        public Slot<T> B1;
        public Slot<T> B2;
        public Slot<T> B3;

        public Slot<T> C1;
        public Slot<T> C2;
        public Slot<T> C3;

        public Slot<T> D1;
        public Slot<T> D2;
        public Slot<T> D3;

        public Slice<T> A;
        public Slice<T> B;
        public Slice<T> C;
        public Slice<T> D;
        public Slice<T> All;
        public Slice<T> Reverse;

        public Ring()
        {
            A1 = new Slot<T>();
            A2 = new Slot<T>();
            A3 = new Slot<T>();

            B1 = new Slot<T>();
            B2 = new Slot<T>();
            B3 = new Slot<T>();

            C1 = new Slot<T>();
            C2 = new Slot<T>();
            C3 = new Slot<T>();

            D1 = new Slot<T>();
            D2 = new Slot<T>();
            D3 = new Slot<T>();

            InitSlices();
        }

        public Ring(Slot<T> a1, Slot<T> a2, Slot<T> a3, Slot<T> b1, Slot<T> b2, Slot<T> b3, Slot<T> c1, Slot<T> c2, Slot<T> c3, Slot<T> d1, Slot<T> d2, Slot<T> d3)
        {
            A1 = a1;
            A2 = a2;
            A3 = a3;
            B1 = b1;
            B2 = b2;
            B3 = b3;
            C1 = c1;
            C2 = c2;
            C3 = c3;
            D1 = d1;
            D2 = d2;
            D3 = d3;
            InitSlices();
        }

        public Ring(Slice<T> a, Slice<T> b, Slice<T> c, Slice<T> d)
        {
            A1 = a[0];
            A2 = a[1];
            A3 = a[2];
            B1 = b[0];
            B2 = b[1];
            B3 = b[2];
            C1 = c[0];
            C2 = c[1];
            C3 = c[2];
            D1 = d[0];
            D2 = d[1];
            D3 = d[2];
            InitSlices();
        }

        private void InitSlices()
        {
            A = new Slice<T>(A1, A2, A3);
            B = new Slice<T>(B1, B2, B3);
            C = new Slice<T>(C1, C2, C3);
            D = new Slice<T>(D1, D2, D3);
            All = new Slice<T>(A1, A2, A3, B1, B2, B3, C1, C2, C3, D1, D2, D3);
            Reverse = All.Reverse();
        }
    }

    public class Section<T> where T : class
    {
        public Ring<T> Inner;
        public Ring<T> Middle;
        public Ring<T> Outer;

        public Section(Ring<T> inner, Ring<T> middle, Ring<T> outer)
        {
            Inner = inner;
            Middle = middle;
            Outer = outer;
        }
    }

    public class GamePuzzle_Stickers<T> where T : class
    {
        public Face<T> Yellow;
        public Face<T> Green;
        public Face<T> Orange;

        public Face<T> Red;
        public Face<T> White;

        public Face<T> Blue;

        public Section<T> Top;
        public Section<T> Left;
        public Section<T> Right;

        public GamePuzzle_Stickers(Face<T> yellow, Face<T> green, Face<T> orange, Face<T> red, Face<T> white, Face<T> blue)
        {
            Yellow = yellow;
            Green = green;
            Orange = orange;
            Red = red;
            White = white;
            Blue = blue;

            Ring<T> innerTop = new Ring<T>(orange.Left, white.Top.Reverse(), red.Top.Reverse(), yellow.Right.Reverse());
            Ring<T> middleTop = new Ring<T>(orange.Center, white.Middle.Reverse(), red.Middle.Reverse(), yellow.Center.Reverse());
            Ring<T> outerTop = new Ring<T>(orange.Right, white.Bottom.Reverse(), red.Bottom.Reverse(), yellow.Left.Reverse());

            Ring<T> innerLeft = new Ring<T>(yellow.Bottom, green.CornerSW.Reverse(), white.Left, blue.CornerNW.Reverse());
            Ring<T> middleLeft = new Ring<T>(yellow.Middle, green.DiagonalBackslash, white.Center, blue.DiagonalForwardslash);
            Ring<T> outerLeft = new Ring<T>(yellow.Top, green.CornerNE, white.Right, blue.CornerSE);

            Ring<T> innerRight = new Ring<T>(blue.CornerNE.Reverse(), red.Right.Reverse(), green.CornerSE.Reverse(), orange.Bottom);
            Ring<T> middleRight = new Ring<T>(blue.DiagonalBackslash.Reverse(), red.Center.Reverse(), green.DiagonalForwardslash.Reverse(), orange.Middle);
            Ring<T> outerRight = new Ring<T>(blue.CornerSW, red.Left.Reverse(), green.CornerNW, orange.Top);

            Top = new Section<T>(innerTop, middleTop, outerTop);
            Left = new Section<T>(innerLeft, middleLeft, outerLeft);
            Right = new Section<T>(innerRight, middleRight, outerRight);
        }
    }

    public class GamePuzzle_Cubies<T> where T : class
    {

        public Slice<T> Front;
        public Slice<T> Middle;
        public Slice<T> Back;

        public Slice<T> Left;
        public Slice<T> Center;
        public Slice<T> Right;

        public Slice<T> Up;
        public Slice<T> Equator;
        public Slice<T> Down;

        public Slice<T> X;
        public Slice<T> Y;
        public Slice<T> Z;

        private Slot<T> Back_NW;
        private Slot<T> Back_N;
        private Slot<T> Back_NE;
        private Slot<T> Back_W;
        private Slot<T> Back_C;
        private Slot<T> Back_E;
        private Slot<T> Back_SW;
        private Slot<T> Back_S;
        private Slot<T> Back_SE;
        private Slot<T> Middle_NW;
        private Slot<T> Middle_N;
        private Slot<T> Middle_NE;
        private Slot<T> Middle_W;
        private Slot<T> Middle_C;
        private Slot<T> Middle_E;
        private Slot<T> Middle_SW;
        private Slot<T> Middle_S;
        private Slot<T> Middle_SE;
        private Slot<T> Front_NW;
        private Slot<T> Front_N;
        private Slot<T> Front_NE;
        private Slot<T> Front_W;
        private Slot<T> Front_C;
        private Slot<T> Front_E;
        private Slot<T> Front_SW;
        private Slot<T> Front_S;
        private Slot<T> Front_SE;

        public GamePuzzle_Cubies(T back_NW, T back_N, T back_NE, T back_W, T back_C, T back_E, T back_SW, T back_S, T back_SE, T middle_NW, T middle_N, T middle_NE, T middle_W, T middle_C, T middle_E, T middle_SW, T middle_S, T middle_SE, T front_NW, T front_N, T front_NE, T front_W, T front_C, T front_E, T front_SW, T front_S, T front_SE)
        {
            Back_NW = new Slot<T>(back_NW);
            Back_N = new Slot<T>(back_N);
            Back_NE = new Slot<T>(back_NE);
            Back_W = new Slot<T>(back_W);
            Back_C = new Slot<T>(back_C);
            Back_E = new Slot<T>(back_E);
            Back_SW = new Slot<T>(back_SW);
            Back_S = new Slot<T>(back_S);
            Back_SE = new Slot<T>(back_SE);
            Middle_NW = new Slot<T>(middle_NW);
            Middle_N = new Slot<T>(middle_N);
            Middle_NE = new Slot<T>(middle_NE);
            Middle_W = new Slot<T>(middle_W);
            Middle_C = new Slot<T>(middle_C);
            Middle_E = new Slot<T>(middle_E);
            Middle_SW = new Slot<T>(middle_SW);
            Middle_S = new Slot<T>(middle_S);
            Middle_SE = new Slot<T>(middle_SE);
            Front_NW = new Slot<T>(front_NW);
            Front_N = new Slot<T>(front_N);
            Front_NE = new Slot<T>(front_NE);
            Front_W = new Slot<T>(front_W);
            Front_C = new Slot<T>(front_C);
            Front_E = new Slot<T>(front_E);
            Front_SW = new Slot<T>(front_SW);
            Front_S = new Slot<T>(front_S);
            Front_SE = new Slot<T>(front_SE);


            Front = new Slice<T>(
                   Front_NW,
                   Front_N,
                   Front_NE,
                   Front_W,
                   Front_C,
                   Front_E,
                   Front_SW,
                   Front_S,
                   Front_SE
               );

            Middle = new Slice<T>(
                  Middle_NW,
                  Middle_N,
                  Middle_NE,
                  Middle_W,
                  Middle_C,
                  Middle_E,
                  Middle_SW,
                  Middle_S,
                  Middle_SE
              );

            Back = new Slice<T>(
                Back_NW,
                Back_N,
                Back_NE,
                Back_W,
                Back_C,
                Back_E,
                Back_SW,
                Back_S,
                Back_SE
            );

            Right = new Slice<T>(
                Front_NE,
                Middle_NE,
                Back_NE,
                Front_E,
                Middle_E,
                Back_E,
                Front_SE,
                Middle_SE,
                Back_SE
           );

            Center = new Slice<T>(
                Front_N,
                Middle_N,
                Back_N,
                Front_C,
                Middle_C,
                Back_C,
                Front_S,
                Middle_S,
                Back_S
            );

            Left = new Slice<T>(
                Front_NW,
                Middle_NW,
                Back_NW,
                Front_W,
                Middle_W,
                Back_W,
                Front_SW,
                Middle_SW,
                Back_SW
            );

            Up = new Slice<T>(
                Front_NW,
                Middle_NW,
                Back_NW,
                Front_N,
                Middle_N,
                Back_N,
                Front_NE,
                Middle_NE,
                Back_NE
            );

            Equator = new Slice<T>(
                Front_W,
                Middle_W,
                Back_W,
                Front_C,
                Middle_C,
                Back_C,
                Front_E,
                Middle_E,
                Back_E
            );

            Down = new Slice<T>(
                Front_SW,
                Middle_SW,
                Back_SW,
                Front_S,
                Middle_S,
                Back_S,
                Front_SE,
                Middle_SE,
                Back_SE
            );

            X = new Slice<T>(
                Front_NE,
                Middle_NE,
                Back_NE,
                Front_E,
                Middle_E,
                Back_E,
                Front_SE,
                Middle_SE,
                Back_SE,
                Front_N,
                Middle_N,
                Back_N,
                Front_C,
                Middle_C,
                Back_C,
                Front_S,
                Middle_S,
                Back_S,
                Front_NW,
                Middle_NW,
                Back_NW,
                Front_W,
                Middle_W,
                Back_W,
                Front_SW,
                Middle_SW,
                Back_SW
            );

            Y = new Slice<T>(
                Front_NW,
                Middle_NW,
                Back_NW,
                Front_N,
                Middle_N,
                Back_N,
                Front_NE,
                Middle_NE,
                Back_NE,
                Front_W,
                Middle_W,
                Back_W,
                Front_C,
                Middle_C,
                Back_C,
                Front_E,
                Middle_E,
                Back_E,
                Front_SW,
                Middle_SW,
                Back_SW,
                Front_S,
                Middle_S,
                Back_S,
                Front_SE,
                Middle_SE,
                Back_SE
            );

            Z = new Slice<T>(
                Front_NW,
                Front_N,
                Front_NE,
                Front_W,
                Front_C,
                Front_E,
                Front_SW,
                Front_S,
                Front_SE,
                Middle_NW,
                Middle_N,
                Middle_NE,
                Middle_W,
                Middle_C,
                Middle_E,
                Middle_SW,
                Middle_S,
                Middle_SE,
                Back_NW,
                Back_N,
                Back_NE,
                Back_W,
                Back_C,
                Back_E,
                Back_SW,
                Back_S,
                Back_SE
            );
        }
    }
}
