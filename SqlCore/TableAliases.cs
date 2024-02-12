namespace TCore.SqlCore;

public class TableAliases
{
    public Dictionary<string, string> Aliases { get; set; }

    public TableAliases()
    {
        Aliases = new Dictionary<string, string>();
    }

    public TableAliases(Dictionary<string, string> aliases)
    {
        Aliases = aliases;
    }

    /// <summary>
    /// Add the table with an automatically generated alias
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public string AddAlias(string tableName)
    {
        string sAlias = $"S{Aliases.Count}";

        AddAlias(tableName, sAlias);
        return sAlias;
    }

    /// <summary>
    /// Add an alias for the given tableName
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="alias"></param>
    /// <returns></returns>
    public string AddAlias(string tableName, string alias)
    {
        Aliases.Add(tableName, alias);

        return alias;
    }

    /// <summary>
    /// Add a collection of aliases
    /// </summary>
    /// <param name="mpAliases"></param>
    public void SetAliases(Dictionary<string, string> mpAliases)
    {
        Aliases.Clear();
       
        foreach (string sKey in mpAliases.Keys)
        {
            AddAlias(sKey, mpAliases[sKey]);
        }
    }

    /// <summary>
    /// Add a collection of aliases
    /// </summary>
    /// <param name="aliases"></param>
    public void SetAliases(TableAliases aliases)
    {
        Aliases.Clear();

        foreach (string sKey in aliases.Aliases.Keys)
        {
            AddAlias(sKey, aliases.Aliases[sKey]);
        }
    }


    private static string? LookupAliasFromTableToAliasMap(string sTable, Dictionary<string, string> mapTableToAlias)
    {
        foreach (KeyValuePair<string, string> entry in mapTableToAlias)
        {
            if (String.CompareOrdinal(entry.Key, sTable) == 0)
                return entry.Value;
        }

        return null;
    }

    /// <summary>
    /// Expand the tableNames and aliases in the given query
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <exception cref="SqlExceptionMalformedQuery"></exception>
    public string ExpandAliases(string query)
    {
        if (Aliases.Keys.Count == 0)
            return query;

        int ich;

        while ((ich = query.IndexOf("$$", StringComparison.Ordinal)) != -1)
        {
            int ichLast = query.IndexOf("$$", ich + 2, StringComparison.Ordinal);

            if (ichLast == -1)
                return query;

            int ichAdjust = query.Substring(ich + 2, 1) == "#" ? 1 : 0;
            ich += ichAdjust;
            string sTable = query.Substring(ich + 2, ichLast - ich - 2);
            string? sAlias = LookupAliasFromTableToAliasMap(sTable, Aliases);

            if (sAlias == null)
                throw new SqlExceptionMalformedQuery($"table {sTable} has no alias registered");

            query = query.Replace("$$#" + sTable + "$$", $"{sTable} {sAlias}");
            query = query.Replace("$$" + sTable + "$$", sAlias);
        }

        return query;
    }
}
