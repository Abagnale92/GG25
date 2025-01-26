using UnityEngine;
public class DestroyOnAnimationEnd : MonoBehaviour
 {
   private Animator animator;
       void Start()
    {
        Animator animator = GetComponentInChildren<Animator>();

        if (animator != null)
        {
            animator.Play(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
            Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
