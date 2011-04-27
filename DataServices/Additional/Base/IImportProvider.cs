
using Common.Entities;
namespace DataServices.Additional.Base
{
    public interface IImportProvider
    {
        ImportProviderSettings Settings { get; set; }

        TextResult ValidateSettings();
        TextResult Import(ImportProviderSettings settings);
    }
}
