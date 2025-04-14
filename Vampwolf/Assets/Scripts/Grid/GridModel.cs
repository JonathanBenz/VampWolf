using UnityEngine;

namespace Vampwolf.Grid
{
    public class GridModel
    {
        public int Width;
        public int Height;
        public GridCell[,] Cells;

        public GridModel(int width, int height)
        {
            // Set the properties of the model
            Width = width;
            Height = height;
            Cells = new GridCell[width, height];

            // Initialize the cells
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Cells[x, y] = new GridCell()
                    {
                        Posiiton = new Vector2Int(x, y),
                        IsWalkable = true
                    };
                }
            }
        }
    }
}
