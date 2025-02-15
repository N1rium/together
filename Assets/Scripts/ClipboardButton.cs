using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClipboardButton : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Button button;

    private void OnEnable()
    {
        button.onClick.AddListener(OnClicked);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnClicked);
    }

    public void OnClicked()
    {
        Debug.Log(text.text);
        GUIUtility.systemCopyBuffer = text.text;
    }
}
