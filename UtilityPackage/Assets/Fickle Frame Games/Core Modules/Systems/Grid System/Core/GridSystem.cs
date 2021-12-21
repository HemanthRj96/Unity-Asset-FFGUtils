using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace FickleFrameGames.Systems
{
    /// <summary>
    /// Use this class to create grids of custom types
    /// </summary>
    /// <typeparam name="TGridType">Type of grid i.e., int, float, bool etc</typeparam>
    public class GridSystem<TGridType> where TGridType : Object
    {
        /*.............................................Private Fields.......................................................*/

        private Vector3 _offset;
        private TGridType[,] _grid;

        /*.............................................Properties...........................................................*/

        public int Height { private set; get; }
        public int Width { private set; get; }
        public Vector2 CellSize { private set; get; }

        /*.............................................Constructor..........................................................*/

        /// <summary>
        /// If you don't want to use text object or sprite renderers then set the last two parameters as false
        /// </summary>
        public GridSystem(Vector3 offset, int height, int width, Vector2 cellSize)
        {
            if (offset == default)
                offset = Vector3.zero;

            height = Mathf.Max(height, 0);
            width = Mathf.Max(width, 0);
            cellSize = new Vector2(Mathf.Max(cellSize.x, 0.001f), Mathf.Max(cellSize.y, 0.001f));

            _offset = offset;
            Height = height;
            Width = width;
            CellSize = cellSize;

            _grid = new TGridType[width, height];
        }

        /*.............................................Public Methods.......................................................*/

        /// <summary>
        /// Converts world position into grid sections
        /// </summary>
        /// <param name="worldPosition">Target position</param>
        /// <param name="x">Out parameter for x</param>
        /// <param name="y">Out parameter for y</param>
        public void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - _offset).x / CellSize.x);
            y = Mathf.FloorToInt((worldPosition - _offset).y / CellSize.y);
        }


        /// <summary>
        /// Converts grid sections to world points
        /// </summary>
        /// <param name="x">Target x</param>
        /// <param name="y">Target y</param>
        /// <returns></returns>
        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x * CellSize.x, y * CellSize.y) + _offset;
        }


        /// <summary>
        /// Returns a list of all cell centers
        /// </summary>
        public List<Vector3> GetCellCenters()
        {
            List<Vector3> cellCenters = new List<Vector3>();
            for (int y = 0; y < Height; ++y)
                for (int x = 0; x < Width; ++x)
                    cellCenters.Add(GetCellCenter(x, y));
            return cellCenters;
        }


        /// <summary>
        /// Returns the center of a cell
        /// </summary>
        /// <param name="x">X value of a cell</param>
        /// <param name="y">Y value of a cell</param>
        /// <returns></returns>
        public Vector3 GetCellCenter(int x, int y)
        {
            return (GetWorldPosition(x, y) + new Vector3(CellSize.x, CellSize.y) / 2);
        }


        /// <summary>
        /// Returns the center of a cell
        /// </summary>
        /// <param name="worldPosition">World position of the cell</param>
        /// <returns></returns>
        public Vector3 GetCellCenter(Vector3 worldPosition)
        {
            GetXY(worldPosition, out int x, out int y);
            return GetCellCenter(x, y);
        }


        /// <summary>
        /// Returns the value from the grid cell
        /// </summary>
        /// <param name="x">X section</param>
        /// <param name="y">Y section</param>
        public TGridType GetValue(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
                return _grid[x, y];
            return default;
        }


        /// <summary>
        /// Returns the value from the grid cell
        /// </summary>
        /// <param name="x">X section</param>
        /// <param name="y">Y section</param>
        public TGridType GetValue(Vector2 worldPosition)
        {
            GetXY(worldPosition, out int x, out int y);
            return GetValue(x, y);
        }


        /// <summary>
        /// Sets the value of the target grid section
        /// </summary>
        /// <param name="x">X section</param>
        /// <param name="y">Y section</param>
        /// <param name="value">Target value</param>
        public void SetValue(int x, int y, TGridType value)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height)
                _grid[x, y] = value;
        }


        /// <summary>
        /// Sets the value of the target grid sectio using world position
        /// </summary>
        /// <param name="worldPosition">World position</param>
        /// <param name="value">Target value</param>
        public void SetValue(Vector3 worldPosition, TGridType value)
        {
            GetXY(worldPosition, out int x, out int y);
            SetValue(x, y, value);
        }


        /// <summary>
        /// Call this method to draw the grid, to see the grid in the game toggle Gizmos
        /// </summary>
        /// <param name="color">Target color for the grid</param>
        /// <param name="duration">Default duration is 0 i.e., the grid will be drawn for a single frame</param>
        public void DrawGrid(Color color, float duration = 0)
        {
            UtilityMethods.DrawGrid(_offset, Height, Width, CellSize, duration, color);
        }
    }
}
