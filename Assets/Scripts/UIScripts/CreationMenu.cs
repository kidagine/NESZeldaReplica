using UnityEngine;

public class CreationMenu : MonoBehaviour
{
    [SerializeField] private GameObject _creationMenu = default; 


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_creationMenu.activeSelf)
            {
                PlayMode();
            }
            else
            {
                CreationMode();
            }
        }
    }

    private void PlayMode()
    {
        _creationMenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1.0f;
    }

    private void CreationMode()
    {
        _creationMenu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0.0f;
    }
}
