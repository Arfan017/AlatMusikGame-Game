using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;
using Unity.VisualScripting;

public class PieceScript : MonoBehaviour
{
    private Vector3 RightPosition;
    public bool InRightPosition;
    public bool Selected;

    void Start()
    {
        RightPosition = transform.position;
        transform.position = new Vector3(Random.Range(0f, 8f), Random.Range(4.169f, -4.377f));
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, RightPosition) < 0.5f)
        {
            if (!Selected)
            {
                if (InRightPosition == false)
                {
                    transform.position = RightPosition;
                    InRightPosition = true;
                    GameObject.Find("PuzzleManager").GetComponent<PuzzelManager>().RemeaningPlace_ += -1;
                    GetComponent<SortingGroup>().sortingOrder = 0;
                }
            }
        }
    }
}