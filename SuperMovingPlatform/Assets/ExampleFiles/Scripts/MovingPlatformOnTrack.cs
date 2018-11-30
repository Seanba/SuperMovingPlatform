using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

namespace SuperMovingPlatform
{
    public class MovingPlatformOnTrack : MonoBehaviour
    {
        // How close we need to be to a track to start off attached to it
        private const float MaxDistanceFromTrack = 16.0f;

        public bool m_IsCounterClockwise = true; // fixit - hook up and override with properties
        public float m_VelocityPixelsPerSecond = 64.0f; // fixit - control through properties

        // This platform is always placed between two points on a track
        // fixit - make private but serializable
        public Vector2[] m_Points;
        public int m_CurrentPointIndex = -1;

        public bool AssignTrackIfClose(EdgeCollider2D track)
        {
            if (m_CurrentPointIndex != -1)
            {
                // Already assigned to a track
                return false;
            }

            var points = track.points;
            Assert.IsTrue(points.Length > 1);

            var pos = gameObject.transform.position;
            var minDistance = float.MaxValue;
            var ptOnTrack = Vector2.zero;
            var ptIndex = -1;

            // Find closest position in the line segments passed in
            for (int i = 0; i < points.Length - 1; i++)
            {
                var A = points[i];
                var B = points[i + 1];
                var ptPotential = ClosestPointOnLineSegment(pos, A, B);

                var distance = Vector2.Distance(pos, ptPotential);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    ptOnTrack = ptPotential;
                    ptIndex = i;
                }
            }

            // Are we close enough to the track to be attached to it?
            if (minDistance < MaxDistanceFromTrack)
            {
                m_CurrentPointIndex = ptIndex;
                m_Points = points;
                gameObject.transform.position = ptOnTrack;
                return true;
            }

            return false;
        }

        private Vector2 ClosestPointOnLineSegment(Vector2 P, Vector2 A, Vector2 B)
        {
            var P2 = new Vector2(B.x - A.x, B.y - A.y);
            var dot = P2.x * P2.x + P2.y * P2.y;
            var u = ((P.x - A.x) * P2.x + (P.y - A.y) * P2.y) / dot;

            if (u > 1)
            {
                u = 1;
            }
            else if (u < 0)
            {
                u = 0;
            }

            return A + (u * P2);
        }

        private void Update()
        {
            if (m_CurrentPointIndex == -1)
            {
                Debug.LogError("Platform is not attached to a track.");
                return;
            }

            float t = 1.0f;
            while (t > 0.0f)
            {
                t = MoveAlongTrack(t);
            }
        }

        private float MoveAlongTrack(float t)
        {
            // fixit - use direction from properties

            // Move along an edge of our track as much as we can
            // If we end up stopping at an edge then return the portion of movement that is left over
            int i = m_CurrentPointIndex;
            int j = (m_CurrentPointIndex + 1) % m_Points.Length;

            var A = m_Points[i];
            var B = m_Points[j];

            var BA = B - A;
            var dv = BA.normalized;

            var posCurrent = (Vector2)gameObject.transform.position;
            var posDesired = posCurrent + (dv * m_VelocityPixelsPerSecond * Time.deltaTime * t);

            var V1A = posCurrent - A;
            var V2A = posDesired - A;

            float dotLimit = Vector2.Dot(dv, BA);
            float dotStart = Vector2.Dot(dv, V1A);
            float dotDesired = Vector2.Dot(dv, V2A);

            if (dotDesired < dotLimit)
            {
                // We are within the bounds of the edge we are moving across
                // Fully move to our desired position
                gameObject.transform.position = posDesired;
                return 0;
            }
            else
            {
                // Our desired position is out out bounds
                // Lock to end position
                gameObject.transform.position = B;

                // Advance to the next edge in our track
                m_CurrentPointIndex = j;

                // How much movement do we have left over as a ratio?
                float leftOverRatio = (dotDesired - dotLimit) / (dotDesired - dotStart);
                return leftOverRatio * t;
            }
        }
    }
}
