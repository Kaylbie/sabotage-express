using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Video;

public class TVScript : NetworkBehaviour
{
    public VideoPlayer videoPlayer;
    public VideoClip[] videoClips;
    private NetworkVariable<int> currentVideoIndex = new NetworkVariable<int>();

    void Start()
    {
        if (videoClips.Length > 0)
        {
            videoPlayer.clip = videoClips[currentVideoIndex.Value];
        }

        currentVideoIndex.OnValueChanged += OnCurrentVideoIndexChanged;
    }

    private void OnCurrentVideoIndexChanged(int oldIndex, int newIndex)
    {
        if (videoClips.Length > 0)
        {
            videoPlayer.clip = videoClips[newIndex];
            videoPlayer.Play();
        }
    }


    [ServerRpc]
    public void PlayNextVideoServerRpc()
    {
        if (videoClips.Length == 0) return;

        int newIndex = (currentVideoIndex.Value + 1) % videoClips.Length;
        currentVideoIndex.Value = newIndex; 
    }

    [ServerRpc]
    public void PlayRandomVideoServerRpc()
    {
        if (videoClips.Length == 0) return;

        int index = Random.Range(0, videoClips.Length);
        currentVideoIndex.Value = index; 
    }

    [ServerRpc]
    public void PauseVideoServerRpc()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
        else
        {
            videoPlayer.Play();
        }
    }
}
