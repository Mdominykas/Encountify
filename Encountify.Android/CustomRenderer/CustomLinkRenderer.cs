using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using Encountify.CustomRenderer;
using Encountify.Droid.CustomRenderer;


[assembly: ExportRenderer(typeof(CustomLink), typeof(CustomLinkRenderer))]
namespace Encountify.Droid.CustomRenderer
{
    class CustomLinkRenderer : LabelRenderer
    {

        public CustomLinkRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {

            }
        }
    }
}