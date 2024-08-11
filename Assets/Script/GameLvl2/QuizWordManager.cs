using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuizWordManager : MonoBehaviour
{
    public static QuizWordManager instance;
    [SerializeField] private GameObject gameComplete;
    [SerializeField] private QuizDataScriptable questionDataScriptable;
    [SerializeField] private Image questionImage;
    [SerializeField] private WordData[] answerWordList;
    [SerializeField] private WordData[] optionsWordList;
    public AudioSource myAudioSource;
    private GameStatus gameStatus = GameStatus.Playing;
    private char[] wordsArray = new char[6];

    private List<int> selectedWordsIndex;
    private int currentAnswerIndex = 0, currentQuestionIndex = 0;
    public TextMeshProUGUI NoQuest;

    private bool correctAnswer = true;
    private string answerWord;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("audio").GetComponent<AudioManager>();
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    void Start()
    {
        myAudioSource.volume = PlayerPrefs.GetFloat("musicVolume");

        selectedWordsIndex = new List<int>();
        SetQuestion();
    }

    void SetQuestion()
    {

        NoQuest.text = questionDataScriptable.questions.Count.ToString() + "/" + (currentQuestionIndex + 1);
        gameStatus = GameStatus.Playing;

        answerWord = questionDataScriptable.questions[currentQuestionIndex].answer;

        questionImage.sprite = questionDataScriptable.questions[currentQuestionIndex].questionImage;

        ResetQuestion();

        selectedWordsIndex.Clear();
        Array.Clear(wordsArray, 0, wordsArray.Length);

        for (int i = 0; i < answerWord.Length; i++)
        {
            wordsArray[i] = char.ToUpper(answerWord[i]);
        }

        for (int j = answerWord.Length; j < wordsArray.Length; j++)
        {
            wordsArray[j] = (char)UnityEngine.Random.Range(65, 90);
        }

        wordsArray = ShuffleList.ShuffleListItems<char>(wordsArray.ToList()).ToArray();

        for (int k = 0; k < optionsWordList.Length; k++)
        {
            optionsWordList[k].SetWord(wordsArray[k]);
        }

    }

    public void ResetQuestion()
    {
        for (int i = 0; i < answerWordList.Length; i++)
        {
            answerWordList[i].gameObject.SetActive(true);
            answerWordList[i].SetWord('_');
        }

        for (int i = answerWord.Length; i < answerWordList.Length; i++)
        {
            answerWordList[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < optionsWordList.Length; i++)
        {
            optionsWordList[i].gameObject.SetActive(true);
        }

        currentAnswerIndex = 0;
    }

    /// <summary>
    /// When we click on any options button this method is called
    /// </summary>
    /// <param name="value"></param>
    public void SelectedOption(WordData value)
    {
        if (gameStatus == GameStatus.Next || currentAnswerIndex >= answerWord.Length) return;

        selectedWordsIndex.Add(value.transform.GetSiblingIndex());
        value.gameObject.SetActive(false);
        answerWordList[currentAnswerIndex].SetWord(value.wordValue);

        currentAnswerIndex++;

        if (currentAnswerIndex == answerWord.Length)
        {
            correctAnswer = true;
            for (int i = 0; i < answerWord.Length; i++)
            {
                if (char.ToUpper(answerWord[i]) != char.ToUpper(answerWordList[i].wordValue))
                {
                    correctAnswer = false;
                    break;
                }
            }

            if (correctAnswer)
            {
                Debug.Log("Correct Answer");
                gameStatus = GameStatus.Next;
                currentQuestionIndex++;

                if (currentQuestionIndex < questionDataScriptable.questions.Count)
                {
                    Invoke("SetQuestion", 0.5f);
                }
                else
                {
                    Debug.Log("Game Complete");
                    gameComplete.SetActive(true);
                    PlayerPrefs.SetInt("QuizWordFinish", 1);
                }
            }
        }
    }

    public void ResetLastWord()
    {
        if (selectedWordsIndex.Count > 0)
        {
            int index = selectedWordsIndex[selectedWordsIndex.Count - 1];
            optionsWordList[index].gameObject.SetActive(true);
            selectedWordsIndex.RemoveAt(selectedWordsIndex.Count - 1);

            currentAnswerIndex--;
            answerWordList[currentAnswerIndex].SetWord('_');
        }
    }

    public void ExitGame(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex);
        Destroy(audioManager.gameObject);
    }

    public void NextGame(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex);
    }

}

[System.Serializable]
public class QuestionData
{
    public Sprite questionImage;
    public string answer;
}

public enum GameStatus
{
    Next,
    Playing
}