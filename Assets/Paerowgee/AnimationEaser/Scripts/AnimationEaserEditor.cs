using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace com.paerowgee.animationeaser
{
    [CustomEditor(typeof(AnimationEaser))]
    public class AnimationEaserEditor : Editor
    {
        SerializedProperty originalClip;
        SerializedProperty easingWorkflow;
        SerializedProperty animationEasingTimeDependent;
        SerializedProperty animationEasingNormalized;
        SerializedProperty durationModifier;
        SerializedProperty clipPath;
        SerializedProperty clipName;
        SerializedProperty sampleDeltaTime;


        private void OnEnable()
        {
            originalClip = serializedObject.FindProperty("originalClip");
            easingWorkflow = serializedObject.FindProperty("easingWorkflow");
            animationEasingTimeDependent = serializedObject.FindProperty("animationEasingTimeDependent");
            animationEasingNormalized = serializedObject.FindProperty("animationEasingNormalized");
            durationModifier = serializedObject.FindProperty("durationModifier");
            clipPath = serializedObject.FindProperty("clipPath");
            clipName = serializedObject.FindProperty("clipName");
            sampleDeltaTime = serializedObject.FindProperty("sampleDeltaTime");
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(originalClip);

            bool isWorkflowTimeDependent = ((AnimationEaser.EasingWorkflow)easingWorkflow.enumValueFlag == AnimationEaser.EasingWorkflow.TimeDependent);
            EditorGUILayout.PropertyField(easingWorkflow);
            if (isWorkflowTimeDependent)
            {
                EditorGUILayout.PropertyField(animationEasingTimeDependent);
            }
            else
            {
                EditorGUILayout.PropertyField(animationEasingNormalized);
                EditorGUILayout.PropertyField(durationModifier);
            }
            EditorGUILayout.PropertyField(clipPath);
            EditorGUILayout.PropertyField(clipName);
            EditorGUILayout.PropertyField(sampleDeltaTime);
            if (GUILayout.Button("Ease Animation"))
            {
                ((AnimationEaser)serializedObject.targetObject).EaseAnimation();
            }


            serializedObject.ApplyModifiedProperties();
        }
    }
}