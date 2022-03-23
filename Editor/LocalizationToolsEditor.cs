#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

/// <summary>
/// Adds "Sync" button to LocalizationSync script.
/// </summary>
[CustomEditor(typeof(LocalizationTools))]
public class LocalizationSyncEditor : UnityEditor.Editor
{
    private const string menuPathLocalizationManager = "Tools/Localization/Select LocalizationManager";
    private const string filePathLocalizationManager = "Assets/Plugins/SimpleLocalization/Prefab/LocalizationManager.prefab";
   
    private const string menuPathLocalizationSettings = "Tools/Localization/Select LocalizationSettings";
    private const string filePathLocalizationSettings = "Assets/Plugins/SimpleLocalization/Resources/LocalizationSettings.asset";

    [MenuItem(menuPathLocalizationManager)]
    public static void SelectLocalizationManager() => SelectObject(filePathLocalizationManager);


    [MenuItem(menuPathLocalizationSettings)]
    public static void SelectLocalizationSettings() => SelectObject(filePathLocalizationSettings);

    private static void SelectObject(string path) => Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(path);
    

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var component = (LocalizationTools)target;

        if (GUILayout.Button("Export From Web"))
        {
            component.ExportLocalizationFromWeb();
        }
    }



}
#endif