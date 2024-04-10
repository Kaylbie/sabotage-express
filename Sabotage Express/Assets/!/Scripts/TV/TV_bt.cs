using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TvButton : Interactable
{
    public VideoPlayer videoPlayer;
    public TextMeshPro text;
    public string keyValue;
    public float pressedScaleY = 0.2f;
    public float animationSpeed = 25f;
    private Vector3 originalScale;
    private Vector3 targetScale;
    private bool isPressed = false;

    private void Start()
    {
        originalScale = transform.localScale;
        targetScale = new Vector3(originalScale.x, originalScale.y * pressedScaleY, originalScale.z);
    }
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, isPressed ? targetScale : originalScale, Time.deltaTime * animationSpeed);
        Vector3 currentTargetScale = isPressed ? targetScale : originalScale;

        if (Vector3.Distance(transform.localScale, currentTargetScale) < 0.001f)
        {
            if (isPressed)
            {
                isPressed = false;
            }
        }
    }
    protected override void Interact(GameObject player)
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
            text.text = ">";
        }
        else
        {
            videoPlayer.Play();
            text.text = "||";
        }
    }
}
