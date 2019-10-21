using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOnObject : MonoBehaviour
{
    [SerializeField] GameObject _objectToFollow = default;


    void Update()
    {
        Vector2 uiPosition = Camera.main.WorldToScreenPoint(this.transform.position);

    }
}
