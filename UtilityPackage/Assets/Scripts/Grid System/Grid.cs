using System.Collections;
using UnityEngine;

public class Grid
{
    public Grid(int height, int width, float cellSize)
    {
        this.height = height;
        this.width = width;
        this.cellSize = cellSize;
    }

    private int height;
    private int width;
    private float cellSize;
}
