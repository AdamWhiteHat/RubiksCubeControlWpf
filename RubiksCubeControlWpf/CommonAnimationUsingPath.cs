using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace RubiksCubeControlWpf
{
    public class CommonAnimationUsingPath : DoubleAnimationUsingPath
    {
        /*
          BeginTime="00:00:02" Duration="00:00:02" 
          Source="X"
          Storyboard.TargetName="bMM" 
          Storyboard.TargetProperty="(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.X)" 
          PathGeometry="{DynamicResource MiddleRingPath}"
         */

        private string _source = "X";
        private string _targetName = "";
        private string _targetProperty { get { return string.Format("(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.{0})", _source); } }

        public CommonAnimationUsingPath(string pathGeometryResourceKey, string targetName)
            : base()
        {
            _targetName = targetName;
            BeginTime = TimeSpan.FromSeconds(2);
            Duration = TimeSpan.FromSeconds(2);
            PathGeometry = (PathGeometry)RubiksCubeControlWpf.Resources.Instance[pathGeometryResourceKey]; // "MiddleRingPath"
        }


    }
}
