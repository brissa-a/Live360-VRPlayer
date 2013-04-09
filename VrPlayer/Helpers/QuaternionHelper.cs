using System;
using System.Windows.Media.Media3D;

namespace VrPlayer.Helpers
{
    public class QuaternionHelper
    {
        public static Quaternion QuaternionFromEulerAngles(Vector3D eulerAngles)
        {
            return QuaternionFromEulerAngles(eulerAngles.X, eulerAngles.Y, eulerAngles.Z);
        }

        //Source: http://www.vbforums.com/showthread.php?637168-WPF-3D-orbiting-camera-(pitch-yaw-rotation-only)
        public static Quaternion QuaternionFromEulerAngles(double pitch, double yaw, double roll)
        {
            Vector3D yawAxis = new Vector3D(0, 1, 0);
            Vector3D pitchAxis = new Vector3D(1, 0, 0);
            Vector3D rollAxis = new Vector3D(0, 0, 1);

            Transform3DGroup group = new Transform3DGroup();
            QuaternionRotation3D r;
            r = new QuaternionRotation3D(new Quaternion(yawAxis, yaw));
            group.Children.Add(new RotateTransform3D(r));
            r = new QuaternionRotation3D(new Quaternion(pitchAxis, pitch));
            group.Children.Add(new RotateTransform3D(r));
            r = new QuaternionRotation3D(new Quaternion(rollAxis, roll));
            group.Children.Add(new RotateTransform3D(r));

            return QuaternionFromMatrix(group.Value);
        }

        //Source: http://www.gamedev.net/topic/543583-quaternion-to-matrix-and-back/
        public static Quaternion QuaternionFromMatrix(Matrix3D m)
        {
            double ZERO_THRESHOLD = 0.00001;

            Quaternion q = new Quaternion();

	        //det(m) must b 1
            double det = m.Determinant;
	        // we'll accept something very close because of rounding errors
	        if(Math.Abs(det-1) > ZERO_THRESHOLD)
	        {
		        return q;
	        }
	
            // diagonal elements must sum > 0
	        double trace = m.M11 + m.M22 + m.M33 + m.M44;
	
            if(trace > ZERO_THRESHOLD)
	        {			
		        q.W = Math.Sqrt(trace) / 2;
		        q.X = (m.M32 - m.M23) / (4*q.W);
		        q.Y = (m.M13 - m.M31) / (4*q.W);
		        q.Z = (m.M21 - m.M12) / (4*q.W);
	        }
	        else
	        {
		        if ((m.M11 > m.M22)&&(m.M11 > m.M33))
		        { 
			        double S = Math.Sqrt( 1.0f + m.M11 - m.M22 - m.M33 ) * 2; // S=4*qx 
			        q.W = (m.M32 - m.M23) / S;
			        q.X = 0.25f * S;
			        q.Y = (m.M12 + m.M21) / S; 
			        q.Z = (m.M13 + m.M31) / S; 
		        }
		        else if (m.M22 > m.M33) 
		        { 
			        double S = Math.Sqrt( 1.0 + m.M22 - m.M11 - m.M33 ) * 2; // S=4*qy
			        q.W = (m.M13 - m.M31) / S;
			        q.X = (m.M12 + m.M21) / S; 
			        q.Y = 0.25f * S;
			        q.Z = (m.M23 + m.M32) / S; 
		        }
		        else 
		        { 
			        double S = Math.Sqrt( 1.0 + m.M33 - m.M11 - m.M22 ) * 2; // S=4*qz
			        q.W = (m.M21 - m.M12) / S;
			        q.X = (m.M13 + m.M31) / S; 
			        q.Y = (m.M23 + m.M32) / S; 
			        q.Z = 0.25f * S;
		        } 
	        }

	        q.Normalize();
	        return q;
        }

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
    }
}