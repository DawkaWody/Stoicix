using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Rendering.Universal;

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

    [Header("Light")]
    [SerializeField] private Light2D cursorLight;
    [SerializeField] private float maxLightIntensity;
    [SerializeField] private float minLightIntensity;
    [SerializeField] private float maxDistanceForLight;

    [SerializeField] private Light2D globalLight;
    [SerializeField] private float maxGlobalLightIntensity;
    [SerializeField] private float minGlobalLightIntensity;
    [SerializeField] private float maxDistanceForGlobalLight;

    [SerializeField] private float howIntense;



    void Start()
    {
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        center.position = screenCenter;
        center.sizeDelta = new Vector2(distance * 2f, distance * 2f);

        StartGame();
    }

    void Update()
    {
        if (!isGameActive) return;

        Vector2 realMousePos = Mouse.current.position.ReadValue();

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();// cool delta thingie
        simulatedPosition += mouseDelta;

        Vector2 direction = (simulatedPosition - screenCenter).normalized;

        // moving cursor
        Vector2 windForce = direction * dragStrength;

        float randomnessStrength = dragStrength * 0.5f; //x % of dragStrength ; tweak this
        Vector2 randomness = new Vector2(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ) * randomnessStrength;

        Vector2 totalForce = (windForce + randomness) * Time.deltaTime;

        simulatedPosition += totalForce;

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

        //light
        if (cursorLight != null)
        {
            float normalized = Mathf.Clamp01(dist / maxDistanceForLight);

            float intensity = Mathf.Lerp(maxLightIntensity, minLightIntensity, normalized);
            cursorLight.intensity = Mathf.Lerp(cursorLight.intensity, intensity, howIntense * Time.deltaTime);
        }
        if (globalLight != null)
        {
            float normalizedGlobal = Mathf.Clamp01(dist / maxDistanceForGlobalLight);

            float intensity = Mathf.Lerp(maxGlobalLightIntensity, minGlobalLightIntensity, normalizedGlobal);
            globalLight.intensity = Mathf.Lerp(globalLight.intensity, intensity, howIntense * Time.deltaTime);
        }
    }

    void StartGame()
    {
        simulatedPosition = screenCenter;

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

    void EndGame(bool success)
    {
        isGameActive = false;

        if (success)
        {
            textMeshPro.text = "You have a lot of luck, you won";
            StartCoroutine(AfterGameEnd(3f,success));
        }
        else
        {
            Debug.Log("You are so bad");
            StartCoroutine(AfterGameEnd(3f, success));
        }
    }

    private IEnumerator AfterGameEnd(float seconds, bool success)
    {
        yield return new WaitForSeconds(seconds);
        if (success) GameManager.Instance.LoadMainLevel();
        else StartGame();
    }
}
