using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace VrPlayer.Models.Wrappers
{
    public class PlaneWrapper : WrapperBase, IWrapper
    {
        double _distance = 1000;

        public new Vector3D CameraLeftPosition
        {
            get
            {
                return new Vector3D(_distance, 0, 0);
            }
        }

        public new Vector3D CameraRightPosition
        {
            get
            {
                return new Vector3D(-_distance, 0, 0);
            }
        }

        public override Point3DCollection Positions
        {
            get 
            {
                Point3DCollection positions = new Point3DCollection();

                //Left
                positions.Add(new Point3D(_distance - 1, -1, 1));//0
                positions.Add(new Point3D(_distance + 1, -1, 1));//1
                positions.Add(new Point3D(_distance + 1, 1, 1));//2
                positions.Add(new Point3D(_distance - 1, 1, 1));//3

                //Right
                positions.Add(new Point3D(-_distance - 1, -1, 1));//4
                positions.Add(new Point3D(-_distance + 1, -1, 1));//5
                positions.Add(new Point3D(-_distance + 1, 1, 1));//6
                positions.Add(new Point3D(-_distance - 1, 1, 1));//7

                return positions;
            }
        }

        public override Int32Collection TriangleIndices
        {
            get 
            {
                Int32Collection triangleIndices = new Int32Collection();

                //Left
                triangleIndices.Add(0);
                triangleIndices.Add(1);
                triangleIndices.Add(2);
                triangleIndices.Add(2);
                triangleIndices.Add(3);
                triangleIndices.Add(0);

                //Right
                triangleIndices.Add(0 + 4);
                triangleIndices.Add(1 + 4);
                triangleIndices.Add(2 + 4);
                triangleIndices.Add(2 + 4);
                triangleIndices.Add(3 + 4);
                triangleIndices.Add(0 + 4);

                return triangleIndices;
            }
        }

        public override PointCollection MonoTextureCoordinates
        {
            get
            {
                PointCollection textureCoordinates = new PointCollection();

                //Left
                textureCoordinates.Add(new Point(1, 1));
                textureCoordinates.Add(new Point(0, 1));
                textureCoordinates.Add(new Point(0, 0));
                textureCoordinates.Add(new Point(1, 0));
                
                //Right
                textureCoordinates.Add(new Point(1, 1));
                textureCoordinates.Add(new Point(0, 1));
                textureCoordinates.Add(new Point(0, 0));
                textureCoordinates.Add(new Point(1, 0));

                return textureCoordinates;
            }
        }

        public override PointCollection OverUnderTextureCoordinates
        {
            get 
            {
                PointCollection textureCoordinates = new PointCollection();

                //Left
                textureCoordinates.Add(new Point(1, 0.5));
                textureCoordinates.Add(new Point(0, 0.5));
                textureCoordinates.Add(new Point(0, 0));
                textureCoordinates.Add(new Point(1, 0));
                
                //Right
                textureCoordinates.Add(new Point(1, 1));
                textureCoordinates.Add(new Point(0, 1));
                textureCoordinates.Add(new Point(0, 0.5));
                textureCoordinates.Add(new Point(1, 0.5));

                return textureCoordinates;
            }
        }

        public override PointCollection SideBySideTextureCoordinates
        {
            get 
            {
                PointCollection textureCoordinates = new PointCollection();

                //Left
                textureCoordinates.Add(new Point(0.5, 1));
                textureCoordinates.Add(new Point(0, 1));
                textureCoordinates.Add(new Point(0, 0));
                textureCoordinates.Add(new Point(0.5, 0));

                //Right
                textureCoordinates.Add(new Point(1, 1));
                textureCoordinates.Add(new Point(0.5, 1));
                textureCoordinates.Add(new Point(0.5, 0));
                textureCoordinates.Add(new Point(1, 0));

                return textureCoordinates;
            }
        }
    }
}

