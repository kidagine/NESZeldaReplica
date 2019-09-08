using UnityEngine;
using TMPro;

public class StartMenuSceneHandle : MonoBehaviour
{
    [SerializeField] private Animator _pressStartAnimator = default;
    [SerializeField] private TextMeshProUGUI _pressStartText = default;


    void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            _pressStartAnimator.SetBool("FadeOut", true);
        }
        
        if (InputChecker.Instance.GetInputState() == InputChecker.InputState.MouseKeyboard)
        {
            _pressStartText.text = "Keyboard";
        }
        else if (InputChecker.Instance.GetInputState() == InputChecker.InputState.Controller)
        { 
            _pressStartText.text = "Controller";
        }
    }
}
