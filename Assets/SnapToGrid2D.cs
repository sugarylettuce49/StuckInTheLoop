using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class SnapToGrid2D : MonoBehaviour
{
    public float gridSize = 1f;
    private Vector3 lastPosition;

    private void OnValidate()
    {
        Snap();
    }

    private void Update()
    {
        if (!Application.isPlaying && transform.position != lastPosition)
        {
            Snap();
        }
    }

    private void Snap()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Round(pos.x / gridSize) * gridSize;
        pos.y = Mathf.Round(pos.y / gridSize) * gridSize;
        pos.z = 0f;
        transform.position = pos;
        lastPosition = transform.position;
    }
}
#endif
