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

namespace ProfApp
{
    [Activity(Label = "AddSectionActivity")]
    public class AddSectionActivity : Activity
    {
        EditText txtsubject, txtcourse, txtsection;
        Button btnaddsection;
        TextView lblview;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddingSections);
            UserDialogs.Init(this);
            // Create your application here
            txtsubject = FindViewById<EditText>(Resource.Id.txtsubject);
            txtcourse = FindViewById<EditText>(Resource.Id.txtcourse);
            txtsection = FindViewById<EditText>(Resource.Id.txtsection);
            btnaddsection = FindViewById<Button>(Resource.Id.btnaddsection);
            lblview = FindViewById<TextView>(Resource.Id.lblview);
            btnaddsection.Click += async delegate
            {

                UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
                await Task.Delay(1000);
                addsection();
                UserDialogs.Instance.HideLoading();
            };
            lblview.Click += async delegate
            {
                UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
                await Task.Delay(1000);
                Intent activity = new Intent(this, typeof(sectionListActivity));
                StartActivity(activity);
                UserDialogs.Instance.HideLoading();
            };
        }

        public void addsection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["profuserID"] = Library.ControlID.userID;
                    values["subject"] = txtsubject.Text;
                    values["course"] = txtcourse.Text;
                    values["section"] = txtsection.Text;

                    var response = client.UploadValues("http://joremtongwebsite.000webhostapp.com/addsection.php", values);
                    var responseString = Encoding.Default.GetString(response);
                    txtsection.Text = txtcourse.Text = txtsubject.Text = string.Empty;
                }
              
            }
            catch (Exception i)
            {
                Toast.MakeText(this, i.Message, ToastLength.Long).Show();
            }
        }
    }
}