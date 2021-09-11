using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class GameArchive
{
    public int bestScore;
    private static string archivePath = Application.persistentDataPath + "/bestscore.save";
    public GameArchive(int bestScore) => this.bestScore = bestScore;

    public static void saveGameArchive(GameArchive ga)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(archivePath);
        bf.Serialize(file, ga);
        file.Close();
    }
    public static GameArchive loadGameArchive()
    {
        if (!File.Exists(archivePath)) 
            return null;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(archivePath, FileMode.Open);
        GameArchive ga = (GameArchive)bf.Deserialize(file);
        file.Close();
        return ga;
    }
}
