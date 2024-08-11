using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class PuzzelManager : MonoBehaviour
{
    public GameObject SelectedPiece;
    public Sprite newPuzzle;
    public AudioSource myAudioSource;
    public bool timerIsRunning = false;
    public TextMeshProUGUI TextTime;
    [SerializeField] private float timeInSeconds;
    private float currentTime;
    private int RemeaningPlace = 16;
    int OIL = 1;

    public int RemeaningPlace_
    {
        get
        {
            return RemeaningPlace;
        }
        set
        {
            RemeaningPlace = value;
            if (RemeaningPlace_ == 0)
            {
                ShowPanelWin();
            }
        }
    }
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        Time.timeScale = 1;
        myAudioSource.volume = PlayerPrefs.GetFloat("musicVolume");
        currentTime = timeInSeconds;
        timerIsRunning = true;
    }

    void Update()
    {

        if (timerIsRunning)
        {
            currentTime -= Time.deltaTime;
            SetTime(currentTime);
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.transform != null && hit.transform.CompareTag("Puzzle"))
            {
                PieceScript pieceScript = hit.transform.GetComponent<PieceScript>();
                if (pieceScript != null && !pieceScript.InRightPosition)
                {
                    SelectedPiece = hit.transform.gameObject;
                    pieceScript.Selected = true;
                    SelectedPiece.GetComponent<SortingGroup>().sortingOrder = OIL;
                    OIL++;
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && SelectedPiece != null)
        {
            PieceScript pieceScript = SelectedPiece.GetComponent<PieceScript>();
            if (pieceScript != null)
            {
                pieceScript.Selected = false;
            }
            SelectedPiece = null;
        }

        if (SelectedPiece != null)
        {
            Vector3 MousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SelectedPiece.transform.position = new Vector3(MousePoint.x, MousePoint.y, 0);
        }
    }

    public GameObject StartPanel, PanelMenang, PanelKalah, PanelSelesai;
    public void SetPuzzlePhoto(Sprite Photo)
    {
        // string nextPuzzleName = PlayerPrefs.GetString("NextPuzzlePhoto", "");
        // // Debug.Log(nextPuzzleName);

        // if (!string.IsNullOrEmpty(nextPuzzleName))
        // {
        //     Photo = Resources.Load<Sprite>(nextPuzzleName);
        //     for (int i = 0; i < 36; i++)
        //     {
        //         Transform pieceTransform = GameObject.Find("Piece (" + i + ")")?.transform;
        //         if (pieceTransform != null)
        //         {
        //             Transform puzzleTransform = pieceTransform.Find("Puzzle");
        //             if (puzzleTransform != null)
        //             {
        //                 puzzleTransform.GetComponent<SpriteRenderer>().sprite = Photo;
        //             }
        //         }
        //     }
        //     PlayerPrefs.DeleteKey("NextPuzzlePhoto");
        // }
        // else
        // {
        // Debug.Log(nextPuzzleName);
        // Photo != null
        for (int i = 0; i < 36; i++)
        {
            Transform pieceTransform = GameObject.Find("Piece (" + i + ")")?.transform;
            if (pieceTransform != null)
            {
                Transform puzzleTransform = pieceTransform.Find("Puzzle");
                if (puzzleTransform != null)
                {
                    puzzleTransform.GetComponent<SpriteRenderer>().sprite = Photo;
                }
            }
        }
        StartPanel.SetActive(false);
        // }
    }

    public void NextPuzzle()
    {
        SceneManager.LoadSceneAsync(6);
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // PlayerPrefs.SetString("NextPuzzlePhoto", newPuzzle.name);
    }

    public void ShowPanelWin()
    {
        if (SceneManager.GetActiveScene().name == "GameLvl3 1" || SceneManager.GetActiveScene().name == "GameLvl3 2")
        {
            Time.timeScale = 0;
            PanelMenang.SetActive(true);
        }
        else
        {
            PanelSelesai.SetActive(true);
        }
    }

    public void ShowPanelLose()
    {
        PanelKalah.SetActive(true);
    }

    public void NextGame(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex);
    }

    public void ExitGame(int sceneIndex)
    {
        PlayerPrefs.SetInt("PuzzleFinish", 1);
        PlayerPrefs.SetInt("GameFinish", 1);
        SceneManager.LoadSceneAsync(sceneIndex);
        Destroy(audioManager.gameObject);
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void SetTime(float value)
    {
        TimeSpan time = TimeSpan.FromSeconds(currentTime);                       //set the time value
        TextTime.text = time.ToString("mm':'ss");   //convert time to Time format

        if (currentTime <= 0)
        {
            Time.timeScale = 0;
            ShowPanelLose();
        }
    }
}