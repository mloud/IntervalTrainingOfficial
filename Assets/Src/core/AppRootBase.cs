using UnityEngine;

namespace core
{
	public class AppRootBase : MonoBehaviour
	{
		public core.TextManager TextManager { get; private set; }

		void Awake()
		{
			PreInit ();
		}

		void Start()
		{
			PostInit ();
		}

		void PreInit()
		{
			Debug.Log ("AppRootBase.PreInit()");

			core.Config.Init ();
			
			TextManager = new core.TextManager ();
			
			if (!TextManager.LoadTextFile (core.Config.GetLanguageFilename(Language())))
			{
				TextManager.LoadTextFile (core.Config.GetLanguageFilename(core.Config.GetDefaultLanguageFilename()));
			}

			OnPreInit ();
		}

		public string Language()
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