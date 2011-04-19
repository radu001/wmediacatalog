
namespace Prism.Wizards
{
    public interface IWizardData
    {
        object Value { get; }

        T GetValue<T>();
        void SetValue<T>(T value);
    }
}
