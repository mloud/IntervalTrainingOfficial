using UnityEngine;

namespace core
{
	public class AppRootBase : MonoBehaviour
	{
		public core.ui.UIManager UIManager { get; private set; }


		void Awake()
		{
			PreInit ();
		}

		void Start()
		{
			PostInit ();
		}

		public static void StaticInit()
		{
			core.Config.Init ();

			if (!core.TextManager.Instance.LoadTextFile (core.Config.GetLanguageFilename(Language())))
			{
				core.TextManager.Instance.LoadTextFile (core.Config.GetLanguageFilename(core.Config.GetDefaultLanguageFilename()));
			}

		}

		void PreInit()
		{
			Debug.Log ("AppRootBase.PreInit()");

			StaticInit ();
		
			//UIManager
			UIManager = GameObject.FindObjectOfType<core.ui.UIManager> ();
			UIManager.DialogFactory = GameObject.FindObjectOfType<core.ui.DialogFactory> ();

			OnPreInit ();
		}

		public static string Language()
		{
			return Application.systemLanguage.ToString ();
		}


		private void PostInit()
		{
			Debug.Log ("AppRootBase.PostInit()");
			OnPostInit ();
		}

		protected virtual void OnApplicationPause() 
		{
			Debug.Log ("AppRootBase.OnApplicationPause()");

			OnPause ();
		}
		
		protected virtual void OnApplicationQuit()
		{
			Debug.Log ("AppRootBase.OnApplicationQuit()");

			OnQuit ();
		}

		protected virtual void OnApplicationResume() 
		{
			Debug.Log ("AppRootBase.OnApplicationResume()");

			OnResume ();
		}




		// Virtual methods for override
		protected virtual void OnPreInit()
		{}
		
		protected virtual void OnPostInit()
		{}
		
		protected virtual void OnPause()
		{}
		
		protected virtual void OnResume()
		{}


		protected virtual void OnQuit()
		{}

	}
}