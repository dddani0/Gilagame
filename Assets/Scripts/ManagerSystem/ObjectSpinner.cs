using UnityEngine;

public static class ObjectSpinner
{
    private const int Offset = 1;

    public static bool IsObjectGoingModulus(float horizontalInput) => horizontalInput > 0;

    public static float ObjectRotationAngle(Vector3 objectPosition, Vector3 upwardVectorAngle, Vector3 newAngle,
        float xInput)
    {
        return Vector3.Angle(upwardVectorAngle, newAngle - objectPosition) *
               (IsObjectGoingModulus(xInput) ? 1 : -1);
    }

    public static float AngleBetweenObjects(Vector3 from, Vector3 to) =>
        Vector3.SignedAngle(from + Vector3.forward, (to - from).normalized, Vector3.up);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="from">Vector2.up is an addition to the calculation.</param>
    /// <param name="to"></param>
    /// <returns></returns>
    public static float AngleBetweenObjects(Vector2 from, Vector2 to) =>
        Vector2.SignedAngle(from + Vector2.up, (to - from).normalized);

    /// <summary>
    /// The direction vector of two positions. Returns a two dimensional vector.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public static Vector2 DirectionVector(Vector2 from, Vector2 to) => Vector3.Normalize(to - from);

    /// <summary>
    /// The direction vector of two positions. Returns a three dimensional vector.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public static Vector3 DirectionVector(Vector3 from, Vector3 to) => (to - from).normalized;

    /// <summary>
    /// Spins the spinningObject with rotationDelta, according to x and y input with, such as with a direction vector. The spinning object must be the child of correlatedObject.
    /// </summary>
    /// <param name="correlatedObject">Parent of the spinning object</param>
    /// <param name="spinningObject">The spinning object.</param>
    /// <param name="xInput"></param>
    /// <param name="yInput"></param>
    /// <param name="rotationDelta">Spinning value</param>
    public static void SpinObject(Transform correlatedObject, Transform spinningObject, float xInput, float yInput,
        float rotationDelta)
    {
        Vector3 FollowDirection(Vector3 objectPosition)
            => objectPosition + new Vector3(Offset * xInput * Time.deltaTime, 0, Offset * yInput * Time.deltaTime);

        var spinningObjectRotation = spinningObject.eulerAngles;
        var spinningObjectPosition = spinningObject.position;
        var targetRotation = ObjectRotationAngle(spinningObjectPosition, correlatedObject.forward,
            FollowDirection(spinningObjectPosition), xInput);
        var turnSpeed = rotationDelta * Time.deltaTime;

        spinningObjectRotation.y = Mathf.MoveTowardsAngle(spinningObjectRotation.y,
            targetRotation, turnSpeed);

        spinningObject.transform.eulerAngles = spinningObjectRotation;
    }

    /// <summary>
    /// Spins the spinningObject facing the target.
    /// </summary>
    /// <param name="correlatedObject">Parent of the spinning object.</param>
    /// <param name="spinningObject"></param>
    /// <param name="target">The player will rotate towards the target.</param>
    public static void SpinObject(Transform correlatedObject, Transform spinningObject, Vector3 target)
    {
        var directionAngle = AngleBetweenObjects(correlatedObject.position, target);
        spinningObject.transform.localEulerAngles = new Vector3(0,
            directionAngle, 0);
    }

    public static void SpinObject(Transform correlatedObject, Transform spinningObject, Vector2 target)
    {
        var correlatedObjectPosition = new Vector2(correlatedObject.position.x, correlatedObject.position.y);
        var directionAngle = AngleBetweenObjects(correlatedObjectPosition, target);
        spinningObject.transform.localEulerAngles = new Vector3(0, 0, directionAngle);
    }
}