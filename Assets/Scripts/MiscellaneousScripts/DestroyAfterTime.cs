using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float _timeToDestroy = default; 

    void Start()
    {
        Destroy(gameObject, _timeToDestroy);
    }
}
