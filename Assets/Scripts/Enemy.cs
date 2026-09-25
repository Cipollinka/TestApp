using UnityEngine;
using UnityEngine.UI; 

public class Enemy : MonoBehaviour
{
    bool isSeeingPlayer = false, isAttacking = false, isIdle = true;
    Animator animator;
    Transform player;

    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float stoppingDistance = 0.6f;

    [SerializeField] private int health = 100;
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject boomPrefab;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        healthBar.transform.parent.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (GameManager.Instance.isFinishGame || !GameManager.Instance.isFirstTap)
        {
            return;
        }
        if (isSeeingPlayer && !isAttacking && player != null)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.y = 0f;

            if (directionToPlayer.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(directionToPlayer);

                float distance = directionToPlayer.magnitude;
                if (distance > stoppingDistance)
                {
                    transform.position += directionToPlayer.normalized * chaseSpeed * Time.deltaTime;
                }
            }

            animator.SetBool("running", true);
            animator.SetBool("attack", false);
            isIdle = false;
        }
        else if (isIdle && !isAttacking)
        {
            transform.position += Vector3.back * GameManager.Instance.velocity * Time.deltaTime;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Instantiate(boomPrefab, transform.position, Quaternion.identity);
            GameManager.Instance.countKilledEnemies++;
            Destroy(gameObject);
        }
        healthBar.transform.parent.gameObject.SetActive(true);
        healthBar.fillAmount = (float)health / 100f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit");
            isAttacking = true;
            animator.SetBool("running", false);
            animator.SetBool("attack", true);
            Vector3 directionAwayFromPlayer = transform.position - collision.transform.position;
            directionAwayFromPlayer.y = 0f;
            TakeDamage(33);
            GameManager.Instance.TakeDamage(5);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isAttacking = false;
            animator.SetBool("running", true);
            animator.SetBool("attack", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            isSeeingPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isSeeingPlayer = false;
            player = null;
            isIdle = true;
        }
    }
}
