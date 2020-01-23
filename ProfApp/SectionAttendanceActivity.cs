using System;
using System.Collections.Generic;

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
    [Activity(Label = "SectionAttendanceActivity")]
    public class SectionAttendanceActivity : Activity
    {
        LinearLayout linearLayout1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SectionAttendance);
            UserDialogs.Init(this);
            linearLayout1 = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
            list();
        }
        public void list()
        {
            linearLayout1.RemoveAllViews();
            try
            {
                using (var client = new WebClient())
                {
                    string id = Library.ControlID.userID;
                    var json = client.DownloadString("http://joremtongwebsite.000webhostapp.com/sectionList.php?profuserID=" + id);
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
                        var param = new LayoutParams(150, 150);
                        Image.LayoutParameters = param;
                        Image.SetPadding(30,50,0,0);
                        Image.SetImageResource(Resource.Drawable.exclamation);

                        var layout = new LinearLayout(this);
                        layout.Orientation = Orientation.Vertical;
                        var layoutParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                        layoutParams.SetMargins(0, 0, 0, 20);
                        layout.SetBackgroundResource(Resource.Drawable.layout_bg);
                        layout.SetPadding(0, 0, 0, 0);
                        layout.LayoutParameters = layoutParams;
                        layout.Click += async delegate
                        {
                            UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
                            await Task.Delay(1000);
                            Intent activity = new Intent(this, typeof(QRActivity));
                            activity.PutExtra("ID", jsonDatas["sectionID"].ToString());
                            StartActivity(activity);
                            UserDialogs.Instance.HideLoading();
                        };

                        var username = new TextView(this);
                        username.SetTextColor(Color.White);
                        username.SetTypeface(Typeface.Default, TypefaceStyle.Bold);
                        username.SetTextSize(global::Android.Util.ComplexUnitType.Dip, 20);
                        username.Text = jsonDatas["subject"].ToString();
                        username.SetPadding(20, 20, 20, 0);

                        var Title = new TextView(this);
                        Title.SetTextColor(Color.White);
                        Title.SetTypeface(Typeface.Default, TypefaceStyle.Bold);
                        Title.SetTextSize(global::Android.Util.ComplexUnitType.Dip, 15);
                        Title.Text = jsonDatas["course"].ToString();
                        Title.SetPadding(20, 0, 20, 8);

                        var content = new TextView(this);
                        content.SetTextColor(Color.DimGray);
                        content.SetTextSize(global::Android.Util.ComplexUnitType.Dip, 15);
                        content.Text = jsonDatas["section"].ToString();
                        content.SetPadding(20, 0, 20, 0);

                        layout.AddView(username);
                        layout.AddView(Title);
                        layout.AddView(content);
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