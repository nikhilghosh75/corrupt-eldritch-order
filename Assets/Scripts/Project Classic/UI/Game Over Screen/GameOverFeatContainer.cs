using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameOverFeatContainer : MonoBehaviour
{
    class FeatProgressInfo
    {
        public BaseFeat feat;
        public float startOfRunProgress;
        public float endOfRunProgress;
    }

    public GameObject elementPrefab;
    public TMP_Text additionalFeatProgressText;

    [Header("Timing")]
    public float timeBetweenFeats;
    public float progressShownTime;

    public int maxFeats = 4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public IEnumerator ShowFeats()
    {
        additionalFeatProgressText.text = "";
        List<FeatProgressInfo> feats = ComputeFeatsToDisplay();

        for (int i = 0; i < feats.Count; i++)
        {
            GameObject spawnedFeatElement = Instantiate(elementPrefab, transform);
            GameOverFeatElement featElement = spawnedFeatElement.GetComponent<GameOverFeatElement>();
            featElement.descriptionText.text = feats[i].feat.Description;
            featElement.progressText.text = feats[i].feat.GetProgressTextAtProgress(feats[i].startOfRunProgress);
            featElement.rewardText.text = feats[i].feat.RewardString();
            featElement.progressImage.fillAmount = feats[i].startOfRunProgress;

            for(float t = 0; t < progressShownTime; t += Time.unscaledDeltaTime)
            {
                float progress = Mathf.Lerp(feats[i].startOfRunProgress, feats[i].endOfRunProgress, t / progressShownTime);
                featElement.progressImage.fillAmount = progress;
                featElement.progressText.text = feats[i].feat.GetProgressTextAtProgress(progress);
                yield return null;
            }

            featElement.progressText.text = feats[i].feat.GetProgressText();
            featElement.progressImage.fillAmount = feats[i].endOfRunProgress;

            yield return new WaitForSecondsRealtime(timeBetweenFeats);
        }

        int featsNotShown = GetNumberOfFeatsWithProgress() - maxFeats;
        if (featsNotShown > 0)
        {
            additionalFeatProgressText.text = "+ " + featsNotShown.ToString() + " more";
        }
    }

    List<FeatProgressInfo> ComputeFeatsToDisplay()
    {
        List<FeatProgressInfo> featProgressInfos = new List<FeatProgressInfo>();
        foreach (BaseFeat feat in FeatManager.instance.feats)
        {
            FeatProgressInfo progressInfo = new FeatProgressInfo();
            progressInfo.feat = feat;
            progressInfo.startOfRunProgress = RunManager.Instance.featProgressAtStartOfRun[feat.Name];
            progressInfo.endOfRunProgress = feat.GetProgress();

            if (progressInfo.startOfRunProgress > 1f)
                progressInfo.startOfRunProgress = 1f;
            if (progressInfo.endOfRunProgress > 1f)
                progressInfo.endOfRunProgress = 1f;

            featProgressInfos.Add(progressInfo);
        }

        featProgressInfos = featProgressInfos.Where((feat) => feat.endOfRunProgress - feat.startOfRunProgress > 0.005f).ToList();

        featProgressInfos.Sort((a, b) => {
            float aProgressDifference = a.endOfRunProgress - b.endOfRunProgress;
            float bProgressDifference = b.endOfRunProgress - b.startOfRunProgress;

            if (Mathf.Abs(aProgressDifference - bProgressDifference) > 0.04f)
            {
                return aProgressDifference > bProgressDifference ? -1 : 1;
            }

            return a.feat.priority > b.feat.priority ? -1 : 1;
        });

        return featProgressInfos.Take(maxFeats).ToList();
    }

    int GetNumberOfFeatsWithProgress()
    {
        int count = 0;
        foreach (BaseFeat feat in FeatManager.instance.feats)
        {
            float startProgress = RunManager.Instance.featProgressAtStartOfRun[feat.Name];
            float endProgress = feat.GetProgress();

            if (endProgress - startProgress > 0.02f)
            {
                count++;
            }
        }

        return count;
    }
}
