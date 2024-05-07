using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ProFeaturesToggle : EditorWindow
{
    private static bool enableProFeatures = false;

    [MenuItem("Custom Tools/Pro Features Toggle")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ProFeaturesToggle), true, "Pro Features Toggle");
    }

    void OnGUI()
    {
        // 現在の状態を表示
        EditorGUILayout.LabelField("Preprocessor Symbol: ENABLE_PRO_FEATURES", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;

        // チェックボックスを表示し、ユーザーの入力に基づいて変数を更新
        enableProFeatures = EditorGUILayout.Toggle("Proライセンス以上でログインしているときのみチェック", enableProFeatures);

        // 変更があった場合、プリプロセッサディレクティブを更新
        if (GUI.changed)
        {
            UpdateDefineSymbols();
        }

        EditorGUI.indentLevel--;
    }

    private static void UpdateDefineSymbols()
    {
        // ターゲットグループごとに定義を設定
        foreach (BuildTargetGroup group in System.Enum.GetValues(typeof(BuildTargetGroup)))
        {
            if (group == BuildTargetGroup.Unknown) continue;

            // ENABLE_PRO_FEATURES ディレクティブを追加または削除
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
            var definesList = new List<string>(definesString.Split(';'));

            if (enableProFeatures)
            {
                if (!definesList.Contains("ENABLE_PRO_FEATURES"))
                {
                    definesList.Add("ENABLE_PRO_FEATURES");
                }
            }
            else
            {
                if (definesList.Contains("ENABLE_PRO_FEATURES"))
                {
                    definesList.Remove("ENABLE_PRO_FEATURES");
                }
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(group, string.Join(";", definesList.ToArray()));
        }
    }
}
