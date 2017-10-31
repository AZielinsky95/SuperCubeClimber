using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameplayState : State
{
    [SerializeField] Player m_Player;   
    [SerializeField] GameObject m_Sections;
    [SerializeField] Text m_ScoreText;
    [SerializeField] Text m_PopUpText;

    public override void Begin()
    {
        Debug.Log("GamePlayState Begin!");
        ResetGame();
    }

    public override void End()
    {
        Debug.Log("GamePlayState End!");
    }

    public override void OnUpdate()
    {
        DisplayScore();
    }

    private void DisplayStartText()
    {
        
    }

    private void ResetGame()
    {
        iTween.StopByName("MovingPlatforms");
        Debug.Log("Resetting Game!");
        m_ScoreText.text = ("0");
        m_ScoreText.enabled = true;
        Camera.main.backgroundColor = ColorManager.Instance.CurrentPalette.GetBGColor;

        m_Player.gameObject.SetActive(true);
        SectionManager.Instance.ResetSections();
        SectionManager.Instance.SpawnSection();
        m_Player.GetComponent<Player>().enabled = true;
        m_Player.GetComponent<Player>().ResetPlayer();
    }

    private void DisplayScore()
    {
        
        m_ScoreText.text = m_Player.GetCurrentPlayerScore().ToString();
    }
}
