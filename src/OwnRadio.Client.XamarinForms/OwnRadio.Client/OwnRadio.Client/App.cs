using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Plugin.Settings;
using System.IO;

namespace OwnRadio.Client
{
    public class App : Application
    {
        String DeviceID;

        public App()
        {
            MainPage = new MainPage();
            DeviceID = CrossSettings.Current.GetValueOrDefault<Guid>("DeviceID").ToString();
            if (DeviceID == "00000000-0000-0000-0000-000000000000")
            {
                CrossSettings.Current.AddOrUpdateValue("DeviceID", Guid.NewGuid());
                DeviceID = CrossSettings.Current.GetValueOrDefault<Guid>("DeviceID").ToString();
            }

            Button buttonPlay = MainPage.FindByName<Button>("ButtonPlay");
            Button buttonNext = MainPage.FindByName<Button>("ButtonNext");
            Label labelDeviceID = MainPage.FindByName<Label>("LabelDeviceID");
            labelDeviceID.Text = "Device ID: " + DeviceID;
            buttonPlay.Clicked += ButtonPlay_Clicked;
            buttonNext.Clicked += ButtonNext_Clicked;
        }

        private void ButtonPlay_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<ITrackPlay>().CurrentTrackPlay(/*DeviceID, PlayerExistFlag,FileName, Method */);
        }

        private void ButtonNext_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<ITrackPlay>().NextTrackPlay();
            DependencyService.Get<ITrackPlay>().CurrentTrackPlay();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
