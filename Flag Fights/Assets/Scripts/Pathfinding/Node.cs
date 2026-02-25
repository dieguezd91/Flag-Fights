using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> neighbours;//<- Esto es lo unico que importa


    //Si utilizan este codigo con los raycast en el start/update/realtime son un punto menos por raycast.
    public bool hasTrap;

    private void Start()
    {
        neighbours.RemoveAll(node => node == this);
    }

}
