# 게임 통계 수집 시스템

## 개요
ML-Agents 환경에서 (게임 종류, 디버프 종류) 조합별 사망/생존 통계를 수집하는 시스템입니다.

## 작업 내용

### 1. 새로 생성된 파일
- **`GameStatisticsCollector.cs`**: 통계 수집 핵심 스크립트

### 2. 수정된 파일
- **`GlobalDebuffManager.cs`**: `GetCurrentDebuff()` 메서드 추가
- **`GlobalGameManager.cs`**: 게임 전환/오버 시 통계 기록 로직 추가

---

## 사용 방법

### 설정
1. **빈 게임 오브젝트 생성** (예: "StatisticsManager")
2. **`GameStatisticsCollector` 스크립트 추가**
3. **Inspector에서 설정 조정**:

| 설정 | 설명 | 기본값 |
|------|------|--------|
| `Export Path` | 파일 저장 경로 | `Application.persistentDataPath` |
| `File Name` | 저장 파일명 (확장자 제외) | `game_statistics` |
| `Auto Save Interval` | 자동 저장 간격 (초), 0이면 비활성화 | `60` |
| `Export CSV` | CSV 형식으로 내보내기 | `true` |
| `Export JSON` | JSON 형식으로 내보내기 | `true` |

### 저장 경로 확인
- **Windows**: `C:\Users\{사용자명}\AppData\LocalLow\{회사명}\{프로젝트명}\`
- **macOS**: `~/Library/Application Support/{회사명}/{프로젝트명}/`

---

## 통계 데이터 구조

### 집계 기준
- **Key**: `(GameKind, DebuffType)` 조합
  - `GameKind`: `Dodge`, `FlappyBird`, `PoopAvoid`
  - `DebuffType`: 디버프 클래스 이름 또는 `"None"` (디버프 없음)

### 수집 데이터
| 필드 | 설명 |
|------|------|
| `deathCount` | 해당 조합에서 사망한 횟수 |
| `surviveCount` | 해당 조합에서 무사히 통과한 횟수 |
| `totalCount` | 전체 시도 횟수 |
| `deathRate` | 사망률 (0.0 ~ 1.0) |
| `surviveRate` | 생존률 (0.0 ~ 1.0) |

---

## 출력 파일 형식

### CSV 예시
```csv
GameKind,DebuffType,DeathCount,SurviveCount,TotalCount,DeathRate,SurviveRate
Dodge,None,15,85,100,0.1500,0.8500
FlappyBird,KeyDebuff,42,58,100,0.4200,0.5800
PoopAvoid,SpeedDebuff,28,72,100,0.2800,0.7200
```

### JSON 예시
```json
{
    "exportTime": "2025-12-04 15:30:00",
    "entries": [
        {
            "gameKind": "Dodge",
            "debuffType": "None",
            "deathCount": 15,
            "surviveCount": 85,
            "totalCount": 100,
            "deathRate": 0.15,
            "surviveRate": 0.85
        }
    ]
}
```

---

## 유의 사항

### 1. 싱글턴 패턴
- `GameStatisticsCollector`는 **싱글턴**으로 구현됨
- 여러 TrainingArea의 통계가 **하나의 인스턴스로 통합**됨
- `DontDestroyOnLoad`로 씬 전환 시에도 유지됨
- **주의**: 씬에 하나만 배치할 것 (중복 시 자동 파괴됨)

### 2. 통계 기록 시점
| 이벤트 | 기록 내용 | 호출 위치 |
|--------|----------|-----------|
| 게임 전환 (`GameChange()`) | 이전 게임 **생존** 기록 | `GlobalGameManager` |
| 게임 오버 (`GameOver()`) | 현재 게임 **사망** 기록 | `GlobalGameManager` |

### 3. 디버프 없는 경우
- 디버프가 활성화되지 않은 상태에서는 `debuffType`이 `"None"`으로 기록됨

### 4. 파일 저장 타이밍
- **자동 저장**: `autoSaveInterval` 간격으로 저장
- **앱 종료 시**: `OnApplicationQuit()`에서 자동 저장
- **수동 저장**: `GameStatisticsCollector.Instance.ExportStatistics()` 호출

### 5. 스크립트 API
```csharp
// 통계 기록 (내부적으로 자동 호출됨)
GameStatisticsCollector.Instance.RecordDeath(gameKind, debuffManager);
GameStatisticsCollector.Instance.RecordSurvive(gameKind, debuffManager);

// 통계 내보내기
GameStatisticsCollector.Instance.ExportStatistics();

// 콘솔에 통계 출력
GameStatisticsCollector.Instance.PrintStatistics();

// 통계 초기화
GameStatisticsCollector.Instance.ClearStatistics();

// 특정 조합 통계 조회
var data = GameStatisticsCollector.Instance.GetStatistics(GameKind.Dodge, "SpeedDebuff");
```

### 6. 성능 고려사항
- 통계는 메모리에 Dictionary로 저장됨
- 파일 I/O는 지정된 간격 또는 종료 시에만 발생
- 대규모 학습 시에도 부하가 적음

---

## 문제 해결

### 파일이 생성되지 않는 경우
1. `Export Path`가 쓰기 가능한 경로인지 확인
2. `Export CSV` 또는 `Export JSON`이 활성화되어 있는지 확인
3. Console에서 에러 메시지 확인

### 통계가 기록되지 않는 경우
1. `GameStatisticsCollector`가 씬에 존재하는지 확인
2. `GlobalGameManager`와 `GlobalDebuffManager`가 올바르게 연결되어 있는지 확인
