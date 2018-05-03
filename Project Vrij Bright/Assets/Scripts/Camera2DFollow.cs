using UnityEngine;

public class Camera2DFollow : MonoBehaviour
{
    public Transform[] targets;
    public float damping = 1;
    public float lookAheadFactor = 3;
    public float lookAheadReturnSpeed = 0.5f;
    public float lookAheadMoveThreshold = 0.1f;

    private float offsetZ;
    public float offsetY;
    private Vector3 lastTargetPosition;
    private Vector3 currentVelocity;
    private Vector3 lookAheadPos;

    // Use this for initialization
    private void Start()
    {
        lastTargetPosition = (targets[0].position + targets[1].position) / 2;
        offsetZ = (transform.position - (targets[0].position + targets[1].position) / 2).z;
        transform.parent = null;
    }

    // Update is called once per frame
    private void Update()
    {
        // only update lookahead pos if accelerating or changed direction
        float xMoveDelta = ((targets[0].position + targets[1].position) / 2 - lastTargetPosition).x;

        bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

        if (updateLookAheadTarget)
        {
            lookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
        }
        else
        {
            lookAheadPos = Vector3.MoveTowards(lookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
        }

        Vector3 aheadTargetPos = (targets[0].position + targets[1].position) / 2 + lookAheadPos + Vector3.forward * offsetZ;
        Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref currentVelocity, damping);

        transform.position = newPos;
        if (transform.position.y < 1.5)
        {
            transform.position = new Vector3(transform.position.x, offsetY, transform.position.z);
        }

        lastTargetPosition = (targets[0].position + targets[1].position) / 2;
    }
}