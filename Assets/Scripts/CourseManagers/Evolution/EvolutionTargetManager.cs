using UnityEngine;

public class EvolutionTargetManager : MonoBehaviour {
    private void Start() {
        this.agentsManager = GetComponent<EvolutionAgentsManager>();
        this.UpdateTargetPosition();
    }

    private void LateUpdate() {
        if (!agentsManager.isRestarting)
        {
            foreach (GameObject agent in this.agentsManager.AgentsObjects)
            {
                if ((targetPosition - agent.transform.position).magnitude <= MIN_DISTANCE_TO_TARGET_FOR_UPDATE)
                {
                    this.UpdateTargetPosition();
                    break;
                }
            }
        }
    }

    public void restart()
    {
        this.targetPosition = Vector3.zero;
        this.targetPosition += TARGET_OFFSET;
        this.agentsManager.TargetPositionUpdated(this.targetPosition);
    }

    private void UpdateTargetPosition() {
        this.targetPosition += TARGET_OFFSET;
        this.agentsManager.TargetPositionUpdated(this.targetPosition);
    }

    private Vector3 targetPosition = Vector3.zero;

    private EvolutionAgentsManager agentsManager;

    private static readonly Vector3 TARGET_OFFSET = new Vector3(35.0f, 0.0f, 0.0f);

    private static readonly float MIN_DISTANCE_TO_TARGET_FOR_UPDATE = 10.0f;
}
