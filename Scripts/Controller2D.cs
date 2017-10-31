using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour {

    const float boxWidth = 0.1f;
    [SerializeField] private int horizontalRayCount = 4;
    [SerializeField] private int verticalRayCount = 4;
    public LayerMask collisionMask;
    float horizontalRaySpacing;
    float verticalRaySpacing;

    BoxCollider2D m_Collider;
    RaycastOrigins raycastOrigins;
    public ColInfo collisionInfo;

    public struct ColInfo
    {
        public bool above, below, left, right;
        public int faceDir;


        public bool IsColliding { get
        {
                if (above || below || left || right)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }   
         }

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }

    void Start() 
    {
        m_Collider = GetComponent<BoxCollider2D>();
        collisionInfo.faceDir = 1;
        CalculateRaySpacing();
    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = collisionInfo.faceDir;
        float rayLength = Mathf.Abs(velocity.x) + boxWidth;

        if (Mathf.Abs(velocity.x) < boxWidth)
        {
            rayLength = 2 * boxWidth;
        }
        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                velocity.x = (hit.distance - boxWidth) * directionX;
                rayLength = hit.distance;

                collisionInfo.left = directionX == -1;
                collisionInfo.right = directionX == 1;
            }
        }



    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + boxWidth;
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - boxWidth) * directionY;
                rayLength = hit.distance;

                collisionInfo.below = directionY == -1;
                collisionInfo.above = directionY == 1;
            }
        }
    

        
    }

    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        collisionInfo.Reset();

        if (velocity.x != 0)
        {
            collisionInfo.faceDir = (int)Mathf.Sign(velocity.x);
        }

        HorizontalCollisions(ref velocity);
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }

        transform.Translate(velocity);
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = m_Collider.bounds;
        bounds.Expand(boxWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = m_Collider.bounds;
        bounds.Expand(boxWidth * - 2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y/(horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x/(verticalRayCount - 1);
    
    }

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

}
