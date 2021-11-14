using System;
using System.Collections.Generic;
using Xamarin.Forms.GoogleMaps;

namespace Encountify.Models
{
    public class CustomMap : Map
    {
        public event EventHandler<CustomPin> RendererNeedToRefreshWindow;
        public List<CustomPin> CustomPins { get; set; }

        public void RefreshWindowForPin(CustomPin pin)
        {
            if (RendererNeedToRefreshWindow != null)
            {
                RendererNeedToRefreshWindow(this, pin);
            }
        }
    }
}
