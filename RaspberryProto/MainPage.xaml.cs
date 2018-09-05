using Newtonsoft.Json;
using RaspberryProto.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace RaspberryProto
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
    {
		private const string url = "http://devicecontrolapi.azurewebsites.net/Device/";
		private bool red_state = false;
		private bool green_state = false;
		private bool yellow_state = false;
		private DispatcherTimer timer;
		private const int RED = 5;
		private const int GREEN = 6;
		private const int YELLOW = 13;
		private GpioPin redPin;
		private GpioPin greenPin;
		private GpioPin yellowPin;

		public MainPage()
        {
            this.InitializeComponent();
			InitializeTimer();
			//InitGpio();
		}

		private void InitializeTimer()
		{
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += GetHostRequest;
			timer.Start();
		}

		private async void GetHostRequest(object sender, object e)
		{
			TimeLabel.Text = DateTime.Now.ToString();

			try
			{

				HttpClient client = new HttpClient();

				HttpResponseMessage response = await client.GetAsync(url);
				if (response.IsSuccessStatusCode)
				{
					string stringstatus = await response.Content.ReadAsStringAsync();
					var test = JsonConvert.DeserializeObject<List<Devices>>(stringstatus);

					if (stringstatus == "true")
					{
						GreenStatusLabel.Text = "Status:ON";
					}

					else
					{
						GreenStatusLabel.Text = "Status:Off";
					}

				}

			}

			catch(Exception ex)
			{
				
			}


		}

		private void InitGpio()
		{
			var gpio = GpioController.GetDefault();
			redPin = gpio.OpenPin(RED);
			greenPin = gpio.OpenPin(GREEN);
			yellowPin = gpio.OpenPin(YELLOW);
			redPin.Write(GpioPinValue.Low);
			greenPin.Write(GpioPinValue.Low);
			yellowPin.Write(GpioPinValue.Low);
			redPin.SetDriveMode(GpioPinDriveMode.Output);
			greenPin.SetDriveMode(GpioPinDriveMode.Output);
			yellowPin.SetDriveMode(GpioPinDriveMode.Output);
		}

		private void Green_Click(object sender, RoutedEventArgs e)
		{
			if (green_state == false)
			{
				greenPin.Write(GpioPinValue.High);
				green_state = true;
				GreenStatusLabel.Text = "Status:ON";
			}
			else
			{
				greenPin.Write(GpioPinValue.Low);
				green_state = false;
				GreenStatusLabel.Text = "Status:Off";
			}
		}
	

		private void Red_Click(object sender, RoutedEventArgs e)
		{		
			if (red_state == false)
			{
				redPin.Write(GpioPinValue.High);
				red_state = true;
				RedStatusLabel.Text = "Status:ON";
			}
			else
			{
				redPin.Write(GpioPinValue.Low);
				red_state = false;
				RedStatusLabel.Text = "Status:Off";
			}
		}


		private async void Yellow_Click(object sender, RoutedEventArgs e)
		{
			yellow_state = !yellow_state;

		    while (yellow_state == true)
			{
				yellowPin.Write(GpioPinValue.High);
				await Task.Delay(500);
				yellowPin.Write(GpioPinValue.Low);
				await Task.Delay(500);
				YellowStatusLabel.Text = "Status:ON";
			}
		}

		private void Page_Unloaded(object sender, RoutedEventArgs e)
		{
			redPin.Dispose();
			greenPin.Dispose();
		}


	}
}
