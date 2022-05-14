using FFG;
using System.Collections.Generic;
using UnityEngine;


public static class GridExtensionHelpers
{
    /// <summary>
    /// Returns true if the cells and the dimensions of the two grid are the same
    /// </summary>
    public static bool IsSame(this FFG.Grid gridA, FFG.Grid gridB)
    {
        bool validDimensions = true;

        if (gridA != null && gridB != null)
        {
            validDimensions &= gridA.TotalCellCount == gridB.TotalCellCount;
            validDimensions &= gridA.GridOrigin == gridB.GridOrigin;
            validDimensions &= gridA.GridDimension == gridB.GridDimension;
            validDimensions &= gridA.CellDimension == gridB.CellDimension;
        }
        else return false;


        if (validDimensions == true)
        {
            if (gridA.GetCellsRaw() != null && gridB.GetCellsRaw() != null)
            {
                for (int v = 0; v < gridA.GridDimension.y; ++v)
                    for (int h = 0; h < gridA.GridDimension.x; ++h)
                        if (gridA.GetCellsRaw()[h, v] != gridB.GetCellsRaw()[h, v])
                            return false;
            }
            else return false;
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// Returns true if the value in the cell is same
    /// </summary>
    public static bool IsSame(this Cell cellA, Cell cellB)
    {
        bool result = true;

        result &= cellA.IsValid() == cellB.IsValid();
        result &= cellA.GetCellData() == cellB.GetCellData();

        return result;
    }

    /// <summary>
    /// Returns true if the grid overlaps with another grid. The out parameter will have all the cells this grid overlaps
    /// </summary>
    /// <param name="otherGrid">Other grid</param>
    /// <param name="overlappedCells">Cells that are overlapped</param>
    public static bool Overlaps(this FFG.Grid thisGrid, FFG.Grid otherGrid, out Cell[,] overlappedCells)
    {
        Rect gridABound = thisGrid.GetGridBounds();
        Rect gridBBound = otherGrid.GetGridBounds();
        bool bDoesGridOverlap = gridABound.Overlaps(gridBBound);

        if (bDoesGridOverlap)
        {
            overlappedCells = new Cell[thisGrid.GridDimension.x, thisGrid.GridDimension.y];
            var cellBounds = thisGrid.GetCellsBounds();
            var gridDimension = thisGrid.GridDimension;

            for (int x = 0; x < gridDimension.x; ++x)
                for (int y = 0; y < gridDimension.y; ++y)
                {
                    if (thisGrid.GetCell(x, y).IsValid() && cellBounds[x, y].Overlaps(gridBBound))
                        overlappedCells[x, y] = thisGrid.GetCellsRaw()[x, y];
                    else
                        overlappedCells[x, y] = null;
                }
        }
        else
            overlappedCells = null;
        return bDoesGridOverlap;
    }

    /// <summary>
    /// Returns true if the grid overlaps with another grid. The out parameter will have all the cells this grid overlaps
    /// </summary>
    /// <param name="otherGridComponent">Other grid component</param>
    /// <param name="overlappedCells">Cells that are overlapped</param>
    public static bool Overlaps(this FFG.Grid thisGrid, FFG.GridComponent otherGridComponent, out Cell[,] overlappedCells)
    {
        Rect gridABound = thisGrid.GetGridBounds();
        Rect gridBBound = otherGridComponent.GetGridBounds();
        bool bDoesGridOverlap = gridABound.Overlaps(gridBBound);

        if (bDoesGridOverlap)
        {
            overlappedCells = new Cell[thisGrid.GridDimension.x, thisGrid.GridDimension.y];
            var cellBounds = thisGrid.GetCellsBounds();
            var gridDimension = thisGrid.GridDimension;

            for (int x = 0; x < gridDimension.x; ++x)
                for (int y = 0; y < gridDimension.y; ++y)
                {
                    if (thisGrid.GetCell(x, y).IsValid() && cellBounds[x, y].Overlaps(gridBBound))
                        overlappedCells[x, y] = thisGrid.GetCellsRaw()[x, y];
                    else
                        overlappedCells[x, y] = null;
                }
        }
        else
            overlappedCells = null;
        return bDoesGridOverlap;
    }

    /// <summary>
    /// Returns true if the grid overlaps with another grid. The out parameter will have all the cells this grid overlaps
    /// </summary>
    /// <param name="otherGridComponent">Other grid component</param>
    /// <param name="overlappedCells">Cells that are overlapped</param>
    public static bool Overlaps(this FFG.GridComponent thisGridComponent, FFG.GridComponent otherGridComponent, out Cell[,] overlappedCells)
    {
        Rect gridABound = thisGridComponent.GetGridBounds();
        Rect gridBBound = otherGridComponent.GetGridBounds();
        bool bDoesGridOverlap = gridABound.Overlaps(gridBBound);

        if (bDoesGridOverlap)
        {
            overlappedCells = new Cell[thisGridComponent.GridDimension.x, thisGridComponent.GridDimension.y];
            var cellBounds = thisGridComponent.GetCellsBounds();
            var gridDimension = thisGridComponent.GridDimension;

            for (int x = 0; x < gridDimension.x; ++x)
                for (int y = 0; y < gridDimension.y; ++y)
                {
                    if (thisGridComponent.GetCell(x, y).IsValid() && cellBounds[x, y].Overlaps(gridBBound))
                        overlappedCells[x, y] = thisGridComponent.GetCellsRaw()[x, y];
                    else
                        overlappedCells[x, y] = null;
                }
        }
        else
            overlappedCells = null;
        return bDoesGridOverlap;
    }

    /// <summary>
    /// Returns true if the grid overlaps with another grid. The out parameter will have all the cells this grid overlaps
    /// </summary>
    /// <param name="otherGrid">Other grid</param>
    /// <param name="overlappedCells">Cells that are overlapped</param>
    public static bool Overlaps(this FFG.GridComponent thisGridComponent, FFG.Grid otherGrid, out Cell[,] overlappedCells)
    {
        Rect gridABound = thisGridComponent.GetGridBounds();
        Rect gridBBound = otherGrid.GetGridBounds();
        bool bDoesGridOverlap = gridABound.Overlaps(gridBBound);

        if (bDoesGridOverlap)
        {
            overlappedCells = new Cell[thisGridComponent.GridDimension.x, thisGridComponent.GridDimension.y];
            var cellBounds = thisGridComponent.GetCellsBounds();
            var gridDimension = thisGridComponent.GridDimension;

            for (int x = 0; x < gridDimension.x; ++x)
                for (int y = 0; y < gridDimension.y; ++y)
                {
                    if (thisGridComponent.GetCell(x, y).IsValid() && cellBounds[x, y].Overlaps(gridBBound))
                        overlappedCells[x, y] = thisGridComponent.GetCellsRaw()[x, y];
                    else
                        overlappedCells[x, y] = null;
                }
        }
        else
            overlappedCells = null;
        return bDoesGridOverlap;
    }
}
