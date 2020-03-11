﻿using Microsoft.AspNetCore.SignalR;
using StreamingBridge.Packet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreamingBridge.Hubs
{
	public class TweetHub:Hub
	{
		public override Task OnDisconnectedAsync(Exception exception)
		{
			Log(exception);
			return null;
		}
		public override Task OnConnectedAsync()
		{
			return base.OnConnectedAsync();
		}
		public async Task Keys(string publicUserKey, string secretUserKey, string publicAppKey, string secretAppKey)
		{
			OAuth.instence.SetKey(publicAppKey, secretAppKey, publicUserKey, secretUserKey);
		}

		public async Task StopStreaming()
		{
			UserStreaming.instence.DisconnectStreaming();
		}
		private static readonly object obj = new object();

		public async Task ResponseStreaming(string json)
		{
			Form1 form = Application.OpenForms[0] as Form1;
			if (form != null)
			{
				form.Send();
			}
			else
			{
				Debug.Print("Send Form NULL");
			}
			lock (obj)
			{
				using (FileStream fs = new FileStream(@"Send.txt", FileMode.Append))
				using (StreamWriter writer = new StreamWriter(fs))
				{
					writer.WriteLine(json);
					writer.Flush();
				}
			}
			Clients.All.SendAsync("ResponseStreaming", json);
		}

		public void Log(Exception e)
		{
			lock (obj)
			{
				using (FileStream fs = new FileStream(@"Log.txt", FileMode.Append))
				using (StreamWriter writer = new StreamWriter(fs))
				{
					writer.WriteLine($"{DateTime.Now:HH:mm:ss}: {e.Message}");
					writer.WriteLine($"{DateTime.Now:HH:mm:ss}: {e.StackTrace}");
					writer.Flush();
				}
			}
		}
	}
}
