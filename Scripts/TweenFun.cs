using UnityEngine;
using System.Collections;

public class TweenFun : MonoBehaviour
{
    [Header("Scale Parameters")]
    [SerializeField]  private Vector3 doubleSize = new Vector3(2,2,2);
    [SerializeField]  private Vector3 halfSize = new Vector3(0.5f, 0.5f, 0.5f);
    [SerializeField]  private float m_ScaleTime = 1.5f;
    private Vector3 m_LocalScale;
    private float m_OriginalAlpha;

    [Header("Move Parameters")]
    [SerializeField] private float m_OffsetY = 1.0f;
    [SerializeField] private float m_MoveTime = 0.5f;
    [SerializeField] private iTween.EaseType m_EaseType;


    private enum TweenType { SCALE,MOVE,FADE}
    [SerializeField] private TweenType m_TweenType = TweenType.SCALE;


    private void Awake()
    {
        m_LocalScale = gameObject.transform.localScale;
    }

    void Start()
    {
        if (m_TweenType == TweenType.SCALE)
        {
            ScaleBy(doubleSize, m_ScaleTime);
        }
        else if (m_TweenType == TweenType.MOVE)
        {
            FloatEffect(m_OffsetY, m_MoveTime);
        }
        else if (m_TweenType == TweenType.FADE)
        {
            m_OriginalAlpha = GetComponent<Renderer>().material.color.a;
            ScaleBy(doubleSize, 2.0f);
            FadeAway(doubleSize, 2.0f);
        }
    }

    private void FloatEffect(float pos,float time)
    {
        Hashtable args = new Hashtable();
        args.Add("y", pos);
        args.Add("time", time);
        args.Add("easetype", m_EaseType);
      //  args.Add("looptype", iTween.LoopType.pingPong);
        iTween.MoveTo(gameObject, args);

    }

    public static void ColorTo(GameObject m_Obj, Color m_Color, iTween.LoopType m_LoopType, iTween.EaseType m_EaseType, float time = 1.0f, float delay = 0.0f, System.Action OnComplete = null)
    {
        Hashtable ColorTween = new Hashtable();
        ColorTween.Add("a", m_Color.a);
        ColorTween.Add("time", time);
        ColorTween.Add("delay", delay);
        if (OnComplete != null)
        {
            ColorTween.Add("oncompletetarget", m_Obj);
            ColorTween.Add("oncomplete", OnComplete.Method.Name);
        }
        ColorTween.Add("looptype", m_LoopType);
        ColorTween.Add("easetype", m_EaseType);
        iTween.ColorTo(m_Obj, ColorTween);
    }


    public static void ScaleBy(GameObject m_Obj, Vector3 m_Amount, float m_Duration, iTween.LoopType m_LoopType, iTween.EaseType m_EaseType, System.Action OnComplete = null)
    {
        Hashtable scaleTween = new Hashtable();
        scaleTween.Add("amount", m_Amount);
        scaleTween.Add("time", m_Duration);
        scaleTween.Add("looptype", m_LoopType);
        scaleTween.Add("easetype", m_EaseType);
        if (OnComplete != null)
        {
            scaleTween.Add("oncomplete", "OnComplete");
        }
        iTween.ScaleBy(m_Obj, scaleTween);
    }

    public void FadeAway(Vector3 m_Amount, float m_Duration, float m_Delay = 0)
    {
        ColorTo(this.gameObject, new Color(0, 0, 0, 0.01f), iTween.LoopType.pingPong, iTween.EaseType.linear, 2.0f, 0);
    }

    private void OnFadeAwayComplete()
    {
      //  this.gameObject.GetComponent<Renderer>().material.color = m_OriginalColor;
        transform.localScale = m_LocalScale;
        FadeAway(doubleSize, 2.0f, 1.0f);
    }

    private void ScaleBy(Vector3 scaleFactor,float time,bool shouldLoop = true,System.Action onComplete = null)
    {
        Hashtable args = new Hashtable();
        args.Add("amount", scaleFactor);
        args.Add("time", time);
        args.Add("easetype", iTween.EaseType.easeInOutSine);

        if (onComplete != null)
        {
            args.Add("oncompletetarget", gameObject);
            args.Add("oncomplete", onComplete.Method.Name);
        }

        if(shouldLoop)
        args.Add("looptype", iTween.LoopType.pingPong);

        iTween.ScaleBy(gameObject, args);     
    }
}
