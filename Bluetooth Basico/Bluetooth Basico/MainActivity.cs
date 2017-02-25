using Android.App;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
using Java.Util;
using System.IO;
using System;
using System.Threading.Tasks;

namespace Bluetooth_Basico
{
    [Activity(Label = "Bluetooth Basico", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private static BluetoothAdapter mBluetoothAdapter = null;
        private static BluetoothSocket btSocket = null;
        private static BluetoothDevice device;
        private static string address = "00:14:01:02:31:00";
        private static UUID MY_UUID = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB");
        private static Stream inStream = null;
        private static Stream outStream = null;
        bool isLightOn = false;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            Button buttonConnect = FindViewById<Button>(Resource.Id.btnConnect);
            Button buttonSend = FindViewById<Button>(Resource.Id.btnSend);

            buttonConnect.Click += ButtonConnect_Click;
            buttonSend.Click += ButtonSend_Click;
        }

        private void ButtonConnect_Click(object sender, System.EventArgs e)
        {
            Connect();
        }

        private void ButtonSend_Click(object sender, System.EventArgs e)
        {
            string finalValue = string.Empty;

            if (isLightOn)
            {
                finalValue = "0";
                Toast.MakeText(this, "Luz encendida", ToastLength.Short).Show();
            }
            else
            {
                finalValue = "1";
                Toast.MakeText(this, "Luz apagada", ToastLength.Short).Show();
            }

            isLightOn = !isLightOn;
            WriteData(finalValue);
        }

        private void CheckBluetoothDevice()
        {
            mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;

            if (!mBluetoothAdapter.Enable())
            {
                Toast.MakeText(this, "Bluetooth disabled", ToastLength.Short).Show();
            }

            if (mBluetoothAdapter == null)
            {
                Toast.MakeText(this, "Bluetooth device is occupied or non-existant", ToastLength.Short).Show();
            }
        }

        private void Connect()
        {
            mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            device = mBluetoothAdapter.GetRemoteDevice(address);
            Toast.MakeText(this, "Conectando a " + device.Name, ToastLength.Short).Show();
            mBluetoothAdapter.CancelDiscovery();
            try
            {
                btSocket = device.CreateRfcommSocketToServiceRecord(MY_UUID);
                btSocket.Connect();
                Toast.MakeText(this, "Conexión establecida", ToastLength.Short).Show();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                try
                {
                    btSocket.Close();
                }
                catch (Exception)
                {
                    Toast.MakeText(this, "Imposible conectar", ToastLength.Short).Show();
                }
            }
            BeginListenForData();
        }

        private void BeginListenForData()
        {
            try
            {
                inStream = btSocket.InputStream;
            }
            catch (IOException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            Task.Factory.StartNew(() => {
                byte[] buffer = new byte[1024];
                int bytes;
                while (true)
                {
                    try
                    {
                        bytes = inStream.Read(buffer, 0, buffer.Length);
                        if (bytes > 0)
                        {
                            //RunOnUiThread(() => {
                            //    string valor = System.Text.Encoding.ASCII.GetString(buffer);
                            //    Result.Text = Result.Text + "\n" + valor;
                            //});
                        }
                    }
                    catch (Java.IO.IOException)
                    {
                        //RunOnUiThread(() => {
                        //    Result.Text = string.Empty;
                        //});
                        //break;
                    }
                }
            });
        }

        private void WriteData(string data)
        {
            try
            {
                outStream = btSocket.OutputStream;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error al enviar" + e.Message);
            }

            Java.Lang.String message = new Java.Lang.String(data);

            byte[] msgBuffer = message.GetBytes();

            try
            {
                outStream.Write(msgBuffer, 0, msgBuffer.Length);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error al enviar" + e.Message);
            }
        }
    }
}

