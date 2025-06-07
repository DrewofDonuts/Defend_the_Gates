using UnityEngine;

namespace Etheral
{
    public class GamesListPanel : MonoBehaviour
    {
        [SerializeField] LoadGameButton buttonPrefab;


        void Start()
        {
            DisplaySavedGames();
        }
        
        public void DisplaySavedGames()
        {
            foreach (var gameName in GameManager.Instance.AllGameNames)
            {
                var button = Instantiate(buttonPrefab, transform);
                button.SetGameName(gameName);
            }
        }
    }
}