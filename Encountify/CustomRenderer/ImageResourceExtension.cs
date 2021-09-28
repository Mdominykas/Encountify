using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Encountify.CustomRenderer
{
    [ContentProperty(nameof(Source))]

    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }

            var imageSource = ImageSource.FromResource(Source, typeof(ImageResourceExtension).GetTypeInfo().Assembly);

            return imageSource;
        }

    }

}