using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections;

public class cursorMover : MonoBehaviour
{
    [SerializeField] private float dragStrength = 10f;  // Siła, z jaką "burza" odciąga kursor
    [SerializeField] private float timeToWin = 30f;  // Czas trwania minigry w sekundach
    [SerializeField] private float timeToDie = 5f;
    [SerializeField] private float distance = 100f;

    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private RectTransform cursor;
    [SerializeField] private RectTransform center;

    private float deathTimer;
    private float winTimer;
    private bool isGameActive;
    private Vector2 screenCenter;
    private Vector2 simulatedPosition;

    void Start()
    {
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        simulatedPosition = screenCenter;

        center.position = screenCenter;
        center.sizeDelta = new Vector2(distance * 2f, distance * 2f);

        deathTimer = timeToDie;
        winTimer = 0f;

                // Zablokuj kursor w obrębie ekranu i spraw, by był niewidoczny:
        Cursor.lockState = CursorLockMode.Confined;  // Ogranicza kursor do okna gry
        Cursor.visible = false;

        if (cursor != null)
        {
            cursor.position = simulatedPosition;
        }
        isGameActive = true;
    }

    void Update()
    {
        if (!isGameActive) return;

        Vector2 realMousePos = Mouse.current.position.ReadValue();

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();// cool delta thingie
        simulatedPosition += mouseDelta;

        Vector2 direction = (simulatedPosition - screenCenter).normalized;

        // moving cursor
        float xDrag = Random.Range(-dragStrength, dragStrength);
        float yDrag = Random.Range(-dragStrength, dragStrength);

        Vector2 offset = new Vector2(xDrag, yDrag);
        Vector2 drag = (direction + offset).normalized * dragStrength * Time.deltaTime;

        simulatedPosition += drag;

        // clamping
        simulatedPosition.x = Mathf.Clamp(simulatedPosition.x, 0, Screen.width);
        simulatedPosition.y = Mathf.Clamp(simulatedPosition.y, 0, Screen.height);

        cursor.position = simulatedPosition;

        float dist = Vector2.Distance(cursor.position, screenCenter);

        // if it's near center - continue, else restart timer
        if (dist <= distance)
        {
            winTimer += Time.deltaTime;
            deathTimer = timeToDie;

            if (winTimer >= timeToWin)
            {
                EndGame(true);
            }
        }
        else
        {
            deathTimer -= Time.deltaTime;
            winTimer = 0f;

            if (deathTimer <= 0)
            {
                EndGame(false);
            }
        }
    }

    void EndGame(bool success)
    {
        isGameActive = false;

        if (success)
        {
            textMeshPro.text = "You have a lot of luck, you won";
            StartCoroutine(LoadWithDelay(3f));
        }
        else
        {
            Debug.Log("You are so bad");
        }
    }

    private IEnumerator LoadWithDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GameManager.Instance.LoadMainLevel();
    }
}
