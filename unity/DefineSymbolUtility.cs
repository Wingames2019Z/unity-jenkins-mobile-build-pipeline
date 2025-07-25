using UnityEngine;
using UnityEditor;
using UnityEditor.Build;

public static class DefineSymbolUtility
{
    public static void AddDefineSymbol(BuildTargetGroup targetGroup, string symbolToAdd)
    {
        string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);

        if (!defines.Contains(symbolToAdd))
        {
            defines = string.IsNullOrEmpty(defines) ? symbolToAdd : $"{defines};{symbolToAdd}";
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defines);
            Debug.Log($"Define symbol '{symbolToAdd}' を追加しました");
        }
        else
        {
            Debug.Log($"Define symbol '{symbolToAdd}' はすでに存在します");
        }
    }
}
