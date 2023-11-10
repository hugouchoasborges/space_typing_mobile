using System;
using System.Collections.Generic;
using UnityEngine;

namespace tools
{
    public static class LayerMaskExtensions
    {
        public static ICollection<int> GetMaskIndexes(this LayerMask layerMask)
        {
            string binaryMask = Convert.ToString(layerMask.value, 2);

            List<int> powersOfTwo = new List<int>();
            int length = binaryMask.Length;

            for (int i = 0; i < length; i++)
            {
                if (binaryMask[i] == '1')
                {
                    int power = length - i - 1;
                    //int value = (int)Math.Pow(2, power);
                    powersOfTwo.Add(power);
                }
            }

            return powersOfTwo;
        }
    }
}
