using FFG;
using UnityEditor;
using UnityEngine;


namespace FFG_Editors
{
    [CustomEditor(typeof(GridComponent))]
    public class GridComponentEditor : BaseEditor<GridComponent>
    {
        private static bool _enableDebugSettings = false;

        SerializedProperty _gridDimesion;
        SerializedProperty _cellDimesion;
        SerializedProperty _gridOffset;

        SerializedProperty _offsetType;
        SerializedProperty _presetType;
        SerializedProperty _shouldDrawGizmos;
        SerializedProperty _gizmoColor;

        public override void InspectorUpdate()
        {
            #region Init

            _gridDimesion = GetProperty("_gridDimension");
            _cellDimesion = GetProperty("_cellDimension");
            _gridOffset = GetProperty("_gridOffset");
            _offsetType = GetProperty("_offsetType");
            _presetType = GetProperty("_presetType");
            _shouldDrawGizmos = GetProperty("_shouldDrawGizmos");
            _gizmoColor = GetProperty("_gizmoColor");

            #endregion

            #region Grid settings

            Heading("Grid Settings");
            Space(10);

            _enableDebugSettings = EditorGUILayout.Toggle("Show debug settings : ", _enableDebugSettings);

            if (_shouldDrawGizmos.boolValue == false)
            {
                if (Button("Enable Grid View"))
                    _shouldDrawGizmos.boolValue = true;
            }
            else
            {
                if (Button("Disable Grid View"))
                    _shouldDrawGizmos.boolValue = false;
            }

            Space(20);

            PropertyField(_gridDimesion, "Grid Dimesions : ", "Width and hieght of the grid");
            PropertyField(_cellDimesion, "Cell Dimesions : ", "The size of a single cell");
            PropertyField(_offsetType, "Pivot Type : ", "");

            #endregion

            int h = _gridDimesion.vector2IntValue.x;
            int v = _gridDimesion.vector2IntValue.y;
            Vector2 cd = _cellDimesion.vector2Value;
            Transform t = Root.transform;

            #region Grid origin & debug settings

            // Grid origin
            switch (_offsetType.enumValueIndex)
            {
                case 0:
                    PropertyField(_presetType, "Select Preset Pivot : ", "");
                    switch (_presetType.enumValueIndex)
                    {
                        case 0:
                            _gridOffset.vector2Value = new Vector2(-h * cd.x, -v * cd.y);
                            break;
                        case 1:
                            _gridOffset.vector2Value = new Vector2(-h * cd.x / 2, -v * cd.y);
                            break;
                        case 2:
                            _gridOffset.vector2Value = new Vector2(0, -v * cd.y);
                            break;
                        case 3:
                            _gridOffset.vector2Value = new Vector2(-h * cd.x, -v * cd.y / 2);
                            break;
                        case 4:
                            _gridOffset.vector2Value = new Vector2(-h * cd.x / 2, -v * cd.y / 2);
                            break;
                        case 5:
                            _gridOffset.vector2Value = new Vector2(0, -v * cd.y / 2);
                            break;
                        case 6:
                            _gridOffset.vector2Value = new Vector2(-h * cd.x, 0);
                            break;
                        case 7:
                            _gridOffset.vector2Value = new Vector2(-h * cd.x / 2, 0);
                            break;
                        case 8:
                            _gridOffset.vector2Value = new Vector2(0, 0);
                            break;
                    }
                    break;
                case 1:
                    PropertyField(_gridOffset, "Pivot Point : ", "");
                    break;
            }

            // Debug settings
            if (_enableDebugSettings)
            {
                Space(15);
                Heading("Debug Settings");

                Space(10);
                _gizmoColor.colorValue = EditorGUILayout.ColorField("Grid line color : ", _gizmoColor.colorValue);
            }

            #endregion
        }

        private void OnSceneGUI()
        {
            Event e = Event.current;
            SceneView scene = SceneView.currentDrawingSceneView;

            if (e.isMouse)
            {
                var mousePosition = e.mousePosition;
                var ppp = EditorGUIUtility.pixelsPerPoint;

                mousePosition.y = scene.camera.pixelHeight - mousePosition.y * ppp;
                mousePosition.x = mousePosition.x * ppp;
                mousePosition = scene.camera.ScreenToWorldPoint(mousePosition);
            }
        }
    }
}