
using System;
namespace Modules.DatabaseSettings.Services
{
    public interface IDataService
    {
        Exception ValidateConnection();
    }
}
