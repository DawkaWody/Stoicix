using UnityEngine;
using TMPro;

public class Monolog : MonoBehaviour
{
    [SerializeField] private string[] _texts;
    [SerializeField] private GameObject _textGo;
    [SerializeField] private GameObject _bubble;

    private int _currentTextIndex;

    private TMP_Text _textBox;
    private TypeWriterEffect _typeWriterEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        _textBox = _textGo.GetComponent<TMP_Text>();
        _typeWriterEffect = _textGo.GetComponent<TypeWriterEffect>();
        _bubble.SetActive(false);
        _currentTextIndex = 0;
    }

    public bool Next()
    {
        if (_typeWriterEffect.IsRunning())
        {
            _typeWriterEffect.Skip();
            return true;
        }
        _typeWriterEffect.Clear();
        if (_currentTextIndex >= _texts.Length)
        {
            Close();
            return false;
        }
        _bubble.SetActive(true);
        _textBox.text = _texts[_currentTextIndex];
        _typeWriterEffect.Trigger();
        _currentTextIndex++;
        return true;
    }

    public void OneShot(string text)
    {
        _bubble.SetActive(true);
        _textBox.text = text;
        _typeWriterEffect.Trigger();
    }

    public void Close()
    {
        _bubble.SetActive(false);
        _currentTextIndex = 0;
        _typeWriterEffect.Clear();
    }

    public TypeWriterEffect GetTypeWriter()
    {
        return _typeWriterEffect;
    }
}
