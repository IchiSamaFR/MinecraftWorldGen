using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollection : MonoBehaviour
{
    public static KeyCollection instance;
    public string forward = "z";
    public string backward = "s";
    public string left = "q";
    public string right = "d";
    public string jump = "space";
    public string interact = "e";
    public string drop = "a";



    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

    }

    public void Refresh()
    {
        if (PlayerPrefs.GetInt("inputs") == 0)
        {
            forward = "w";
            backward = "s";
            left = "a";
            right = "d";
            interact = "e";
            drop = "q";
        }
        else
        {
            forward = "z";
            backward = "s";
            left = "q";
            right = "d";
            interact = "e";
            drop = "a";
        }
    }
}
