using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace StocksDevice
{
    public partial class device : Form
    {
        public device()
        {
            InitializeComponent();
            outputLabel.Text = "";
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }

        private async void SendState(object sender, EventArgs e)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://anceag-001-site1.itempurl.com/api/items/writestatewithdevice");
                request.Method = "POST";
                request.AutomaticDecompression = DecompressionMethods.GZip;
                request.ContentType = "application/json";

                using (var stream = await request.GetRequestStreamAsync())
                using (var streamWriter = new StreamWriter(stream))
                {
                    if (int.TryParse(massTextBox.Text, out int mass) && int.TryParse(deviceIdTextBox.Text, out int id))
                    {
                        var state = new State() { DeviceId = id, Mass = mass };
                        streamWriter.Write(JsonConvert.SerializeObject(state));
                    }
                    else
                    {
                        outputLabel.Text = "Data is invalid";
                        return;
                    }
                }

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    await reader.ReadToEndAsync();
                    outputLabel.Text = "Success";
                }
            }
            catch
            {
                outputLabel.Text = "Error";
            }
        }
    }
}
