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

  [SerializeField] int maxEssence = 4;
  [SerializeField, ReadOnly]
  int essence;

  public Vector2 input;
  void Start()
  {
    controller = GetComponent<Controller2D>();
    animator = GetComponent<Animator>();
    gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
    maxJumpForce = Mathf.Abs(gravity) * timeToJumpApex;
    minJumpForce = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
  }

  internal int DecreaseEssence(int amount)
  {
    essence -= amount;
    if (essence < 0)
      essence = 0;
    return essence;
  }

  internal int GetEssence()
  {
    return essence;
  }
  internal int IncreaseEssence(int amount)
  {
    essence += amount;
    if (essence > maxEssence)
      essence = maxEssence;
    return essence;
  }

  [SerializeField] Vector2 knockedVelocity;
  void Update()
  {
    if (knockedTimer > 0)
    {
      velocity.x = knockedVelocity.x * Mathf.Sign(transform.position.x - knockedPosition.x);
      velocity.y = knockedVelocity.y;

      knockedTimer -= Time.deltaTime;
      controller.Move(velocity * Time.deltaTime, Vector2.zero);
      return;
    }
    if (stabSelfTimer > 0)
    {
      if (Time.time > stabbedTime + (stabSelfDuration * 0.8))
      {
        essence = maxEssence;
        GetComponent<PlayerSword>().SetSwordPower(essence);
      }
      stabSelfTimer -= Time.deltaTime;
      return;
    }

    if (controller.collisions.above || controller.collisions.below)
      velocity.y = 0;

    input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

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

    if (essence == 0)
      controller.Move(velocity * Time.deltaTime, input);
    else
      controller.Move(velocity * Time.deltaTime * (1 + (essence * .2f)), input);
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
    GetComponent<Health>().DecreaseHealth(1);
    stabbedTime = Time.time;
    stabSelfTimer = stabSelfDuration;
    animator.SetTrigger("stabSelf");
  }

  internal void GetHit(Vector3 position)
  {
    GetComponent<Health>().DecreaseHealth(1);
    knockedTimer = knockedTime;
    knockedPosition = position;
  }

  bool beingKnocked;
  float knockedTimer;
  [SerializeField] float knockedTime = 0.4f;
  Vector3 knockedPosition;

}
