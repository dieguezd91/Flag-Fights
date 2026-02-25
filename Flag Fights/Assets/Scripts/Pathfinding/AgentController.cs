using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    KnightController enemy;
    public float radius;
    public LayerMask maskNodes;
    public LayerMask maskObs;
    public Node target;

    private void Start()
    {
        enemy = GetComponent<KnightController>();
    }

    public void RunThetaStar()
    {
        var start = GetNearNode(transform.position);
        if (start == null)
        {
            Debug.Log("No se encontro un nodo cercano");
            return;
        }

        List<Node> path = ThetaStar.Run(start, GetConnections, IsSatiesfies, GetCost, Heuristic, InView);
        if (path.Count == 0)
        {
            return;
        }

        enemy.GetStateWaypoints.SetWayPoints(path);
        Debug.Log("Camino encontrado");
    }

    bool InView(Node grandParent, Node child)
    {
        return InView(grandParent.transform.position, child.transform.position);
    }

    bool InView(Vector3 a, Vector3 b)
    {
        Vector3 dir = b - a;
        return !Physics.Raycast(a, dir.normalized, dir.magnitude, maskObs);
    }

    float Heuristic(Node current)
    {
        return Vector3.Distance(current.transform.position, target.transform.position);
    }

    float GetCost(Node parent, Node child)
    {
        float cost = 0;
        float multiplierDistance = 1;
        float multiplierTrap = 200;
        cost += Vector3.Distance(parent.transform.position, child.transform.position) * multiplierDistance;
        if (child.hasTrap)
        {
            cost += multiplierTrap;
        }
        return cost;
    }

    Node GetNearNode(Vector3 pos)
    {
        var nodes = Physics.OverlapSphere(pos, radius, maskNodes);
        Node nearNode = null;
        float nearDistance = 0;
        for (int i = 0; i < nodes.Length; i++)
        {
            var currentNode = nodes[i];
            var dir = currentNode.transform.position - pos;
            float currentDistance = dir.magnitude;
            if (nearNode == null || currentDistance < nearDistance)
            {
                if (!Physics.Raycast(pos, dir.normalized, currentDistance, maskObs))
                {
                    nearNode = currentNode.GetComponent<Node>();
                    nearDistance = currentDistance;
                }
            }
        }
        if (nearNode == null)
        {
            Debug.Log("No se encontro un nodo cercano");
        }
        return nearNode;
    }

    List<Node> GetConnections(Node current)
    {
        return current.neighbours;
    }

    bool IsSatiesfies(Node current)
    {
        return current == target;
    }
}

