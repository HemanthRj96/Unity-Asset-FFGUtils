using UnityEngine;


namespace FFG.BehaviourTree
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        BehaviourTree tree;

        private void Start()
        {
            tree = ScriptableObject.CreateInstance<BehaviourTree>();

            var log1 = ScriptableObject.CreateInstance<DebugLogNode>();
            var log2 = ScriptableObject.CreateInstance<DebugLogNode>();
            var log3 = ScriptableObject.CreateInstance<DebugLogNode>();

            log1.Message = "Hi";
            log2.Message = "I'm";
            log3.Message = "Batman";

            var seq = ScriptableObject.CreateInstance<SequencerNode>();
            seq.Children.Add(log1);
            seq.Children.Add(log2);
            seq.Children.Add(log3);

            var loop = ScriptableObject.CreateInstance<RepeatNode>();
            loop.Child = seq;
            loop.RepeatCount = 1;
            loop.bUseCondition = false;

            tree.RootNode = loop;
        }

        private void Update()
        {
            tree.Update();
        }
    }
}