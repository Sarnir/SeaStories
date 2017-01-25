using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class TriangleData
{
	public Vector3 Center;
	public Vector3 Normal;

	public TriangleData(Vector3 v1, Vector3 v2, Vector3 v3)
	{
		Center = CalculateCenter(v1, v2, v3);
		Normal = CalculateNormal(v1, v2, v3);
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

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(Rigidbody))]
public class BoatPhysics : MonoBehaviour {

	public MeshFilter waterMesh;
	public float rho = 997.8f;
	float g = Physics.gravity.y;

	Mesh mesh;
	Rigidbody rigidBody;

	List<TriangleData> triangles;

	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshFilter> ().mesh;
		rigidBody = GetComponent<Rigidbody> ();
		triangles = new List<TriangleData> ();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		CheckUnderwaterVertices ();
		CalculateBuoyancy ();
	}

	void CheckUnderwaterVertices()
	{
		triangles.Clear ();

		var vertices = mesh.vertices;
		var tris = mesh.triangles;
		// for now let's say that water is perfectly flat
		float waterY = waterMesh.mesh.vertices[0].y + waterMesh.transform.position.y;

		for (int i = 0; i < vertices.Length; i++)
		{
			if (transform.TransformPoint(vertices [i]).y < waterY)
				Debug.Log ("Vertex " + i + " is under water!");
		}

		for (int i = 0; i < tris.Length; i += 3)
		{
			float d1 = transform.TransformPoint (vertices [tris[i]]).y - waterY;
			float d2 = transform.TransformPoint (vertices [tris[i + 1]]).y - waterY;
			float d3 = transform.TransformPoint (vertices [tris[i + 2]]).y - waterY;

			if (d1 < 0 || d2 < 0 || d3 < 0)
			{
				Debug.Log ("Triangle " + i + "is underwater");

				triangles.Add (new TriangleData ( vertices [tris[i]], vertices [tris[i + 1]], vertices [tris[i + 2]]));
			}	
		}
	}

	void CalculateBuoyancy()
	{
		// F-> = -rho * g * h * n->

		// for each submerged triangle calculate force get F
		// list of fully submerged triangles is calculated above
		// sum up all forces and use only y coord for now

		Vector3 force = new Vector3 ();

		for (int i = 0; i < triangles.Count; i++)
		{
			if (triangles [i].Center.y < waterMesh.transform.TransformPoint (waterMesh.mesh.vertices [0]).y)
				force += -rho * g * triangles [i].Center.y * triangles [i].Normal;
		}
		rigidBody.AddForce (force);
	}
}
