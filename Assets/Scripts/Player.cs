using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlatformManager platformManager;
    public PlatformSpawner platformSpawner;

    public float moveDistance = 1f;
    public float moveSpeed = 5f;
    public float moveCooldown = 0.2f;
    public float jumpHeight = 2f;
    public float jumpDuration = 2.0f; // Slower jump

    private Vector3 targetPosition;
    private bool isMoving = false;
    private float moveTimer = 0f;
    private bool isJumping = false;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        moveTimer += Time.deltaTime;

        if (!isMoving && moveTimer >= moveCooldown && !isJumping)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                HandleDirectionOnly();
            }
            else
            {
                HandleMovement();
            }
        }

        HandleColorLock();
        HandleJump();
    }

    void HandleDirectionOnly()
    {
        if (Input.GetKey(KeyCode.W)) transform.rotation = Quaternion.LookRotation(Vector3.forward);
        if (Input.GetKey(KeyCode.S)) transform.rotation = Quaternion.LookRotation(Vector3.back);
        if (Input.GetKey(KeyCode.A)) transform.rotation = Quaternion.LookRotation(Vector3.left);
        if (Input.GetKey(KeyCode.D)) transform.rotation = Quaternion.LookRotation(Vector3.right);
    }

    void HandleMovement()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) direction = Vector3.forward;
        if (Input.GetKey(KeyCode.S)) direction = Vector3.back;
        if (Input.GetKey(KeyCode.A)) direction = Vector3.left;
        if (Input.GetKey(KeyCode.D)) direction = Vector3.right;

        if (direction != Vector3.zero)
        {
            Vector3 nextPosition = transform.position + direction * moveDistance;
            if (Mathf.Abs(nextPosition.x) <= 0.5f && Mathf.Abs(nextPosition.z) <= 0.5f)
            {
                StartCoroutine(MoveTo(nextPosition));
                transform.rotation = Quaternion.LookRotation(direction);
                moveTimer = 0f;
            }
        }
    }

    IEnumerator MoveTo(Vector3 destination)
    {
        isMoving = true;
        while (Vector3.Distance(transform.position, destination) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = destination;
        isMoving = false;
    }

    void HandleColorLock()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, 0.1f);
            foreach (Collider hit in hits)
            {
                Platform platform = hit.GetComponent<Platform>();
                if (platform != null && !platform.isLocked)
                {
                    platform.LockColor();
                    if (platformManager != null)
                    {
                        platformManager.AddColorToStack(platform.GetColorCode());
                    }
                    break;
                }
            }
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (platformManager != null && platformManager.colorStack.Count == 4 && platformManager.IsStackMatched())
            {
                StartCoroutine(JumpAndTransition());
            }
        }
    }

    IEnumerator JumpAndTransition()
    {
        if (isJumping) yield break;
        isJumping = true;

        // Start platform transition immediately
        if (platformManager != null)
        {
            StartCoroutine(platformManager.TransitionToNextStage());
        }

        Vector3 startPos = transform.position;
        Vector3 peakPos = startPos + Vector3.up * jumpHeight;
        float timer = 0f;

        // Jump up
        while (timer < jumpDuration / 2f)
        {
            transform.position = Vector3.Lerp(startPos, peakPos, timer / (jumpDuration / 2f));
            timer += Time.deltaTime;
            yield return null;
        }

        // Jump down
        timer = 0f;
        while (timer < jumpDuration / 2f)
        {
            transform.position = Vector3.Lerp(peakPos, startPos, timer / (jumpDuration / 2f));
            timer += Time.deltaTime;
            yield return null;
        }

        isJumping = false;
    }
}
