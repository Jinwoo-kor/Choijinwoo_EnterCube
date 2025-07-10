using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveDistance = 1f;
    public float moveSpeed = 5f;
    public float moveCooldown = 0.2f;
    public float jumpHeight = 1f;
    public float jumpDuration = 0.5f;
    public PlatformManager platformManager;

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
            if (IsValidMove(nextPosition))
            {
                StartCoroutine(MoveTo(nextPosition));
                transform.rotation = Quaternion.LookRotation(direction);
                moveTimer = 0f;
            }
        }
    }

    bool IsValidMove(Vector3 pos)
    {
        Vector3 offset = pos - Vector3.zero;
        Vector3[] validOffsets = new Vector3[]
        {
            new Vector3(-0.5f, 0.1f, -0.5f),
            new Vector3(-0.5f, 0.1f, 0.5f),
            new Vector3(0.5f, 0.1f, 0.5f),
            new Vector3(0.5f, 0.1f, -0.5f)
        };

        foreach (Vector3 valid in validOffsets)
        {
            if (Vector3.Distance(offset, valid) < 0.1f)
                return true;
        }
        return false;
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
                    platformManager.AddColorToStack(platform.GetColorCode());
                    break;
                }
            }
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (platformManager.colorStack.Count == 4 && platformManager.IsStackMatched())
            {
                StartCoroutine(JumpAndTransition());
            }
        }
    }

    IEnumerator JumpAndTransition()
    {
        yield return StartCoroutine(JumpEffect());

        bool allBelowZero = true;
        foreach (GameObject platform in platformManager.platformSpawner.spawnedPlatforms)
        {
            if (platform != null && platform.transform.position.y > 0f)
            {
                allBelowZero = false;
                break;
            }
        }

        if (allBelowZero)
        {
            platformManager.StartCoroutine(platformManager.TransitionToNextStage());
        }
        else
        {
            Debug.Log("플랫폼이 아직 충분히 내려가지 않았습니다.");
        }
    }

    IEnumerator JumpEffect()
    {
        isJumping = true;
        float elapsed = 0f;
        Vector3 startPos = transform.position;

        while (elapsed < jumpDuration)
        {
            float t = elapsed / jumpDuration;
            float height = Mathf.Sin(Mathf.PI * t) * jumpHeight;
            transform.position = new Vector3(startPos.x, startPos.y + height, startPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = startPos;
        isJumping = false;
    }
}
