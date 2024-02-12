namespace SqlCore;

public class SqlText
{
    /*----------------------------------------------------------------------------
        %%Function: Sqlify
        %%Qualified: TCore.Sql.Sqlify

        consider using command parameters
    ----------------------------------------------------------------------------*/
    public static string Sqlify(string s)
    {
        return s.Replace("'", "''");
    }

    public static string SqlifyQuoted(string s)
    {
        return $"'{s.Replace("'", "''")}'";
    }

    /*----------------------------------------------------------------------------
        %%Function: Nullable
        %%Qualified: TCore.Sql.Nullable
    ----------------------------------------------------------------------------*/
    public static string Nullable(string? s)
    {
        if (s == null)
            return "null";
        else
            return $"'{s}'";
    }

    /*----------------------------------------------------------------------------
        %%Function: Nullable
        %%Qualified: TCore.Sql.Nullable
    ----------------------------------------------------------------------------*/
    public static string Nullable(int? n)
    {
        if (n == null)
            return "null";
        else
            return $"{n.Value}";
    }

    /*----------------------------------------------------------------------------
        %%Function: Nullable
        %%Qualified: TCore.Sql.Nullable
    ----------------------------------------------------------------------------*/
    public static string Nullable(bool? f)
    {
        if (f == null)
            return "null";
        else
            return f.Value ? "1" : "0";
    }

    /*----------------------------------------------------------------------------
        %%Function: Nullable
        %%Qualified: TCore.Sql.Nullable
    ----------------------------------------------------------------------------*/
    public static string Nullable(DateTime? dttm)
    {
        if (dttm == null)
            return "null";
        else
            return
                $"'{dttm.Value.Year:D4}-{dttm.Value.Month:D2}-{dttm.Value.Day:D2}T{dttm.Value.Hour:D2}:{dttm.Value.Minute:D2}:{dttm.Value.Second:D2}.{dttm.Value.Millisecond:D3}'";
    }

    /*----------------------------------------------------------------------------
        %%Function: Nullable
        %%Qualified: TCore.Sql.Nullable
    ----------------------------------------------------------------------------*/
    public static string Nullable(Guid? guid)
    {
        if (guid == null)
            return "null";
        else
            return $"'{guid.Value.ToString()}'";
    }
}
