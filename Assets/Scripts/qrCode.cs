using UnityEngine;
using QRCoder;
using QRCoder.Unity;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class qrCode : MonoBehaviour
{
    [SerializeField] private int pixelsPerModule = 10;
    [SerializeField] private string startText = "https://rtuitlab.dev/";
    private RawImage image;

    private void Awake()
    {
        image = GetComponent<RawImage>();
        GenerateQR(startText);
    }

    public void GenerateQR(string text)
    {
        image.texture = GenerateTextureQR(text, pixelsPerModule);
    }

    private static Texture2D GenerateTextureQR(string text, int pixelsPerModule)
    {
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        UnityQRCode qrCode = new UnityQRCode(qrCodeData);
        Texture2D qrCodeAsTexture2D = qrCode.GetGraphic(pixelsPerModule);
        return qrCodeAsTexture2D;
    }
}
