using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragUIElement : MonoBehaviour, UnityEngine.EventSystems.IBeginDragHandler, UnityEngine.EventSystems.IDragHandler, UnityEngine.EventSystems.IEndDragHandler
{
    // Prefab to instantiate when dragging ends
    [SerializeField]
    GameObject PrefabToInstantiate;

    // The UI element that can be dragged
    [SerializeField]
    RectTransform UIDragElement;

    // Canvas containing the draggable UI element
    [SerializeField]
    RectTransform Canvas;

    // Layer mask for detecting valid placement areas on the floor
    [SerializeField]
    LayerMask FloorLayer;

    // Variables for tracking original positions during drag operations
    private Vector2 mOriginalLocalPointerPosition;
    private Vector3 mOriginalPanelLocalPosition;
    private Vector2 mOriginalPosition;

    void Start()
    {
        // Original position of the draggable UI
        mOriginalPosition = UIDragElement.localPosition;
    }

    // Called when the drag operation begins
    public void OnBeginDrag(PointerEventData data)
    {
        // Save the original position of the UI element
        mOriginalPanelLocalPosition = UIDragElement.localPosition;

        // Convert screen point to local point relative to the canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Canvas, data.position, data.pressEventCamera, out mOriginalLocalPointerPosition);
    }

    // Called during the drag operation
    public void OnDrag(PointerEventData data)
    {
        Vector2 localPointerPosition;
        // Update the position of the UI element
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(Canvas, data.position, data.pressEventCamera, out localPointerPosition))
        {
            Vector3 offsetToOriginal = localPointerPosition - mOriginalLocalPointerPosition;
            UIDragElement.localPosition = mOriginalPanelLocalPosition + offsetToOriginal;
        }
    }

    // Called when the drag operation ends
    public void OnEndDrag(PointerEventData data)
    {
        // Smoothly move the UI element back to its original position
        StartCoroutine(Coroutine_MoveUIElement(UIDragElement, mOriginalPosition, 0.5f));

        // Check if the pointer is over a valid floor area
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000.0f, FloorLayer))
        {
            // Instantiate the prefab at the hit location in the world
            Vector3 worldPoint = hit.point;
            CreateObject(worldPoint);
        }
        else
        {
            Debug.Log("Invalid area");
        }
    }

    // Instantiates a prefab at the specified position
    void CreateObject(Vector3 position)
    {
        if (PrefabToInstantiate == null)
        {
            Debug.Log("No prefab to instantiate");
            return;
        }

        GameObject obj = Instantiate(PrefabToInstantiate, position, Quaternion.identity);
    }

    // Smoothly moves the UI element to the target position over a specified duration
    IEnumerator Coroutine_MoveUIElement(RectTransform r, Vector2 targetPosition, float duration = 0.1f)
    {
        float elapsedTime = 0;
        Vector2 startintPos = r.localPosition;

        while (elapsedTime < duration)
        {
            // Interpolate between the starting and target positions
            r.localPosition = Vector2.Lerp(startintPos, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        // Ensure the final position is set precisely
        r.localPosition = targetPosition;
    }

}
