using UnityEngine;
using UnityEngine.SceneManagement;

namespace CardGame
{
    public class GameRestart : MonoBehaviour
    {
        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
        }
    }
}
