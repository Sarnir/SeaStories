using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class Math
    {
        public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
        {
            return Mathf.Atan2(
                Vector3.Dot(n, Vector3.Cross(v1, v2)),
                Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
        }

        public static float ClampAngleBetween0360(float angle)
        {
            while (angle < 0)
                angle += 360f;

            while (angle > 360)
                angle -= 360f;

            return angle;
        }
    }
}