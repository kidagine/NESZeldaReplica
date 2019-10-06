using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private GameObject _pfbBombExplosion = default;


    void Start()
    {
        StartCoroutine(Explode());    
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(1.0f);
        Instantiate(_pfbBombExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
