using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class PresetController : MonoBehaviour
{
	[SerializeField]
	Animator animator;

	[SerializeField]
	trn.ui.PresetButton button;

	[SerializeField]
	Button removeButton;

	[SerializeField]
	Button confirmButton;

	
	[SerializeField]
	public core.ui.ExtText ExtText;

	[SerializeField]
	float holdTime = 0.5f;

	public bool Removable { get; set; }

	public string PresetName { get; set; }


	private float Timer { get; set; }

	private Vector3 _mouseDownPos;

	void Awake()
	{
		ExtText = GetComponentInChildren<core.ui.ExtText> ();

		removeButton.gameObject.SetActive (false);
		confirmButton.gameObject.SetActive (false);

		button.OnPointerDownDelegate = OnButtonDown;
		button.OnPointerUpDelegate = OnButtonUp;

		Timer = -1;
	}

	void Update()
	{

		if (Timer > 0)
		{
			if ( (_mouseDownPos - Input.mousePosition).sqrMagnitude > 10 * 10)
			{
				Timer = -1;			
			}
		}

	
		if (Removable && Timer > 0 && Time.time > Timer)
		{
			Timer = -1;

			OnRemoveModeStart();
		}
	}

	private void OnRemoveModeStart()
	{
		GetComponentInParent<SettingsController> ().DeselectAll ();

		removeButton.gameObject.SetActive(true);

		animator.SetTrigger ("select");
	}

	private void OnRemoveModeEnd()
	{
		removeButton.gameObject.SetActive(false);
		
		animator.Play("Idle");
	}

	private void OnEditModeStart()
	{
		confirmButton.gameObject.SetActive(true);
	}

	private void OnEditModeEnd()
	{
		confirmButton.gameObject.SetActive(false);
		
		animator.Play("Idle");
	}


	public void OnClick()
	{

		if (Time.time < Timer)
		{
			if (removeButton.gameObject.activeSelf)
			{
				OnRemoveModeEnd();
			}
			else
			{
				AppRoot.Instance.OnPresetClick (this);

				OnRemoveModeEnd();
			}
		}

		Timer = -1;
	}


	public void OnButtonDown(Button button)
	{
		Timer = Time.time + holdTime;

		_mouseDownPos = Input.mousePosition;
	}

	public void OnButtonUp(Button button)
	{
		
	}

	public void OnRemove()
	{
		AppRoot.Instance.OnRemovePreset (this);
	}

	public void OnConfirm()
	{
		AppRoot.Instance.OnConfirmPreset (this);


	}

	public void ShowEditButton()
	{
		OnEditModeStart ();
	}


	public void Hightlight()
	{
		animator.SetTrigger ("edit");
	}

	public void Deselect()
	{
		confirmButton.gameObject.SetActive (false);
		removeButton.gameObject.SetActive (false);
		animator.Play("Idle");
	}

}
