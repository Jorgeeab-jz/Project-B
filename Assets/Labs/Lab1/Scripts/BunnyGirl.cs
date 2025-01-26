using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BunnyGirl : MonoBehaviour
{
    [SerializeField] private String[] _phrases;
    [SerializeField] private TextMeshProUGUI _textDisplay;

    [SerializeField] private StarGrabChannel _resetChannel;

    private void Awake() {
        _resetChannel.onInteraction += ChangePhrase;
    }

    private void ChangePhrase() 
    {
        int index = UnityEngine.Random.Range(0, _phrases.Length);

        _textDisplay.text = _phrases[index];
    }

}
