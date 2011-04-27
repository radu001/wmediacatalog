
using System;
using Common.Entities;
using DataServices.Additional;
namespace Modules.DatabaseSettings.Services
{
    public interface IDataService
    {
        Exception ValidateConnection();
        TextResult ValidateExportSettings(ExportProviderSettings settings);
        TextResult ValidateImportSettings(ImportProviderSettings settings);

        TextResult Export(ExportProviderSettings settings);
        TextResult Import(ImportProviderSettings settings);
    }
}
