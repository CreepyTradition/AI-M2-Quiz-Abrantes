using UnityEngine;
using UnityEngine.AI;

public class ArmyFormationController : MonoBehaviour
{
    public int rows = 3;
    public int columns = 5;
    public float spacing = 2.5f;
    public float formationDistance = 5.0f;
    public string playerTag = "Player";
    public float positionCheckRadius = 1.0f;

    private Transform player;
    private NavMeshAgent[] agents;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj == null)
        {
            Debug.LogError("No GameObject with tag '" + playerTag + "' found in scene.");
            return;
        }

        player = playerObj.transform;
        agents = GetComponentsInChildren<NavMeshAgent>();
    }

    void Update()
    {
        if (player == null || agents == null) return;

        Vector3 forward = player.forward;
        Vector3 right = player.right;
        Vector3 basePosition = player.position + forward * formationDistance;

        int agentIndex = 0;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (agentIndex >= agents.Length) return;

                Vector3 offset = (right * ((col - (columns - 1) / 2.0f) * spacing)) - (forward * (row * spacing));
                Vector3 targetPos = basePosition + offset;

                // Check if the position is clear using a sphere check
                if (!IsPositionBlocked(targetPos))
                {
                    agents[agentIndex].SetDestination(targetPos);
                    agentIndex++;
                }
            }
        }
    }

    bool IsPositionBlocked(Vector3 position)
    {
        Collider[] hits = Physics.OverlapSphere(position, positionCheckRadius);
        foreach (Collider hit in hits)
        {
            if (hit.gameObject != null && hit.gameObject.name.Contains("AgentAI"))
            {
                return true;
            }
        }
        return false;
    }
}
