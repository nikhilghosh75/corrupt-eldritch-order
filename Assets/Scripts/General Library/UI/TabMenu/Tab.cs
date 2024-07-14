/* Created by Billy Bostick '20
 * Basic implementation of a tab that can open and close its pagecontent
*/

using TMPro;
using UnityEngine.Events;
using UnityEngine;

namespace WSoft.UI
{
    public class Tab : MonoBehaviour
    {
        protected Animator AnimatorComponent;

        public UnityEvent OpenEvent;
        public UnityEvent CloseEvent;

        protected TextMeshProUGUI TabName;
        protected string          TabNameString;

        public int TabIndex { get; set; }

        [SerializeField]
        public GameObject PageContent;

        protected virtual void Awake()
        {
            // this is provided for ease of use and standardization
            // the following functions will be called when the event fires
            this.OpenEvent.AddListener(OnOpenEvent);
            this.CloseEvent.AddListener(OnCloseEvent);
            if (PageContent)
            {
                this.PageContent.SetActive(false);
            }
            this.AnimatorComponent = GetComponent<Animator>();

            this.TabName = GetComponentInChildren<TextMeshProUGUI>();
            if (this.TabName)
            { 
                this.TabName.text = TabNameString;
            }
        }

        /// <summary>
        /// Called on OpenEvent
        /// override this to add functionality
        /// </summary>
        protected virtual void OnOpenEvent()
        {
            if (this.PageContent != null)
            {
                this.PageContent.SetActive(true);
            }
            if (this.AnimatorComponent != null)
            {
                this.AnimatorComponent.SetBool("Open", true);
            }
        }

        /// <summary>
        /// Called on CloseEvent
        /// override this to add functionality
        /// </summary>
        protected virtual void OnCloseEvent()
        {
            if (this.PageContent != null)
            {
                this.PageContent.SetActive(false);
            }
            if (this.AnimatorComponent != null)
            {
                this.AnimatorComponent.SetBool("Open", false);
            }
        }

        public void SetTabName(string Name)
        {
            this.TabNameString = Name;
            if (this.TabName != null)
            {
                this.TabName.text = Name;
            }
        }
    }
}