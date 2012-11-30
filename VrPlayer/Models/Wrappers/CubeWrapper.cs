using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace VrPlayer.Models.Wrappers
{
    public class CubeWrapper : WrapperBase, IWrapper
    {
        double _distance = 2;

        public override Point3DCollection Positions
        {
            get 
            {
                Point3DCollection positions = new Point3DCollection();

                //Left Cube

                //Bottom
                positions.Add(new Point3D(_distance - 1, -1, -1));//0
                positions.Add(new Point3D(_distance + 1, -1, -1));//1
                positions.Add(new Point3D(_distance + 1, -1, 1));//2
                positions.Add(new Point3D(_distance - 1, -1, 1));//3

                //Front
                positions.Add(new Point3D(_distance - 1, -1, 1));//4
                positions.Add(new Point3D(_distance + 1, -1, 1));//5
                positions.Add(new Point3D(_distance + 1, 1, 1));//6
                positions.Add(new Point3D(_distance - 1, 1, 1));//7

                //Right
                positions.Add(new Point3D(_distance + 1, -1, 1));//8
                positions.Add(new Point3D(_distance + 1, -1, -1));//9
                positions.Add(new Point3D(_distance + 1, 1, -1));//10
                positions.Add(new Point3D(_distance + 1, 1, 1));//11

                //Top
                positions.Add(new Point3D(_distance + 1, 1, 1));//12
                positions.Add(new Point3D(_distance + 1, 1, -1));//13
                positions.Add(new Point3D(_distance - 1, 1, -1));//14
                positions.Add(new Point3D(_distance - 1, 1, 1));//15

                //Left
                positions.Add(new Point3D(_distance - 1, -1, 1));//16
                positions.Add(new Point3D(_distance - 1, 1, 1));//17
                positions.Add(new Point3D(_distance - 1, 1, -1));//18
                positions.Add(new Point3D(_distance - 1, -1, -1));//19

                //Back
                positions.Add(new Point3D(_distance - 1, -1, -1));//20
                positions.Add(new Point3D(_distance - 1, 1, -1));//21
                positions.Add(new Point3D(_distance + 1, 1, -1));//22
                positions.Add(new Point3D(_distance + 1, -1, -1));//23

                //Right cube

                //Bottom
                positions.Add(new Point3D(-_distance - 1, -1, -1));//0-24
                positions.Add(new Point3D(-_distance + 1, -1, -1));//1-25
                positions.Add(new Point3D(-_distance + 1, -1, 1));//2-26
                positions.Add(new Point3D(-_distance - 1, -1, 1));//3-27

                //Front
                positions.Add(new Point3D(-_distance - 1, -1, 1));//4-28
                positions.Add(new Point3D(-_distance + 1, -1, 1));//5-29
                positions.Add(new Point3D(-_distance + 1, 1, 1));//6-30
                positions.Add(new Point3D(-_distance - 1, 1, 1));//7-31

                //Right
                positions.Add(new Point3D(-_distance + 1, -1, 1));//8-32
                positions.Add(new Point3D(-_distance + 1, -1, -1));//9-33
                positions.Add(new Point3D(-_distance + 1, 1, -1));//10-34
                positions.Add(new Point3D(-_distance + 1, 1, 1));//11-35

                //Top
                positions.Add(new Point3D(-_distance + 1, 1, 1));//12-36
                positions.Add(new Point3D(-_distance + 1, 1, -1));//13-37
                positions.Add(new Point3D(-_distance - 1, 1, -1));//14-38
                positions.Add(new Point3D(-_distance - 1, 1, 1));//15-39

                //Left
                positions.Add(new Point3D(-_distance - 1, -1, 1));//16-40
                positions.Add(new Point3D(-_distance - 1, 1, 1));//17-41
                positions.Add(new Point3D(-_distance - 1, 1, -1));//18-42
                positions.Add(new Point3D(-_distance - 1, -1, -1));//19-43

                //Back
                positions.Add(new Point3D(-_distance - 1, -1, -1));//20-44
                positions.Add(new Point3D(-_distance - 1, 1, -1));//21-45
                positions.Add(new Point3D(-_distance + 1, 1, -1));//22-46
                positions.Add(new Point3D(-_distance + 1, -1, -1));//23-47

                return positions;
            }
        }

        public override Int32Collection TriangleIndices
        {
            get 
            {
                Int32Collection triangleIndices = new Int32Collection();

                //Left cube

                //Bottom
                triangleIndices.Add(0);
                triangleIndices.Add(1);
                triangleIndices.Add(2);
                triangleIndices.Add(2);
                triangleIndices.Add(3);
                triangleIndices.Add(0);

                //Front
                triangleIndices.Add(4);
                triangleIndices.Add(5);
                triangleIndices.Add(6);
                triangleIndices.Add(6);
                triangleIndices.Add(7);
                triangleIndices.Add(4);

                //Right
                triangleIndices.Add(8);
                triangleIndices.Add(9);
                triangleIndices.Add(10);
                triangleIndices.Add(10);
                triangleIndices.Add(11);
                triangleIndices.Add(8);

                //Top
                triangleIndices.Add(12);
                triangleIndices.Add(13);
                triangleIndices.Add(14);
                triangleIndices.Add(14);
                triangleIndices.Add(15);
                triangleIndices.Add(12);

                //Right
                triangleIndices.Add(16);
                triangleIndices.Add(17);
                triangleIndices.Add(18);
                triangleIndices.Add(18);
                triangleIndices.Add(19);
                triangleIndices.Add(16);

                //Back
                triangleIndices.Add(20);
                triangleIndices.Add(21);
                triangleIndices.Add(22);
                triangleIndices.Add(22);
                triangleIndices.Add(23);
                triangleIndices.Add(20);

                //Right cube

                //Bottom
                triangleIndices.Add(0 + 24);
                triangleIndices.Add(1 + 24);
                triangleIndices.Add(2 + 24);
                triangleIndices.Add(2 + 24);
                triangleIndices.Add(3 + 24);
                triangleIndices.Add(0 + 24);

                //Front
                triangleIndices.Add(4 + 24);
                triangleIndices.Add(5 + 24);
                triangleIndices.Add(6 + 24);
                triangleIndices.Add(6 + 24);
                triangleIndices.Add(7 + 24);
                triangleIndices.Add(4 + 24);

                //Right
                triangleIndices.Add(8 + 24);
                triangleIndices.Add(9 + 24);
                triangleIndices.Add(10 + 24);
                triangleIndices.Add(10 + 24);
                triangleIndices.Add(11 + 24);
                triangleIndices.Add(8 + 24);

                //Top
                triangleIndices.Add(12 + 24);
                triangleIndices.Add(13 + 24);
                triangleIndices.Add(14 + 24);
                triangleIndices.Add(14 + 24);
                triangleIndices.Add(15 + 24);
                triangleIndices.Add(12 + 24);

                //Right
                triangleIndices.Add(16 + 24);
                triangleIndices.Add(17 + 24);
                triangleIndices.Add(18 + 24);
                triangleIndices.Add(18 + 24);
                triangleIndices.Add(19 + 24);
                triangleIndices.Add(16 + 24);

                //Back
                triangleIndices.Add(20 + 24);
                triangleIndices.Add(21 + 24);
                triangleIndices.Add(22 + 24);
                triangleIndices.Add(22 + 24);
                triangleIndices.Add(23 + 24);
                triangleIndices.Add(20 + 24);

                return triangleIndices;
            }
        }

        public override PointCollection MonoTextureCoordinates
        {
            get 
            {
                PointCollection textureCoordinates = new PointCollection();

                //Left cube

                textureCoordinates.Add(new Point(-1, 1));
                textureCoordinates.Add(new Point(-1, -1));
                textureCoordinates.Add(new Point(1, -1));
                textureCoordinates.Add(new Point(1, 1));

                textureCoordinates.Add(new Point(-1, 1));
                textureCoordinates.Add(new Point(-1, -1));
                textureCoordinates.Add(new Point(1, -1));
                textureCoordinates.Add(new Point(1, 1));

                textureCoordinates.Add(new Point(-1, 1));
                textureCoordinates.Add(new Point(-1, -1));
                textureCoordinates.Add(new Point(1, -1));
                textureCoordinates.Add(new Point(1, 1));

                textureCoordinates.Add(new Point(-1, 1));
                textureCoordinates.Add(new Point(-1, -1));
                textureCoordinates.Add(new Point(1, -1));
                textureCoordinates.Add(new Point(1, 1));

                textureCoordinates.Add(new Point(-1, 1));
                textureCoordinates.Add(new Point(-1, -1));
                textureCoordinates.Add(new Point(1, -1));
                textureCoordinates.Add(new Point(1, 1));

                textureCoordinates.Add(new Point(-1, 1));
                textureCoordinates.Add(new Point(-1, -1));
                textureCoordinates.Add(new Point(1, -1));
                textureCoordinates.Add(new Point(1, 1));

                //Right cube

                textureCoordinates.Add(new Point(-1, 1));
                textureCoordinates.Add(new Point(-1, -1));
                textureCoordinates.Add(new Point(1, -1));
                textureCoordinates.Add(new Point(1, 1));

                textureCoordinates.Add(new Point(-1, 1));
                textureCoordinates.Add(new Point(-1, -1));
                textureCoordinates.Add(new Point(1, -1));
                textureCoordinates.Add(new Point(1, 1));

                textureCoordinates.Add(new Point(-1, 1));
                textureCoordinates.Add(new Point(-1, -1));
                textureCoordinates.Add(new Point(1, -1));
                textureCoordinates.Add(new Point(1, 1));

                textureCoordinates.Add(new Point(-1, 1));
                textureCoordinates.Add(new Point(-1, -1));
                textureCoordinates.Add(new Point(1, -1));
                textureCoordinates.Add(new Point(1, 1));

                textureCoordinates.Add(new Point(-1, 1));
                textureCoordinates.Add(new Point(-1, -1));
                textureCoordinates.Add(new Point(1, -1));
                textureCoordinates.Add(new Point(1, 1));

                textureCoordinates.Add(new Point(-1, 1));
                textureCoordinates.Add(new Point(-1, -1));
                textureCoordinates.Add(new Point(1, -1));
                textureCoordinates.Add(new Point(1, 1));

                return textureCoordinates;
            }
        }

        public override PointCollection OverUnderTextureCoordinates
        {
            get 
            {
                PointCollection textureCoordinates = new PointCollection();

                //Left cube

                textureCoordinates.Add(new Point(-1, 0));
                textureCoordinates.Add(new Point(-1, -1));
                textureCoordinates.Add(new Point(1, -1));
                textureCoordinates.Add(new Point(1, 0));

                textureCoordinates.Add(new Point(-1, 0));
                textureCoordinates.Add(new Point(-1, -1));
                textureCoordinates.Add(new Point(1, -1));
                textureCoordinates.Add(new Point(1, 0));

                textureCoordinates.Add(new Point(-1, 0));
                textureCoordinates.Add(new Point(-1, -1));
                textureCoordinates.Add(new Point(1, -1));
                textureCoordinates.Add(new Point(1, 0));

                textureCoordinates.Add(new Point(-1, 0));
                textureCoordinates.Add(new Point(-1, -1));
                textureCoordinates.Add(new Point(1, -1));
                textureCoordinates.Add(new Point(1, 0));

                textureCoordinates.Add(new Point(-1, 0));
                textureCoordinates.Add(new Point(-1, -1));
                textureCoordinates.Add(new Point(1, -1));
                textureCoordinates.Add(new Point(1, 0));

                textureCoordinates.Add(new Point(-1, 0));
                textureCoordinates.Add(new Point(-1, -1));
                textureCoordinates.Add(new Point(1, -1));
                textureCoordinates.Add(new Point(1, 0));

                //Right cube

                textureCoordinates.Add(new Point(-1, 1));
                textureCoordinates.Add(new Point(-1, 0));
                textureCoordinates.Add(new Point(1, 0));
                textureCoordinates.Add(new Point(1, 1));

                textureCoordinates.Add(new Point(-1, 1));
                textureCoordinates.Add(new Point(-1, 0));
                textureCoordinates.Add(new Point(1, 0));
                textureCoordinates.Add(new Point(1, 1));

                textureCoordinates.Add(new Point(-1, 1));
                textureCoordinates.Add(new Point(-1, 0));
                textureCoordinates.Add(new Point(1, 0));
                textureCoordinates.Add(new Point(1, 1));

                textureCoordinates.Add(new Point(-1, 1));
                textureCoordinates.Add(new Point(-1, 0));
                textureCoordinates.Add(new Point(1, 0));
                textureCoordinates.Add(new Point(1, 1));

                textureCoordinates.Add(new Point(-1, 1));
                textureCoordinates.Add(new Point(-1, 0));
                textureCoordinates.Add(new Point(1, 0));
                textureCoordinates.Add(new Point(1, 1));

                textureCoordinates.Add(new Point(-1, 1));
                textureCoordinates.Add(new Point(-1, 0));
                textureCoordinates.Add(new Point(1, 0));
                textureCoordinates.Add(new Point(1, 1));

                return textureCoordinates;
            }
        }

        public override PointCollection SideBySideTextureCoordinates
        {
            get 
            {
                PointCollection textureCoordinates = new PointCollection();
                
                //Left cube

                textureCoordinates.Add(new Point(-1, 1));
                textureCoordinates.Add(new Point(-1, -1));
                textureCoordinates.Add(new Point(0, -1));
                textureCoordinates.Add(new Point(0, 1));

                textureCoordinates.Add(new Point(-1, 1));
                textureCoordinates.Add(new Point(-1, -1));
                textureCoordinates.Add(new Point(0, -1));
                textureCoordinates.Add(new Point(0, 1));

                textureCoordinates.Add(new Point(-1, 1));
                textureCoordinates.Add(new Point(-1, -1));
                textureCoordinates.Add(new Point(0, -1));
                textureCoordinates.Add(new Point(0, 1));

                textureCoordinates.Add(new Point(-1, 1));
                textureCoordinates.Add(new Point(-1, -1));
                textureCoordinates.Add(new Point(0, -1));
                textureCoordinates.Add(new Point(0, 1));

                textureCoordinates.Add(new Point(-1, 1));
                textureCoordinates.Add(new Point(-1, -1));
                textureCoordinates.Add(new Point(0, -1));
                textureCoordinates.Add(new Point(0, 1));

                textureCoordinates.Add(new Point(-1, 1));
                textureCoordinates.Add(new Point(-1, -1));
                textureCoordinates.Add(new Point(0, -1));
                textureCoordinates.Add(new Point(0, 1));

                //Right cube

                textureCoordinates.Add(new Point(0, 1));
                textureCoordinates.Add(new Point(0, -1));
                textureCoordinates.Add(new Point(1, -1));
                textureCoordinates.Add(new Point(1, 1));

                textureCoordinates.Add(new Point(0, 1));
                textureCoordinates.Add(new Point(0, -1));
                textureCoordinates.Add(new Point(1, -1));
                textureCoordinates.Add(new Point(1, 1));

                textureCoordinates.Add(new Point(0, 1));
                textureCoordinates.Add(new Point(0, -1));
                textureCoordinates.Add(new Point(1, -1));
                textureCoordinates.Add(new Point(1, 1));

                textureCoordinates.Add(new Point(0, 1));
                textureCoordinates.Add(new Point(0, -1));
                textureCoordinates.Add(new Point(1, -1));
                textureCoordinates.Add(new Point(1, 1));

                textureCoordinates.Add(new Point(0, 1));
                textureCoordinates.Add(new Point(0, -1));
                textureCoordinates.Add(new Point(1, -1));
                textureCoordinates.Add(new Point(1, 1));

                textureCoordinates.Add(new Point(0, 1));
                textureCoordinates.Add(new Point(0, -1));
                textureCoordinates.Add(new Point(1, -1));
                textureCoordinates.Add(new Point(1, 1));

                return textureCoordinates;
            }
        }

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
    }
}

