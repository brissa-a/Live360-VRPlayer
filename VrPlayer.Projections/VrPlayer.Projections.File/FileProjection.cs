using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using VrPlayer.Contracts.Projections;

namespace VrPlayer.Projections.File
{
    [Export(typeof(IProjection))]
    public class FileProjection : ProjectionBase, IProjection
    {
        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                OnPropertyChanged("FilePath");
                OnPropertyChanged("Geometry");
            }
        }

        public override MeshGeometry3D Geometry 
        {
            get
            {
                var geometry = ReadGeometryFromFile(_filePath);
                //Todo: Modify texture coordinate based on 3d format
                return geometry;
            }
        }

        private MeshGeometry3D ReadGeometryFromFile(string path)
        {
            var geometry = new MeshGeometry3D();
            var models = new Model3DGroup();
            
            var fileInfo = new FileInfo(path);

            if (fileInfo.Extension == ".obj")
            {
                var reader = new ObjReader();
                try
                {
                    models = reader.Read(fileInfo.FullName);
                }
                catch (Exception)
                {
                }
            }

            if (fileInfo.Extension == ".3ds")
            {
                var reader = new StudioReader();
                try
                {
                    models = reader.Read(fileInfo.FullName);
                }
                catch (Exception)
                {
                }
            }

            if (models.Children.Count > 0)
            {
                var model = models.Children[0] as GeometryModel3D;
                if (model != null) geometry = model.Geometry as MeshGeometry3D;
            }

            return geometry;
        }

        public new Vector3D CameraLeftPosition
        {
            get { return new Vector3D(0, 0, 0); }
        }

        public new Vector3D CameraRightPosition
        {
            get { return new Vector3D(0, 0, 0); }
        }

        public override Int32Collection TriangleIndices
        {
            get { throw new System.NotImplementedException(); }
        }

        public override PointCollection MonoTextureCoordinates
        {
            get { throw new NotImplementedException(); }
        }

        public override PointCollection OverUnderTextureCoordinates
        {
            get { throw new NotImplementedException(); }
        }

        public override PointCollection SideBySideTextureCoordinates
        {
            get { throw new NotImplementedException(); }
        }

        public override Point3DCollection Positions
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}