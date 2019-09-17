using UnityEngine;

public class CreationMenu : MonoBehaviour
{
    [SerializeField] private GameObject _creationMenu = default; 


    void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            if (_creationMenu.activeSelf)
            {
                _creationMenu.SetActive(false);
            }
            else
            {
                _creationMenu.SetActive(true);
            }
        }
    }
}
