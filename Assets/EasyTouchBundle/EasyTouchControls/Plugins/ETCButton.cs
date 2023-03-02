using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[System.Serializable]
public class ETCButton : ETCBase, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
	[System.Serializable] public class OnDownHandler : UnityEvent { }
	[System.Serializable] public class OnPressedHandler : UnityEvent { }
	[System.Serializable] public class OnPressedValueandler : UnityEvent<float> { }
	[System.Serializable] public class OnUPHandler : UnityEvent { }

	[SerializeField] public OnDownHandler onDown;
	[SerializeField] public OnPressedHandler onPressed;
	[SerializeField] public OnPressedValueandler onPressedValue;
	[SerializeField] public OnUPHandler onUp;

	public ETCAxis axis;

	public Sprite normalSprite;
	public Color normalColor;

	public Sprite pressedSprite;
	public Color pressedColor;

	Image cachedImage;
	bool isOnPress;
	GameObject previousDargObject;
	bool isOnTouch;

	public ETCButton ()
	{
		axis = new ETCAxis ("Button");
		_visible = true;
		_activated = true;
		isOnTouch = false;

		enableKeySimulation = true;

		axis.unityAxis = "Jump";
		showPSInspector = true;
		showSpriteInspector = false;
		showBehaviourInspector = false;
		showEventInspector = false;
	}

	protected override void Awake ()
	{
		base.Awake ();
		cachedImage = GetComponent<Image> ();
	}

	public override void Start ()
	{
		axis.InitAxis ();
		base.Start ();
		isOnPress = false;

		if (allowSimulationStandalone && enableKeySimulation && !Application.isEditor) SetVisible (visibleOnStandalone);
	}

	protected override void UpdateControlState () => UpdateButton ();

	protected override void DoActionBeforeEndOfFrame () => axis.DoGravity ();

	public void OnPointerEnter (PointerEventData eventData)
	{
		if (isSwipeIn && !isOnTouch)
		{
			if (eventData.pointerDrag != null)
			{
				if (eventData.pointerDrag.GetComponent<ETCBase> () && eventData.pointerDrag != gameObject) previousDargObject = eventData.pointerDrag;
			}

			eventData.pointerDrag = gameObject;
			eventData.pointerPress = gameObject;
			OnPointerDown (eventData);
		}
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		if (_activated && !isOnTouch)
		{
			pointId = eventData.pointerId;

			axis.ResetAxis ();
			axis.axisState = ETCAxis.AxisState.Down;

			isOnPress = false;
			isOnTouch = true;

			onDown.Invoke ();
			ApllyState ();
			axis.UpdateButton ();
		}
	}

	public void OnPointerUp (PointerEventData eventData)
	{
		if (pointId == eventData.pointerId)
		{
			isOnPress = false;
			isOnTouch = false;
			axis.axisState = ETCAxis.AxisState.Up;
			axis.axisValue = 0;
			onUp.Invoke ();
			ApllyState ();

			if (previousDargObject)
			{
				ExecuteEvents.Execute<IPointerUpHandler> (previousDargObject, eventData, ExecuteEvents.pointerUpHandler);
				previousDargObject = null;
			}
		}
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		if (pointId == eventData.pointerId)
		{
			if (axis.axisState == ETCAxis.AxisState.Press && !isSwipeOut)
			{
				OnPointerUp (eventData);
			}
		}
	}

	void UpdateButton ()
	{
		if (axis.axisState == ETCAxis.AxisState.Down)
		{
			isOnPress = true;
			axis.axisState = ETCAxis.AxisState.Press;
		}

		if (isOnPress)
		{
			axis.UpdateButton ();
			onPressed.Invoke ();
			onPressedValue.Invoke (axis.axisValue);

		}

		if (axis.axisState == ETCAxis.AxisState.Up)
		{
			isOnPress = false;
			axis.axisState = ETCAxis.AxisState.None;
		}


		if (enableKeySimulation && _activated && _visible && !isOnTouch)
		{
			if (Input.GetButton (axis.unityAxis) && axis.axisState == ETCAxis.AxisState.None)
			{
				axis.ResetAxis ();
				onDown.Invoke ();
				axis.axisState = ETCAxis.AxisState.Down;
			}

			if (!Input.GetButton (axis.unityAxis) && axis.axisState == ETCAxis.AxisState.Press)
			{
				axis.axisState = ETCAxis.AxisState.Up;
				axis.axisValue = 0;

				onUp.Invoke ();
			}

			axis.UpdateButton ();
			ApllyState ();
		}
	}

	protected override void SetVisible (bool forceUnvisible = false)
	{
		bool localVisible = _visible;
		if (!visible) localVisible = visible;
		GetComponent<Image> ().enabled = localVisible;
	}

	void ApllyState ()
	{
		if (cachedImage)
		{
			switch (axis.axisState)
			{
				case ETCAxis.AxisState.Down:
				case ETCAxis.AxisState.Press:
				cachedImage.sprite = pressedSprite;
				cachedImage.color = pressedColor;
				break;
				default:
				cachedImage.sprite = normalSprite;
				cachedImage.color = normalColor;
				break;
			}
		}
	}

	protected override void SetActivated ()
	{
		if (!_activated)
		{
			isOnPress = false;
			isOnTouch = false;
			axis.axisState = ETCAxis.AxisState.None;
			axis.axisValue = 0;
			ApllyState ();
		}
	}
}