using UnityEngine;
using System.Collections.Generic;

namespace core
{
	public class Save
	{
		public void Set(string key, string value)
		{
			PlayerPrefs.SetString (key, value);
			PlayerPrefs.Save ();
		}

		public bool HasKey(string key)
		{
			return PlayerPrefs.HasKey(key);
		}

		public string GetStringValue(string key)
		{
			return PlayerPrefs.GetString(key, null);
		}

		public void RemoveKey(string key)
		{
			PlayerPrefs.DeleteKey (key);
		}

	}
}