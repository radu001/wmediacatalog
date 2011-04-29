using Modules.Import.Services.Utils.FileSystem;

namespace MediaCatalog.Tests.Mocks
{
    public class StubFileSelectorSettings : IFileSelectorSettings
    {
        #region IFileSelectorSettings Members

        public string[] FileMasks { get; private set; }

        #endregion

        public StubFileSelectorSettings(string[] extensionMasks)
        {
            FileMasks = extensionMasks;
        }
    }
}
