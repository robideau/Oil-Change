/* David Robideau
 * 3/7/2016
 * 
 * This script implements player chat in a way that can be utilized in both build and play mode.
 * Will leave as open-ended as possible for future use.
 *
 * Last update - 3/30/2016
 */

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

//Warnings to ignore - DEV ONLY, REMOVE FOR FINAL BUILDS
#pragma warning disable 0618 //deprecated network view

public class ModularChat : MonoBehaviour {

	//UI components
	public GameObject ChatUI; //Holds all UI components - used to easily hide/display chat
	public UnityEngine.UI.InputField chatInput;
	public ScrollRect chatLogView;
	public Text chatLogText;
	public Button sendButton;
	public NetworkView nv;

	//Message data
	private string senderID = "Unknown";
	private List<chatMessage> messageLog;
	private string senderIDA = "PlayerA";
	private string senderIDB = "PlayerB";

	//Reference to player car - build mode only
	public PlayerCarController playerCarController = null;

	void Start () {
		messageLog = new List<chatMessage> ();
		chatLogView.transform.FindChild ("Scrollbar Vertical").GetComponent<Scrollbar> ().value = 0f;
		ChatUI.SetActive (false);
	}

	void Update () {

		//Deactivate car controls if chat input field is focused - does nothing in other modes
		if (chatInput.isFocused && playerCarController != null) {
			playerCarController.setMovementEnabled (false);
			playerCarController.cameraControlEnabled = false;
			playerCarController.fullResetEnabled = false;
			playerCarController.shortResetEnabled = false;
		}	else if (!chatInput.isFocused && playerCarController != null) {
			playerCarController.setMovementEnabled (true);
			playerCarController.cameraControlEnabled = true;
			playerCarController.fullResetEnabled = true;
			playerCarController.shortResetEnabled = true;
		}

		//Send message using enter key
		if (chatInput.isFocused && Input.GetKeyDown ("return")) {
			OnSendClick ();
		}

		//Toggle chat visible/hidden
		if (Input.GetKeyDown ("t") && !chatInput.isFocused) {
			ChatUI.SetActive (!ChatUI.activeSelf);
		}
			
	}

	//Add message to message log, send message to chat window, clear input field - called when "send" button is clicked or enter key is pressed
	public void OnSendClick() {
		assignSenderIDs ();
		nv.RPC ("sendChatMessage", RPCMode.All, chatInput.text, senderID);
		chatInput.text = "";
		chatInput.ActivateInputField();
	}

	//Called by On End Edit method of input field - must ensure that enter key has been pressed before sending message
	public void OnEnterPress() {
		if (Input.GetKeyDown("return")) {
			OnSendClick();
		}
	}

	//Assign sender IDs - replace with actual user data when ready
	private void assignSenderIDs() {
		if (nv.isMine) {
			senderID = senderIDA;
		} else {
			senderID = senderIDB;
		}
	}

	public void setSenderIDs(string playerNameA, string playerNameB) {
		senderIDA = playerNameA;
		senderIDB = playerNameB;
	}

	//RPC to update chat log
	[RPC] void sendChatMessage(string message, string senderID) {
		chatMessage sentMessage = new chatMessage (DateTime.Now.ToShortTimeString(), senderID, message); 
		messageLog.Add (sentMessage);
		chatLogText.text += ("\n" + senderID + ": " + message);
	}

	public void disableInput() {
		chatInput.gameObject.SetActive (false);
	}

	public void enableInput() {
		chatInput.gameObject.SetActive (true);
	}
}


//This class holds message data to be used in the detailed chat log
public class chatMessage {
	private string timeStamp;
	private string senderID;
	private string message;

	public chatMessage(string timeStamp, string senderID, string message) {
		this.timeStamp = timeStamp;
		this.senderID = senderID;
		this.message = message;
	}

	public string logString() {
		return ("[" + timeStamp + "] " + senderID + ": '" + message + "'");
	}
}
