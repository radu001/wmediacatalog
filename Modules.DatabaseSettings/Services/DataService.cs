
using System;
using Common.Entities;
using DataServices;
using DataServices.Additional;
namespace Modules.DatabaseSettings.Services
{
    public class DataService : IDataService
    {
        public DataService(IExportProvider exportProvider)
        {
            this.exportProvider = exportProvider;
        }

        public Exception ValidateConnection()
        {
            DataProvider provider = new DataProvider();
            return provider.ValidateConnection();
        }

        public TextResult ValidateProviderSettings(ExportProviderSettings settings)
        {
            exportProvider.Settings = settings;
            return exportProvider.ValidateSettings();
        }

        public void BeginExport(ExportProviderSettings settings, Action<bool> completeAction)
        {
            exportProvider.Settings = settings;
            exportProvider.BeginExport(completeAction);
        }

        private IExportProvider exportProvider;
    }
}
