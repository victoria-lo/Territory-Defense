using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Stats : MonoBehaviour
{
    public Material radarMaterial;
    private Stats stats;
    private CanvasRenderer RadarMesh;

    private void Awake()
    {
        RadarMesh = transform.Find("RadarMesh").GetComponent<CanvasRenderer>();
    }

    public void SetStats(Stats stats)
    {
        this.stats = stats;
        UpdateStatsVisual();
    }
    private void UpdateStatsVisual()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[7]; //6 stats + 1 origin
        Vector2[] uv = new Vector2[7];  //6 stats + 1 origin
        int[] triangles = new int[3 * 6]; //3 vertices for each vertex

        float angleIncrement = 360f / 6;
        float radarSize = 21.3f;

        int hpVertexIndex = 1;
        int atkVertexIndex = 2;
        int defVertexIndex = 3;
        int speedVertexIndex = 4;
        int spdVertexIndex = 5;
        int spaVertexIndex = 6;

        Vector3 hpVertex = Quaternion.Euler(0, 0, -angleIncrement * 0)*Vector3.up*radarSize*stats.GetStatNorm(Stats.Type.HP);
        Vector3 atkVertex = Quaternion.Euler(0, 0, -angleIncrement * 1) * Vector3.up * radarSize * stats.GetStatNorm(Stats.Type.Attack);
        Vector3 defVertex = Quaternion.Euler(0, 0, -angleIncrement * 2) * Vector3.up * radarSize * stats.GetStatNorm(Stats.Type.Defense);
        Vector3 speedVertex = Quaternion.Euler(0, 0, -angleIncrement * 3) * Vector3.up * radarSize * stats.GetStatNorm(Stats.Type.Speed);
        Vector3 spdVertex = Quaternion.Euler(0, 0, -angleIncrement * 4) * Vector3.up * radarSize * stats.GetStatNorm(Stats.Type.SpAtk);
        Vector3 spaVertex = Quaternion.Euler(0, 0, -angleIncrement * 5) * Vector3.up * radarSize * stats.GetStatNorm(Stats.Type.SpDef);

        //Vertices
        vertices[0] = Vector3.zero;
        vertices[hpVertexIndex] = hpVertex;
        vertices[atkVertexIndex] = atkVertex;
        vertices[defVertexIndex] = defVertex;
        vertices[speedVertexIndex] = speedVertex;
        vertices[spdVertexIndex] = spdVertex;
        vertices[spaVertexIndex] = spaVertex;

        //Triangles
        triangles[0] = 0;
        triangles[1] = hpVertexIndex;
        triangles[2] = atkVertexIndex;

        triangles[3] = 0;
        triangles[4] = atkVertexIndex;
        triangles[5] = defVertexIndex;

        triangles[6] = 0;
        triangles[7] = defVertexIndex;
        triangles[8] = speedVertexIndex;

        triangles[9] = 0;
        triangles[10] = speedVertexIndex;
        triangles[11] = spdVertexIndex;

        triangles[12] = 0;
        triangles[13] = spdVertexIndex;
        triangles[14] = spaVertexIndex;

        triangles[15] = 0;
        triangles[16] = spaVertexIndex;
        triangles[17] = hpVertexIndex;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        RadarMesh.SetMesh(mesh);
        RadarMesh.SetMaterial(radarMaterial, null);
    }
}
