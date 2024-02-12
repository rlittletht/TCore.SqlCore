using System.Collections.Generic;
using System;
using TCore;

namespace TCore.Sql.Core;

public interface ISql
{

    public bool InTransaction { get; }

    public ISqlTransaction? Transaction { get; }

    public ISqlCommand CreateCommand();

    public void ExecuteNonQuery(
        string commandText,
        CustomizeCommandDelegate? customizeParams = null,
        TableAliases? aliases = null);

    public void ExecuteNonQuery(
        SqlCommandTextInit commandText,
        CustomizeCommandDelegate? customizeParams = null);

    public T DoGenericQueryDelegateRead<T>(
        Guid crids,
        string query,
        ISqlReader.DelegateReader<T> delegateReader,
        TableAliases? aliases = null,
        CustomizeCommandDelegate? customizeDelegate = null) where T : new();

    public T DoGenericMultiSetQueryDelegateRead<T>(
        Guid crids,
        string sQuery,
        ISqlReader.DelegateMultiSetReader<T> delegateReader,
        CustomizeCommandDelegate? customizeDelegate = null) where T : new();

    public string SExecuteScalar(SqlCommandTextInit cmdText);
    public int NExecuteScalar(SqlCommandTextInit cmdText);
    public DateTime DttmExecuteScalar(SqlCommandTextInit cmdText);

    public void BeginExclusiveTransaction();
    public void BeginTransaction();
    public void Rollback();
    public void Commit();
    public void Close();
}
