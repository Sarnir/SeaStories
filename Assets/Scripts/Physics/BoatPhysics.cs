using System;
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

[RequireComponent(typeof(Rigidbody))]
public class BoatPhysics : MonoBehaviour
{

    public bool ShowSubmergedMesh;

    [SerializeField]
    Vector3 centerOfMass;

    public Vector3 CenterOfMass
    {
        get { return rigidBody.centerOfMass; }
        set { rigidBody.centerOfMass = value; }
    }
    
	public MeshFilter bodyMesh;
    public float rho = 2f;
    public float DragCoefficient = 1f;

    float g = Physics.gravity.y;

    Mesh originalMesh;
    MeshFilter submergedMeshFilter;
    Rigidbody rigidBody;
    Sails sails;
    Rudder rudder;

    MeshData originalMeshData;
    List<TriangleData> submergedTris;
    float submergedArea;
	Vector3 submergedCenter;
	float angularDrag;

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        submergedTris = new List<TriangleData>();
		originalMesh = bodyMesh.mesh;

		angularDrag = rigidBody.angularDrag;
        rigidBody.centerOfMass = centerOfMass;

        sails = GetComponentInChildren<Sails>();
        rudder = GetComponentInChildren<Rudder>();

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

    void FixedUpdate()
    {
        CheckUnderwaterVertices();

        ProcessPhysics();

		if (ShowSubmergedMesh)
			RenderSubmergedMesh();
    }

	void ApplyBuoyancy()
	{
		if (submergedArea > 0f)
		{
			var force = new Vector3 (0f, rho * Mathf.Abs(g) * submergedArea, 0f);

			rigidBody.AddForceAtPosition (force, submergedCenter);
		}
	}

    void ApplyWaterResistance()
    {
        if (submergedArea > 0f)
        {
            var velocity = rigidBody.velocity;
            var Fd = -0.5f * rho * velocity.sqrMagnitude * velocity.normalized * submergedArea * DragCoefficient;
            
            rigidBody.AddForceAtPosition(Fd, submergedCenter);
            rigidBody.angularDrag = angularDrag + (DragCoefficient * submergedArea / originalMeshData.Area);
        }
    }

    void ProcessPhysics()
    {
        submergedArea = 0f;
		submergedCenter = Vector3.zero;
        for (int i = 0; i < submergedTris.Count; i++)
        {
            Vector3 force = Vector3.zero;
            submergedArea += submergedTris[i].Area;
			submergedCenter += submergedTris [i].Center;    
        }

		if(submergedTris.Count > 0)
			submergedCenter /= submergedTris.Count;

		ApplyBuoyancy ();
        ApplyRudderForces();
        ApplySailForces ();
        ApplyWaterResistance ();

        myLog = "Velocity: " + rigidBody.velocity.magnitude;
    }

    void ApplyRudderForces()
    {
        if (rudder == null)
            return;

        var vel = rigidBody.velocity;
        vel.y = 0f;
        var velocityParam = Mathf.Clamp01(vel.magnitude);

        //Debug.Log("Velocity param = " + velocityParam);
        
        float force = velocityParam * rudder.RudderCoefficient * Mathf.Sin(Mathf.Deg2Rad * -rudder.Angle);
        
        rigidBody.AddTorque(0f, force, 0f);
    }

    public float GetAngleOfAttack()
    {
        // TODO: ogarnąć, żeby używać tutaj poprawnego wektora kierunku :D
        var angleOfAttack = 180f + Utils.Math.AngleSigned(transform.up, GetApparentWindForce(), Vector3.up); // Vector3.Angle (transform.up, apparentWindForce);
        //myLog = "Angle of attack: " + angleOfAttack;

        return angleOfAttack;
    }

    public float GetAngleOfAttackSigned()
    {
        var angleOfAttack = GetAngleOfAttack();
        
        if (angleOfAttack > 180f)
        {
            angleOfAttack = angleOfAttack - 360f;
        }

        return angleOfAttack;
    }

    public Vector3 GetApparentWindForce()
    {
        return WeatherController.Instance.GetTrueWind() - rigidBody.velocity;
    }

    void ApplySailForces()
    {
        if (sails == null || rigidBody == null || submergedArea <= 0f)
            return;

        Vector3 apparentWindForce = GetApparentWindForce();
        var angleOfAttack = GetAngleOfAttack();

        if (sails.IsUsingConfig)
        {
            if (angleOfAttack > 180f)
            {
                angleOfAttack = angleOfAttack + (180f - angleOfAttack);
            }

            var forwardForce = apparentWindForce.magnitude * sails.GetSailForce(angleOfAttack) * transform.up;

            rigidBody.AddForceAtPosition(forwardForce, submergedCenter);
        }
        else
        {
            float sign = 1f;
            if (angleOfAttack > 180f)
            {
                angleOfAttack = 180f - angleOfAttack;
                sign = -1f;
            }

            Vector3 drag = 0.5f * rho * Vector3.SqrMagnitude(apparentWindForce) *
            sails.GetArea() * sails.GetDragCoefficient(angleOfAttack) * apparentWindForce.normalized;
            Vector3 lift = 0.5f * rho * Vector3.SqrMagnitude(apparentWindForce) *
                sails.GetArea() * sails.GetLiftCoefficient(angleOfAttack) *
                Vector3.Cross(Vector3.up, apparentWindForce.normalized);
            
            var forwardForce = (lift.magnitude * Mathf.Sin(Mathf.Deg2Rad * angleOfAttack) -
                drag.magnitude * Mathf.Cos(Mathf.Deg2Rad * angleOfAttack)) * transform.up * sign;

            var lateralForce = (lift.magnitude * Mathf.Cos(Mathf.Deg2Rad * angleOfAttack) +
                drag.magnitude * Mathf.Sin(Mathf.Deg2Rad * angleOfAttack)) *
                -Vector3.Cross(Vector3.up, forwardForce.normalized);

            // hack for getting forward force less than 0 (when facing the wind)
            if (Vector3.Dot(forwardForce.normalized, transform.up.normalized) < 0)
                forwardForce = Vector3.zero;
            
            //Debug.DrawRay(transform.position, apparentWindForce * 5f, Color.yellow);
            //Debug.DrawRay(transform.position, drag * 5f, Color.red);
            //Debug.DrawRay(transform.position, lift * 5f, Color.green);
            //Debug.DrawRay(transform.position, lateralForce * 5f, Color.blue);
            //Debug.DrawRay(transform.position, forwardForce * 5f, Color.black);
            rigidBody.AddForceAtPosition(forwardForce, submergedCenter);
            rigidBody.AddForceAtPosition(lateralForce, new Vector3(submergedCenter.x, sails.GetCenter().y, submergedCenter.z));
            rigidBody.AddForceAtPosition(-lateralForce, submergedCenter);
        }
        //Debug.DrawRay(transform.position, sails.GetTrueWind() * 5f, Color.cyan);
    }

	string myLog;

	void OnGUI ()
	{
        return;
        if (!string.IsNullOrEmpty(myLog))
        {
            var style = new GUIStyle();
            style.fontSize = 40;
            GUI.TextField(new Rect(10, 10, 120, 50), myLog, style);
        }
	}

    void RenderSubmergedMesh()
    {
        Mesh submergedMesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        int[] tris = new int[submergedTris.Count * 3];
        int indiceNum = 0;
        for (int i = 0; i < submergedTris.Count; i++)
        {
			vertices.Add(transform.InverseTransformPoint(submergedTris[i].V1.Pos));
            tris[indiceNum] = vertices.Count - 1;
            indiceNum++;

			vertices.Add(transform.InverseTransformPoint(submergedTris[i].V2.Pos));
            tris[indiceNum] = vertices.Count - 1;
            indiceNum++;

			vertices.Add(transform.InverseTransformPoint(submergedTris[i].V3.Pos));
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
			sMeshObj.transform.localScale = Vector3.one;
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
        //float waterY = waterMesh.transform.position.y;

        List<Vector3> intersectionPoints = new List<Vector3>();

        for (int i = 0; i < originalMeshData.Triangles.Length; i++)
        {
            // get vh vm vl
            List<Vector3> triangle = new List<Vector3>();
            triangle.Add(transform.TransformPoint(originalMeshData.Triangles[i].V1.Pos));
            triangle.Add(transform.TransformPoint(originalMeshData.Triangles[i].V2.Pos));
            triangle.Add(transform.TransformPoint(originalMeshData.Triangles[i].V3.Pos));

            float d1 = triangle[0].y - WaterController.Instance.GetWaterYPos(triangle[0]);
            float d2 = triangle[1].y - WaterController.Instance.GetWaterYPos(triangle[1]);
            float d3 = triangle[2].y - WaterController.Instance.GetWaterYPos(triangle[2]);

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

                d1 = triangle[0].y - WaterController.Instance.GetWaterYPos(triangle[0]);
                d2 = triangle[1].y - WaterController.Instance.GetWaterYPos(triangle[1]);
                d3 = triangle[2].y - WaterController.Instance.GetWaterYPos(triangle[2]);

                if (d1 > 0f && d2 > 0f)
                {
                    // two tris above
                    float tm = (-d3) / (d2 - d3);
                    float th = (-d3) / (d1 - (d3));

                    Vector3 Jm = triangle[2] + (triangle[1] - triangle[2]) * tm;
                    Vector3 Jh = triangle[2] + (triangle[0] - triangle[2]) * th;

                    intersectionPoints.Add(Jm);
                    intersectionPoints.Add(Jh);

                    submergedTris.Add(new TriangleData(triangle[2], Jh, Jm));
                }
                else
                {
                    // one tris above
                    float tm = (-d2) / (d1 - (d2));
                    float tl = (-d3) / (d1 - (d3));

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
            //Debug.DrawLine(intersectionPoints[i], intersectionPoints[i + 1], Color.red);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        if (rigidBody)
            Gizmos.DrawSphere(rigidBody.worldCenterOfMass, 0.01f);
    }

    void OnValidate()
    {
        if(rigidBody)
            rigidBody.centerOfMass = centerOfMass;
    }
}
