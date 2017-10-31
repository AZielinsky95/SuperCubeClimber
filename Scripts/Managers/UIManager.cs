using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [Header("UI Groups")]
    [SerializeField] private GameObject m_MenuPanel;
    [SerializeField] private GameObject m_WinPanel;
    [SerializeField] private GameObject m_GameplayUI;
    [SerializeField] private GameObject m_LosePanel;
    [SerializeField] private GameObject m_PopUpPanel;

    public enum Panels { MENU, WIN, LOSE, POPUP }
    private Panels m_Panel;

    private static UIManager m_Instance;
    public static UIManager Instance
    {
        get
        {
            return m_Instance;
        }
    }

    void Awake()
    {
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(gameObject);
        }

        m_Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public GameObject MenuPanel { get { return m_MenuPanel; } }

    public GameObject LosePanel { get { return m_LosePanel; } }


    private void OnSlideInComplete()
    {
      
    }

    public void HideMenuButtons()
    {
        foreach (Button b in m_MenuPanel.GetComponentsInChildren<Button>())
        {
            b.gameObject.SetActive(false);
        }
    }

    private void OnSlideOutComplete()
    {
        m_MenuPanel.SetActive(false);
    }

    public void SlideOutPanel(GameObject obj)
    {
        Hashtable args = new Hashtable();
        args.Add("y", 25.0f);
        args.Add("time", 1.0f);
        args.Add("delay", 0.25f);
        args.Add("easetype", iTween.EaseType.linear);
        args.Add("oncompletetarget", gameObject);
        args.Add("oncomplete", "OnSlideOutComplete");
        iTween.MoveTo(obj, args);
    }

    public void SlideInPanel(GameObject obj)
    {
        Hashtable args = new Hashtable();
        args.Add("y",obj.transform.position.y + 15.0f);
        args.Add("time", 1.5f);
        args.Add("delay", 0.5f);
        args.Add("easetype", iTween.EaseType.linear);
        //args.Add("onstarttarget", gameObject);
        //args.Add("onstart", "OnSlideInStart");
        args.Add("oncompletetarget", gameObject);
        args.Add("oncomplete", "OnSlideOutComplete");
        iTween.MoveFrom(obj, args);
        LosePanel.SetActive(true);
    }

    //private void OnSlideInStart()
    //{
    //}

    public void HideLosePanel()
    {
        m_LosePanel.SetActive(false);
    }

    public void ShowLosePanel()
    {
        m_LosePanel.SetActive(true);
    }


    public void HideMenu()
    {
        m_MenuPanel.SetActive(false);
    }

    public void ShowMenu()
    {
        m_MenuPanel.GetComponentInParent<GameObject>().SetActive(true);
    }

    //public void DisableControls()
    //{
    //    foreach (Button button in m_MenuPanel.gameObject.GetComponentsInChildren<Button>())
    //    {
    //        button.enabled = false;
    //    }
    //}

    //public void EnableControls()
    //{
    //    foreach (Button button in m_MenuPanel.gameObject.GetComponentsInChildren<Button>())
    //    {
    //        button.enabled = true;
    //    }
    //}

    public void EnableWinPanel()
    {
        m_WinPanel.SetActive(true);
    }

    public void DisableWinPanel()
    {
        m_WinPanel.SetActive(false);
    }

    public void EnableLosePanel()
    {
        m_LosePanel.SetActive(true);
    }

    public void DisableLosePanel()
    {
        m_LosePanel.SetActive(false);
    }

    public void EnablePopUpPanel()
    {
        m_PopUpPanel.SetActive(true);
    }

    public void DisablePopUpPanel()
    {
        m_PopUpPanel.SetActive(false);
    }
}
