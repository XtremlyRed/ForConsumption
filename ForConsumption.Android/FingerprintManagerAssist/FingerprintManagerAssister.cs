using Android.Content;

using AndroidX.Core.Hardware.Fingerprint;

namespace ForConsumption.Droid.FingerprintManagerAssist
{
    public static class FingerprintManagerAssister
    {
        public static void Register(Context context)
        {
            const int flags = 0; /* always zero (0) */

            // CryptoObjectHelper is described in the previous section.
            CryptoObjectHelper cryptoHelper = new CryptoObjectHelper();

            // cancellationSignal can be used to manually stop the fingerprint scanner. 
            AndroidX.Core.OS.CancellationSignal cancellationSignal = new AndroidX.Core.OS.CancellationSignal();


            FingerprintManagerCompat fingerprintManager = FingerprintManagerCompat.From(context);

            // AuthenticationCallback is a base class that will be covered later on in this guide.
            FingerprintManagerCompat.AuthenticationCallback authenticationCallback = new MyAuthCallbackSample();

            // Start the fingerprint scanner.
            fingerprintManager.Authenticate(cryptoHelper.BuildCryptoObject(), flags, cancellationSignal, authenticationCallback, null);

        }
    }
}