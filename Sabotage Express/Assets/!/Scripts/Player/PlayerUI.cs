using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;


public class PlayerUI : NetworkBehaviour
{
    private TextMeshProUGUI promptText;

    void Awake()
    {
        promptText = GetComponentInChildren<TextMeshProUGUI>(true);
    }

    void Update()
    {
        //if (!IsOwner) return;
    }

    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }
}
