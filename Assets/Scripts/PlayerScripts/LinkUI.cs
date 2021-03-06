﻿using UnityEngine;

public class LinkUI : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private KeySystem _keySystem = default;
    [SerializeField] private HeartSystem _heartSystem = default;
    [SerializeField] private RupeeSystem _rupeeSystem = default;
    [Header("Prefabs")]
    [SerializeField] private GameObject _pfbOpenPrompt = default;
    [Header("Canvas")]
    [SerializeField] private GameObject _inGameCanvas = default;
    private GameObject _player;
    private GameObject _currentlyShownPrompt;
    private Vector2 _promptPosition;


    public KeySystem KeySystem { get { return _keySystem; } private set { _keySystem = value; } }
    public HeartSystem HeartSystem { get { return _heartSystem; } private set { _heartSystem = value; } }
    public RupeeSystem RupeeSystem { get { return _rupeeSystem; } private set { _rupeeSystem = value; } }

    void Update()
    {
        if (_currentlyShownPrompt != null)
        {
            _promptPosition = Camera.main.WorldToScreenPoint(new Vector2(_player.transform.position.x + 1.0f, _player.transform.position.y - 0.5f));
            _currentlyShownPrompt.transform.position = _promptPosition;
        }
    }

    public void ShowPrompt(GameObject player)
    {
        if (_currentlyShownPrompt == null)
        {
            _player = player;
            _promptPosition = Camera.main.WorldToScreenPoint(new Vector2(_player.transform.position.x + 1.0f, _player.transform.position.y - 0.5f));
            _currentlyShownPrompt = Instantiate(_pfbOpenPrompt, _promptPosition, Quaternion.identity);
            _currentlyShownPrompt.transform.SetParent(_inGameCanvas.transform, false);
            _currentlyShownPrompt.transform.position = _promptPosition;
        }
    }

    public void HidePrompt()
    {
        Destroy(_currentlyShownPrompt);
    }
}
