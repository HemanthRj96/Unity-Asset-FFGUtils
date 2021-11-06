using System.Collections.Generic;

namespace FickleFrames.Systems.Internal
{
    public abstract class CompositeNode : Node
    {
        public List<Node> Children = new List<Node>();
    }
}