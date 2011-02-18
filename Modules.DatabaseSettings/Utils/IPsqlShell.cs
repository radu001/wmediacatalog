
using System.IO;
namespace Modules.DatabaseSettings.Utils
{
    public interface IPsqlShell
    {
        FileInfo PsqlExecutable{ get; }
    }
}
