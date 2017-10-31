using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // public PlayerStats stats;

    private static GameManager m_Instance;
    public static GameManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                GameObject obj = new GameObject("GameManager");
                m_Instance = obj.AddComponent<GameManager>();
                DontDestroyOnLoad(obj);
            }

            return m_Instance;
        }
    }
   // [SerializeField] Transform m_LevelAnchor;

    void Awake()
    {
        Application.targetFrameRate = 60;

        if (m_Instance != null && m_Instance != this)
        {
            Destroy(gameObject);
        }

        m_Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SetState(typeof(MenuState));
    }

    private State m_CurrentState;

    public State CurrentState
    {
        get { return m_CurrentState; }
    }

    public void SetState(System.Type newStateType)
    {
        if (m_CurrentState != null)
        {
            m_CurrentState.End();
        }

        m_CurrentState = GetComponentInChildren(newStateType) as State;
        if (m_CurrentState != null)
        {
            m_CurrentState.Begin();
        }
    }

    void Update()
    {
        if (m_CurrentState != null)
        {
            m_CurrentState.OnUpdate();
        }
    }

}

