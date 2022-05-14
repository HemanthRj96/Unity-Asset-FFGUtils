using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


namespace FFG.BehaviourTree.Internal
{

    public class BehaviourTreeEditor : EditorWindow
    {
        private BehaviourTreeView _treeView;
        private InspectorView _inspectorView;


        [MenuItem("Behaviour Tree/Editor...")]
        public static void OpenWindow()
        {
            BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
            wnd.titleContent = new GUIContent("Behaviour Tree Editor");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/FFG/Core Modules/Behaviour Tree/Editor Window/BehaviourTreeEditor.uxml");
            visualTree.CloneTree(root);

            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/FFG/Core Modules/Behaviour Tree/Editor Window/BehaviourTreeEditor.uss");
            root.styleSheets.Add(styleSheet);

            _treeView = root.Q<BehaviourTreeView>();
            _inspectorView = root.Q<InspectorView>();

            //OnSelectionChange();
        }

        //private void OnSelectionChange()
        //{
        //    //BehaviourTree tree = Selection.activeObject as BehaviourTree;
            
        //    if(tree)
        //    {
        //        _treeView.PopulateView(tree);
        //    }
        //}
    } 
}