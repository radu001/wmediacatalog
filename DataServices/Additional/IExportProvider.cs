
using System;
using Common.Entities;
namespace DataServices.Additional
{
    public interface IExportProvider
    {
        ExportProviderSettings Settings { get; set; }

        TextResult ValidateSettings();
        void BeginExport(Action<bool> finishAction);
    }
}
