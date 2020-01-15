using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController
{
    private bool isInitialized = false;

    private void Awake()
    {
        GameplayManager.Instance.OnInitialize += OnInitialize;
    }

    private void OnDestroy()
    {
        GameplayManager.Instance.OnInitialize -= OnInitialize;
    }

    public void OnInitialize()
    {
        isInitialized = true;
    }

    private void Update()
    {
        if (isInitialized)
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            Move(moveX, moveZ);
        }
    }
}
