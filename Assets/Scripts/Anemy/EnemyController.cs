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
        // 착지 처리
        if (!hasLanded && transform.position.y <= 0f)
        {
            hasLanded = true;

            // 중력 제거
            rb.useGravity = false;

            // y 위치 고정
            Vector3 pos = transform.position;
            pos.y = 0f;
            transform.position = pos;

            // y축 속도 제거
            rb.velocity = Vector3.zero;

            // 이동 시작
            rb.velocity = moveDirection.normalized * speed;
        }

        // 플랫폼 감지 및 중력 적용
        if (hasLanded)
        {
            CheckPlatformBelow();
        }

        // 낙하 후 소멸
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
