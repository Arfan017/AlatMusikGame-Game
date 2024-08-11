using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class AlatMusik
{
    public String nama;
    [TextArea(15, 20)]
    public String materi;
    public Sprite image;
    public string sumber;
}