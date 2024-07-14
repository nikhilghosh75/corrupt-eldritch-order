/* Created by Billy Bostick '20
 * Custom tab for displaying dynamic lines of text
*/

using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace WSoft.UI
{
    public class TextTab : Tab
    {
        [SerializeField]
        public GameObject TextPrefab;

        protected TextMeshProUGUI[] TextMeshes;

        /// <summary>
        /// Used to create/disable TextMeshes to get to the appropriate number
        /// </summary>
        /// <param name="Num"> number of meshes desired </param>
        protected void SetNumTextMeshes(int Num)
        {
            if (this.TextMeshes == null)
            {
                this.RefreshTextRefs();
            }
            // disable the unneeded ones
            for (int i = Num; i < this.TextMeshes.Length; ++i)
            {
                TextMeshes[i].gameObject.SetActive(false);
            }

            // if we need more, create them
            if (TextPrefab != null)
            {
                for (int i = this.TextMeshes.Length; i < Num; ++i)
                {
                    Instantiate(this.TextPrefab, this.PageContent.transform);
                }
                this.RefreshTextRefs();
            }
        }

        /// <summary>
        /// Checks for new TextMesh children
        /// </summary>
        protected void RefreshTextRefs()
        {
            this.TextMeshes = this.PageContent.GetComponentsInChildren<TextMeshProUGUI>();
        }

        /// <summary>
        /// sets the TextMeshes to have the corresponding strings
        /// </summary>
        /// <param name="DisplayTexts"> The strings that will be displayed on the ContentInstance object's text objects </param>
        public void SetText(List<string> DisplayTexts)
        {
            this.SetNumTextMeshes(DisplayTexts.Count);
            for (int i = 0; i < DisplayTexts.Count; ++i)
            {
                if (i < this.TextMeshes.Length)
                {
                    this.TextMeshes[i].gameObject.SetActive(true);
                    this.TextMeshes[i].text = DisplayTexts[i];
                }
            }
        }
    }
}