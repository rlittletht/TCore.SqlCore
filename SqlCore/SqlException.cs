
using System;
using TCore.Exceptions;

namespace TCore.SqlCore;

public class SqlException : TcException
{
    public SqlException() : base(Guid.Empty, "unknown sql excelption") { }
    public SqlException(Guid crids) : base(crids) { }
    public SqlException(string errorMessage) : base(errorMessage) { }
    public SqlException(string errorMessage, Exception innerException) : base(errorMessage, innerException) { }
    public SqlException(Guid crids, string errorMessage) : base(crids, errorMessage) { }
    public SqlException(Guid crids, Exception innerException, string errorMessage) : base(crids, innerException, errorMessage) { }
}

// Exception when the result set did not contain a single row
public class SqlExceptionNotSingleRow : SqlException
{
    public SqlExceptionNotSingleRow() : base(Guid.Empty, "SqlExceptionNotSingleRow") { }
    public SqlExceptionNotSingleRow(Guid crids) : base(crids) { }
    public SqlExceptionNotSingleRow(string errorMessage) : base(errorMessage) { }
    public SqlExceptionNotSingleRow(string errorMessage, Exception innerException) : base(errorMessage, innerException) { }
    public SqlExceptionNotSingleRow(Guid crids, string errorMessage) : base(crids, errorMessage) { }
    public SqlExceptionNotSingleRow(Guid crids, Exception innerException, string errorMessage) : base(crids, innerException, errorMessage) { }
}

public class SqlExceptionNoResults : SqlException
{
    public SqlExceptionNoResults() : base(Guid.Empty, "SqlExceptionNoResults") { }
    public SqlExceptionNoResults(Guid crids) : base(crids) { }
    public SqlExceptionNoResults(string errorMessage) : base(errorMessage) { }
    public SqlExceptionNoResults(string errorMessage, Exception innerException) : base(errorMessage, innerException) { }
    public SqlExceptionNoResults(Guid crids, string errorMessage) : base(crids, errorMessage) { }
    public SqlExceptionNoResults(Guid crids, Exception innerException, string errorMessage) : base(crids, innerException, errorMessage) { }
}

public class SqlExceptionInTransaction : SqlException
{
    public SqlExceptionInTransaction() : base(Guid.Empty, "SqlExceptionInTransaction") { }
    public SqlExceptionInTransaction(Guid crids) : base(crids) { }
    public SqlExceptionInTransaction(string errorMessage) : base(errorMessage) { }
    public SqlExceptionInTransaction(string errorMessage, Exception innerException) : base(errorMessage, innerException) { }
    public SqlExceptionInTransaction(Guid crids, string errorMessage) : base(crids, errorMessage) { }
    public SqlExceptionInTransaction(Guid crids, Exception innerException, string errorMessage) : base(crids, innerException, errorMessage) { }
}

public class SqlExceptionNotInTransaction : SqlException
{
    public SqlExceptionNotInTransaction() : base(Guid.Empty, "SqlExceptionNotInTransaction") { }
    public SqlExceptionNotInTransaction(Guid crids) : base(crids) { }
    public SqlExceptionNotInTransaction(string errorMessage) : base(errorMessage) { }
    public SqlExceptionNotInTransaction(string errorMessage, Exception innerException) : base(errorMessage, innerException) { }
    public SqlExceptionNotInTransaction(Guid crids, string errorMessage) : base(crids, errorMessage) { }
    public SqlExceptionNotInTransaction(Guid crids, Exception innerException, string errorMessage) : base(crids, innerException, errorMessage) { }
}

public class SqlExceptionNoConnection : SqlException
{
    public SqlExceptionNoConnection() : base(Guid.Empty, "SqlExceptionNoConnection") { }
    public SqlExceptionNoConnection(Guid crids) : base(crids) { }
    public SqlExceptionNoConnection(string errorMessage) : base(errorMessage) { }
    public SqlExceptionNoConnection(string errorMessage, Exception innerException) : base(errorMessage, innerException) { }
    public SqlExceptionNoConnection(Guid crids, string errorMessage) : base(crids, errorMessage) { }
    public SqlExceptionNoConnection(Guid crids, Exception innerException, string errorMessage) : base(crids, innerException, errorMessage) { }
}

public class SqlExceptionNotImplementedInThisClient : SqlException
{
    public SqlExceptionNotImplementedInThisClient() : base(Guid.Empty, "SqlExceptionNotImplementedInThisClient") { }
    public SqlExceptionNotImplementedInThisClient(Guid crids) : base(crids) { }
    public SqlExceptionNotImplementedInThisClient(string errorMessage) : base(errorMessage) { }
    public SqlExceptionNotImplementedInThisClient(string errorMessage, Exception innerException) : base(errorMessage, innerException) { }
    public SqlExceptionNotImplementedInThisClient(Guid crids, string errorMessage) : base(crids, errorMessage) { }
    public SqlExceptionNotImplementedInThisClient(Guid crids, Exception innerException, string errorMessage) : base(crids, innerException, errorMessage) { }
}

public class SqlExceptionNoReader : SqlException
{
    public SqlExceptionNoReader() : base(Guid.Empty, "SqlExceptionNoReader") { }
    public SqlExceptionNoReader(Guid crids) : base(crids) { }
    public SqlExceptionNoReader(string errorMessage) : base(errorMessage) { }
    public SqlExceptionNoReader(string errorMessage, Exception innerException) : base(errorMessage, innerException) { }
    public SqlExceptionNoReader(Guid crids, string errorMessage) : base(crids, errorMessage) { }
    public SqlExceptionNoReader(Guid crids, Exception innerException, string errorMessage) : base(crids, innerException, errorMessage) { }
}


public class SqlExceptionMalformedQuery : SqlException
{
    public SqlExceptionMalformedQuery() : base(Guid.Empty, "SqlExceptionMalformedQuery") { }
    public SqlExceptionMalformedQuery(Guid crids) : base(crids) { }
    public SqlExceptionMalformedQuery(string errorMessage) : base(errorMessage) { }
    public SqlExceptionMalformedQuery(string errorMessage, Exception innerException) : base(errorMessage, innerException) { }
    public SqlExceptionMalformedQuery(Guid crids, string errorMessage) : base(crids, errorMessage) { }
    public SqlExceptionMalformedQuery(Guid crids, Exception innerException, string errorMessage) : base(crids, innerException, errorMessage) { }
}

public class SqlExceptionLockTimeout : SqlException
{
    public SqlExceptionLockTimeout() : base(Guid.Empty, "SqlExceptionLockTimeout") { }
    public SqlExceptionLockTimeout(Guid crids) : base(crids) { }
    public SqlExceptionLockTimeout(string errorMessage) : base(errorMessage) { }
    public SqlExceptionLockTimeout(string errorMessage, Exception innerException) : base(errorMessage, innerException) { }
    public SqlExceptionLockTimeout(Guid crids, string errorMessage) : base(crids, errorMessage) { }
    public SqlExceptionLockTimeout(Guid crids, Exception innerException, string errorMessage) : base(crids, innerException, errorMessage) { }
}
