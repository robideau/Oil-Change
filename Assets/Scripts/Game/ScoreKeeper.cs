/* David Robideau
 * 4/7/2016
 * 
 * This script handles all scoring - tracks times and calculates scores accordingly, then presents to players once the game is over.
 *
 * Last update - 4/7/2016
 */

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class ScoreKeeper : MonoBehaviour {

	//Time recording variables - in seconds for easy calculation
	private double playerTestStart, playerTestFinish, playerRaceStart, playerRaceFinish;

	public bool playerTestTimeSet = false;
	public bool playerRaceTimeSet = false;

	private double playerTestTime, playerRaceTime, opponentTestTime, opponentRaceTime;
	private string playerName, opponentName;

	public Text buildTimer;
	public Text raceTimer;
	public TransitionHandler transitionHandler;
	private NetworkView nv;
	private accountInfo accInfo;

	void Start () {
		playerRaceStart = 0;
		nv = GetComponent<NetworkView> ();
		accInfo = GameObject.Find ("Script manager").GetComponent<accountInfo> ();
		playerName = accInfo.getName ();
	}

	//Reset previous finish time (if it exists) and record start time
	public void startRecordingTestTime() {
		playerTestFinish = 0;
		string currentTime = "00:" + buildTimer.text.Substring(11);
		playerTestStart = TimeSpan.Parse (currentTime).TotalSeconds;
		playerTestTimeSet = false;
	}

	//Record finish time and determine total drive time
	public void finishRecordingTestTime() {
		string currentTime = "00:" + buildTimer.text.Substring(11);
		playerTestFinish = TimeSpan.Parse (currentTime).TotalSeconds;

		//Since build timer counts down, subtract finish time from start time
		playerTestTime = playerTestStart - playerTestFinish;
		playerTestTimeSet = true;
	}

	public void finishRecordingRaceTime() {
		string currentTime = "00:" + raceTimer.text;
		playerRaceFinish = TimeSpan.Parse (currentTime).TotalSeconds;

		//Since race timer counts up, race time is equivalent to finish time.
		playerRaceTime = playerRaceFinish;
		playerRaceTimeSet = true;
		//Send scores to opponent
		nv.RPC ("sendScores", RPCMode.Others, (int)playerTestTime, (int)playerRaceTime, playerName);
	}

	public void outputTextResults() {
		print ("Your test time: " + playerTestTime);
		print ("Your race time: " + playerRaceTime);
		print ("Opponent test time: " + opponentTestTime);
		print ("Opponent race time: " + opponentRaceTime);
		print ("Your track difference: " + (opponentRaceTime - playerTestTime));
		print ("Opponent track difference: " + (playerRaceTime - opponentTestTime));
		if ((opponentRaceTime - playerTestTime) > (playerRaceTime - opponentTestTime)) {
			print ("You won.");
		} else if ((opponentRaceTime - playerTestTime) < (playerRaceTime - opponentTestTime)) {
			print ("Opponent won.");
		} else {
			print ("Tie.");
		}
	}

	[RPC] void sendScores(int testTime, int raceTime, string playerName) {
		opponentTestTime = testTime;
		opponentRaceTime = raceTime;
		opponentName = playerName;
	}

	public double getSelfTestTime() {
		return playerTestTime;
	}

	public double getSelfRaceTime() {
		return playerRaceTime;
	}

	public double getOpponentTestTime() {
		return opponentTestTime;
	}

	public double getOpponentRaceTime() {
		return opponentRaceTime;
	}

	public string getPlayerName() {
		return playerName;
	}

	public string getOpponentName() {
		return opponentName;
	}
}
