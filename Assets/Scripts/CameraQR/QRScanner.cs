using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ZXing;
using ZXing.Common;
using System.Collections.Generic;

public class QRScanner : MonoBehaviour
{
    public RawImage cameraView;
    public TMP_Text qrText;

    private WebCamTexture camTexture;
    private BarcodeReader reader;
    private float noCardTime = 0f;
    void Start()
    {
        Debug.Log("Start wurde aufgerufen");

        WebCamDevice[] devices = WebCamTexture.devices;

        Debug.Log("Anzahl Kameras: " + devices.Length);

        if (devices.Length == 0)
        {
            qrText.text = "Keine Kamera gefunden!";
            Debug.LogError("Keine Kamera gefunden!");
            return;
        }

        string cameraName = devices[0].name;

        // Frontkamera auswählen
        foreach (WebCamDevice device in devices)
        {
            if (device.isFrontFacing)
            {
                cameraName = device.name;
                break;
            }
        }

        Debug.Log("Verwendete Kamera: " + cameraName);

        // Kamera mit fester Auflösung starten
        camTexture = new WebCamTexture(cameraName, 1920, 1080);

        cameraView.texture = camTexture;

        cameraView.rectTransform.anchorMin = Vector2.zero;
        cameraView.rectTransform.anchorMax = Vector2.one;
        cameraView.rectTransform.offsetMin = Vector2.zero;
        cameraView.rectTransform.offsetMax = Vector2.zero;

        camTexture.Play();


        //zeigt die Auflösung der Kamera an
        Debug.Log("Auflösung: " + camTexture.width + "x" + camTexture.height);

        Debug.Log("Kamera gestartet");

        reader = new BarcodeReader
        {
            AutoRotate = true,
            Options = new DecodingOptions
            {
                TryHarder = true,
                TryInverted = true,
                PossibleFormats = new List<BarcodeFormat>
                {
                    BarcodeFormat.QR_CODE
                }
            }
        };


    }

    void Update()
    {
        if (camTexture == null)
            return;

        if (!camTexture.isPlaying)
            return;

        if (camTexture.width < 100)
            return;

        qrText.text = "Kamera: " +
              camTexture.width + "x" +
              camTexture.height;

        cameraView.rectTransform.localEulerAngles =
            new Vector3(0, 0, -camTexture.videoRotationAngle);

        try
        {
            
            var result = reader.Decode(
                camTexture.GetPixels32(),
                camTexture.width,
                camTexture.height
            );

            if (result != null)
{
    noCardTime = 0f;

     Debug.Log("QR erkannt: [" + result.Text + "]");

    qrText.gameObject.SetActive(true);
    qrText.text = "Karte erkannt:\n" + result.Text;

    AttackCardManager.Instance.ReceiveCard(result.Text.Trim());
}
else
{
    noCardTime += Time.deltaTime;

    if (noCardTime >= 1.0f)
    {
        AttackCardManager.Instance.ResetLastScannedCard();
    }
}
        }
        catch (System.Exception e)
        {
            Debug.LogError("Fehler beim Scannen: " + e.Message);
        }
    }
}