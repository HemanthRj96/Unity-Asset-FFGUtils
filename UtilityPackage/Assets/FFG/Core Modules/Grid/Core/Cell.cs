using UnityEngine;

namespace FFG
{
    [System.Serializable]
    public class Cell
    {
        #region Constructors


        public Cell() { }

        public Cell(Vector2Int cellIndex)
        {
            _isValid = true;
            _data = default;
            _cellIndex = cellIndex;
        }


        #endregion
        #region Fields & Properties


        private bool _isValid = true;
        private object _data = default;
        private Vector2Int _cellIndex = Vector2Int.zero;


        #endregion
        #region Public Methods


        /// <summary>
        /// Call to enable this cell
        /// </summary>
        public void EnableCell() => _isValid = true;

        /// <summary>
        /// Call to disable this cell
        /// </summary>
        public void DisableCell() => _isValid = false;

        /// <summary>
        /// Returns if the cell is valid or not
        /// </summary>
        public bool IsValid() => _isValid;

        /// <summary>
        /// Returns cell data if the cell is valid otherwise returns the default value of data
        /// </summary>
        public object GetCellData() => _isValid ? _data : default;

        /// <summary>
        /// Method to set data inside this cell
        /// </summary>
        /// <param name="data">Target data</param>
        public void SetCellData(object data) => _data = data;

        /// <summary>
        /// Returns the index of this cell
        /// </summary>
        public Vector2Int GetCellIndex() => _cellIndex;

        /// <summary>
        /// Override method to format the values inside the cell
        /// </summary>
        public override string ToString()
        {
            string dataValue = _data == null ? "-" : _data.ToString();
            return $"{dataValue}:{_isValid}";
        }

        #endregion
    }
}