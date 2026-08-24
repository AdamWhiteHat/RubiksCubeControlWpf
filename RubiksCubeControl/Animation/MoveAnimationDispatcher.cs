using RubiksCubeControl.GameState;
using RubiksCubeControl.Synchronization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RubiksCubeControl.Animation
{
    public class MoveAnimationDispatcher : IDisposable
    {
        public event RoutedEventHandler AnimationCompleted;

        private RubiksCubeControl2D _rubiksCubeControl2D;
        private RubiksCubeControl3D _rubiksCubeControl3D;
        private ConcurrentQueue<MoveParameters> _moveQueue { get; set; }
        private InterlockedCountdown _controlAnimationsCountdown;
        private bool _isDisposed = false;

        public MoveAnimationDispatcher(RubiksCubeControl2D rubiksCubeControl2D, RubiksCubeControl3D rubiksCubeControl3D)
        {
            _rubiksCubeControl2D = rubiksCubeControl2D;
            _rubiksCubeControl3D = rubiksCubeControl3D;

            _moveQueue = new ConcurrentQueue<MoveParameters>();
            _controlAnimationsCountdown = new InterlockedCountdown("AnimationDispatcher.ControlAnimationCountdown", 2);
            _controlAnimationsCountdown.CountdownComplete += ControlAnimations_CountdownComplete;

            _rubiksCubeControl2D.AnimationCompleted += RubiksCubeControl2D_AnimationCompleted;
            _rubiksCubeControl3D.AnimationCompleted += RubiksCubeControl3D_AnimationCompleted;
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                _controlAnimationsCountdown.CountdownComplete -= ControlAnimations_CountdownComplete;
                _rubiksCubeControl2D.AnimationCompleted -= RubiksCubeControl2D_AnimationCompleted;
                _rubiksCubeControl3D.AnimationCompleted -= RubiksCubeControl3D_AnimationCompleted;

                _moveQueue.Clear();
                _controlAnimationsCountdown.Reset();
                ExclusiveAccess.ReleaseLock();
            }
        }

        public void PerformMove(RubiksCubeMoves move, bool counterRotate, AnimationSpeedParameters animationSpeed)
        {
            MoveParameters parameters = new MoveParameters(move, counterRotate, animationSpeed);

            _moveQueue.Enqueue(parameters);

            if (!ExclusiveAccess.TryObtainLock())
            {
                return;
            }

            if (!_controlAnimationsCountdown.IsCompleted())
            {
                return;
            }

            ProcessNextCommand();
        }

        protected virtual void RaiseAnimationCompleted()
        {
            RoutedEventHandler routed = AnimationCompleted;
            if (routed != null)
            {
                RoutedEventArgs e = new RoutedEventArgs();
                routed.Invoke(this, e);
            }
        }

        private void ProcessNextCommand()
        {
            if (_moveQueue.TryDequeue(out MoveParameters parameters))
            {
                _controlAnimationsCountdown.Reset();
                _rubiksCubeControl2D.AnimateMove(parameters.Move, parameters.CounterRotate, parameters.AnimationSpeed);
                _rubiksCubeControl3D.AnimateMove(parameters.Move, parameters.CounterRotate, parameters.AnimationSpeed);
            }
            else
            {
                ExclusiveAccess.ReleaseLock();
            }
        }

        private void RubiksCubeControl2D_AnimationCompleted(object sender, System.Windows.RoutedEventArgs e)
        {
            _controlAnimationsCountdown.Signal();
        }

        private void RubiksCubeControl3D_AnimationCompleted(object sender, System.Windows.RoutedEventArgs e)
        {
            _controlAnimationsCountdown.Signal();
        }

        private void ControlAnimations_CountdownComplete(object? sender, EventArgs e)
        {
            RaiseAnimationCompleted();
            ProcessNextCommand();
        }
    }
}
