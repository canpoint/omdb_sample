using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace omdb_to_list
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public class DataList
        {
            public string poster { get; set; }
        }


        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
			// check if online
			if(getIsInternetAccessAvailable)
				Loaded += new RoutedEventHandler(Page_Loaded);
			else {
				// from cache
				Loaded += new RoutedEventHandler(Cache_Loaded);
			}
			
        }
		
		void Cache_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < obj.Count; i++)
            {
				Windows.Storage.ApplicationDataContainerlocalSettings = Windows.Storage.ApplicationData.Current.LocalSettings;  
					var item = new DataList();
					item.poster = row["poster"].ToString();
					if(item.poster != null) 
						list.Items.Add(item); 
			}
        }


        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            download_data dwl = new download_data();

            dwl.downloadDatacomplete += data_arrived;
			// for poster api key is required
            dwl.get_data("http://www.omdbapi.com/?t=&y=2016&plot=short&apiKey=xxx&r=json");
        }


        void data_arrived(object sender, DownloadCompleteData e)
        {

            String data = e.data;

            JArray obj = JArray.Parse(data);
			JObject row = JObject.Parse(obj[i].ToString());

            for (int i = 0; i < localSettings.Values["poster"].Count; i++)
            {
				Windows.Storage.ApplicationDataContainerlocalSettings = Windows.Storage.ApplicationData.Current.LocalSettings;  
  
				if ((string) localSettings.Values["IsFirstTimeLaunched"] == "true")  
				{  
					var item = new DataList();
					item.poster = localSettings.Values["poster"][i].ToString();
					if(item.poster != null) 
						list.Items.Add(item); 
				} else  
				{  
					

					var item = new DataList();
					item.poster = row["poster"].ToString();
					if(item.poster != null) {
							list.Items.Add(item);
							localSettings.Values["poster"][i] = item;							
					}   
					// first time flag
					localSettings.Values["IsFirstTimeLaunched"] = "true";    
			  
				}  
                
            }


        }
		
		public static bool getIsInternetAccessAvailable()
		{
			switch(NetworkInformation.GetInternetConnectionProfile().GetNetworkConnectivityLevel())
			{
				case NetworkConnectivityLevel.InternetAccess:
					return true;
				default:
					return false;
			}
		}

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }
    }
}
