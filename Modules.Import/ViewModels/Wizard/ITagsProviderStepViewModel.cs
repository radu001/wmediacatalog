
using System.Collections.Generic;
using Modules.Import.Model;
using Modules.Import.ViewModels.Wizard.Common;
namespace Modules.Import.ViewModels.Wizard
{
    public interface ITagsProviderStepViewModel : IWizardViewModel
    {
        string Message { get; }
        IEnumerable<TagsProvider> AvaliableProviders { get; }
        TagsProvider SelectedProvider { get; }
    }
}
