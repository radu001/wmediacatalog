
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

        public bool ProviderSelected
        {
            get
            {
                return providerSelected;
            }
            private set
            {
                providerSelected = value;
                NotifyPropertyChanged(() => ProviderSelected);
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

        private bool providerSelected;
        private IEnumerable<TagsProvider> avaliableProviders;

        #endregion
    }
}
