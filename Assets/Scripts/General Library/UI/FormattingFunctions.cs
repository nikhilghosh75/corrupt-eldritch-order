/*
 * Formatting Functions is a class meant to carry a bunch of static functions that may need to be called by any class s.
 * Note that since all functions are static, this is to be called like GameFunctions.TimeToText(...);
 * Written by Nikhil Ghosh '24
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSoft.UI
{
    public class FormattingFunctions : MonoBehaviour
    {
        /*
         * Returns a string of the formatted time. 
         * Note that time is in seconds and millisecondsDisplayed determines if milliseconds will be shown on the output (01:12 vs 01:12:025)
         * For instance, TimeToText(125.5469, true) = "2:05.546" and TimeToText(125.5469) = "2:05"
         * Written by Nikhil Ghosh
         */
        public static string TimeToText(float time, bool millisecondsDisplayed = false)
        {
            string result = "";

            int hours = (int)Mathf.Floor(time / 3600f);
            if (hours > 0)
            {
                result += hours + ":";
            }

            int minutes = (int)Mathf.Floor(time / 60f);
            if (minutes >= 10)
            {
                result += minutes;
            }
            else
            {
                result += "0" + minutes;
            }

            result += ":";

            int seconds = ((int)Mathf.Floor(time)) % 60;
            if (seconds >= 10)
            {
                result += seconds;
            }
            else
            {
                result += "0" + seconds;
            }

            if (millisecondsDisplayed)
            {
                int milliseconds = (int)Mathf.Floor((time - Mathf.Floor(time)) * 1000);
                result += ".";
                if (milliseconds >= 100)
                {
                    result += milliseconds;
                }
                else if (milliseconds >= 10)
                {
                    result += "0" + milliseconds;
                }
                else
                {
                    result += "00" + milliseconds;
                }
            }

            return result;
        }

        /*
         * Returns a string that gives a number with commas appropriately placed. Works for positive and negative integers.
         * FOr instance, NumberWithCommas(12345) = "12,345".
         * Written by Nikhil Ghosh
         */

        public static string NumberWithCommas(int n)
        {
            string result = "";
            if (n < 0)
            {
                result += "-";
            }
            if (Mathf.Abs(n) < 1000)
            {
                result += n;
            }
            else
            {
                n = (int)(Mathf.Abs(n) + 0.001);
                int digits = (int)Mathf.Log(n, 10) + 1;
                string nString = "" + n;
                for (int i = 0; i < digits; i++)
                {
                    int j = digits - i - 1;
                    if (j % 3 == 2 && i != 0)
                    {
                        result += ",";
                    }
                    result += nString[i];
                }
            }
            return result;
        }

        /*
         * Same as above, but works with floating point numbers instead of integer values.
         * Digits represents the number of digits after the decimal point.
         * For Instance, NumberWithCommas(12345.6789, 2) = "12,345.68".
         */

        public static string NumberWithCommas(float n, int digits)
        {
            string result = NumberWithCommas((int)n);
            if (digits <= 0)
            {
                return result;
            }
            float decimalValue = n - Mathf.Floor(n);
            if (decimalValue < Mathf.Pow(10, -digits))
            {
                return result;
            }
            string formatString = "#.";
            for (int i = 0; i < digits; i++)
            {
                formatString += "0";
            }
            return result + decimalValue.ToString(formatString);
        }
    }
}
