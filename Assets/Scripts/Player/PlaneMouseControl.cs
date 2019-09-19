using UnityEngine;

public class PlaneMouseControl : MonoBehaviour {

    [SerializeField, Range(0f, 1f)]
    private float MouseFollowSpeed = 0.5f;
    [SerializeField]
    private float RotationOnTangegeCoof = 5f;
    [SerializeField, Range(0f, 90f)]
    private float ClampRotationOnTangege = 30f;
    [SerializeField]
    private Vector2 AdditionalPosClamp;

    private Camera Camera;
    private bool CursorOutOfGameView;
    private Vector3 LastCursorPos;

    private void Awake()
    {
        Camera = Camera.main;
    }

    private void Update ()
    {
        Vector3 newPos =
            Vector2.Lerp(
                transform.position,
                CursorOutOfGameView ?
                    LastCursorPos :
                    ClampPos(Camera.ScreenToWorldPoint(Input.mousePosition)),
                MouseFollowSpeed);

        transform.localRotation = 
            Quaternion.Euler(
                Mathf.Clamp(
                    (newPos.y - transform.position.y) * RotationOnTangegeCoof,
                    -ClampRotationOnTangege,
                    ClampRotationOnTangege)
                , 0f, 0f);

        transform.position = newPos;
	}

    private void LateUpdate()
    {
        bool casheCursorOutOfGameView = CursorOutOfGameView;
        CursorOutOfGameView = !GameController.CameraWorldRect.Contains(Camera.ScreenToWorldPoint(Input.mousePosition));
        if (CursorOutOfGameView && !casheCursorOutOfGameView)
            LastCursorPos = ClampPos(Camera.ScreenToWorldPoint(Input.mousePosition));
    }

    private Vector3 ClampPos(Vector3 targetPos)
    {
        return new Vector2(
                    Mathf.Clamp(
                        targetPos.x,
                        GameController.CameraWorldRect.xMin + AdditionalPosClamp.x,
                        GameController.CameraWorldRect.xMax - AdditionalPosClamp.x),
                    Mathf.Clamp(
                        targetPos.y,
                        GameController.CameraWorldRect.yMin + AdditionalPosClamp.y,
                        GameController.CameraWorldRect.yMax - AdditionalPosClamp.y));
    }
}