using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class MessageAlarm : MonoBehaviour, IWindowUI
{
    public GameObject alarmUI;
    public List<CharacterSO> characters = new List<CharacterSO>();
    public GameObject Portrait;
    public TMPro.TextMeshProUGUI RelationshipText;

    private int lastCheckedFav = -1;
    private MemberType lastSelectedIdol;

    private Queue<(CharacterSO character, int fav)> alarmQueue = new Queue<(CharacterSO, int)>();
    private bool isProcessing = false;

    private void Start()
    {
        if (UserDataManager.Instance != null)
        {
            lastSelectedIdol = UserDataManager.Instance.GetSelectedIdol();
            lastCheckedFav = UserDataManager.Instance.GetFavorability();
            InitializeAllCharacters();
        }
        UserDataManager.Instance.SetSelectedIdol(lastSelectedIdol);
        StartCoroutine(CheckIdolFav());
    }
    private void InitializeAllCharacters()
    {
        foreach (var character in characters)
        {
            UserDataManager.Instance.SetSelectedIdol(character.Member_ID);
            int currentFav = UserDataManager.Instance.GetFavorability();
            DataManager.Instance.SetMaxBranch(character.ID, currentFav);
            character.favorability = currentFav;

            alarmQueue.Enqueue((character, currentFav));
            if (isProcessing == false)
            {
                StartCoroutine(AlarmEffect());
            }
        }
        DataManager.Instance.TotalAlarmUI();
    }

    private void UpdateMaxBranch()
    {
        MemberType currentIdol = UserDataManager.Instance.GetSelectedIdol();
        int currentFav = UserDataManager.Instance.GetFavorability();

        CharacterSO target = characters.Find(x => x.Member_ID == currentIdol);
        if (target != null)
        {
            CharacterDialogueData dialogueData = DataManager.Instance.GetDialogueByID(target.ID);

            if (dialogueData != null && dialogueData.dialogueData != null)
            {
                int maxAvailableBranch = dialogueData.dialogueData.branches.Count - 1;

                int safeBranchIndex = Mathf.Min(currentFav, maxAvailableBranch);

                DataManager.Instance.SetMaxBranch(target.ID, safeBranchIndex);
            }
        }
        DataManager.Instance.TotalAlarmUI();
    }


    private IEnumerator CheckIdolFav()
    {
        while (true)
        {
            if (UserDataManager.Instance == null || DataManager.Instance == null)
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            int currentFav = UserDataManager.Instance.GetFavorability();
            MemberType currentIdol = UserDataManager.Instance.GetSelectedIdol();

            if (currentFav > lastCheckedFav)
            {
                CharacterSO target = characters.Find(x => x.Member_ID == currentIdol);
                if (target != null)
                {
                    target.favorability = currentFav;

                    CharacterDialogueData dialogueData = DataManager.Instance.GetDialogueByID(target.ID);
                    int safeBranchIndex = currentFav;

                    if (dialogueData != null && dialogueData.dialogueData != null)
                    {
                        int maxAvailableBranch = dialogueData.dialogueData.branches.Count - 1;

                        if (currentFav <= maxAvailableBranch)
                        {
                            DataManager.Instance.SetMaxBranch(target.ID, currentFav);
                            DataManager.Instance.TotalAlarmUI();

                            alarmQueue.Enqueue((target, target.favorability));
                            if (isProcessing == false)
                            {
                                StartCoroutine(AlarmEffect());
                            }
                        }
                        else
                        {
                            DataManager.Instance.SetMaxBranch(target.ID, maxAvailableBranch);
                        }
                    }
                }
                lastCheckedFav = currentFav;
                
            }

            if (currentIdol != lastSelectedIdol)
            {
                lastSelectedIdol = currentIdol;
                lastCheckedFav = UserDataManager.Instance.GetFavorability();
                UpdateMaxBranch();
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator AlarmEffect()
    {
        while(alarmQueue.Count > 0)
        {
            isProcessing = true;

            var (character, fav) = alarmQueue.Dequeue();

            if (Portrait != null && Portrait.GetComponent<Image>() != null)
            {
                Portrait.GetComponent<Image>().sprite = character.portrait;
            }

            if (RelationshipText != null)
            {
                RelationshipText.text = fav.ToString();
            }

            DataManager.Instance.SetMessageAlarm(character.ID, true);

            Show();
            yield return new WaitForSeconds(1.0f);
            Hide();
            yield return new WaitForSeconds(0.5f);
        }
        isProcessing = false;
    }

    #region IWindowUI
    public void Hide() => alarmUI.SetActive(false);
    public void Show() => alarmUI.SetActive(true);
    #endregion
}