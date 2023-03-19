using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NetPack
{
	public enum NetPackState
	{
		WaitLength = 0,
		WaitContent = 1,
	}

	private static readonly int MAX_PACK_LENGTH = 65535;

	byte[] _buffer = new byte[MAX_PACK_LENGTH];

	private NetPackState _netPackState = NetPackState.WaitLength;
	private int _reqLength; //总的长度
	private int _copyIndex; //下次开始拷贝的位置

	public List<byte[]> ProcessMessage(byte[] msg)
	{
		var totalLength = msg.Length;
		var curIndex = 0;
		var result = new List<byte[]>();

		while (curIndex < totalLength)
		{
			switch (_netPackState)
			{
				case NetPackState.WaitLength:
					{
						_reqLength = msg[curIndex] * 128 + msg[curIndex+1];
						_copyIndex = 0;
						curIndex += 2;
						_netPackState = NetPackState.WaitContent;
						break;
					}
				case NetPackState.WaitContent:
					{
						int needLength = _reqLength - _copyIndex;
						int leftLength = totalLength - curIndex;

						if (leftLength >= needLength)
						{
							var d = new byte[_reqLength];
							Array.Copy(msg, curIndex, _buffer, _copyIndex, needLength);
							Array.Copy(_buffer, d, _reqLength);
							result.Add(d);
							
							_copyIndex = 0;
							_reqLength = 0;
							curIndex += needLength;
							_netPackState = NetPackState.WaitLength;
						}
						else
						{
							Array.Copy(msg, curIndex, _buffer, _copyIndex, leftLength);
							_copyIndex += leftLength;
							curIndex += leftLength;
						}
						
						break;
					}
			}
		}
		return result;
	}
}
