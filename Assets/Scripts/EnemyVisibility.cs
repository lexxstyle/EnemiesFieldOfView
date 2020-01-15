using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisibility : MonoBehaviour
{
    public Transform        Pivot;
    public float            Distance = 5;
    public float            Angle = 120;

    public LayerMask        DefaultMask;
    public FOVVisualizer2D  FOV2D;

    [SerializeField] private EnemyController _EnemyController = null;
    private EnemyController m_EnemyController
    {
        get {
            if (_EnemyController == null)
                _EnemyController = GetComponent<EnemyController>();
            return _EnemyController;
        }
    }

    private void Awake()
    {
        GameplayManager.Instance.OnInitialize += OnInitialize;
        FOV2D.OnPlayerSpotted += OnPlayerSpotted;
    }

    private void OnDestroy()
    {
        GameplayManager.Instance.OnInitialize -= OnInitialize;
        FOV2D.OnPlayerSpotted -= OnPlayerSpotted;
    }

    public void OnInitialize()
    {
        FOV2D.Initialize(this);
        FOV2D.enabled = true;
    }

    public void OnPlayerSpotted(Vector3 _worldPos)
    {
        m_EnemyController.GoToPosition(_worldPos);
    }
}
