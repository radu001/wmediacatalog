using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.ViewModels;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Composite.Events;

namespace Modules.Import.ViewModels
{
    public class ImportViewModel : ViewModelBase, IImportViewModel
    {
        public ImportViewModel(IUnityContainer container, IEventAggregator eventAggregator)
            : base(container, eventAggregator)
        {
        }
    }
}
