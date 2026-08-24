using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RubiksCubeApplication
{
    /// <summary>
    /// Interaction logic for NotationInputWindow.xaml
    /// </summary>
    public partial class NotationInputWindow : Window
    {
        public string InputText { get; private set; }

        public NotationInputWindow()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            OkConfirmDialog();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            CancelAbortDialog();
        }

        private void OkConfirmDialog()
        {
            InputText = notationInput.Text;
            this.DialogResult = true;
        }

        private void CancelAbortDialog()
        {
            InputText = string.Empty;
            this.DialogResult = false;
        }

        private void notationInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                if (e.Key == Key.Enter)
                {
                    if (!string.IsNullOrWhiteSpace(notationInput.Text))
                    {
                        OkConfirmDialog();
                    }
                }
            }
        }
    }
}
