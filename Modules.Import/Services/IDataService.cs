using System.Collections.Generic;
using BusinessObjects;
using Modules.Import.Model;

namespace Modules.Import.Services
{
    public interface IDataService
    {
        IEnumerable<Artist> BeginScan(ScanSettings settings);
    }
}
