using UnityEngine;

public class TargetFollower : MonoBehaviour {

    public Transform Target;
    public Vector3 Scale = Vector3.one;
    public Vector3 Offset;
    public bool IgnoreX;
    public bool IgnoreY;
    public bool IgnoreZ;

    private void Update ()
    {
        Vector3 newPos = Vector3.Scale(Target.position, Scale) + Offset;
        transform.position = new Vector3(
            IgnoreX ? transform.position.x : newPos.x,
            IgnoreY ? transform.position.y : newPos.y,
            IgnoreZ ? transform.position.z : newPos.z);
    }
}