using UnityEngine.UIElements;


namespace FFG.BehaviourTree.Internal
{
    public class SplitView : TwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> { }

        public SplitView() {  }
    }
}