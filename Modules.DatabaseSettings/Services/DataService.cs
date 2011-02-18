
using DataServices;
using System;
namespace Modules.DatabaseSettings.Services
{
    public class DataService : IDataService
    {
        public Exception ValidateConnection()
        {
            DataProvider provider = new DataProvider();
            return provider.ValidateConnection();
        }
    }
}
