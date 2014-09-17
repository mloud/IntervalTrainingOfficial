using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace trn
{
	namespace ui
	{

		public class DialogDemo : core.ui.Dialog
		{
			[SerializeField]
			private core.ui.ExtText extText;

			void Awake()
			{
				extText.SetTextKey ("STR_DEMO_VERSION", "minutes", (core.Config.Demo.TimeLimit / 60).ToString ());
			}
		
			public void OnEnable()
			{
				AppRoot.Instance.DemoVersionButton.gameObject.SetActive (false);
			}
		
			public void OnDisable()
			{
				AppRoot.Instance.DemoVersionButton.gameObject.SetActive (true);
			}

		}


	}
}