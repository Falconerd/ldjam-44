using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
  [SerializeField]
  int damage = 1;
  [SerializeField]
  float startTimeBtwAttack = 0.3f;
  [SerializeField]
  float attackRange;
  [SerializeField]
  Transform attackTransform;
  [SerializeField]
  LayerMask layerMask;
  [SerializeField, ReadOnly]
  float timeBtwAttack;
  [SerializeField]
  Animator animator;

  float lastAttackTime;
  [SerializeField] float attackAnimLength;
  [SerializeField]
  CameraShake cameraShake;

  [SerializeField] GameObject sparksPrefab;
  float lastHitTime;

  Player player;

  public AudioClip attackSound;
  public AudioClip invalidHitSound;
  public AudioClip hitSound;
  public AudioSource audioSource;
  public AudioSource slashSource;

  void Start()
  {
    player = GetComponent<Player>();
    audioSource = GetComponent<AudioSource>();
  }
  void Update()
  {
    if (timeBtwAttack <= 0)
    {
      if (Input.GetButtonDown("Attack"))
      {
        bool invalidHit = false;
        slashSource.clip = attackSound;
        slashSource.Play();
        lastAttackTime = Time.time;
        animator.SetTrigger("attack");
        animator.SetBool("isAttacking", true);
        GetComponent<Player>().attacking = true;
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackTransform.position, attackRange, layerMask);
        List<Enemy> uniqueEnemyList = new List<Enemy>();
        int essence = player.GetEssence();
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
          // Check if we hit the correct side of the enemy.
          Enemy enemy = enemiesToDamage[i].GetComponent<Enemy>();
          if (uniqueEnemyList.Contains(enemy))
            continue;

          uniqueEnemyList.Add(enemy);
          int damageGiven = enemy.TakeDamage(damage + essence, gameObject);
          if (damageGiven > 0)
          {
            if (Time.time - 1 < lastHitTime)
            {
              Debug.Log("Hit an enemy within 1s");
              // add some essence or whatever
              player.IncreaseEssence(1);
              GetComponent<PlayerSword>().IncreaseSwordPower();
            }
            lastHitTime = Time.time;
          }
          else
            invalidHit = true;
        }

        if (enemiesToDamage.Length > 0)
        {
          StartCoroutine(cameraShake.Shake(.15f, .04f));
          audioSource.clip = hitSound;
          audioSource.Play();
        }
        else
        {
          player.DecreaseEssence(1);
          GetComponent<PlayerSword>().DecreaseSwordPower();
        }

        if (invalidHit)
        {
          audioSource.clip = invalidHitSound;
          audioSource.Play();
        }

        timeBtwAttack = startTimeBtwAttack;
      }
    }
    else
      timeBtwAttack -= Time.deltaTime;

    if (Time.time >= lastAttackTime + attackAnimLength)
    {
      animator.SetBool("isAttacking", false);
      GetComponent<Player>().attacking = false;
    }
  }

  void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(attackTransform.position, attackRange);
  }
}
