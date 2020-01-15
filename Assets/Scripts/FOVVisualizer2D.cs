using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generate the mesh based on raycasts calculations
/// </summary>
public class FOVVisualizer2D : MonoBehaviour
{
    [Range(3, 256)] [SerializeField] private int    AmountOfRaycasts = 32;
    [SerializeField] private Vector3                OriginOffset;

    private bool                isInitialized = false;
    private RaycastHit          hitInfo = new RaycastHit();
    private EnemyVisibility     Visibility;

    public Action<Vector3>      OnPlayerSpotted;

    private MeshFilter _MeshFilter;
    private MeshFilter m_MeshFilter
    {
        get
        {
            if (_MeshFilter == null)
                _MeshFilter = GetComponent<MeshFilter>();
            return _MeshFilter;
        }
        set
        {
            _MeshFilter = value;
        }
    }

    private MeshRenderer _MeshRenderer = null;
    private MeshRenderer m_MeshRenderer
    {
        get
        {
            if (_MeshRenderer == null)
                _MeshRenderer = GetComponent<MeshRenderer>();
            return _MeshRenderer;
        }
        set { _MeshRenderer = value; }
    }


    private Material _Material;
    public Material m_Material
    {
        get
        {
            if (_Material == null) 
                _Material = m_MeshRenderer.material;
            return _Material;
        }

        set { _Material = value; }
    }

    public float Distance
    {
        get { return (Visibility != null) ? Visibility.Distance : 10; }
    }

    public float Angle
    {
        get { return (Visibility != null) ? Visibility.Angle : 120; }
    }

    /// <summary>
    /// Initialization
    /// </summary>
    /// <param name="_enemyVisibility">controller of character's visibility</param>
    public void Initialize(EnemyVisibility _enemyVisibility)
    {
        Visibility = _enemyVisibility;

        if (m_MeshRenderer == null || m_Material == null)
        {
            Debug.LogError("Some required components = NULL");
            return;
        }

        transform.localPosition += OriginOffset;

        isInitialized = true;
    }

    private void OnEnable()
    {
        if (!isInitialized) return;

        if (!m_MeshRenderer.enabled) m_MeshRenderer.enabled = true;
    }

    private void OnDisable()
    {
        if (!isInitialized) return;

        Reset();
    }

    /// <summary>
    /// Used for cleaning memory
    /// </summary>
    public void Reset()
    {
        m_MeshRenderer.enabled = false;
        if (m_MeshFilter.mesh != null)
            m_MeshFilter.mesh.Clear();
    }

    private void Update()
    {
        if (!GameplayManager.isStartedUp) return;

        RenderFOV(Visibility.DefaultMask);
    }

    /// <summary>
    /// Using raycasts generate the interactive points, in global positions
    /// </summary>
    /// <param name="__castMask">layermask for filtering visibility</param>
    private void RenderFOV(LayerMask __castMask)
    {
        Vector3[] points = new Vector3[AmountOfRaycasts + 2];
        transform.rotation = Quaternion.identity;

        for (int i = 1; i <= AmountOfRaycasts + 1; i++)
        {
            float degrees = (((i - 1.0f) / AmountOfRaycasts) * Angle) - Angle / 2 - transform.parent.rotation.eulerAngles.y + 90;

            if (Physics.Raycast(new Ray(transform.position, RadiansToVector3(degrees * Mathf.Deg2Rad)), out hitInfo, Distance, __castMask))
            {
                points[i] = hitInfo.point - transform.position;

                if (Time.frameCount % 15 == 0)
                {
                    if (hitInfo.transform != null && hitInfo.transform.CompareTag("Player"))
                        OnPlayerSpotted?.Invoke(hitInfo.transform.position);
                }
            }
            else
                points[i] = RadiansToVector3(degrees * Mathf.Deg2Rad).normalized * Distance;
        }
        m_MeshFilter.mesh = GenerateMesh(points);
    }

    /// <summary>
    /// Generate the mesh
    /// </summary>
    /// <param name="_vertices">generated points in world coordinates</param>
    /// <returns></returns>
    private Mesh GenerateMesh(Vector3[] _vertices)
    {
        Mesh m = new Mesh();
        m.name = "FOV2D";

        _vertices[0] = Vector3.zero;
        m.vertices = _vertices;

        int[] trianglesArray = new int[(m.vertices.Length - 1) * 3];
        int count = 1;
        for (int i = 0; i < trianglesArray.Length - 3; i += 3)
        {
            trianglesArray[i] = count;
            trianglesArray[i + 1] = 0;
            trianglesArray[i + 2] = count + 1;
            count++;
        }
        m.triangles = trianglesArray;
        m.RecalculateNormals();

        return m;
    }

    Vector3 RadiansToVector3(float degrees)
    {
        return new Vector3(Mathf.Cos(degrees), 0, Mathf.Sin(degrees));
    }
}