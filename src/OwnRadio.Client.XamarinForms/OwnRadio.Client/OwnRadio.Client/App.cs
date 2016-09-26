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
            DeviceID = CrossSettings.Current.GetValueOrDefault<Guid>("DeviceID", Guid.NewGuid()).ToString();
            MainPage = new MainPage();
            Button buttonPlay = MainPage.FindByName<Button>("ButtonPlay");
            Button buttonNext = MainPage.FindByName<Button>("ButtonNext");
            Label labelDeviceID = MainPage.FindByName<Label>("LabelDeviceID");
            labelDeviceID.Text = "Device ID: " + DeviceID;
            buttonPlay.Clicked += ButtonPlay_Clicked;
            buttonNext.Clicked += ButtonNext_Clicked;
        }

        public void CPTrackPlay()
        {
            if (!PlayerExistFlag)
            {
                try
                {
                    //if (File.Exists(FileName))
                    //{
                    //    File.Delete(FileName);
                    //}

                   GUID = DependencyService.Get<IGetNextTrackID>().GetNextTrackID(DeviceID, out Method);
                   FileName = DependencyService.Get<IGetTrack>().GetTrackByID(GUID);

                   PlayerExistFlag = true;
                }
                catch (Exception ex)
                {
                    PlayerExistFlag = false;
                }
            }
            DependencyService.Get<ITrackPlay>().CurrentTrackPlay(PlayerExistFlag, FileName);
        }

        private void ButtonPlay_Clicked(object sender, EventArgs e)
        {
            CPTrackPlay();
        }

        private void ButtonNext_Clicked(object sender, EventArgs e)
        {
            PlayerExistFlag = false;
            DependencyService.Get<ITrackPlay>().NextTrackPlay();
            CPTrackPlay();
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
