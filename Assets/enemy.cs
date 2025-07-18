using UnityEngine;
using System.Collections;

public class enemy : MonoBehaviour
{
    [SerializeField] SpriteRenderer enemySprite;
    [SerializeField] Sprite[] enemySprites;
    [SerializeField] AudioSource[] Sounds;
    public float Horizontal;
    int collisions = 0;
    bool walking = false;
    IEnumerator animCoroutine;
    Rigidbody2D rb;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.05f;

    [SerializeField] float stompCheckDistance = 0.2f;
    [SerializeField] GameObject stompedPrefab;
    [SerializeField] LayerMask playerLayer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        Horizontal = 0;
        StartAnimationHandling();
    }

    public void ACTIVATEFRFR()
    {
        Horizontal = -1f;
    }

    void FixedUpdate()
    {
        walking = Horizontal != 0;
        rb.linearVelocity = new Vector2(Horizontal * 3.5f, rb.linearVelocity.y);

        if (onGround())
        {
            if (Horizontal > 0)
                enemySprite.flipX = false;
            else if (Horizontal < 0)
                enemySprite.flipX = true;
        }
    }

    void CheckForStomp(float offset)
    {
        Vector2 rayOrigin = new Vector2(transform.position.x + offset, transform.position.y) + Vector2.up * 0.25f;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, stompCheckDistance, playerLayer);
        Debug.DrawRay(rayOrigin, Vector2.up * stompCheckDistance, Color.red);

        if (hit.collider != null)
        {

            Rigidbody2D playerRb = hit.collider.GetComponent<Rigidbody2D>();
            if (playerRb != null && playerRb.linearVelocity.y < -1f)
            {
                playerRb.AddForce(transform.up * 3f, ForceMode2D.Impulse);
                Instantiate(stompedPrefab, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }

    void StartAnimationHandling()
    {
        if (animCoroutine != null)
            StopCoroutine(animCoroutine);
        animCoroutine = AnimationHandling();
        StartCoroutine(animCoroutine);
    }

    IEnumerator AnimationHandling()
    {
        CheckForStomp(0f);
        CheckForStomp(-0.5f);
        CheckForStomp(0.5f);
        
        if (onGround())
        {
            if (walking)
            {
                if (enemySprite.sprite == enemySprites[1])
                {
                    Sounds[1].Play();
                    enemySprite.sprite = enemySprites[0];
                }
                else
                {
                    Sounds[0].Play();
                    enemySprite.sprite = enemySprites[1];
                }
            }
            else
            {
                enemySprite.sprite = enemySprites[0];
            }
        }
        else
        {
            enemySprite.sprite = enemySprites[1];
        }

        yield return new WaitForSeconds(0.1f);
        StartAnimationHandling();
        yield break;
    }

    bool onGround()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius);
        foreach (Collider2D hit in hits)
        {
            if (hit != null && hit.gameObject != gameObject)
                return true;
        }
        return false;
    }
}
