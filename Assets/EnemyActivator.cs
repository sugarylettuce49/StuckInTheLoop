using UnityEngine;

public class EnemyActivator : MonoBehaviour
{
    [SerializeField] enemy Enemy;

    void OnTriggerEnter2D(Collider2D thing)
    {
        if (thing.gameObject.name == "Player")
        {
            Enemy.ACTIVATEFRFR();
            Destroy(gameObject);
        }
    }
}
