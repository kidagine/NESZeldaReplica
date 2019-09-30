using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu = default;


    void Update()
    {
        if (Input.GetButtonDown("Select"))
        {
            if (_pauseMenu.activeSelf)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Resume()
    {
        _pauseMenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1.0f;
    }

    private void Pause()
    {
        _pauseMenu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0.0f;
    }

    public void GetCurrentlySelectedElement(GameObject selectedElement)
    {
        AudioManager.Instance.Play("Slide");
        GameObject selectionBorder = selectedElement.transform.GetChild(0).gameObject;
        selectionBorder.SetActive(true);
    }

    public void GetCurrentlyUnSelectedElement(GameObject selectedElement)
    {
        GameObject selectionBorder = selectedElement.transform.GetChild(0).gameObject;
        selectionBorder.SetActive(false);
    }
}
