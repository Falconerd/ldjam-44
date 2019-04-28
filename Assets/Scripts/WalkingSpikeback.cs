using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingSpikeback : MonoBehaviour
{
  [SerializeField]
  float speed = 4;
  [SerializeField]
  LayerMask collisionMask;
  float direction = 1; // Right

  void Update()
  {
    // Check if hit wall
    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * direction, 0.3f, collisionMask);
    if (hit)
      Reverse();

    // Check for edge
    hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, collisionMask);
    if (!hit)
    {
      Reverse();
      // Move an extra frame so we don't get stuck on the edge
      transform.Translate(new Vector2(direction * speed * Time.deltaTime, 0));
    }


    Debug.DrawRay(transform.position, Vector2.right * direction * 0.3f, Color.red, Time.deltaTime);
    Debug.DrawRay(transform.position, Vector2.down * 1f, Color.red, Time.deltaTime);
    // Walk forard until hitting a wall or edge, then change direction.
    transform.Translate(new Vector2(direction * speed * Time.deltaTime, 0));
  }

  void Reverse()
  {
    direction *= -1;
    float newLocalScaleX = transform.localScale.x * -1;
    transform.localScale = new Vector3(newLocalScaleX, 1, 1);
  }
}
