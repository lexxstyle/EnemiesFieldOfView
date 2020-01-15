using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseController : MonoBehaviour
{
    [SerializeField] protected float MoveSpeed = 2.0f;

    private CharacterController _CharController = null;
    protected CharacterController m_CharController
    {
        get
        {
            if (_CharController == null)
                _CharController = GetComponent<CharacterController>();
            return _CharController;
        }
    }

    private NavMeshAgent _NavAgent = null;
    protected NavMeshAgent m_NavAgent
    {
        get
        {
            if (_NavAgent == null)
                _NavAgent = GetComponent<NavMeshAgent>();
            return _NavAgent;
        }
    }

    protected Vector3 MoveMotion;

    private Vector3 PointDestination;
    private bool isArrivedToDestination = true;

    /// <summary>
    /// Call this from loop methods
    /// </summary>
    protected void Move(float _right, float _forward)
    {
        MoveMotion = new Vector3(_right, 0, _forward) * MoveSpeed;
        if (m_CharController != null)
        {
            m_CharController.SimpleMove(MoveMotion);
        }
    }

    /// <summary>
    /// Send to target point
    /// </summary>
    /// <param name="_worldPos">world coordinates</param>
    public void GoToPosition(Vector3 _worldPos)
    {
        PointDestination = _worldPos;
        isArrivedToDestination = false;
    }

    void Update()
    {
        if (!isArrivedToDestination)
        {
            isArrivedToDestination = !m_NavAgent.SetDestination(PointDestination);
        }
    }
}
