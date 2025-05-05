using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Transform _player;
    private Vector3 _playerPosition;

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
        _player = GameObject.FindGameObjectWithTag("Player").transform;
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
        _player.position = _playerPosition;

        SceneManager.sceneLoaded -= LoadMainLevelData;
    }
}
