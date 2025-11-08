using UnityEngine;

public class Pyramid : MonoBehaviour
{
    public Material material;
    public Vector2 basePos;
    public float baseSize = 1f;
    public float zPos = 0f;
    public float height = 1.5f;

    private void OnPostRender()
    {
        DrawPyramid();
    }

    private void DrawPyramid()
    {
        if (material == null)
        {
            Debug.LogError("You need to add a material");
            return;
        }

        GL.PushMatrix();
        GL.Begin(GL.LINES);
        material.SetPass(0);

        var baseSquare = GetBase();

        float baseZ = PerspectiveCamera.Instance.GetPerspective(zPos);
        float topZ = PerspectiveCamera.Instance.GetPerspective(zPos - height);

        var computedBase = RenderSquare(baseSquare, baseZ);

        for (int i = 0; i < 4; i++)
        {
            GL.Vertex(basePos * baseSize * topZ);
            GL.Vertex(computedBase[i]);
        }

        GL.End();
        GL.PopMatrix();
    }

    private Vector2[] GetBase()
    {
        var baseArray = new Vector2[]
        {
            new Vector2(1, 1f),
            new Vector2(-1f, 1f),
            new Vector2(-1f, -1f),
            new Vector2(1f, -1f),
        };

        for (var i = 0; i < baseArray.Length; i++)
        {
            baseArray[i] = new Vector2(basePos.x + baseArray[i].x, basePos.y + baseArray[i].y) * baseSize;
        }

        return baseArray;
    }

    private Vector2[] RenderSquare(Vector2[] squareElements, float perspective)
    {
        var computedSquare = new Vector2[squareElements.Length];
        for (var i = 0; i < squareElements.Length; i++)
        {
            computedSquare[i] = squareElements[i] * perspective;

            GL.Vertex(squareElements[i] * perspective);
            GL.Vertex(squareElements[(i + 1) % squareElements.Length] * perspective);
        }

        return computedSquare;
    }
}

