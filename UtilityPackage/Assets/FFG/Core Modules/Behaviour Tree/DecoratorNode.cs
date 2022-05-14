using System.Collections;
using UnityEngine;

namespace FFG.BehaviourTree
{
    public abstract class DecoratorNode : Node
    {
        public Node Child;
    }
}