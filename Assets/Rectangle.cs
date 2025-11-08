using UnityEngine;

public class Rectangle : MonoBehaviour
{
    public Material material;
    public float width = 2f;   
    public float height = 6f;  
    public float depth = 2f; 

    [Header("Position Settings")]
    public Vector2 rectPos;
    public float zPos;

    private void OnPostRender()
    {
        DrawRectangle();
    }

    public void DrawRectangle()
    {
        if (material == null)
        {
            Debug.LogError("You need to add a material!");
            return;
        }

        GL.PushMatrix();
        GL.Begin(GL.LINES);
        material.SetPass(0);

        var frontFace = GetRectangle();
        var frontZ = PerspectiveCamera.Instance.GetPerspective(zPos + depth * 0.5f);
        var backFace = GetRectangle();
        var backZ = PerspectiveCamera.Instance.GetPerspective(zPos - depth * 0.5f);

        var computedFront = RenderRectangle(frontFace, frontZ);
        var computedBack = RenderRectangle(backFace, backZ);

        for (int i = 0; i < 4; i++)
        {
            GL.Vertex(computedFront[i]);
            GL.Vertex(computedBack[i]);
        }

        GL.End();
        GL.PopMatrix();
    }

    public Vector2[] GetRectangle()
    {
        var faceArray = new Vector2[]
        {
            new Vector2 (0.5f, 0.5f),
            new Vector2 (-0.5f, 0.5f),
            new Vector2 (-0.5f, -0.5f),
            new Vector2 (0.5f, -0.5f),
        };

        for (var i = 0; i < faceArray.Length; i++)
        {
            faceArray[i] = new Vector2(
                rectPos.x + faceArray[i].x * width,
                rectPos.y + faceArray[i].y * height
            );
        }

        return faceArray;
    }

    private Vector2[] RenderRectangle(Vector2[] rectElements, float perspective)
    {
        var computedRect = new Vector2[rectElements.Length];

        for (var i = 0; i < rectElements.Length; i++)
        {
            computedRect[i] = rectElements[i] * perspective;

            GL.Vertex(rectElements[i] * perspective);
            GL.Vertex(rectElements[(i + 1) % rectElements.Length] * perspective);
        }

        return computedRect;
    }
}
