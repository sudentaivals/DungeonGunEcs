using UnityEngine;

public class CustomCamera : MonoBehaviour
{
    [SerializeField] Transform _target;
    [Header("Settings")]
    [SerializeField] float _deadzoneRadius;
    [SerializeField] float _movementFactor = 2.5f;

    private float _interpolationTime = 0.3f;
    private Vector3 _velocity;
    private Camera _mainCam;
    void Start()
    {
        _mainCam = Camera.main;
    }

    void LateUpdate()
    {
        var mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        var cameraTargetPos = _target.position;
        if(Vector2.Distance((Vector2)_target.position, (Vector2)mousePos) > _deadzoneRadius)
        {
            Vector2 midPoint = (mousePos - _target.position) / _movementFactor;
            cameraTargetPos = new(midPoint.x, midPoint.y, 0);
        }
        transform.position = Vector3.SmoothDamp(transform.position, cameraTargetPos, ref _velocity, _interpolationTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }
}
