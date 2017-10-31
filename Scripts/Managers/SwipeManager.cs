using UnityEngine;
using System.Collections;

public enum SwipeDirection
{
    None = 0,
    Left = 1,
    Right = 2,
    Up = 4,
    Down = 8,

    LeftDown = 9,
    LeftUp   = 5,
    RightDown = 10,
    RightUp   = 6,
}

public class SwipeManager : MonoBehaviour
{

    private static SwipeManager instance;
	public static SwipeManager Instance 
	{ 
		get { 
			if (instance != null) 
			{
				return  instance;
			} 
			else 
			{
				GameObject swipemanager = new  GameObject ("SwipeManager");
				swipemanager.AddComponent<SwipeManager> ();
				instance = swipemanager.GetComponent<SwipeManager>();
				return instance;
			}
		}
	}
    private Vector3 touchPosition;
    private float swipeResX = 20.0f;
    private float swipeResY = 20.0f;

    public SwipeDirection Direction { set; get; }

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        //Reset Swipe Direction
        Direction = SwipeDirection.None;

        if (Input.GetMouseButtonDown(0))
        {
            touchPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 deltaSwipe = touchPosition - Input.mousePosition;

            if (Mathf.Abs(deltaSwipe.x) > swipeResX)
            {
                //Swipe on the X Axis
                Direction |= (deltaSwipe.x < 0) ? SwipeDirection.Right : SwipeDirection.Left;
            }

            if (Mathf.Abs(deltaSwipe.y) > swipeResY)
            {
                //Swipe on the Y Axis
                Direction |= (deltaSwipe.y < 0) ? SwipeDirection.Up : SwipeDirection.Down;
            }

        }
    }

    public bool IsSwiping(SwipeDirection dir)
    {
        return (Direction & dir) == dir;
    }
}
