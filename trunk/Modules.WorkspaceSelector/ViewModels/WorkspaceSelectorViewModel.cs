using System;
using System.Windows.Controls;
using Common.Commands;
using Common.Enums;
using Common.Events;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Modules.WorkspaceSelector.Services;

namespace Modules.WorkspaceSelector.ViewModels
{
    public class WorkspaceSelectorViewModel : IWorkspaceSelectorViewModel
    {
        public WorkspaceSelectorViewModel(IEventAggregator eventAggregator, IDataService dataService)
        {
            this.eventAggregator = eventAggregator;
            this.dataService = dataService;

            WorkspaceChangeCommand = new DelegateCommand<SelectionChangedArgs>(OnWorkspaceChange);
        }

        #region IWorkspaceSelectorViewModel Members

        public DelegateCommand<SelectionChangedArgs> WorkspaceChangeCommand { get; private set; }

        public void OnWorkspaceChange(SelectionChangedArgs parameter)
        {
            TabItem tabItem = parameter.SelectedValue as TabItem;
            if (tabItem != null && tabItem.Tag != null)
            {
                //Enum.Parse( tabItem.Tag.ToString()
                WorkspaceNameEnum workspace;

                if (Enum.TryParse<WorkspaceNameEnum>(tabItem.Tag.ToString(), out workspace))
                {
                    eventAggregator.GetEvent<ChangeWorkspaceEvent>().Publish(workspace);
                }
            }
        }

        #endregion

        #region Private methods

        #endregion

        #region Private fields

        private IEventAggregator eventAggregator;
        private IDataService dataService;

        #endregion
    }
}
