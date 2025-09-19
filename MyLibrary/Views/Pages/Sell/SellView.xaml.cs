using MyLibrary.Helpers;
using MyLibrary.Server.Data.DTOs;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyLibrary.Views.Pages.Sell
{
    /// <summary>
    /// Interaction logic for SellView.xaml
    /// </summary>
    public partial class SellView : UserControl
    {
        private readonly SellViewModel _viewModel;

        public SellView(SellViewModel viewModel)
        {
            _viewModel = viewModel;
            InitializeComponent();
            DataContext = _viewModel;
            ChangeItemDataVisibility();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ReceiptGrid.Focus();
        }
        private void ReceiptGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var grid = sender as DataGrid;
            if (grid == null || grid.Items.Count == 0)
                return;

            int newIndex = grid.SelectedIndex;

            if (e.Key == Key.Down)
            {
                newIndex = (grid.SelectedIndex == grid.Items.Count - 1) ? 0 : grid.SelectedIndex + 1;
                e.Handled = true; // important: prevent default handling
            }
            else if (e.Key == Key.Up)
            {
                newIndex = (grid.SelectedIndex == 0) ? grid.Items.Count - 1 : grid.SelectedIndex - 1;
                e.Handled = true; // important
            }

            if (newIndex != grid.SelectedIndex)
            {
                grid.SelectedIndex = newIndex;
                grid.ScrollIntoView(grid.SelectedItem);

                grid.UpdateLayout();
                var row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(newIndex);
                row?.Focus();
            }

            var order = grid.SelectedItem as DisplayOrder;
            if (order != null)
            {
                _viewModel.SelectBookFromReceipt(order.ItemISBN!);
            }
        }
        private void ReceiptGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var grid = sender as DataGrid;

            if (grid == null || grid.Items.Count == 0)
                return;

            var order = grid.SelectedItem as DisplayOrder;
            if (order != null)
            {
                _viewModel.SelectBookFromReceipt(order.ItemISBN!);
            }
            ChangeItemDataVisibility();
        }
        private void ChangeItemDataVisibility()
        {
            var selectedItem = _viewModel.SelectedBookFromReceipt;

            if(selectedItem is not null)
            {
                Left.Visibility = Visibility.Visible;

                // Show or Hide Item's Title.
                if (!string.IsNullOrWhiteSpace(selectedItem.Title))
                {
                    ItemTitleLabel.Visibility = Visibility.Visible;
                    ItemTitle.Visibility = Visibility.Visible;
                }
                else
                {
                    ItemTitleLabel.Visibility = Visibility.Collapsed;
                    ItemTitle.Visibility = Visibility.Collapsed;
                }

                // Show or Hide Item's Genre.
                if (!string.IsNullOrWhiteSpace(selectedItem.Genre))
                {
                    ItemGenreLabel.Visibility = Visibility.Visible;
                    ItemGenre.Visibility = Visibility.Visible;
                }
                else
                {
                    ItemGenreLabel.Visibility = Visibility.Collapsed;
                    ItemGenre.Visibility = Visibility.Collapsed;
                }

                // Show or Hide Item's Author.
                if (!string.IsNullOrWhiteSpace(selectedItem.Author))
                {
                    ItemAuthorLabel.Visibility = Visibility.Visible;
                    ItemAuthor.Visibility = Visibility.Visible;
                }
                else
                {
                    ItemAuthorLabel.Visibility = Visibility.Collapsed;
                    ItemAuthor.Visibility = Visibility.Collapsed;
                }

                // Show or Hide Item's Publisher.
                if (!string.IsNullOrWhiteSpace(selectedItem.Publisher))
                {
                    ItemPublisherLabel.Visibility = Visibility.Visible;
                    ItemPublisher.Visibility = Visibility.Visible;
                }
                else
                {
                    ItemPublisherLabel.Visibility = Visibility.Collapsed;
                    ItemPublisher.Visibility = Visibility.Collapsed;
                }

                // Show or Hide Item's Description.
                if (!string.IsNullOrWhiteSpace(selectedItem.Description))
                {
                    ItemDescriptionLabel.Visibility = Visibility.Visible;
                    ItemDescription.Visibility = Visibility.Visible;
                }
                else
                {
                    ItemDescriptionLabel.Visibility = Visibility.Collapsed;
                    ItemDescription.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                Left.Visibility = Visibility.Hidden;
            }
        }
        private async void OpenNewItemDialog(object sender, EventArgs e)
        {
            await _viewModel.OpenAddItemDialog();
        }
    }
}
