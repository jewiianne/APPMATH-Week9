using UnityEngine;

public class Cylinder : MonoBehaviour
{
    public Material material;
    public float cylinderRadius = 1f;
    public float cylinderHeight = 2f;
    public Vector2 cylinderPos;
    public float zPos;
    public int heightSegments = 10;
    public int radialSegments = 24;

    private void OnPostRender()
    {
        DrawLine();
    }

    public void DrawLine()
    {
        if (material == null)
        {
            Debug.LogError("You need to add a material");
            return;
        }

        GL.PushMatrix();
        GL.Begin(GL.LINES);
        material.SetPass(0);

        float halfHeight = cylinderHeight * 0.5f;

        for (int i = 0; i < radialSegments; i++)
        {
            float angle = 2f * Mathf.PI * i / radialSegments;
            float x = Mathf.Cos(angle) * cylinderRadius;
            float zOffset = Mathf.Sin(angle) * cylinderRadius;

            Vector2 top = new Vector2(cylinderPos.x + x, cylinderPos.y + halfHeight);
            Vector2 bottom = new Vector2(cylinderPos.x + x, cylinderPos.y - halfHeight);

            float perspectiveTop = PerspectiveCamera.Instance.GetPerspective(zPos + zOffset);
            float perspectiveBottom = PerspectiveCamera.Instance.GetPerspective(zPos + zOffset);

            GL.Vertex(top * perspectiveTop);
            GL.Vertex(bottom * perspectiveBottom);
        }

        for (int h = 1; h < heightSegments; h++)
        {
            float t = (float)h / heightSegments;
            float y = Mathf.Lerp(-halfHeight, halfHeight, t);
            DrawCircle(zPos, y);
        }

        GL.End();
        GL.PopMatrix();
    }

    private void DrawCircle(float baseZ, float heightOffset)
    {
        Vector2[] points = new Vector2[radialSegments];

        for (int i = 0; i < radialSegments; i++)
        {
            float angle = 2f * Mathf.PI * i / radialSegments;
            float x = Mathf.Cos(angle) * cylinderRadius;
            float zOffset = Mathf.Sin(angle) * cylinderRadius;

            float perspective = PerspectiveCamera.Instance.GetPerspective(baseZ + zOffset);
            points[i] = new Vector2(cylinderPos.x + x, cylinderPos.y + heightOffset) * perspective;
        }

        for (int i = 0; i < radialSegments; i++)
        {
            GL.Vertex(points[i]);
            GL.Vertex(points[(i + 1) % radialSegments]);
        }
    }
}
