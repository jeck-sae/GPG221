using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

public class CameraMovement : Singleton<CameraMovement>
{
    [SerializeField] private Vector2 xRange = new Vector2(-20f, 20f);
    [SerializeField] private Vector2 yRange = new Vector2(-20f, 20f);
    [SerializeField] private Vector2 zoomRange = new Vector2(.5f, 20f);

    [SerializeField] private float zoomSpeed;
    [SerializeField] private float WASDSpeed;

    [SerializeField] private float lerpAmount;

    private Vector3 dragOrigin;

    private static Camera cam => Helpers.Camera;


    public void SetRestrictions(float minZoom, float maxZoom, float minx, float miny, float maxx, float maxy)
    {
        zoomRange = new Vector2(minZoom, maxZoom);
        xRange = new Vector2(minx, maxx);
        yRange = new Vector2(miny, maxy);
    }

    public void SetPosition(Vector3 position, float zoom)
    {
        MoveTo(position);
        SetZoom(zoom);
    }

    private void Update()
    {
        WasdMovement();
        PanCamera();
        InputZoom();
    }

    private void WasdMovement()
    {
        float axis = Input.GetAxisRaw("Horizontal");
        if (axis != 0)
            TranslateCamera(Vector2.right * (WASDSpeed * Time.deltaTime * Mathf.Lerp(cam.orthographicSize, 0, .5f) * axis));
        axis = Input.GetAxisRaw("Vertical");
        if (axis != 0)
            TranslateCamera(Vector2.up * (WASDSpeed * Time.deltaTime * Mathf.Lerp(cam.orthographicSize, 0, .5f) * axis));
    }

    private void PanCamera()
    {
        if (Input.GetMouseButtonDown(2) || Input.GetMouseButtonDown(1))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
            return;
        }

        if(Input.GetMouseButton(2) || Input.GetMouseButton(1))
        {
            Vector3 diff = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);

            TranslateCamera(diff);
        }
    }

    private void InputZoom()
    {
        if (Input.mouseScrollDelta.y != 0)
            ZoomTowards(- Input.mouseScrollDelta.y * zoomSpeed, Input.mousePosition);
    }
    public void ZoomTowards(float zoomAmount, Vector2 screenPos)
    {
        Vector2 from = cam.ScreenToWorldPoint(screenPos);
        ZoomBy(zoomAmount);
        Vector2 to = cam.ScreenToWorldPoint(screenPos);
        TranslateCamera(from - to);
    }
    public void ZoomBy(float amount)
        => SetZoom(cam.orthographicSize + amount);
    public void SetZoom(float zoom)
        => cam.orthographicSize = Mathf.Clamp(zoom, zoomRange.x, zoomRange.y);
        

    public void TranslateCamera(Vector3 moveBy)
    {
        float x = cam.transform.position.x + moveBy.x;
        float y = cam.transform.position.y + moveBy.y;

        MoveTo(x, y);
    }

    public void MoveTo(Vector3 destination)
        => MoveTo(destination.x, destination.y);
    public void MoveTo(float x, float y)
    {
        if      (x > xRange.y) x = xRange.y;
        else if (x < xRange.x) x = xRange.x;
        if      (y > yRange.y) y = yRange.y;
        else if (y < yRange.x) y = yRange.x;

        cam.transform.position = new Vector3(x, y, cam.transform.position.z);
    }
}
