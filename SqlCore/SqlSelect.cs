using System;
using System.Collections.Generic;
using System.Text;
using TCore.Sql.Core;

namespace TCore.Sql.SqlClient;

public class SqlSelect
{
    readonly List<SqlInnerJoin> m_innerJoins = new();
    private string? m_base;
    private readonly SqlWhere m_sw;
    private string? m_sOrderBy;
    private string? m_sGroupBy;
    private string? m_sAs;

    public override string ToString()
    {
        string sBase = m_base ?? "";

        StringBuilder sb = new(256);

        // if there is an AS suffix, then entire select must be in a parens
        if (m_sAs != null)
            sb.Append("(");

        sb.Append(m_sw.Aliases.ExpandAliases(sBase));
        foreach (SqlInnerJoin innerJoin in m_innerJoins)
        {
            sb.Append(" ");
            sb.Append(m_sw.Aliases.ExpandAliases(innerJoin.ToString()));
        }

        string sBaseForWhere = sb.ToString();

        sb = new StringBuilder(256);

        sb.Append(m_sw.GetWhere(sBaseForWhere));
        if (m_sGroupBy != null)
        {
            sb.Append(" GROUP BY ");
            sb.Append(m_sw.Aliases.ExpandAliases(m_sGroupBy));
        }

        if (m_sOrderBy != null)
        {
            sb.Append(" ORDER BY ");
            sb.Append(m_sw.Aliases.ExpandAliases(m_sOrderBy));
        }

        if (m_sAs != null)
        {
            sb.Append(") AS ");
            sb.Append(m_sAs);
        }

        return sb.ToString();
    }

    public SqlSelect(string @base)
    {
        m_base = @base;
        m_sw = new SqlWhere();
    }

    public SqlSelect(SqlCommandTextInit cmdText)
    {
        m_base = cmdText.CommandText;
        m_sw = new SqlWhere();

        if (cmdText.Aliases != null)
            m_sw.AddAliases(cmdText.Aliases);
    }

    public SqlSelect(string @base, Dictionary<string, string> mpAliases)
    {
        m_base = @base;
        m_sw = new SqlWhere();
        m_sw.AddAliases(mpAliases);
    }

    public SqlSelect()
    {
        m_sw = new SqlWhere();
    }

    public void AddBase(string sBase)
    {
        m_base = sBase;
    }

    public void AddAs(string sAs)
    {
        m_sAs = sAs;
    }
    public void AddAliases(TableAliases aliases)
    {
        m_sw.AddAliases(aliases);
    }

    public void AddOrderBy(string sOrderBy)
    {
        m_sOrderBy = sOrderBy;
    }

    public SqlWhere Where
    {
        get { return m_sw; }
    }

    public void AddInnerJoin(SqlInnerJoin ij)
    {
        m_innerJoins.Add(ij);
    }


    /* A D D  G R O U P  B Y */
    /*----------------------------------------------------------------------------
        %%Function: AddGroupBy

        add a group by clause, expanding aliases.			
    ----------------------------------------------------------------------------*/
    public void AddGroupBy(string s)
    {
        s = m_sw.Aliases.ExpandAliases(s);

        m_sGroupBy = s;
    }

}