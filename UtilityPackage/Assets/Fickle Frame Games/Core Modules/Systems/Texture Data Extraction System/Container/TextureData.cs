using UnityEngine;


namespace FickleFrameGames.Systems
{
    /// <summary>
    /// Data container to hold color value and color coordinates of a 2D texture map
    /// </summary>
    public struct TextureData
    {
        public TextureData(Color pixelColor, Vector2Int textureCoords)
        {
            PixelColor = pixelColor;
            TextureCoords = textureCoords;
        }

        public Color PixelColor;
        public Vector2Int TextureCoords;
    }
}
