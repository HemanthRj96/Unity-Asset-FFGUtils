using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace FickleFrameGames.Systems
{
    /// <summary>
    /// Class used to get color value and the texture coordinates of these unique colors
    /// </summary>
    public static class TextureDataExtractor
    {
        /// <summary>
        /// Returns TextureData array which has all information about Color and Coordinates of the passed Texture
        /// </summary>
        /// <param name="texture">Texture from which data has to be parsed</param>
        public static TextureData[] GetTextureData(Texture2D texture)
        {
            if (!texture.isReadable)
            {
                Debug.LogWarning("Texture is not readable, please set Read/Write as true in the advanced settings for this texture");
                return null;
            }

            Dictionary<Color, Vector2Int> colorLookup = new Dictionary<Color, Vector2Int>();
            List<TextureData> data = new List<TextureData>();

            for (int x = 0; x < texture.width; ++x)
                for (int y = 0; y < texture.height; ++y)
                {
                    Color color = texture.GetPixel(x, y);
                    if (!colorLookup.ContainsKey(color))
                        colorLookup.Add(color, new Vector2Int(x, y));
                }
            foreach (var colorData in colorLookup.ToList())
                data.Add(new TextureData(colorData.Key, colorData.Value));

            return data.ToArray();
        }
    }
}