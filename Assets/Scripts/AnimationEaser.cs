using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AnimationEaser : MonoBehaviour
{
    public enum EasingWorkflow { Normalized, TimeDependent }

    public AnimationClip originalClip;
    public EasingWorkflow easingWorkflow = EasingWorkflow.Normalized;
    public AnimationCurve animationEasingTimeDependent = AnimationCurve.Linear(0f,0f,1f,1f);
    public AnimationCurve animationEasingNormalized = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    public float durationModifier = 1f;
    public string clipPath = "Assets/AnimationEaser/EasedAnimations/";
    public string clipName = "Clip1";
    public float sampleDeltaTime = 0.05f;




    public void EaseAnimation()
    {
        #region Copying clip and checking values
        if (clipPath == null || clipPath.Length < 1 || clipName == null || clipName.Length < 1)
        {
            Debug.Log("Both clip path and name must be non-empty");
            return;
        }

        string copyPath = clipPath + clipName + ".anim";
        if (!AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(originalClip), copyPath))
        {
            Debug.Log("Unable to copy the original clip");
            return;
        }

        AnimationClip animationClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(copyPath);
        if (animationClip == null)
        {
            Debug.Log("Error loading the copy of the clip");
            return;
        }

        if (sampleDeltaTime <= 0f)
        {
            Debug.Log("Sample delta time must be strictly positive");
            return;
        }

        if (easingWorkflow == EasingWorkflow.Normalized)
        {
            if (durationModifier <= 0f)
            {
                Debug.Log("Duration modifier must be strictly positive");
                return;
            }
            if (animationEasingNormalized == null || animationEasingNormalized.length < 1)
            {
                Debug.Log("Curve must contain at least one key");
                return;
            }
        }
        else if (easingWorkflow == EasingWorkflow.TimeDependent)
        {
            if (animationEasingTimeDependent == null || animationEasingTimeDependent.length < 1)
            {
                Debug.Log("Curve must contain at least one key");
                return;
            }
        }
        #endregion

        var bindings = AnimationUtility.GetCurveBindings(animationClip);

        float oldDuration = animationClip.length;
        float newDuration = easingWorkflow == EasingWorkflow.Normalized ?
                oldDuration * durationModifier : animationEasingTimeDependent.keys[animationEasingTimeDependent.keys.Length - 1].time;
        int samplesAmount = (int)(oldDuration / sampleDeltaTime);
        float _durationModifier = newDuration / oldDuration;

        for (int i = 0; i < bindings.Length; i++)
        {
            var binding = bindings[i];
            AnimationCurve originalCurve = AnimationUtility.GetEditorCurve(animationClip, binding);
            AnimationCurve newCurve = new AnimationCurve();

            for (int j = 0; j < samplesAmount; j++)
            {
                float oldCurveTimeStamp = j * sampleDeltaTime;
                float newCurveTimeStamp = oldCurveTimeStamp * _durationModifier;
                float evalParameter = easingWorkflow == EasingWorkflow.Normalized ?
                    animationEasingNormalized.Evaluate(newCurveTimeStamp / newDuration) * oldDuration : animationEasingTimeDependent.Evaluate(newCurveTimeStamp);
                newCurve.AddKey(newCurveTimeStamp, originalCurve.Evaluate(evalParameter));
                if (i == 0)
                {
                    Debug.Log("newCurveTimeStamp " + newCurveTimeStamp + " oldCurveTimeStamp " + oldCurveTimeStamp + " evalParameter " + evalParameter);
                }
            }
            newCurve.AddKey(newDuration, originalCurve.Evaluate(oldDuration));
            if (i == 0)
            {
                Debug.Log("newDuration" + newDuration + " oldDuration " + oldDuration);
            }

            animationClip.SetCurve(binding.path, binding.type, binding.propertyName, newCurve);
            //Debug.Log(i + " - " + binding.path + "/" + binding.propertyName + ", Keys: " + originalCurve.keys[originalCurve.keys.Length - 1].time);
        }

        animationClip.EnsureQuaternionContinuity();
        EditorUtility.SetDirty(animationClip);
        AssetDatabase.SaveAssets();

    }


}


