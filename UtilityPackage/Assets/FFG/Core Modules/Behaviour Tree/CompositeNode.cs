using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FFG.BehaviourTree
{
    public abstract class CompositeNode : Node
    {
        public List<Node> Children = new List<Node>();
    }
}