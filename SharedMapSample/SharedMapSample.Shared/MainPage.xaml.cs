using System;
using System.Collections.Generic;
using Windows.Devices.Geolocation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;
using Bing.Maps;

using NuclearPlants;

namespace SharedMapSample
{
    public sealed partial class MainPage : Page
    {
        #region Constructor

        public MainPage()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Button Handlers

        private void GoToLondonBtn_Clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //MyMap.SetView(new BasicGeoposition() { Latitude = 51.5, Longitude = -0.05 }, 11);

            ShowNuclear();
        }

        List<Nuclear> nuclear = null;

        void ShowNuclear()
        {
                 if (nuclear == null)
                {
                    NuclearRepository.LoadFromDatabase();
                    nuclear = NuclearRepository.Nuclear;

                    Debug.WriteLine("Loaded nuclear: " + nuclear.Count);

                    foreach (var item in nuclear)
                    {
                        var loc = new BasicGeoposition() { Latitude = item.Latitude, Longitude = item.Longitude };
                        MyMap.AddPushpin(loc, item.Name);
                    }
                }            
        }

        private void ToggleTrafficBtn_Clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var toggle = sender as AppBarToggleButton;

            MyMap.ShowTraffic = toggle.IsChecked.HasValue && toggle.IsChecked.Value;
        }

        private void MapStyleBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var btn = sender as MenuFlyoutItem;
            MyMap.SetMapType(btn.Tag as string);
        }

        private void AddPushpinsBtn_Clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var locs = GetSamplePoints();

            for (int i = 0; i < locs.Count; i++)
            {
                MyMap.AddPushpin(locs[i], (i + 1).ToString());
            }
        }

        private void AddPolylineBtn_Clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var locs = GetSamplePoints();
            MyMap.AddPolyline(locs, GetRandomColor(), 5);           
        }

        private void AddPolygonBtn_Clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var locs = GetSamplePoints();
            MyMap.AddPolygon(locs, GetRandomColor(), GetRandomColor(), 2);
        }

        private void ClearMapBtn_Clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            MyMap.ClearMap();
        }

        #endregion

        #region Helper Methods

        private List<BasicGeoposition> GetSamplePoints()
        {
            var center = MyMap.Center.Position;

            var rand = new Random();
            center.Latitude += rand.NextDouble() * 0.05 - 0.025;
            center.Longitude += rand.NextDouble() * 0.05 - 0.025;

            var locs = new List<BasicGeoposition>();
            locs.Add(new BasicGeoposition() { Latitude = center.Latitude - 0.05, Longitude = center.Longitude - 0.05 });
            locs.Add(new BasicGeoposition() { Latitude = center.Latitude - 0.05, Longitude = center.Longitude + 0.05 });
            locs.Add(new BasicGeoposition() { Latitude = center.Latitude + 0.05, Longitude = center.Longitude + 0.05 });
            locs.Add(new BasicGeoposition() { Latitude = center.Latitude + 0.05, Longitude = center.Longitude - 0.05 });
            return locs;
        }

        private Color GetRandomColor()
        {
            var rand = new Random();

            byte[] bytes = new byte[3];
            rand.NextBytes(bytes);

            return Color.FromArgb(150, bytes[0], bytes[1], bytes[2]);
        }

        #endregion

        private void AddBoundedImageBtn_Clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var boundedImage = new ScaledImage(MyMap, new Uri("ms-appx:///Assets/topographicMap.gif"),
                new BasicGeoposition() { Latitude = 40.5, Longitude = -123.5 },
                new BasicGeoposition() { Latitude = 40.25, Longitude = -123 });
            
            MyMap.PinLayer.Add(boundedImage);
            MyMap.SetView(new BasicGeoposition() { Latitude = 40.4, Longitude = -123.25 }, 12);
        }

        private void buttonAddImagePin_Click(object sender, RoutedEventArgs e)
        {
            // Create Regular Image Bitmap
            Image img = new Image();
            img.Width = 65;
            img.Height = 65;
            img.Source = new BitmapImage(new Uri("ms-appx:///Images/PinImage.png"));

            // Get the maps current center location (that's where im going to place the image)
            var CurrentMapCenterLocation = MyMap.Center;

            // Add image to map layer
            //defaultMapLayer.Children.Add(img);

            // Set location on map layer
            // Note:    In SetPositionAnchor 65 represents is the image size, anchor requires us to take that 
            //          into account and divide by 2 if we want to use the center point of the image as the anchor

            // Places image on the map layer at a specific lat/long
           // MapLayer.SetPosition(img, CurrentMapCenterLocation);

            // Locks the image with an Anchor pointer in the center of the image to prevent it from moving during map zoom
            //MapLayer.SetPositionAnchor(img, new Point(img.Width / 2, img.Height / 2));
        }
    }
}
