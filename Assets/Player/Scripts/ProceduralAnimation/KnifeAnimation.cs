using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Collections;
using System.Collections.Generic;

public class KnifeAnimation : MonoBehaviour
{
    Transform constraintParent; 
    [SerializeField] Rig knifeRig;               // Knife rig
    [SerializeField] float transitionDuration ; // Duration of each state transition
    [SerializeField] float holdDuration ;       // How long each state stays active

    [SerializeField] Transform KnifeHand;

    private bool isAnimating = false;

    [SerializeField] List<Transform> States;

    public MultiParentConstraint constraint;

    private void Start()
    {
        constraintParent = GetComponent<Transform>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isAnimating)
        {
            StartCoroutine(PlayKnifeAnimationSequence());
        }
    }

    private IEnumerator PlayKnifeAnimationSequence()
    {
        isAnimating = true;

        // Enable Rig
        knifeRig.weight = 1f;
        foreach (Transform state in States)
        {
            constraint = state.gameObject.GetComponent<MultiParentConstraint>();
            // enable this state's constraint
            if (constraint != null)
                constraint.weight = 1f;

            yield return StartCoroutine(
                LerpTransformLocal(
                    KnifeHand,
                    state.transform.position,
                    state.transform.rotation,
                    transitionDuration
                )
            );

            yield return new WaitForSeconds(holdDuration);
        }

        knifeRig.weight = 0;
        isAnimating = false;

    }
    GameObject PrevObject;

    // Smoothly lerps position and rotation
    private IEnumerator LerpTransformLocal(
        Transform target,
        Vector3 targetLocalPos,
        Quaternion targetLocalRot,
        float duration)
    {
        Vector3 startLocalPos = target.position;
        Quaternion startLocalRot = target.rotation;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            target.position = Vector3.Lerp(startLocalPos, targetLocalPos, t);
            target.rotation = Quaternion.Slerp(startLocalRot, targetLocalRot, t);

            yield return null;
        }

        target.position = targetLocalPos;
        target.rotation = targetLocalRot;
    }

}