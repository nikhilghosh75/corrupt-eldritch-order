/* Stores a list of control prompts.
 * @Zhenyuan Zhang '21
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WSoft.Input.ControlPrompt
{
    [CreateAssetMenu(fileName = "New Control Prompt Database", menuName = "WSoft/Control Prompt/Database")]
    public class ControlPromptDatabase : ScriptableObject, IEnumerable<ControlPrompt>
    {
        [SerializeField] List<ControlPrompt> controlPrompts;
        public int size { get => controlPrompts.Count; }

        public ControlPrompt Find(string action)
        {
            return controlPrompts.Find(control => control.actionName == action);
        }

        public int FindIndex(string action)
        {
            return controlPrompts.FindIndex(control => control.actionName == action);
        }

        public IEnumerator<ControlPrompt> GetEnumerator()
        {
            return controlPrompts.GetEnumerator() as IEnumerator<ControlPrompt>;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}