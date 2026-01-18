using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace RubiksCubeControl.Shapes
{
    public class Cube : FrameworkElement
    {
        public GeometryModel3D Model { get; set; }

        public Point3D Center
        {
            get { return (Point3D)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }

        public Rotation3D Rotation
        {
            get { return (Rotation3D)GetValue(RotationProperty); }
            set { SetValue(RotationProperty, value); }
        }

        public static readonly DependencyProperty RotationProperty = DependencyProperty.Register(nameof(Rotation),typeof(Rotation3D),typeof(Cube),new FrameworkPropertyMetadata(default(Rotation3D),FrameworkPropertyMetadataOptions.AffectsRender,new PropertyChangedCallback(Cube.RaiseRotationChanged)));
        public static readonly RoutedEvent RotationChangedEvent = EventManager.RegisterRoutedEvent(nameof(RotationChanged), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Rotation3D>), typeof(Cube));

        public event RoutedPropertyChangedEventHandler<Rotation3D> RotationChanged
        {
            add { base.AddHandler(RotationChangedEvent, value); }
            remove { base.RemoveHandler(RotationChangedEvent, value); }
        }

        protected virtual void RaiseRotationChanged(Rotation3D oldValue, Rotation3D newValue)
        {
            RoutedPropertyChangedEventArgs<Rotation3D> e = new RoutedPropertyChangedEventArgs<Rotation3D>(oldValue, newValue);
            e.RoutedEvent = RotationChangedEvent;
            base.RaiseEvent(e);
        }

        private static void RaiseRotationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Cube element = (Cube)d;
            element.RaiseRotationChanged((Rotation3D)e.OldValue, (Rotation3D)e.NewValue);
        }

        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register(nameof(Center),typeof(Point3D),typeof(Cube),new PropertyMetadata(default(Point3D),new PropertyChangedCallback(Cube.RaiseCenterChanged)));
        public static readonly RoutedEvent CenterChangedEvent = EventManager.RegisterRoutedEvent(nameof(CenterChanged), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Point3D>), typeof(Cube));

        public event RoutedPropertyChangedEventHandler<Point3D> CenterChanged
        {
            add { base.AddHandler(CenterChangedEvent, value); }
            remove { base.RemoveHandler(CenterChangedEvent, value); }
        }

        protected virtual void RaiseCenterChanged(Point3D oldValue, Point3D newValue)
        {
            RoutedPropertyChangedEventArgs<Point3D> e = new RoutedPropertyChangedEventArgs<Point3D>(oldValue, newValue);
            e.RoutedEvent = CenterChangedEvent;
            base.RaiseEvent(e);
        }

        private static void RaiseCenterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Cube element = (Cube)d;
            element.RaiseCenterChanged((Point3D)e.OldValue, (Point3D)e.NewValue);
        }

        public Cube()
        {
        }

        public Cube(GeometryModel3D model, string name)
            : this()
        {
            Model = model;
            Name = name;
            this.RotationChanged += Cube_RotationChanged;
        }

        private void Cube_RotationChanged(object sender, RoutedPropertyChangedEventArgs<Rotation3D> e)
        {
            RotateTransform3D newRotation3D = new RotateTransform3D(e.NewValue, Center);
            Model.Transform = newRotation3D;
        }

        public void FinalizeRotationMovement(AxisAngleRotation3D finalRotationValue)
        {
            Quaternion quaternion = new Quaternion(finalRotationValue.Axis, finalRotationValue.Angle);
            quaternion.Normalize();

            Matrix3D rotateMatrix = Matrix3D.Identity;
            rotateMatrix.RotateAt(quaternion, Center);

            Matrix3D transformMatrix = Model.Transform.Value;

            //if(!transformMatrix.Equals(rotateMatrix))
            //{
            //    //throw new Exception("!transformMatrix.Equals(rotateMatrix)");
            //}

            GeometryModel3D model = Model;
            MeshGeometry3D geometry = (MeshGeometry3D)model.Geometry;
            List<Point3D> positions = geometry.Positions.ToList();

            List < Point3D > newPositions = new List<Point3D>();
            foreach (Point3D position in positions)
            {
                newPositions.Add(transformMatrix.Transform(position));
            }

            geometry.Positions = new Point3DCollection(newPositions);
            Model.Transform = Transform3D.Identity;


        }
    }
}
