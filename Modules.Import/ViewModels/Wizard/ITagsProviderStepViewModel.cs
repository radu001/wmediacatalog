
using System.Collections.Generic;
using Microsoft.Practices.Prism.Commands;
using Modules.Import.Model;
using Modules.Import.ViewModels.Wizard.Common;
namespace Modules.Import.ViewModels.Wizard
{
    public interface ITagsProviderStepViewModel : IWizardViewModel
    {
        string Message { get; }
        string PathMessage { get; }
        IEnumerable<TagsProvider> AvaliableProviders { get; }
        TagsProvider SelectedProvider { get; }
        string ScanPath { get; }

        bool CanContinue { get; }

        DelegateCommand<object> SelectScanPathCommand { get; }
    }
}
