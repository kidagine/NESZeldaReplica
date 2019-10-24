using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LinkAnimationEvents : MonoBehaviour
{
    [SerializeField] private Animator _fadeInOutAnimator;

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
