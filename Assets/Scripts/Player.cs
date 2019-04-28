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
  internal bool attacking;

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
    if (stabSelfTimer > 0)
    {
      if (Time.time > stabbedTime + (stabSelfDuration * 0.8))
      {
        GetComponent<PlayerSword>().SetSwordPower(3);
      }
      stabSelfTimer -= Time.deltaTime;
      return;
    }

    if (controller.collisions.above || controller.collisions.below)
      velocity.y = 0;

    Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

    if (attacking)
      input.x = 0;

    animator.SetBool("isRunning", input.x != 0);
    animator.SetBool("isJumping", !controller.collisions.below);

    if (Input.GetButtonDown("Jump") && controller.collisions.below)
      Jump();

    if (Input.GetButtonUp("Jump") && velocity.y > minJumpForce)
      CancelJump();

    if (Input.GetButtonDown("StabSelf") && velocity.y == 0)
      StabSelf();

    velocity.x = input.x * moveSpeed;
    velocity.y += gravity * Time.deltaTime;

    if (Mathf.Sign(velocity.x) != transform.localScale.x && velocity.x != 0)
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


  float stabSelfTimer;
  float stabbedTime;
  float stabSelfDuration = 1.08f;

  void StabSelf()
  {
    stabbedTime = Time.time;
    stabSelfTimer = stabSelfDuration;
    animator.SetTrigger("stabSelf");
  }
}
