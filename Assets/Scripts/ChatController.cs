using UnityEngine;

public class ChatController: MonoBehaviour
{
    private Animator animator;

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void PlayAnimation(string animationName)
    {
        if (animator != null)
        {
            animator.Play(animationName);
        }
        else
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }
    }
    
}
