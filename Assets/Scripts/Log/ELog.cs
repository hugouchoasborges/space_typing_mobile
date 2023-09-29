using log.settings;
using tools;
using System;
using UnityEngine;

namespace log
{
    public class ELog : Singleton<ELog>
    {
        private ELogTypeSettingsSO _settings;
        public ELogTypeSettingsSO Settings
        {
            get
            {
                if (_settings == null)
                    _settings = ELogTypeSettingsSO.Instance;
                return _settings;
            }
        }

        private static string GetFormattedLog(ELogType type, string log)
        {
            return string.Format("[{0}] {1}", type, log);
        }

        private static bool LogEnabled(ELogType type)
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                return ELogTypeSettingsSO.Instance.EntryEnabled(type);
#endif

            return Instance.Settings.EntryEnabled(type);
        }

        public static void Log(ELogType type, string log, params object[] args)
        {
            if (!LogEnabled(type)) return;

            Debug.LogFormat(GetFormattedLog(type, log), args);
        }

        public static void LogWarning(ELogType type, string log, params object[] args)
        {
            if (!LogEnabled(type)) return;

            Debug.LogWarningFormat(GetFormattedLog(type, log), args);
        }

        public static void LogError(ELogType type, string log, params object[] args)
        {
            if (!LogEnabled(type)) return;

            Debug.LogErrorFormat(GetFormattedLog(type, log), args);
        }

        public static void LogException(ELogType type, Exception e)
        {
            if (!LogEnabled(type)) return;

            Debug.LogException(e);
        }


        #region ========================== Log Debug ============================

        public static void Log_Debug(ELogType type, string log = "", params object[] args)
        {
#if UNITY_EDITOR
            System.Reflection.MethodBase methodBase = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod();
            Log(type, string.Format("[Debug][{0}.{1}] {2}", methodBase.ReflectedType.Name, methodBase.Name, log), args);
#endif
        }

        public static void LogWarning_Debug(ELogType type, string log = "", params object[] args)
        {
#if UNITY_EDITOR
            System.Reflection.MethodBase methodBase = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod();
            LogWarning(type, string.Format("[Debug][{0}.{1}] {2}", methodBase.ReflectedType.Name, methodBase.Name, log), args);
#endif
        }

        public static void LogError_Debug(ELogType type, string log = "", params object[] args)
        {
#if UNITY_EDITOR
            System.Reflection.MethodBase methodBase = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod();
            LogError(type, string.Format("[Debug][{0}.{1}] {2}", methodBase.ReflectedType.Name, methodBase.Name, log), args);
#endif
        }

        #endregion Log Debug

    }
}
