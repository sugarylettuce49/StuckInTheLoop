using UnityEngine;
using System.Collections;

public class DeSpawn : MonoBehaviour
{

    public float timeFR;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(wait2Despawn());
    }

    IEnumerator wait2Despawn()
    {
        yield return new WaitForSeconds(timeFR);
        Destroy(gameObject);
        yield break;
    }
}
