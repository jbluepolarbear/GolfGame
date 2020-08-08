using System.Collections;
using System.Collections.Generic;
using Contexts;
using UnityEngine;

public class FingerPoint : MonoBehaviour
{
    [SerializeField]
    private bool _stationary = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputController inputController = Context.Get<InputController>();
        if (inputController.Dragging)
        {
            if (!_stationary)
            {
                transform.localPosition = new Vector3(inputController.PointerPosition.x, inputController.PointerPosition.y, 0.0f);
            }
            else
            {
                transform.localPosition = new Vector3(inputController.PointerStartPosition.x, inputController.PointerStartPosition.y, 0.0f);
            }
        }
    }
}
