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
    [Activity(Label = "AddAnnouncementActivity")]
    public class AddAnnouncementActivity : Activity
    {

        EditText txttitle, txtcontent;
        Button btnAnnounce;
        TextView lblview;
        string ID;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddAnnouncement);
            ID = Intent.GetStringExtra("ID");
            txttitle = FindViewById<EditText>(Resource.Id.txttitle);
            txtcontent = FindViewById<EditText>(Resource.Id.txtcontent);
            btnAnnounce = FindViewById<Button>(Resource.Id.btnAnnounce);
            lblview = FindViewById<TextView>(Resource.Id.lblview);
            lblview.Click += async delegate
            {
                UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
                await Task.Delay(1000);
                Intent activity = new Intent(this, typeof(freeViewAnnouncement));
                activity.PutExtra("ID", ID);
                StartActivity(activity);
                UserDialogs.Instance.HideLoading();
            };
            btnAnnounce.Click += async delegate
            {
                UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
                await Task.Delay(1000);
                save();
                UserDialogs.Instance.HideLoading();
            };
            // Create your application here
        }

        public void save()
        {
            try
            {
                using (var client = new WebClient())
                {
                    //string userID = Library.ControlID.userID;
                    var values = new NameValueCollection();
                    values["title"] = txttitle.Text;
                    values["content"] = txtcontent.Text;
                    values["sectionID"] = ID;
                    values["profID"] = Library.ControlID.userID;

                    var response = client.UploadValues("http://joremtongwebsite.000webhostapp.com/addAnnouncement.php", values);
                    var responseString = Encoding.Default.GetString(response);

                }
                Toast.MakeText(this, "Save", ToastLength.Long).Show();
               txttitle.Text = txtcontent.Text = string.Empty;
            }
            catch (Exception i)
            {
                Toast.MakeText(this, i.Message, ToastLength.Long).Show();
            }
        }




    }
}