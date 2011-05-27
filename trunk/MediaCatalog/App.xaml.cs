using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace MediaCatalog
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            Application.Current.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(Current_DispatcherUnhandledException);

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.RunWorkerAsync();

            splash = new SplashWindow();
            splash.ShowSplash();
        }

        void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if ( e.Exception != null )
                File.WriteAllText("Unhandled.log", e.Exception.ToString());
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            splash.CloseSplash();

            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //SessionFactory.Initialize();
        }

        SplashWindow splash;
    }
}
