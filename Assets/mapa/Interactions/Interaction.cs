using UnityEngine;

public class Interaction : MonoBehaviour
{
    Animator animator => GetComponent<Animator>();
    private string anim;

    void Update()
    {
        if (!Camera.main)
            return;

        anim = GetDistanceBetweenPoints(Camera.main.transform) < 5 ? "FadeIn" : "FadeOut";

        animator.Play(anim);
    }

    float GetDistanceBetweenPoints(Transform a)
    {
        return Vector3.Distance(a.position, transform.position);
    }
}
