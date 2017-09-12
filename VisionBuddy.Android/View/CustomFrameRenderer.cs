using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using VisionBuddy.Droid.View;

[assembly: ExportRenderer(typeof(CustomFrame), typeof(CustomFrameRenderer))]
namespace VisionBuddy.Droid.View
{
    class CustomFrameRenderer : VisualElementRenderer<Frame>
    {
        public CustomFrameRenderer()
        {
            SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.black_rect));
        }
    }
}