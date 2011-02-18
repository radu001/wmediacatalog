using Common.Dialogs;
using Modules.Listenings.ViewModels;
using System.Windows.Controls;
using System.Windows.Data;
using Modules.Listenings.Converters;
using System.Windows;

namespace Modules.Listenings.Views
{
    /// <summary>
    /// Interaction logic for ListeningDialog.xaml
    /// </summary>
    public partial class ListeningDialog : DialogWindow
    {
        public ListeningDialog(IListeningEditViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;

            AlbumTextBox.SetBinding(TextBox.TextProperty, new Binding()
            {
                Source = viewModel,
                Path = new PropertyPath("Listening.Album"),
                ValidatesOnExceptions = true,
                Mode = BindingMode.TwoWay,
                Converter = new AlbumConverter()
                {
                    Listening = viewModel.Listening
                }
            });
        }

        private void HeaderBorder_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
