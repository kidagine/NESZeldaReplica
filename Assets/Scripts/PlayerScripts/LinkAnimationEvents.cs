using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LinkAnimationEvents : MonoBehaviour
{
    [SerializeField] private LinkLOZMovement _linkMovement = default;
    [SerializeField] private Animator _fadeInOutAnimator = default;
    [SerializeField] private Animator _linkAnimator = default;


    public void WalkToRoom(Vector2 playerDirection)
    {
        _linkMovement._cantMove =  true;
        _linkAnimator.speed = 1;
        _linkAnimator.SetFloat("Horizontal", playerDirection.x);
        _linkAnimator.SetFloat("Vertical", playerDirection.y);
        _linkAnimator.SetTrigger("WalkToRoom");
    }

    public void ResetMovement()
    {
        _linkMovement._cantMove = false;
        GameObject player = _linkMovement.REMOVETHIS();
        player.transform.position = gameObject.transform.position;
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
