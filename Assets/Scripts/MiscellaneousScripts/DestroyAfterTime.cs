using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = default; 

    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }
}
