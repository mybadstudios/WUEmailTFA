using UnityEngine;
using System;

namespace MBS
{
    public static class WUEmailTFA 
    {
        enum WUETFAActions { GenerateTFAEntry, ValidateKey }
        const string filepath = "wub_emailtfa/unity_functions.php";
        const string ASSET = "EMAILTFA";

        static public void GenerateTFAEntry(string gamename, Action<CML> OnSuccess = null, Action<CMLData> OnFail = null)
        {
            CMLData data = new ();
            data.Set("gamename", gamename);
            WPServer.ContactServer(WUETFAActions.GenerateTFAEntry, filepath, ASSET, data, OnSuccess, OnFail);
        }
        static public void ValidateKey(string key, Action<CML> OnSuccess = null, Action<CMLData> OnFail = null)
        {
            CMLData data = new ();
            data.Set("code", Encoder.Base64Encode(key));
            WPServer.ContactServer(WUETFAActions.ValidateKey, filepath, ASSET, data, OnSuccess, OnFail);
        }
    }
}