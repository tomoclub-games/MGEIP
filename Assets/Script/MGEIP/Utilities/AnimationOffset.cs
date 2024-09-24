using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOffset : MonoBehaviour
{
    public Animator animator1;
    public Animator animator2;
    public Animator animator3;

    void Start()
    {
        StartCoroutine(TriggerAnimations());
    }

    IEnumerator TriggerAnimations()
    {
        animator1.SetTrigger("StartAnimation");
        yield return new WaitForSeconds(2f);  // Delay for 1 second
        animator2.SetTrigger("StartAnimation");
        yield return new WaitForSeconds(2f);  // Delay for 2 seconds
        animator3.SetTrigger("StartAnimation");
    }
}
