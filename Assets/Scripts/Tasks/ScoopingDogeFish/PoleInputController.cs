using UnityEngine;

public class PoleInputController : MonoBehaviour
{
    public PoleController PoleController;

    public Camera camera;
    private void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        Vector3 worldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
        PoleController.transform.position = worldPosition;
    }
}
