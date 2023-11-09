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
        /// <param name="loops">Number of cycles to play (-1 for infinite - will be converted to 1 in case the tween is nested in a Sequence</param>
        /// <param name="loopType">Loop behaviour type (default: LoopType.Restart)</param>
        /// <returns></returns>
        public static DelayedCall DelayedCall(TweenCallback callback, float interval, int loops = 0, LoopType loopType = LoopType.Restart)
        {
            DelayedCall delayedCall = new DelayedCall(interval, callback, loops: loops, loopType: loopType);

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
        public int Loops { get; private set; }
        public LoopType LoopType { get; private set; }

        public DelayedCall(float interval, TweenCallback callback, int loops = 0, LoopType loopType = LoopType.Restart)
        {
            interval = Mathf.Max(interval, 0);

            Interval = interval;
            Callback = callback;
            Loops = loops;
            LoopType = loopType;

            Sequence = DOTween.Sequence().AppendInterval(interval).AppendCallback(OnDelayedCallFinished);
            if (loops != 0)
                Sequence.SetLoops(loops, loopType);
        }

        public void SetPaused(bool paused)
        {
            if (paused)
                Sequence.Pause();
            else
                Sequence.Play();
        }

        private void OnDelayedCallFinished()
        {
            Callback?.Invoke();

            if (Loops > 0) Loops--;
            if (Loops == 0)
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
