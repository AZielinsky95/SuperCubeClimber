using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoseState : State
{
    private readonly Vector3 ORIGINAL_CAMERA_POSITION = new Vector3(0, -0.5f, -10);

    [SerializeField] private Button m_PlayButton;
    [SerializeField] private Text m_ScoreText;
    [SerializeField] private Text m_HighScoreText;
    [SerializeField] private Player m_Player;

    public override void Begin()
    {
        // UIManager.Instance.HideControls();
        Camera.main.GetComponent<CameraFollow>().enabled = false;
        DisplayScore();
        UIManager.Instance.SlideInPanel(UIManager.Instance.LosePanel);
        m_PlayButton.onClick.AddListener(OnPlay);
    }

    public override void End()
    {

    }

    public override void OnUpdate()
    {

    }

    private void DisplayScore()
    {
        m_ScoreText.text = "Score : " + m_Player.GetHighScore.ToString();
    }

    void OnPlay()
    {
        UIManager.Instance.HideLosePanel();
        //CameraReset();
        Camera.main.transform.position = ORIGINAL_CAMERA_POSITION;
        OnCameraResetComplete();
    }

    private void CameraReset()
    {
        Hashtable args = new Hashtable();
        args.Add("position", ORIGINAL_CAMERA_POSITION);
        args.Add("time", 1.5f);
        args.Add("oncompletetarget", gameObject);
        args.Add("oncomplete", "OnCameraResetComplete");
        iTween.MoveTo(Camera.main.gameObject, args);
    }

    void OnCameraResetComplete()
    {
        GameManager.Instance.SetState(typeof(GameplayState));
    }
}
