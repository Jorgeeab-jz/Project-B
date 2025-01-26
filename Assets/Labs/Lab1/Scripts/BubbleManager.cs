using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;

public class BubbleManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputActionReference _rotateSelectionInput;
    [SerializeField] private GameObject[] _bubblesPrefabs;
    [SerializeField] private Sprite _bubbleSprite;
    [SerializeField] private Image _selectionDisplay;
    [SerializeField] private GameObject _selectedBubblePrefab;


    [Space(10)]
    [Header("Values")]
    [SerializeField] private int _bubbleValue1;
    [SerializeField] private int _bubbleValue2;
    [SerializeField] private int _selectedBubble = 0;


    public GameObject SelectedBubble { get { return _selectedBubblePrefab; } }

    private void OnEnable()
    {
        DisplaySelectedBubble(_selectedBubble);

        _rotateSelectionInput.action.performed += RotateSelection;
        _rotateSelectionInput.action.Enable();
    }

    private void OnDisable()
    {
        _rotateSelectionInput.action.performed -= RotateSelection;
        _rotateSelectionInput.action.Disable();
    }




    // Methods
    private void RotateSelection(InputAction.CallbackContext context)
    {
        _selectedBubble++;

        if (_selectedBubble == _bubblesPrefabs.Length) _selectedBubble = 0;


        DisplaySelectedBubble(_selectedBubble);

    }

    private void DisplaySelectedBubble(int index)
    {
        _selectedBubblePrefab = _bubblesPrefabs[_selectedBubble];
        _selectionDisplay.sprite = _bubblesPrefabs[_selectedBubble].GetComponent<Bubble>().Sprite;
        _selectionDisplay.rectTransform.DOShakeScale(0.2f, new Vector3(0.5f, 0.2f), 6, 80);

    }

    private int SlotSum()
    {
        return _bubbleValue1 + _bubbleValue2;
    }

    
    public BubbleType GetBubbleType(int bubbleSum)
    {
        BubbleType type = BubbleType.normal;

        switch (bubbleSum)
        {
            case 1:
                type = BubbleType.fire;
                break;

            case 4:
                type = BubbleType.steam;
                break;

            case 3:
                type = BubbleType.water;
                break;

            case 8:
                type = BubbleType.mud;
                break;

            case 5:
                type = BubbleType.earth;
                break;

            case 6:
                type = BubbleType.lava;
                break;
        }

        return type;

    }

}
