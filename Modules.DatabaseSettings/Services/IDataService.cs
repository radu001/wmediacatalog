
using System;
using Common.Entities;
using DataServices.Additional;
namespace Modules.DatabaseSettings.Services
{
    public interface IDataService
    {
        Exception ValidateConnection();
        TextResult ValidateProviderSettings(ExportProviderSettings settings);
        void BeginExport(ExportProviderSettings settings, Action<TextResult> completeAction);
    }
}
