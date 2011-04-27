
using System;
using Common.Entities;
using DataServices;
using DataServices.Additional;
using DataServices.Additional.Base;
namespace Modules.DatabaseSettings.Services
{
    public class DataService : IDataService
    {
        public DataService(IExportProvider exportProvider, IImportProvider importProvider)
        {
            this.exportProvider = exportProvider;
            this.importProvider = importProvider;
        }

        public Exception ValidateConnection()
        {
            DataProvider provider = new DataProvider();
            return provider.ValidateConnection();
        }

        public TextResult ValidateExportSettings(ExportProviderSettings settings)
        {
            exportProvider.Settings = settings;
            return exportProvider.ValidateSettings();
        }

        public TextResult Export(ExportProviderSettings settings)
        {
            exportProvider.Settings = settings;
            return exportProvider.Export();
        }

        public TextResult ValidateImportSettings(ImportProviderSettings settings)
        {
            importProvider.Settings = settings;
            return importProvider.ValidateSettings();
        }

        public TextResult Import(ImportProviderSettings settings)
        {
            importProvider.Settings = settings;
            return importProvider.Import(settings);
        }

        private IExportProvider exportProvider;
        private IImportProvider importProvider;
    }
}
