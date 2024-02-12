namespace TCore.Sql.Core;

public interface ISqlTransaction
{
    public void Rollback();
    public void Commit();
    public void Dispose();
}
