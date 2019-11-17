using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LinkAnimationEvents : MonoBehaviour
{
    [SerializeField] private Animator _linkAnimator = default;
    [SerializeField] private Animator _fadeInOutAnimator = default;
    private GameObject _link;


    void Awake()
    {
        _link = transform.parent.gameObject;    
    }

    public void WalkToRoom(Vector2 playerDirection)
    {
        _link.GetComponent<LinkLOZ>().CantMove = true;
        _linkAnimator.speed = 1;
        _linkAnimator.SetFloat("Horizontal", playerDirection.x);
        _linkAnimator.SetFloat("Vertical", playerDirection.y);
        _linkAnimator.SetTrigger("WalkToRoom");
    }

    public void ResetMovement()
    {
        _link.GetComponent<LinkLOZ>().CantMove = false;
        _link.transform.position = gameObject.transform.position;
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
