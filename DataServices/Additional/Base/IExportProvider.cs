
using Common.Entities;
namespace DataServices.Additional.Base
{
    public interface IExportProvider
    {
        ExportProviderSettings Settings { get; set; }

        TextResult ValidateSettings();
        TextResult Export();
    }
}
