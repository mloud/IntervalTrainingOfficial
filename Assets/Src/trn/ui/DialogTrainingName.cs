using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace trn
{
	namespace ui
	{
		public class DialogTrainingName : core.ui.Dialog
		{
			[SerializeField]
			private InputField InputField;
			

			void Awake()
			{
			}

			protected override void OnEnable()
			{
				//InputField.value = "";

			}
		}
	}
}