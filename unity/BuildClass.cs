using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;

public class BuildClass
{
	public enum BuildType
	{
		iOS,
		Android
	}
	public static void Build()
	{
		var commands = System.Environment.GetCommandLineArgs();
		int methodIndex = -1;
		for (int i = 0; i < commands.Length; i++)
		{
			if (commands[i] == "BuildClass.Build")
			{
				methodIndex = i;
			}
		}

		var args = commands[methodIndex + 1];
		Debug.Log("args:" + args);
		var input = args.Split(',');
		Dictionary<string, string> dic = new Dictionary<string, string>();
		for (int i = 0; i < input.Length; i++)
		{
			var arg = input[i].Split(':');
			if (arg.Length != 2)
			{
				Debug.LogError("Error: command args invalid size");
				return;
			}

			dic.Add(arg[0], arg[1]);
		}

		BuildProject(dic);

	}

    /// <summary>
    /// ビルドプロジェクト
    /// </summary>
    private static void BuildProject(Dictionary<string, string> input)
    {
        string projectName = "";
        string buildName = "";
        string productName = "";
        string version = "";


        if (input.TryGetValue("proj", out projectName) == false)
        {
            Debug.LogError("Error: proj input not found.");
            return;
        }

        if (input.TryGetValue("target", out buildName) == false)
        {
            Debug.LogError("Error: target input not found.");
            return;
        }

        input.TryGetValue("name", out productName);
        if (input.TryGetValue("version", out version))
        {
            int idx = version.IndexOf('_');
            if (idx >= 0)
            {
                version = version.Substring(0, idx);
            }
        }
		var projectType = ProjectTypeEnum.Parse(projectName);
		var buildType = (BuildType)System.Enum.Parse(typeof(BuildType), buildName);

        ProjectConfiguration setting = new ProjectConfiguration();

        if (setting != null)
        {
            setting.PreBuild();
        }

        List<string> allScene = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                allScene.Add(scene.path);
            }
        }

        PlayerSettings.productName = productName;
        PlayerSettings.bundleVersion = version;

        if (input.ContainsKey("bundleNum"))
        {
            int number;
            if (int.TryParse(input["bundleNum"], out number) && number > 0)
            {
                switch (buildType)
                {
                    case BuildType.iOS:
                        PlayerSettings.iOS.buildNumber = number.ToString();
                        break;
                    case BuildType.Android:
                        PlayerSettings.Android.bundleVersionCode = number;
                        break;
                }
            }
        }

        SetPlayerSetting(buildType, projectType.ToString());

        BuildReport buildReport = null;
        BuildOptions opt = BuildOptions.AllowDebugging |
                   BuildOptions.ConnectWithProfiler |
                   BuildOptions.Development;


        switch (buildType)
		{
            case BuildType.iOS:
                buildReport = BuildPipeline.BuildPlayer(
                        allScene.ToArray(),
                        "Builds/iOSProject",
                        BuildTarget.iOS,
                        opt
                );
                break;

            case BuildType.Android:
                buildReport = BuildPipeline.BuildPlayer(
                        allScene.ToArray(),
                        "Builds/Android.aab",
                        BuildTarget.Android,
                        opt
                );
                break;
        }

        // 結果出力
        if (buildReport.summary.result == BuildResult.Succeeded)
        {
            Debug.Log("[Success!]");
        }
        else
        {
            Debug.LogError("[Build failed!] Build failed!");
            foreach (var step in buildReport.steps)
            {
                foreach (var msg in step.messages)
                {
                    if (msg.type == LogType.Error)
                    {
                        Debug.LogError($"[Build Error] {msg.content}");
                    }
                }
            }
        }
    }


    static void SetPlayerSetting(BuildType buildType, string projectType)
    {
        string bundleId = $"com.example.project.{projectType.ToLower()}";
        switch (buildType)
        {
            case BuildType.iOS:
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, bundleId);
                DefineSymbolUtility.AddDefineSymbol(BuildTargetGroup.iOS, projectType);
                break;

            case BuildType.Android:
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, bundleId);
                DefineSymbolUtility.AddDefineSymbol(BuildTargetGroup.Android, projectType);
                break;
        }
    }

}

