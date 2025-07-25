using System;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ProjectConfiguration 
{

    public void PreBuild()
    {
        Debug.Log("ProjectConfiguration PreBuild");
        SetAndroidKeySign();
    }

    private void SetAndroidKeySign()
    {
        Debug.Log("ProjectConfiguration PreBuild >>> SetAndroidKeySign");
        PlayerSettings.Android.keystoreName = Directory.GetCurrentDirectory() + "/Keystore/user.keystore";
        PlayerSettings.Android.keystorePass = "DefaultCompany";
        PlayerSettings.Android.keyaliasName = "DefaultCompany";
        PlayerSettings.Android.keyaliasPass = "DefaultCompany";
    }
}
