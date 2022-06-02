using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NatSuite.Recorders;
using NatSuite.Recorders.Clocks;

public class camera : MonoBehaviour
{

    /*public RawImage rawimage;*/
    private WebCamTexture webcamTexture;
    private MP4Recorder recorder;
    private Coroutine recordVideoCoroutine;

    public async void startRecording()
    {
        // Create a recorder
        recorder = new MP4Recorder(1280, 720, 30);
        //Start recording
        recordVideoCoroutine = StartCoroutine(recording());
    }
    private IEnumerator recording()
    {
        // Create a clock for generating recording timestamps
        var clock = new RealtimeClock();
        for (int i = 0; ; i++)
        {
            // Commit the frame to NatCorder for encoding
            recorder.CommitFrame(webcamTexture.GetPixels32(), clock.timestamp);
            // Wait till end of frame
            yield return new WaitForEndOfFrame();
        }
    }
    public async void stopRecording()
    {
        //Stop Coroutine
        StopCoroutine(recordVideoCoroutine);
        // Finish writing
        var recordingPath = await recorder.FinishWriting();
        //save video to gallery
        NativeGallery.Permission permission = NativeGallery.SaveVideoToGallery(recordingPath, "CameraTest", "testVideo.mp4", (success, path) => Debug.Log("Media save result: " + success + " " + path));
    }

    /*// Start is called before the first frame update
    void Start()
    {
        //Obtain camera devices available
        WebCamDevice[] cam_devices = WebCamTexture.devices;
        //Set a camera to the webcamTexture
        webcamTexture = new WebCamTexture( 720, 1280, 30);
        //Set the webcamTexture to the texture of the rawimage
        rawimage.texture = webcamTexture;
        rawimage.material.mainTexture = webcamTexture;
        //Start the camera
        webcamTexture.Play();
    }

    private IEnumerator SaveImage()
    {
        //Create a Texture2D with the size of the rendered image on the screen.
        Texture2D texture = new Texture2D(rawimage.texture.width, rawimage.texture.height, TextureFormat.ARGB32, false);
        //Save the image to the Texture2D
        texture.SetPixels(webcamTexture.GetPixels());
        //texture = RotateTexture(texture, -90);
        texture.Apply();
        yield return new WaitForEndOfFrame();
        // Save the screenshot to Gallery/Photos
        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(texture, "CameraTest", "CaptureImage.png", (success, path) => Debug.Log("Media save result: " + success + " " + path));
        // To avoid memory leaks
        Destroy(texture);
    }

    public void clickCapture()
    {
        StartCoroutine(SaveImage());
    }*/

}
