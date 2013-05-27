﻿using System.Windows;
using System.Windows.Media.Media3D;
using VrPlayer.ViewModels;

namespace VrPlayer.Views.ViewPorts
{
    public partial class LeftViewPort : Window
    {
        private readonly ViewPortViewModel _viewModel;
        public GeometryModel3D Geometry { get; set; }

        public LeftViewPort(GeometryModel3D geometry)
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
