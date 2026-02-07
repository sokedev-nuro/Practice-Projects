using Godot;
using System;
using System.Collections.Generic;

public sealed class PlayerSaveData
{
    public int Level { get; set; }
    public int Gold { get; set; }
    public List<string> Inventory { get; set; } = new();
}

public sealed class SaveContainer<T>
{
    public int Version { get; set; } = 1;
    public long SavedAtUnix { get; set; }
    public T Data { get; set; } = default;
}
