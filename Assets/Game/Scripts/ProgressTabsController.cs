using UnityEngine;

public class ProgressTabsController : MonoBehaviour
{
    [Header("Содержимое вкладок")]
    [SerializeField] private GameObject dailyRewardsContent;
    [SerializeField] private GameObject dailyQuestsContent;

    [Header("Подчеркивания вкладок")]
    [SerializeField] private GameObject rewardsUnderline;
    [SerializeField] private GameObject questsUnderline;

    private void Start()
    {
        ShowRewards();
    }

    public void ShowRewards()
    {
        dailyRewardsContent.SetActive(true);
        dailyQuestsContent.SetActive(false);

        rewardsUnderline.SetActive(true);
        questsUnderline.SetActive(false);
    }

    public void ShowQuests()
    {
        dailyRewardsContent.SetActive(false);
        dailyQuestsContent.SetActive(true);

        rewardsUnderline.SetActive(false);
        questsUnderline.SetActive(true);
    }
}
