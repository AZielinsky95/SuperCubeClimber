using UnityEngine;
using System.Collections;

public class WinState : State
{
    public override void Begin()
    {
        Debug.Log("WinState Begin!");
       // UIManager.Instance.HideControls();
     //   UIManager.Instance.SlideInPanel(UIManager.Panels.WIN);
    }

    public override void End()
    {

    }

    public override void OnUpdate()
    {

    }
}
