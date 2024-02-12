namespace TCore.SqlCore;

public interface ISqlTransaction
{
    public void Rollback();
    public void Commit();
    public void Dispose();
}
