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
    [Activity(Label = "RegistrationActivity")]
    public class RegistrationActivity : Activity
    {
        EditText txtname, txtnumber, txtemail, txtusername, txtpassword;
        Button btnregister;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Registration);
            // Create your application here

            txtname = FindViewById<EditText>(Resource.Id.txtname);
            txtnumber = FindViewById<EditText>(Resource.Id.txtnumber);
            txtemail = FindViewById<EditText>(Resource.Id.txtemail);
            txtusername = FindViewById<EditText>(Resource.Id.txtusername);
            txtpassword = FindViewById<EditText>(Resource.Id.txtpassword);
            btnregister = FindViewById<Button>(Resource.Id.btnregister);

            btnregister.Click += async delegate
             {


                 UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
                 await Task.Delay(1000);
                 try
                 {
                     if (txtpassword.Text != "" || txtpassword.Text != "" || txtnumber.Text != "" || txtname.Text != "")
                     {

                         string password = Library.EasyMD5.Hash(txtpassword.Text);
                         using (var client = new WebClient())
                         {
                             var values = new NameValueCollection();
                             values["fullname"] = txtname.Text;
                             values["email"] = txtemail.Text;
                             values["contact"] = txtnumber.Text;
                             values["username"] = txtusername.Text;
                             values["password"] = password;
                             var response = client.UploadValues("http://joremtongwebsite.000webhostapp.com/profRegistration.php", values);
                             var responseString = Encoding.Default.GetString(response);

                         }
                         Toast.MakeText(this, "Save", ToastLength.Long).Show();
                         Intent activity = new Intent(this, typeof(MainActivity));
                         StartActivity(activity);
                     }
                     else
                     {
                         AlertDialog.Builder alert = new AlertDialog.Builder(this);
                         alert.SetTitle("Alert");
                         alert.SetMessage("Fill-up All Form");
                         alert.SetPositiveButton("OK", (senderAlert, args) => {
                             // write your own set of instructions
                         });
                         RunOnUiThread(() => {
                             alert.Show();
                         });

                         
                     }
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