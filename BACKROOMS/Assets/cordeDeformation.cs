using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cordeDeformation : MonoBehaviour
{
    public float amplitude = 50;
    public float amplitude2 = 2;//f remove
    private MeshFilter meshFilter;
    private Mesh oriMesh;
    private Vector3[] oriVertices;
    //https://forum.unity.com/threads/i-am-trying-to-make-my-procedural-mesh-move-like-a-wave-for-a-project-but-cant-find-anything-online.1367022/
    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        oriMesh = meshFilter.mesh;
        oriVertices = oriMesh.vertices;
    }

    void Update()
    {
        Vector3[] vertices = meshFilter.mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = oriVertices[i];
            vertex.x += Mathf.Sin((vertex.y + Time.timeSinceLevelLoad) * 30) * amplitude;
            vertex.z += Mathf.Sin((vertex.y +Time.timeSinceLevelLoad/2) * 40)* amplitude;
            vertices[i] =vertex;
        }
        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.RecalculateNormals();
    }
}
