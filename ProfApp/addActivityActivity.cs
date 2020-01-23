using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;


namespace ProfApp
{
    [Activity(Label = "addActivityActivity")]
    public class addActivityActivity : Activity
    {
        Button btnsavehandouts;
        EditText txtpoints, txtactivity;
        string userID;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddActivity);
            userID = Intent.GetStringExtra("userID");
            txtpoints = FindViewById<EditText>(Resource.Id.txtpoints);
            txtactivity = FindViewById<EditText>(Resource.Id.txtactivity);
            btnsavehandouts = FindViewById<Button>(Resource.Id.btnsavehandouts);
            btnsavehandouts.Click += async delegate
            {
                UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
                await Task.Delay(1000);
                try
                {
                    using (var client = new WebClient())
                    {
                        var values = new NameValueCollection();
                        values["userID"] = userID;
                        values["profID"] = Library.ControlID.userID;
                        values["activity"] = txtactivity.Text;
                        values["score"] = txtpoints.Text;

                        var response = client.UploadValues("http://joremtongwebsite.000webhostapp.com/addactivity.php", values);
                        var responseString = Encoding.Default.GetString(response);

                    }
                    Toast.MakeText(this, "Save", ToastLength.Long).Show();
                    Intent activity = new Intent(this, typeof(studentProfileActivity));
                    activity.PutExtra("userID", userID);
                    StartActivity(activity);
                }
                catch (Exception i)
                {
                    Toast.MakeText(this, i.Message, ToastLength.Long).Show();
                }
                UserDialogs.Instance.HideLoading();
            };
        }
    }
}