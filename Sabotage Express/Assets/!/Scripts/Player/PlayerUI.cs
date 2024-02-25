using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerUI : MonoBehaviour
{
    //[SerializeField]
    private TextMeshProUGUI promptText;

    void Awake()
    {
        promptText = GetComponentInChildren<TextMeshProUGUI>(true);
    }

    // Update is called once per frame
    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }
}
