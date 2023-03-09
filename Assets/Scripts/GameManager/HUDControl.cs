using TMPro;
using UnityEngine;

/// <summary>
/// Esta classe é atribuida ao panel HUD no canvas, e manipula seus gameObjects filhos
/// </summary>
public class HUDControl : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtPoints;

    private void Start()
    {
        txtPoints.text = "0";
    }

    /// <summary>
    /// Atualiza os textos quando o UnityEvents OnAddPoints é chamado
    /// </summary>
    /// <param name="points"></param>
    public void UpdateTextPoints()
    {
        txtPoints.text = GameData.GetInstance().GetPoint().ToString();
    }
}
