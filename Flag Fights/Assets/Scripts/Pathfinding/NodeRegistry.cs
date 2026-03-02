using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeRegistry : MonoBehaviour
{
    List<Node> _nodes;

    void Awake()
    {
        _nodes = FindObjectsOfType<Node>().ToList();
    }

    public Node GetRandomNode()
    {
        if (_nodes == null || _nodes.Count == 0) return null;
        return _nodes[Mathf.FloorToInt(MyRandoms.Range(0, _nodes.Count))];
    }
}
