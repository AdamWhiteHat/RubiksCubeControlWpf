using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace RubiksCubeControl
{
    public static class ProjectionCameraExtensions
    {
        public static Vector3D GetYawAxis(this ProjectionCamera camera) => camera.UpDirection;
        public static Vector3D GetRollAxis(this ProjectionCamera camera) => camera.LookDirection;
        public static Vector3D GetPitchAxis(this ProjectionCamera camera) => Vector3D.CrossProduct(camera.UpDirection, camera.LookDirection);

        public static void LookAtPoint(this ProjectionCamera camera, Point3D point)
        {
            Vector3D newLookDir = point - camera.Position;
            camera.LookDirection = newLookDir;
        }

        #region Move

        public static PerspectiveCamera MoveBy(this PerspectiveCamera camera, Key key)  => camera.MoveBy(key, camera.FieldOfView / 180d);

        public static TCamera MoveBy<TCamera>(this TCamera camera, Key key, double step) where TCamera : ProjectionCamera => key switch
        {
            Key.W => camera.MoveAlongAxis(Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) ? camera.GetYawAxis() : camera.GetRollAxis(), +step),
            Key.S => camera.MoveAlongAxis(Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) ? camera.GetYawAxis() : camera.GetRollAxis(), -step),
            Key.A => camera.MoveAlongAxis(camera.GetPitchAxis(), +step),

            Key.D => camera.MoveAlongAxis(camera.GetPitchAxis(), -step),
            Key.Space => camera.MoveAlongAxis(camera.GetYawAxis(), +step),

            _ => camera
        };

        public static TCamera MoveAlongAxis<TCamera>(this TCamera camera, Vector3D axis, double step)
            where TCamera : ProjectionCamera
        {
            camera.Position += axis * step;
            return camera;
        }

        #endregion

        #region Rotate

        public static PerspectiveCamera RotateBy(this PerspectiveCamera camera, Key key) => camera.RotateBy(key, camera.FieldOfView / 45d);

        public static TCamera RotateBy<TCamera>(this TCamera camera, Key key, double angle) where TCamera : ProjectionCamera
            => key switch
            {
                Key.Left => camera.RotateAlongAxis(camera.GetYawAxis(), +angle),
                Key.Right => camera.RotateAlongAxis(camera.GetYawAxis(), -angle),
                Key.Down => camera.RotateAlongAxis(camera.GetPitchAxis(), +angle),
                Key.Up => camera.RotateAlongAxis(camera.GetPitchAxis(), -angle),

                _ => camera
            };

        public static TCamera RotateAlongAxis<TCamera>(this TCamera camera, Vector3D axis, double angle)
            where TCamera : ProjectionCamera
        {
            Matrix3D matrix3D = new();
            matrix3D.RotateAt(new(axis, angle), camera.Position);
            camera.LookDirection *= matrix3D;
            return camera;
        }

        #endregion

    }
}
