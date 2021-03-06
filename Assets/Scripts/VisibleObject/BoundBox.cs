using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DimBoxes
{
    [ExecuteInEditMode]
    public class BoundBox : MonoBehaviour
    {
        public List<MeshLines.Line> meshLines;
        private Mesh mesh;
        public enum BoundSource
        {
            meshes,
            boxCollider,
        }

        public BoundSource boundSource = BoundSource.meshes;

        [Tooltip("permanent or On/Off onMouseDown")]
        public bool interactive = true; //permanent vs onMouseDown

        //public bool setupOnAwake = false;
        public Material lineMaterial;

        [Header("Rendering methods")]
        public bool wire_renderer = false;
        [HideInInspector]
        public Color wireColor = new Color(0f, 1f, 0.4f, 0.74f);
        [HideInInspector]
        public bool line_renderer = true;
        public bool mesh_line_renderer = true;
        [HideInInspector]
        public Object linePrefab;
        [HideInInspector]
        [Range(0.005f, 0.25f)] public float lineWidth = 0.03f;
        [HideInInspector]
        public Color lineColor = Color.red;
        [HideInInspector]
        public int numCapVertices = 10;

        protected Bounds bound;
        protected Vector3 boundOffset;
        [HideInInspector]
        public Bounds colliderBound;
        [HideInInspector]
        public Vector3 colliderBoundOffset;
        [HideInInspector]
        public Bounds meshBound;
        [HideInInspector]
        public Vector3 meshBoundOffset;

        protected Vector3[] corners = new Vector3[0];

        protected Vector3[,] lines = new Vector3[0, 0];

        private Quaternion quat;

       // private DimBoxes.DrawLines cameralines;

        protected LineRenderer[] lineList;

        //private MeshFilter[] meshes;

        //private Material lineMaterial;

        private Vector3 topFrontLeft;
        private Vector3 topFrontRight;
        private Vector3 topBackLeft;
        private Vector3 topBackRight;
        private Vector3 bottomFrontLeft;
        private Vector3 bottomFrontRight;
        private Vector3 bottomBackLeft;
        private Vector3 bottomBackRight;

        [HideInInspector]
        public Vector3 startingScale;
        private Vector3 previousScale;
        private Vector3 startingBoundSize;
        private Vector3 startingBoundCenterLocal;
        private Vector3 previousPosition;
        private Quaternion previousRotation;


        void Reset()
        {
            calculateBounds();
            // Start();
        }


        void Start()
        {
            /*cameralines = FindObjectOfType(typeof(DimBoxes.DrawLines)) as DimBoxes.DrawLines;

            if (!cameralines)
            {
                Debug.LogError("DimBoxes: no camera with DimBoxes.DrawLines in the scene", gameObject);
                return;
            }*/

            previousPosition = transform.position;
            previousRotation = transform.rotation;
            startingBoundSize = bound.size;
            startingScale = transform.localScale;
            previousScale = startingScale;
            startingBoundCenterLocal = transform.InverseTransformPoint(bound.center);
            init();
        }

        public void init()
        {
            SetPoints();
            SetLines();
            SetLineRenderers();
        }

        void LateUpdate()
        {
            UpdateLineWidths();

            if (transform.localScale != previousScale)
            {
                SetPoints();
            }
            if (transform.position != previousPosition || transform.rotation != previousRotation || transform.localScale != previousScale)
            {
                SetLines();
                previousRotation = transform.rotation;
                previousPosition = transform.position;
                previousScale = transform.localScale;
            }
            //if(wire_renderer) cameralines.setOutlines(lines, wireColor, new Vector3[0][]);
        }

        void calculateBounds()
        {
            quat = transform.rotation;//object axis AABB
            Vector3 locScale = transform.localScale;

            meshBound = new Bounds();
            MeshFilter[] meshes = GetComponentsInChildren<MeshFilter>();
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            transform.localScale = Vector3.one;

            for (int i = 0; i < meshes.Length; i++)
            {
                Mesh ms = meshes[i].sharedMesh;
                int vc = ms.vertexCount;
                for (int j = 0; j < vc; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        meshBound = new Bounds(meshes[i].transform.TransformPoint(ms.vertices[j]), Vector3.zero);
                    }
                    else
                    {
                        meshBound.Encapsulate(meshes[i].transform.TransformPoint(ms.vertices[j]));
                    }
                }
            }
            transform.rotation = quat;
            transform.localScale = locScale;
            meshBoundOffset = meshBound.center - transform.position;
        }

        void SetPoints()
        {

            if (boundSource == BoundSource.boxCollider)
            {
                BoxCollider bc = GetComponent<BoxCollider>();
                if (!bc)
                {
                    Debug.LogError("no BoxCollider - add BoxCollider to " + gameObject.name + " gameObject");
                    return;

                }
                bound = new Bounds(bc.center, bc.size * 0.5f);
                boundOffset = bc.center;
            }

            else
            {
                bound = meshBound;
                boundOffset = meshBoundOffset;
            }
            //bound.size = new Vector3(bound.size.x * transform.localScale.x / startingScale.x, bound.size.y * transform.localScale.y / startingScale.y, bound.size.z * transform.localScale.z / startingScale.z);
            //boundOffset = new Vector3(boundOffset.x * transform.localScale.x / startingScale.x, boundOffset.y * transform.localScale.y / startingScale.y, boundOffset.z * transform.localScale.z / startingScale.z);

            float o = 0.005f;

            topFrontRight = boundOffset + Vector3.Scale(bound.extents, new Vector3((1+o), (1+o), (1+o)));
            topFrontLeft = boundOffset + Vector3.Scale(bound.extents, new Vector3(-(1+o), (1+o), (1+o)));
            topBackLeft = boundOffset + Vector3.Scale(bound.extents, new Vector3(-(1+o), (1+o), -(1+o)));
            topBackRight = boundOffset + Vector3.Scale(bound.extents, new Vector3((1+o), (1+o), -(1+o)));
            bottomFrontRight = boundOffset + Vector3.Scale(bound.extents, new Vector3((1+o), -(1+o), (1+o)));
            bottomFrontLeft = boundOffset + Vector3.Scale(bound.extents, new Vector3(-(1+o), -(1+o), (1+o)));
            bottomBackLeft = boundOffset + Vector3.Scale(bound.extents, new Vector3(-(1+o), -(1+o), -(1+o)));
            bottomBackRight = boundOffset + Vector3.Scale(bound.extents, new Vector3((1+o), -(1+o), -(1+o)));

            corners = new Vector3[] { topFrontRight, topFrontLeft, topBackLeft, topBackRight, bottomFrontRight, bottomFrontLeft, bottomBackLeft, bottomBackRight };
        }

        protected virtual void SetLines()
        {

            //Quaternion rot = transform.rotation;
            //Vector3 pos = transform.position;
            if (wire_renderer)
            {
                List<Vector3[]> _lines = new List<Vector3[]>();
                //int linesCount = 12;

                Vector3[] _line;
                for (int i = 0; i < 4; i++)
                {
                    //width
                    _line = new Vector3[] { corners[2 * i], corners[2 * i + 1] };
                    _lines.Add(_line);
                    //height
                    _line = new Vector3[] { corners[i], corners[i + 4] };
                    _lines.Add(_line);
                    //depth
                    _line = new Vector3[] { corners[2 * i], corners[2 * i + 3 - 4 * (i % 2)] };
                    _lines.Add(_line);

                }
                lines = new Vector3[_lines.Count, 2];
                for (int j = 0; j < _lines.Count; j++)
                {
                    lines[j, 0] = _lines[j][0];
                    lines[j, 1] = _lines[j][1];
                }
            }
            //
            if (line_renderer)
            {
                for (int i = 0; i < lineList.Length; i++)
                {

#if UNITY_EDITOR
                    if (!Application.isPlaying)
                    {
                        if (PrefabUtility.GetCorrespondingObjectFromSource(lineList[i].gameObject) == linePrefab)
                        {
                            lineList[i].enabled = true;
                        }
                        // else
                        // {
                        //     lineList[i].gameObject.SetActive(false);
                        //     Debug.Log("BB");
                        //     GameObject go = PrefabUtility.InstantiatePrefab(linePrefab) as GameObject;
                        //     go.transform.SetParent(transform);
                        //     go.transform.position = Vector3.zero;
                        //     go.transform.rotation = Quaternion.identity;
                        //     lineList[i] = go.GetComponent<LineRenderer>();
                        // }
                    }
#endif
                    lineList[i].enabled = true;
                }

                foreach (var line in lineList) {
                for (int i = 0; i < lineList.Length; i++)
                    lineList[i].SetPositions(new Vector3[] {
                        transform.TransformPoint(meshLines[i].a),
                        transform.TransformPoint(meshLines[i].b)
                    });
                }
                // TODO: implement a more generic transformation
                // for (int i = 0; i < 4; i++)
                // {
                    // if (meshLines.Count == 12) {
                        // //width
                        // lineList[i].SetPositions(new Vector3[] { transform.TransformPoint(corners[2 * i]), transform.TransformPoint(corners[2 * i + 1]) });
                        // //height
                        // lineList[i + 4].SetPositions(new Vector3[] { transform.TransformPoint(corners[i]), transform.TransformPoint(corners[i + 4]) });
                        // //depth
                        // lineList[i + 8].SetPositions(new Vector3[] { transform.TransformPoint(corners[2 * i]), transform.TransformPoint(corners[2 * i + 3 - 4 * (i % 2)]) });
                //     }
                // }
            }
        }

        public void UpdateLineWidths()
        {
            GameObject player = GameObject.Find("Hero_Prefab");
            foreach (LineRenderer lr in GetComponentsInChildren<LineRenderer>(true))
            {
                float dist = Vector3.Distance(player.transform.position, transform.position);
                lr.startWidth = 0.03f + dist * 0.001f;
                // lr.widthMultiplier = 1 + dist * 0.5f;
            }
        }

        public void SetLineRenderers()
        {
            Gradient colorGradient = new Gradient();
            colorGradient.SetKeys(new GradientColorKey[] { new GradientColorKey(lineColor, 0.0f)},  new GradientAlphaKey[] { new GradientAlphaKey(lineColor.a, 0.0f) });
            foreach (LineRenderer lr in GetComponentsInChildren<LineRenderer>(true))
            {
                lr.startWidth = lineWidth;
                lr.enabled = line_renderer;
                lr.numCapVertices = numCapVertices;
                lr.colorGradient = colorGradient;
                lr.material = lineMaterial;
            }
        }

        protected virtual void OnMouseDown()
        {
            if (!interactive) return;
            enabled = !enabled;
        }

        void OnEnable()
        {
            //cameralines = FindObjectOfType(typeof(DimBoxes.DrawLines)) as DimBoxes.DrawLines;
            lineList = GetComponentsInChildren<LineRenderer>(true);
            LineRenderer[] lrs = GetComponentsInChildren<LineRenderer>(true);
            for (int i = 0; i < lrs.Length; i++)
            {
                // if ( PrefabUtility.IsPartOfPrefabInstance(lrs[i].transform) )
                // {
                //     //if a part of a prefab instance then get the instance handle
                //     Object prefabInstance = PrefabUtility.GetPrefabInstanceHandle(lrs[i].transform);
                //     //destroy the handle
                //     GameObject.DestroyImmediate(prefabInstance);
                // }

                
                // if (!lrs[i].gameObject.activeSelf) DestroyImmediate(lrs[i].gameObject);
            }
            init();
        }
        void OnDestroy()
        {
            LineRenderer[] lrs = GetComponentsInChildren<LineRenderer>(true);
            for (int i = 0; i < lrs.Length; i++)
            {
                DestroyImmediate(lrs[i].gameObject);
            }
        }

        void OnDisable()
        {
            LineRenderer[] lrs = GetComponentsInChildren<LineRenderer>();
            for (int i = 0; i < lrs.Length; i++)
            {
                lrs[i].enabled = false;
            }
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            if (EditorApplication.isPlaying) return;
            // init();
        }


#endif

        void OnDrawGizmos()
        {
            if (!wire_renderer) return;
            Gizmos.color = wireColor;
            Gizmos.matrix = transform.localToWorldMatrix;
            for (int i = 0; i < lines.GetLength(0); i++)
            {
                Gizmos.DrawLine(lines[i, 0], lines[i, 1]);
            }
        }

        void OnRenderObject()
        {
            if (lines == null||!wire_renderer||!lineMaterial) return;
            lineMaterial.SetPass(0);

            GL.PushMatrix();
            // Set transformation matrix for drawing to
            // match our transform
            GL.MultMatrix(transform.localToWorldMatrix);
            GL.Begin(GL.LINES);

                GL.Color(wireColor);
                for (int i = 0; i < lines.GetLength(0); i++)
                {
                    GL.Vertex(lines[i, 0]);
                    GL.Vertex(lines[i, 1]);
                }
 
            GL.End();
            GL.PopMatrix();
        }

#if UNITY_EDITOR
        public void GenerateMeshLines() {
            if (TryGetComponent<MeshFilter>(out MeshFilter meshFilter)) {
                meshLines = MeshLines.GenerateMeshLines(meshFilter.sharedMesh, false, .001f);
                lineList = new LineRenderer[meshLines.Count];
                for (int i = 0; i < meshLines.Count; i++)
                {
                    GameObject go = PrefabUtility.InstantiatePrefab(linePrefab) as GameObject;
                    go.transform.SetParent(transform);
                    go.transform.position = Vector3.zero;
                    go.transform.rotation = Quaternion.identity;
                    lineList[i] = go.GetComponent<LineRenderer>();
                    lineList[i].SetPositions(new Vector3[] {
                        transform.TransformPoint(meshLines[i].a),
                        transform.TransformPoint(meshLines[i].b)
                    });
                }
            } else {
                Debug.LogWarning("Could not find mesh.");
            }
        }
#endif
    }
}
