using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartMenuSceneHandler : MonoBehaviour
{
    [SerializeField] private Animator _fadeInOutAnimator = default;
    [SerializeField] private Animator _pressStartAnimator = default;
    [SerializeField] private GameObject _pressStart = default;
    [SerializeField] private GameObject _modeSelection = default;
    [SerializeField] private GameObject _makeSelect = default;
    [SerializeField] private TextMeshProUGUI _pressStartText = default;
    private bool _hasPressedStart;
    private bool _checkInput;


    void Awake()
    {
        _fadeInOutAnimator.SetTrigger("FadeIn");
    }

    void Update()
    {
        if (!_hasPressedStart)
        {
            PressStart();
        }
        else
        {
            SelectMode();
        }
    }

    private void PressStart()
    {
        if (_checkInput)
        {
            if (Input.GetKeyUp(KeyCode.Return))
            {
                _checkInput = false;
            }
        }
        else
        {
            if (Input.GetAxis("Left Trigger") != 0.0f && Input.GetAxis("Right Trigger") != 0.0f || Input.GetKeyDown(KeyCode.Return))
            {
                AudioManager.Instance.Play("Selection");
                _pressStartAnimator.SetBool("FadeOut", true);
                _hasPressedStart = true;
                _pressStart.SetActive(false);
                _modeSelection.SetActive(true);
                EventSystem.current.SetSelectedGameObject(_makeSelect);
            }
        }

        if (InputChecker.Instance.GetInputState() == InputChecker.InputState.MouseKeyboard)
        {
            _pressStartText.text = "Press <sprite=0>";
        }
        else if (InputChecker.Instance.GetInputState() == InputChecker.InputState.Controller)
        {
            _pressStartText.text = "Press <sprite=1>";
        }
    }

    private void SelectMode()
    {
        CheckForNoSelection();
    }

    private void CheckForNoSelection()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (InputChecker.Instance.GetInputState() == InputChecker.InputState.Controller)
            {
                EventSystem.current.SetSelectedGameObject(_makeSelect);
            }
        }
    }

    public void GetCurrentlySelectedElement(GameObject selectedElement)
    {
        Transform selectionBorder = selectedElement.transform.GetChild(0);
        AudioManager.Instance.Play("Slide");
        selectionBorder.gameObject.SetActive(true);
    }

    public void GetCurrentlyUnSelectedElement(GameObject selectedElement)
    {
        Transform selectionBorder = selectedElement.transform.GetChild(0);
        selectionBorder.gameObject.SetActive(false);
    }

    public void SelectGoBack()
    {
        AudioManager.Instance.Play("Selection");
        _hasPressedStart = false;
        _pressStart.SetActive(true);
        _modeSelection.SetActive(false);
        if (Input.GetKey(KeyCode.Return))
        {
            _checkInput = true;
        }
    }

    public void SelectMake()
    {
        AudioManager.Instance.Play("Selection");
        _fadeInOutAnimator.SetTrigger("FadeOut");
        StartCoroutine(LoadScene("MakeLevelScene"));
    }

    public void SelectPlay()
    {
        AudioManager.Instance.Play("Selection");
        // TO DO
    }

    IEnumerator LoadScene(string sceneName)
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(sceneName);
    }
}
