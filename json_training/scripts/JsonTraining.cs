using Godot;
using System.Collections.Generic;

public partial class JsonTraining : Node
{
    private const string SaveSlot = "slot_1";
    private const string Password = "qwerty123";

    public override void _Ready()
    {
        GD.Print("Creating demo save data...");

        var data = new PlayerSaveData
        {
            Level = 5,
            Gold = 999,
            Inventory = new List<string> { "Sword", "Shield", "Potion"}
        };

        var saveErr = SaveManager.Save(SaveSlot, data, Password);
        GD.Print($"Save result: {saveErr}");

        var loadedOK = SaveManager.TryLoad<PlayerSaveData>(SaveSlot, Password, out var container, out var loadErr);

        if (!loadedOK || container == null)
        {
            GD.PrintErr($"Load failed: {loadErr}");
            return;
        }

        GD.Print($"Load OK. Version={container.Version}, SavedAt={container.SavedAtUnix}");
        var loaded = container.Data;
        GD.Print($"Player: Level={loaded.Level}, Gold={loaded.Gold}, " + 
        $"Inventory={string.Join(", ", loaded.Inventory)}");
    }
}
