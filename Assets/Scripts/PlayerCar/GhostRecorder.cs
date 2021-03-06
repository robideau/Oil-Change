﻿/* David Robideau
 * 2/8/2016
 * 
 * This script handles recording of driver "ghost" data to be used during play mode.
 *
 * Last update - 2/18/2016
 */

using UnityEngine;
using System.Collections;

public class GhostRecorder : MonoBehaviour {

	//References to car components to track
	public GameObject carBase;
	public GameObject carBody;
	private GameObject ghostBody;
	private Transform initialTransform;

	//Tracking data
	private Vector3[] positionData;
	private Quaternion[] rotationData;
	private int currentFrame;
	private int recordingCount;
	private bool isRecording;
	private bool isReplaying;

	//Parameters and settings
	public int recordingIntervals; //number of frames between each recording
	public int maxRecordingSize; //maximum number of transforms to record

	void Start () {
		initialTransform = carBase.transform;
		positionData = new Vector3[maxRecordingSize];
		rotationData = new Quaternion[maxRecordingSize];
		currentFrame = 0;
		recordingCount = 0;
		setIsRecording (true);
		setIsReplaying (false);
	}

	void Update () {
		currentFrame++;

		//If current frame is a recording frame according to recordingIntervals
		if (getIsRecording ()) {
			if ((currentFrame % recordingIntervals == 0) && !(recordingCount >= maxRecordingSize)) {
				recordTransform (recordingCount);
				recordingCount++;
			}
		}

		//If ghost is currently replaying
		if (getIsReplaying () && currentFrame < recordingCount-1) {
			ghostBody.transform.position = new Vector3 (positionData [currentFrame].x, positionData[currentFrame].y + .2f, positionData[currentFrame].z);
			ghostBody.transform.rotation = rotationData [currentFrame];
			ghostBody.transform.Rotate(new Vector3(0, -90, 0));
		}

	}

	//Record one frame of transform data
	public void recordTransform (int recordingNumber) {
		positionData [recordingNumber] = carBase.transform.position;
		rotationData [recordingNumber] = carBase.transform.rotation;
	}

	//Instantiate and begin replaying ghost data
	public void replayGhost() {
		ghostBody = Instantiate (carBody);
		ghostBody.GetComponent<BoxCollider> ().enabled = false;
		ghostBody.transform.position = new Vector3 (initialTransform.position.x, initialTransform.transform.position.y + .2f, initialTransform.position.z);
		ghostBody.transform.rotation = initialTransform.rotation;
		ghostBody.transform.localScale = new Vector3 (0.13f, 0.13f, 0.13f);
		setIsReplaying (true);
		currentFrame = 0;
	}

	public void setIsRecording(bool recording) {
		isRecording = recording;
	}

	public bool getIsRecording() {
		return isRecording;
	}

	public void setIsReplaying(bool replaying) {
		isReplaying = replaying;
	}

	public bool getIsReplaying() {
		return isReplaying;
	}

	public Vector3 getFramePosition(int frameNumber) {
		if (frameNumber < recordingCount && frameNumber >= 0) {
			return positionData [frameNumber];
		} else {
			return positionData [0];
		}
	}

	public Quaternion getFrameRotation(int frameNumber) {
		if (frameNumber < recordingCount && frameNumber >= 0) {
			return rotationData [frameNumber];
		} else {
			return rotationData [0];
		}
	}

	public Transform getInitialTransform() {
		return initialTransform;
	}

	public int getRecordingCount() {
		return recordingCount;
	}
}
