
namespace Prism.Wizards
{
    public class WizardData : IWizardData
    {
        #region IWizardData Members

        public object Value { get; private set; }

        public T GetValue<T>()
        {
            var tp = typeof(T);
            if (Value != null && Value.GetType() == tp)
            {
                return (T)Value;
            }

            return default(T);
        }

        public void SetValue<T>(T value)
        {
            Value = value;
        }

        #endregion
    }
}
