using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using Microsoft.Xaml.Behaviors;

namespace RubiksCubeControl
{
    public class CameraNavigationBehavior : Behavior<UserControl>
    {
        private Point _from;
        private PerspectiveCamera _camera;

        public CameraNavigationBehavior(PerspectiveCamera camera)
        {
            _camera = camera;
        }

        protected override void OnAttached()
        {
            AssociatedObject.PreviewKeyDown += AssociatedObject_PreviewKeyDown;
            AssociatedObject.PreviewMouseMove += AssociatedObject_PreviewMouseMove;
            AssociatedObject.PreviewMouseWheel += AssociatedObject_PreviewMouseWheel;

            base.OnAttached();
        }

        private void AssociatedObject_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            _camera.MoveBy(e.Key).RotateBy(e.Key);
        }

        private void AssociatedObject_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Point till = e.GetPosition(sender as IInputElement);
            double dx = till.X - _from.X;
            double dy = till.Y - _from.Y;
            _from = till;

            var distance = dx * dx + dy * dy;
            if (distance <= 0d)
            {
                return;
            }

            double hypot = Math.Sqrt(distance);
            if (e.MouseDevice.LeftButton is MouseButtonState.Pressed)
            {
                if (Keyboard.Modifiers == ModifierKeys.Shift)
                {
                    Vector3D axis = new Vector3D(-dy, dx, 0d);
                    var angle = (distance / _camera.FieldOfView) % _camera.FieldOfView;
                    _camera.RotateAlongAxis(axis, angle);
                }
                else
                {
                    Vector3D axis = new Vector3D(-dx, -dy, 0d);
                    _camera.MoveAlongAxis(axis, 0.25 /*Math.Sqrt(distance)*/);
                }
            }
        }

        private void AssociatedObject_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            decimal value = e.Delta / 120;
            decimal scaledValue = Math.Abs(value * 0.05m);
            int sign = Math.Sign(value);

            if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
                if (sign == -1)
                {
                    _camera.FieldOfView--;
                }
                else if (sign == 1)
                {
                    _camera.FieldOfView++;
                }
            }
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.PreviewKeyDown -= AssociatedObject_PreviewKeyDown;
                AssociatedObject.PreviewMouseMove -= AssociatedObject_PreviewMouseMove;
                AssociatedObject.PreviewMouseWheel -= AssociatedObject_PreviewMouseWheel;
            }

            base.OnDetaching();
        }
    }
}
