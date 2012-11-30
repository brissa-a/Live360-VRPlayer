//Source: Based on SphereMeshGenerator by Charles Petzold
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace VrPlayer.Models.Wrappers
{
    public class SphereWrapper : WrapperBase, IWrapper
    {
        int _slices = 32;
        int _stacks = 16;
        Point3D _center = new Point3D();
        double _radius = 1;

        public int Slices
        {
            get { return _slices; }
            set
            {
                _slices = value;
                OnPropertyChanged("Slices");
            }
        }

        public int Stacks
        {
            get { return _stacks; }
            set
            {
                _stacks = value;
                OnPropertyChanged("Stacks");
            }
        }

        public Point3D Center
        {
            get { return _center; }
            set
            {
                _center = value;
                OnPropertyChanged("Center");
            }
        }

        public double Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                OnPropertyChanged("Radius");
            }
        }

        public new Vector3D CameraLeftPosition
        {
            get
            {
                return new Vector3D(_radius, 0, 0);
            }
        }

        public new Vector3D CameraRightPosition
        {
            get
            {
                return new Vector3D(-_radius, 0, 0);
            }
        }

        public override Point3DCollection Positions
        {
            get 
            {
                Point3DCollection positions = new Point3DCollection();

                //LEFT
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    double phi = Math.PI / 2 - stack * Math.PI / Stacks;
                    double y = Radius * Math.Sin(phi);
                    double scale = -Radius * Math.Cos(phi);

                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        double theta = slice * 2 * Math.PI / Slices;
                        double x = scale * Math.Sin(theta) + Radius;
                        double z = scale * Math.Cos(theta);

                        Vector3D normal = new Vector3D(x, y, z);
                        positions.Add(normal + Center);
                    }
                }

                //RIGH
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    double phi = Math.PI / 2 - stack * Math.PI / Stacks;
                    double y = Radius * Math.Sin(phi);
                    double scale = -Radius * Math.Cos(phi);

                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        double theta = slice * 2 * Math.PI / Slices;
                        double x = scale * Math.Sin(theta) - Radius;
                        double z = scale * Math.Cos(theta);

                        Vector3D normal = new Vector3D(x, y, z);
                        positions.Add(normal + Center);
                    }
                }

                return positions;
            }
        }

        public override Int32Collection TriangleIndices
        {
            get 
            {
                Int32Collection triangleIndices = new Int32Collection();

                //LEFT
                for (int stack = 0; stack < Stacks; stack++)
                {
                    int top = (stack + 0) * (Slices + 1);
                    int bot = (stack + 1) * (Slices + 1);

                    for (int slice = 0; slice < Slices; slice++)
                    {
                        if (stack != 0)
                        {
                            triangleIndices.Add(top + slice);
                            triangleIndices.Add(bot + slice);
                            triangleIndices.Add(top + slice + 1);
                        }

                        if (stack != Stacks - 1)
                        {
                            triangleIndices.Add(top + slice + 1);
                            triangleIndices.Add(bot + slice);
                            triangleIndices.Add(bot + slice + 1);
                        }
                    }
                }

                // RIGHT
                for (int stack = Stacks; stack <= (Stacks * 2); stack++)
                {
                    int top = (stack + 0) * (Slices + 1);
                    int bot = (stack + 1) * (Slices + 1);

                    for (int slice = 0; slice < Slices; slice++)
                    {
                        if (stack != 0)
                        {
                            triangleIndices.Add(top + slice);
                            triangleIndices.Add(bot + slice);
                            triangleIndices.Add(top + slice + 1);
                        }

                        if (stack != Stacks - 1)
                        {
                            triangleIndices.Add(top + slice + 1);
                            triangleIndices.Add(bot + slice);
                            triangleIndices.Add(bot + slice + 1);
                        }
                    }
                }

                return triangleIndices;
            }
        }

        public override PointCollection MonoTextureCoordinates
        {
            get 
            {
                PointCollection textureCoordinates = new PointCollection();

                //Left
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    double phi = Math.PI / 2 - stack * Math.PI / Stacks;
                    double y = Radius * Math.Sin(phi);
                    double scale = -Radius * Math.Cos(phi);

                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        double theta = slice * 2 * Math.PI / Slices;
                        double x = scale * Math.Sin(theta);
                        double z = scale * Math.Cos(theta);

                        Vector3D normal = new Vector3D(x, y, z);
                        textureCoordinates.Add(
                                    new Point((double)slice / Slices,
                                              (double)stack / Stacks));
                    }
                }

                //Right
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    double phi = Math.PI / 2 - stack * Math.PI / Stacks;
                    double y = Radius * Math.Sin(phi);
                    double scale = -Radius * Math.Cos(phi);

                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        double theta = slice * 2 * Math.PI / Slices;
                        double x = scale * Math.Sin(theta);
                        double z = scale * Math.Cos(theta);

                        Vector3D normal = new Vector3D(x, y, z);
                        textureCoordinates.Add(
                                    new Point((double)slice / Slices,
                                              (double)stack / Stacks));
                    }
                }

                return textureCoordinates;
            }
        }

        public override PointCollection OverUnderTextureCoordinates
        {
            get 
            {
                PointCollection textureCoordinates = new PointCollection();

                //LEFT
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    double phi = Math.PI / 2 - stack * Math.PI / Stacks;
                    double y = Radius * Math.Sin(phi);
                    double scale = -Radius * Math.Cos(phi);

                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        double theta = slice * 2 * Math.PI / Slices;
                        double x = scale * Math.Sin(theta) + Radius;
                        double z = scale * Math.Cos(theta);

                        Vector3D normal = new Vector3D(x, y, z);
                        textureCoordinates.Add(
                                    new Point((double)slice / Slices,
                                              (double)stack / Stacks / 2));
                    }
                }

                //RIGH
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    double phi = Math.PI / 2 - stack * Math.PI / Stacks;
                    double y = Radius * Math.Sin(phi);
                    double scale = -Radius * Math.Cos(phi);

                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        double theta = slice * 2 * Math.PI / Slices;
                        double x = scale * Math.Sin(theta) - Radius;
                        double z = scale * Math.Cos(theta);

                        Vector3D normal = new Vector3D(x, y, z);
                        textureCoordinates.Add(
                                    new Point((double)slice / Slices,
                                              0.5 + (double)stack / Stacks / 2));
                    }
                }

                return textureCoordinates;
            }
        }

        public override PointCollection SideBySideTextureCoordinates
        {
            get
            {
                PointCollection textureCoordinates = new PointCollection();

                //LEFT
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    double phi = Math.PI / 2 - stack * Math.PI / Stacks;
                    double y = Radius * Math.Sin(phi);
                    double scale = -Radius * Math.Cos(phi);

                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        double theta = slice * 2 * Math.PI / Slices;
                        double x = scale * Math.Sin(theta) + Radius;
                        double z = scale * Math.Cos(theta);

                        Vector3D normal = new Vector3D(x, y, z);
                        textureCoordinates.Add(
                                    new Point((double)slice / Slices / 2,
                                              (double)stack / Stacks));
                    }
                }

                //RIGH
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    double phi = Math.PI / 2 - stack * Math.PI / Stacks;
                    double y = Radius * Math.Sin(phi);
                    double scale = -Radius * Math.Cos(phi);

                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        double theta = slice * 2 * Math.PI / Slices;
                        double x = scale * Math.Sin(theta) - Radius;
                        double z = scale * Math.Cos(theta);

                        Vector3D normal = new Vector3D(x, y, z);
                        textureCoordinates.Add(
                                    new Point(0.5 + (double)slice / Slices / 2,
                                              (double)stack / Stacks));
                    }
                }

                return textureCoordinates;
            }
        }
    }
}

