using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct xianduan
{
    public Vector3 A;
    public Vector3 B;
    public Vector3 normal;
    public bool auto;
}

public class Wall : MonoBehaviour
{
    [SerializeField]
    public List<xianduan> xianduanList;

    public float angle;
    public int what;
}
