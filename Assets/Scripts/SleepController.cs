using System.Collections;
using UnityEngine;

public class SleepController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
            return;
        }
    }

    public void Sleep()
    {
        animator.Play("Sleep");
    }
}
