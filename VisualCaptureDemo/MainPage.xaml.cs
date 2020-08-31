using Microsoft.Graphics.Canvas;
using System.Numerics;
using Windows.Graphics;
using Windows.Graphics.Capture;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace VisualCaptureDemo
{
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
