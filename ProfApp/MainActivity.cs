using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Acr.UserDialogs;
using System.Net;
using Newtonsoft.Json;
using Android.Content;
using System;
using System.Threading.Tasks;

namespace ProfApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.Design.Light.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        EditText txtusername, txtpassword;
        Button btnlogin;
        ImageView imageView1;
        TextView lblregister;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            UserDialogs.Init(this);
            imageView1 = FindViewById<ImageView>(Resource.Id.imageView1);
            txtusername = FindViewById<EditText>(Resource.Id.txtusername);
            txtpassword = FindViewById<EditText>(Resource.Id.txtpassword);
            btnlogin = FindViewById<Button>(Resource.Id.btnlogin);
            lblregister = FindViewById<TextView>(Resource.Id.lblregister);
            btnlogin.Click += async delegate
            {
                // await OpenFolderDialogAsync();

                UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
                await Task.Delay(1000);
                login();
                UserDialogs.Instance.HideLoading();
            };

            lblregister.Click += delegate
            {
                Intent activity = new Intent(this, typeof(RegistrationActivity));
                StartActivity(activity);
            };

        }


        public void login()
        {
            string password = Library.EasyMD5.Hash(txtpassword.Text);
            try
            {
                using (var client = new WebClient())
                {
                    var json = client.DownloadString("http://joremtongwebsite.000webhostapp.com/profLogin.php?username=" + txtusername.Text + "&&password=" + password);
                    dynamic jsonData = JsonConvert.DeserializeObject(json);
                    string username = jsonData[0].username;
                    string passwords = jsonData[0].password;
                    string userID = jsonData[0].profuserID;

                    if (password == passwords && txtusername.Text == username)
                    {
                        Library.ControlID.userID = userID;
                        Intent activity = new Intent(this, typeof(DashboardActivity));
                        StartActivity(activity);
                    }

                }
            }
            catch (Exception i)
            {
                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                alert.SetTitle("Alert");
                alert.SetMessage("Wrong password/username");
                alert.SetPositiveButton("OK", (senderAlert, args) => {
                    // write your own set of instructions
                });
                RunOnUiThread(() => {
                    alert.Show();
                });
            }
        }
    }
}