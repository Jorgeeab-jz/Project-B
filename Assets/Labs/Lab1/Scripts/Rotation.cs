using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Rotation : MonoBehaviour
{
    private void Start() {
        transform.DORotate(new Vector3(0,0,360f), 2f, RotateMode.Fast).SetLoops(-1).SetEase(Ease.Linear);
    }
}
