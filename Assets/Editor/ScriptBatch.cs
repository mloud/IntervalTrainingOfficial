using UnityEditor;
using System.Diagnostics;
using UnityEngine;
using System.Collections.Generic;

public class ScriptBatch : MonoBehaviour 
{
	private static string DestinationPath = "~/Desktop/Builds/";
	private static string CompanyName = "MloudWork";


	private class Configuration
	{
		public Configuration(string name, string symbols, string filename, string dstPath, string srcIcon, string productName, string bundleIndentificator, BuildTarget target, BuildTargetGroup targetGroup)
		{
			Name = name;
			Symbols = symbols;
			Filename = filename;
			DstPath = dstPath;
			SrcIcon = srcIcon;
			ProductName = productName;
			BundleIndentificator = bundleIndentificator;
			Target = target;
			TargetGroup = targetGroup;
		}

		public string Name;
		public string Symbols;
		public string Filename;
		public string DstPath;
		public string SrcIcon;
		public string ProductName;
		public string BundleIndentificator;
		public BuildTarget Target;
		public BuildTargetGroup TargetGroup;
	}

	private static List<Configuration> Configs { get; set; }

	static ScriptBatch()
	{
		Configs = new List<Configuration> ();


		// DEMO config
		Configs.Add (new Configuration ("demo",
		                              "DEMO;ADVERTS",
		                              "YourIntervalTrainerDemo.apk",
		                              "~/Desktop/Builds/",
		                              "Assets/Icons/demo/icon144.png",
		                              "Your Interval Trainer Demo",
		                              "com.MloudWork.IntervalTraining",
		                                BuildTarget.Android,
		                                BuildTargetGroup.Android)
				);

		// FULL config
		Configs.Add (new Configuration ("full",
		                                "",
		                                "YourIntervalTrainer.apk",
		                                "~/Desktop/Builds/",
		                                "Assets/Icons/full/icon144.png",
		                                "Your Interval Trainer",
		                                "com.MloudWork.IntervalTrainingFull",
		                                BuildTarget.Android,
		                                BuildTargetGroup.Android)
		             );
	}




	private static string[] levels = new string[] 
	{
		"Assets/Scenes/Splash.unity", 
		"Assets/Scenes/MainScene.unity"
	};

	[MenuItem("MyTools/BuildAndroidDemo")]
	public static void Build_Demo()
	{
		BuildGame ("demo");
	}

	[MenuItem("MyTools/BuildAndroidFull")]
	public static void Build_Full()
	{
		BuildGame ("full");
	}



    public static void BuildGame (string configName)
    {
		Configuration config = Configs.Find(x=>x.Name == configName);

        // Get filename.
        //string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
        
       
		Texture2D texture = AssetDatabase.LoadMainAssetAtPath(config.SrcIcon) as Texture2D;

		int [] sizeList = PlayerSettings.GetIconSizesForTargetGroup(config.TargetGroup);
		Texture2D[] iconList = new Texture2D[sizeList.Length];
		for(int i = 0;i < sizeList.Length; i++)
		{
			int iconSize = sizeList[i];
			iconList[i] = (Texture2D)Instantiate(texture);
			iconList[i].Resize(iconSize, iconSize, TextureFormat.ARGB32, false);
		}
		PlayerSettings.SetIconsForTargetGroup(config.TargetGroup, iconList);
		PlayerSettings.companyName = CompanyName;
		PlayerSettings.productName = config.ProductName;
		PlayerSettings.bundleIdentifier = config.BundleIndentificator;
		PlayerSettings.SetScriptingDefineSymbolsForGroup (config.TargetGroup, config.Symbols);
		//PlayerSettings.keyaliasPass = "primax";


		BuildPipeline.BuildPlayer(levels, config.DstPath + config.Filename, config.Target, BuildOptions.None);
		

		//// Copy a file from the project folder to the build folder, alongside the built game.
        //FileUtil.CopyFileOrDirectory("Assets/WebPlayerTemplates/Readme.txt", path + "Readme.txt");
    }
}