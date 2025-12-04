using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// 게임 종류와 디버프 조합별 통계를 수집하는 클래스.
/// 빈 게임 오브젝트에 이 스크립트를 추가하면 작동합니다.
/// 여러 TrainingArea의 통계가 이 하나의 인스턴스로 통합됩니다.
/// </summary>
public class GameStatisticsCollector : MonoBehaviour
{
    // 싱글턴 패턴 - 여러 TrainingArea에서 공통 통계로 모임
    public static GameStatisticsCollector Instance { get; private set; }

    [Header("Export Settings")]
    [Tooltip("통계 파일 저장 경로 (비어있으면 Application.persistentDataPath 사용)")]
    [SerializeField] private string exportPath = "";
    
    [Tooltip("파일명 (확장자 제외)")]
    [SerializeField] private string fileName = "game_statistics";

    [Tooltip("자동 저장 간격 (초). 0이면 자동 저장 안함")]
    [SerializeField] private float autoSaveInterval = 60f;

    [Tooltip("CSV로 저장")]
    [SerializeField] private bool exportCSV = true;
    
    [Tooltip("JSON으로 저장")]
    [SerializeField] private bool exportJSON = true;

    // 통계 데이터 구조
    // Key: (GameKind, DebuffTypeName) - DebuffTypeName이 null이면 "None"
    private Dictionary<StatisticsKey, StatisticsData> statistics = new Dictionary<StatisticsKey, StatisticsData>();

    private float lastSaveTime = 0f;

    [Serializable]
    public struct StatisticsKey : IEquatable<StatisticsKey>
    {
        public GameKind gameKind;
        public string debuffType; // 클래스 이름 또는 "None"

        public StatisticsKey(GameKind gameKind, string debuffType)
        {
            this.gameKind = gameKind;
            this.debuffType = debuffType ?? "None";
        }

        public bool Equals(StatisticsKey other)
        {
            return gameKind == other.gameKind && debuffType == other.debuffType;
        }

        public override bool Equals(object obj)
        {
            return obj is StatisticsKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(gameKind, debuffType);
        }

        public override string ToString()
        {
            return $"({gameKind}, {debuffType})";
        }
    }

    [Serializable]
    public class StatisticsData
    {
        public int deathCount = 0;      // 해당 조합에서 사망한 횟수
        public int surviveCount = 0;    // 해당 조합에서 무사히 통과한 횟수

        public int TotalCount => deathCount + surviveCount;
        public float DeathRate => TotalCount > 0 ? (float)deathCount / TotalCount : 0f;
        public float SurviveRate => TotalCount > 0 ? (float)surviveCount / TotalCount : 0f;
    }

    // JSON 직렬화용 래퍼 클래스
    [Serializable]
    private class StatisticsEntry
    {
        public string gameKind;
        public string debuffType;
        public int deathCount;
        public int surviveCount;
        public int totalCount;
        public float deathRate;
        public float surviveRate;
    }

    [Serializable]
    private class StatisticsExport
    {
        public string exportTime;
        public List<StatisticsEntry> entries = new List<StatisticsEntry>();
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (string.IsNullOrEmpty(exportPath))
        {
            exportPath = Application.persistentDataPath;
        }
    }

    void Update()
    {
        if (autoSaveInterval > 0 && Time.time - lastSaveTime >= autoSaveInterval)
        {
            lastSaveTime = Time.time;
            ExportStatistics();
        }
    }

    void OnApplicationQuit()
    {
        ExportStatistics();
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            ExportStatistics();
            Instance = null;
        }
    }

    /// <summary>
    /// 사망 이벤트 기록
    /// </summary>
    /// <param name="gameKind">현재 게임 종류</param>
    /// <param name="debuffManager">현재 활성화된 디버프 매니저 (null 가능)</param>
    public void RecordDeath(GameKind gameKind, DebuffManager debuffManager)
    {
        string debuffType = GetDebuffTypeName(debuffManager);
        StatisticsKey key = new StatisticsKey(gameKind, debuffType);
        
        if (!statistics.TryGetValue(key, out StatisticsData data))
        {
            data = new StatisticsData();
            statistics[key] = data;
        }
        
        data.deathCount++;
        GlobalDatas.DebugLog(() => $"[Statistics] Death recorded: {key}, Total deaths: {data.deathCount}");
    }

    /// <summary>
    /// 생존(무사 통과) 이벤트 기록
    /// </summary>
    /// <param name="gameKind">현재 게임 종류</param>
    /// <param name="debuffManager">현재 활성화된 디버프 매니저 (null 가능)</param>
    public void RecordSurvive(GameKind gameKind, DebuffManager debuffManager)
    {
        string debuffType = GetDebuffTypeName(debuffManager);
        StatisticsKey key = new StatisticsKey(gameKind, debuffType);
        
        if (!statistics.TryGetValue(key, out StatisticsData data))
        {
            data = new StatisticsData();
            statistics[key] = data;
        }
        
        data.surviveCount++;
        GlobalDatas.DebugLog(() => $"[Statistics] Survive recorded: {key}, Total survives: {data.surviveCount}");
    }

    private string GetDebuffTypeName(DebuffManager debuffManager)
    {
        if (debuffManager == null)
        {
            return "None";
        }
        return debuffManager.GetType().Name;
    }

    /// <summary>
    /// 통계 데이터 내보내기
    /// </summary>
    public void ExportStatistics()
    {
        if (exportCSV)
        {
            ExportToCSV();
        }
        if (exportJSON)
        {
            ExportToJSON();
        }
    }

    private void ExportToCSV()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("GameKind,DebuffType,DeathCount,SurviveCount,TotalCount,DeathRate,SurviveRate");

            foreach (var kvp in statistics)
            {
                StatisticsKey key = kvp.Key;
                StatisticsData data = kvp.Value;
                sb.AppendLine($"{key.gameKind},{key.debuffType},{data.deathCount},{data.surviveCount},{data.TotalCount},{data.DeathRate:F4},{data.SurviveRate:F4}");
            }

            string fullPath = Path.Combine(exportPath, fileName + ".csv");
            File.WriteAllText(fullPath, sb.ToString(), Encoding.UTF8);
            Debug.Log($"[Statistics] CSV exported to: {fullPath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[Statistics] Failed to export CSV: {e.Message}");
        }
    }

    private void ExportToJSON()
    {
        try
        {
            StatisticsExport export = new StatisticsExport();
            export.exportTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            foreach (var kvp in statistics)
            {
                StatisticsKey key = kvp.Key;
                StatisticsData data = kvp.Value;

                StatisticsEntry entry = new StatisticsEntry
                {
                    gameKind = key.gameKind.ToString(),
                    debuffType = key.debuffType,
                    deathCount = data.deathCount,
                    surviveCount = data.surviveCount,
                    totalCount = data.TotalCount,
                    deathRate = data.DeathRate,
                    surviveRate = data.SurviveRate
                };

                export.entries.Add(entry);
            }

            string json = JsonUtility.ToJson(export, true);
            string fullPath = Path.Combine(exportPath, fileName + ".json");
            File.WriteAllText(fullPath, json, Encoding.UTF8);
            Debug.Log($"[Statistics] JSON exported to: {fullPath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[Statistics] Failed to export JSON: {e.Message}");
        }
    }

    /// <summary>
    /// 현재 통계를 콘솔에 출력
    /// </summary>
    public void PrintStatistics()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("=== Game Statistics ===");
        sb.AppendLine("GameKind | DebuffType | Deaths | Survives | Total | DeathRate");
        sb.AppendLine("-----------------------------------------------------------");

        foreach (var kvp in statistics)
        {
            StatisticsKey key = kvp.Key;
            StatisticsData data = kvp.Value;
            sb.AppendLine($"{key.gameKind,-12} | {key.debuffType,-20} | {data.deathCount,6} | {data.surviveCount,8} | {data.TotalCount,5} | {data.DeathRate:P2}");
        }

        Debug.Log(sb.ToString());
    }

    /// <summary>
    /// 통계 초기화
    /// </summary>
    public void ClearStatistics()
    {
        statistics.Clear();
        Debug.Log("[Statistics] Statistics cleared");
    }

    /// <summary>
    /// 특정 조합의 통계 조회
    /// </summary>
    public StatisticsData GetStatistics(GameKind gameKind, string debuffType)
    {
        StatisticsKey key = new StatisticsKey(gameKind, debuffType);
        return statistics.TryGetValue(key, out StatisticsData data) ? data : null;
    }

    /// <summary>
    /// 모든 통계 데이터 반환
    /// </summary>
    public Dictionary<StatisticsKey, StatisticsData> GetAllStatistics()
    {
        return new Dictionary<StatisticsKey, StatisticsData>(statistics);
    }
}
