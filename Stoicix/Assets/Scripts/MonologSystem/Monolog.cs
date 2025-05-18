using UnityEngine;
using TMPro;

public class Monolog : MonoBehaviour
{
    [SerializeField] private string[] _texts;
    [SerializeField] private TMP_Text _textBox;
    [SerializeField] private TypeWriterEffect _typeWriterEffect;
    [SerializeField] private GameObject _bubble;

    private int _currentTextIndex;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
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
