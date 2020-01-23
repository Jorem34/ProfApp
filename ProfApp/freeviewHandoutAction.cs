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
    [Activity(Label = "freeviewHandoutAction")]
    
    public class freeviewHandoutAction : Activity
    {
        LinearLayout linearLayout1;
        string ID;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.freeviewOfHandouts);
            UserDialogs.Init(this);
            ID = Intent.GetStringExtra("ID");
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
                    var json = client.DownloadString("http://joremtongwebsite.000webhostapp.com/handoutList.php?sectionID=" + ID);
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
                        Image.SetPadding(30, 40, 0, 0);
                        Image.SetImageResource(Resource.Drawable.pdffile);

                        var layout = new LinearLayout(this);
                        layout.Orientation = Orientation.Vertical;
                        var layoutParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                        layoutParams.SetMargins(0, 0, 0, 20);
                        layout.SetBackgroundResource(Resource.Drawable.layout_bg);
                        layout.SetPadding(0, 0, 0, 0);
                        layout.LayoutParameters = layoutParams;
                        layout.Click += delegate
                        {
                            string url = "http://joremtongwebsite.000webhostapp.com/upload/" + jsonDatas["filename"].ToString();
                            if (!url.Contains("http://"))
                            {
                                string address = url;
                                url = String.Format("http://(0)", address);
                            }
                            var uri = Android.Net.Uri.Parse(url);
                            Intent intent = new Intent(Intent.ActionView, uri);
                            StartActivity(intent);
                        };
                   
                        var username = new TextView(this);
                        username.SetTextColor(Color.White);
                        username.SetTypeface(Typeface.Default, TypefaceStyle.Bold);
                        username.SetTextSize(global::Android.Util.ComplexUnitType.Dip, 20);
                        username.Text = jsonDatas["title"].ToString();
                        username.SetPadding(20, 20, 20, 0);
                        
                        var Title = new TextView(this);
                        Title.SetTextColor(Color.White);
                        Title.SetTypeface(Typeface.Default, TypefaceStyle.Normal);
                        Title.SetTextSize(global::Android.Util.ComplexUnitType.Dip, 15);
                        Title.Text = jsonDatas["subject"].ToString();
                        Title.SetPadding(20, 0, 20, 8);

                        layout.AddView(username);
                        layout.AddView(Title);
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