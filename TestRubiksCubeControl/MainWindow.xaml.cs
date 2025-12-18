using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RubiksCubeControlWpf;

namespace TestRubiksCubeControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            rubiksCubeControl.RegisterForInputEvents(this);
            rubiksCubeControl.MouseLeftButtonDown += RubiksCubeControl_MouseLeftButtonDown;
            rubiksCubeControl.QuitClientRequested += RubiksCubeControl_ClientCloseRequested;
            rubiksCubeControl.ScaleZoomChanged += RubiksCubeControl_ScaleZoomChanged;
        }

        private void RubiksCubeControl_ScaleZoomChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double width = this.ActualWidth * rubiksCubeControl.ScaleZoom;
            double height = this.ActualHeight *  rubiksCubeControl.ScaleZoom;

            double topSave =  this.Top;
            double leftSave =  this.Left;

            this.Width = width;
            this.Height = height;

            this.Top = topSave;
            this.Left = leftSave;
        }

        private void RubiksCubeControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                QuitClientRequested();
                return;
            }
        }

        private void RubiksCubeControl_ClientCloseRequested(object sender, RoutedEventArgs e)
        {
            QuitClientRequested();
        }

        private void QuitClientRequested()
        {
            this.Close();
        }
    }
}