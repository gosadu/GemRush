using UnityEngine;

/// <summary>
/// Central script referencing all managers from Stages 1–17. No placeholders remain.
/// </summary>
public class ModuleReference : MonoBehaviour
{
    public static ModuleReference Instance;

    [Header("Stage1 Modules")]
    public WorldManager worldManager;
    public RealmProgressionManager realmProgressionManager;
    public SublocationManager sublocationManager;
    public SceneTransitionManager transitionManager;
    public AudioOverlayManager audioOverlayManager;
    public DailyTaskManager dailyTaskManager;
    public TutorialManager tutorialManager;
    public ProjectionSummonManager cameoManager;
    public TownHubManager townHubManager;

    [Header("Stage2 Modules")]
    public PuzzleBoardManager puzzleBoardManager;
    public GemSelector gemSelector;

    [Header("Stage3 Modules")]
    public ResourceManager resourceManager;
    public RealmExpansionManager realmExpansionManager;

    [Header("Stage4 Modules")]
    public MysticForgeManager mysticForgeManager;
    public WorkshopManager workshopManager;

    [Header("Stage5 Modules")]
    public HeroCollectionManager heroCollectionManager;
    public GachaManager gachaManager;
    public MasteryManager masteryManager;
    public PartySystemManager partySystemManager;

    [Header("Stage6 Modules")]
    public SurgeManager surgeManager;

    [Header("Stage7 Modules")]
    public GuildManager guildManager;
    public GuildBossManager guildBossManager;

    [Header("Stage8 Modules")]
    public PremiumCurrencyManager premiumCurrencyManager;
    public ShopManager shopManager;
    public SkipTokenManager skipTokenManager;
    public PassSystemManager passSystemManager;
    public PaymentIntegration paymentIntegration;

    [Header("Stage9 Modules")]
    public QuestManager questManager;

    [Header("Stage10 Modules")]
    public EncounterManager encounterManager;
    public AvatarManager avatarManager;

    [Header("Stage11 Modules")]
    public ProceduralHazardGenerator hazardGen;
    public ProceduralNodeGenerator nodeGen;
    public ProceduralQuestGenerator questGen;
    public SingleDevFeasibilityManager singleDevFeasibility;

    [Header("Stage12 Modules")]
    public HardModeManager hardModeManager;
    public LiveOpsManager liveOpsManager;
    public ExpansionManager expansionManager;

    [Header("Stage13 Modules")]
    public LeaderboardManager leaderboardManager;
    public PvPManager pvpManager;

    [Header("Stage14+")]
    public IntegrationInitializer integrationInitializer;
    public FinalGameLoopManager finalGameLoopManager;

    private void Awake()
    {
        if(Instance==null) 
        {
            Instance= this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
