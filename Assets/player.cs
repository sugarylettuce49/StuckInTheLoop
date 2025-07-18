using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class player : MonoBehaviour
{
    public float TimeUsed = 0f;
    public int Keys = 0;
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] Sprite[] playerSprites;
    [SerializeField] AudioSource[] Sounds;
    [SerializeField] GameObject[] Prefabs;
    [SerializeField] TextMeshProUGUI TimerText;
    [SerializeField] TextMeshPro LeaderBoard;
    int collisions = 0;
    bool walking = false;
    IEnumerator animCoroutine;
    Rigidbody2D rb;
    [SerializeField] GameObject deathScreen;
    GameObject theGuyWhoTouchedMe;
    public GameObject needMoreKeys;
    public GameObject portal;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.05f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        StartAnimationHandling();

        if (!PlayerPrefs.HasKey("Wins"))
        {
            PlayerPrefs.SetInt("Wins", 0);
        }
        else
        {
            float top1 = 999;
            float top2 = 999;
            float top3 = 999;
            for (int i = 0; i < PlayerPrefs.GetInt("Wins"); i++)
            {
                float score = PlayerPrefs.GetFloat("Game" + (i + 1));

                if (score < top1)
                {
                    top3 = top2;
                    top2 = top1;
                    top1 = score;
                }
                else if (score < top2)
                {
                    top3 = top2;
                    top2 = score;
                }
                else if (score < top3)
                {
                    top3 = score;
                }
            }
            string top1s = (top1 == 999) ? "Empty" : top1.ToString("F2");
            string top2s = (top2 == 999) ? "Empty" : top2.ToString("F2");
            string top3s = (top3 == 999) ? "Empty" : top3.ToString("F2");
            LeaderBoard.text = "Leaderboard\n1st. " + top1s + "\n2nd. " + top2s + "\n3rd. " + top3s;
        }
    }

    // Update is called once per frame
    void Update()
    {
        walking = Input.GetAxis("Horizontal") != 0;
    }

    void FixedUpdate()
    {
        if ((Input.GetAxis("Horizontal") != 0) && (TimeUsed == 0f))
        {
            TimeUsed += 0.01f;
            StartCoroutine(Timer());
        }
        rb.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * 5f, rb.linearVelocity.y);

        if (onGround())
        {
            if (Input.GetAxis("Horizontal") > 0)
                playerSprite.flipX = false;
            else if (Input.GetAxis("Horizontal") < 0)
                playerSprite.flipX = true;
        }

        
        if ((onGround()) && (Input.GetAxis("Vertical") > 0.1f))
        {
            Sounds[0].Play();
            rb.AddForce(transform.up * 7f, ForceMode2D.Impulse);
        }

        if (Keys >= 5)
        {
            portal.SetActive(true);
            needMoreKeys.SetActive(false);
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
        if (onGround())
        {
            if (walking)
            {
                if (playerSprite.sprite == playerSprites[1])
                {
                    Sounds[1].Play();
                    playerSprite.sprite = playerSprites[0];
                }
                else
                {
                    Sounds[0].Play();
                    playerSprite.sprite = playerSprites[1];
                }
            }
            else
            {
                playerSprite.sprite = playerSprites[0];
            }
        }
        else if (!onGround())
        {
            playerSprite.sprite = playerSprites[1];
        }
        yield return new WaitForSeconds(0.1f);
        StartAnimationHandling();
        yield break;
    }

    IEnumerator Timer()
    {
        int StartTime = 60;
        for (int i = 0; i < StartTime; i++)
        {
            for (int cs = 0; cs < 100; cs++)
            {
                yield return new WaitForSeconds(0.012f);
                TimeUsed += 0.01f;
                TimerText.text = "Time Left: " + (Mathf.Round((StartTime - TimeUsed) * 100f) / 100f);
            }
        }
        TimerText.text = "Time's UP!";
        Die();
        yield break;
    }

    void OnCollisionEnter2D(Collision2D thing)
    {
        collisions += 1;
        if (thing.gameObject.CompareTag("Key"))
        {
            Instantiate(Prefabs[0], thing.gameObject.transform.position, thing.gameObject.transform.rotation);
            Destroy(thing.gameObject);
            Keys += 1;
        }
        if (thing.gameObject.CompareTag("Enemy"))
        {
            theGuyWhoTouchedMe = thing.gameObject;
            StartCoroutine(wait2CheckDeath());
        }
        if (thing.gameObject.CompareTag("Portal"))
        {
            PlayerPrefs.SetInt("Wins", PlayerPrefs.GetInt("Wins") + 1);
            PlayerPrefs.SetFloat("Game" + PlayerPrefs.GetInt("Wins"), TimeUsed);
            SceneManager.LoadScene("Win");
        }
    }

    void OnCollisionExit2D()
    {
        collisions -= 1;
    }

    IEnumerator wait2CheckDeath()
    {
        yield return new WaitForSeconds(0.1f);
        if (theGuyWhoTouchedMe != null)
            Die();
        yield break;
    }

    void Die()
    {
        StopCoroutine(animCoroutine);
        Sounds[2].Play();
        playerSprite.sprite = playerSprites[2];
        deathScreen.SetActive(true);
        StartCoroutine(wait2Reset());
    }

    IEnumerator wait2Reset()
    {
        yield return new WaitForSeconds(1.6f);
        SceneManager.LoadScene("Title");
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
