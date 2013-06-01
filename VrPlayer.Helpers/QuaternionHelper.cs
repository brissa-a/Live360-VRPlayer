using System;
using System.Windows.Media.Media3D;

namespace VrPlayer.Helpers
{
    public class QuaternionHelper
    {
        public static Quaternion EulerAnglesInDegToQuaternion(Vector3D angles)
        {
            return EulerAnglesInDegToQuaternion(angles.X, angles.Y, angles.Z);
        }

        public static Quaternion EulerAnglesInDegToQuaternion(double yaw, double pitch, double roll)
        {
            return EulerAnglesInRadToQuaternion(
                DegreeToRadian(yaw),
                DegreeToRadian(pitch),
                DegreeToRadian(roll));
        }

        public static Quaternion EulerAnglesInRadToQuaternion(Vector3D angles)
        {
            return EulerAnglesInRadToQuaternion(angles.X, angles.Y, angles.Z);
        }

        public static Quaternion EulerAnglesInRadToQuaternion(double pitch, double yaw, double roll)
        {
            var rollOver2 = roll * 0.5;
            var sinRollOver2 = Math.Sin(rollOver2);
            var cosRollOver2 = Math.Cos(rollOver2);
            var yawOver2 = -yaw * 0.5;
            var sinYawOver2 = Math.Sin(yawOver2);
            var cosYawOver2 = Math.Cos(yawOver2);
            var pitchOver2 = pitch * 0.5;
            var sinPitchOver2 = Math.Sin(pitchOver2);
            var cosPitchOver2 = Math.Cos(pitchOver2);
            var result = new Quaternion
            {
                X = cosPitchOver2 * cosYawOver2 * cosRollOver2 + sinPitchOver2 * sinYawOver2 * sinRollOver2,
                Y = cosPitchOver2 * cosYawOver2 * sinRollOver2 - sinPitchOver2 * sinYawOver2 * cosRollOver2,
                Z = cosPitchOver2 * sinYawOver2 * cosRollOver2 + sinPitchOver2 * cosYawOver2 * sinRollOver2,
                W = sinPitchOver2 * cosYawOver2 * cosRollOver2 - cosPitchOver2 * sinYawOver2 * sinRollOver2
            };
            return result;
        }

        public static Vector3D QuaternionToEulerAnglesInDeg(Quaternion q)
        {
            return RadianToDegree(QuaternionToEulerAnglesInRad(q));
        }

        public static Vector3D QuaternionToEulerAnglesInRad(Quaternion q)
        {
            var pitchYawRoll = new Vector3D
            {
                Y = (float)Math.Atan2(2f * q.X * q.W + 2f * q.Y * q.Z, 1 - 2f * (q.Z * q.Z + q.W * q.W)),
                X = (float)Math.Asin(2f * (q.X * q.Z - q.W * q.Y)),
                Z = (float)Math.Atan2(2f * q.X * q.Y + 2f * q.Z * q.W, 1 - 2f * (q.Y * q.Y + q.Z * q.Z))
            };
            return pitchYawRoll;
        }

        #region Specifics Vectors

        public static Vector3D FrontVectorFromQuaternion(Quaternion q)
        {
            return new Vector3D(
                2 * (q.X * q.Z + q.W * q.Y),
                2 * (q.Y * q.Z - q.W * q.X),
                1 - 2 * (q.X * q.X + q.Y * q.Y)
            );
        }

        public static Vector3D UpVectorFromQuaternion(Quaternion q)
        {
            return new Vector3D(
                2 * (q.X * q.Y - q.W * q.Z),
                1 - 2 * (q.X * q.X + q.Z * q.Z),
                2 * (q.Y * q.Z + q.W * q.X)
            );
        }

        #endregion

        #region Deg / Rad conversion

        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static Vector3D DegreeToRadian(Vector3D angles)
        {
            return new Vector3D
            {
                X = DegreeToRadian(angles.X),
                Y = DegreeToRadian(angles.Y),
                Z = DegreeToRadian(angles.Z)
            };
        }

        public static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        public static Vector3D RadianToDegree(Vector3D angles)
        {
            return new Vector3D
            {
                X = RadianToDegree(angles.X),
                Y = RadianToDegree(angles.Y),
                Z = RadianToDegree(angles.Z)
            };
        }

        #endregion
    }
}