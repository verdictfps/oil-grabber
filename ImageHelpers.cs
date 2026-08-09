using System.IO;
using BepInEx;
using PerfectRandom.Sulfur.Core.Items;
using PerfectRandom.Sulfur.Core.Weapons;
using UnityEngine;

public class ImageHelpers {
    public static void SaveBaseImage(ItemDefinition item)
    {
        byte[] pngBytes = ImageConversion.EncodeToPNG(MakeTextureReadable(item.artwork.texture));

        string outputPath = Path.Combine(Paths.PluginPath, "OilGrabber\\Extracted Data\\Extracted Images\\", $"{item.LocalizedDisplayName.ToLower().Replace(" ", "_")}_icon.png");
        File.WriteAllBytes(outputPath, pngBytes);
    }

    public static Texture2D MakeTextureReadable(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
            source.width, source.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;

        Texture2D readableText = new(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }
}