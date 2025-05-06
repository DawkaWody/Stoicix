using UnityEngine;

public class SortingOrderHandling : MonoBehaviour
{
    /*[SerializeField] private SpriteRenderer[] _sprites;
    [SerializeField] private float _yOffset = 0.5f;

    private Vector2 _checkPosition;
    private Transform _player;
    private int _playerSortingOrder;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _checkPosition = _sprites[^1].transform.position;
        _playerSortingOrder = _player.GetComponent<SpriteRenderer>().sortingOrder;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_player == null) return;

        if (_player.position.y > _checkPosition.y + _yOffset)
        {
            foreach (SpriteRenderer colPart in _sprites) colPart.sortingOrder = _playerSortingOrder + 1;
        }
        else
        {
            foreach (SpriteRenderer colPart in _sprites) colPart.sortingOrder = _playerSortingOrder - 1;
        }
    }*/
    [SerializeField] private SpriteRenderer[] _sprites;
    [SerializeField] private int sortingOffset = 1000; // to round to int
    [SerializeField] private float referenceYOffset = 0f;

    public int sortord;
    void LateUpdate()
    {
        if (_sprites == null || _sprites.Length == 0) return;

        float baseY = transform.position.y + referenceYOffset;
        int sortOrder = Mathf.RoundToInt(-baseY * sortingOffset);
        //sortOrder /= sortingOffset;

        foreach (var sprite in _sprites) sprite.sortingOrder = sortOrder;
        sortord = sortOrder;
    }
}
