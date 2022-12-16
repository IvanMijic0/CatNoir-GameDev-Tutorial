using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float interpolationVelocity;
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 offset;

    [SerializeField] private Vector3 minValues, maxValues;

    private Vector3 _targetPos;

    private void Start () {
        _targetPos = transform.position;
    }

    private void FixedUpdate ()
    {
        if (!target) return;

        var position = transform.position;
        var posNoZ = position;
        var targetPosition = target.transform.position + offset;

        Vector3 boundPosition = new Vector3(
        Mathf.Clamp(targetPosition.x, minValues.x, maxValues.x), 
        Mathf.Clamp(targetPosition.y, minValues.y, maxValues.y),
        Mathf.Clamp(targetPosition.z, minValues.z, maxValues.z));
        
        posNoZ.z = targetPosition.z;

        var targetDirection = (targetPosition - posNoZ);

        interpolationVelocity = targetDirection.magnitude * 5f;

        _targetPos = position + (targetDirection.normalized * (interpolationVelocity * Time.deltaTime)); 

        position = Vector3.Lerp(transform.position, boundPosition + offset, 0.25f);
        transform.position = position;
    }
}
