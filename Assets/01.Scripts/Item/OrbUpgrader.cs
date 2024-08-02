using UnityEngine;
using UnityEngine.UI;

public class OrbUpgrader : MonoBehaviour
{
    public Button[] upgradeButtons;
    public GameObject popUp_OrbList;

    public Image prefab_Orb;
    public Image prefab_RareOrb;
    public Image prefab_EpicOrb;

    public ItemSlotUI slotPrefab;
    public ItemSlotUI upgradeSlot;


    private void OnEnable()
    {


        upgradeButtons = GetComponentsInChildren<Button>();

        foreach(Button button in upgradeButtons )
        {
            button.onClick.AddListener(PopUpOrbList);
        }

    }

    private void OnDisable()
    {
    }

    private void OnTransformChildrenChanged()
    {

    }

    private void UpgradeOrb()
    {

    }

    private void PopUpOrbList()
    {
        popUp_OrbList.SetActive(true);
    }
}
