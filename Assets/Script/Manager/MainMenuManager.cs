using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public GameObject panelUtama;
    public GameObject panelMainmenu;
    public GameObject panelAbout;
    // public AudioSource myAudio;
    public TextMeshProUGUI textAbout;
    public Slider mySlider;
    private float musicVolume = 1f;
    public VolumeSetting volumeSetting;

    private void Awake()
    {
        panelMainmenu.SetActive(false);
        panelAbout.SetActive(false);
    }

    private void Start()
    {

        if (PlayerPrefs.HasKey("musicVolume"))
        {
            // LoadVolume();
            volumeSetting.LoadVolume();
        }
        else
        {
            volumeSetting.setMusicVolume();
            // updateVolume(musicVolume);
        }

        int statusPanelUtama = PlayerPrefs.GetInt("PanelUtamaTerbuka", 1);
        if (statusPanelUtama == 1)
        {
            panelUtama.SetActive(true);
        }
        else
        {
            panelUtama.SetActive(false);
            panelMainmenu.SetActive(true);
        }
    }

    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void DisablePanel()
    {
        panelUtama.SetActive(false);
        PlayerPrefs.SetInt("PanelUtamaTerbuka", 0);
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("PanelUtamaTerbuka");
        PlayerPrefs.Save();
    }

    public void updateVolume(float volume)
    {
        musicVolume = volume;
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}