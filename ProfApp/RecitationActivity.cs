using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace ProfApp
{
    [Activity(Label = "RecitationActivity")]
    public class RecitationActivity : Activity
    {
        TextView lblname;
        Button btnrandom;
        string ID;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Recitation);
            UserDialogs.Init(this);
            ID = Intent.GetStringExtra("ID");
            lblname = FindViewById<TextView>(Resource.Id.lblname);
            btnrandom = FindViewById<Button>(Resource.Id.btnrandom);
            // Create your application here

            btnrandom.Click += async delegate
            {
                UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
                await Task.Delay(1000);
                random();
                UserDialogs.Instance.HideLoading();
            };
        }


        public void random()
        {
            try
            {
                using (var client = new WebClient())
                {
                    var json = client.DownloadString("http://joremtongwebsite.000webhostapp.com/randomRecitation.php?sectionID=" + ID);
                    dynamic jsonData = JsonConvert.DeserializeObject(json);
                    string username = jsonData[0].username;
                    lblname.Text = jsonData[0].fullName;

                }
            }
            catch (Exception i)
            {
                Toast.MakeText(this, i.Message, ToastLength.Long).Show();
            }
        }
    }
}