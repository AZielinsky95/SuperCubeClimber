using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Palette
{
    Color32 m_BackgroundColor;
    Color32 m_WallColor;
    Color32 m_ObstacleColor;
    Color32 m_DiamondColor;
    Color32 m_PlayerColor;

    public Color32 GetBGColor { get { return m_BackgroundColor; } }
    public Color32 GetWallColor { get { return m_WallColor; } }
    public Color32 GetObstacleColor { get { return m_ObstacleColor; } }
    public Color32 GetDiamondColor { get { return m_DiamondColor; } }
    public Color32 GetPlayerColor { get { return m_PlayerColor; } }

    public Palette(Color32 background, Color32 walls, Color32 obstacles, Color32 diamond,Color32 player)
	{
        m_BackgroundColor = background;
        m_WallColor = walls;
        m_ObstacleColor = obstacles;
        m_DiamondColor = diamond;
        m_PlayerColor = player;
	}
}

//Palettes:
//BlACK BG / WHITE WALLS / RED OBSTACLES / PURPLE DIAMOND
//RED BG / BLACK WALLS / WHITE OBSTACLES / BLUE DIAMOND
	
public class ColorManager : MonoBehaviour
{
    private static ColorManager m_Instance;
    public static ColorManager Instance
    {
        get
        {
            return m_Instance;
        }
    }

    private static Color32 m_Red = new Color32(255,122,122,255);
    private static Color32 m_Black = new Color32(60, 60, 60, 255);
    private static Color32 m_White = new Color32(255, 250, 238, 255);
    private static Color32 m_Purple = new Color32(252, 69, 255, 255);
    private static Color32 m_Blue = new Color32(36, 237, 255, 255);
    private static Color32 m_Azul = new Color32(54, 255, 180, 255);

    private Palette m_RedBGPalette = new Palette(m_Red, m_Black, m_White, m_Blue, m_Azul);
    private Palette m_BlackBGPalette = new Palette(m_Black,m_White,m_Red, m_Purple, m_Azul);
    private Palette m_WhiteBGPalette = new Palette(m_Black,m_Red, m_Blue, m_Blue, m_Azul);

    private Palette m_CurrentPalette;

    public Palette CurrentPalette { get { return m_CurrentPalette; } set { m_CurrentPalette = value; } }

    void Awake()
    {
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(gameObject);
        }

        m_Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        m_CurrentPalette = m_BlackBGPalette;
    }

    public void SetSectionColor(Section s)
    {
        foreach (SpriteRenderer sprite in s.GetComponentsInChildren<SpriteRenderer>())
        {
            if (sprite.gameObject.tag == "Wall")
            {
                sprite.color = m_CurrentPalette.GetWallColor;
            }
            else if (sprite.gameObject.tag == "Spike" || sprite.gameObject.tag == "Blocker")
            {
                sprite.color = m_CurrentPalette.GetObstacleColor;
            }
            else if (sprite.gameObject.tag == "Diamond")
            {
                sprite.color = m_CurrentPalette.GetDiamondColor;
            }
        }

        s.ColorSet = true;
    }

    public IEnumerator ChangeColorAfterTime(SpriteRenderer s,float delayTime, Color[] colors)
    {
        Color currentcolor = (Color)colors[UnityEngine.Random.Range(0, colors.Length)]; ;
        Color nextcolor;

        s.color = currentcolor;

        while (true)
        {
            nextcolor = (Color)colors[UnityEngine.Random.Range(0, colors.Length)];

            for (float t = 0; t < delayTime; t += Time.deltaTime)
            {
                s.color = Color.Lerp(currentcolor, nextcolor, t / delayTime);
                yield return null;
            }
            currentcolor = nextcolor;
        }
    }
}
