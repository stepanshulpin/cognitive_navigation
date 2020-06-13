using UnityEngine;

public class EvolutionDynamicTargetManager : MonoBehaviour {
    private void Start() {
        this.agentsManager = GetComponent<EvolutionDynamicAgentsManager>();
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

    private EvolutionDynamicAgentsManager agentsManager;

    private static readonly Vector3 TARGET_OFFSET = new Vector3(135.0f, 0.0f, 0.0f);

    private static readonly float MIN_DISTANCE_TO_TARGET_FOR_UPDATE = 50.0f;
}
