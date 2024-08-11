using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class QuizManagar : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string questionText;
        public Sprite imageQuestion;
        public List<string> options;
        public int correctOptionIndex;
    }

    public List<Question> questions;
    private List<Question> remainingQuestions;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI NoQuest;
    public Image imageQuestion;
    public Animator panelBenar;
    public Animator panelSalah;
    public List<Button> optionButtons;
    public GameObject health0, health1, health2;
    public GameObject PanelMenang, PanelKalah, panelbenar, panelsalah;
    public AudioSource myAudioSource;
    public AudioSource soundBenar;
    public AudioSource soundSalah;
    private int currentQuestionIndex;
    private int health;
    private bool isAnswered;
    private bool QuizFinish;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("audio").GetComponent<AudioManager>();
    }

    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }

    void Start()
    {
        remainingQuestions = new List<Question>(questions);
        myAudioSource.volume = PlayerPrefs.GetFloat("musicVolume");
        Health = 3;

        // TrueAnswer = 0;
        QuizFinish = false;
        health0.gameObject.SetActive(true);
        health1.gameObject.SetActive(true);
        health2.gameObject.SetActive(true);
        // health3.gameObject.SetActive(true);

        currentQuestionIndex = 0;
        isAnswered = false;
        ShuffleQuestions();
        ShowQuestion();
    }

    void ShuffleQuestions()
    {
        // Mengacak urutan pertanyaan menggunakan algoritma Fisher-Yates
        for (int i = remainingQuestions.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Question temp = remainingQuestions[i];
            remainingQuestions[i] = remainingQuestions[j];
            remainingQuestions[j] = temp;
        }
    }

    void ReduceHealth()
    {
        Health -= 1;
        switch (Health)
        {
            case 3:
                health0.gameObject.SetActive(true);
                health1.gameObject.SetActive(true);
                health2.gameObject.SetActive(true);
                break;

            case 2:
                health0.gameObject.SetActive(false);
                break;

            case 1:
                health1.gameObject.SetActive(false);
                break;

            case 0:
                health2.gameObject.SetActive(false);
                ShowPanelKalah();
                break;
        }
    }

    void ShowQuestion()
    {
        if (remainingQuestions.Count > 0)
        {
            Question currentQuestion = remainingQuestions[0];
            NoQuest.text = questions.Count.ToString() + "/" + (questions.Count - remainingQuestions.Count + 1);
            questionText.text = currentQuestion.questionText;
            imageQuestion.sprite = currentQuestion.imageQuestion;

            for (int i = 0; i < optionButtons.Count; i++)
            {
                if (i < currentQuestion.options.Count)
                {
                    optionButtons[i].gameObject.SetActive(true);
                    optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.options[i];
                }
                else
                {
                    optionButtons[i].gameObject.SetActive(false);
                }
            }
            isAnswered = false;
        }
        else
        {
            if (Health != 0)
            {
                ShowPanelMenang();
            }
        }
    }

    public void OnOptionSelected(int optionIndex)
    {
        if (!isAnswered)
        {
            isAnswered = true;

            Question currentQuestion = remainingQuestions[0];

            if (optionIndex == currentQuestion.correctOptionIndex)
            {
                Panelbenar();
            }
            else
            {
                Panelsalah();
                ReduceHealth();
            }
            NextQuestionWithDelay();
        }
    }

    public void NextQuestionWithDelay()
    {
        // currentQuestionIndex++;
        remainingQuestions.RemoveAt(0);
        ShowQuestion();
    }

    public void ShowPanelMenang()
    {
        Time.timeScale = 0;
        QuizFinish = true;
        PlayerPrefs.SetInt("QuizFinish", 1);
        PanelMenang.SetActive(true);
    }

    public void ShowPanelKalah()
    {
        Time.timeScale = 0;
        PanelKalah.SetActive(true);
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame(int sceneIndex)
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(sceneIndex);
        Destroy(audioManager.gameObject);
    }

    public void NextGame(int sceneIndex)
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(sceneIndex);
    }

    private int boolToInt(bool val)
    {
        if (val)
            return 1;
        else
            return 0;
    }

    public void Panelbenar()
    {
        panelbenar.SetActive(true);
        soundBenar.Play();
    }

    public void Panelsalah()
    {
        panelsalah.SetActive(true);
        soundSalah.Play();
    }

    public void animationEnd()
    {
        gameObject.SetActive(false);
    }
}