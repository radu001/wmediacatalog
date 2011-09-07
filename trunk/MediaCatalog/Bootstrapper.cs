using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Modules.Albums;
using Modules.Artists;
using Modules.DatabaseSettings;
using Modules.Import;
using Modules.Listenings;
using Modules.Login;
using Modules.Main;
using Modules.Notifications;
using Modules.Tags;
using Modules.WorkspaceSelector;

namespace MediaCatalog
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {

            Shell shell = Container.TryResolve<Shell>();
            shell.Show();
            return shell;
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            ModuleCatalog moduleCatalog = new ModuleCatalog();
            moduleCatalog.AddModule(typeof(NotificationsModule));
            moduleCatalog.AddModule(typeof(LoginModule));
            moduleCatalog.AddModule(typeof(MainModule));
            moduleCatalog.AddModule(typeof(DatabaseSettingsModule));
            moduleCatalog.AddModule(typeof(TagsModule));
            moduleCatalog.AddModule(typeof(ArtistsModule));
            moduleCatalog.AddModule(typeof(AlbumsModule));
            moduleCatalog.AddModule(typeof(ListeningsModule));
            moduleCatalog.AddModule(typeof(ImportModule));
            moduleCatalog.AddModule(typeof(WorkspaceSelectorModule));
            return moduleCatalog;
        }
    }
}
