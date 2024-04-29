using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine;

// NOTE: This script is a modified version of the PrefabImagePairManager script from the ARFoundation Samples.
// The main modifications are seen in the OnTrackedImagesChanged() method.

// Listens for images detected by the XRImageTrackingSubsystem and overlays some prefabs on top of the detected image.
[RequireComponent(typeof(ARTrackedImageManager))]
public class PrefabImagePairManager : MonoBehaviour, ISerializationCallbackReceiver
{
	// Associate an XRReferenceImage with a Prefab using the XRReferenceImage's guid as a unique identifier per image.
	[Serializable]
	struct NamedPrefab
	{
		// System.Guid isn't serializable, so we store the Guid as a string. At runtime, this is converted back to a System.Guid
		public string imageGuid;
		public GameObject imagePrefab;
		public NamedPrefab(Guid guid, GameObject prefab)
		{
			imageGuid = guid.ToString();
			imagePrefab = prefab;
		}
	}

	[SerializeField]
	[HideInInspector]
	List<NamedPrefab> m_PrefabsList = new();

	Dictionary<Guid, GameObject> m_PrefabsDictionary = new();

	ARTrackedImageManager m_TrackedImageManager;

	[SerializeField]
	XRReferenceImageLibrary m_ImageLibrary;

	CurrencyController currencyController;

	public XRReferenceImageLibrary ImageLibrary
	{
		get => m_ImageLibrary;
		set => m_ImageLibrary = value;
	}

	public void OnBeforeSerialize()
	{
		m_PrefabsList.Clear();
		foreach (KeyValuePair<Guid, GameObject> kvp in m_PrefabsDictionary)
		{
			m_PrefabsList.Add(new NamedPrefab(kvp.Key, kvp.Value));
		}
	}

	public void OnAfterDeserialize()
	{
		m_PrefabsDictionary = new Dictionary<Guid, GameObject>();
		foreach (NamedPrefab entry in m_PrefabsList)
		{
			m_PrefabsDictionary.Add(Guid.Parse(entry.imageGuid), entry.imagePrefab);
		}
	}

	void Start()
	{
		m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
		m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
		currencyController = gameObject.GetComponent<CurrencyController>();
	}

	// When a new image is detected, instantiate a prefab on top of it.
	void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
	{
		ARAnchor planeAnchor = gameObject.GetComponent<PlaneController>().groundAnchor;
		// for each new image
		foreach (ARTrackedImage newImage in eventArgs.added)
		{
			// Give the initial image a smallish default scale based on dimensions
			float minLocalScalar = Mathf.Min(newImage.size.x, newImage.size.y) / 4;
			newImage.transform.localScale = new Vector3(minLocalScalar, minLocalScalar, minLocalScalar);

			if (m_PrefabsDictionary.TryGetValue(newImage.referenceImage.guid, out GameObject prefab) && planeAnchor != null)
			{
				string prefabTag = prefab.tag;
				if (prefabTag == "Tower")
				{
					newImage.transform.position.Set(
						newImage.transform.position.x,
						planeAnchor.transform.position.y,
						newImage.transform.position.z);
					newImage.transform.rotation = planeAnchor.transform.rotation;
					currencyController.SubtractCurrency(10); // tower costs 10 currency
				}
				else // nodes are offset above the ground plane for enemies to travel at turret firing height
				{
					newImage.transform.position.Set(
						newImage.transform.position.x,
						planeAnchor.transform.position.y + 1.4f * minLocalScalar,
						newImage.transform.position.z);
					newImage.transform.rotation = planeAnchor.transform.rotation;
					// nodes not visible in release build
					if (!Debug.isDebugBuild)
					{
						prefab.GetComponent<MeshRenderer>().enabled = false;
					}
				}
				Instantiate(prefab, newImage.transform);
			}
		}
		// for each updated image
		foreach (ARTrackedImage updatedImage in eventArgs.updated)
		{
			if (m_PrefabsDictionary.TryGetValue(updatedImage.referenceImage.guid, out GameObject prefab) && planeAnchor != null)
			{
				string prefabTag = prefab.tag;
				if (prefabTag == "Tower")
				{
					updatedImage.transform.position.Set(
						updatedImage.transform.position.x,
						planeAnchor.transform.position.y,
						updatedImage.transform.position.z);
					updatedImage.transform.rotation = planeAnchor.transform.rotation;
				}
				else // nodes are offset above the ground plane for enemies to travel at turret firing height
				{
					updatedImage.transform.position.Set(
						updatedImage.transform.position.x,
						planeAnchor.transform.position.y + 1.4f * updatedImage.transform.localScale.x,
						updatedImage.transform.position.z);
					updatedImage.transform.rotation = planeAnchor.transform.rotation;
				}
			}
		}
		// for each removed image
		foreach (ARTrackedImage _ in eventArgs.removed)
		{
			// do nothing
		}
	}

	public GameObject GetPrefabForReferenceImage(XRReferenceImage referenceImage)
	{
		if (m_PrefabsDictionary.TryGetValue(referenceImage.guid, out GameObject prefab))
		{
			return prefab;
		}
		return null;
	}

#if UNITY_EDITOR
	// customise the inspector component and update the prefab list when the reference image library is changed.
	[CustomEditor(typeof(PrefabImagePairManager))]
	class PrefabImagePairManagerInspector : Editor
	{
		readonly List<XRReferenceImage> m_ReferenceImages = new();
		bool m_IsExpanded = true;

		bool HasLibraryChanged(XRReferenceImageLibrary library)
		{
			if (library == null)
			{
				return m_ReferenceImages.Count == 0;
			}
			if (m_ReferenceImages.Count != library.count)
			{
				return true;
			}
			for (int i = 0; i < library.count; i++)
			{
				if (m_ReferenceImages[i] != library[i])
				{
					return true;
				}
			}
			return false;
		}

		public override void OnInspectorGUI()
		{
			// customised inspector
			PrefabImagePairManager behaviour = (PrefabImagePairManager)serializedObject.targetObject;

			serializedObject.Update();
			using (new EditorGUI.DisabledScope(true))
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
			}

			SerializedProperty libraryProperty = serializedObject.FindProperty(nameof(m_ImageLibrary));
			EditorGUILayout.PropertyField(libraryProperty);
			XRReferenceImageLibrary library = (XRReferenceImageLibrary)libraryProperty.objectReferenceValue;

			// check for library change
			if (HasLibraryChanged(library))
			{
				if (library)
				{
					Dictionary<Guid, GameObject> tempDictionary = new();
					foreach (XRReferenceImage referenceImage in library)
					{
						tempDictionary.Add(referenceImage.guid, behaviour.GetPrefabForReferenceImage(referenceImage));
					}
					behaviour.m_PrefabsDictionary = tempDictionary;
				}
			}

			// update current
			m_ReferenceImages.Clear();
			if (library)
			{
				foreach (XRReferenceImage referenceImage in library)
				{
					m_ReferenceImages.Add(referenceImage);
				}
			}

			// show prefab list
			m_IsExpanded = EditorGUILayout.Foldout(m_IsExpanded, "Prefab List");
			if (m_IsExpanded)
			{
				using (new EditorGUI.IndentLevelScope())
				{
					EditorGUI.BeginChangeCheck();

					Dictionary<Guid, GameObject> tempDictionary = new();
					foreach (XRReferenceImage image in library)
					{
						GameObject prefab = (GameObject)EditorGUILayout.ObjectField(image.name, behaviour.m_PrefabsDictionary[image.guid], typeof(GameObject), false);
						tempDictionary.Add(image.guid, prefab);
					}

					if (EditorGUI.EndChangeCheck())
					{
						Undo.RecordObject(target, "Update Prefab");
						behaviour.m_PrefabsDictionary = tempDictionary;
						EditorUtility.SetDirty(target);
					}
				}
			}
			serializedObject.ApplyModifiedProperties();
		}
	}
#endif
}
