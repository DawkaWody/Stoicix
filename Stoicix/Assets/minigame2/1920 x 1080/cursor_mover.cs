using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEditor;
using UnityEngine.SceneManagement;

public class ZenonMinigame : MonoBehaviour
{
    public float dragStrength = 10f;  // Siła, z jaką "burza" odciąga kursor
    public float maxTime = 30f;  // Czas trwania minigry w sekundach
    public float distance = 100f; 
    private float timer;
    private Vector2 screenCenter;
    private bool isGameActive;
    private Vector2 currentMousePosition;
    public TextMeshProUGUI textMeshPro;

    void Start()
    {
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        timer = maxTime;
        isGameActive = true;

                // Zablokuj kursor w obrębie ekranu i spraw, by był niewidoczny:
        Cursor.lockState = CursorLockMode.Confined;  // Ogranicza kursor do okna gry

    }

    void Update()
    {
        Cursor.visible = true; // idk why it's switching it off
        if (isGameActive)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                EndGame(true);
            }
            // getting cursor position
            currentMousePosition = Mouse.current.position.ReadValue();
            Vector2 direction = screenCenter - currentMousePosition; 

            // moving cursor
            float xDrag = Random.Range(-dragStrength, dragStrength);
            float yDrag = Random.Range(-dragStrength, dragStrength);

            currentMousePosition += direction * Time.deltaTime * dragStrength + new Vector2(xDrag, yDrag);

            // currentMousePosition.x = Mathf.Clamp(currentMousePosition.x, 0, Screen.width);
            // currentMousePosition.y = Mathf.Clamp(currentMousePosition.y, 0, Screen.height);

            // setting position
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);

            Debug.Log(timer + " " + Vector2.Distance(currentMousePosition, screenCenter)); 

            // if it's near center - continue, else restart timer
            if (Vector2.Distance(currentMousePosition, screenCenter) > distance)
            {
                timer = maxTime; 
            }
            if (timer <= 0)
            {
                EndGame(true);
            }
        }
    }

    void EndGame(bool success)
    {
        isGameActive = false;

        if (success)
        {
            textMeshPro.text = "You have a lot of luck, you won"; 
            Invoke("chengeScene", 3f); 
        }
        else
        {
            Debug.Log("You are so bad");
        }
    }

    void changeScene(){
        SceneManager.LoadScene("MainLevel"); 
    }
}
