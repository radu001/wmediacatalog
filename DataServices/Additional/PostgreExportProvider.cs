using System;
using Common.Entities;

namespace DataServices.Additional
{
    public class PostgreExportProvider : IExportProvider
    {
        #region IExportProvider Members

        public ExportProviderSettings Settings { get; set; }

        public TextResult ValidateSettings()
        {
            throw new NotImplementedException();
        }

        public void BeginExport(Action<bool> finishAction)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
