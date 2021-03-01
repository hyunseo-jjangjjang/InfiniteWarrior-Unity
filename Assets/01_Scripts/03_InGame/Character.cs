using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected int hp = 3;
    private float attackDistance = 1.0f;

    private Animator animator;
    private Rigidbody2D playerRigidbody;
    private SpriteRenderer spriteRenderer;
    private Collider2D characterCollider;

    public Transform particlePosition;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();    
        spriteRenderer = GetComponent<SpriteRenderer>();
        characterCollider = GetComponent<Collider2D>();
    }
    public int GetHP()
    {
        return hp;
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up, transform.right, attackDistance);
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
        playerRigidbody.AddForce(Vector2.up * 5.2f, ForceMode2D.Impulse);
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
            
            ParticleSystem particle = ParticleFactory.Instance.CreateParticle(ParticleType.Jump);
            particle.transform.position = particlePosition.position;
            particle.transform.parent = collision.collider.gameObject.transform;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("obstacle") || collision.CompareTag("monster"))
        {
            hp -= 1;
            InGameEventService.Instance.hitCharacterEvent();
            InGameEventService.Instance.cameraShake();
        }
        else if(collision.CompareTag("dieObstacle"))
        {
            InGameEventService.Instance.dieCharacterEvent();
            InGameEventService.Instance.cameraShake();
        }
    }
    public IEnumerator StartHitAnimation()
    {
        InGameEventService.Instance.cameraShake();
        for (int i = 0;i < 5; i++)
        {
            spriteRenderer.color = new Color(1.0f, 0.4f, 0.4f);
            yield return new WaitForSeconds(0.05f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
