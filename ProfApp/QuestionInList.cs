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
    [Activity(Label = "QuestionInList")]
    public class QuestionInList : Activity
    {
        EditText txtchoices1, txtchoices2, txtchoices3, txtchoices4, txtcorrect, txtquestion;
        Button btnaddquestion;
        TextView lblview;
        string quizID;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.QuestionInList);
            UserDialogs.Init(this);
            quizID = Library.ControlID.quizID;
            txtchoices1 = FindViewById<EditText>(Resource.Id.txtchoices1);
            txtchoices2 = FindViewById<EditText>(Resource.Id.txtchoices2);
            txtchoices3 = FindViewById<EditText>(Resource.Id.txtchoices3);
            txtchoices4 = FindViewById<EditText>(Resource.Id.txtchoices4);
            txtquestion = FindViewById<EditText>(Resource.Id.txtquestion);
            txtcorrect = FindViewById<EditText>(Resource.Id.txtcorrect);
            btnaddquestion = FindViewById<Button>(Resource.Id.btnaddquestion);
            lblview = FindViewById<TextView>(Resource.Id.lblview);
            lblview.Click += async delegate
            {
                UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
                await Task.Delay(1000);
                Intent activity = new Intent(this, typeof(QuizFreeViewActivity));
                StartActivity(activity);
                UserDialogs.Instance.HideLoading();

            };

            btnaddquestion.Click += delegate
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        var values = new NameValueCollection();
                        values["question"] = txtquestion.Text;
                        values["choiceone"] = txtchoices1.Text;
                        values["choicetwo"] = txtchoices2.Text;
                        values["choicethree"] = txtchoices3.Text;
                        values["choicefour"] = txtchoices4.Text;
                        values["correctanswer"] = txtcorrect.Text;
                        values["quizID"] = quizID;
                        var response = client.UploadValues("http://joremtongwebsite.000webhostapp.com/questionList.php", values);
                        var responseString = Encoding.Default.GetString(response);
                        txtquestion.Text = txtchoices1.Text = txtchoices2.Text = txtchoices3.Text = txtchoices4.Text = txtcorrect.Text = string.Empty;
                    }
                    Toast.MakeText(this, "Save", ToastLength.Long).Show();
                  
                }
                catch (Exception i)
                {
                    Toast.MakeText(this, i.Message, ToastLength.Long).Show();
                }
            };

        }
    }
}