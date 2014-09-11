using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


namespace core
{
	namespace ui
	{
		public class UIManager : MonoBehaviour
		{
			[SerializeField]
			 RectTransform dialogQueueRoot;


			public DialogFactory DialogFactory { get; set; }


			private List<Dialog> Dialogs { get; set; }

			void Awake()
			{
				Dialogs = new List<Dialog> ();
			}

			public void OpenDialog(string dialogId)
			{
				Dialog dialog = null;
				for (int i = 0; i < dialogQueueRoot.childCount; ++i)
				{
					if (dialogQueueRoot.GetChild(i).name == dialogId)
					{
						dialog = dialogQueueRoot.GetChild(i).GetComponent<Dialog>();
						dialog.gameObject.SetActive(true);
						if (!Dialogs.Contains(dialog))
							Dialogs.Add (dialog);
						break;
					}
				}
			
				dialogQueueRoot.GetChild (0).gameObject.SetActive (true); // activate bg 
			
				/*
				core.dbg.Dbg.Assert (DialogFactory != null, "UIManager.OpenDialog(): DialogFactory is null");

				Dialog dialog = DialogFactory.CreateDialog(dialogId);

				if (dialog != null)
				{
					dialog.transform.SetParent(dialogQueueRoot);
					Dialogs.Add (dialog);
				}
				*/
			}

			public void CloseDialog(string dialogId)
			{
				Dialog dialog = Dialogs.Find (x => x.Id == dialogId);
			
				core.dbg.Dbg.Assert (dialog != null, "UIManager.CloseDialog(): No dialog with id " + dialogId + " found");

				if (dialog != null)
				{
					Dialogs.Remove(dialog);

					dialog.gameObject.SetActive(false);
					//Destroy (dialog.gameObject);
				}

				if (Dialogs.Count == 0)
				{
					dialogQueueRoot.GetChild (0).gameObject.SetActive (false); // deactivate bg 
				}
			}
		}
	}
}