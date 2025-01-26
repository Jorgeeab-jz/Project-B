using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StarsManager : MonoBehaviour
{
    [SerializeField] private Transform[] _possiblePositions;
    [SerializeField] private Transform[] _enemiesPositions;
    [SerializeField] private StarGrabChannel _grabChannel;
    [SerializeField] private TextMeshProUGUI _ammountDisplay;

    [SerializeField] private GameObject _starPrefab;
    [SerializeField] private GameObject _enemyPrefab;

    private int _starsAmmount = 0;

    private bool _isStart = true;

    private void Awake()
    {

        _ammountDisplay.text = "0";
        _grabChannel.onInteraction += SpawnStar;

        Instantiate(_starPrefab, _possiblePositions[0]);

    }

    private void Start() {
        InvokeRepeating("SpawnEnemy", 3f, 10f);
    }

    public void SpawnStar()
    {
        int index = UnityEngine.Random.Range(0, _possiblePositions.Length);

        Debug.Log($"[StarManager]: Position {index}");

        UpdateStars();

        Instantiate(_starPrefab, _possiblePositions[index]);

    }

    private void SpawnEnemy() 
    {
        int index = UnityEngine.Random.Range(0, _enemiesPositions.Length);

        Instantiate(_enemyPrefab, _enemiesPositions[index]);
    }

    private void UpdateStars()
    {
        _starsAmmount++;
        _ammountDisplay.text = _starsAmmount.ToString();
    }
}
