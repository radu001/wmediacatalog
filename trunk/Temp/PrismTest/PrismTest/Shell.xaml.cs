using System;
using System.Windows;

namespace PrismTest
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : Window
    {
        public Shell()
        {
            InitializeComponent();

            Closed += new EventHandler(Shell_Closed);
        }

        private void Shell_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
