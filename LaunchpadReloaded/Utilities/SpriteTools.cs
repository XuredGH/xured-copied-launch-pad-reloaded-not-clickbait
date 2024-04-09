﻿using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Reactor.Utilities.Extensions;
using System;
using System.Reflection;
using UnityEngine;

namespace LaunchpadReloaded.Utilities;

public static class SpriteTools
{
    private delegate bool DLoadImage(IntPtr tex, IntPtr data, bool markNonReadable);

    private static DLoadImage _iCallLoadImage;

    private static bool LoadImage(Texture2D tex, byte[] data, bool markNonReadable)
    {
        _iCallLoadImage ??= IL2CPP.ResolveICall<DLoadImage>("UnityEngine.ImageConversion::LoadImage");

        var il2CPPArray = (Il2CppStructArray<byte>)data;

        return _iCallLoadImage.Invoke(tex.Pointer, il2CPPArray.Pointer, markNonReadable);
    }

    public static Sprite LoadSpriteFromPath(string resourcePath)
    {
        var tex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
        var assembly = Assembly.GetExecutingAssembly();
        var myStream = assembly.GetManifestResourceStream(resourcePath);
        if (myStream != null)
        {
            var buttonTexture = myStream.ReadFully();
            LoadImage(tex, buttonTexture, false);
        }

        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }
}