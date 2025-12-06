using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Collections;

public class KnifeAnimation : MonoBehaviour
{
    [SerializeField] Transform constraintParent; // Parent containing all knife constraints
    [SerializeField] Rig knifeRig;               // Knife rig
    [SerializeField] float transitionDuration ; // Duration of each state transition
    [SerializeField] float holdDuration ;       // How long each state stays active

    private bool isAnimating = false;

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

        // Get all child constraints
        MultiParentConstraint[] constraints = constraintParent.GetComponentsInChildren<MultiParentConstraint>();

        // Define the sequence of states
        string[] sequence = { "Release", "Slash", "Pull" };

        foreach (string state in sequence)
        {
            // Capture current weights
            float[] startWeights = new float[constraints.Length];
            for (int i = 0; i < constraints.Length; i++)
                startWeights[i] = constraints[i].weight;

            // Determine target weights
            float[] targetWeights = new float[constraints.Length];
            for (int i = 0; i < constraints.Length; i++)
            {
                string lowerName = constraints[i].name.ToLower();
                targetWeights[i] = lowerName.Contains(state.ToLower()) ? 1f : 0f;
            }

            // Lerp weights
            float elapsed = 0f;
            while (elapsed < transitionDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / transitionDuration;
                for (int i = 0; i < constraints.Length; i++)
                {
                    constraints[i].weight = Mathf.Lerp(startWeights[i], targetWeights[i], t);
                }
                yield return null;
            }

            // Ensure final weights
            for (int i = 0; i < constraints.Length; i++)
                constraints[i].weight = targetWeights[i];

            // Hold the state for a short duration
            yield return new WaitForSeconds(holdDuration);
        }

        // Reset Rig weight at the end
        knifeRig.weight = 0f;

        // Reset all constraints to 0
        foreach (var constraint in constraints)
            constraint.weight = 0f;

        isAnimating = false;
    }
}
