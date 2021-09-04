using AndroidX.Core.Hardware.Fingerprint;

using Java.Lang;

using Javax.Crypto;

namespace ForConsumption.Droid.FingerprintManagerAssist
{
    internal class MyAuthCallbackSample : FingerprintManagerCompat.AuthenticationCallback
    {
        private static readonly byte[] SECRET_BYTES = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        public MyAuthCallbackSample()
        {
        }

        public override void OnAuthenticationSucceeded(FingerprintManagerCompat.AuthenticationResult result)
        {
            if (result.CryptoObject.Cipher != null)
            {
                try
                {
                    byte[] doFinalResult = result.CryptoObject.Cipher.DoFinal(SECRET_BYTES);

                }
                catch (BadPaddingException)
                {
                    // Can't really trust the results. 
                }
                catch (IllegalBlockSizeException)
                {
                    // Can't really trust the results. 
                }
            }
            else
            {
                // No cipher used, assume that everything went well and trust the results.
            }
        }

        public override void OnAuthenticationError(int errMsgId, ICharSequence errString)
        {
            // Report the error to the user. Note that if the user canceled the scan,
            // this method will be called and the errMsgId will be FingerprintState.ErrorCanceled.
        }

        public override void OnAuthenticationFailed()
        {
            // Tell the user that the fingerprint was not recognized.
        }

        public override void OnAuthenticationHelp(int helpMsgId, ICharSequence helpString)
        {
            // Notify the user that the scan failed and display the provided hint.
        }
    }
}