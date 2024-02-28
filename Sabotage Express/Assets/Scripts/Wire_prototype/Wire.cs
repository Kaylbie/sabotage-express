using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wire : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshFilter meshFilter;

    [SerializeField] Transform startTransform, endTransform;
    [SerializeField] int segmentCount = 10;
    [SerializeField] float totalLength = 10;

    [SerializeField] float radius = 0.5f;
    [SerializeField] int sides = 4;

    [SerializeField] float totalWeight = 10;
    [SerializeField] float drag = 1;
    [SerializeField] float angularDrag = 1;

    [SerializeField] bool usePhysics = false;

    private List<Transform> segments = new List<Transform>();

    [SerializeField] Transform segmentParent;
    private int prevSegmentCount;
    private float prevTotalLength;
    private float prevDrag;
    private float prevTotalWeight;
    private float prevAngularDrag;
    private float prevRadius;

    private MeshDataRope meshData;

    private Vector3[] vertices;
    private int[,] vertexIndicesMap;
    private Mesh mesh;
    private bool createTriangles;
    private int prevSides;

    private void Start()
    {
        vertices = new Vector3[segmentCount * sides * 3];
        GenerateMesh();

    }

    private void Update()
    {
        if (prevSegmentCount != segmentCount)
        {
            RemoveSegments();
            segments = new List<Transform>(segmentCount);
            GenerateSegments();
            GenerateMesh();

        }
        prevSegmentCount = segmentCount;
        if (totalLength != prevTotalWeight || drag != prevDrag || totalWeight != prevTotalWeight || angularDrag != prevAngularDrag)
        {
            UpdateWire();
            GenerateMesh();
        }
        prevTotalLength = totalLength;
        prevDrag = drag;
        prevTotalWeight = totalWeight;
        prevAngularDrag = angularDrag;
        if (sides != prevSides)
        {
            vertices = new Vector3[segmentCount * sides * 3];
            GenerateMesh();
        }
        prevSides = sides;

        if (prevRadius != radius && usePhysics)
        {
            UpdateRadius();
            GenerateMesh();
        }
        prevRadius = radius;
        UpdateMesh();
    }

    private void UpdateRadius()
    {
        for (int i = 0; i < segments.Count; i++)
        {
            SetRadiusOnSegments(segments[i]);
        }
    }

    void UpdateMesh()
    {
        GenerateVertices();

        meshFilter.mesh.vertices = vertices;
    }

    private void GenerateMesh()
    {
        createTriangles = true;

        if (meshData == null)
        {
            meshData = new MeshDataRope(sides, segmentCount + 1, false);
        }
        else
        {
            meshData.ResetMesh(sides, segmentCount + 1, false);
        }

        GenerateVertices();
        GenerateIndicesMap();

        meshData.ProcessMesh();
        mesh = meshData.CreateMesh();

        meshFilter.sharedMesh = mesh;

        createTriangles = false;
    }

    private void GenerateIndicesMap()
    {
        // Array.Clear(vertexIndicesMap, 0, vertexIndicesMap.Length);

        vertexIndicesMap = new int[segmentCount + 1, sides + 1];
        int meshVertexIndex = 0;
        for (int segmentIndex = 0; segmentIndex < segmentCount; segmentIndex++)
        {
            for (int sideIndex = 0; sideIndex < sides; sideIndex++)
            {
                vertexIndicesMap[segmentIndex, sideIndex] = meshVertexIndex;
                meshVertexIndex++;
            }
        }
    }

    private void GenerateVertices()
    {
        for (int i = 0; i < segments.Count; i++)
        {
            GenerateCircleVerticesAndTriangles(segments[i], i);
        }
    }

    private void GenerateCircleVerticesAndTriangles(Transform segmentTransform, int segmentIndex)
    {
        float angleDiff = 360 / sides;

        Quaternion diffRotation = Quaternion.FromToRotation(Vector3.forward, segmentTransform.forward);

        for (int sideIndex = 0; sideIndex < sides; sideIndex++)
        {
            float angleInRad = sideIndex * angleDiff * Mathf.Deg2Rad;
            float x = -1f * radius * Mathf.Cos(angleInRad);
            float y = radius * Mathf.Sin(angleInRad);

            Vector3 pointOffset = new(x, y, 0f);

            Vector3 pointRotated = diffRotation * pointOffset;

            Vector3 pointRotatedAtCenterOfTransform = segmentTransform.position + pointRotated;

            int vertecIndex = segmentIndex * sides + sideIndex;
            vertices[vertecIndex] = pointRotatedAtCenterOfTransform;

            if (createTriangles)
            {
                meshData.AddVertex(pointRotatedAtCenterOfTransform, new(0, 0), vertecIndex);

                bool createThisTriangle = segmentIndex < segmentCount - 1;
                //TODO: real time modification of segments 
                if (createThisTriangle)
                {
                    int currentIcrement = 1;
                    int a = vertexIndicesMap[segmentIndex, sideIndex];
                    int b = vertexIndicesMap[segmentIndex + currentIcrement, sideIndex];
                    int c = vertexIndicesMap[segmentIndex, sideIndex + currentIcrement];
                    int d = vertexIndicesMap[segmentIndex + currentIcrement, sideIndex + currentIcrement];


                    bool isLastGap = sideIndex == sides - 1;
                    if (isLastGap)
                    {
                        c = vertexIndicesMap[segmentIndex, 0];
                        d = vertexIndicesMap[segmentIndex + currentIcrement, 0];
                    }

                    // meshData.AddTriangle(a, c, b);
                    // meshData.AddTriangle(c, d, b);

                    meshData.AddTriangle(a, b, c);
                    meshData.AddTriangle(b, d, c);

                }
            }
        }
    }


    private void SetRadiusOnSegments(Transform transform)
    {
        SphereCollider sphereCollider = transform.GetComponent<SphereCollider>();
        sphereCollider.radius = radius;
    }

    private void UpdateWire()
    {
        UpdateSegment(segments[0]);
        for (int i = 1; i < segments.Count; i++)
        {
            UpdateLengthOnSegment(segments[i]);
            UpdateSegment(segments[i]);
        }

    }

    private void UpdateSegment(Transform transform)
    {
        Rigidbody rigidbody = transform.GetComponent<Rigidbody>();
        rigidbody.mass = totalWeight;
        rigidbody.drag = drag;
        rigidbody.angularDrag = angularDrag;

    }

    private void UpdateLengthOnSegment(Transform transform)
    {
        ConfigurableJoint joint = transform.GetComponent<ConfigurableJoint>();
        if (joint != null)
        {
            joint.connectedAnchor = Vector3.forward * totalLength / segmentCount;
        }
    }

    private void RemoveSegments()
    {
        foreach (var segment in segments)
        {
            if (segment != null)
            {
                Destroy(segment.gameObject);
            }
        }
        segments.Clear();
    }

    void OnDrawGizmos()
    {
        if (segments != null)
        {
            foreach (var segment in segments)
            {
                Gizmos.DrawWireSphere(segment.position, 0.1f);
            }
        }
        if (vertices != null)
        {
            foreach (var vertice in vertices)
            {

                Gizmos.DrawSphere(vertice, 0.05f);
            }
        }
    }

    private void GenerateSegments()
    {
        JoinSegment(startTransform, null, true);

        Transform prevTransform = startTransform;

        var heading = endTransform.position - startTransform.position;
        var distance = heading.magnitude;
        Vector3 direction = heading / distance;


        //TODO: last segment has to have the same position and rotation as end_segment;

        for (int i = 0; i < segmentCount; i++)
        {
            GameObject segment = new GameObject($"segment_{i}");
            segment.transform.SetParent(segmentParent);
            segments.Add(segment.transform);
            Vector3 pos = prevTransform.position + (direction / segmentCount);

            if (i == 0 || i + 1 == segmentCount)
            {
                if (i + 1 == segmentCount)
                {
                    segment.transform.position = endTransform.position;
                }
                else
                {
                    segment.transform.position = prevTransform.position;
                }
                JoinSegment(segment.transform, prevTransform.transform, false, true);
            }
            else
            {
                segment.transform.position = pos;
                JoinSegment(segment.transform, prevTransform.transform);
            }


            prevTransform = segment.transform;



        }
        JoinSegment(endTransform, prevTransform, true, true);
    }

    private void JoinSegment(Transform current, Transform connectedTransform, bool isKinetic = false, bool isCloseConnected = false)
    {
        if (current.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rigidbody = current.AddComponent<Rigidbody>();
            rigidbody.isKinematic = isKinetic;
            rigidbody.mass = totalWeight / segmentCount;
            rigidbody.drag = drag;
            rigidbody.angularDrag = angularDrag;
        }

        if (usePhysics)
        {
            SphereCollider sphereCollider = current.AddComponent<SphereCollider>();
            sphereCollider.radius = radius;
        }


        if (connectedTransform != null)
        {
            ConfigurableJoint joint = current.GetComponent<ConfigurableJoint>();
            if (joint == null)
            {
                joint = current.AddComponent<ConfigurableJoint>();
            }

            joint.connectedBody = connectedTransform.GetComponent<Rigidbody>();

            joint.autoConfigureConnectedAnchor = false;
            if (isCloseConnected)
            {
                joint.connectedAnchor = Vector3.forward * 0.1f;
            }
            else
            {
                joint.connectedAnchor = Vector3.forward * (totalLength / segmentCount);
            }
            // Debug.Log(direction.normalized.ToString() + Vector3.forward);

            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;

            joint.angularXMotion = ConfigurableJointMotion.Free;
            joint.angularYMotion = ConfigurableJointMotion.Free;
            joint.angularZMotion = ConfigurableJointMotion.Limited;

            SoftJointLimit softJointLimit = new SoftJointLimit();
            softJointLimit.limit = 0;
            joint.angularZLimit = softJointLimit;

            JointDrive jointDrive = new JointDrive();
            jointDrive.positionDamper = 0;
            jointDrive.positionSpring = 0;
            joint.angularXDrive = jointDrive;
            joint.angularYZDrive = jointDrive;


        }
    }
}
