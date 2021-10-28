using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode] [RequireComponent(typeof(MeshFilter))]
public class MeshLines : MonoBehaviour {

    public MonoBehaviour saveTo;
    public Mesh mesh;
    [Range(0, 2)] public double normalThreshold = 0;
    public List<Line> lines;
    public bool removeQuads;
    public bool softMesh;
    public bool updateRealTime;

    private void Start() {
        if (!mesh) {
            GetComponent<MeshFilter>().mesh = mesh;
        } else {
            mesh = GetComponent<MeshFilter>().sharedMesh;
        }
    }

    public void GenerateMeshLines() {
        lines = GenerateMeshLines(mesh, softMesh, normalThreshold);
    }

    public static List<Line> GenerateMeshLines(Mesh mesh, bool softMesh, double normalThreshold) {
        List<Line> lines = new List<Line>();
        Vector3[] verts = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector3[] norms = mesh.normals;

        // For each triangle, check another triangle, if they share 2 vertices, draw a line between shared vertices
        for (int i = 0; i < triangles.Length; i+=3) {
            for (int j = 0; j < triangles.Length; j+=3) {
                if (i != j) {

                    if (!softMesh) {
                        List<Vector3> points = new List<Vector3>();
                        // Loop through both triangles
                        for (int k = 0; k < 3; k++) {
                            for (int l = 0; l < 3; l++) {
                                
                                // Compare vertices
                                if (verts[triangles[i+k]] == verts[triangles[j+l]]) {

                                    // Remove very soft edges
                                    double normalDiff = 1 - (double)Vector3.Dot(norms[triangles[i+k]].normalized, norms[triangles[j+l]].normalized);
                                    if (normalDiff >= normalThreshold) {
                                        points.Add(verts[triangles[i+k]]);
                                    }
                                }
                                if (k == 1 && l == 2) {
                                    if(points.Count == 0) {
                                        k = l = 3;
                                    }
                                }
                            }
                        }
                        // If triangle shared 2 vertices, draw make a line between them
                        if (points.Count == 2) {
                            Line line = new Line(points[0], points[1]);
                            Line lineInverted = new Line(points[1], points[0]);
                            if (!lines.Contains(line) && !lines.Contains(lineInverted)) {
                                lines.Add(line);
                            }
                        }
                    } else {
                        List<int> index = new List<int>();
                        List<Vector3> faceNormal = new List<Vector3>();
                        faceNormal.Add(new Vector3(0,0,0));
                        faceNormal.Add(new Vector3(0,0,0)); // TODO: make this static array
                        // Loop through both triangles
                        for (int k = 0; k < 3; k++) {
                            for (int l = 0; l < 3; l++) {
                                
                                // Compare triangles
                                if (triangles[i+k] == triangles[j+l]) {
                                    index.Add(triangles[i+k]);
                                }
                                faceNormal[0] += norms[triangles[i+k]];
                                faceNormal[1] += norms[triangles[j+l]];
                                if (k == 1 && l == 2) {
                                    if(index.Count == 0) {
                                        k = l = 3;
                                    }
                                }
                            }
                        }
                        // If triangle shared 2 vertices, draw make a line between them
                        if (index.Count == 2) {
                            double normalDiff = 1 - (double)Vector3.Dot(faceNormal[0].normalized, faceNormal[1].normalized);
                            if (normalDiff >= normalThreshold) {
                                Line line = new Line(verts[index[0]], verts[index[1]]);
                                Line lineInverted = new Line(verts[index[1]], verts[index[0]]);
                                if (!lines.Contains(line) && !lines.Contains(lineInverted)) {
                                    lines.Add(line);
                                }
                            }
                        }
                    }
                    
                }
            }
        }
        return lines;
    }

    [System.Serializable]
    public struct Line {
        public Vector3 a;
        public Vector3 b;
        public Line(Vector3 _a, Vector3 _b) {
            a = _a;
            b = _b;
        }
    }

    public void refresh() {
        if (!mesh) {
            GetComponent<MeshFilter>().sharedMesh = mesh;
        } else {
            // mesh = GetComponent<MeshFilter>().mesh;
        }
        if (updateRealTime) {
            GenerateMeshLines();
        }
    }

    public void SetUpdateRealTime(bool toggle) {
        if (mesh.vertices.Length > 50) {
            return;
        }
        updateRealTime = toggle;
    }

    public void OnDrawGizmos() {
        foreach (var line in lines) {
            Gizmos.DrawLine(line.a, line.b);
        }
    }
}
