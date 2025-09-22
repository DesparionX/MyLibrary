using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static MyLibrary.Helpers.CustomDialogTypes;

namespace MyLibrary.ViewModels
{
    public partial class CustomDialogViewModel : ObservableObject
    {
        [ObservableProperty] private string _title = string.Empty;
        [ObservableProperty] private string _message = string.Empty;
        [ObservableProperty] private string? _confirmButtonText = default;
        [ObservableProperty] private string? _cancelButtonText = default;
        [ObservableProperty] private DialogType _dType = DialogType.Info;
        [ObservableProperty] private Window? _dialog;

        public Visibility ShowCancelButton => CancelButtonText is null ? Visibility.Collapsed : Visibility.Visible;

        public CustomDialogViewModel(DialogType type, string title, string message, string? confirmButtonText = default, string? cancelButtonText = default)
        {
            Title = title;
            Message = message;
            DType = type;
            ConfirmButtonText = confirmButtonText;
            CancelButtonText = cancelButtonText;
        }

        [RelayCommand]
        public void Confirm()
        {
            Dialog?.DialogResult = true;
            Dialog?.Close();
        }

        [RelayCommand]
        public void Cancel()
        {
            Dialog?.DialogResult = false;
            Dialog?.Close();
        }
    }
}
