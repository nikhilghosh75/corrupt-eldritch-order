using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.Combat;

public class BossTrigger : MonoBehaviour
{
    public List<LevelExit> levelExits;

    public float exitTransitionTime;
    public float exitStayTime;
    public float bossTransitionTime;
    public float bossStayTime;
    public float bossCameraSize;
    public GameObject bossRoomCenter;

    [Header("Portal")]
    public GameObject portal;

    bool bossTransitionComplete = false;

    void Start()
    {
        portal.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController player) && !bossTransitionComplete)
        {
            StartCoroutine(PerformBossTransition(player));
        }
    }

    IEnumerator PerformBossTransition(PlayerController player)
    {
        CinemachineVirtualCamera virtualCamera = (CinemachineVirtualCamera)CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera;

        // Freeze Player
        player.DisablePlayer();

        // Get camera body settings
        var body = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        float prevXDamping = body.m_XDamping;
        float prevYDamping = body.m_YDamping;

        // Show the level exit
        LevelExit currentLevelExit = levelExits.Find(exit => exit.removed);
        body.m_XDamping = 2.5f;
        body.m_YDamping = 2.5f;
        body.m_UnlimitedSoftZone = true;
        virtualCamera.Follow = currentLevelExit.transform;

        yield return new WaitForSeconds(exitTransitionTime);

        currentLevelExit.Add();

        yield return new WaitForSeconds(exitStayTime);

        virtualCamera.Follow = bossRoomCenter.transform;

        yield return new WaitForSeconds(bossTransitionTime);

        float originalSize = virtualCamera.m_Lens.OrthographicSize;

        for(float t = 0; t < bossStayTime; t += Time.deltaTime)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(originalSize, bossCameraSize, t / bossStayTime);
            yield return null;
        }

        virtualCamera.m_Lens.OrthographicSize = bossCameraSize;

        BossHealth bossHealth = FindFirstObjectByType<BossHealth>(FindObjectsInactive.Include);
        bossHealth.gameObject.SetActive(true);
        bossHealth.ActivateHealthBar();

        BossBehavior boss = FindFirstObjectByType<BossBehavior>();
        boss.SelectRandomAttacks();
        boss.Activate();

        boss.GetComponent<HealthSystem>().events.OnDeath.AddListener(OpenPortal);

        player.EnablePlayer();
        bossTransitionComplete = true;
    }

    void OpenPortal()
    {
        portal.SetActive(true);
    }
}
