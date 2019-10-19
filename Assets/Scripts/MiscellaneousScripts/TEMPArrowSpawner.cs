using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMPArrowSpawner : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    private GameObject _arrow;

    void Start()
    {
        StartCoroutine(SpawnArrow());
    }

    IEnumerator SpawnArrow()
    {
        yield return new WaitForSeconds(1f);
        _arrow = Instantiate(arrow, GetObjectPosition(), GetObjectRotation());
        StartCoroutine(SpawnArrow());
    }

    private Vector2 GetObjectPosition()
    {
        Vector2 prefabPosition;
            prefabPosition = new Vector2(transform.position.x + 0.06f, transform.position.y - 1.0f);
        return prefabPosition;
    }

    private Quaternion GetObjectRotation()
    {
        Quaternion prefabRotation;
            prefabRotation = Quaternion.Euler(0, 0, 270);
        return prefabRotation;
    }
}
