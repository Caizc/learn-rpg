using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    private NetworkService _network;

    private string _filename;

    public void Startup(NetworkService service)
    {
        Debug.Log("Data manager starting...");

        _network = service;

        // Application.persistentDataPath: ~/Library/Application Support/company name/product name
        _filename = Path.Combine(Application.persistentDataPath, "LearnRPG.dat");

        status = ManagerStatus.Started;
    }

    public void SaveGameState()
    {
        Debug.Log("Saving game data...");
        Dictionary<string, object> gameState = new Dictionary<string, object>();
        gameState.Add("inventory", Managers.Inventory.GetData());
        gameState.Add("health", Managers.Player.health);
        gameState.Add("maxHealth", Managers.Player.maxHealth);
        gameState.Add("currentLevel", Managers.Mission.currentLevel);
        gameState.Add("maxLevel", Managers.Mission.maxLevel);

        FileStream stream = File.Create(_filename);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, gameState);
        stream.Close();
        Debug.Log("Game data have been saved!");
    }

    public void LoadGameState()
    {
        if (!File.Exists(_filename))
        {
            Debug.Log("No saved game!");
            return;
        }
        else
        {
            Debug.Log("Loading game data...");
            Dictionary<string, object> gameState;

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = File.Open(_filename, FileMode.Open);
            gameState = formatter.Deserialize(stream) as Dictionary<string, object>;
            stream.Close();

            Managers.Inventory.UpdateData((Dictionary<string, int>)gameState["inventory"]);
            Managers.Player.UpdateData((int)gameState["health"], (int)gameState["maxHealth"]);
            Managers.Mission.UpdateData((int)gameState["currentLevel"], (int)gameState["maxLevel"]);

            Managers.Mission.RestartCurrent();
            Debug.Log("Game data have been loaded!");
        }
    }
}