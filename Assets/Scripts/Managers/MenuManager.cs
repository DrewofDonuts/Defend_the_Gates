using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Etheral
{
    public class MenuManager : MonoBehaviour
    {
        [field: SerializeField] public Menu MainMenuPrefab { get; private set; }
        [field: SerializeField] public Menu ControlsScreenPrefab { get; private set; }
        [field: SerializeField] public Menu LoadGamePrefab { get; private set; }
        
        
        // [field: SerializeField] public Menu QuitPrefab { get; private set; }

        [SerializeField] Transform _menuParent;

        Stack<Menu> _menuStack = new();

        //refers to the single instance of the MenuManager
        static MenuManager _instance;

        // ReSharper disable once ConvertToAutoProperty
        public static MenuManager Instance => _instance;

        void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                InitializeMenus();
            }
        }

        void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }

        void InitializeMenus()
        {
            if (_menuParent == null)
            {
                //Create a new GameObject that will receive the same transform as the Gamemanager
                GameObject menuParentObject = new GameObject("Menus");
                _menuParent = menuParentObject.transform;
            }

            //EVERY MENU ITEM NEEDS TO GO HERE
            Menu[] menuPrefabs = { MainMenuPrefab, ControlsScreenPrefab, LoadGamePrefab };

            //Instantiate and disable all the menus in array, unless the prefab is the MainMenuPrefab.
            foreach (var prefab in menuPrefabs)
            {
                if (prefab != null)
                {
                    var menuInstance = Instantiate(prefab, _menuParent);

                    //Main Menu will be active
                    if (prefab != MainMenuPrefab)
                    {
                        menuInstance.gameObject.SetActive(false);
                    }
                    else
                    {
                        //if prefab is not menuInstance, then pass to OpenMenu
                        //if it's not the main menu, then will pass to OpenMenu and start disabled
                        OpenMenu(menuInstance);
                    }
                }
            }
        }

        public void OpenMenu(Menu menuInstance)
        {
            if (menuInstance == null)
            {
                Debug.LogWarning("MENU MANAGER OpenMenu ERROR: Invalid menu");
                return;
            }

            //disable all the menus found in the stack (which initially won't include Main Menu)
            //Then below we will activate the menuInstance that was passed to this method
            if (_menuStack.Count > 0)
            {
                foreach (var menu in _menuStack)
                {
                    menu.gameObject.SetActive(false);
                }
            }

            //at this point, we've just started the game and the stack is empty
            //or, we have menus in the stack and everything is disabled

            //activate the one menu we want in our UI
            menuInstance.gameObject.SetActive(true);

            //make sure it's at the top of the stack - this way when we hit back, it's the first OUT (LIFO)
            _menuStack.Push(menuInstance);
            
            ChangeSelectedEvent(menuInstance.FirstButton);

        }

        public void ChangeSelectedEvent(Button firstButton)
        {
            // FindObjectOfType<EventSystem>().firstSelectedGameObject = firstButton;
            
            firstButton.Select();

        }

        public void CloseMenu()
        {
            if (_menuStack.Count == 0)
            {
                Debug.LogWarning("MENU MANAGER CloseMenu ERROR: No menus in stack!");
                return;
            }

            //Pop returns the top stack and removes it
            Menu _topMenu = _menuStack.Pop();
            _topMenu.gameObject.SetActive(false);

            if (_menuStack.Count > 0)
            {
                //shows the next item in the stack without removing it
                Menu _nextMenu = _menuStack.Peek();
                _nextMenu.gameObject.SetActive(true);
                ChangeSelectedEvent(_nextMenu.FirstButton);
            }
        }
    }
}