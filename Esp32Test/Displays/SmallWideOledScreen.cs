using System.Device.I2c;
using System.Text;

namespace Esp32Test.Displays
{
	internal class SmallWideOledScreen
	{
		public SmallWideOledScreen(int sdaGpio, int sclGpio)
		{

		}

		public void Show()
		{
			// Set I2C address for the GME64128-02 (usually 0x3C or 0x3D depending on the device)
			const int GME64128_Address = 0x3C; // Check your module's address

			// Configure I2C settings
			var i2cConnectionSettings = new I2cConnectionSettings(1, GME64128_Address, I2cBusSpeed.FastMode);
			I2cDevice i2cDevice = I2cDevice.Create(i2cConnectionSettings);

			// Initialize the LCD (this will depend on the LCD's datasheet, but typically involves sending initialization commands)
			InitializeLcd(i2cDevice);

			// Test writing something to the LCD
			WriteDataToLcd(i2cDevice, "Hello, World!");
		}

		// Example initialization of the LCD
		private void InitializeLcd(I2cDevice device)
		{
			// Example commands for initialization (check the LCD datasheet for exact commands)
			// Send initialization command sequence
			byte[] initCommands = new byte[]
			{
				0x00, // Command mode
				// Add specific initialization commands from the LCD datasheet here
				0xAE, // Display OFF (sleep mode)
				0xA4, // Set display to follow RAM content
				0xA6, // Set normal display mode (not inverted)
				0xAF  // Display ON
			};

			// Send initialization sequence to the LCD
			device.Write(initCommands);
		}

		// Example method to write data to the LCD
		private void WriteDataToLcd(I2cDevice device, string data)
		{
			// Convert string data to bytes to send to the LCD
			byte[] dataBytes = Encoding.UTF8.GetBytes(data);

			// Command to set the cursor position or to start data writing can be needed (refer to the datasheet)
			byte[] command = new byte[] { 0x40 }; // Typically 0x40 for data mode, check LCD specifics

			// Write the command followed by data
			device.Write(command);
			device.Write(dataBytes);
		}
	}
}
