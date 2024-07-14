/*
 * A library of functions that should be used with strings
 * All functions are public and modularized so that other scripts can take advantage of it.
 * 
 * @ Nikhil Ghosh '24
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace WSoft.UI
{
    public delegate void StringSetter(string s);

    public class StringFunctions
    {
        /// <summary>
        /// A Function that allows you to fill up text gradually over time
        /// </summary>
        /// <param name="text">The text that you eventually want to be displayed on screen</param>
        /// <param name="textSetter">The function that sets the text you want displayed (its a long story)</param>
        /// <param name="charactersPerSecond">The amount of characters displayed every second</param>
        /// <param name="OnDialogueEvent">The event trigged when the Dialogue ends</param>
        public static IEnumerator FillText(string text, StringSetter textSetter, float charactersPerSecond, UnityEvent OnDialogueEvent = null)
        {
            textSetter("");
            List<string> activeTags = new List<string>();

            for (int i = 0; i < text.Length; i++)
            {
                string outputText = "";
                if (text[i] == '<')
                {
                    int j = i;
                    while (text[i] != '>')
                    {
                        i++;
                    }

                    if (i < text.Length - 1)
                    {
                        i++;
                        if (text[j + 1] == '/')
                        {
                            activeTags.Remove(text.Substring(j + 2, i - j - 2));
                        }
                        else
                        {
                            activeTags.Add(text.Substring(j + 1, i - j - 2));
                        }
                    }
                    else
                    {
                        textSetter(text);
                        if (OnDialogueEvent != null)
                        {
                            OnDialogueEvent.Invoke();
                        }
                        yield break;
                    }
                }
                outputText = text.Substring(0, i + 1);
                for (int j = 0; j < activeTags.Count; j++)
                {
                    outputText += "</" + activeTags[j] + ">";
                }
                textSetter(outputText);
                yield return new WaitForSeconds(1f / charactersPerSecond);
            }
            if (OnDialogueEvent != null)
            {
                OnDialogueEvent.Invoke();
            }
            yield break;
        }

        /// <summary>
        /// Returns the width of an entire word given the word and the character table.
        /// </summary>
        /// <param name="text">A string containing the text that you want the width of.</param>
        /// <param name="characterTable">A list of all the character data in the font.</param>
        /// <returns></returns>
        public static int GetFontLength(string text, List<TMP_Character> characterTable)
        {
            int total = 0;
            for (int i = 0; i < text.Length; i++)
            {
                total += (int)characterTable[text[i]].glyph.metrics.width;
            }
            return total;
        }

        /// <summary>
        /// Returns the width of an entire word given the word and the character table.
        /// </summary>
        /// <param name="text">A string containing the text that you want the width of.</param>
        /// <param name="characterTable">A list of all the character data in the font.</param>
        /// <returns></returns>
        public static int GetFontLength(string text, List<CharacterInfo> characterTable)
        {
            int total = 0;
            for (int i = 0; i < text.Length; i++)
            {
                total += characterTable[text[i]].glyphWidth;
            }
            return total;
        }

        /// <summary>
        /// A function to turn a string into a list of words by seperating on spaces,.
        /// </summary>
        /// <param name="text">The text you wish to turn into words</param>
        /// <returns>A list of strings. For instance, "Nico Williams" returns ["Nico", "Williams"]</returns>
        public static List<string> ParseWords(string text)
        {
            List<string> words = new List<string>();

            int lastSpace = -1;

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == ' ')
                {
                    if (lastSpace != i - 1)
                    {
                        words.Add(text.Substring(lastSpace + 1, i - lastSpace - 1));
                    }
                    lastSpace = i;
                }
            }
            words.Add(text.Substring(lastSpace + 1));
            return words;
        }
    }
}
