using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting;

public class ClickChallenge : MonoBehaviour
{
    public float challengeTime = 5f;
    public int clicksNeeded = 40; // 8 CPS przez 5s = 40 kliknięć
    private int clickCount = 0;
    private float timeLeft;
    private bool started = false;

    public Text clicksText;
    public Text timerText;
    public Animator playerAnimator;

    void Start()
    {
        Debug.Log("ARGHH!"); 
    }
    void Update()
    {
        if (!started && Mouse.current.leftButton.wasPressedThisFrame) // lewy przycisk
        {
            started = true;
            timeLeft = challengeTime;
            // playerAnimator.SetTrigger("mid_air");
            Debug.Log("START!"); 
        }

        if (started)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                clickCount++;
                // clicksText.text = "Kliknięcia: " + clickCount;
                Debug.Log(clickCount); 
            }

            timeLeft -= Time.deltaTime;
            Debug.Log(timeLeft); 
            // timerText.text = "Czas: " + timeLeft.ToString("F2");

            if (timeLeft <= 0f)
            {
                EndChallenge();
            }
        }
    }

    void EndChallenge()
    {
        started = false;

        if (clickCount >= clicksNeeded)
        {
            playerAnimator.SetTrigger("Win"); // animacja podniesienia sztangi
            Invoke("ReturnToMap", 2f); // wróć po 2 sekundach
        }
        else
        {
            playerAnimator.SetTrigger("Lose");
            Invoke("ReturnToMap", 2f);
        }
    }

    void ReturnToMap()
    {
        SceneManager.LoadScene("MapScene"); // wraca na główną mapę
    }
}
