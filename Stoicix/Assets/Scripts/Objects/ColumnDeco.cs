using UnityEngine;

public class ColumnDeco : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] _sprites;
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
    void Update()
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
    }
}
