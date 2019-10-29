using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LinkAnimationEvents : MonoBehaviour
{
    [SerializeField] private Animator _fadeInOutAnimator = default;
    [SerializeField] private Animator _linkAnimation = default;


    public void WalkToRoom()
    {
        //_linkAnimation.enabled = true;
        //_linkAnimation.SetTrigger("WalkToRoom");
    }

    public void RestartEvent()
    {
        StartCoroutine(FadeToBlack());
    }

    IEnumerator FadeToBlack()
    {
        _fadeInOutAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
