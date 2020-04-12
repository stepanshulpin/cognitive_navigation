using UnityEngine;

public class FollowingCamera : MonoBehaviour {

    public Transform target;

    public float smoothSpeed = 5.2f;

    public Vector3 offset;

    public bool followTargetLook = false;

    private void Start() {
        if (this.target != null) {
            this.transform.position = this.target.position + this.offset;
        }
    }

    private void LateUpdate() {
        if (this.target != null) {
            Vector3 desiredPosition = this.target.position + this.offset;
            Vector3 smoothedPosition = Vector3.Lerp(this.transform.position, desiredPosition, this.smoothSpeed * Time.deltaTime);
            this.transform.position = smoothedPosition;
            if (this.followTargetLook) {
                this.transform.LookAt(this.target);
            }
        }
    }

}
