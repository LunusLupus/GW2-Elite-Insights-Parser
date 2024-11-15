﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace GW2EIEvtcParser.EIData
{
    public class Point3D
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }

        private static float Mix(float a, float b, float c)
        {
            return (1.0f - c) * a + c * b;
        }

        public static bool operator >(Point3D a, Point3D b)
        {
            return a.X > b.X && a.Y > b.Y && a.Z > b.Z;
        }

        public static bool operator <(Point3D a, Point3D b)
        {
            return a.X < b.X && a.Y < b.Y && a.Z < b.Z;
        }

        public static bool operator >=(Point3D a, Point3D b)
        {
            return a.X >= b.X && a.Y >= b.Y && a.Z >= b.Z;
        }

        public static bool operator <=(Point3D a, Point3D b)
        {
            return a.X <= b.X && a.Y <= b.Y && a.Z <= b.Z;
        }

        public static Point3D operator +(Point3D a, Point3D b)
        {
            var newPt = new Point3D(a);
            newPt.Add(b);
            return newPt;
        }

        public static Point3D operator -(Point3D a, Point3D b)
        {
            var newPt = new Point3D(a);
            newPt.Substract(b);
            return newPt;
        }
        public static Point3D operator *(Point3D a, Point3D b)
        {
            var newPt = new Point3D(a);
            newPt.Multiply(b);
            return newPt;
        }
        public static Point3D operator *(float a, Point3D b)
        {
            var newPt = new Point3D(b);
            newPt.MultiplyScalar(a);
            return newPt;
        }
        public static Point3D operator *(Point3D a, float b)
        {
            var newPt = new Point3D(a);
            newPt.MultiplyScalar(b);
            return newPt;
        }
        public static Point3D operator /(Point3D a, Point3D b)
        {
            var newPt = new Point3D(a);
            newPt.Divide(b);
            return newPt;
        }
        public static Point3D operator /(float a, Point3D b)
        {
            var newPt = new Point3D(b);
            newPt.DivideScalar(a);
            return newPt;
        }
        public static Point3D operator /(Point3D a, float b)
        {
            var newPt = new Point3D(a);
            newPt.DivideScalar(b);
            return newPt;
        }
        public static Point3D operator -(Point3D a)
        {
            var newPt = new Point3D(a);
            newPt.MultiplyScalar(-1);
            return newPt;
        }
        public static float ScalarProduct(Point3D pt1, Point3D pt2)
        {
            return pt1.X * pt2.X + pt1.Y * pt2.Y + pt1.Z * pt2.Z;
        }

        private void Add(Point3D a)
        {
            X += a.X;
            Y += a.Y;
            Z += a.Z;
        }
        private void Substract(Point3D a)
        {
            X -= a.X;
            Y -= a.Y;
            Z -= a.Z;
        }
        private void Multiply(Point3D a)
        {
            X *= a.X;
            Y *= a.Y;
            Z *= a.Z;
        }
        private void Divide(Point3D a)
        {
            X /= a.X;
            Y /= a.Y;
            Z /= a.Z;
        }
        private void MultiplyScalar(float a)
        {
            X *= a;
            Y *= a;
            Z *= a;
        }
        private void DivideScalar(float a)
        {
            X /= a;
            Y /= a;
            Z /= a;
        }

        public float DistanceToPoint(Point3D endPoint)
        {
            float distance = (float)Math.Sqrt((endPoint.X - X) * (endPoint.X - X) + (endPoint.Y - Y) * (endPoint.Y - Y) + (endPoint.Z - Z) * (endPoint.Z - Z));
            return distance;
        }
        public float Distance2DToPoint(Point3D endPoint)
        {
            float distance = (float)Math.Sqrt((endPoint.X - X) * (endPoint.X - X) + (endPoint.Y - Y) * (endPoint.Y - Y));
            return distance;
        }

        public float Length2D()
        {
            float length = (float)Math.Sqrt(X * X + Y * Y);
            return length;
        }

        public float Length()
        {
            float length = (float)Math.Sqrt(X * X + Y * Y + Z * Z);
            return length;
        }

        public Point3D Normalize()
        {
            var l = Length();
            if (l ==  0.0f)
            {
                return new Point3D(0.0f, 0.0f, 0.0f);
            }
            return new Point3D(this) / l;
        }
        public Point2D Normalize2D()
        {
            var l = Length2D();
            if (l == 0.0f)
            {
                return new Point2D(0.0f, 0.0f);
            }
            return new Point2D(X, Y) / Length2D();
        }

        public Point3D(float x, float y)
        {
            X = x;
            Y = y;
            Z = 0;
        }

        public Point3D(float x, float y, float z) : this(x, y)
        {
            Z = z;
        }

        public Point3D(Point3D a) : this(a.X, a.Y, a.Z)
        {
        }
        public Point3D(Point2D a) : this(a, 0)
        {
        }
        public Point3D(Point2D a, float z) : this(a.X, a.Y, z)
        {
        }



        public Point3D(Point3D a, Point3D b, float ratio)
        {
            X = Mix(a.X, b.X, ratio);
            Y = Mix(a.Y, b.Y, ratio);
            Z = Mix(a.Z, b.Z, ratio);
        }

        public static Point3D ProjectPointOnLine(Point3D toProject, Point3D pointOnLine, Point3D directionVector)
        {
            var normalizedDirectionVector = directionVector.Normalize();
            var vectorToProject = toProject - pointOnLine;
            return ScalarProduct(vectorToProject, normalizedDirectionVector) * normalizedDirectionVector + pointOnLine;
        }

        public static Point3D ProjectPointOn2DLine(Point3D toProject, Point3D pointOnLine, Point3D directionVector)
        {
            return new Point3D(Point2D.ProjectPointOnLine(new Point2D(toProject), new Point2D(pointOnLine), new Point2D(directionVector)), pointOnLine.Z);
        }

        public static float GetZRotationFromFacing(Point3D facing)
        {
            return (float)Math.Round(ParserHelper.RadianToDegree(Math.Atan2(facing.Y, facing.X)), ParserHelper.CombatReplayDataDigit);
        }

        /// <summary>
        /// Returs true if p is inside or on the edges the triangle defined by p0, p1 and p2
        /// Triangle can be clockwise or counter clock wise
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static bool IsInTriangle2D(Point3D p, Point3D p0, Point3D p1, Point3D p2)
        {
            // barycentric coordinates
            // https://stackoverflow.com/questions/2049582/how-to-determine-if-a-point-is-in-a-2d-triangle
            // properly handles clockwise or counter clockwise
            var s = (p0.X - p2.X) * (p.Y - p2.Y) - (p0.Y - p2.Y) * (p.X - p2.X);
            var t = (p1.X - p0.X) * (p.Y - p0.Y) - (p1.Y - p0.Y) * (p.X - p0.X);

            if ((s < 0) != (t < 0) && s != 0 && t != 0)
            {
                return false;
            }

            var d = (p2.X - p1.X) * (p.Y - p1.Y) - (p2.Y - p1.Y) * (p.X - p1.X);
            return d == 0 || (d < 0) == (s + t <= 0);
        }

        /// <summary>
        /// Returns true if p in inside or on the edges of the triangle defined by points
        /// points must have exactly 3 values, returns false otherwise
        /// Triangle can be clockwise or counter clock wise
        /// </summary>
        /// <param name="p"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        public static bool IsInTriangle2D(Point3D p, IReadOnlyList<Point3D> points)
        {
            return points.Count == 3 && IsInTriangle2D(p, points[0], points[1], points[2]);
        }


        public static bool IsInBox(Point3D p, Point3D p0, Point3D p1)
        {
            var minBB = new Point3D(Math.Min(p0.X, p1.X), Math.Min(p0.Y, p1.Y), Math.Min(p0.Z, p1.Z));
            var maxBB = new Point3D(Math.Max(p0.X, p1.X), Math.Max(p0.Y, p1.Y), Math.Max(p0.Z, p1.Z));
            return p >= minBB && p <= maxBB;
        }

        public static bool IsInBox2D(Point3D p, Point3D p0, Point3D p1)
        {
            return Point2D.IsInBox(new Point2D(p), new Point2D(p0), new Point2D(p1));
        }
        public static bool IsInBox2D(Point3D p, Point2D p0, Point2D p1)
        {
            return Point2D.IsInBox(new Point2D(p), p0, p1);
        }


        /// <summary>
        /// Calculates the central 3D point based on the points provided.
        /// </summary>
        /// <param name="points">List of points to use to find the center.</param>
        /// <returns><see cref="Point3D"/> with the center values.</returns>
        public static Point3D FindCentralPoint(IReadOnlyList<Point3D> points)
        {
            float sumX = points.Sum(p => p.X);
            float sumY = points.Sum(p => p.Y);
            float sumZ = points.Sum(p => p.Z);
            return new Point3D(sumX / points.Count, sumY / points.Count, sumZ / points.Count);
        }
    }
}
