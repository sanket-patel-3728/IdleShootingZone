using UnityEngine;

namespace HedgehogTeam.EasyTouch
{
	// Represents informations on Finger for touch
	// Internal use only, DO NOT USE IT
	public class Finger : BaseFinger
	{
		public float startTimeAction;
		public Vector2 oldPosition;
		public int tapCount;                // Number of taps.
		public TouchPhase phase;            // Describes the phase of the touch.
		public EasyTouch.GestureType gesture;
		public EasyTouch.SwipeDirection oldSwipeType;
	}
}