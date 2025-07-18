using UnityEngine;

public class AutoSetCollider : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        collider.size = sprite.size;
        collider.offset = new Vector2(sprite.size.x / 2, (sprite.size.y / 2) * -1);
    }
}
