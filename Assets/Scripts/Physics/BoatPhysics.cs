using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class VertexData
{
    public Vector3 Pos;
    public int Index;

    public VertexData(Vector3 pos, int index = 1)
    {
        Pos = pos;
        Index = index;
    }
}

class TriangleData
{
    public VertexData V1;
    public VertexData V2;
    public VertexData V3;
    public Vector3 Center;
    public Vector3 Normal;
    public float Area;

    public TriangleData(VertexData v1, VertexData v2, VertexData v3)
    {
        V1 = v1;
        V2 = v2;
        V3 = v3;
        Center = CalculateCenter(v1.Pos, v2.Pos, v3.Pos);
        Normal = CalculateNormal(v1.Pos, v2.Pos, v3.Pos);
        Area = CalculateArea(v1.Pos, v2.Pos, v3.Pos);
    }

    public TriangleData(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        V1 = new VertexData(v1);
        V2 = new VertexData(v2);
        V3 = new VertexData(v3);
        Center = CalculateCenter(v1, v2, v3);
        Normal = CalculateNormal(v1, v2, v3);
        Area = CalculateArea(v1, v2, v3);
    }

    public float GetHighest()
    {
        return Mathf.Max(V1.Pos.y, Mathf.Max(V2.Pos.y, V3.Pos.y));
    }

    public float GetLowest()
    {
        return Mathf.Min(V1.Pos.y, Mathf.Min(V2.Pos.y, V3.Pos.y));
    }

    public Vector3 CalculateBuoyancy(float rho, float waterLevel)
    {
        // F-> = -rho * g * h * area * n->

        // for each submerged triangle calculate force get F
        // list of fully submerged triangles is calculated above
        // sum up all forces and use only y coord for now
        var g = Physics.gravity.y;
        if (Center.y < waterLevel)
        {
            var force = -rho * g * Center.y * Area * Normal;
            return new Vector3(0f, force.y, 0f);
        }
        else
            return Vector3.zero;
    }

    public Vector3 CalculatePressureDrag(Rigidbody rb, float dragCoefficient, float falloffPow, float waterLevel)
    {
        Vector3 drag = Vector3.zero;

        if (Center.y < waterLevel)
        {
            var v = GetVelocity(rb);
            var cosTheta = Vector3.Dot(v.normalized, Normal);
            drag = -dragCoefficient * Area * Mathf.Sign(cosTheta) * Mathf.Pow(Mathf.Abs(cosTheta), falloffPow) * Normal;
        }

        return drag;
    }

    public Vector3 CalculateSlamming(float rho, float waterLevel)
    {
        return Vector3.zero;
    }

    public List<VertexData> GetSortedVertices(Vector3 center)
    {
        var list = new List<VertexData>();
        list.Add(V1);
        list.Add(V2);
        list.Add(V3);

        list.Sort((x, y) => Vector3.Angle(x.Pos, Center).CompareTo(Vector3.Angle(y.Pos, center)));

        return list;
    }

    public Vector3 GetVelocity(Rigidbody body)
    {
        // vi = Vg + Omg x GCi

        return body.velocity + Vector3.Cross(body.angularVelocity, Center - body.worldCenterOfMass);
    }

    Vector3 CalculateCenter(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        return (v1 + v2 + v3) / 3f;
    }

    Vector3 CalculateNormal(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        return Vector3.Cross(v2 - v1, v3 - v1).normalized;
    }

    float CalculateArea(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        var a = Vector3.Distance(v1, v2);
        var c = Vector3.Distance(v3, v1);
        var sin = Mathf.Sin(Vector3.Angle(v2 - v1, v3 - v1) * Mathf.Deg2Rad);
        return a * c * sin / 2f;
    }
}

class MeshData
{
    public VertexData[] Vertices;
    public TriangleData[] Triangles;
    public float Area;
    public Vector3 Center;

    public void CalculateArea()
    {
        Area = 0f;
        var trisNum = Triangles.Length;
        for (int i = 0; i < trisNum; i++)
        {
            Area += Triangles[i].Area;
        }
    }

    public void CalculateCenter()
    {
        Center = new Vector3();
        var trisNum = Triangles.Length;
        for (int i = 0; i < trisNum; i++)
        {
            Center += Triangles[i].Center;
        }
    }
}

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(Rigidbody))]
public class BoatPhysics : MonoBehaviour
{

    public bool ShowSubmergedMesh;

    public bool Buoyancy;
    public bool PressureDrag;
    public bool Slamming;
    
    public MeshFilter waterMesh;
    public float rho = 997.8f;
    public float dampConst = 1f;
    public float DragCoefficient = 10f;
    public float FallofPower = 0.9f;

    float g = Physics.gravity.y;

    Mesh originalMesh;
    MeshFilter submergedMeshFilter;
    Rigidbody rigidBody;
    MeshRenderer meshRenderer;

    MeshData originalMeshData;
    List<TriangleData> submergedTris;
    float submergedArea;

    // Use this for initialization
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        rigidBody = GetComponent<Rigidbody>();
        submergedTris = new List<TriangleData>();
        originalMesh = GetComponent<MeshFilter>().mesh;

        SetupMeshData(originalMesh);
    }

    void SetupMeshData(Mesh mesh)
    {
        originalMeshData = new MeshData();
        originalMeshData.Vertices = new VertexData[mesh.vertices.Length];

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            originalMeshData.Vertices[i] = new VertexData(mesh.vertices[i], i);
        }

        var tris = mesh.triangles;
        originalMeshData.Triangles = new TriangleData[tris.Length / 3];
        int trisCount = 0;
        for (int i = 0; i < tris.Length; i += 3)
        {
            originalMeshData.Triangles[trisCount] = new TriangleData(
                originalMeshData.Vertices[tris[i]],
                originalMeshData.Vertices[tris[i + 1]],
                originalMeshData.Vertices[tris[i + 2]]);
            trisCount++;
        }

        originalMeshData.CalculateArea();
        originalMeshData.CalculateCenter();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckUnderwaterVertices();
        /*if(Buoyancy) CalculateBuoyancy();
        CalculateDamp();
        if(PressureDrag) CalculatePressureDrag();
        if (Slamming) CalculateSlammingForce();*/

        ProcessPhysics();
    }

    void ProcessPhysics()
    {
        submergedArea = 0;
        var waterLevel = waterMesh.transform.TransformPoint(waterMesh.mesh.vertices[0]).y;
        for (int i = 0; i < submergedTris.Count; i++)
        {
            Vector3 force = Vector3.zero;
            submergedArea += submergedTris[i].Area;

            if (Buoyancy)
            {
                var buoyancy = submergedTris[i].CalculateBuoyancy(rho, waterLevel);
                force += buoyancy;

                Debug.DrawRay(submergedTris[i].Center, buoyancy.normalized * -3f, Color.blue);
            }

            if (PressureDrag)
            {
                var drag = submergedTris[i].CalculatePressureDrag(rigidBody, DragCoefficient, FallofPower, waterLevel);
                force += drag;

                Debug.DrawRay(submergedTris[i].Center, drag * 3f, Color.black);
            }

            if(Slamming)
                force += submergedTris[i].CalculateSlamming(rho, waterLevel);

            rigidBody.AddForceAtPosition(force, submergedTris[i].Center);

            //Debug

            //Normal
            Debug.DrawRay(submergedTris[i].Center, submergedTris[i].Normal * 3f, Color.white);

            //Buoyancy
            

            //Velocity
            //Debug.DrawRay(triangleCenter, triangleVelocityDir * 3f, Color.black);

            //Viscous Water Resistance
            //Debug.DrawRay(triangleCenter, viscousWaterResistanceForce.normalized * 3f, Color.black);
        }
    }

    void Update()
    {
        //meshRenderer.enabled = !ShowSubmergedMesh;
        if (ShowSubmergedMesh)
            RenderSubmergedMesh();
    }

    void RenderSubmergedMesh()
    {
        Mesh submergedMesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        int[] tris = new int[submergedTris.Count * 3];
        int indiceNum = 0;
        for (int i = 0; i < submergedTris.Count; i++)
        {

            var list = submergedTris[i].GetSortedVertices(originalMeshData.Center + transform.position);

            vertices.Add(transform.InverseTransformPoint(list[0].Pos));
            tris[indiceNum] = vertices.Count - 1;
            indiceNum++;

            vertices.Add(transform.InverseTransformPoint(list[1].Pos));
            tris[indiceNum] = vertices.Count - 1;
            indiceNum++;

            vertices.Add(transform.InverseTransformPoint(list[2].Pos));
            tris[indiceNum] = vertices.Count - 1;
            indiceNum++;
        }

        submergedMesh.vertices = vertices.ToArray();
        submergedMesh.triangles = tris;

        if(submergedMeshFilter == null)
        {
            // spawn object with meshfilter
            var sMeshObj = new GameObject("SubmergedMesh");
            submergedMeshFilter = sMeshObj.AddComponent<MeshFilter>();
            sMeshObj.AddComponent<MeshRenderer>();
            sMeshObj.transform.parent = transform;
            sMeshObj.transform.localPosition = Vector3.zero;
            sMeshObj.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }

        submergedMeshFilter.mesh = submergedMesh;
        submergedMeshFilter.gameObject.SetActive(ShowSubmergedMesh);
    }

    void CheckUnderwaterVertices()
    {
        submergedTris.Clear();

        var vertices = originalMesh.vertices;
        var tris = originalMesh.triangles;
        // for now let's say that water is perfectly flat
        float waterY = waterMesh.transform.position.y;

        List<Vector3> intersectionPoints = new List<Vector3>();

        for (int i = 0; i < originalMeshData.Triangles.Length; i++)
        {
            // get vh vm vl
            List<Vector3> triangle = new List<Vector3>();
            triangle.Add(transform.TransformPoint(originalMeshData.Triangles[i].V1.Pos));
            triangle.Add(transform.TransformPoint(originalMeshData.Triangles[i].V2.Pos));
            triangle.Add(transform.TransformPoint(originalMeshData.Triangles[i].V3.Pos));

            float d1 = triangle[0].y - waterY;
            float d2 = triangle[1].y - waterY;
            float d3 = triangle[2].y - waterY;

            if (d1 > 0f && d2 > 0f && d3 > 0f)
            {
                // triangle above water
            }
            else if (d1 < 0f && d2 < 0f && d3 < 0f)
            {
                // triangle fully under water
                submergedTris.Add(new TriangleData(triangle[0], triangle[1], triangle[2]));
            }
            else
            {
                // one or two vertices under water
                triangle.Sort((x, y) => x.y.CompareTo(y.y));
                triangle.Reverse();

                if (triangle[0].y - waterY > 0f && triangle[1].y - waterY > 0f)
                {
                    // two tris above
                    float tm = (-triangle[2].y - waterY) / (triangle[1].y - waterY - (triangle[2].y - waterY));
                    float th = (-triangle[2].y - waterY) / (triangle[0].y - waterY - (triangle[2].y - waterY));

                    Vector3 Jm = triangle[2] + (triangle[1] - triangle[2]) * tm;
                    Vector3 Jh = triangle[2] + (triangle[0] - triangle[2]) * th;

                    intersectionPoints.Add(Jm);
                    intersectionPoints.Add(Jh);

                    submergedTris.Add(new TriangleData(triangle[2], Jh, Jm));
                }
                else
                {
                    // one tris above
                    float tm = (-triangle[1].y - waterY) / (triangle[0].y - waterY - (triangle[1].y - waterY));
                    float tl = (-triangle[2].y - waterY) / (triangle[0].y - waterY - (triangle[2].y - waterY));

                    Vector3 Im = triangle[1] + (triangle[0] - triangle[1]) * tm;
                    Vector3 Il = triangle[2] + (triangle[0] - triangle[2]) * tl;

                    intersectionPoints.Add(Im);
                    intersectionPoints.Add(Il);

                    submergedTris.Add(new TriangleData(triangle[1], Im, Il));
                    submergedTris.Add(new TriangleData(triangle[1], Il, triangle[2]));
                }
            }
        }

        // draw water level
        for (int i = 0; i < intersectionPoints.Count; i += 2)
        {
            Debug.DrawLine(intersectionPoints[i], intersectionPoints[i + 1], Color.red);
        }
    }

    void CalculateBuoyancy()
    {
        // F-> = -rho * g * h * area * n->

        // for each submerged triangle calculate force get F
        // list of fully submerged triangles is calculated above
        // sum up all forces and use only y coord for now
        submergedArea = 0f;

        for (int i = 0; i < submergedTris.Count; i++)
        {
            if (submergedTris[i].Center.y < waterMesh.transform.TransformPoint(waterMesh.mesh.vertices[0]).y)
            {
                var force = -rho * g * submergedTris[i].Center.y * submergedTris[i].Area * submergedTris[i].Normal;
                rigidBody.AddForceAtPosition(new Vector3(0f, force.y, 0f), submergedTris[i].Center);
                submergedArea += submergedTris[i].Area;
            }
        }
    }

    void CalculateDamp()
    {
        var rs = submergedArea / originalMeshData.Area;
        var dampForce = -dampConst * rs * rigidBody.velocity;
        var dampTorque = -dampConst * rs * rigidBody.angularVelocity;

        rigidBody.AddForce(dampForce);
        rigidBody.AddTorque(dampTorque);
    }

    void CalculatePressureDrag()
    {
        Vector3 drag = new Vector3();
        for (int i = 0; i < submergedTris.Count; i++)
        {
            if (submergedTris[i].Center.y < waterMesh.transform.TransformPoint(waterMesh.mesh.vertices[0]).y)
            {
                var area = submergedTris[i].Area;
                var n = submergedTris[i].Normal;
                var v = submergedTris[i].GetVelocity(rigidBody);
                var cosTheta = Vector3.Dot(v.normalized, n);
                drag = -DragCoefficient * area * Mathf.Sign(cosTheta) * Mathf.Pow(Mathf.Abs(cosTheta), FallofPower) * n;
                rigidBody.AddForceAtPosition(drag, submergedTris[i].Center);
            }
        }
    }

    void CalculateSlammingForce()
    {
        for (int i = 0; i < submergedTris.Count; i++)
        {
            if (submergedTris[i].Center.y < waterMesh.transform.TransformPoint(waterMesh.mesh.vertices[0]).y)
            {
                var n = submergedTris[i].Normal;
                var v = submergedTris[i].GetVelocity(rigidBody);
                var cosTheta = Vector3.Dot(v.normalized, n.normalized);
                var force = ((2f * submergedArea * cosTheta) / originalMeshData.Area) * rigidBody.mass * rigidBody.velocity;
                rigidBody.AddForceAtPosition(force, submergedTris[i].Center);
            }
        }
    }
}

