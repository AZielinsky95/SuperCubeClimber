using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
public class SuperCube : MonoBehaviour {

	[Header("Super Cube Properties")]
	[SerializeField] private float JumpX;
	[SerializeField] private float JumpHeight = 3.5f;
	[SerializeField] private float gravity = -20f;
	[SerializeField] private Vector2 wallLeap;
	[SerializeField] private float wallSlideSpeedMax = 3;
	[SerializeField] private float m_MovementSpeed = 2;


	private bool m_IsMovingRight = false;
	private bool m_IsMovingLeft = false;
	private bool m_FirstTouch = false;

	private Vector2 m_Velocity;
	private bool gameWon;
	private bool gameOver;
	private bool wallSliding;
	private bool facingLeft;
	private Vector2 startFaceDir;

	Controller2D controller;

	void Start () 
	{
		//Get Controller Script
		controller = GetComponent<Controller2D>();
	}

	void Rotate()
	{
		Hashtable rotation = new Hashtable();
		if (m_IsMovingRight)
			rotation.Add("z", 90);
		else
			rotation.Add("z", -180);

		rotation.Add("delay", 0.125f);
		rotation.Add("easetyoe", iTween.EaseType.linear);
		rotation.Add("time", 0.7f);
		iTween.RotateAdd(gameObject.transform.GetChild(0).gameObject, rotation);
	}

	private void JumpLeft()
	{
		if (wallSliding)
		{
			m_Velocity.y = wallLeap.y;
			m_Velocity.x = -wallLeap.x;
			wallSliding = false;
			Rotate ();
		}

		if (controller.collisionInfo.below)
		{
			wallSliding = false;
			//Jump
			m_Velocity.y = JumpHeight;
			m_Velocity.x = -JumpX;
			Rotate ();
		}

	}

	void JumpRight()
	{
		if (wallSliding && !m_IsMovingRight)
		{
			m_Velocity.y = wallLeap.y;
			m_Velocity.x = wallLeap.x;
			wallSliding = false;
			//Rotate ();
		}

		if (controller.collisionInfo.below)
		{
			wallSliding = false;
			m_Velocity.y = JumpHeight;
			m_Velocity.x = JumpX;
			//Rotate ();
		}
	}

	private void MoveRight()
	{
		m_IsMovingRight = true;
		m_IsMovingLeft = false;
		Debug.Log ("MoveRight Breh");
		m_Velocity.x = m_MovementSpeed;
	}

	private void MoveLeft()
	{
		m_IsMovingLeft = true;
		m_IsMovingRight = false;

		m_Velocity.x = -m_MovementSpeed;
		Debug.Log ("MoveLeft Breh");
	}



	private void SuperCubeControls()
	{
		Vector3 mousePosition = Input.mousePosition;

		if (Mathf.Abs (mousePosition.x) > Screen.width / 2) 
		{
			//TODO: If not moving, add velocity along x  
			//If already in motion then JUMP & ROTATE!
			//If Wallsliding jump
			if (controller.collisionInfo.below) 
			{
				if (m_IsMovingLeft == true && !wallSliding) 
				{
					MoveRight();
				} 				
				else if (m_IsMovingRight) 
				{
					JumpRight ();
				}
				else if(!m_FirstTouch)
				{
					m_FirstTouch = true;
					MoveRight();
				}
			}
		}
		else if( Mathf.Abs(mousePosition.x) < Screen.width / 2)
		{
			if (controller.collisionInfo.below) 
			{
				if (m_IsMovingRight == true && !wallSliding) {
					MoveLeft ();
				} else if (m_IsMovingLeft) {
					JumpLeft ();
				} else if (!m_FirstTouch) {
					m_FirstTouch = true;
					MoveLeft ();
				} else if (m_IsMovingRight && wallSliding) 
				{
					Debug.Log ("WALLJUMP");
					JumpLeft ();
				}
			}
		}
	}

	void FixedUpdate()
	{
		if(!gameOver)
		{
			//IF PLAYER IS COLLIDING WITH EITHER WALL .. WALLSLIDING = TRUE
			if ((controller.collisionInfo.left || controller.collisionInfo.right) && !controller.collisionInfo.below && m_Velocity.y < 0)
			{
				wallSliding = true;
				Debug.Log ("WallSliding");
				m_Velocity.x = 0;

				if (m_Velocity.y < -wallSlideSpeedMax)
				{
					m_Velocity.y = -wallSlideSpeedMax;
				}	          
			}

			if (Input.GetMouseButtonDown(0))
			{
				SuperCubeControls ();
			}

			m_Velocity.y += gravity * Time.deltaTime;
			controller.Move(m_Velocity * Time.deltaTime);
			//Debug.Log ("Cube Velocity : " + m_Velocity);
		}

	}
}
