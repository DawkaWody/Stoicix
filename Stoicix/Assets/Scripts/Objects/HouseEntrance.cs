using UnityEngine;
using UnityEngine.InputSystem;

public class HouseEntrance : MonoBehaviour
{
    [SerializeField] private string houseSceneName = "Minigame1";
    [SerializeField] private Transform returnPositionOutside;

    private PlayerInputHandler _playerInput;
    private bool _playerInTrigger = false;

    void Update()
    {
        if (_playerInTrigger && _playerInput != null && _playerInput.InteractWasPressed)
        {
            GameManager.Instance.EnterHouse(houseSceneName, returnPositionOutside.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInTrigger = true;
            _playerInput = other.GetComponent<PlayerInputHandler>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInTrigger = false;
            _playerInput = null;
        }
    }
}
