using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace tools
{
    public class DOTweenDelayedCall
    {
        private static List<DelayedCall> _runningDelayedCalls = new List<DelayedCall>();


        // ----------------------------------------------------------------------------------
        // ========================== Creating Delayed Calls ============================
        // ----------------------------------------------------------------------------------


        /// <summary>
        /// Creates a delayed call using DOTween
        /// </summary>
        /// <param name="callback">Callback to be invoked</param>
        /// <param name="interval">Interval for this call in seconds</param>
        /// <returns></returns>
        public static DelayedCall DelayedCall(TweenCallback callback, float interval)
        {
            DelayedCall delayedCall = new DelayedCall(interval, callback);

            // Save sequence if interval is greater than 0
            if (delayedCall.Interval > 0)
                _runningDelayedCalls.Add(delayedCall);

            return delayedCall;
        }


        // ----------------------------------------------------------------------------------
        // ========================== Removing Delayed Calls ============================
        // ----------------------------------------------------------------------------------


        /// <summary>
        /// Kills all DOTween delayed calls that match the specified callback
        /// </summary>
        /// <param name="callback">Callback to be searched</param>
        /// <returns>True if any delayedcalls were killed, False otherwise</returns>
        public static bool KillDelayedCall(TweenCallback callback)
        {
            bool killed = false;
            for (int i = _runningDelayedCalls.Count - 1; i >= 0; i--)
            {
                if (_runningDelayedCalls[i].Callback == callback)
                {
                    killed |= KillDelayedCall(_runningDelayedCalls[i]);
                }
            }

            return killed;
        }

        /// <summary>
        /// Kills all DOTween delayed calls that match the specified callback and interval
        /// </summary>
        /// <param name="callback">Callback to be searched</param>
        /// <param name="interval">Interval to be searched</param>
        /// <returns>True if any delayedcalls were killed, False otherwise</returns>
        public static bool KillDelayedCall(TweenCallback callback, float interval)
        {
            bool killed = false;
            for (int i = _runningDelayedCalls.Count - 1; i >= 0; i--)
            {
                if (_runningDelayedCalls[i].Callback == callback && _runningDelayedCalls[i].Interval == interval)
                {
                    killed |= KillDelayedCall(_runningDelayedCalls[i]);
                }
            }

            return killed;
        }

        /// <summary>
        /// Kills all DOTween delayed calls that match the specified parameter
        /// </summary>
        /// <param name="delayedCall">DelayedCall to be killed</param>
        /// <returns>True if any delayedcalls were killed, False otherwise</returns>
        public static bool KillDelayedCall(DelayedCall delayedCall)
        {
            _runningDelayedCalls.Remove(delayedCall);
            return delayedCall.Kill();
        }
    }

    public class DelayedCall
    {
        public Sequence Sequence { get; private set; }
        public float Interval { get; private set; }
        public TweenCallback Callback { get; private set; }

        public DelayedCall(float interval, TweenCallback callback)
        {
            interval = Mathf.Max(interval, 0);

            Interval = interval;
            Callback = callback;

            Sequence = DOTween.Sequence().AppendInterval(interval).AppendCallback(OnDelayedCallFinished);
        }

        private void OnDelayedCallFinished()
        {
            Callback?.Invoke();
            DOTweenDelayedCall.KillDelayedCall(this);
        }

        public bool Kill()
        {
            if (Sequence == null) return false;

            Sequence?.Kill();
            Sequence = null;

            DOTweenDelayedCall.KillDelayedCall(this);
            return true;
        }
    }
}
