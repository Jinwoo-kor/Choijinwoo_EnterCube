using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Vector3 moveDirection;
    public float speed = 3f;

    private Rigidbody rb;
    private bool hasLanded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
    }

    void Update()
    {
        // ���� ó��
        if (!hasLanded && transform.position.y <= 0f)
        {
            hasLanded = true;

            // �߷� ����
            rb.useGravity = false;

            // y ��ġ ����
            Vector3 pos = transform.position;
            pos.y = 0f;
            transform.position = pos;

            // y�� �ӵ� ����
            rb.velocity = Vector3.zero;

            // �̵� ����
            rb.velocity = moveDirection.normalized * speed;
        }

        // �÷��� ���� �� �߷� ����
        if (hasLanded)
        {
            CheckPlatformBelow();
        }

        // ���� �� �Ҹ�
        if (transform.position.y <= -20f ||
            transform.position.x <= -30f || transform.position.x >= 30f ||
            transform.position.z <= -30f || transform.position.z >= 30f)
        {
            Destroy(gameObject);
        }


    }

    void CheckPlatformBelow()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 1f))
        {
            if (hit.collider.CompareTag("Platform"))
            {
                rb.useGravity = true;
            }
            else
            {
                rb.useGravity = false;
            }
        }
        else
        {
            rb.useGravity = false;
        }
    }
}
