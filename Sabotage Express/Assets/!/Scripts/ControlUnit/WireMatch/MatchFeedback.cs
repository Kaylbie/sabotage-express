using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchFeedback : MonoBehaviour
{
    public Material matchMaterial;
    public Material misMatchMaterial;

    private new Renderer renderer;  

    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }
    public void ChangeMaterialWithMatch(bool isCorrectMatch)
    {
        if (isCorrectMatch)
        {
            renderer.material = matchMaterial;
        }
        else
        {
            renderer.material = misMatchMaterial;
        }
    }
}
