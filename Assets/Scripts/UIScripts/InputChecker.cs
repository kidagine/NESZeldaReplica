using UnityEngine;

public class InputChecker : MonoBehaviour
{
    public enum InputState { MouseKeyboard, Controller};
    private InputState _state = InputState.MouseKeyboard;


    public static InputChecker Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void OnGUI()
    {
        switch (_state)
        {
            case InputState.MouseKeyboard:
                if (IsControlerInput())
                {
                    _state = InputState.Controller;
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                break;
            case InputState.Controller:
                if (IsMouseKeyboard())
                {
                    _state = InputState.MouseKeyboard;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                break;
        }
    }

    public InputState GetInputState()
    {
        return _state;
    }

    private bool IsMouseKeyboard()
    {
        // mouse & keyboard buttons
        if (Event.current.isKey || Event.current.isMouse || Input.GetAxis("Mouse ScrollWheel") != 0.0f)
        {
            return true;
        }
        // mouse movement
        if (Input.GetAxis("Mouse X") != 0.0f ||
            Input.GetAxis("Mouse Y") != 0.0f)
        {
            return true;
        }
        return false;
    }

    private bool IsControlerInput()
    {
        // joystick buttons
        if (Input.GetKey(KeyCode.Joystick1Button0) ||
           Input.GetKey(KeyCode.Joystick1Button1) ||
           Input.GetKey(KeyCode.Joystick1Button2) ||
           Input.GetKey(KeyCode.Joystick1Button3) ||
           Input.GetKey(KeyCode.Joystick1Button4) ||
           Input.GetKey(KeyCode.Joystick1Button5) ||
           Input.GetKey(KeyCode.Joystick1Button6) ||
           Input.GetKey(KeyCode.Joystick1Button7) ||
           Input.GetKey(KeyCode.Joystick1Button8) ||
           Input.GetKey(KeyCode.Joystick1Button9) ||
           Input.GetKey(KeyCode.Joystick1Button10) ||
           Input.GetKey(KeyCode.Joystick1Button11) ||
           Input.GetKey(KeyCode.Joystick1Button12) ||
           Input.GetKey(KeyCode.Joystick1Button13) ||
           Input.GetKey(KeyCode.Joystick1Button14) ||
           Input.GetKey(KeyCode.Joystick1Button15) ||
           Input.GetKey(KeyCode.Joystick1Button16) ||
           Input.GetKey(KeyCode.Joystick1Button17) ||
           Input.GetKey(KeyCode.Joystick1Button18) ||
           Input.GetKey(KeyCode.Joystick1Button19))
        {
            return true;
        }

        // joystick axis
        if (Input.GetAxis("Left Stick X") != 0.0f || Input.GetAxis("Left Stick Y") != 0.0f ||
            Input.GetAxis("Right Stick X") != 0.0f || Input.GetAxis("Right Stick Y") != 0.0f ||
            Input.GetAxis("Left Trigger") != 0.0f || Input.GetAxis("Right Trigger") != 0.0f ||
            Input.GetAxis("Dpad X") != 0.0f || Input.GetAxis("Dpad Y") != 0.0f)
        {
            return true;
        }

        return false;
    }
}
