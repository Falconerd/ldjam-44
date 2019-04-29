using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// From: https://www.youtube.com/watch?v=WL_PaUyRAXQ
public class CameraFollow : MonoBehaviour
{
  [SerializeField] BoxCollider2D target;
  [SerializeField] Vector2 focusAreaSize;
  public Player player;

  public float VerticalOffset;
  public float lookAheadDistanceX;
  public float lookSmoothTimeX;
  public float verticalSmoothTime;

  FocusArea focusArea;

  float currentLookAheadX;
  float targetLookAheadX;
  float lookAheadDirectionX;
  float smoothLookVelocityX;
  float smoothVelocityY;

  bool lookAheadStopped;

  BoxCollider2D levelBounds;
  void Start()
  {
    focusArea = new FocusArea(target.bounds, focusAreaSize);
    levelBounds = GetComponent<BoxCollider2D>();
  }

  void LateUpdate()
  {
    focusArea.Update(target.bounds);

    Vector2 focusPosition = focusArea.centre + Vector2.up * VerticalOffset;

    if (focusArea.velocity.x != 0)
    {
      lookAheadDirectionX = Mathf.Sign(focusArea.velocity.x);
      if (Mathf.Sign(player.input.x) == Mathf.Sign(focusArea.velocity.x) && player.input.x != 0)
      {
        targetLookAheadX = lookAheadDirectionX * lookAheadDistanceX;
        lookAheadStopped = false;
      }
      else
      {
        if (!lookAheadStopped)
        {
          targetLookAheadX = currentLookAheadX + (lookAheadDirectionX * lookAheadDistanceX - currentLookAheadX) / 4;
          lookAheadStopped = true;
        }
      }
    }

    targetLookAheadX = lookAheadDirectionX * lookAheadDistanceX;
    currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);

    focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);
    focusPosition += Vector2.right * currentLookAheadX;

    transform.position = (Vector3)focusPosition + Vector3.forward * -10;

    // Get edges
  }

  void OnDrawGizmos()
  {
    Gizmos.color = new Color(1, 0, 0, 0.5f);
    Gizmos.DrawCube(focusArea.centre, focusAreaSize);
  }

  struct FocusArea
  {
    public Vector2 centre;
    public Vector2 velocity;
    float left, right;
    float top, bottom;

    public FocusArea(Bounds targetBounds, Vector2 size)
    {
      left = targetBounds.center.x - size.x / 2;
      right = targetBounds.center.x + size.x / 2;
      bottom = targetBounds.min.y;
      top = targetBounds.min.y + size.y;

      velocity = Vector2.zero;
      centre = new Vector2((left + right) / 2, (top + bottom) / 2);
    }

    public void Update(Bounds targetBounds)
    {
      float shiftX = 0;
      if (targetBounds.min.x < left)
        shiftX = targetBounds.min.x - left;
      else if (targetBounds.max.x > right)
        shiftX = targetBounds.max.x - right;

      left += shiftX;
      right += shiftX;

      float shiftY = 0;
      if (targetBounds.min.y < bottom)
        shiftY = targetBounds.min.y - bottom;
      else if (targetBounds.max.y > top)
        shiftY = targetBounds.max.y - top;

      top += shiftY;
      bottom += shiftY;

      centre = new Vector2((left + right) / 2, (top + bottom) / 2);
      velocity = new Vector2(shiftX, shiftY);
    }
  }
}
