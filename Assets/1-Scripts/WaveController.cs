using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
public enum MobType
{
    Thraex,
    Sagittarius,
    Hoplomachus,
    Dimachaerus
}
public enum BossType
{
    
}
[System.Serializable]
public struct Mob
{
    public GameObject Object;
    public MobType mobType;
}
[System.Serializable]
public struct Bos
{
    public GameObject Object;
    public BossType mobType;
}
[System.Serializable]
public struct MobAndCount
{
    public MobType mob;
    public int number;
}
[System.Serializable]
public struct Wave
{
    public List<MobAndCount> mobAndCounts;
    public int difficulty;
}
[System.Serializable]
public struct CategoryWave
{
    public List<Wave> waves;
    public int categoryID;
}
[System.Serializable]
public struct ArenaMatchSystem
{
    public int categoryID;
    public int totalDifficulty;
    public int totalWave;
    [HideInInspector]public List<Wave> Waves;
}

public class WaveController : Singleton<WaveController>
{
    [Header("Mobs & Bosses")]
    [SerializeField] Mob[] Mobs;
    [SerializeField] Bos[] Boss;
    [Header("Spawn Loc")]
    public SpawnLoc[] SpawnLoc;
    [Header("Waves")]
    public float WaveDelayTime;
    public Dictionary<MobType,GameObject> MobsDic;
    public Dictionary<BossType, GameObject> BossDic;
    [SerializeField]public List<CategoryWave> waves;

    public Dictionary<int, List<Wave>> wavesDic;
    [Header("Arena Match System")]
    public List<ArenaMatchSystem> arenaMatchSystems;
    public List<ArenaMatchSystem> arenaMatchSystemCopy;

    [HideInInspector] public UnityEvent ArenaMatchesFinished;
    [HideInInspector]public UnityEvent waveStarted;
    [HideInInspector]public UnityEvent waveEnded;

    [HideInInspector] public UnityEvent ArenaMatchStarted;
    [HideInInspector] public UnityEvent ArenaMatchEnded;

    ArenaMatchSystem currArenaMatch;
    int EnemyNumber;

    public Stack<int> NavmeshOrderStack;

    [HideInInspector] public GameObject trash;

    [SerializeField] private bool closeWave;

    public override void Awake()
    {
        base.Awake();

        trash = GameObject.FindGameObjectWithTag("Trash");
        ArenaMatchesFinished = new UnityEvent();
        waveStarted = new UnityEvent();
        waveEnded = new UnityEvent();
        ArenaMatchStarted = new UnityEvent();
        ArenaMatchEnded = new UnityEvent();
        waveEnded.AddListener(() => WaveBeginDelay(waveStarted));
        MobsDic = new Dictionary<MobType, GameObject>();
        wavesDic = new Dictionary<int, List<Wave>>();
        for (int i = 0; i < Mobs.Length; i++)
        {
            MobsDic.Add(Mobs[i].mobType, Mobs[i].Object);
        }
        for (int i = 0; i < Boss.Length; i++)
        {
            //BossDic.Add(Boss[i].mobType, Boss[i].Object);
        }
        foreach (var item in waves)
        {
            OrderDiffWaves(item.waves, 0, item.waves.Count - 1);
        }
        foreach (var item in waves)
        {
            wavesDic.Add(item.categoryID,new List<Wave>(item.waves));
        }
        arenaMatchSystemCopy = new List<ArenaMatchSystem>(arenaMatchSystems);
        SetArenaSystemWave();
        
        NavmeshOrderStack = new Stack<int>();
        for (int i = 0; i < 100; i++)
        {
            NavmeshOrderStack.Push(i);
        }
        ArenaMatchEnded.AddListener(Ludus.Instance.LudusShow);
        ArenaMatchEnded.AddListener(LudusAnimation.Instance.EnterLudusEvent);
        ArenaMatchEnded.AddListener(PlayerCharacterMovement.Instance.LockMovement);
        ArenaMatchEnded.AddListener(() => PlayerCharacterCombat.Instance.LockAttack(true));

        ArenaMatchStarted.AddListener(Ludus.Instance.Ludushide);
        ArenaMatchStarted.AddListener(LudusAnimation.Instance.ExitLudusEvent);
        ArenaMatchStarted.AddListener(PlayerCharacterMovement.Instance.UnlockMovement);
        ArenaMatchStarted.AddListener(() => PlayerCharacterCombat.Instance.LockAttack(false));
    }

    public override void RunEnded()
    {
        base.RunEnded();
    }

    public void RestartGame()
    {
        EnemyNumber = 0;
        wavesDic = new Dictionary<int, List<Wave>>();
        foreach (var item in waves)
        {
            wavesDic.Add(item.categoryID, new List<Wave>(item.waves));
        }
        arenaMatchSystemCopy = new List<ArenaMatchSystem>(arenaMatchSystems);
        SetArenaSystemWave();
    }

    public void ClearTrash()
    {
        foreach (Transform item in trash.transform)
        {
            GameObject.Destroy(item.gameObject);
        }
    }

    public override void RunStarted()
    {
        ArenaMatchStarted.Invoke();
        base.RunStarted();
    }

    public override void Start()
    {
        base.Start();
        if (!closeWave)
        {
            ArenaMatchStarted.AddListener(() =>
            {
                currArenaMatch = arenaMatchSystemCopy[0];
                PlayerCharacterMovement.Instance.ResetPosition();
                WaveBeginDelay(waveStarted);
            });
            waveStarted.AddListener(() =>
            {
                var Wave = GetWave();
                if (Wave != null)
                {
                    SpawnEnemy(Wave.Value);
                    arenaMatchSystemCopy[0].Waves.Remove(Wave.Value);
                }
                else
                {
                    arenaMatchSystemCopy.RemoveAt(0);
                    if (arenaMatchSystemCopy.Count > 0)
                    {
                        ArenaMatchEnded.Invoke();
                    }
                    else
                    {
                        Debug.Log("Match End");
                        ArenaMatchesFinished.Invoke();
                    }
                }
            });
        }
    }

    public void WaveBeginDelay(UnityEvent Wave)
    {
        StartCoroutine(WaveBeginDelayCor(Wave));
    }

    IEnumerator<WaitForSeconds> WaveBeginDelayCor( UnityEvent Wave)
    {
        yield return new WaitForSeconds(WaveDelayTime);
        if (Wave != null)
        {
            Wave.Invoke();
        }
    }

    public void EnemyDeath()
    {
        --EnemyNumber;
        if (EnemyNumber <= 0)
        {
            EnemyNumber = 0;
            waveEnded.Invoke();
        }
    }

    public void OrderDiffWaves(List<Wave> Waves,int leftIndex, int rightIndex)
    {
        var i = leftIndex;
        var j = rightIndex;
        var pivot = Waves[leftIndex].difficulty;
        while (i <= j)
        {
            while (Waves[i].difficulty < pivot)
            {
                i++;
            }

            while (Waves[j].difficulty > pivot)
            {
                j--;
            }
            if (i <= j)
            {
                Wave temp = Waves[i];
                Waves[i] = Waves[j];
                Waves[j] = temp;
                i++;
                j--;
            }
        }

        if (leftIndex < j)
            OrderDiffWaves(Waves, leftIndex, j);
        if (i < rightIndex)
            OrderDiffWaves(Waves, i, rightIndex);
    }

    public void SetArenaSystemWave()
    {
        for (int i = 0; i < arenaMatchSystemCopy.Count; i++)
        {
            List<Wave> waves = new List<Wave>(wavesDic[arenaMatchSystemCopy[i].categoryID]); 
            int difficulty = arenaMatchSystemCopy[i].totalDifficulty;
            for (int a = 0; a < arenaMatchSystemCopy[i].totalWave; a++)
            {
                var difficultyTemp = difficulty;
                for (int y = 0; y < arenaMatchSystemCopy[i].totalWave - (a + 1); y++)
                {
                    difficultyTemp -= waves[y].difficulty;
                }
                if (difficultyTemp <= 0) break; 
                var LastNumber = SearchAlgorithm(waves, i, difficultyTemp);
                if (LastNumber - 1 < 0) break;
                Wave wave;
                if (a+1 < arenaMatchSystemCopy[i].totalWave)
                    wave = waves[Random.Range(0, LastNumber)];
                else
                    wave = waves[LastNumber - 1];
                arenaMatchSystemCopy[i].Waves.Add(wave);
                waves.Remove(wave);
                difficulty -= wave.difficulty;
                if (difficulty <= 0) break;
            }
        }
    }

    public int SearchAlgorithm(List<Wave> waves, int i, int difficulty)
    {
        int baslangic = 0, bitis = waves.Count, arananSayi = difficulty;
        int orta = (baslangic + bitis) / 2;
        bool IsLeft = false;
        while (bitis - baslangic > 1)
        {
            orta = (baslangic + bitis) / 2;
            if (waves[orta].difficulty > arananSayi)
            {
                IsLeft = false;
                bitis = orta;
            }
            else if (waves[orta].difficulty < arananSayi)
            {
                IsLeft = true;
                baslangic = orta;
            }
            else
            {
                return orta;
            }
        }
        if (IsLeft)
        {
            return orta + 1;
        }
        else
        {
            return orta;
        }
    }

    public Wave? GetWave()
    {
        if (currArenaMatch.Waves.Count <= 0)
        {
            return null;
        }
        else
        {
            return currArenaMatch.Waves[0];
        }
    }

    public void SpawnEnemy(Wave wave)
    {
        var SpawnLocTem = new List<SpawnLoc>(SpawnLoc);
        foreach (var item in wave.mobAndCounts)
        {
            for (int i = 0; i < item.number; i++)
            {
                if (SpawnLocTem.Count <= 0) return;
                int Rand = Random.Range(0,SpawnLocTem.Count);
                var SpawnVec = SpawnLocTem[Rand];
                var GO = Instantiate(MobsDic[item.mob], SpawnVec.transform.position, Quaternion.identity, trash.transform);
                var agent = GO.GetComponent<NavMeshAgent>();
                agent.avoidancePriority = NavmeshOrderStack.Pop();
                agent.enabled = false;
                
                GO.GetComponent<EnemyAI>().Move(SpawnVec.dir);
                ++EnemyNumber;
                SpawnLocTem.RemoveAt(Rand);
            }
        }
    }

    public void StartNewArena()
    {
        ArenaMatchStarted.Invoke();
    }
}
