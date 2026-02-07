using Godot;
using System;
using System.Text.Json;

public static class SaveManager
{
    private const string SaveDirPath = "user://saves";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = false
    };

    private static string SlotToPath(string slot) => $"{SaveDirPath}/{slot}.sav";
    private static string SlotToTempPath(string slot) => $"{SaveDirPath}/{slot}.tmp";

    public static Error Save<T>(string slot, T data, string password, int version = 1)
    {
        if (string.IsNullOrWhiteSpace(slot))
            throw new ArgumentException("Slot name is empty.", nameof(slot));
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("Password is empty.", nameof(password));

        var mkErr = DirAccess.MakeDirRecursiveAbsolute(SaveDirPath);
        if (mkErr != Error.Ok)
            return mkErr;

        var container = new SaveContainer<T>
        {
            Version = version,
            SavedAtUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            Data = data!
        };

        var json = JsonSerializer.Serialize(container, JsonOptions);

        var tempPath = SlotToTempPath(slot);

        using (var file = FileAccess.OpenEncryptedWithPass(tempPath, FileAccess.ModeFlags.Write, password))
        {
            if (file == null)
                return FileAccess.GetOpenError();

            file.StoreString(json);
            file.Flush();
        }

        var finalPath = SlotToPath(slot);

        if (FileAccess.FileExists(finalPath))
        {
            var rmErr = DirAccess.RemoveAbsolute(finalPath);
            if (rmErr != Error.Ok)
                return rmErr;
        }
        return DirAccess.RenameAbsolute(tempPath, finalPath);
    }

    public static bool TryLoad<T>(string slot, string password, out SaveContainer<T>? container, out Error error)
    {
        container = null;
        error = Error.Ok;

        if (string.IsNullOrWhiteSpace(slot))
        {
            error = Error.InvalidParameter;
            return false;
        }

        var path = SlotToPath(slot);

        if (!FileAccess.FileExists(path))
        {
            error = Error.FileNotFound;
            return false;
        }

        string json;

        using (var file = FileAccess.OpenEncryptedWithPass(path, FileAccess.ModeFlags.Read, password))
        {
            if (file == null)
            {
                error = FileAccess.GetOpenError();
                return false;
            }
            json = file.GetAsText();
        }

        try
        {
            container = JsonSerializer.Deserialize<SaveContainer<T>>(json, JsonOptions);
            if (container == null)
            {
                error = Error.ParseError;
                return false;
            }
            return true;
        }
        catch (JsonException)
        {
            error = Error.ParseError;
            return false;
        }
    }
}
