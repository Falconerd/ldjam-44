using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  [SerializeField]
  float moveSpeed = 6;
  [SerializeField]
  float minJumpHeight = 1;
  [SerializeField]
  float maxJumpHeight = 4;
  [SerializeField]
  float timeToJumpApex = 0.4f;

  [SerializeField, ReadOnly]
  float gravity;
  [SerializeField, ReadOnly]
  float maxJumpForce;
  [SerializeField, ReadOnly]
  float minJumpForce;

  Vector3 velocity;

  Controller2D controller;

  void Start()
  {
    controller = GetComponent<Controller2D>();
    gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
    maxJumpForce = Mathf.Abs(gravity) * timeToJumpApex;
    minJumpForce = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
  }

  void Update()
  {
    if (controller.collisions.above || controller.collisions.below)
      velocity.y = 0;

    Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

    if (Input.GetButtonDown("Jump") && controller.collisions.below)
      velocity.y = maxJumpForce;

    if (Input.GetButtonUp("Jump") && velocity.y > minJumpForce)
      velocity.y = minJumpForce;

    velocity.x = input.x * moveSpeed;
    velocity.y += gravity * Time.deltaTime;
    controller.Move(velocity * Time.deltaTime, input);
  }
}
