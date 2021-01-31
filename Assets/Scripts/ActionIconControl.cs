using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionIconControl : MonoBehaviour
{
    [HideInInspector] public Interactable_Source obj;
    [HideInInspector] public int inventoryIndex;

    private bool iconEnabled = false;

    private Vector3 halfScreen;
    Vector2 canvasSize;
    Vector2 canvasScreenScalar;
    bool canvasSizeDefined = false;

    private NavMeshController navMeshController;

    private void Awake()
    {
        halfScreen = new Vector3(Screen.width / 2, Screen.height / 2);
        navMeshController = FindObjectOfType<NavMeshController>();
    }

    private void Update()
    {
        if (!iconEnabled)
            return;

        GoToCursor();
    }

    public void Enable()
    {
        iconEnabled = true;
        navMeshController.heldItem = obj;
        navMeshController.heldItemIndex = inventoryIndex;
        Cursor.visible = false;
    }

    private void GoToCursor()
    {
        //Debug.Log(new Vector2(Screen.width, Screen.height));
        //RectTransform canvas = transform.parent.GetComponent<RectTransform>();
        //Debug.Log(new Vector2(canvas.sizeDelta.x, canvas.sizeDelta.y));
        Vector2 initPos = Input.mousePosition - halfScreen;
        if (!canvasSizeDefined)
        {
            RectTransform canvas = transform.parent.GetComponent<RectTransform>();
            canvasSize = new Vector2(canvas.sizeDelta.x, canvas.sizeDelta.y);
            float x = canvasSize.x / Screen.width;
            float y = canvasSize.y / Screen.height;
            canvasScreenScalar = new Vector2(x, y);
            canvasSizeDefined = true;
        }
        transform.localPosition = new Vector3(initPos.x * canvasScreenScalar.x, initPos.y * canvasScreenScalar.y, 0);
    }

    public void Disable()
    {
        iconEnabled = false;
        navMeshController.heldItem = null;
        navMeshController.heldItemIndex = -1;
        Cursor.visible = true;
        Destroy(gameObject);
    }
}
