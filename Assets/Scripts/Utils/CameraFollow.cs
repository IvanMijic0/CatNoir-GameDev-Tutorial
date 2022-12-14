using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float interpolationVelocity;
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 offset;

    private Vector3 _targetPos;

    private void Start () {
        _targetPos = transform.position;
    }

    private void FixedUpdate ()
    {
        if (!target) return;

        var position = transform.position;
        var posNoZ = position;
        var targetPosition = target.transform.position;
        
        posNoZ.z = targetPosition.z;

        var targetDirection = (targetPosition - posNoZ);

        interpolationVelocity = targetDirection.magnitude * 5f;

        _targetPos = position + (targetDirection.normalized * (interpolationVelocity * Time.deltaTime)); 

        position = Vector3.Lerp( position, _targetPos + offset, 0.25f);
        transform.position = position;
    }
}
