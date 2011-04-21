
using System;
using System.Collections.Generic;
using FolderPickerLib;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Modules.Import.Model;
using Modules.Import.ViewModels.Wizard.Common;
using Prism.Wizards.Events;
namespace Modules.Import.ViewModels.Wizard
{
    public class TagsProviderStepViewModel : WizardViewModelBase, ITagsProviderStepViewModel
    {
        public TagsProviderStepViewModel(IUnityContainer container, IEventAggregator eventAggregator)
            : base(container, eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            InitAvaliableProviders();

            SelectScanPathCommand = new DelegateCommand<object>(OnSelectScanPathCommand);
        }

        public override void OnContinueCommand(object parameter)
        {
            var data = GetSharedData();
            data.ScanPath = ScanPath;
            data.TagsProvider = SelectedProvider;

            eventAggregator.GetEvent<CompleteWizardStepEvent>().Publish(null);
        }

        #region ITagsProviderStepViewModel Members

        public string Message
        {
            get
            {
                return @"Please select tags provider:";
            }
        }

        public string PathMessage
        {
            get
            {
                return @"Please select path containing media files:";
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
                UpdateCanContinue();
            }
        }

        public string ScanPath
        {
            get
            {
                return scanPath;
            }
            private set
            {
                scanPath = value;
                NotifyPropertyChanged(() => ScanPath);
                UpdateCanContinue();
            }
        }

        public bool CanContinue
        {
            get
            {
                return canContinue;
            }
            private set
            {
                canContinue = value;
                NotifyPropertyChanged(() => CanContinue);
            }
        }

        public DelegateCommand<object> SelectScanPathCommand { get; private set; }

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

        private void OnSelectScanPathCommand(object parameter)
        {
            FolderPickerDialog dialog = new FolderPickerDialog();
            if (dialog.ShowDialog() == true)
            {
                ScanPath = dialog.SelectedPath;
            }
        }

        private void UpdateCanContinue()
        {
            if (SelectedProvider != null && !String.IsNullOrWhiteSpace(ScanPath))
            {
                CanContinue = true;
            }
            else
            {
                CanContinue = false;
            }
        }

        #endregion

        #region Private fields

        private IEventAggregator eventAggregator;
        private TagsProvider selectedProvider;
        private IEnumerable<TagsProvider> avaliableProviders;
        private string scanPath;
        private bool canContinue;

        #endregion

    }
}
