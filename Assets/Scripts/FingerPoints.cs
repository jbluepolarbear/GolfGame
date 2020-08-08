using System.Collections;
using System.Collections.Generic;
using Contexts;
using UnityEngine;
using UnityEngine.UI;

public class FingerPoints : MonoBehaviour
{
    [SerializeField]
    private FingerPoint _fingerPointPivot;
    
    [SerializeField]
    private FingerPoint _fingerPointAim;

    [SerializeField]
    private Image _bringeImage;

    [SerializeField]
    private float _minBridgeLength = 90.0f;

    [SerializeField]
    private Animator _animator;

    // Update is called once per frame
    private void Update()
    {
        InputController inputController = Context.Get<InputController>();
        if (inputController.Dragging)
        {
            if (_animator)
            {
                _animator.SetBool("show", true);
            }
            else
            {
                GetComponent<CanvasGroup>().alpha = 1.0f;
            }

            _bringeImage.transform.localPosition = inputController.PointerStartPosition;
            Vector2 direction = inputController.PointerPosition - inputController.PointerStartPosition;
            float distance = direction.magnitude;
            if (distance <= _minBridgeLength)
            {
                _bringeImage.transform.localScale = new Vector3(0.0f, 1.0f, 1.0f);
                return;
            }
            _bringeImage.transform.localScale = new Vector3(distance, 1.0f, 1.0f);
            Vector2 dragNormal = inputController.DragDirection.normalized;
            float angle = Mathf.Atan2(dragNormal.y, dragNormal.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle));
            _bringeImage.transform.rotation = rotation;
        }
        else
        {
            if (_animator)
            {
                _animator.SetBool("show", false);
            }
            else
            {
                GetComponent<CanvasGroup>().alpha = 0.0f;
            }
        }
    }
}
