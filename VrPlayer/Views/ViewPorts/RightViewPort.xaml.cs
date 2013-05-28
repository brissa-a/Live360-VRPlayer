using System.Windows;
using System.Windows.Media.Media3D;
using VrPlayer.Helpers;
using VrPlayer.ViewModels;

namespace VrPlayer.Views.ViewPorts
{
    public partial class RightViewPort : FullScreenWindow
    {
        private readonly ViewPortViewModel _viewModel;
        public GeometryModel3D Geometry { get; set; }

        public RightViewPort(GeometryModel3D geometry)
        {
            Geometry = geometry;
            InitializeComponent();
            try
            {
                _viewModel = ((App) Application.Current).ViewModelFactory.CreateViewPortViewModel();
                DataContext = _viewModel;
            }
            catch
            {
            }
        }
    }
}
