using System.Collections;
using UnityEngine;

namespace FickleFrames.Systems
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        public BehaviourTree tree;

        private void Start()
        {
            tree = ScriptableObject.CreateInstance<BehaviourTree>();

            var log1 = ScriptableObject.CreateInstance<DebugLogNode>();
            log1.LogMessage = "Testing Message # 1";
            var log2 = ScriptableObject.CreateInstance<DebugLogNode>();
            log2.LogMessage = "Testing Message # 2";
            var log3 = ScriptableObject.CreateInstance<DebugLogNode>();
            log3.LogMessage = "Testing Message # 3";

            var pause = ScriptableObject.CreateInstance<DelayNode>();
            pause.Delay = .01f;

            var sequence = ScriptableObject.CreateInstance<SequencerNode>();
            sequence.Children.Add(pause);
            sequence.Children.Add(log1);
            sequence.Children.Add(pause);
            sequence.Children.Add(log2);
            sequence.Children.Add(pause);
            sequence.Children.Add(log3);

            var loop = ScriptableObject.CreateInstance<RepeatNode>();
            loop.Child = sequence;
            //loop.RepeatCount = -1;

            tree.RootNode = loop;
        }

        private void Update()
        {
            tree.Update();
        }
    }
}