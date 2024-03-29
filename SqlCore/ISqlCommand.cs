﻿namespace TCore.SqlCore;

public delegate void AddParameterWithValueDelegate(string parameter, object value);
public delegate void CustomizeCommandDelegate(ISqlCommand command);

public interface ISqlCommand
{
    public string CommandText { get; set; }
    public int CommandTimeout { get; set; }
    public ISqlTransaction? Transaction { get; set; }
    public ISqlReader ExecuteReader();
    public int ExecuteNonQuery();
    public object ExecuteScalar();
    public void AddParameterWithValue(string parameterName, object? value);
    public void Close();
}