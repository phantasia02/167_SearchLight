using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

[CustomEditor(typeof(BreakableV2))]
public class BreakableV2Editor : Editor
{
    private Texture logo;
    private bool showInfo = false;
    BreakableV2 BreakObjScript;

    void OnEnable()
    {
        logo = (Texture)Resources.Load("editorimage");
    }

    void Update()
    {
        if (BreakObjScript != null && showInfo == true)
        {
            EditorApplication.update();
        }

    }

    public override void OnInspectorGUI()
    {
        GUILayout.Label(logo);
        BreakObjScript = (BreakableV2)target;
        EditorGUILayout.LabelField("Damage", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("");

        BreakObjScript.UseCollisionForce = EditorGUILayout.Toggle("Use Collision Force", BreakObjScript.UseCollisionForce);

        if (BreakObjScript.UseCollisionForce == true)
        {

            BreakObjScript.CollisionForce = EditorGUILayout.FloatField("Collision Force", BreakObjScript.CollisionForce);
            EditorGUILayout.LabelField("");

        }

        BreakObjScript.UseHp = EditorGUILayout.Toggle("Use Health", BreakObjScript.UseHp);

        if (BreakObjScript.UseHp == true)
        {
            BreakObjScript.MaxHp = EditorGUILayout.FloatField("Max Health", BreakObjScript.MaxHp);
            EditorGUILayout.LabelField("");
        }

        BreakObjScript.UseThisObject = EditorGUILayout.Toggle("Use This Object", BreakObjScript.UseThisObject);

        SerializedProperty cp = serializedObject.FindProperty("CollisionPrefabs");
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(cp, true);
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }

        BreakObjScript.CollisionEffectDuration = EditorGUILayout.FloatField("Collision Effect Duration", BreakObjScript.CollisionEffectDuration);

        SerializedProperty cs = serializedObject.FindProperty("CollisionSounds");
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(cs, true);
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }

        EditorGUILayout.LabelField("________________________________________________________________________________________", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Breaking", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("");

        BreakObjScript.BreakDelay = EditorGUILayout.FloatField("Break Delay (s)", BreakObjScript.BreakDelay);

        if (BreakObjScript.BreakDelay > 0)
        {

            SerializedProperty adbp = serializedObject.FindProperty("AfterDelayBrokenPrefabs");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(adbp, true);
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
            EditorGUILayout.LabelField("");

        }
        else
        {

            if (BreakObjScript.ApplyExplosionForce == true)
            {
                EditorGUILayout.LabelField("");
            }
        }

        BreakObjScript.ApplyExplosionForce = EditorGUILayout.Toggle("Use Explosion Force", BreakObjScript.ApplyExplosionForce);

        if (BreakObjScript.ApplyExplosionForce == true)
        {

            BreakObjScript.ExplosionForce = EditorGUILayout.FloatField("Explosion Force", BreakObjScript.ExplosionForce);
            BreakObjScript.ExplosionRadius = EditorGUILayout.FloatField("Explosion Radius", BreakObjScript.ExplosionRadius);
            EditorGUILayout.LabelField("");

        }

        SerializedProperty ndbp = serializedObject.FindProperty("BrokenPrefabs");
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(ndbp, true);
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }

        SerializedProperty bs = serializedObject.FindProperty("BreakingSounds");
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(bs, true);
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }

        EditorGUILayout.LabelField("________________________________________________________________________________________", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Cleanup", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("");

        BreakObjScript.UseAutoDestruct = EditorGUILayout.Toggle("Destroy broken objects", BreakObjScript.UseAutoDestruct);
        if (BreakObjScript.UseAutoDestruct == true)
        {
            BreakObjScript.AutoDestructTimer = EditorGUILayout.FloatField("Timer (s)", BreakObjScript.AutoDestructTimer);
        }
        if (Application.isPlaying == false)
        {
            BreakObjScript.CanReset = EditorGUILayout.Toggle("Object can reset", BreakObjScript.CanReset);
        }

        if (showInfo == true)
        {

            EditorGUILayout.LabelField("________________________________________________________________________________________", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Info", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("");

            if (BreakObjScript.broken == false)
            {

                if (BreakObjScript.currentHp == BreakObjScript.MaxHp)
                {
                    EditorGUILayout.LabelField("Status: Undamaged");
                }
                else
                {
                    EditorGUILayout.LabelField("Status: HP " + (Mathf.Round(BreakObjScript.currentHp * 100f) / 100f).ToString() + "/" + (Mathf.Round(BreakObjScript.MaxHp * 100f) / 100f).ToString());
                }

            }
            else
            {

                if (BreakObjScript.delayTimer > 0)
                {
                    EditorGUILayout.LabelField("Status: Broken, waiting to spawn prefab");
                }
                else {
                    if (BreakObjScript.destructionTimer > 0)
                    {
                        EditorGUILayout.LabelField("Status: Broken, waiting for cleanup");
                    }
                    else {
                        EditorGUILayout.LabelField("Status: Broken, all actions complete");
                    }
                }
            }

            if (GUILayout.Button("Hide object information"))
            {
                showInfo = false;
            }

        }
        else
        {
            if (GUILayout.Button("Show object information"))
            {
                showInfo = true;
            }
        }

        if (GUILayout.Button("Reset object"))
        {
            BreakObjScript.Reset();
        }
    }
}
