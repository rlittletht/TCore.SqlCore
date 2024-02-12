
namespace TCore.SqlCore;

public class SqlCommandTextInit
{
    public string CommandText { get; set; }
    public TableAliases? Aliases { get; set; }

    public SqlCommandTextInit(string text, TableAliases? aliases = null)
    {
        CommandText = text;
        Aliases = aliases;
    }
}
