using UnityEditor;
using System.IO;
using System.Text;
using UnityEngine;

[CustomEditor(typeof(EnumDefinitionSO))]
public class EnumDefinitionSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate Enum"))
        {
            GenerateEnum((EnumDefinitionSO)target);
        }
    }

    private static void GenerateEnum(EnumDefinitionSO so)
    {
        if (string.IsNullOrEmpty(so.enumName))
        {
            Debug.LogError("Enum name is empty.");
            return;
        }

        var path = "Assets/Scripts/Runtime/Generated";
        var filePath = $"{path}{so.enumName}.cs";

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        var sb = new StringBuilder();
        sb.AppendLine("public enum " + so.enumName);
        sb.AppendLine("{");

        for (int i = 0; i < so.values.Count; i++)
        {
            var value = so.values[i].Replace(" ", "_");

            sb.AppendLine($"    {value}" + (i < so.values.Count - 1 ? "," : ""));
        }

        sb.AppendLine("}");

        File.WriteAllText(filePath, sb.ToString());
        AssetDatabase.Refresh();
    }
}
