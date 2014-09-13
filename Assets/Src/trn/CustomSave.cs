using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace trn
{
	public class CustomSave : core.Save
	{
		public void SavePresets()
		{
			BinaryFormatter bf = new BinaryFormatter ();

			MemoryStream memoryStream = new MemoryStream ();
			bf.Serialize (memoryStream, AppRoot.Instance.Timer.Presets);
			string tmp = System.Convert.ToBase64String (memoryStream.ToArray ());
			PlayerPrefs.SetString ( "Presets", tmp);
		}

		public void LoadPresets()
		{
			string tmp = PlayerPrefs.GetString ("Presets", string.Empty);
			if (tmp != string.Empty )
			{
				MemoryStream memoryStream = new MemoryStream (System.Convert.FromBase64String (tmp));

				BinaryFormatter bf = new BinaryFormatter ();

				AppRoot.Instance.Timer.Presets = bf.Deserialize (memoryStream) as List<Timer.Config>;
			}
		}


		public void RestorePresets()
		{
			PlayerPrefs.DeleteKey ("Presets");
		}
		
	}
}
