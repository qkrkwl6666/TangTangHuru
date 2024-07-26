using UnityEngine;

public class MainUIManager : MonoBehaviour
{
    public void OnStartButton()
    {
        GameManager.Instance.StartGame();
    }

    // Todo : юс╫ц

    private void Awake()
    {
        GameManager.Instance.currentWeapon = "OneSword";
    }

    public void OnOneSwordButton(bool isOn)
    {
        GameManager.Instance.currentWeapon = "OneSword";
    }

    public void OnAxeButton(bool isOn)
    {
        GameManager.Instance.currentWeapon = "Axe";
    }

    public void OnBowButton(bool isOn)
    {
        GameManager.Instance.currentWeapon = "Bow";
    }

    public void OnCrossbowsButton(bool isOn)
    {
        GameManager.Instance.currentWeapon = "Crossbow";
    }

    public void OnWandsButton(bool isOn)
    {
        GameManager.Instance.currentWeapon = "Wand";
    }

    public void OnStaffButton(bool isOn)
    {
        GameManager.Instance.currentWeapon = "Staff";
    }
}
