
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Common.Entities.Pagination;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
namespace Common.ViewModels
{
    public abstract class FilterViewModelBase : ViewModelBase, IFilterViewModel
    {
        public readonly string InitialFilterValue = String.Empty;

        public FilterViewModelBase(IUnityContainer container, IEventAggregator eventAggregator)
            : base(container, eventAggregator)
        {
            selectedValue = InitialFilterValue;

            this.Fields = new ObservableCollection<IField>(InitializeFields());

            if (Fields.Count > 0)
            {
                IField firstField = Fields[0];
                SelectedField = firstField;
            }
        }

        #region IFilterViewModel Members

        public IField SelectedField
        {
            get
            {
                return selectedField;
            }
            set
            {
                if (value != selectedField)
                {
                    selectedField = value;
                    NotifyPropertyChanged(() => SelectedField);

                    SelectedValue = InitialFilterValue;
                }
            }
        }

        public string SelectedValue
        {
            get
            {
                return selectedValue;
            }
            set
            {
                if (value != selectedValue)
                {
                    selectedValue = value;
                    NotifyPropertyChanged(() => SelectedValue);

                    OnFilterValueChanged(SelectedField, SelectedValue);
                }
            }
        }

        public ObservableCollection<IField> Fields
        {
            get
            {
                return fields;
            }
            set
            {
                fields = value;
                NotifyPropertyChanged(() => Fields);
            }
        }

        public bool GroupingEnabled
        {
            get
            {
                return groupingEnabled;
            }
            set
            {
                if (value != groupingEnabled)
                {
                    groupingEnabled = value;
                    NotifyPropertyChanged(() => GroupingEnabled);
                }
            }
        }

        public IField GetInitialField()
        {
            return Fields[0];
        }


        #endregion

        #region Abstract methods

        public abstract IEnumerable<IField> InitializeFields();

        public abstract void OnFilterValueChanged(IField selectedField, string selectedValue);

        #endregion

        #region Private fields

        private ObservableCollection<IField> fields;
        private bool groupingEnabled;
        private IField selectedField;
        private string selectedValue;

        #endregion
    }
}
