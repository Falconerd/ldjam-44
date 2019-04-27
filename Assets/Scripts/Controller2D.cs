using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Based on https://github.com/SebLague/2DPlatformer-Tutorial
public class Controller2D : MonoBehaviour
{
  [SerializeField]
  const float skinWidth = 0.015f;

  [SerializeField]
  int horizontalRayCount = 4;
  [SerializeField]
  int verticalRayCount = 4;
  [SerializeField]
  LayerMask collisionMask;

  float horizontalRaySpacing;
  float verticalRaySpacing;
  BoxCollider2D boxCollider;
  RaycastOrigins raycastOrigins;

  internal CollisionInfo collisions;

  void Start()
  {
    boxCollider = GetComponent<BoxCollider2D>();

    CalculateRaySpacing();
  }

  internal void Move(Vector3 velocity, Vector2 input)
  {
    UpdateRaycastorigins();
    collisions.Reset();

    if (velocity.x != 0)
      HorizontalCollisions(ref velocity);
    if (velocity.y != 0)
      VerticalCollisions(ref velocity, input);

    transform.Translate(velocity);
  }
  void HorizontalCollisions(ref Vector3 velocity)
  {
    float direction = Mathf.Sign(velocity.x);
    float rayLength = Mathf.Abs(velocity.x) + skinWidth;

    for (int i = 0; i < horizontalRayCount; i++)
    {
      Vector2 rayOrigin = direction == 1 ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
      rayOrigin += Vector2.up * (horizontalRaySpacing * i);

      RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector3.right * direction, rayLength, collisionMask);

      Debug.DrawRay(rayOrigin, Vector2.right * direction * rayLength, Color.magenta);

      if (hit)
      {
        velocity.x = (hit.distance - skinWidth) * direction;
        rayLength = hit.distance;

        collisions.left = direction == -1;
        collisions.right = direction == 1;
      }
    }
  }
  void VerticalCollisions(ref Vector3 velocity, Vector2 input)
  {
    float direction = Mathf.Sign(velocity.y);
    float rayLength = Mathf.Abs(velocity.y) + skinWidth;

    for (int i = 0; i < verticalRayCount; i++)
    {
      Vector2 rayOrigin = direction == 1 ? raycastOrigins.topLeft : raycastOrigins.bottomLeft;
      rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);

      RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * direction, rayLength, collisionMask);

      Debug.DrawRay(rayOrigin, Vector2.up * direction * rayLength, Color.magenta);

      if (hit)
      {
        velocity.y = (hit.distance - skinWidth) * direction;
        rayLength = hit.distance;

        collisions.below = direction == -1;
        collisions.above = direction == 1;

        // If we are trying to move, ignore this
        if (input.x == 0 && hit.transform.gameObject.tag == "MovingPlatform")
        {
          // velocity = hit.transform.gameObject.GetComponent<MovingPlatform>().Velocity;
        }
      }
    }
  }

  void UpdateRaycastorigins()
  {
    Bounds bounds = boxCollider.bounds;
    bounds.Expand(skinWidth * -2);

    raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
    raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
    raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
    raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
  }

  void CalculateRaySpacing()
  {
    Bounds bounds = boxCollider.bounds;
    bounds.Expand(skinWidth * -2);

    horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
    verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

    horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
    verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
  }

  internal struct RaycastOrigins
  {
    internal Vector2 topLeft, topRight;
    internal Vector2 bottomLeft, bottomRight;
  }

  internal struct CollisionInfo
  {
    internal bool above, below;
    internal bool left, right;

    public void Reset()
    {
      above = below = false;
      left = right = false;
    }
  }
}
