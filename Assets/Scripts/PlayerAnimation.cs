using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("따라갈 대상 오브젝트 (Player가 붙은 Cube)")]
    public Transform targetObject;

    private Animator animator;
    private Vector3 lastPosition;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        if (targetObject != null)
        {
            transform.position = targetObject.position;
            lastPosition = targetObject.position;
        }
    }

    void Update()
    {
        if (targetObject == null || animator == null) return;

        // 위치 따라가기
        transform.position = targetObject.position;
        transform.rotation = targetObject.rotation;

        // 이동 여부 판단
        Vector3 movement = targetObject.position - lastPosition;
        float speed = movement.magnitude / Time.deltaTime;

        // 애니메이션 파라미터 설정
        if (speed > 0.01f)
        {
            animator.SetInteger("AnimationPar", 1); // 이동 중
        }
        else
        {
            animator.SetInteger("AnimationPar", 0); // 멈춤
        }

        lastPosition = targetObject.position;
    }
}
