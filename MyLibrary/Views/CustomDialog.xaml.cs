using MyLibrary.ViewModels;
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
using static MyLibrary.Helpers.CustomDialogTypes;

namespace MyLibrary.Views
{
    /// <summary>
    /// Interaction logic for CustomDialog.xaml
    /// </summary>
    public partial class CustomDialog : Window
    {
        private readonly CustomDialogViewModel _viewModel;

        public CustomDialog(CustomDialogViewModel viewModel)
        {
            _viewModel = viewModel;
            InitializeComponent();
            _viewModel.Dialog = this;
            DataContext = _viewModel;
            SetBorderColor();
        }

        public void SetBorderColor()
        {
            DialogBorder.Background = _viewModel.DType switch
            {
                DialogType.Info => Brushes.WhiteSmoke,
                DialogType.Warning => Brushes.LightYellow,
                DialogType.Error => Brushes.Red,
                DialogType.Success => Brushes.LightGreen,
                _ => Brushes.WhiteSmoke,
            };
        }
    }
}
