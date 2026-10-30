using System;
using System.IO;
using System.Runtime.InteropServices;

namespace GwentAccess
{
    /// <summary>Sends text to NVDA through the NVDA controller client.</summary>
    internal static class Speech
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private delegate int SpeakTextFn([MarshalAs(UnmanagedType.LPWStr)] string text);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int NoArgFn();

        private static SpeakTextFn _speak;
        private static NoArgFn _cancel;
        private static NoArgFn _test;
        private static readonly object Lock = new object();

        public static bool Init(string pluginDir, Action<string> log)
        {
            try
            {
                string path = Path.Combine(pluginDir, "nvdaControllerClient.dll");
                IntPtr lib = NativeLibrary.Load(path);
                _speak = Marshal.GetDelegateForFunctionPointer<SpeakTextFn>(NativeLibrary.GetExport(lib, "nvdaController_speakText"));
                _cancel = Marshal.GetDelegateForFunctionPointer<NoArgFn>(NativeLibrary.GetExport(lib, "nvdaController_cancelSpeech"));
                _test = Marshal.GetDelegateForFunctionPointer<NoArgFn>(NativeLibrary.GetExport(lib, "nvdaController_testIfRunning"));
                log("NVDA controller loaded. NVDA running: " + IsNvdaRunning);
                return true;
            }
            catch (Exception e)
            {
                log("Could not load NVDA controller: " + e);
                return false;
            }
        }

        public static bool IsNvdaRunning
        {
            get { try { return _test != null && _test() == 0; } catch { return false; } }
        }

        /// <summary>Speak text. interrupt=true stops whatever NVDA is saying first.</summary>
        private static string _last = "";
        public static void Repeat() { if (_last.Length > 0) Say(_last, true); }

        public static void Say(string text, bool interrupt = false)
        {
            if (string.IsNullOrWhiteSpace(text) || _speak == null) return;
            _last = text;
            lock (Lock)
            {
                try
                {
                    if (interrupt) _cancel();
                    _speak(text);
                }
                catch { }
            }
            Plugin.LogInfo("Spoke: " + text);
        }
    }
}
