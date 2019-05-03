using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;
using Windows.Graphics.Capture;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace VisualCaptureDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            _device = new CanvasDevice();
        }

        private void MainWebView_Loaded(object sender, RoutedEventArgs e)
        {
            var compositor = Window.Current.Compositor;
            var visual = ElementCompositionPreview.GetElementVisual(MainWebView);

            var size = new SizeInt32()
            {
                Width = (int)MainWebView.ActualWidth,
                Height = (int)MainWebView.ActualHeight
            };

            var item = GraphicsCaptureItem.CreateFromVisual(visual);
            _capture = new SimpleCapture(_device, item, size);
            var surface = _capture.CreateSurface(compositor);

            _visual = compositor.CreateSpriteVisual();
            _visual.RelativeSizeAdjustment = Vector2.One;
            _visual.Brush = compositor.CreateSurfaceBrush(surface);

            ElementCompositionPreview.SetElementChildVisual(VisualGrid, _visual);
            _capture.StartCapture();
        }

        private CanvasDevice _device;
        private SimpleCapture _capture;
        private SpriteVisual _visual;
    }
}
