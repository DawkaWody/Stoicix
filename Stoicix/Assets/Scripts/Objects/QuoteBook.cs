using System.Collections;
using UnityEngine;

public class QuoteBook : MonoBehaviour, IInteractable
{
    [SerializeField] private string[] _quotes;
    [SerializeField] private Monolog _bookMonolog;
    [SerializeField] private Animator _bookAnimator;

    public int InteractPriority { get; set; } = 2;

    private bool _isOpened;

    public void Interact()
    {
        if (_isOpened)
        {
            if (_bookMonolog.GetTypeWriter().IsRunning())
            {
                _bookMonolog.GetTypeWriter().Skip();
            }
            else
            {
                _bookMonolog.Close();
                _isOpened = false;
            }
        }
        else StartCoroutine(OpenCo());
    }

    private IEnumerator OpenCo()
    {
        _isOpened = true;
        _bookAnimator.SetTrigger("flip");
        yield return new WaitForSeconds(0.1f);
        _bookMonolog.OneShot(_quotes[Random.Range(0, _quotes.Length)]);
    }
}
