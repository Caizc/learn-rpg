using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    public int currentLevel { get; private set; }
    public int maxLevel { get; private set; }

    private NetworkService _network;

    public void Startup(NetworkService service)
    {
        Debug.Log("Mission manager starting...");

        _network = service;

        UpdateData(0, 3);

        status = ManagerStatus.Started;
    }

    public void GoToNextLevel()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
            string name = "Level" + currentLevel;
            Debug.Log("Loading " + name);
            SceneManager.LoadScene(name);
        }
        else
        {
            Debug.Log("Last level");
            Messenger.Broadcast(GameEvent.GAME_COMPLETE);
        }
    }

    public void ReachObjective()
    {
        Messenger.Broadcast(GameEvent.LEVEL_COMPLETE);
    }

    public void RestartCurrent()
    {
        string name = "Level" + currentLevel;
        Debug.Log("Loading " + name);
        SceneManager.LoadScene(name);
    }

    public void UpdateData(int currentLevel, int maxLevel)
    {
        this.currentLevel = currentLevel;
        this.maxLevel = maxLevel;
    }
}
