using UnityEngine;
using System.Collections;

public class RefreshGalleryWrapper : MonoBehaviour {

	#if UNITY_ANDROID

	private const string READ_STORAGE_PERMISSION = "android.permission.READ_EXTERNAL_STORAGE";
	private const string WRITE_STORAGE_PERMISSION = "android.permission.WRITE_EXTERNAL_STORAGE";

	void Start()
	{
		StoragePermissionRequest();
		SetGalleryPath ();
	}

	void SetGalleryPath()
	{
		if (Application.platform == RuntimePlatform.Android) {
			string captureAndSaveGameObjectName = "CaptureAndSave";
			CaptureAndSave captureAndSave = GameObject.FindObjectOfType<CaptureAndSave> ();
			if (captureAndSave != null) {
				captureAndSaveGameObjectName =	captureAndSave.gameObject.name;
			}
		
			AndroidJavaClass javaClass = new AndroidJavaClass ("com.astricstore.androidutils.AndroidGallery");
			javaClass.CallStatic ("SetGalleryPath", captureAndSaveGameObjectName);
		}
	}

	void RefreshGallery(string path)
	{
		if (Application.platform == RuntimePlatform.Android) {
			AndroidJavaClass javaClass = new AndroidJavaClass ("com.astricstore.androidutils.AndroidGallery");
			javaClass.CallStatic ("RefreshGallery", path);
		}
	}

	bool CheckPermissions()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return true;
		}
		
		return (AndroidPermissionsManager.IsPermissionGranted(READ_STORAGE_PERMISSION) && AndroidPermissionsManager.IsPermissionGranted(WRITE_STORAGE_PERMISSION));
	}


	void StoragePermissionRequest()
	{
		if (CheckPermissions ())
			return;

		AndroidPermissionsManager.RequestPermission(new []{READ_STORAGE_PERMISSION, WRITE_STORAGE_PERMISSION}, new AndroidPermissionCallback(
			grantedPermission =>
			{
			// permission granted.
		},
		deniedPermission =>
		{
			// The permission was denied.
		}));
	}
	#endif
}
