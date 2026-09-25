using UnityEngine;
using UnityEngine.InputSystem; 

public class Bullet : MonoBehaviour
{
    float speed = 20f;

    void Update()
    {
        if (GameManager.Instance.isFinishGame)
        {
            return;
        }
        transform.Translate(speed * Time.deltaTime * Vector3.forward);
        if (transform.position.z > 50f || transform.position.z < -50f || transform.position.x > 50f || transform.position.x < -50f)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(50);
            Destroy(gameObject);

        }
    }

}