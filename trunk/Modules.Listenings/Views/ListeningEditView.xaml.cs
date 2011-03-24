using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Modules.Listenings.Converters;
using Modules.Listenings.ViewModels;

namespace Modules.Listenings.Views
{
    /// <summary>
    /// Interaction logic for ListeningDialog.xaml
    /// </summary>
    public partial class ListeningEditView : UserControl
    {
        public ListeningEditView(IListeningEditViewModel viewModel)
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
    }
}
