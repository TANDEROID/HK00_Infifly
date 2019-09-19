using UnityEngine;

public class SelfMovingAnimator : MonoBehaviour
{
    public Transform Holder;

    public void SetAnimatorToHolderPosition()
    {
        transform.position = Holder.position;
        Holder.localPosition = Vector3.zero;
    }

}