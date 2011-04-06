
using System;
namespace Prism.Wizards.Utils
{
    public static class StepsRegionNameResolver
    {
        public static string ResolveRegionName(string wizardName, string wizardRegionName)
        {
            return String.Format("{0}_{1}", wizardName, "StepRegion");
        }
    }
}
