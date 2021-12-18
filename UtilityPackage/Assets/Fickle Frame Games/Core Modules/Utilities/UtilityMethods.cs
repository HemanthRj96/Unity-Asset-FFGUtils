using UnityEngine;
using TMPro;


/// <summary>
/// This class contains all utility methods
/// </summary>
public static class UtilityMethods
{

    /// <summary>
    /// Returns the world position of the mouse
    /// </summary>
    public static Vector3 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


    /// <summary>
    /// Call this method to draw a line from startPosition to endPosition for a specific amount of time
    /// </summary>
    public static void DrawLine(Vector2 startPosition, Vector2 endPosition, float duration = 2f, Color color = default)
    {
        if (color == default)
            color = Color.white;
        Debug.DrawLine(startPosition, endPosition, color, duration, false);
    }


    /// <summary>
    /// Call this method to draw a rectangle by passing the bottom-left corener and top-right corner
    /// </summary>
    public static void DrawRectangle(Vector2 bottomCorner, Vector2 topCorner, float duration = 2f, Color color = default)
    {
        if (color == default)
            color = Color.white;

        // Horizontals
        Debug.DrawLine(bottomCorner, new Vector2(bottomCorner.x, topCorner.y), color, duration, false);
        Debug.DrawLine(new Vector2(topCorner.x, bottomCorner.y), topCorner, color, duration, false);
        // Verticals
        Debug.DrawLine(new Vector2(bottomCorner.x, topCorner.y), topCorner, color, duration, false);
        Debug.DrawLine(bottomCorner, new Vector2(topCorner.x, bottomCorner.y), color, duration, false);
    }


    /// <summary>
    /// Call this method to draw a grid of a specific height, width, cellsize and duration
    /// </summary>
    public static void DrawGrid(Vector2 origin, int height, int width, float cellSize, float duration = 0f, Color color = default)
    {
        DrawGrid(origin, height, width, new Vector2(cellSize, cellSize), duration, color);
    }


    /// <summary>
    /// Call this method to draw a grid of a specific height, width, cellsize and duration
    /// </summary>
    public static void DrawGrid(Vector2 origin, int height, int width, Vector2 cellSize, float duration = 0f, Color color = default)
    {
        Vector2 corner = origin + new Vector2(width * cellSize.x, height * cellSize.y);
        DrawRectangle(origin, corner, duration, color);

        // Horizontal lines
        for (int h = 1; h < height; ++h)
            DrawLine(origin + (Vector2.up * h * cellSize.y), new Vector2(corner.x, origin.y) + (Vector2.up * h * cellSize.y), duration, color);

        // Vertical lines
        for (int w = 1; w < width; ++w)
            DrawLine(origin + (Vector2.right * w * cellSize.x), new Vector2(origin.x, corner.y) + (Vector2.right * w * cellSize.x), duration, color);
    }


    /// <summary>
    /// Call this method to create a gameObject with TextMeshPro component
    /// </summary>
    /// <param name="objectName">Name of the target text gameObject</param>
    /// <param name="text">Text content for the textmeshpro</param>
    /// <param name="parent">Parent to which this gameObject should be attached</param>
    /// <param name="localPosition">Position with respect to the parent if any otherwise worldPosition</param>
    /// <param name="textBoxSize">Size of the text box since auto resizing is enabled font size is automatically set</param>
    /// <param name="textColor">Color of the text</param>
    /// <returns>Returns the TextMeshPro component of the newly created GameObject</returns>
    public static TextMeshPro CreateWorldText(string objectName, string text, Transform parent, Vector3 localPosition, Vector2 textBoxSize, Color textColor)
    {
        if (objectName.Length == 0)
            objectName = "World_Text";

        GameObject textObject = new GameObject(objectName, typeof(TextMeshPro));
        TextMeshPro textMesh = textObject.GetComponent<TextMeshPro>();
        RectTransform rectTransform = textObject.GetComponent<RectTransform>();

        if (parent != null)
            textObject.transform.SetParent(parent);

        textMesh.text = text;
        textMesh.enableAutoSizing = true;
        textMesh.fontSizeMin = 0.1f;
        textMesh.fontSizeMax = 40f;
        textMesh.horizontalAlignment = HorizontalAlignmentOptions.Center;
        textMesh.verticalAlignment = VerticalAlignmentOptions.Middle;
        textMesh.color = textColor;

        rectTransform.position = localPosition;
        rectTransform.sizeDelta = textBoxSize;

        return textMesh;
    }


    /// <summary>
    /// Call this method to create a gameObject with Sprite Renderer component 
    /// </summary>
    /// <param name="objectName">Name of the gameObject</param>
    /// <param name="parent">Parent tranform for this gameObject</param>
    /// <param name="sprite">Target sprite to be used</param>
    /// <param name="localPosition">Local position of this object if parent is there otherwise worldPosition</param>
    /// <param name="localScale">Local scale of the SpriteRenderer component</param>
    /// <param name="color">Target color for the sprite</param>
    /// <returns>Returns the SpriteRenderer of the newly created GameObject</returns>
    public static SpriteRenderer CreateRenderer(string objectName, Transform parent, Sprite sprite, Vector3 localPosition, Vector3 localScale, Color color = default)
    {
        if (sprite == null)
            return null;
        if (color == default)
            return null;
        if (objectName.Length == 0)
            objectName = "Renderer";

        GameObject renderObject = new GameObject(objectName, typeof(SpriteRenderer));
        SpriteRenderer renderer = renderObject.GetComponent<SpriteRenderer>();

        if (parent != null)
            renderObject.transform.SetParent(parent);

        renderObject.transform.localPosition = localPosition;
        renderObject.transform.localScale = localScale;
        renderer.sprite = sprite;
        renderer.color = color;

        return renderer;
    }


    /// <summary>
    /// Call this method to cast a ray in 3D space, set shouldDrawLine as true to see debug line that represents the RayCast
    /// </summary>
    public static RaycastHit CastRay(Vector3 origin, Vector3 direction, float distance, LayerMask layerMask, bool shouldDrawLine = false, float duration = 10f)
    {
        Ray ray = new Ray(origin, direction);
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo, distance, layerMask);
        if (shouldDrawLine)
            DrawLine(origin, direction * distance, duration, Color.red);
        return hitInfo;
    }


    /// <summary>
    /// Call this method to cast a ray in 2D space, set shouldDrawLine as true to see debug line that represents the RayCast
    /// </summary>
    public static RaycastHit2D Cast2DRay(Vector3 origin, Vector3 direction, float distance, LayerMask layerMask, bool shouldDrawLine = false, float duration = 10f)
    {
        return Physics2D.Raycast(origin, direction, distance, layerMask);
    }


    /// <summary>
    /// Call this method cast a linear BoxCast in 2D space, set shouldDrawBox as true to see debug box that represents the BoxCast
    /// </summary>
    public static RaycastHit2D BoxCast(Vector3 origin, Vector3 direction, Vector2 size, float distance, LayerMask layerMask, bool shouldDrawBox, float duration)
    {
        if (shouldDrawBox)
        {
            Vector3 bottomCorner = origin - (Vector3)(size / 2);
            Vector3 topCorner = origin + (Vector3)(size / 2);
            if (direction == Vector3.up || direction == Vector3.right)
                DrawRectangle(bottomCorner, origin + (Vector3)(size / 2) + direction * distance, duration, Color.red);
            else if (direction == Vector3.down || direction == Vector3.left)
                DrawRectangle(topCorner, origin - (Vector3)(size / 2) + direction * distance, duration, Color.red);
        }
        return Physics2D.BoxCast(origin, size, 0, direction, distance, layerMask);
    }
}
