using UnityEngine;
using UnityEngine.InputSystem; 

public class Turret : MonoBehaviour
{
    private static WaitForSeconds _waitForSeconds1 = new WaitForSeconds(1f);
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform parentContainer;

    void Start()
    {
        StartCoroutine(ShootBullets());
    }

    private System.Collections.IEnumerator ShootBullets()
    {
        while (true)
        {
            if (GameManager.Instance.isFirstTap && !GameManager.Instance.isFinishGame)
            {
                Instantiate(bulletPrefab, firePoint.position + transform.forward, transform.rotation, parent: parentContainer.transform);
            }
            yield return _waitForSeconds1; 
        }
    }

    void Update()
    {
        if (!GameManager.Instance.isFirstTap || GameManager.Instance.isFinishGame)
        {
            return;
        }
        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 tapPosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(tapPosition);
            Plane groundPlane = new(Vector3.up, transform.position);
            if (groundPlane.Raycast(ray, out float rayDistance))
            {
                Vector3 targetPoint = ray.GetPoint(rayDistance);
                Vector3 direction = targetPoint - transform.position;
                direction.y = 0; 

                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);
                }
            }
        }
    }
}