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
    [SerializeField] private Transform cursor;
    [SerializeField] private RectTransform center;

    private float deathTimer;
    private float winTimer;
    private bool isGameActive;
    private Vector2 screenCenter;
    private Vector2 simulatedPosition;
    private Vector2 lastMousePosition;

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

    private float zOffset;

    [Header("Simulation Flags")]
    [SerializeField] private bool simulateWebGL = false;

    void Start()
    {
        screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        zOffset = Mathf.Abs(Camera.main.transform.position.z - cursor.transform.position.z);

        Vector3 centerWorld = Camera.main.ScreenToWorldPoint(new Vector3(screenCenter.x, screenCenter.y, zOffset));
        centerWorld.z = 0f;
        center.position = centerWorld;

        StartGame();
    }

    void Update()
    {
        if (!isGameActive) return;

        Vector2 mouseDelta = Vector2.zero;
        if (simulateWebGL || Application.platform == RuntimePlatform.WebGLPlayer)
        {
            // WebGL: Use Input.mousePosition and calculate the delta
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            mouseDelta = mousePosition - lastMousePosition;
            lastMousePosition = mousePosition;
        }
        else if (!simulateWebGL || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            // Windows (and other desktop platforms): Use the new Input System
            mouseDelta = Mouse.current.delta.ReadValue();
            lastMousePosition = Mouse.current.position.ReadValue();
        }

        simulatedPosition += mouseDelta;

        Vector2 direction = (simulatedPosition - screenCenter).normalized;

        // moving cursor
        Vector2 windForce = direction * dragStrength;

        float randomnessStrength = dragStrength * 0.5f; //x % of dragStrength ; tweak this
        Vector2 randomness = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * randomnessStrength;

        Vector2 totalForce = (windForce + randomness) * Time.deltaTime;

        simulatedPosition += totalForce;

        // clamping
        simulatedPosition.x = Mathf.Clamp(simulatedPosition.x, 0, Screen.width);
        simulatedPosition.y = Mathf.Clamp(simulatedPosition.y, 0, Screen.height);

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(simulatedPosition);
        worldPos.z = 0f;
        cursor.position = worldPos;

        float dist = Vector2.Distance(simulatedPosition, screenCenter);

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

        if (simulateWebGL || Application.platform == RuntimePlatform.WebGLPlayer)
        {
            // WebGL: Don't lock the cursor, just hide it
            Cursor.visible = false;
        }
        else if (!simulateWebGL || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            // Windows and other desktop platforms: Lock and hide the cursor
            Cursor.lockState = CursorLockMode.Confined;  // Lock the cursor within the game window
            Cursor.visible = false;
        }

        if (cursor != null)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(simulatedPosition);
            worldPos.z = 0f;
            cursor.position = worldPos;
        }
        textMeshPro.text = "Utrzymaj kursor na środku ekranu";
        isGameActive = true;
    }

    void EndGame(bool success)
    {
        isGameActive = false;

        if (success)
        {
            StartCoroutine(AfterGameEnd(3f,success));
        }
        else
        {
            textMeshPro.text = "Nie przetrwałeś burzy. Spróbuj ponownie.";
            StartCoroutine(AfterGameEnd(3f, success));
        }
    }

    private IEnumerator AfterGameEnd(float seconds, bool success)
    {
        if (success) UiManager.Instance.ShowWinScreen();
        yield return new WaitForSeconds(seconds);
        if (success) GameManager.Instance.CompleteQuest();
        else StartGame();
    }
}
