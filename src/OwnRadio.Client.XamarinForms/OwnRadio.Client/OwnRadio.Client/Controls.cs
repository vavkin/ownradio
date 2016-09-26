using System;

using Xamarin.Forms;

namespace OwnRadio.Client
{
	public class Controls : ContentPage
	{
		public Controls(Guid DeviceID)
		{
            Label LabelDeviceID = new Label
            {
                Text = "Device ID: " + DeviceID,
            };
			Button ButtonPlay = new Button
			{
				Text = "Play/Pause"
                //VerticalOptions = LayoutOptions.CenterAndExpand
            };
			ButtonPlay.Clicked += OnButtonPlayClicked;

			Button ButtonNext = new Button
			{
				Text = "Next"
			};
            ButtonNext.Clicked += OnButtonNextClicked;

			this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

            this.Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    LabelDeviceID,
					ButtonPlay,
                    ButtonNext
                }
			};
		}

		void OnButtonPlayClicked(object sender, EventArgs e)
		{
            DependencyService.Get<ITrackPlay>().CurrentTrackPlay();
        }

        void OnButtonNextClicked(object sender, EventArgs e)
		{
			
		}
	}
}

