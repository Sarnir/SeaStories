using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class VertexData
{
	public Vector3 Pos;
	public int Index;

	public VertexData(Vector3 pos, int index)
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

	public TriangleData(VertexData v1, VertexData v2, VertexData v3)
	{
		V1 = v1;
		V2 = v2;
		V3 = v3;
		Center = CalculateCenter(v1.Pos, v2.Pos, v3.Pos);
		Normal = CalculateNormal(v1.Pos, v2.Pos, v3.Pos);
	}

	public float GetHighest()
	{
		return Mathf.Max(V1.Pos.y, Mathf.Max(V2.Pos.y, V3.Pos.y));
	}

	public float GetLowest()
	{
		return Mathf.Min(V1.Pos.y, Mathf.Min(V2.Pos.y, V3.Pos.y));
	}

	Vector3 CalculateCenter(Vector3 v1, Vector3 v2, Vector3 v3)
	{
		return (v1 + v2 + v3) / 3f;
	}

	Vector3 CalculateNormal(Vector3 v1, Vector3 v2, Vector3 v3)
	{
		return Vector3.Cross (v2 - v1, v3 - v1).normalized;
	}
}

class MeshData
{
	public VertexData[] Vertices;
	public TriangleData[] Triangles;
}

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(Rigidbody))]
public class BoatPhysics : MonoBehaviour {

	public bool ShowSubmergedMesh;

	public MeshFilter waterMesh;
	public float rho = 997.8f;
	float g = Physics.gravity.y;

	Mesh originalMesh;
	Rigidbody rigidBody;

	MeshData originalMeshData;
	List<TriangleData> submergedTris;

	// Use this for initialization
	void Start () {
		originalMesh = GetComponent<MeshFilter> ().mesh;
		rigidBody = GetComponent<Rigidbody> ();
		submergedTris = new List<TriangleData> ();

		SetupMeshData (originalMesh);
	}

	void SetupMeshData(Mesh mesh)
	{
		originalMeshData = new MeshData();
		originalMeshData.Vertices = new VertexData[mesh.vertices.Length];

		for (int i = 0; i < mesh.vertices.Length; i++)
		{
			originalMeshData.Vertices [i] = new VertexData (mesh.vertices [i], i);
		}

		var tris = mesh.triangles;
		originalMeshData.Triangles = new TriangleData[tris.Length / 3];
		int trisCount = 0;
		for (int i = 0; i < tris.Length; i += 3)
		{
			originalMeshData.Triangles[trisCount] = new TriangleData (
				originalMeshData.Vertices [tris [i]],
				originalMeshData.Vertices [tris [i+1]],
				originalMeshData.Vertices [tris [i+2]]);
			trisCount++;
		}
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		CheckUnderwaterVertices ();
		CalculateBuoyancy ();
	}

	void CheckUnderwaterVertices()
	{
		submergedTris.Clear ();

		var vertices = originalMesh.vertices;
		var tris = originalMesh.triangles;
		// for now let's say that water is perfectly flat
		float waterY = waterMesh.mesh.vertices[0].y + waterMesh.transform.position.y;

		for (int i = 0; i < vertices.Length; i++)
		{
			//if (transform.TransformPoint(vertices [i]).y < waterY)
				//Debug.Log ("Vertex " + i + " is under water!");
		}

		for (int i = 0; i < tris.Length; i += 3)
		{
			float d1 = transform.TransformPoint (vertices [tris[i]]).y - waterY;
			float d2 = transform.TransformPoint (vertices [tris[i + 1]]).y - waterY;
			float d3 = transform.TransformPoint (vertices [tris[i + 2]]).y - waterY;

			if (d1 < 0 || d2 < 0 || d3 < 0)
			{
				//Debug.Log ("Triangle " + i + "is underwater");

				//triangles.Add (new TriangleData ( vertices [tris[i]], vertices [tris[i + 1]], vertices [tris[i + 2]]));
			}	
		}

		List<Vector3> intersectionPoints = new List<Vector3> ();

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
				//submergedTris.Add( new TriangleData(triangle[0], triangle[1], triangle[2]));
			}
			else
			{
                // one or two vertices under water
                triangle.Sort((x, y) => x.y.CompareTo(y.y));
                triangle.Reverse();

				if (triangle [0].y - waterY > 0f && triangle [1].y - waterY > 0f)
				{
					// two tris above

				}
				else
				{
					// one tris above
					float tm = (-triangle[1].y - waterY)/(triangle[0].y - waterY -(triangle[1].y - waterY));
					float tl = (-triangle[2].y - waterY)/(triangle[0].y - waterY -(triangle[2].y - waterY));

					Vector3 Im = triangle[1] + (triangle[0] - triangle[1])*tm;
					Vector3 Il = triangle[2] + (triangle[0] - triangle[2])*tl;

					intersectionPoints.Add (Im);
					intersectionPoints.Add (Il);
				}
			}

			// get two points of intersection
		}

		// draw water level
		for (int i = 0; i < intersectionPoints.Count; i+=2)
		{
			Debug.DrawLine (intersectionPoints [i], intersectionPoints [i+1], Color.red);
		}

		for (int i = 0; i < tris.Length; i += 3)
		{
			float d1 = transform.TransformPoint (vertices [tris[i]]).y - waterY;
			float d2 = transform.TransformPoint (vertices [tris[i + 1]]).y - waterY;
			float d3 = transform.TransformPoint (vertices [tris[i + 2]]).y - waterY;

			Vector3 v1 = transform.TransformPoint (vertices [tris [i]]);
			Vector3 v2 = transform.TransformPoint (vertices [tris [i+1]]);
			Vector3 v3 = transform.TransformPoint (vertices [tris [i+2]]);

			Vector3 vH;
			Vector3 vM;
			Vector3 vL;

			if (v1.y > v2.y && v1.y > v3.y) vH = v1;
			else if (v2.y > v1.y && v2.y > v3.y) vH = v2;
			else vH = v3;

			if (v1.y < v2.y && v1.y < v3.y) vL = v1;
			else if (v2.y < v1.y && v2.y < v3.y) vL = v2;
			else vL = v3;

			if ((vH == v2 && vL == v3) || (vH == v3 && vL == v2)) vM = v1;
			else if ((vH == v1 && vL == v3) || (vH == v3 && vL == v1)) vM = v2;
			else vM = v3;

			if (d1 < 0 || d2 < 0 || d3 < 0)
			{
				//Debug.Log ("Triangle " + i + "is underwater");

				//triangles.Add (new TriangleData ( vertices [tris[i]], vertices [tris[i + 1]], vertices [tris[i + 2]]));
			}	
		}
	}

	void CalculateBuoyancy()
	{
		// F-> = -rho * g * h * area * n->

		// for each submerged triangle calculate force get F
		// list of fully submerged triangles is calculated above
		// sum up all forces and use only y coord for now

		Vector3 force = new Vector3 ();

		for (int i = 0; i < submergedTris.Count; i++)
		{
			if (submergedTris [i].Center.y < waterMesh.transform.TransformPoint (waterMesh.mesh.vertices [0]).y)
				force += -rho * g * submergedTris [i].Center.y * submergedTris [i].Normal;
		}
		rigidBody.AddForce (force);
	}
}

