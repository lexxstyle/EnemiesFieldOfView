using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    private static GameplayManager _instance;
    public static GameplayManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameplayManager>();
            }
            return _instance;
        }
    }

    private bool _isStartedUp = false;
    public static bool isStartedUp
    {
        get { if (Instance != null) return Instance._isStartedUp; else return false; }
        set { if (Instance != null) Instance._isStartedUp = value; }
    }

    public Action OnInitialize;


    void Start()
    {
        isStartedUp = true;

        OnInitialize?.Invoke();
    }
}
