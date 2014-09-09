using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace trn
{
	namespace ui
	{
		public class DialogFactory : core.ui.DialogFactory
		{

			[System.Serializable]
			class DialogRec
			{
				public string Id;
				public GameObject Prefab;
			}

			[SerializeField]
			List<DialogRec> dialogRecs;


			public override core.ui.Dialog CreateDialog(string dialogId)
			{
				core.ui.Dialog dialog = null;

				DialogRec dialogRec = dialogRecs.Find(x=>x.Id == dialogId);

				core.dbg.Dbg.Assert (dialogRec != null, "DialogFactory.CreateDialog() no dialog " + dialogId + " found");

				if (dialogRec != null)
				{
					GameObject dialogGo = GameObject.Instantiate(dialogRec.Prefab) as GameObject;
					dialog = dialogGo.GetComponent<core.ui.Dialog>();
					dialog.name = dialogId;
				}



				return dialog;
			}
		}
	}
}