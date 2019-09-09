using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class StartMenuSceneHandler : MonoBehaviour
{
    [SerializeField] private Animator _pressStartAnimator = default;
    [SerializeField] private GameObject _pressStart = default;
    [SerializeField] private GameObject _modeSelection = default;
    [SerializeField] private GameObject _makeSelect = default;
    [SerializeField] private TextMeshProUGUI _pressStartText = default;
    private bool _hasPressedStart;


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
        if (Input.GetAxis("Left Trigger") != 0.0f && Input.GetAxis("Right Trigger") != 0.0f || Input.GetKeyDown(KeyCode.Return))
        {
            AudioManager.Instance.Play("Selection");
            _pressStartAnimator.SetBool("FadeOut", true);
            _hasPressedStart = true;
            _pressStart.SetActive(false);
            _modeSelection.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_makeSelect);
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

    public void SelectGoBack()
    {
        AudioManager.Instance.Play("Selection");
        _hasPressedStart = false;
        _pressStart.SetActive(true);
        _modeSelection.SetActive(false);
    }

    public void SelectMake()
    {
        AudioManager.Instance.Play("Selection");
        // TO DO
    }

    public void SelectPlay()
    {
        AudioManager.Instance.Play("Selection");
        // TO DO
    }
}
