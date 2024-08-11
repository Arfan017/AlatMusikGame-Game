using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button GameLv2, GameLv3;
    public GameObject lock2, lock3;
    public AudioSource myAudioSource;
    public GameObject PanelTamat;
    public AudioSource audioSource;
    int TrueAnswer, IsComplete;
    private bool QuizFinish, QuizWordFinish, PuzzleFinish;
    AudioManager audioManager;
    Boolean tamat;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        myAudioSource.volume = PlayerPrefs.GetFloat("musicVolume");
        GameLv2.interactable = false;
        GameLv3.interactable = false;
        tamat = intToBool(PlayerPrefs.GetInt("GameFinish", 0));
        QuizFinish = intToBool(PlayerPrefs.GetInt("QuizFinish"));
        QuizWordFinish = intToBool(PlayerPrefs.GetInt("QuizWordFinish"));
        PuzzleFinish = intToBool(PlayerPrefs.GetInt("PuzzleFinish"));



        if (QuizFinish)
        {
            GameLv2.interactable = true;
            lock2.SetActive(false);
        }

        if (QuizWordFinish)
        {
            GameLv3.interactable = true;
            lock3.SetActive(false);
        }

        if (PuzzleFinish)
        {
            if (tamat)
            {
                PanelTamat.SetActive(true);
                audioSource.Play();
                PlayerPrefs.SetInt("GameFinish", 0);
            }
        }
    }

    public void ChangeSceneToGame(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        Destroy(audioManager.gameObject);
    }

    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    private bool intToBool(int val)
    {
        if (val != 0)
            return true;
        else
            return false;
    }
}