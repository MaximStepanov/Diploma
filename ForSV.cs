using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Vuforia;

public class ForSV : MonoBehaviour {

	public TextMesh tx;
	// specify these in Unity Inspector
	public GameObject augmentationObject = null;  // what to use as Border for example
	public string dataSetName = "";  //  Assets/Editor/QCAR/ImagweTargetTextures/ExDB


	void Start()
	{
		VuforiaBehaviour vb = GameObject.FindObjectOfType<VuforiaBehaviour>();
		vb.RegisterVuforiaStartedCallback(LoadDataSet);

	}



	void LoadDataSet()
	{

		ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();

		DataSet dataSet = objectTracker.CreateDataSet();

		if (dataSet.Load(dataSetName)) {

			objectTracker.Stop();  // stop tracker so that we can add new dataset

			if (!objectTracker.ActivateDataSet(dataSet)) {
				// Note: ImageTracker cannot have more than 100 total targets activated
				Debug.Log("<color=yellow>Failed to Activate DataSet: " + dataSetName + "</color>");
			}

			if (!objectTracker.Start()) {
				Debug.Log("<color=yellow>Tracker Failed to Start.</color>");
			}

			// int counter = 0; //is never used 

			IEnumerable<TrackableBehaviour> tbs = TrackerManager.Instance.GetStateManager().GetTrackableBehaviours();
			foreach (TrackableBehaviour tb in tbs) {
				if (tb.name == "New Game Object") {

					// change generic name to include trackable name
					tb.gameObject.name = tb.TrackableName;
					//change TEXT in TextMesh which will be shown at the screen in border
					string second = "";
					second = tb.TrackableName.Split('_')[1]; //looking for CLASSNAME

					switch (second)  // changing TextMesh value in case of CLASSNAME
					{
					case "DOG":
						tx.text = "Dog";
						break;

					case "CAT":
						tx.text = "Cat";
						break;

					case "MARKER":
						tx.text = "Marker";
						break;

					case "LAPTOP":
						tx.text = "Laptop";
						break;

					case "KEYBOARD":
						tx.text = "Keyboard";
						break;

					case "HAND":
						tx.text = "Hand";
						break;

					case "LIGHTS":
						tx.text = "Lights";
						break;

					case "BOOK":
						tx.text = "Book";
						break;

					case "OTHER":
						tx.text = tb.TrackableName.Split ('_') [2];
						break;

					default:
						tx.text = "Unexpected Object";
						break;

					}
					//tx.text = tb.name;

					// add additional script components for trackable
					tb.gameObject.AddComponent<DefaultTrackableEventHandler>();
					tb.gameObject.AddComponent<TurnOffBehaviour>();

					if (augmentationObject != null) {
						// instantiate augmentation object and parent to trackable
						GameObject augmentation = (GameObject)GameObject.Instantiate(augmentationObject);
						augmentation.transform.parent = tb.gameObject.transform;
						augmentation.transform.localPosition = new Vector3(0f, 0f, 0f);
						augmentation.transform.localRotation = Quaternion.identity;
						augmentation.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f); //size of my border 
						augmentation.gameObject.SetActive(true);
					} else {
						Debug.Log("<color=yellow>Warning: No augmentation object specified for: " + tb.TrackableName + "</color>");
					}
				}
			}
		} else {
			Debug.LogError("<color=yellow>Failed to load dataset: '" + dataSetName + "'</color>");
		}
	}
}
