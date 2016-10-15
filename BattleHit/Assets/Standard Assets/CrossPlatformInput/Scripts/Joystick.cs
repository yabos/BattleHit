using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityStandardAssets.CrossPlatformInput
{
	public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
	{
		public enum AxisOption
		{
			// Options for which axes to use
			Both, // Use both
			OnlyHorizontal, // Only horizontal
			OnlyVertical // Only vertical
		}

		public int MovementRange = 100;
		public AxisOption axesToUse = AxisOption.Both; // The options for the axes that the still will use
		public string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
		public string verticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input

		Vector3 m_StartPos;
		bool m_UseX; // Toggle for using the x axis
		bool m_UseY; // Toggle for using the Y axis
		CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
		CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; // Reference to the joystick in the cross platform input

        Image m_Image;

        void OnEnable()
		{
			CreateVirtualAxes();
		}

        void Start()
        {
            m_Image = gameObject.GetComponent<Image>();
            m_StartPos = transform.position;
        }

        bool bJoystickActive = false;
        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                CreateVirtualAxes();

                transform.position = Input.mousePosition;
                m_StartPos = Input.mousePosition;
                bJoystickActive = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                CrossPlatformInputManager.UnRegisterVirtualAxis(horizontalAxisName);
                CrossPlatformInputManager.UnRegisterVirtualAxis(verticalAxisName);
                bJoystickActive = false;
            }
#else
            if (Input.touchCount > 0)
            {
                CreateVirtualAxes();

                Touch touch = Input.GetTouch(0);
                transform.position = new Vector3(touch.position.x, touch.position.y);
                m_StartPos = transform.position;
                bJoystickActive = true;
            }

            if (Input.touchCount == 0)
            {
                CrossPlatformInputManager.UnRegisterVirtualAxis(horizontalAxisName);
                CrossPlatformInputManager.UnRegisterVirtualAxis(verticalAxisName);
                bJoystickActive = false;
            }
#endif
            if (m_Image != null)
            {
                m_Image.enabled = bJoystickActive;
                if (bJoystickActive)
                {
                    Vector2 vPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    OnDragPoint(vPos);
                }
            }
        }

        void UpdateVirtualAxes(Vector3 value)
		{
			var delta = m_StartPos - value;
			delta.y = -delta.y;
			delta /= MovementRange;
			if (m_UseX)
			{
				m_HorizontalVirtualAxis.Update(-delta.x);
			}

			if (m_UseY)
			{
				m_VerticalVirtualAxis.Update(delta.y);
			}
		}

		void CreateVirtualAxes()
		{
			// set axes to use
			m_UseX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
			m_UseY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);

			// create new axes based on axes to use
			if (m_UseX)
			{
                if (CrossPlatformInputManager.AxisExists(horizontalAxisName))
                {
                    CrossPlatformInputManager.UnRegisterVirtualAxis(horizontalAxisName);
                }

                m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
			}
			if (m_UseY)
			{
                if (CrossPlatformInputManager.AxisExists(verticalAxisName))
                {
                    CrossPlatformInputManager.UnRegisterVirtualAxis(verticalAxisName);
                }

                m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
			}
		}

		public void OnDrag(PointerEventData data)
		{
			//Vector3 newPos = Vector3.zero;

			//if (m_UseX)
			//{
			//	int delta = (int)(data.position.x - m_StartPos.x);
			//	//delta = Mathf.Clamp(delta, - MovementRange, MovementRange);
			//	newPos.x = delta;
			//}

			//if (m_UseY)
			//{
			//	int delta = (int)(data.position.y - m_StartPos.y);
			//	//delta = Mathf.Clamp(delta, -MovementRange, MovementRange);
			//	newPos.y = delta;
			//}
			//transform.position = Vector3.ClampMagnitude(new Vector3(newPos.x, newPos.y, newPos.z), MovementRange) + m_StartPos;
			//UpdateVirtualAxes(transform.position);
		}


		public void OnPointerUp(PointerEventData data)
		{
			//transform.position = m_StartPos;
			//UpdateVirtualAxes(m_StartPos);
        }

		public void OnPointerDown(PointerEventData data) {}

        void OnDragPoint(Vector2 vPos)
        {
            Vector3 newPos = Vector3.zero;

            if (m_UseX)
            {
                int delta = (int)(vPos.x - m_StartPos.x);
                //delta = Mathf.Clamp(delta, - MovementRange, MovementRange);
                newPos.x = delta;
            }

            if (m_UseY)
            {
                int delta = (int)(vPos.y - m_StartPos.y);
                //delta = Mathf.Clamp(delta, -MovementRange, MovementRange);
                newPos.y = delta;
            }
            transform.position = Vector3.ClampMagnitude(new Vector3(newPos.x, newPos.y, newPos.z), MovementRange) + m_StartPos;
            UpdateVirtualAxes(transform.position);
        }

        void OnDisable()
		{
			// remove the joysticks from the cross platform input
			if (m_UseX)
			{
				m_HorizontalVirtualAxis.Remove();
			}
			if (m_UseY)
			{
				m_VerticalVirtualAxis.Remove();
			}
		}
	}
}