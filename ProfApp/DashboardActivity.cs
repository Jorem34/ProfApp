using System;
using System.Collections.Generic;
using System.Linq;
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
    [Activity(Label = "DashboardActivity")]
    public class DashboardActivity : Activity
    {
        LinearLayout linearLayoutQR, linearLayoutStudents, linearLayoutStudentUploadPdf, linearLayoutAttendance, Randompicker, Quiz;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.dashboard);
            UserDialogs.Init(this);
            linearLayoutQR = FindViewById<LinearLayout>(Resource.Id.linearLayoutQR);
            linearLayoutStudents = FindViewById<LinearLayout>(Resource.Id.linearLayoutStudents);
            linearLayoutAttendance = FindViewById<LinearLayout>(Resource.Id.linearLayoutAttendance);
            linearLayoutStudentUploadPdf = FindViewById<LinearLayout>(Resource.Id.linearLayoutStudentUploadPdf);
            Randompicker = FindViewById<LinearLayout>(Resource.Id.Randompicker);
            Quiz = FindViewById<LinearLayout>(Resource.Id.Quiz);
            linearLayoutQR.Click += async delegate
            {
                UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
                await Task.Delay(1000);
                Intent activity = new Intent(this, typeof(SectionAttendanceActivity));
                StartActivity(activity);
                UserDialogs.Instance.HideLoading();
            };
            linearLayoutStudents.Click += async delegate
            {
                UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
                await Task.Delay(1000);
                Intent activity = new Intent(this, typeof(AddSectionActivity));
                StartActivity(activity);
                UserDialogs.Instance.HideLoading();
            };
            linearLayoutAttendance.Click += async delegate
            {
                UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
                await Task.Delay(1000);
                Intent activity = new Intent(this, typeof(handoutUploaderActivity));
                StartActivity(activity);
                UserDialogs.Instance.HideLoading();
            };
            linearLayoutStudentUploadPdf.Click += async delegate
            {
                UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
                await Task.Delay(1000);
                Intent activity = new Intent(this, typeof(AnnouncementPickSectionActivity));
                StartActivity(activity);
                UserDialogs.Instance.HideLoading();
            };
            Randompicker.Click += async delegate
            {
                UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
                await Task.Delay(1000);
                Intent activity = new Intent(this, typeof(RecitationPickerActivity));
                StartActivity(activity);
                UserDialogs.Instance.HideLoading();
            };
            Quiz.Click += async delegate
            {
                UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
                await Task.Delay(1000);
                Intent activity = new Intent(this, typeof(SelectionForQuiz));
                StartActivity(activity);
                UserDialogs.Instance.HideLoading();
            };
        }
        public override void OnBackPressed()
        {
            return;
        }
    }
}