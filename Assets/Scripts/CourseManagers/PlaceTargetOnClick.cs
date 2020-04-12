using UnityEngine;

public class PlaceTargetOnClick : MonoBehaviour {

    public GameObject targetPrefab;

    public LayerMask walkableSurfaceMask;

    public Transform Target {
        get {
            return this.target.transform;
        }
    }

    private void Update() {
        if (Input.GetMouseButtonUp(LEFT_MOUSE_BUTTON)) {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit, 100.0f)) {
                int raycastedSurface = raycastHit.collider.gameObject.layer;
                LayerMask surfaceLayerMask = LayerMask.GetMask(LayerMask.LayerToName(raycastedSurface));
                if (surfaceLayerMask.Equals(this.walkableSurfaceMask)) {
                    this.TargetPositionUpdated(raycastHit.point);
                } else {
                    Debug.Log("Can't place target because the surface is not walkable");
                }
            }
        }
    }

    private void TargetPositionUpdated(Vector3 position) {
        if (this.target == null) {
            this.target = Instantiate(this.targetPrefab);
        }
        this.target.transform.position = position;
        foreach (GameObject agent in GameObject.FindGameObjectsWithTag("Agent")) {
            agent.GetComponent<Agent>().TargetPositionUpdated(position);
        }
    }

    private GameObject target;

    private static readonly int LEFT_MOUSE_BUTTON = 0;
}
