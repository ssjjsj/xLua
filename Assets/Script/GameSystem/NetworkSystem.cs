using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleTCP;
using UnityEngine;

[XLua.LuaCallCSharp]
public class NetworkSystem : GameSystemBase
{
	SimpleTcpClient _tcpClient;
	NetPack _netPack = new NetPack();
	public bool IsConnected { get; set; }

	public XLua.LuaFunction MessageDispatch;

	private static NetworkSystem instance;
	public static NetworkSystem GetInstance()
	{
		return instance;
	}
	public override void Init()
	{
		instance = this;
		_tcpClient = new SimpleTcpClient();
	}

	public void Connect(string ip, int port)
	{
		_tcpClient.Connect(ip, port);
		IsConnected = true;
		_tcpClient.DataReceived += new EventHandler<Message>(onDataReceived);
	}

	private void onDataReceived(object sender, Message msg)
	{
		var dataReceived = _netPack.ProcessMessage(msg.Data);
		if (MessageDispatch != null)
		{
			if (dataReceived.Count > 0)
			{
				foreach (var msgdata in dataReceived)
				{
					MessageDispatch.Call(msgdata);
				}
			}
		}	
	}

	public void Send(byte[] data)
	{
		var length = data.Length;
		var sendData = new byte[data.Length + 2];
		sendData[1] = (byte)(length % 128);
		sendData[0] = (byte)(length / 128);
		Array.Copy(data, 0, sendData, 2, data.Length);

		_tcpClient.Write(sendData);
	}

	public override void Destroy()
	{
		base.Destroy();
		_tcpClient.Dispose();
	}
}
