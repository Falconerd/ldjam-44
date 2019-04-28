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

  void Update()
  {
    if (timeBtwAttack <= 0)
    {
      if (Input.GetButtonDown("Attack"))
      {
        lastAttackTime = Time.time;
        animator.SetTrigger("attack");
        animator.SetBool("isAttacking", true);
        GetComponent<PlayerSword>().DecreaseSwordPower();
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackTransform.position, attackRange, layerMask);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
          // Check if we hit the correct side of the enemy.
          Enemy enemy = enemiesToDamage[i].GetComponent<Enemy>();
          if (enemy.transform.localScale == transform.localScale)
          {
            // facing same direction, so must have hit the back
            Instantiate(sparksPrefab, enemy.transform.position, Quaternion.identity);
          }
          else
            enemy.TakeDamage(damage);
        }
        if (enemiesToDamage.Length > 0)
          StartCoroutine(cameraShake.Shake(.15f, .04f));
        timeBtwAttack = startTimeBtwAttack;
      }
    }
    else
      timeBtwAttack -= Time.deltaTime;

    if (Time.time >= lastAttackTime + attackAnimLength)
      animator.SetBool("isAttacking", false);
  }

  void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(attackTransform.position, attackRange);
  }
}
