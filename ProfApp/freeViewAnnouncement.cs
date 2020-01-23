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
    [Activity(Label = "freeViewAnnouncement")]
    public class freeViewAnnouncement : Activity
    {
        LinearLayout linearLayout1;
        string ID;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.freeViewAnnouncement);
            UserDialogs.Init(this);
            ID = Intent.GetStringExtra("ID");
            linearLayout1 = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
            list();            // Create your application here
        }
        public void list()
        {

            try
            {
                linearLayout1.RemoveAllViews();


                using (var client = new WebClient())
                {
                    var json = client.DownloadString("http://joremtongwebsite.000webhostapp.com/announcementList.php?sectionID=" + ID);
                    dynamic jsonData = JsonConvert.DeserializeObject(json);
                    foreach (dynamic jsonDatas in jsonData)
                    {
                        var layout = new LinearLayout(this);
                        layout.Orientation = Orientation.Vertical;
                        var layoutParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                        layoutParams.SetMargins(0, 0, 0, 20);
                        layout.SetBackgroundResource(Resource.Drawable.layout_bg);
                        layout.SetPadding(0, 0, 0, 40);
                        layout.LayoutParameters = layoutParams;


                        var username = new TextView(this);
                        username.SetTextColor(Color.White);
                        username.SetTypeface(Typeface.Default, TypefaceStyle.BoldItalic);
                        username.SetTextSize(global::Android.Util.ComplexUnitType.Dip, 20);
                        username.Text = jsonDatas["title"].ToString();
                        username.SetPadding(20, 20, 20, 0);

                        var timedate = new TextView(this);
                        timedate.SetTextColor(Color.LightGray);
                        timedate.SetTypeface(Typeface.Default, TypefaceStyle.Normal);
                        timedate.SetTextSize(global::Android.Util.ComplexUnitType.Dip, 8);
                        timedate.Text = jsonDatas["createdOn"].ToString();
                        timedate.SetPadding(20, 0, 20, 20);


                        var content = new TextView(this);
                        content.SetTextColor(Color.DimGray);
                        content.SetTextSize(global::Android.Util.ComplexUnitType.Dip, 15);
                        content.Text = jsonDatas["content"].ToString();
                        content.SetPadding(20, 0, 20, 0);

                        layout.AddView(username);
                        layout.AddView(timedate);
                        layout.AddView(content);
                        linearLayout1.AddView(layout);
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