
using PsqlDotnet;
namespace DbTool.Data
{
    public class PsqlScriptTask : ScriptTask
    {
        public string DbName { get; private set; }

        public IPsqlShell Shell { get; set; }

        public PsqlScriptTask(int index, string name, string description, string script, string dbName)
            :base(index, name, description, script)
        {
            DbName = dbName;
        }

        public override bool Deploy()
        {
            base.Deploy();

            bool success = false;

            try
            {
                Shell.ExecuteScript(Script, DbName);
                success = true;
            }
            catch { }

            FinishDeploy(success);

            return success;
        }
    }
}
