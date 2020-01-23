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
    [Activity(Label = "AddHandOutsActivity")]
    public class AddHandOutsActivity : Activity
    {
        Button btnupload, btnsavehandouts;
        EditText txttitle, txtsubject;
        string filepath;
        string ID;
        string fileName;

        TextView lblview;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddHandOuts);
            UserDialogs.Init(this);
            ID = Intent.GetStringExtra("ID");
            btnupload = FindViewById<Button>(Resource.Id.btnupload);
            btnsavehandouts = FindViewById<Button>(Resource.Id.btnsavehandouts);
            txttitle = FindViewById<EditText>(Resource.Id.txttitle);
            lblview = FindViewById<TextView>(Resource.Id.lblview);
            txtsubject = FindViewById<EditText>(Resource.Id.txtsubject);
            lblview.Click += async delegate
            {
                UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
                await Task.Delay(1000);
                Intent activity = new Intent(this, typeof(freeviewHandoutAction));
                activity.PutExtra("ID", ID);
                StartActivity(activity);
                UserDialogs.Instance.HideLoading();
            };
            btnsavehandouts.Click += async delegate
            {
                UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
                await Task.Delay(1000);
                savefile();
                save();
                UserDialogs.Instance.HideLoading();
            };
            btnupload.Click += async delegate
            {
                await OpenFolderDialogAsync();
            };
        }
        public async Task OpenFolderDialogAsync()
        {
            try
            {
                FileData filedata = await CrossFilePicker.Current.PickFile();
                if (filedata == null)
                    return; // user canceled file picking

                filepath = filedata.FilePath;
                fileName = filedata.FileName;
               
                Toast.MakeText(this, filepath, ToastLength.Long).Show();

            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();

            }
        }
        public void savefile()
        {
            try
            {
                // create WebClient object

                WebClient client = new WebClient();
                client.Credentials = CredentialCache.DefaultCredentials;
                client.UploadFile(@"http://joremtongwebsite.000webhostapp.com/upload.php", "POST", filepath);
                client.Dispose();

                Toast.MakeText(this, "Uploaded", ToastLength.Long).Show();
            }
            catch (Exception err)
            {
                Toast.MakeText(this, err.Message, ToastLength.Long).Show();
            }
        }

        public void save()
        {

            try
            {
                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["sectionID"] = ID;
                    values["title"] = txttitle.Text;
                    values["subject"] = txtsubject.Text;
                    values["filename"] = fileName;
                    
                    var response = client.UploadValues("http://joremtongwebsite.000webhostapp.com/addhandout.php", values);
                    var responseString = Encoding.Default.GetString(response);

                }
                Toast.MakeText(this, "Save", ToastLength.Long).Show();
                txttitle.Text = txtsubject.Text = fileName = filepath = string.Empty;
            }
            catch (Exception i)
            {
                Toast.MakeText(this, i.Message, ToastLength.Long).Show();
            }
        }
    }
}