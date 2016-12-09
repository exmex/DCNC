namespace Shared
{
    /// <summary>
    /// A few commonly used math-related functions.
    /// </summary>
    public static class Math2
    {
        /// <summary>
        /// Returns min, if val is lower than min, max, if val is
        /// greater than max, or simply val.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Clamp(int min, int max, int val)
        {
            if (val < min)
                return min;
            if (val > max)
                return max;
            return val;
        }

        /// <summary>
        /// Returns min, if val is lower than min, max, if val is
        /// greater than max, or simply val.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static float Clamp(float min, float max, float val)
        {
            if (val < min)
                return min;
            if (val > max)
                return max;
            return val;
        }

        /// <summary>
        /// Returns min, if val is lower than min, max, if val is
        /// greater than max, or simply val.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static long Clamp(long min, long max, long val)
        {
            if (val < min)
                return min;
            if (val > max)
                return max;
            return val;
        }

        /// <summary>
        /// Returns true if val is between min and max (incl).
        /// </summary>
        /// <param name="val"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool Between(int val, int min, int max)
        {
            return (val >= min && val <= max);
        }

        /// <summary>
        /// Multiplies initial value with multiplicator, returns either the
        /// result or Min/MaxValue if the multiplication caused an overflow.
        /// </summary>
        /// <param name="initialValue"></param>
        /// <param name="multiplicator"></param>
        /// <returns></returns>
        public static short MultiplyChecked(short initialValue, double multiplicator)
        {
            try
            {
                checked { return (short)(initialValue * multiplicator); }
            }
            catch
            {
                if (initialValue >= 0)
                    return short.MaxValue;
                else
                    return short.MinValue;
            }
        }
        /// <summary>
        /// Multiplies initial value with multiplicator, returns either the
        /// result or Min/MaxValue if the multiplication caused an overflow.
        /// </summary>
        /// <param name="initialValue"></param>
        /// <param name="multiplicator"></param>
        /// <returns></returns>
        public static int MultiplyChecked(int initialValue, double multiplicator)
        {
            try
            {
                checked { return (int)(initialValue * multiplicator); }
            }
            catch
            {
                if (initialValue >= 0)
                    return int.MaxValue;
                else
                    return int.MinValue;
            }
        }

        /// <summary>
        /// Multiplies initial value with multiplicator, returns either the
        /// result or Min/MaxValue if the multiplication caused an overflow.
        /// </summary>
        /// <param name="initialValue"></param>
        /// <param name="multiplicator"></param>
        /// <returns></returns>
        public static long MultiplyChecked(long initialValue, double multiplicator)
        {
            try
            {
                checked { return (long)(initialValue * multiplicator); }
            }
            catch
            {
                if (initialValue >= 0)
                    return long.MaxValue;
                else
                    return long.MinValue;
            }
        }
    }
}
