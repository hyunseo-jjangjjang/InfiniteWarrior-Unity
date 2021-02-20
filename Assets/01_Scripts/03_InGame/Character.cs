using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected int hp = 3;
    private float attackDistance = 1.0f;

    private Animator animator;
    private Rigidbody2D playerRigidbody;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();    
    }
    public int GetHP()
    {
        return hp;
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, attackDistance);
        Debug.DrawRay(transform.position, transform.right * attackDistance, Color.red, 0.3f);

        if (hit)
        {
            if (hit.collider.CompareTag("monster"))
            {
                GameObject checkObject = hit.collider.gameObject;
                IMonster monster = Monster.CheckMonster(checkObject);
                monster.Hit();
            }
        }
    }

    public void Jump()
    {
        animator.SetTrigger("Jump");
        playerRigidbody.AddForce(Vector2.up * 220.0f);
    }

    public void Idle()
    {
        animator.SetTrigger("Idle");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("ground"))
        {
            InGameEventService.Instance.enterGroundEvent();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("obstacle"))
        {
            hp -= 1;
            InGameEventService.Instance.hitCharacterEvent();
        }
        else if(collision.CompareTag("dieObstacle"))
        {
            InGameEventService.Instance.dieCharacterEvent();
        }
    }
}
