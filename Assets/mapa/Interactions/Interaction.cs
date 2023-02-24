using UnityEngine;

public class Interaction : MonoBehaviour
{
    Animator animator => GetComponent<Animator>();
    private string anim;

    void Update()
    {
        anim = GetDistanceBetweenPoints(Camera.main.transform) < 5 ? "FadeIn" : "FadeOut";

        animator.Play(anim);

        if (transform.root.GetComponent<Outline>())
            transform.root.GetComponent<Outline>().enabled = GetDistanceBetweenPoints(Camera.main.transform) < 5;
    }

    float GetDistanceBetweenPoints(Transform a)
    {
        return Vector3.Distance(a.position, transform.position);
    }
}
