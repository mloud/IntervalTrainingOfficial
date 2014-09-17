using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NumItem : MonoBehaviour 
{

	public delegate void ValueChangedDelegate(NumItem numItem);

	public ValueChangedDelegate OnValueChanged;

	[SerializeField]
	Animator animator;

	
	[SerializeField]
	Text text;

	[SerializeField]
	int minValue;

	[SerializeField]
	int maxValue;


	public int Value { get; private set; }

	void Awake()
	{
		Refresh ();
	}

	public void SetValue(int value)
	{
		Value = value;
		Refresh ();
	}

	public void PlayChangeAnim()
	{
		//animator.SetTrigger("change");
	}

	public void OnPlus()
	{
		int newValue = Mathf.Min (maxValue, Value + 1);

		if (newValue != Value)
		{
			Value = newValue;
		}

		Refresh ();

		OnValueChanged (this);
	}

	public void OnMinus()
	{
		int newValue = Mathf.Max (minValue, Value - 1);

		if (newValue != Value)
		{
			Value = newValue;
		}
		Refresh ();

		OnValueChanged (this);
	}

	private void Refresh()
	{
		text.text = Value.ToString ();
	}

}
