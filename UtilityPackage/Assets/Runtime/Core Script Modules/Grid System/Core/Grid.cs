using FickleFrames.Utility;
using TMPro;
using UnityEngine;

namespace FickleFrames
{
    /// <summary>
    /// Use this class to create grids of custom types
    /// </summary>
    /// <typeparam name="TGridType">Type of grid i.e., int, float, bool etc</typeparam>
    public class Grid<TGridType>
    {

        #region Private Fields

        private Vector3 offset;
        private TGridType[,] grid;
        private TextMeshPro[,] textGrid;
        private SpriteRenderer[,] spriteGrid;
        bool shouldUseText = true;
        bool shouldUseRenderer = true;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// If you don't want to use text object or sprite renderers then set the last two parameters as false
        /// </summary>
        public Grid(Vector3 offset, int height, int width, float cellSize, bool shouldUseText = true, bool shouldUseRenderer = true)
        {
            if (offset == default)
                offset = Vector3.zero;

            height = Mathf.Max(height, 0);
            width = Mathf.Max(width, 0);
            cellSize = Mathf.Max(cellSize, 0.1f);

            this.offset = offset;
            this.height = height;
            this.width = width;
            this.cellSize = cellSize;

            this.shouldUseText = shouldUseText;
            this.shouldUseRenderer = shouldUseRenderer;

            grid = new TGridType[width, height];
            if (shouldUseText)
                textGrid = new TextMeshPro[width, height];
            if (shouldUseRenderer)
                spriteGrid = new SpriteRenderer[width, height];
        }

        #endregion Public Constructors

        #region Public Properties

        public int height { private set; get; }
        public int width { private set; get; }
        public float cellSize { private set; get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Converts world position into grid sections
        /// </summary>
        /// <param name="worldPosition">Target position</param>
        /// <param name="x">Out parameter for x</param>
        /// <param name="y">Out parameter for y</param>
        public void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - offset).x / cellSize);
            y = Mathf.FloorToInt((worldPosition - offset).y / cellSize);
        }


        /// <summary>
        /// Converts grid sections to world points
        /// </summary>
        /// <param name="x">Target x</param>
        /// <param name="y">Target y</param>
        /// <returns></returns>
        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * cellSize + offset;
        }


        /// <summary>
        /// Sets the value of the target grid section
        /// </summary>
        /// <param name="x">X section</param>
        /// <param name="y">Y section</param>
        /// <param name="value">Target value</param>
        public void SetValue(int x, int y, TGridType value)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                grid[x, y] = value;
                textGrid[x, y].text = value.ToString();
            }
        }


        /// <summary>
        /// Sets the value of the target grid sectio using world position
        /// </summary>
        /// <param name="worldPosition">World position</param>
        /// <param name="value">Target value</param>
        public void SetValue(Vector3 worldPosition, TGridType value)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            SetValue(x, y, value);
        }


        /// <summary>
        /// Sets the text color of the renderer
        /// </summary>
        /// <param name="x">X section</param>
        /// <param name="y">Y section</param>
        /// <param name="color">Target color</param>
        public void SetTextColor(int x, int y, Color color)
        {
            if (shouldUseText && x >= 0 && y >= 0 && x < width && y < height)
            {
                textGrid[x, y].color = color;
            }
        }


        /// <summary>
        /// Sets the text color of the renderer from world position
        /// </summary>
        /// <param name="worldPosition">Target world position</param>
        /// <param name="color">Target color</param>
        public void SetTextColor(Vector3 worldPosition, Color color)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            SetTextColor(x, y, color);
        }


        /// <summary>
        /// Sets the color for the sprite renderer
        /// </summary>
        /// <param name="x">X section</param>
        /// <param name="y">Y section</param>
        /// <param name="color">Target color</param>
        public void SetRenderColor(int x, int y, Color color)
        {
            if (shouldUseRenderer && x >= 0 && y >= 0 && x < width && y < height)
            {
                spriteGrid[x, y].color = color;
            }
        }


        /// <summary>
        /// Sets the color for the sprite renderer
        /// </summary>
        /// <param name="worldPosition">Target world position</param>
        /// <param name="color">Target color</param>
        public void SetRenderColor(Vector3 worldPosition, Color color)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            SetRenderColor(x, y, color);
        }


        /// <summary>
        /// Call this method to draw the grid, to see the grid in the game toggle Gizmos
        /// </summary>
        /// <param name="color">Target color for the grid</param>
        /// <param name="duration">Default duration is 0 i.e., the grid will be drawn for a single frame</param>
        public void DrawGrid(Color color, float duration = 0)
        {
            UtilityMethods.DrawGrid(offset, height, width, cellSize, duration, color);
        }


        /// <summary>
        /// Call this method to draw the underlying values of the grid
        /// </summary>
        /// <param name="color">Color for the text</param>
        /// <param name="parentTransform">Parent transform which can be used as a handle to keep the scene hierarchy clean</param>
        public void DrawWorldText(Color color = default, Transform parentTransform = null)
        {
            if (!shouldUseText)
                return;
            if (color == default)
                color = Color.white;

            for (int x = 0; x < width; ++x)
                for (int y = 0; y < height; ++y)
                    textGrid[x, y] = UtilityMethods.CreateWorldText("", grid[x, y].ToString(), parentTransform, GetWorldPosition(x, y) + (Vector3.one * cellSize / 2), Vector2.one * cellSize, color);
        }


        /// <summary>
        /// Call this method to draw sprite renderer for each grid sections
        /// </summary>
        /// <param name="sprite">Target sprite you want to draw</param>
        /// <param name="color">Target color of the sprite</param>
        /// <param name="parent">Parent transform which can be used as a handle to keep the scene hierarchy clean</param>
        public void DrawBackground(Sprite sprite = null, Color color = default, Transform parent = null)
        {
            if (!shouldUseRenderer)
                return;
            if (sprite == null)
                return;
            if (color == default)
                color = Color.white;

            for (int x = 0; x < width; ++x)
                for (int y = 0; y < height; ++y)
                    spriteGrid[x, y] = UtilityMethods.CreateRenderer("", parent, sprite, GetWorldPosition(x, y) + (Vector3.one * cellSize / 2), Vector3.one * cellSize, color);
        }

        #endregion Public Methods
    }
}
