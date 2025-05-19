using UnityEngine;

public class HouseEntrance : MonoBehaviour
{
    [SerializeField] private string houseSceneName = "Minigame1";
    [SerializeField] private Transform returnPositionOutside;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.EnterHouse(houseSceneName, returnPositionOutside.position);
        }
    }
}
