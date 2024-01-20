using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Video;

public class MenuController : MonoBehaviour
{
    [Header ("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private int defaultVolume = 50;

    [Header ("Gameplay Settings")]
    [SerializeField] private TMP_Text ControllerSenTextValue = null;
    [SerializeField] private Slider controllerSenSlider = null;
    [SerializeField] private int defaultSen = 8;
    public int mainControllerSen = 8;

    [Header ("Toggle Settings")]
    [SerializeField] private Toggle invertYToggle = null;

    [Header ("Confirmation Prompt")]
    [SerializeField] private GameObject confirmationPrompt = null;

    [Header ("Levels to Load")]
    public int _newGameLevel;
    private string levelToLoad;
    [SerializeField] private GameObject noSavedGameDialog = null;

    [Header("VideoPlayer Settings")]
    [SerializeField] private GameObject videoPlayerObject;
    private VideoPlayer videoPlayer;

    private void Start()
    {
        videoPlayer = videoPlayerObject.GetComponent<VideoPlayer>();
    }

    //LoadsNewGame
    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(_newGameLevel);
    }


    //Loads an exisisting game
    public void LoadGameDialogYes()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else{
            noSavedGameDialog.SetActive(true);
        }
    }


    //EXIT button closes game
    public void ExitButton()
    {
        Application.Quit();
    }

    public void setVolume(float volume)
    {
        AudioListener.volume = volume/100f;
        volumeTextValue.text = volume.ToString();
       if (videoPlayer != null)
    {
        videoPlayer.SetDirectAudioVolume(0, volume / 100f);
    }
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    public void SetControllerSen(float sensitivity)
    {
        mainControllerSen = Mathf.RoundToInt(sensitivity);
        ControllerSenTextValue.text = sensitivity.ToString("0");
    }


    public void GameplayApply()
    {
        if (invertYToggle.isOn)
        {
            PlayerPrefs.SetInt("masterInvertY", 1);
        }
        else
        {
            PlayerPrefs.SetInt("masterInvertY", 0);
        }

        PlayerPrefs.SetFloat("masterSen", mainControllerSen);
        StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(string MenuType)
    {
        if (MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString();
            VolumeApply();
        }

        if (MenuType == "Gameplay")
        {
            ControllerSenTextValue.text = defaultSen.ToString("0");
            controllerSenSlider.value = defaultSen;
            mainControllerSen = defaultSen;
            invertYToggle.isOn = false;
            GameplayApply();
        }
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }
}
