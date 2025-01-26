using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SctarGrabCannel", menuName = "Scriptables/Channels/Star Grab")]
public class StarGrabChannel : ScriptableObject
{
    public delegate void OnInteraction();

    public OnInteraction onInteraction;
}
