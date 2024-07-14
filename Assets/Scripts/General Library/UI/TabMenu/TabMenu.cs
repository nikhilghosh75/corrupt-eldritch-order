/* Created by Billy Bostick '20
 * UI Menu with tabs. Meant to be used as a base and inherited from
*/

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace WSoft.UI
{
    public class TabMenu : MonoBehaviour
    {
        /// <summary>
        /// is this item currently visible?
        /// </summary>
        protected bool bIsVisible;

        /// <summary>
        /// Parent of all tabs and content
        /// Disable this object to disable everything
        /// </summary>
        [SerializeField]
        protected GameObject ContainerObj;

        public UnityEvent OnSetVisible;
        public UnityEvent OnSetInvisible;

        /// <summary>
        /// Parent of tabs
        /// </summary>
        [SerializeField]
        protected GameObject TabContainer;

        /// <summary>
        /// Parent of Content
        /// </summary>
        [SerializeField]
        protected GameObject ContentContainer;

        /// <summary>
        /// @Input
        /// List of the Tabs to send Events to
        /// Will include all Objects in TabPrefabs with a Tab Script
        /// </summary>
        [SerializeField]
        protected Tab[] Tabs;

        /// <summary>
        /// @Input
        /// The prefab that will be spawned on CreateTab
        /// </summary>
        [SerializeField]
        protected GameObject TabPrefab;

        /// <summary>
        /// @Input
        /// The prefab for content that will be spawned on CreateTab
        /// </summary>
        [SerializeField]
        protected GameObject ContentPrefab;

        /// <summary>
        /// The index of the active Tab
        /// </summary>
        protected int ActiveTabIndex;

        protected enum ENavigationDirection
        {
            UP,
            DOWN
        };

        // Start is called before the first frame update
        protected virtual void Awake()
        {
            this.bIsVisible = false;
            this.ContainerObj.SetActive(false);
            this.ActiveTabIndex = 0;
            this.RefreshTabRefs();

            for (int i = 0; i < this.Tabs.Length; ++i)
            {
                SetOnTabPressedDelegate(Tabs[i].gameObject, i);
            }
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if(bIsVisible)
            {
                if(Keyboard.current.leftBracketKey.wasPressedThisFrame)
                {
                    Navigate(ENavigationDirection.UP);
                }
                if(Keyboard.current.rightBracketKey.wasPressedThisFrame)
                {
                    Navigate(ENavigationDirection.DOWN);
                }
            }

            if (Keyboard.current.slashKey.wasPressedThisFrame)
            {
                ToggleVisible();
            }

            /*
            if (bIsVisible)
            {
                if (Input.GetKeyDown(KeyCode.LeftBracket))
                {
                    this.Navigate(ENavigationDirection.UP);
                }
                if (Input.GetKeyDown(KeyCode.RightBracket))
                {
                    this.Navigate(ENavigationDirection.DOWN);
                }
            }

            if (Input.GetKeyDown(KeyCode.Slash))
            {
                this.ToggleVisible();
            }
            */
        }

        protected void Navigate(ENavigationDirection navigationDirection)
        {
            int NewTabIndex = 0;
            if (navigationDirection == ENavigationDirection.UP)
            {
                NewTabIndex = TabMenu.Mod((this.ActiveTabIndex - 1), this.Tabs.Length);
            }
            else if (navigationDirection == ENavigationDirection.DOWN)
            {
                NewTabIndex = TabMenu.Mod((this.ActiveTabIndex + 1), this.Tabs.Length);
            }
            this.NavigateTo(NewTabIndex);
        }

        protected void NavigateTo(int IndexToNavigateTo)
        {
            if (this.ActiveTabIndex >= 0 && this.ActiveTabIndex < this.Tabs.Length)
            {
                if (this.Tabs[this.ActiveTabIndex] == null)
                {
                    this.RefreshTabRefs();
                    return;
                }
                else
                {
                    this.Tabs[this.ActiveTabIndex].CloseEvent.Invoke();
                }
            }
            if (IndexToNavigateTo >= 0 && IndexToNavigateTo < this.Tabs.Length)
            {
                if (this.Tabs[IndexToNavigateTo] == null)
                {
                    this.RefreshTabRefs();
                    return;
                }
                else
                {
                    this.Tabs[IndexToNavigateTo].OpenEvent.Invoke();
                    this.ActiveTabIndex = IndexToNavigateTo;
                }
            }
        }

        void ToggleVisible()
        {
            if (this.ContainerObj == null)
            {
                Debug.Log("TabMenu.cs: ContainerObj is null", this.gameObject);
                return;
            }

            if (bIsVisible)
            {
                this.ContainerObj.SetActive(false);
                this.bIsVisible = false;
                OnSetInvisible.Invoke();
                this.NavigateTo(0);
            }
            else
            {
                this.ContainerObj.SetActive(true);
                this.bIsVisible = true;
                this.OnSetVisible.Invoke();
                this.NavigateTo(0);
            }
        }

        protected void RefreshTabRefs()
        {
            this.Tabs = this.TabContainer.GetComponentsInChildren<Tab>();
            for (int i = 0; i < this.Tabs.Length; ++i)
            {
                this.Tabs[i].TabIndex = i;
            }
        }

        protected void CreateTab()
        {
            if (this.TabPrefab != null &&
                this.ContentPrefab != null &&
                this.TabContainer != null &&
                this.ContentContainer != null)
            {
                GameObject NewTab = Instantiate(this.TabPrefab, this.TabContainer.transform);
                GameObject NewContent = Instantiate(this.ContentPrefab, this.ContentContainer.transform);
                NewContent.SetActive(false);
                Tab TabComponent = NewTab.GetComponent<Tab>();
                if (TabComponent != null)
                {
                    TabComponent.PageContent = NewContent;
                }
                this.RefreshTabRefs();
                SetOnTabPressedDelegate(NewTab, TabComponent.TabIndex);
            }
        }

        public void SetNumTabs(int Num)
        {
            for (int i = Num; i < Tabs.Length; ++i)
            {
                Tabs[i].CloseEvent.Invoke();
                Tabs[i].gameObject.SetActive(false);
            }

            if (TabPrefab != null)
            {
                for (int i = Tabs.Length; i < Num; ++i)
                {
                    this.CreateTab();
                }
            }
        }

        protected void SetOnTabPressedDelegate(GameObject NewTab, int TabIndex)
        {
            Button ButtonComponent = NewTab.GetComponent<Button>();
            if (ButtonComponent != null)
            { 
                ButtonComponent.onClick.AddListener(() => this.NavigateTo(TabIndex));
            }
        }

        protected internal static int Mod(int x, int modBase)
        {
            return (x % modBase + modBase) % modBase;
        }
    }
}