using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


namespace core
{
	namespace ui
	{
		public class Dialog : MonoBehaviour
		{
			public object Paramater { get; set; }

			public string Id { get { return gameObject.name; } }
	
			protected virtual void OnEnable()
			{}
		}
	}
}