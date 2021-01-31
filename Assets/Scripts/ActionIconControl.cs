using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionIconControl : MonoBehaviour
{
    [HideInInspector] public Interactable_Source obj;
    [HideInInspector] public int inventoryIndex;

    private bool iconEnabled = false;

    private Vector3 halfScreen;

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
    }

    private void GoToCursor()
    {
        transform.localPosition = Input.mousePosition - halfScreen;
    }

    public void Disable()
    {
        iconEnabled = false;
        navMeshController.heldItem = null;
        navMeshController.heldItemIndex = -1;
        Destroy(gameObject);
    }
}
