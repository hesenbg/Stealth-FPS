using UnityEngine;
using UnityEditor;

public class SampleAnimationFirstFrame : EditorWindow
{
    public GameObject targetObject;
    public AnimationClip clip;

    [MenuItem("Tools/Sample First Frame")]
    public static void ShowWindow()
    {
        GetWindow<SampleAnimationFirstFrame>("Sample First Frame");
    }

    private void OnGUI()
    {
        targetObject = (GameObject)EditorGUILayout.ObjectField("Target Object", targetObject, typeof(GameObject), true);
        clip = (AnimationClip)EditorGUILayout.ObjectField("Animation Clip", clip, typeof(AnimationClip), false);

        if (GUILayout.Button("Apply First Frame"))
        {
            if (targetObject == null || clip == null)
            {
                Debug.LogWarning("Assign both Target Object and Animation Clip.");
                return;
            }

            ApplyFirstFrame(targetObject, clip);
        }
    }

    private void ApplyFirstFrame(GameObject target, AnimationClip clip)
    {
        // Sample the animation at time = 0
        clip.SampleAnimation(target, 0f);


    }
}
