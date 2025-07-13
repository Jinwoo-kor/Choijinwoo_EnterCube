using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("���� ��� ������Ʈ (Player�� ���� Cube)")]
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

        // ��ġ ���󰡱�
        transform.position = targetObject.position;
        transform.rotation = targetObject.rotation;

        // �̵� ���� �Ǵ�
        Vector3 movement = targetObject.position - lastPosition;
        float speed = movement.magnitude / Time.deltaTime;

        // �ִϸ��̼� �Ķ���� ����
        if (speed > 0.01f)
        {
            animator.SetInteger("AnimationPar", 1); // �̵� ��
        }
        else
        {
            animator.SetInteger("AnimationPar", 0); // ����
        }

        lastPosition = targetObject.position;
    }
}
