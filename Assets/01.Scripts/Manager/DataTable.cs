using System;

public abstract class DataTable
{
    public abstract void Load(string path, Action tableLoaded);
}
