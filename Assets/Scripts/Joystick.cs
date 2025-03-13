using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {
    
	public RectTransform joystickBackground;
	public RectTransform joystickHandle;
	
	[HideInInspector] public Vector2 direction;
	
	Vector2 start;
	float range = 1f;

	public void OnPointerDown(PointerEventData eventData)
    {
		OnDrag(eventData);
    }

	public void OnDrag(PointerEventData eventData)
    {
        Vector2 touchPos;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBackground, eventData.position, eventData.pressEventCamera, out touchPos))
        {
            touchPos.x = (touchPos.x / joystickBackground.sizeDelta.x) * 2;
            touchPos.y = (touchPos.y / joystickBackground.sizeDelta.y) * 2;

            direction = new Vector2(touchPos.x, touchPos.y);
            direction = (direction.magnitude > 1.0f) ? direction.normalized : direction;

            joystickHandle.anchoredPosition = new Vector2(
                direction.x * (joystickBackground.sizeDelta.x / 2) * range,
                direction.y * (joystickBackground.sizeDelta.y / 2) * range
            );
        }
    }

	public void OnPointerUp(PointerEventData eventData)
    {
        direction = Vector2.zero;
        joystickHandle.anchoredPosition = Vector2.zero;
    }
}
