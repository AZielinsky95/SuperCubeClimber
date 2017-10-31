using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuState : State
{

    [SerializeField] private Button m_PlayButton;

    public override void Begin()
    {
        m_PlayButton.onClick.AddListener(OnPlay);
    }

    public override void End()
    {
        Debug.Log("MenuState End!");
    }

    public override void OnUpdate()
    {

    }

    void OnPlay()
    {

        //QUICKINTRO 
        //HideMenu();
        //GameManager.Instance.SetState(typeof(GameplayState));

        //LONG INTRO
        UIManager.Instance.HideMenuButtons();
        UIManager.Instance.SlideOutPanel(UIManager.Instance.MenuPanel.GetComponentInChildren<Image>().gameObject);
        GameManager.Instance.SetState(typeof(GameplayState));
        //  MoveCamera();
    }

    void OnColorSwitch()
    {

    }

    private void HideMenu()
    {
        UIManager.Instance.HideMenu();
    }

    private void OnCameraMoveComplete()
    {
        GameManager.Instance.SetState(typeof(GameplayState));
    }

    //private void MoveCamera()
    //{
    //    Hashtable args = new Hashtable();
    //    args.Add("y", 2.0f);
    //    args.Add("time", 1.5f);
    //    args.Add("delay", 2.0f);
    //    args.Add("onstarttarget", gameObject);
    //    args.Add("onstart", "HideMenu");
    //    args.Add("easetype", iTween.EaseType.linear);
    //    args.Add("oncompletetarget", gameObject);
    //    args.Add("oncomplete", "OnCameraMoveComplete");
    //    iTween.MoveTo(Camera.main.gameObject, args);
    //}
}
