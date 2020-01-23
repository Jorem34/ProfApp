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
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using static Android.App.ActionBar;

namespace ProfApp
{
    [Activity(Label = "QRActivity")]
    public class QRActivity : Activity
    {
        Button btnscan;
        string ID;
        LinearLayout linearLayout1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.QRScannerAttendance);
            // Create your application here
            linearLayout1 = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
            btnscan = FindViewById<Button>(Resource.Id.btnscan);
            ID = Intent.GetStringExtra("ID");
            listOfStudent();
            btnscan.Click += async delegate
            {
                #if __ANDROID__
                // Initialize the scanner first so it can track the current context
                ZXing.Mobile.MobileBarcodeScanner.Initialize(this.Application);
                #endif

                var scanner = new ZXing.Mobile.MobileBarcodeScanner();

                var result = await scanner.Scan();

                if (result != null)
                {
                    try
                    {
                        using (var client = new WebClient())
                        {
                            var values = new NameValueCollection();
                            values["userID"] = result.Text;
                            values["sectionID"] = ID;

                            var response = client.UploadValues("http://joremtongwebsite.000webhostapp.com/attendance.php", values);
                            var responseString = Encoding.Default.GetString(response);

                        }
                        Toast.MakeText(this, "Save", ToastLength.Long).Show();
                      
                    }
                    catch (Exception i)
                    {
                        Toast.MakeText(this, i.Message, ToastLength.Long).Show();
                    }
                    Toast.MakeText(this, "Save Attendance", ToastLength.Long).Show();
                }
                listOfStudent();

            };
        }

        public void listOfStudent()
        {
            linearLayout1.RemoveAllViews();

            try
            {
                using (var client = new WebClient())
                {
                    DateTime date = DateTime.Now;
                    string id = Library.ControlID.userID;
                    var json = client.DownloadString("http://joremtongwebsite.000webhostapp.com/attendanceList.php?DATE=" + date.ToString("yyyy-MM-dd") + "&&sectionID=" + ID);
                    dynamic jsonData = JsonConvert.DeserializeObject(json);
                    foreach (dynamic jsonDatas in jsonData)
                    {
                        var layout2 = new LinearLayout(this);
                        layout2.Orientation = Orientation.Horizontal;
                        var layoutParams2 = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                        layoutParams2.SetMargins(0, 0, 0, 20);
                        layout2.SetBackgroundResource(Resource.Drawable.layout_bg);
                        layout2.SetPadding(0, 0, 0, 0);
                        layout2.LayoutParameters = layoutParams2;

                        var Image = new ImageView(this);
                        var param = new LayoutParams(120, 120);
                        Image.LayoutParameters = param;
                        Image.SetPadding(20, 30, 0, 0);
                        Image.SetImageResource(Resource.Drawable.study);

                        var layout = new LinearLayout(this);
                        layout.Orientation = Orientation.Vertical;
                        var layoutParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent); ;
                        layoutParams.SetMargins(0, 0, 0, 20);
                        layout.SetBackgroundResource(Resource.Drawable.layout_bg);
                        layout.SetPadding(10, 10, 10, 50);
                        layout.LayoutParameters = layoutParams;

                        var username = new TextView(this);
                        username.SetTextColor(Color.White);
                        username.SetTypeface(Typeface.Default, TypefaceStyle.BoldItalic);
                        username.SetTextSize(global::Android.Util.ComplexUnitType.Dip, 20);
                        username.Text = jsonDatas["fullName"].ToString();
                        username.SetPadding(20, 20, 20, 0);

                        layout.AddView(username);
                        layout2.AddView(Image);
                        layout2.AddView(layout);
                        linearLayout1.AddView(layout2);
                    }

                }
            }
            catch (Exception i)
            {
                var layout = new LinearLayout(this);
                layout.Orientation = Orientation.Vertical;
                LayoutParams layoutParams = new LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
                layoutParams.SetMargins(5, 5, 5, 5);
                layout.SetBackgroundColor(Color.White);
                layout.SetPadding(0, 0, 0, 0);

                var username = new TextView(this);
                username.SetTextColor(Color.Red);
                username.SetTypeface(Typeface.Default, TypefaceStyle.BoldItalic);
                username.SetTextSize(global::Android.Util.ComplexUnitType.Dip, 20);
                username.Text = "NO DATA YET";
                username.SetPadding(20, 20, 20, 0);

                layout.AddView(username);
                linearLayout1.AddView(layout);
            }
        }
    }
}