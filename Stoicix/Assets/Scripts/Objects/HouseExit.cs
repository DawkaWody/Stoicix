using UnityEngine;
using UnityEngine.InputSystem;

public class HouseExit : MonoBehaviour
{
    [SerializeField] private string mainSceneName = "MainLevel";

    private PlayerInputHandler _playerInput;
    private bool _playerInTrigger = false;

    void Update()
    {
        if (_playerInTrigger && _playerInput != null && _playerInput.InteractWasPressed)
        {
            GameManager.Instance.ExitHouse(mainSceneName);
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
