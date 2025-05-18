using UnityEngine;

public interface IMinigame
{
    void StartGame(IInteractable.EmptyCallback onGameEnd);
}
