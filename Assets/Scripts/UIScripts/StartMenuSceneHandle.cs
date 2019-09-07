using UnityEngine;

public class StartMenuSceneHandle : MonoBehaviour
{
    [SerializeField] private Animator pressStartAnimator;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            pressStartAnimator.SetBool("FadeOut", true);
        }
    }
}
