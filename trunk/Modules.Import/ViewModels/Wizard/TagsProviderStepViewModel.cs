
using System.Collections.Generic;
using Microsoft.Practices.Prism.Events;
using Modules.Import.Model;
using Modules.Import.ViewModels.Wizard.Common;
namespace Modules.Import.ViewModels.Wizard
{
    public class TagsProviderStepViewModel : WizardViewModelBase, ITagsProviderStepViewModel
    {
        public TagsProviderStepViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            InitAvaliableProviders();
        }

        #region ITagsProviderStepViewModel Members

        public string Message
        {
            get
            {
                return @"Please select tags provider to continue";
            }
        }

        public IEnumerable<TagsProvider> AvaliableProviders
        {
            get
            {
                return avaliableProviders;
            }
            private set
            {
                avaliableProviders = value;
                NotifyPropertyChanged(() => AvaliableProviders);
            }
        }

        public TagsProvider SelectedProvider
        {
            get
            {
                return selectedProvider;
            }
            set
            {
                selectedProvider = value;
                NotifyPropertyChanged(() => SelectedProvider);
            }
        }

        #endregion

        #region Private methods

        private void InitAvaliableProviders()
        {
            avaliableProviders = new List<TagsProvider>()
            {
                new TagsProvider()
                {
                    Name = "FLAC"
                },
                new TagsProvider()
                {
                    Name = "MP3"
                }
            };
        }

        #endregion

        #region Private fields

        private TagsProvider selectedProvider;
        private IEnumerable<TagsProvider> avaliableProviders;

        #endregion
    }
}
