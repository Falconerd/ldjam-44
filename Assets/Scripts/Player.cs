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

  Animator animator;

  void Start()
  {
    controller = GetComponent<Controller2D>();
    animator = GetComponent<Animator>();
    gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
    maxJumpForce = Mathf.Abs(gravity) * timeToJumpApex;
    minJumpForce = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
  }

  void Update()
  {
    if (controller.collisions.above || controller.collisions.below)
      velocity.y = 0;

    Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

    animator.SetBool("isRunning", input.x != 0);
    animator.SetBool("isJumping", !controller.collisions.below);

    if (Input.GetButtonDown("Jump") && controller.collisions.below)
      Jump();

    if (Input.GetButtonUp("Jump") && velocity.y > minJumpForce)
      CancelJump();

    // if (Input.GetButtonDown("Attack"))


    velocity.x = input.x * moveSpeed;
    velocity.y += gravity * Time.deltaTime;

    if (Mathf.Sign(velocity.x) != transform.localScale.x)
      Flip();

    controller.Move(velocity * Time.deltaTime, input);
  }

  void Flip()
  {
    float newLocalScaleX = transform.localScale.x * -1;
    transform.localScale = new Vector3(newLocalScaleX, 1, 1);
  }

  void Jump()
  {
    velocity.y = maxJumpForce;
    animator.SetTrigger("takeOff");
  }

  void CancelJump()
  {
    velocity.y = minJumpForce;
  }
}
