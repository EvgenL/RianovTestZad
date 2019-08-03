using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Planet : MonoBehaviour
    {
        [SerializeField] private Image Visual;
        [SerializeField] private TextMeshProUGUI Txt;

        public bool Hidden { get; private set; }

        public void Draw(Sprite sprite, int rank)
        {
            Visual.sprite = sprite;
            Txt.text = rank.ToString();
            HideRank();
            Hidden = false;
        }

        public void HideRank()
        {
            Txt.enabled = false;
        }
        public void DisplayRank()
        {
            Txt.enabled = true;
        }

        public void HidePlanet()
        {
            Hidden = true;
            gameObject.SetActive(false);
        }

        public void ShowPlanet()
        {
            Hidden = false;
            gameObject.SetActive(true);
        }
    }
}
