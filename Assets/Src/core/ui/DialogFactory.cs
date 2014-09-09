using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace core
{
	namespace ui
	{
		public abstract class DialogFactory : MonoBehaviour
		{
			public abstract Dialog CreateDialog(string dialog);
		}
	}
}