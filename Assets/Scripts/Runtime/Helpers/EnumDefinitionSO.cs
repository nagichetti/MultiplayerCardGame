using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnumDefinition",menuName = "Tools/Enum Definition")]
public class EnumDefinitionSO : ScriptableObject
{
    public string enumName = "GeneratedEnum";

    public List<string> values = new();
}
