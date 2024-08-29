using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RuleBookUI : MonoBehaviour
{
    public List<Sprite> attackSprites = new ();
    public List<Sprite> coreSprites = new ();
    public List<Sprite> equipSprites = new ();
    public List<Sprite> createSprites = new ();

    public Button ruleBookButton;
    public Button cencelButton;

    public Button prevButton;
    public Button nextButton;

    public List<Button> buttons = new ();

    public Image currentImage;
    public TextMeshProUGUI currentDescText;
    public TextMeshProUGUI currentShowText;

    public List<string> ruleDescStrings;

    private int currentIndex = 0;
    private int maxIndex = 0;
    private RuleBook currentRuleBook = RuleBook.Battle;

    private void OnEnable()
    {
        if(ruleDescStrings == null || ruleDescStrings.Count == 0)
        {
            ruleDescStrings.Clear();
            ruleDescStrings = DataTableManager.Instance.Get<StringTable>(DataTableManager.String).GetRuleBookTexts();
        }

        currentIndex = 0;
        maxIndex = attackSprites.Count;
        currentRuleBook = RuleBook.Battle;

        SetUI();
    }

    private void Awake()
    {
        ruleBookButton.onClick.AddListener(() => { Defines.DotweenScaleActiveTrue(gameObject); });
        cencelButton.onClick.AddListener(() => { Defines.DotweenScaleActiveFalse(gameObject); });

        buttons[(int)RuleBook.Battle].onClick.AddListener(BattleButton);
        buttons[(int)RuleBook.Core].onClick.AddListener(CoreButton);
        buttons[(int)RuleBook.Equip].onClick.AddListener(EquipButton);
        buttons[(int)RuleBook.Create].onClick.AddListener(CreateButton);

        prevButton.onClick.AddListener(PrevButton);
        nextButton.onClick.AddListener(NextButton);

        gameObject.SetActive(false);
    }

    public void BattleButton()
    {
        currentIndex = 0;
        maxIndex = attackSprites.Count;
        currentRuleBook = RuleBook.Battle;

        SetUI();
    }

    public void CoreButton()
    {
        currentIndex = 0;
        maxIndex = coreSprites.Count;
        currentRuleBook = RuleBook.Core;

        SetUI();
    }

    public void EquipButton()
    {
        currentIndex = 0;
        maxIndex = equipSprites.Count;
        currentRuleBook = RuleBook.Equip;

        SetUI();
    }
    public void CreateButton()
    {
        currentIndex = 0;
        maxIndex = createSprites.Count;
        currentRuleBook = RuleBook.Create;

        SetUI();
    }

    public void NextButton()
    {
        currentIndex++;

        if (currentIndex >= maxIndex)
        {
            currentIndex = 0;
            SetUI();
            return;
        }

        SetUI();
    }

    public void PrevButton()
    {
        currentIndex--;

        if (currentIndex < 0)
        {
            currentIndex = maxIndex - 1;
            SetUI();
            return;
        }

        SetUI();
    }

    public void SetUI()
    {
        switch(currentRuleBook)
        {
            case RuleBook.Battle:
                currentImage.sprite = attackSprites[currentIndex];
                currentShowText.text = $"{currentIndex + 1}/{attackSprites.Count}";
                currentDescText.text = ruleDescStrings[currentIndex];
                break;
            case RuleBook.Core:
                currentImage.sprite = coreSprites[currentIndex];
                currentShowText.text = $"{currentIndex + 1}/{coreSprites.Count}";
                currentDescText.text = ruleDescStrings[currentIndex + 8];
                break;
            case RuleBook.Equip:
                currentImage.sprite = equipSprites[currentIndex];
                currentShowText.text = $"{currentIndex + 1}/{equipSprites.Count}";
                currentDescText.text = ruleDescStrings[currentIndex + 10];
                break;
            case RuleBook.Create:
                currentImage.sprite = createSprites[currentIndex];
                currentShowText.text = $"{currentIndex + 1}/{createSprites.Count}";
                currentDescText.text = ruleDescStrings[currentIndex + 14];
                break;
        }

    }

}

public enum RuleBook
{
    Battle,
    Core,
    Equip,
    Create,
}
