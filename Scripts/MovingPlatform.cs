using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    [SerializeField]  private float m_YPos;
    [SerializeField]  private float m_Speed;
    [SerializeField]  private float m_OriginalYPosition;
    // Use this for initialization
    public void ResetPosition()
    {
        gameObject.transform.localPosition = new Vector3(transform.localPosition.x, m_OriginalYPosition, transform.localPosition.z);
    }

    public void MoveUpDown()
    {
        Hashtable args = new Hashtable();
        args.Add("y", transform.position.y + m_YPos);
        //args.Add("onstarttarget", gameObject);
        //args.Add("onstart", "OnMoveStart");
        args.Add("name", "MovingPlatforms");
        args.Add("speed", m_Speed);
        args.Add("easetype", iTween.EaseType.linear);
        args.Add("looptype", iTween.LoopType.pingPong);
        iTween.MoveTo(gameObject, args);
    }
}
