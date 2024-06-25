using System;

namespace MBS
{
    public static class WUEmailTFA 
    {
        enum WUETFAActions { GenerateTFAEntry, ValidateKey }
        const string filepath = "wub_emailtfa/unity_functions.php";
        const string ASSET = "EMAILTFA";

        static public void GenerateTFAEntry(Action<CML> OnSuccess = null, Action<CMLData> OnFail = null)
        {
            WPServer.ContactServer(WUETFAActions.GenerateTFAEntry, filepath, ASSET, null, OnSuccess, OnFail);
        }
        static public void ValidateKey(string key, Action<CML> OnSuccess = null, Action<CMLData> OnFail = null)
        {
            CMLData data = new ();
            data.Set("code", Encoder.Base64Encode(key));
            WPServer.ContactServer(WUETFAActions.ValidateKey, filepath, ASSET, data, OnSuccess, OnFail);
        }
    }
}