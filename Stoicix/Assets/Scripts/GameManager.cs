using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Transform _player;
    private Vector3 _playerPosition;

    // house for minigame1
    public bool IsReturningFromHouse { get; private set; } = false;
    private Vector3 _houseReturnPosition;

    //for quests
    public int CurrentQuestGiverID { get; private set; } = -1;
    public HashSet<int> CompletedQuests = new HashSet<int>();

    public bool JustCompletedQuest { get; private set; } = false;
    public bool QuestCompletionPending => CurrentQuestGiverID != -1 && !CompletedQuests.Contains(CurrentQuestGiverID);

    public int gemCount { get; private set; } = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void StartQuest(int philosopherID)
    {
        CurrentQuestGiverID = philosopherID;
        JustCompletedQuest = false;
        if (philosopherID == 1) GameObject.FindAnyObjectByType<Bar>().SetInteractable();
    }

    public void CompleteQuest()// for scene changing minigames
    {
        JustCompletedQuest = true;
        LoadMainLevel();
    }

    public void MarkQuestCompletedInScene() // for bar lift
    {
        JustCompletedQuest = true;
    }

    public void FinishQuest(int philosopherID)
    {
        if(!CompletedQuests.Contains(philosopherID))
        {
            CompletedQuests.Add(philosopherID);
            RewardGem();
        }
        CurrentQuestGiverID = -1;
        JustCompletedQuest = false;
    }

    public void RewardGem()
    {
        gemCount++;
        UiManager.Instance.UpdateGemCount(gemCount);
        if (gemCount == 3) GameWon();
    }

    private void GameWon()
    {
        Cursor.visible = true;
        SceneManager.LoadScene(4);
    }

    public void LoadMinigame(int index)
    {
        SaveMainLevelData();
        SceneManager.LoadScene(index);
    }

    public void LoadMainLevel()
    {
        SceneManager.LoadScene(0);
        SceneManager.sceneLoaded += LoadMainLevelData;
    }

    private void SaveMainLevelData()
    {
        _playerPosition = _player.position;
    }

    private void LoadMainLevelData(Scene scene, LoadSceneMode mode)
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            _player = playerObject.transform;
            _player.position = _playerPosition;
        }

        SceneManager.sceneLoaded -= LoadMainLevelData;
    }

    public void EnterHouse(string houseSceneName, Vector3 returnPosition)
    {
        _houseReturnPosition = returnPosition;
        IsReturningFromHouse = true;
        SaveMainLevelData();
        SceneManager.LoadScene(houseSceneName);
    }

    public void ExitHouse(string mainSceneName = "MainLevel")
    {
        SceneManager.LoadScene(mainSceneName);
        SceneManager.sceneLoaded += HandleHouseReturn;
    }

    private void HandleHouseReturn(Scene scene, LoadSceneMode mode)
    {
        if (!IsReturningFromHouse) return;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            _player = playerObj.transform;
            _player.position = _houseReturnPosition;
        }

        IsReturningFromHouse = false;
        SceneManager.sceneLoaded -= HandleHouseReturn;
    }
}
