using System.Collections.Generic;
using UnityEngine;


namespace FFG
{
    /// <summary>
    /// Use this class to create grids of custom types
    /// </summary>
    [System.Serializable]
    public class Grid
    {
        #region Constructors


        public Grid() { }

        public Grid(Vector3 origin, int horizontalCellCount, int verticalCellCount, Vector2 cellSize)
        {
            _gridDimension = new Vector2Int(Mathf.Max(horizontalCellCount, 0), Mathf.Max(verticalCellCount, 0));
            _cellDimension = new Vector2(Mathf.Max(cellSize.x, 0.001f), Mathf.Max(cellSize.y, 0.001f));
            _origin = origin;
            _cells = new Cell[horizontalCellCount, verticalCellCount];

            for (int x = 0; x < horizontalCellCount; x++)
                for (int y = 0; y < verticalCellCount; y++)
                    _cells[x, y] = new Cell(new Vector2Int(x, y));
        }


        #endregion
        #region Fields & Properties


        private Vector2 _origin = new Vector2();
        private Vector2Int _gridDimension = new Vector2Int();
        private Vector2 _cellDimension = new Vector2();
        private Cell[,] _cells = null;

        public Vector2 GridOrigin => _origin;
        public Vector2Int GridDimension => _gridDimension;
        public Vector2 CellDimension => _cellDimension;
        public int TotalCellCount => _gridDimension.x * _gridDimension.y;


        #endregion
        #region Public Methods


        /// <summary>
        /// Returns true if x and y is inside the grid
        /// </summary>
        /// <param name="x">X value of the grid</param>
        /// <param name="y">Y value of the grid</param>
        public bool IsInside(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < _gridDimension.x && y < _gridDimension.y)
                return true;
            return false;
        }

        /// <summary>
        /// Returns true if the point is inside the grid
        /// </summary>
        /// <param name="worldPosition">Target world position</param>
        public bool IsInside(Vector3 worldPosition)
        {
            GetXY(worldPosition, out int x, out int y);
            return IsInside(x, y);
        }

        /// <summary>
        /// Converts world position into grid sections
        /// </summary>
        /// <param name="worldPosition">Target position</param>
        /// <param name="x">Out parameter for x</param>
        /// <param name="y">Out parameter for y</param>
        public void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - (Vector3)_origin).x / _cellDimension.x);
            y = Mathf.FloorToInt((worldPosition - (Vector3)_origin).y / _cellDimension.y);
        }

        /// <summary>
        /// Converts grid sections to world points
        /// </summary>
        /// <param name="x">Target x</param>
        /// <param name="y">Target y</param>
        /// <returns></returns>
        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector2(x * _cellDimension.x, y * _cellDimension.y) + _origin;
        }

        /// <summary>
        /// Returns the grid bounds
        /// </summary>
        public Rect GetGridBounds()
        {
            return new Rect(GetWorldPosition(0, 0), new Vector2(_cellDimension.x * _gridDimension.x, _cellDimension.y * _gridDimension.y));
        }

        /// <summary>
        /// Returns the target cell's bounds
        /// </summary>
        /// <param name="x">Target x value</param>
        /// <param name="y">Target y value</param>
        public Rect GetCellBounds(int x, int y)
        {
            if (IsInside(x, y))
                return new Rect(GetWorldPosition(x, y), _cellDimension);
            return new Rect(0, 0, -1, -1);
        }

        /// <summary>
        /// Returns the bounds of all cells
        /// </summary>
        public Rect[,] GetCellsBounds()
        {
            Rect[,] bounds = new Rect[_gridDimension.x, _gridDimension.y];
            for (int x = 0; x < _gridDimension.x; ++x)
                for (int y = 0; y < _gridDimension.y; ++y)
                    bounds[x, y] = GetCellBounds(x, y);
            return bounds;
        }

        /// <summary>
        /// Returns a list of all cell centers
        /// </summary>
        public Vector3[,] GetCellCenters()
        {
            Vector3[,] cellCenters = new Vector3[_gridDimension.x, _gridDimension.y];
            for (int y = 0; y < _gridDimension.y; ++y)
                for (int x = 0; x < _gridDimension.x; ++x)
                    cellCenters[x, y] = GetCellCenter(x, y);
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
            return (GetWorldPosition(x, y) + new Vector3(_cellDimension.x, _cellDimension.y) / 2);
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
        public object GetValue(int x, int y)
        {
            if (IsInside(x, y))
                return _cells[x, y].GetCellData();
            return default;
        }

        /// <summary>
        /// Returns the value from the grid cell
        /// </summary>
        /// <param name="x">X section</param>
        /// <param name="y">Y section</param>
        public object GetValue(Vector2 worldPosition)
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
        public void SetValue(int x, int y, object value)
        {
            if (x >= 0 && y >= 0 && x < _gridDimension.y && y < _gridDimension.x)
                _cells[x, y].SetCellData(value);
        }

        /// <summary>
        /// Sets the value of the target grid sectio using world position
        /// </summary>
        /// <param name="worldPosition">World position</param>
        /// <param name="value">Target value</param>
        public void SetValue(Vector3 worldPosition, object value)
        {
            GetXY(worldPosition, out int x, out int y);
            SetValue(x, y, value);
        }

        /// <summary>
        /// Makes the cell valid
        /// </summary>
        /// <param name="x">X component</param>
        /// <param name="y">Y component</param>
        public void EnableCell(int x, int y)
        {
            if (IsInside(x, y))
                _cells[x, y].EnableCell();
        }

        /// <summary>
        /// Makes the cell valid
        /// </summary>
        /// <param name="worldPosition">World position</param>
        public void EnableCell(Vector2 worldPosition)
        {
            GetXY(worldPosition, out int x, out int y);
            EnableCell(x, y);
        }

        /// <summary>
        /// Makes the cell invalid
        /// </summary>
        /// <param name="x">X component</param>
        /// <param name="y">Y component</param>
        public void DisableCell(int x, int y)
        {
            if (IsInside(x, y))
                _cells[x, y].DisableCell();
        }

        /// <summary>
        /// Makes the cell invalid
        /// </summary>
        /// <param name="worldPosition">World position</param>
        public void DisableCell(Vector2 worldPosition)
        {
            GetXY(worldPosition, out int x, out int y);
            DisableCell(x, y);
        }

        /// <summary>
        /// Overrided
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string retValue = "";

            for (int v = 0; v < _gridDimension.y; ++v)
            {
                for (int h = 0; h < _gridDimension.x; ++h)
                    retValue += _cells[h, v].ToString() + " ";
                retValue += "\n";
            }
            return retValue;
        }

        /// <summary>
        /// Returns cell array
        /// </summary>
        public Cell[,] GetCellsRaw() => _cells;

        /// <summary>
        /// Returns cell from the grid
        /// </summary>
        /// <param name="x">X Value</param>
        /// <param name="y">Y Value</param>
        public Cell GetCell(int x, int y)
        {
            if (IsInside(x, y))
                return _cells[x, y];
            return null;
        }


        #endregion
    }
}
