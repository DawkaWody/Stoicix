using System.Collections;
using UnityEngine;
using TMPro;

public class TypeWriterEffect : MonoBehaviour
{
    [SerializeField] private float _charsPerSecond;
    [SerializeField] private TMP_Text _text;

    private int _charIndex;
    private bool _isRunning;
    private WaitForSeconds _delay;
    private Coroutine _typeWriterCo;

    private TMP_TextInfo _textInfo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!_text) _text = GetComponent<TMP_Text>();
        _textInfo = _text.textInfo;

        _delay = new WaitForSeconds(1f / _charsPerSecond);
    }

    public void Trigger()
    {
        _text.ForceMeshUpdate();
        _text.maxVisibleCharacters = 0;
        _typeWriterCo = StartCoroutine(TypeWriterCo());
    }

    public void Clear()
    {
        _text.maxVisibleCharacters = 0;
        _charIndex = 0;
    }

    public void Skip()
    {
        if (_typeWriterCo != null) StopCoroutine(_typeWriterCo);
        _text.maxVisibleCharacters = _textInfo.characterCount;
        _isRunning = false;
    }

    private IEnumerator TypeWriterCo()
    {
        _isRunning = true;
        while (_charIndex < _textInfo.characterCount)
        {
            _text.maxVisibleCharacters++;

            yield return _delay;

            _charIndex++;
        }
        _isRunning = false;
    }

    public bool IsRunning()
    {
        return _isRunning;
    }
}
