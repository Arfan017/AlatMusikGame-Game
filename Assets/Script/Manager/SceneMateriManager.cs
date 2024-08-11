using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMateriManager : MonoBehaviour
{
    public GameObject panelMateri1;
    public GameObject panelMateri2;
    public GameObject panelMateri3;
    public AudioSource myAudioSource;

    private void Start()
    {
        myAudioSource.volume = PlayerPrefs.GetFloat("musicVolume");
        panelMateri1.SetActive(false);
        panelMateri2.SetActive(false);
        panelMateri3.SetActive(false);
    }

    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
