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
    [Activity(Label = "studentSectionAction")]
    public class studentSectionAction : Activity
    {

        Button btnscan;
        string ID;
        TextView lblsection, lblcourse, lblsubject;
        LinearLayout linearLayout1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.studentSections);
            btnscan = FindViewById<Button>(Resource.Id.btnscan);
            lblsection = FindViewById<TextView>(Resource.Id.lblsection);
            lblcourse = FindViewById<TextView>(Resource.Id.lblcourse);
            lblsubject = FindViewById<TextView>(Resource.Id.lblsubject);
            linearLayout1 = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
            ID = Intent.GetStringExtra("ID");
            info();
            listOfStudent();
            btnscan.Click += async delegate
            {
                #if __ANDROID__
                ZXing.Mobile.MobileBarcodeScanner.Initialize(this.Application);
                #endif

                var scanner = new ZXing.Mobile.MobileBarcodeScanner();

                var result = await scanner.Scan();

                if (result != null)
                {
                    Toast.MakeText(this, "Student Added From the Section", ToastLength.Long).Show();
                    try
                    {
                        using (var client = new WebClient())
                        {
                            var values = new NameValueCollection();
                            values["sectionID"] = ID;
                            values["userID"] = result.Text;
                            
                            var response = client.UploadValues("http://joremtongwebsite.000webhostapp.com/addstudentSection.php", values);
                            var responseString = Encoding.Default.GetString(response);
                            listOfStudent();
                        }
                    }
                    catch (Exception i)
                    {
                        Toast.MakeText(this, i.Message, ToastLength.Long).Show();
                    }
                }
                    
            };
        }

        public void info()
        {
            try
            {
                using (var client = new WebClient())
                {
                    var json = client.DownloadString("http://joremtongwebsite.000webhostapp.com/studentSectionId.php?sectionID=" + ID);
                    dynamic jsonData = JsonConvert.DeserializeObject(json);
                    lblsection.Text = jsonData[0].section;
                    lblcourse.Text = "COURSE: " + jsonData[0].course;
                    lblsubject.Text = "SUBJECT: " + jsonData[0].subject;

                }
            }
            catch (Exception i)
            {
                Toast.MakeText(this, i.Message, ToastLength.Long).Show();
            }
        }

        public void listOfStudent()
        {
            linearLayout1.RemoveAllViews();

            try
            {
                using (var client = new WebClient())
                {
                    string id = Library.ControlID.userID;
                    var json = client.DownloadString("http://joremtongwebsite.000webhostapp.com/studentList.php?sectionID=" + ID);
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
                        layoutParams.SetMargins(0, 30, 0, 20);
                        layout.SetBackgroundResource(Resource.Drawable.layout_bg);
                        layout.SetPadding(10, 10, 10, 50);
                        layout.LayoutParameters = layoutParams;
                        layout.Click += async delegate
                        {
                            UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
                            await Task.Delay(1000);
                            Intent activity = new Intent(this, typeof(studentProfileActivity));
                            activity.PutExtra("userID", jsonDatas["userID"].ToString());
                            StartActivity(activity);
                            UserDialogs.Instance.HideLoading();
                        };

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
                username.Text = "NO DATA YET";
                username.SetPadding(20, 20, 20, 0);

                layout.AddView(username);
                layout2.AddView(Image);
                layout2.AddView(layout);
                linearLayout1.AddView(layout2);
            }
        }


    }
}