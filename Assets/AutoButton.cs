using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AutoButton : MonoBehaviour 
{
	public enum Type
	{
		Plus,
		Minus
	}

	[SerializeField]
	Type type;

	[SerializeField]
	NumItem numItem;


	private ExtButton _extButton;

	void Awake()
	{
		_extButton = GetComponent<ExtButton> ();
		_extButton.OnPointerDownDelegate = OnExtButtonPointerDown;
	}

	void Start () 
	{
	
	}
	
	void Update () 
	{
	
	}


	public void OnExtButtonPointerDown(ExtButton extButton)
	{
		if (type == Type.Minus)
		{
			numItem.OnMinus();
		}
		else if (type == Type.Plus)
		{
			numItem.OnPlus();
		}
	}

}
