using UnityEngine;

public class HouseExit : MonoBehaviour
{
    [SerializeField] private string mainSceneName = "MainLevel";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.ExitHouse(mainSceneName);
        }
    }
}
