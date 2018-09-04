using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace RaspberryProto
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
		private bool red_state = false;
		private bool green_state = false;
		private const int RED = 5;
		private const int GREEN = 18;
		private GpioPin redPin;
		private GpioPin greenPin;

		public MainPage()
        {
            this.InitializeComponent();
			InitGpio();
		}

		private void InitGpio()
		{
			var gpio = GpioController.GetDefault();
			redPin = gpio.OpenPin(RED);
			greenPin = gpio.OpenPin(GREEN);
			redPin.Write(GpioPinValue.Low);
			greenPin.Write(GpioPinValue.Low);
			redPin.SetDriveMode(GpioPinDriveMode.Output);
			greenPin.SetDriveMode(GpioPinDriveMode.Output);
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

		private void Page_Unloaded(object sender, RoutedEventArgs e)
		{
			redPin.Dispose();
			greenPin.Dispose();
		}
	}
}
