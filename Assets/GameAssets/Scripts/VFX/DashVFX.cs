using UnityEngine;

public class DashVFX : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
    public void PlayAnimation()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        animator.Play("DashVFX", 0, 0);
    }
    public void OnAnimationEnd()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}
