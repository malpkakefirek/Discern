using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Anomaly : MonoBehaviour
{
    public bool is_extra_object = false;

    public bool is_scp = false;

    public bool is_light_anomaly = false;

    public bool can_be_resized = false;
    public float resized_scale_x = 1f;
    public float resized_scale_y = 1f;
    public float resized_scale_z = 1f;

    public bool can_disappear = false;

    public bool can_be_moved = false;
    public float moved_x = 0f;
    public float moved_y = 0f;
    public float moved_z = 0f;

    public bool can_change_models = false;
    public GameObject model_to_change;

    public bool can_be_painting_anomaly = false;
    public Texture2D image_to_change;

    // Hold a list of all anomalies that are possible
    private List<string> activeAnomalies = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        if (is_extra_object)
            activeAnomalies.Add("Extra Object Anomaly");
        else if (is_scp)
            activeAnomalies.Add("SCP Anomaly");
        else if (is_light_anomaly)
            activeAnomalies.Add("Light Anomaly");
        else { 
            if (can_be_resized)
                activeAnomalies.Add("Resizable Anomaly");
            if (can_disappear)
                activeAnomalies.Add("Disappearing Anomaly");
            if (can_be_moved)
                activeAnomalies.Add("Moving Anomaly");
            if (can_change_models)
                activeAnomalies.Add("Model Changing Anomaly");
            if (can_be_painting_anomaly)
                activeAnomalies.Add("Painting Anomaly");
        }

        //tests
        if (activeAnomalies.Count > 0)
        {
            int randomIndex = Random.Range(0, activeAnomalies.Count);
            string selectedAnomaly = activeAnomalies[randomIndex];
            Debug.Log("Selected Anomaly for "+gameObject.name+" : " + selectedAnomaly);
        }
        else
        {
            Debug.LogWarning("No active anomalies selected for "+gameObject.name+" in "+ transform.parent.parent.parent.parent.gameObject.name);// nie pytaj xd
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[CustomEditor(typeof(Anomaly))]
public class AdvancedSettingsExampleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Anomaly anomalySettings = (Anomaly)target;

        EditorGUILayout.HelpBox("Special Anomaly Options", MessageType.Info);
        // Extra object option
        if (!anomalySettings.is_scp && !anomalySettings.is_light_anomaly)
        {
            EditorGUILayout.LabelField("Extra Object Options", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            anomalySettings.is_extra_object = EditorGUILayout.Toggle("is extra object", anomalySettings.is_extra_object);
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
        }

        // Is SCP options
        if (!anomalySettings.is_extra_object && !anomalySettings.is_light_anomaly)
        {
            EditorGUILayout.LabelField("Is SCP options", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            anomalySettings.is_scp = EditorGUILayout.Toggle("is SCP", anomalySettings.is_scp);
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
        }

        // Is Light options
        if (!anomalySettings.is_extra_object && !anomalySettings.is_scp)
        {
            EditorGUILayout.LabelField("Is Light Anomaly options", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            anomalySettings.is_light_anomaly = EditorGUILayout.Toggle("is Light Anomaly", anomalySettings.is_light_anomaly);
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
        }

        if (!anomalySettings.is_extra_object && !anomalySettings.is_scp && !anomalySettings.is_light_anomaly)
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Normal Anomaly Options", MessageType.Info);
            // Resized options
            EditorGUILayout.LabelField("Resized Options", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            anomalySettings.can_be_resized = EditorGUILayout.Toggle("Can Be Resized", anomalySettings.can_be_resized);
            if (anomalySettings.can_be_resized)
            {
                EditorGUI.indentLevel++;
                anomalySettings.resized_scale_x = EditorGUILayout.FloatField("Scale X", anomalySettings.resized_scale_x);
                anomalySettings.resized_scale_y = EditorGUILayout.FloatField("Scale Y", anomalySettings.resized_scale_y);
                anomalySettings.resized_scale_z = EditorGUILayout.FloatField("Scale Z", anomalySettings.resized_scale_z);
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();

            // Disappeared options
            EditorGUILayout.LabelField("Disappeared Options", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            anomalySettings.can_disappear = EditorGUILayout.Toggle("Can Disappear", anomalySettings.can_disappear);
            EditorGUI.indentLevel--;
        
            EditorGUILayout.Space();

            // Moved Options
            EditorGUILayout.LabelField("Moved Options", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            anomalySettings.can_be_moved = EditorGUILayout.Toggle("Can Be Moved", anomalySettings.can_be_moved);
            if (anomalySettings.can_be_moved)
            {
                EditorGUI.indentLevel++;
                anomalySettings.moved_x = EditorGUILayout.FloatField("Move X", anomalySettings.moved_x);
                anomalySettings.moved_y = EditorGUILayout.FloatField("Move Y", anomalySettings.moved_y);
                anomalySettings.moved_z = EditorGUILayout.FloatField("Move Z", anomalySettings.moved_z);
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();

            // Changed Model Options
            EditorGUILayout.LabelField("Changed Model Options", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            anomalySettings.can_change_models = EditorGUILayout.Toggle("Can Change Models", anomalySettings.can_change_models);
            if (anomalySettings.can_change_models)
            {
                EditorGUI.indentLevel++;
                anomalySettings.model_to_change = (GameObject)EditorGUILayout.ObjectField("Model To Change", anomalySettings.model_to_change, typeof(GameObject), true);
            }
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();

            // Painting Options
            EditorGUILayout.LabelField("Painting Anomaly Options", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            anomalySettings.can_be_painting_anomaly = EditorGUILayout.Toggle("Can Be Painting Anomaly", anomalySettings.can_be_painting_anomaly);
            if (anomalySettings.can_be_painting_anomaly)
            {
                EditorGUI.indentLevel++;
                anomalySettings.image_to_change = (Texture2D)EditorGUILayout.ObjectField("Image To Change to", anomalySettings.image_to_change, typeof(Texture2D), true);
            }
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
        }
    }
}
