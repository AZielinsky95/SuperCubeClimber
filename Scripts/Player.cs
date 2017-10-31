using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {

    #region Fields
    private readonly Vector3 PLAYER_START_POSITION = new Vector3(0, -4.0f, 0);
    private enum PLAYER_STATES { LOCKED, GROUNDED, AIRBOURNE , JUMPING, WALLSLIDING }

    private PLAYER_STATES m_CurrentState = PLAYER_STATES.LOCKED;

	[SerializeField] private float JumpX;
	[SerializeField] private float JumpHeight = 3.5f;
	[SerializeField] private float gravity = -20f;
	[SerializeField] private Vector2 wallLeap;
	[SerializeField] private float wallSlideSpeedMax = 3;
    [SerializeField] private ParticleSystem m_Particles;
    [SerializeField] private LayerMask m_WallMask;
    [SerializeField] private SpriteRenderer m_PlayerSprite;
    // [SerializeField] Image m_TapIndicator;

    [SerializeField] private AudioClip m_JumpSFX;
    [SerializeField] private AudioClip m_LoseSFX;

    Controller2D controller;    
    Vector2 velocity;
    private bool gameOver;
    private int m_AvailableJumpCount = 2;
    private int counter;
    private bool m_WallHit = false;

    private int m_HighScore = 0;
    private int m_CurrentScore = 0;

    private Coroutine m_PowerUp;
    #endregion

    #region Properties
    public int GetHighScore { get { return m_HighScore; } }

    public int GetCurrentPlayerScore()
    {
        m_CurrentScore = (int)this.transform.position.y;

        if (m_CurrentScore > m_HighScore)
        {
            m_HighScore = m_CurrentScore;
        }

        return m_HighScore;
    }
    #endregion

    #region Methods

    void Start () {
        controller = GetComponent<Controller2D>();
        ResetPlayer();
        gameObject.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.x, 1);
    }

    void Rotate()
    {
        Hashtable rotation = new Hashtable();
        if (counter == 0)
            rotation.Add("z", -1080);
        else
            rotation.Add("z", 1080);

        //rotation.Add("delay", 0.1f);
        rotation.Add("easetype", iTween.EaseType.easeInSine);
        rotation.Add("time", 2.0f);
        rotation.Add("name", "playerRotate");
        iTween.RotateAdd(m_PlayerSprite.gameObject, rotation);
    }

    void TryJump()
    {
        if (m_AvailableJumpCount > 0)
        {
            velocity.y = wallLeap.y;
            m_CurrentState = PLAYER_STATES.JUMPING;
            m_WallHit = false;
            m_AvailableJumpCount--;
            //  TriggerTapIndicator();
            AudioManager.Instance.PlayOneShotSFX(m_JumpSFX, 0.5f);


            //Reminder: JumpLeft = 0  JumpRight = 1
            if (counter == 0)
            {
                velocity.x = -wallLeap.x;
                Rotate();
            }
            else
            {
                velocity.x = wallLeap.x;
                Rotate();
            }
        }
    }

    private void ResetPlayerColor()
    {
        m_PlayerSprite.color = ColorManager.Instance.CurrentPalette.GetPlayerColor;
    }

    public void ResetPlayer()
    {
        m_CurrentState = PLAYER_STATES.LOCKED;
        ResetRotation();
        transform.position = PLAYER_START_POSITION;
        velocity = Vector2.zero;
        gameOver = false;
        m_AvailableJumpCount = 2;
        counter = 0;
        ResetPlayerColor();
        m_PlayerSprite.enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        Camera.main.GetComponent<CameraFollow>().enabled = true;
    }

    private void ResetJumpCounter()
    {
        if (counter == 0)
            counter++;
        else
            counter--;

        m_AvailableJumpCount = 2;
    }

    private void HandleCollisions()
    {
        //If the player is colliding with the left or right wall -> We are wall sliding
        if (controller.collisionInfo.left || controller.collisionInfo.right && !controller.collisionInfo.below && velocity.y < 0)
        {
            m_CurrentState = PLAYER_STATES.WALLSLIDING;
            //Stop any active rotation tween!
            iTween.StopByName("playerRotate");
            //Reset rotation so it looks clean!
            m_PlayerSprite.transform.rotation = new Quaternion(0, 0, 0, 0);
          
            //This check is to make sure we only reset the jump counter once instead of every frame! 
            if (!m_WallHit)
            {
                m_WallHit = true;
                ResetJumpCounter();
                if (m_PowerUp != null)
                {
                    StopCoroutine(m_PowerUp);
                    ResetPlayerColor();
                }
                velocity.y = -0.5f;
            }

            WallSlide();
        }

        if (controller.collisionInfo.below)
        {
            m_CurrentState = PLAYER_STATES.GROUNDED;
        }
        else if (!controller.collisionInfo.IsColliding && m_CurrentState != PLAYER_STATES.JUMPING && m_CurrentState != PLAYER_STATES.LOCKED)
        {
            m_CurrentState = PLAYER_STATES.AIRBOURNE;

            velocity.x = 0;
        }
    }

    private void WallSlide()
    {
        if (velocity.y > -wallSlideSpeedMax)
        {
            velocity.y -= 0.002f;
        }
        else
        {
            velocity.y = -wallSlideSpeedMax;
        }
    }

    private void MoveController()
    {
        if(m_CurrentState!=PLAYER_STATES.WALLSLIDING)
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void Update()
    {
		if(!gameOver)
		{
            MoveController();
            HandleCollisions();

            if (Input.GetMouseButtonDown(0) && m_CurrentState != PLAYER_STATES.LOCKED)
	        {
	            TryJump();
            }
        }

	}

    private void GameOver()
    {
        gameOver = true;
        AudioManager.Instance.PlaySFX(m_LoseSFX);
        m_Particles.Play();
        m_PlayerSprite.enabled = false;
        GameManager.Instance.SetState(typeof(LoseState));
        GetComponent<BoxCollider2D>().enabled = false;
        enabled = false;
    }

    private void ResetRotation()
    {
        iTween.StopByName("playerRotate");
        transform.rotation = Quaternion.identity;
     //   m_PlayerSprite.gameObject.transform.rotation = Quaternion.identity;
    }

    void OnBecameInvisible()
    {
        if(!gameOver && m_CurrentState != PLAYER_STATES.LOCKED)
        GameOver();
    }

    //private void TriggerTapIndicator()
    //{
    //    iTween.Stop();
    //    m_TapIndicator.gameObject.transform.position = transform.position;
    //    m_TapIndicator.gameObject.SetActive(true);
    //    //Hashtable scaleArgs = new Hashtable();
    //    //scaleArgs.Add("scale", new Vector3(2.0f, 2.0f, 2.0f));
    //    //scaleArgs.Add("time", 1.0f);


    //   // m_TapIndicator.GetComponent<SpriteRenderer>().color.a
    //    Hashtable args = new Hashtable();
    //    args.Add("scale", new Vector3(5, 5, 5));
    //    args.Add("delay", 0.1f);
    //    args.Add("time", 0.4f);
    //    args.Add("oncompletetarget", gameObject);
    //    args.Add("oncomplete", "OnTapIndicatorComplete");
    //    iTween.ScaleTo(m_TapIndicator.gameObject, args);
    //    //  iTween.FadeTo(m_TapIndicator.gameObject, 0.1f, 1.0f);
    //}

    //private void OnTapIndicatorComplete()
    //{
    //    m_TapIndicator.transform.localScale = new Vector3(1, 1, 1);
    // //   m_TapIndicator.gameObject.SetActive(false);
    //}

    private void IsPlayerOutOfCamera()
    {
        if (this.gameObject.transform.position.y < Camera.main.transform.position.y - Screen.width / 2)
        {
            GameOver();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Spike")
        {
            GameOver();
        }

        if (col.gameObject.tag == "Blocker")
        {
            GameOver();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "SpawnTrigger")
        {
            if (col.transform.GetComponentInParent<Section>().SpawnTriggerEnabled)
            {
                SectionManager.Instance.SpawnSection();
                //Make sure you cant retrigger 
                col.transform.GetComponentInParent<Section>().SpawnTriggerEnabled = false;
            }
        }
        else if (col.gameObject.tag == "ExtraJump")
        {
            m_AvailableJumpCount+=20;
            m_PowerUp = StartCoroutine(ColorManager.Instance.ChangeColorAfterTime(m_PlayerSprite, 0.3f, new Color[] {
                new Color(189 / 255f, 38  / 255f, 27  / 255f),
                new Color(242 / 255f, 110 / 255f, 55  / 255f),
                new Color(209 / 255f, 188 / 255f, 49  / 255f),
                new Color(52  / 255f, 123 / 255f, 36  / 255f),
                new Color(66  / 255f, 187 / 255f, 249 / 255f),
            }
));
            col.gameObject.SetActive(false);
        }
        else if (col.gameObject.tag == "Diamond")
        {
            //TODO: PLAY COIN SOUND 
            col.gameObject.SetActive(false);
        }
    }
    #endregion
}
