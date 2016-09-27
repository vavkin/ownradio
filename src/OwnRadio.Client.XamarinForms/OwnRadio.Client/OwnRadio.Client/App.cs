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
        bool PlayerExistFlag = false; // выбрана ли песня
        String FileName;
        String GUID = "-1";//for the 1st request
        String DeviceID;
        String Method = "новых"; 

        public App()
        {
            //CrossSettings.Current.Remove("DeviceID");
            //Guid guidID = Guid.NewGuid();
            //CrossSettings.Current.AddOrUpdateValue("DeviceID", guidID);
            //DeviceID = CrossSettings.Current.GetValueOrDefault<Guid>("DeviceID").ToString();

            DeviceID = CrossSettings.Current.GetValueOrDefault<Guid>("DeviceID", Guid.NewGuid()).ToString();
            MainPage = new MainPage();
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
