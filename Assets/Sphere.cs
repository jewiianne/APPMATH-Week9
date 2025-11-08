using UnityEngine;

public class Sphere : MonoBehaviour
{
    public Material material;
    public float sphereRadius = 1f;
    public Vector2 spherePos;
    public float zPos;
    public int latSegments = 10; 
    public int lonSegments = 20;

    private void OnPostRender()
    {
        DrawSphere();
    }

    public void DrawSphere()
    {
        if (material == null)
        {
            Debug.LogError("You need to add a material");
            return;
        }

        GL.PushMatrix();
        GL.Begin(GL.LINES);
        material.SetPass(0);

        for (int lat = 1; lat < latSegments; lat++)
        {
            float theta = Mathf.PI * lat / latSegments;
            float y = Mathf.Cos(theta) * sphereRadius;
            float ringRadius = Mathf.Sin(theta) * sphereRadius;

            Vector2[] ring = new Vector2[lonSegments];
            for (int lon = 0; lon < lonSegments; lon++)
            {
                float pi = 2f * Mathf.PI * lon / lonSegments;

                float zOffset = Mathf.Sin(pi) * ringRadius;
                float x = Mathf.Cos(pi) * ringRadius;

                float perspective = PerspectiveCamera.Instance.GetPerspective(zPos + zOffset);
                ring[lon] = new Vector2(spherePos.x + x, spherePos.y + y) * perspective;
            }

            RenderConnectedLine(ring);
        }

        for (int lon = 0; lon < lonSegments; lon++)
        {
            float pi = 2f * Mathf.PI * lon / lonSegments;
            Vector2[] arc = new Vector2[latSegments + 1];

            for (int lat = 0; lat <= latSegments; lat++)
            {
                float theta = Mathf.PI * lat / latSegments;
                float y = Mathf.Cos(theta) * sphereRadius;
                float ringRadius = Mathf.Sin(theta) * sphereRadius;
                float zOffset = Mathf.Sin(pi) * ringRadius;
                float x = Mathf.Cos(pi) * ringRadius;

                float perspective = PerspectiveCamera.Instance.GetPerspective(zPos + zOffset);
                arc[lat] = new Vector2(spherePos.x + x, spherePos.y + y) * perspective;
            }

            RenderConnectedLine(arc);
        }

        GL.End();
        GL.PopMatrix();
    }

    private void RenderConnectedLine(Vector2[] points)
    {
        for (int i = 0; i < points.Length - 1; i++)
        {
            GL.Vertex(points[i]);
            GL.Vertex(points[i + 1]);
        }

        GL.Vertex(points[points.Length - 1]);
        GL.Vertex(points[0]);
    }
}