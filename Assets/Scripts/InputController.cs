using System;
using System.Collections;
using System.Collections.Generic;
using Contexts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputController : ContextProvider<InputController>, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private float _inputTolerance = 0.05f;
    [SerializeField]
    private float _maxDragPercentage = 0.5f; //Percentage of parent canvas height
    [SerializeField]
    private RectTransform _dragTarget;
    [SerializeField]
    private float _dragTargetTolerance = 200.0f;
    
    /// <summary>
    ///  Turns input on and off
    /// </summary>
    public bool Interactable { get; set; } = true;
    
    /// <summary>
    /// Is a dragging action being performed
    /// </summary>
    public bool Dragging { get; private set; }
    
    /// <summary>
    /// Direction from the Start position to current position.
    /// </summary>
    public Vector2 DragDirection { get; private set; }
    
    /// <summary>
    /// Direction from the Start position to current position.
    /// </summary>
    public float DragPercentage { get; private set; }
    public Vector2 PointerStartPosition => _dragTarget.localPosition;
    public Vector2 PointerPosition => ToCanvas(_pointerDown.position);

    public delegate void DragCallback();
    public event DragCallback DraggingBeginEvent;
    public event DragCallback DraggingEndEvent;
    
    // Update is called once per frame
    void Update()
    {
        if (!Interactable)
        {
            return;
        }
        
        if (Dragging && _pointerDown != null && PointerPosition != PointerStartPosition)
        {
            UpdateDrag();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Dragging || !Interactable)
        {
            return;
        }

        _pointerDown = eventData;
        if ((PointerPosition - PointerStartPosition).magnitude > _dragTargetTolerance)
        {
            return;
        }
        
        DragPercentage = 0.0f;
        DragDirection = Vector2.zero;
        Dragging = true;
        _pointerDown = eventData;
        DraggingBeginEvent?.Invoke();
        UpdateDrag();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!Dragging || !Interactable)
        {
            return;
        }
        
        DraggingEndEvent?.Invoke();
        ResetDrag();
    }
    
    private void UpdateDrag()
    {
        Canvas canvas = GetComponentInParent<Canvas>();

        Vector2 diff = PointerPosition - PointerStartPosition;

        RectTransform canvasRect = ((RectTransform) canvas.transform);

        float travelTarget = _inputTolerance * canvasRect.rect.width * canvas.scaleFactor;
        if (diff.magnitude < travelTarget)
        {
            DragPercentage = 0.0f;
            DragDirection = Vector2.zero;
        }
        else
        {
            DragDirection = diff.normalized;
            float maxDragMagnitude = _maxDragPercentage * canvasRect.rect.width * canvas.scaleFactor;
            if (diff.magnitude > maxDragMagnitude)
            {
                diff = DragDirection * maxDragMagnitude;
            }
            DragPercentage = diff.magnitude / maxDragMagnitude;
        }
    }

    private void ResetDrag()
    {
        Dragging = false;
        DragPercentage = 0.0f;
        DragDirection = Vector2.zero;
        _pointerDown = null;
    }

    private Vector2 ToCanvas(Vector2 screenPosition)
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        Camera camera = canvas.worldCamera;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform) canvas.transform,
            screenPosition,
            camera,
            out Vector2 localPoint);
        return localPoint;
    }
    
    private PointerEventData _pointerDown;
}
