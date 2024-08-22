using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    public static SaveDataV1 SaveDataV1 = new SaveDataV1();

    public static readonly int SaveVersion = 1;

    public static readonly string SaveName = "AutoSave1.json";

    public static bool isSaveFile = false;

    private void Awake()
    {
        LoadGame();
    }

    private void Start()
    {

    }

    private string SaveDirectory
    {
        get { return $"{Application.persistentDataPath}/Save"; }
    }

    public void SaveGame(SaveDataV1 saveData)
    {
        if (!System.IO.Directory.Exists(SaveDirectory))
        {
            System.IO.Directory.CreateDirectory(SaveDirectory);
        }

        var path = Path.Combine(SaveDirectory, SaveName);

        using (var writer = new JsonTextWriter(new StreamWriter(path)))
        {
            var serializer = new JsonSerializer();

            serializer.Converters.Add(new ItemDataJsonConverter());

            serializer.Formatting = Formatting.Indented;
            serializer.TypeNameHandling = TypeNameHandling.All;
            serializer.Serialize(writer, saveData);
        }

        isSaveFile = true;
    }

    public SaveDatas LoadGame()
    {
        var path = Path.Combine(SaveDirectory, SaveName);

        if (!System.IO.File.Exists(Path.Combine(SaveDirectory, SaveName)))
        {
            isSaveFile = false;
            return null;
        }

        using (var writer = new JsonTextReader(new StreamReader(path)))
        {
            var serializer = new JsonSerializer();

            serializer.Converters.Add(new ItemDataJsonConverter());
            serializer.TypeNameHandling = TypeNameHandling.All;

            SaveDatas saveData = serializer.Deserialize<SaveDatas>(writer);

            while (saveData.Version < SaveVersion)
            {
                saveData = saveData.VersionUp();
            }

            SaveDataV1 = saveData as SaveDataV1;
            isSaveFile = true;

            return saveData;
        }


    }
}
