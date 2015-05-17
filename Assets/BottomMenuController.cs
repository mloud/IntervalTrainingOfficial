using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class BottomMenuController : MonoBehaviour 
{
	[SerializeField]
	Mode mode;

	public enum Mode
	{
		Add,
		NoAdd
	}

	public enum PanelType
	{
		Timer = 0,
		Settings,
		Workout,
		Music,
		Info
	}


	[SerializeField]
	PanelType activePanel;

	[SerializeField]
	Button btnLock;

	[System.Serializable]
	class Panel
	{
		public PanelType Type;
		public Button UIButton;
		public RectTransform UIPanel;
	}

	[SerializeField]
	List<Panel> Panels;


	void Awake()
	{
		if (core.Config.ShowAdverts)
		{
			gameObject.SetActive(mode == Mode.Add);
		}
		else
		{
			gameObject.SetActive(mode == Mode.NoAdd);
		}
	}

	void Start()
	{
		Init ();
	}

	public void Init()
	{
		OnButtonClick (Panels.Find (x => x.Type == activePanel).UIButton);
	}


	public void OnButtonClick(Button button)
	{

		if (button == btnLock)
		{
			AppRoot.Instance.Locker.ToggleLock();

			ColorBlock colorBlock = button.colors;
			if (AppRoot.Instance.Locker.Locked)
			{
				colorBlock.normalColor = Color.green;
			}
			else
			{
				colorBlock.normalColor = Color.white;
			}

			button.colors = colorBlock;


		}
		else
		{

			for (int i = 0; i < Panels.Count; ++i)
			{
				ColorBlock colorBlock = button.colors;
				if (Panels[i].UIButton == button)
				{
					colorBlock.normalColor = Color.red;
					Panels[i].UIButton.colors = colorBlock;
					ShowPanel(i);
				}
				else
				{
					colorBlock.normalColor = Color.white;
					Panels[i].UIButton.colors = colorBlock;
				}
			}
		}
	}


	private void ShowPanel(int index)
	{
		for (int i = 0; i < Panels.Count; ++i)
		{
			Panels[i].UIPanel.gameObject.SetActive(i == index);
		}

		AppRoot.Instance.Anl.LogEvent (new EventHitBuilder ().SetEventCategory (trn.anl.Def.EvtClick).SetEventAction (Panels[index].UIPanel.gameObject.name));

	}


}
