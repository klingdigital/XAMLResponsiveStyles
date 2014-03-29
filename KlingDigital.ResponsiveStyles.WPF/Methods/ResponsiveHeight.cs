﻿/*
XAMLResponsiveStyles created by James H. at http://www.klingdigital.net
Source code from http://github.com/klingdigital/XAMLResponsiveStyles
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KlingDigital.ResponsiveStyles.WPF.Methods
{
    public class ResponsiveHeight : DependencyObject, IResponsiveMethod
    {
        public ResponsiveHeight()
        {
        }

        eResponsiveState state = eResponsiveState.None;
        
        public double CompactHeight
        {
            get { return (double)GetValue(CompactHeightProperty); }
            set { SetValue(CompactHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CompactWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CompactHeightProperty =
            DependencyProperty.Register("CompactHeight", typeof(double), typeof(ResponsiveHeight), new PropertyMetadata(null));

        
        public ResourceDictionaryCollection CompactStyles
        {
            get { return (ResourceDictionaryCollection)GetValue(CompactStylesProperty); }
            set { SetValue(CompactStylesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CompactStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CompactStylesProperty =
            DependencyProperty.Register("CompactStyles", typeof(ResourceDictionaryCollection), typeof(ResponsiveHeight), new PropertyMetadata(null));


        public ResourceDictionaryCollection RegularStyles
        {
            get { return (ResourceDictionaryCollection)GetValue(RegularStylesProperty); }
            set { SetValue(RegularStylesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RegularStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RegularStylesProperty =
            DependencyProperty.Register("RegularStyles", typeof(ResourceDictionaryCollection), typeof(ResponsiveHeight), new PropertyMetadata(null));



        public void HandleChange(Size size, IList<ResourceDictionary> resources)
        {
            determineStyle(size.Height, resources);
        }

        void determineStyle(double newHeight, IList<ResourceDictionary> resources)
        {
            //DEFAULT STYLE

            //apply default regular styles
            if (state == eResponsiveState.None)
            {
                state = eResponsiveState.Regular;

                //clear existing responsive styles if any
                removeExistingStyles(resources);

                //add compact style xaml to resources
                if (this.RegularStyles != null)
                {
                    foreach (var style in this.RegularStyles)
                        resources.Add(style);
                }
            }

            //STYLE SWITCHING

            //switch to compact styles
            if (state == eResponsiveState.Regular && newHeight <= this.CompactHeight)
            {
                state = eResponsiveState.Compact;

                //clear existing responsive styles if any
                removeExistingStyles(resources);

                //add compact style xaml to resources
                if (this.CompactStyles != null)
                {
                    foreach (var style in this.CompactStyles)
                        resources.Add(style);
                }

            }
            //switch to regular styles
            else if (state == eResponsiveState.Compact && newHeight > this.CompactHeight)
            {
                state = eResponsiveState.Regular;

                //clear existing responsive styles if any
                removeExistingStyles(resources);

                //add regular style xaml to resources
                if (this.RegularStyles != null)
                {
                    foreach (var style in this.RegularStyles)
                        resources.Add(style);
                }

            }
        }

        void removeExistingStyles(IList<ResourceDictionary> resources)
        {
            var allStyles = new List<ResourceDictionary>();

            if (this.RegularStyles != null)
                allStyles.AddRange(this.RegularStyles);

            if (this.CompactStyles != null)
                allStyles.AddRange(this.CompactStyles);

            for (int i = 0; i < resources.Count; i++)
            {
                var res = resources[i] as ResourceDictionary;
                if (res == null)
                    continue;

                var match = allStyles.FirstOrDefault(s => s.Source == res.Source);
                if (match != null)
                {
                    resources.Remove(res);
                }
            }

        }

    }
}
