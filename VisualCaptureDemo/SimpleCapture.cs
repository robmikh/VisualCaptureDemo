using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Composition;
using System;
using Windows.Graphics;
using Windows.Graphics.Capture;
using Windows.Graphics.DirectX;
using Windows.UI;
using Windows.UI.Composition;

namespace VisualCaptureDemo
{
    class SimpleCapture : IDisposable
    {
        public SimpleCapture(CanvasDevice device, GraphicsCaptureItem item, SizeInt32 size)
        {
            _item = item;
            _device = device;

            // TODO: Dpi?
            _swapChain = new CanvasSwapChain(_device, size.Width, size.Height, 96);

            _framePool = Direct3D11CaptureFramePool.CreateFreeThreaded(
                    _device,
                    DirectXPixelFormat.B8G8R8A8UIntNormalized,
                    2,
                    size);
            _session = _framePool.CreateCaptureSession(item);

            _framePool.FrameArrived += OnFrameArrived;
        }

        public void StartCapture()
        {
            _session.StartCapture();
        }

        public ICompositionSurface CreateSurface(Compositor compositor)
        {
            return CanvasComposition.CreateCompositionSurfaceForSwapChain(compositor, _swapChain);
        }

        public void Dispose()
        {
            _session?.Dispose();
            _framePool?.Dispose();
            _swapChain?.Dispose();

            _swapChain = null;
            _framePool = null;
            _session = null;
            _item = null;
        }

        private void OnFrameArrived(Direct3D11CaptureFramePool sender, object args)
        {
            using (var frame = sender.TryGetNextFrame())
            {
                using (var bitmap = CanvasBitmap.CreateFromDirect3D11Surface(_device, frame.Surface))
                using (var drawingSession = _swapChain.CreateDrawingSession(Colors.Transparent))
                {
                    drawingSession.DrawImage(bitmap);
                }

            } // retire the frame

            _swapChain.Present();
        }

        private GraphicsCaptureItem _item;
        private Direct3D11CaptureFramePool _framePool;
        private GraphicsCaptureSession _session;

        private CanvasDevice _device;
        private CanvasSwapChain _swapChain;
    }
}
