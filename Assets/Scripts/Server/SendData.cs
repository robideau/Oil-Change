/*
 * written by Nick Bramanti
 * 3/10/2016
 * 
 * this script will be for sending given data, with a string name for identification to other players via RPC
 */

using UnityEngine;
using System.Collections;
using System.Text;


public class SendData  : MonoBehaviour {
	private string dataToSend;
	private byte[] dataBytes;
	public NetworkView nView;

	//Constructor for SendData
	//Data should be formatted as ID,X,Y,Z,prefab
	public SendData(string data) {
		dataToSend = data;
		byte[] dataBytes = new byte[data.Length * sizeof(char)];
		dataBytes = getBytes (data);
		//send the data to everyone but yourself
		nView.RPC ("recieveData", RPCMode.Others, dataBytes);
	}

	//convert a string into a byte array
	byte[] getBytes(string str) {
		char[] arr = str.ToCharArray ();
		dataBytes = Encoding.ASCII.GetBytes (arr);
		return dataBytes;
	}

	//convert a byte array to a string
	string getString(byte[] bytes) {
		string str = "";
		return str;
	}

	//RPC call for transferring the data to other players
	//takes in a byte array and then tr
	[RPC]
	public string receiveData(byte[] data) {
		char[] chars = new char[data.Length];
		chars = Encoding.ASCII.GetChars (data);
		return new string (chars);
	}

}
