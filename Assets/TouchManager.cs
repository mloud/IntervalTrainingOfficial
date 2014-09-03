using UnityEngine;
using System.Collections.Generic;


namespace MUI
{
	public struct Touch
	{
		public Touch(int id, Vector3 pos)
		{
			Id = id;
			Position = pos;
		}

		public int Id;
		public Vector3 Position;
	}

	public interface ITouchListener
	{
		void TouchBegan(Touch touch);
		void TouchEnded(Touch touch);
		void TouchMoved(Touch touch);
	}

	public class TouchManager : MonoBehaviour
	{
		private List<ITouchListener> TouchListeners { get; set; }


		public void Register(ITouchListener listener)
		{
			TouchListeners.Add(listener);
		}

		public void Unregister(ITouchListener listener)
		{
			TouchListeners.Remove(listener);
		}


		void Awake()
		{
			TouchListeners = new List<ITouchListener>();
		}


		void Update ()
		{
			ProcessTouch();
		}

		private void TouchBegan(Touch touch)
		{
			for (int i = 0; i < TouchListeners.Count; ++i)
			{
				TouchListeners[i].TouchBegan(touch);
			}
		}

		private void TouchMoved(Touch touch)
		{
			for (int i = 0; i < TouchListeners.Count; ++i)
			{
				TouchListeners[i].TouchMoved(touch);
			}
		}

		private void TouchEnded(Touch touch)
		{
			for (int i = 0; i < TouchListeners.Count; ++i)
			{
				TouchListeners[i].TouchEnded(touch);
			}
		}

#if UNITY_EDITOR
		private bool mouseMoving;
		void ProcessTouch()
		{
			if (Input.GetMouseButtonDown(0))
			{
				mouseMoving = true;
				TouchBegan(new Touch(0, Input.mousePosition));
			}
			else if (Input.GetMouseButtonUp(0))
			{
				mouseMoving = false;
				TouchEnded(new Touch(0, Input.mousePosition));
			}

			if (mouseMoving)
			{
				TouchMoved(new Touch(0, Input.mousePosition));
			}
		}
#else
		void ProcessTouch()
		{

			for (int i = 0; i < Input.touchCount; ++i)
			{
				if (Input.GetTouch(i).phase == TouchPhase.Began)
				{
					TouchBegan(new Touch(Input.GetTouch(i).fingerId, Input.GetTouch(i).position));
				}
				else if (Input.GetTouch(i).phase == TouchPhase.Ended)
				{
					TouchEnded(new Touch(Input.GetTouch(i).fingerId, Input.GetTouch(i).position));
				}
				else if (Input.GetTouch(i).phase == TouchPhase.Moved)
				{
					TouchMoved(new Touch(Input.GetTouch(i).fingerId, Input.GetTouch(i).position));
				}
			}
		}
#endif


	}
}