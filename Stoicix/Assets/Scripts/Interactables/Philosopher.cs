using UnityEngine;

public class Philosopher : MonoBehaviour, IInteractable
{
    [SerializeField] private int _id;
    [SerializeField] private Monolog _monolog;
    [SerializeField] private Monolog _questCompleteMonolog;
    [SerializeField] private bool _usesMinigameScene;
    [SerializeField] private int _minigameScene;

    public int InteractPriority { get; set; } = 3;

    public void Interact(IInteractable.EmptyCallback OnInteractionEnd)
    {
        if(GameManager.Instance.CurrentQuestGiverID == _id && GameManager.Instance.JustCompletedQuest)
        {
            if (_questCompleteMonolog.Next()) return;

            GameManager.Instance.FinishQuest(_id);
            OnInteractionEnd?.Invoke();
            return;
        }

        if(GameManager.Instance.CompletedQuests.Contains(_id))
        {
            OnInteractionEnd?.Invoke();
            return;
        }

        if (_monolog.Next()) return;

        GameManager.Instance.StartQuest(_id);
        OnInteractionEnd?.Invoke();
        
        if(_usesMinigameScene)
        {
            GameManager.Instance.LoadMinigame(_minigameScene);
        }
    }
}
