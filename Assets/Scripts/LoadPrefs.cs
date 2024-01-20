using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class LoadPrefs : MonoBehaviour
{
    [Header("General Setting")]
    [SerializeField] private bool canUse = false;
    [SerializeField] private MenuController menuController;

    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private GameObject videoPlayerObject; 
    private VideoPlayer videoPlayer;



    private void Awake()
    {
        if (canUse)
        {
            if(PlayerPrefs.HasKey("masterVolume"))
            {
                videoPlayer = videoPlayerObject.GetComponent<VideoPlayer>();

                float localVolume = PlayerPrefs.GetFloat("masterVolume");
                volumeTextValue.text = localVolume.ToString();
                volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;
                videoPlayer.SetDirectAudioVolume(0, localVolume);
            }
            else{
                menuController.ResetButton("Audio");
            }

            
        }
        
    }
}
