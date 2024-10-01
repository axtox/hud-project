using Iot.Device.Hcsr04.Esp32;
using Iot.Device.Ssd13xx;
using Iot.Device.Vl53L0X;
using nanoFramework.Hardware.Esp32;
using System;
//using nanoFramework.UI.GraphicDrivers;
using System.Device.I2c;
using System.Diagnostics;
using UnitsNet;

namespace Esp32Test
{
	public class Program
	{
		public static void Main()
		{
			Configuration.SetPinFunction(21, DeviceFunction.I2C1_DATA);
			Configuration.SetPinFunction(22, DeviceFunction.I2C1_CLOCK);

			// Initialize SSD1306 display
			I2cConnectionSettings settings = new I2cConnectionSettings(1, Ssd1306.DefaultI2cAddress);
			I2cDevice i2cDevice = I2cDevice.Create(settings);
			using Ssd1306 ssd1306 = new Ssd1306(i2cDevice, Ssd1306.DisplayResolution.OLED64x128);
			ssd1306.Font = new BasicFont();
			ssd1306.ClearScreen();

			using Hcsr04 ultrasonicSensor = new Hcsr04(16, 17);

			using Vl53L0X vL53L0X = new Vl53L0X(I2cDevice.Create(new I2cConnectionSettings(1, Vl53L0X.DefaultI2cAddress)));

			string previousMeasurementMessage = string.Empty;
			string previousMeasurementMessageLaser = string.Empty;
			while (true)
			{
				if (!ultrasonicSensor.TryGetDistance(out Length distance))
				{
					try
					{
						var currentMeasurement = $"{(int)distance.Value} mm";

						if (previousMeasurementMessage.Length != currentMeasurement.Length)
						{
							ssd1306.ClearDirectAligned(48, 0, 8, 80);
							ssd1306.Display();
						}

						previousMeasurementMessage = currentMeasurement;
						ssd1306.DrawString(0, 8, currentMeasurement, 1);
						ssd1306.Display();
					}
					catch (Exception ex)
					{
						Debug.WriteLine($"Exception: {ex.Message}");
					}
				}

				try
				{
					var currentMeasurement = $"{(int)vL53L0X.Distance} mm";

					if (previousMeasurementMessageLaser.Length != currentMeasurement.Length)
					{
						ssd1306.ClearDirectAligned(24, 0, 8, 80);
						ssd1306.Display();
					}

					previousMeasurementMessageLaser = currentMeasurement;
					ssd1306.DrawString(0, 32, currentMeasurement, 1);
					ssd1306.Display();
				}
				catch (Exception ex)
				{
					Debug.WriteLine($"Exception: {ex.Message}");
					continue;
				}
				/*if ((DateTime.UtcNow - beginTime).TotalMilliseconds > 1000)
				{
					//ssd1306.ClearDirectAligned(80, 0, 128 - 80, 8);
					ssd1306.ClearScreen();
					ssd1306.DrawString(80, 0, $"{frameCount} fps", 1);
					ssd1306.Display();

					beginTime = DateTime.UtcNow;
					frameCount = 0;
				}*/
			}
			//ssd1306.Write(0, 0, "Danya loh!!!");
			//ssd1306.Display();

			/*var i2c = new I2cConfiguration(1, 0x3C, true);
			var screen = new ScreenConfiguration(0, 0, 64, 128, nanoFramework.UI.GraphicDrivers.Ssd1306.GraphicDriver);
			DisplayControl.Initialize();
			Thread.Sleep(Timeout.Infinite);*/

			// Depending on the device you are using, you may have to adjust the pins
			// Here for an ESP32 with the Hardware.Esp32 nuget
			// For other devices like STM32, refer to the documentation, also remove the esp32 nuget
			// and comment the line above.

			/*Debug.WriteLine("Hello from I2C Scanner!");
			SpanByte span = new byte[1];
			bool isDevice;
			// On a normal bus, not all those ranges are supported but scanning anyway
			for (int i = 0x30; i <= 0xFF; i++)
			{
				isDevice = false;
				I2cDevice i2c = new(new I2cConnectionSettings(1, i));
				// What we write is not important
				var res = i2c.WriteByte(0x07);
				// A successfull write will be: 0x10 Write: 1, transferred: 1
				// A non successful one: 0x0F Write: 4, transferred: 0
				Debug.Write($"0x{i:X2} Write: {res.Status}, transferred: {res.BytesTransferred}");
				isDevice = res.Status == I2cTransferStatus.FullTransfer;

				// What we read doesn't matter, reading only 1 element is what's needed
				res = i2c.Read(span);
				// A successfull write will be: Read: 1, transferred: 1
				// A non successfull one: Read: 2, transferred: 0
				Debug.WriteLine($", Read: {res.Status}, transferred: {res.BytesTransferred}");

				// For most devices, success should be when you can write and read
				// Now, this can be adjusted with just read or write depending on the
				// device you are looking for
				isDevice &= res.Status == I2cTransferStatus.FullTransfer;
				Debug.WriteLine($"0x{i:X2} - {(isDevice ? "Present" : "Absent")}");

				// Just force to dispose so we can use the next one
				i2c.Dispose();
			}

			Thread.Sleep(Timeout.Infinite);*/
		}
	}
}
