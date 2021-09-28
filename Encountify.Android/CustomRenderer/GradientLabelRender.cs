using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Encountify.CustomRenderer;
using Encountify.Droid.Renderers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(GradientLabel), typeof(GradientLabelRenderer))]
namespace Encountify.Droid.Renderers
{
    public class GradientLabelRenderer : LabelRenderer
    {
        public GradientLabelRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            SetColors();
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            SetColors();
        }
        private void SetColors()
        {
            var c1 = (Element as GradientLabel).TextColor1.ToAndroid();
            var c2 = (Element as GradientLabel).TextColor2.ToAndroid();
            Shader myShader = new LinearGradient(
                0, 0, Control.MeasuredWidth, 0,
                c1, c2,
                Shader.TileMode.Clamp);
            Control.Paint.SetShader(myShader);
            Control.Invalidate();
        }
    }
}