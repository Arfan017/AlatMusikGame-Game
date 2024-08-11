using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MateriManager : MonoBehaviour
{
    public GameObject panelIlmu;
    public Button tombol1;
    public Button tombol2;
    public Button buttonRight;
    public Button buttonLeft;
    public Button buttonIlmu;
    public Image imageAlat;
    public TextMeshProUGUI tittle;
    public GameObject parentContent;
    public TextMeshProUGUI content;
    public Scrollbar scrollbar;
    public AlatMusik[] alatMusiks;
    private AlatMusik alatMusik;

    public AlatMusik AlatMusik
    {
        get
        {
            return alatMusik;
        }
        set
        {
            alatMusik = value;
        }
    }

    private void Awake()
    {
        panelIlmu.SetActive(false);
        content.gameObject.SetActive(false);
        buttonRight.gameObject.SetActive(false);
        buttonLeft.gameObject.SetActive(false);
    }

    private void Start()
    {
        tombol1.onClick.AddListener(() => TampilkanMateri(0));
        tombol2.onClick.AddListener(() => TampilkanMateri(1));
    }

    public void TampilkanMateri(int indeks)
    {
        if (indeks >= 0 && indeks < alatMusiks.Length)
        {
            AlatMusik = alatMusiks[indeks];
            panelIlmu.SetActive(true);
            buttonRight.gameObject.SetActive(true);
            tittle.text = AlatMusik.nama;
            imageAlat.sprite = AlatMusik.image;
            content.text = AlatMusik.materi;
            buttonIlmu.onClick.AddListener(() => Application.OpenURL(AlatMusik.sumber));
        }
    }

    public void ClickRight()
    {
        content.gameObject.SetActive(true);
        parentContent.SetActive(true);
        imageAlat.gameObject.SetActive(false);
        buttonRight.gameObject.SetActive(false);
        buttonLeft.gameObject.SetActive(true);
    }

    public void ClickLeft()
    {
        imageAlat.gameObject.SetActive(true);
        parentContent.SetActive(false);
        buttonRight.gameObject.SetActive(true);
        buttonLeft.gameObject.SetActive(false);
    }

    public void Back()
    {
        ClickLeft();
    }
}