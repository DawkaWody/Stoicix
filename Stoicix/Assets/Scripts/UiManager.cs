using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    [SerializeField] private TMP_Text _gemCountText;
    [SerializeField] private Image _winScreen;
    [SerializeField] private float _winScreenDuration;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void UpdateGemCount(int gemCount)
    {
        _gemCountText.text = gemCount.ToString();
    }

    public void ShowWinScreen()
    {
        StartCoroutine(ShowWinScreenCo());
    }

    private IEnumerator ShowWinScreenCo()
    {
        _winScreen.gameObject.SetActive(true);
        yield return new WaitForSeconds(_winScreenDuration);
        _winScreen.gameObject.SetActive(false);
    }
}
