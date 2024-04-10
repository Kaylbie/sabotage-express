using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;


public class PlayerUI : NetworkBehaviour
{
    //[SerializeField]
    private TextMeshProUGUI promptText;

    void Awake()
    {
        promptText = GetComponentInChildren<TextMeshProUGUI>(true);
    }

    void Update()
    {
        //if (!IsOwner) return;
    }

    // Update is called once per frame
    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }
}
