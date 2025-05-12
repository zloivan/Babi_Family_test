using _Game.Scripts.Core.Services;
using UnityEngine;

public class InputService : IInputService
{
    public Vector3 GetPointerWorldPosition(Camera camera)
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -camera.transform.position.z;
        return camera.ScreenToWorldPoint(mousePosition);
    }

    public bool IsPointerDown()
    {
        return Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
    }

    public bool IsPointerUp()
    {
        return Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended);
    }
}