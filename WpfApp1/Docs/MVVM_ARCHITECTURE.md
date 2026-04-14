# MVVM 구조 및 프로젝트 설계 초안

문서명: 스마트 자동화 설비 통합 제어 소프트웨어 MVVM 구조 및 프로젝트 설계 초안  
문서 버전: v0.1  
최종 수정일: 2026-04-14  
기준 문서: `PROJECT_PLAN.md`, `REQUIREMENTS_SPEC.md`, `SCREEN_DESIGN.md`

## 1. 문서 목적

본 문서는 WPF 애플리케이션을 MVVM 패턴으로 구현하기 위한 기본 구조를 정의한다.  
초기 구현 시 폴더 구조, 클래스 책임, 서비스 인터페이스, 데이터 흐름, 구현 우선순위를 통일된 기준으로 사용한다.

## 2. 설계 원칙

- View는 화면 표시와 사용자 입력 바인딩만 담당한다.
- ViewModel은 화면 상태와 명령 처리 흐름을 담당한다.
- Model은 도메인 데이터와 상태를 표현한다.
- 실제 장비 제어는 ViewModel에서 직접 수행하지 않고 서비스 계층을 통해 호출한다.
- 데이터 저장과 통신은 Infrastructure 계층으로 분리한다.
- 현재는 Mock 구현체를 기본값으로 사용하고, 이후 실제 구현체로 교체한다.

## 3. 권장 프로젝트 구조

현재는 단일 WPF 프로젝트로 시작하되, 내부 폴더를 레이어 기준으로 나눈다.

```text
WpfApp1/
  App.xaml
  App.xaml.cs
  MainWindow.xaml
  MainWindow.xaml.cs
  Common/
    Base/
      ObservableObject.cs
      ViewModelBase.cs
    Commands/
      RelayCommand.cs
      AsyncRelayCommand.cs
    Constants/
    Helpers/
  Models/
    Inspection/
      InspectionRecord.cs
      InspectionSummary.cs
    Alarm/
      AlarmRecord.cs
      AlarmLevel.cs
    Recipe/
      Recipe.cs
      RecipeParameter.cs
    Equipment/
      EquipmentStatus.cs
      DeviceConnectionStatus.cs
      SystemMode.cs
    Control/
      ManualCommandResult.cs
  Services/
    Interfaces/
      IInspectionService.cs
      IAlarmService.cs
      IRecipeService.cs
      IEquipmentStatusService.cs
      IManualControlService.cs
      ICommunicationService.cs
      IVisionService.cs
      IIoControllerService.cs
      IRobotService.cs
    Mock/
      MockInspectionService.cs
      MockAlarmService.cs
      MockRecipeService.cs
      MockEquipmentStatusService.cs
      MockManualControlService.cs
    Data/
      SqliteInspectionRepository.cs
      SqliteAlarmRepository.cs
      SqliteRecipeRepository.cs
    Communication/
      SerialCommunicationService.cs
      TcpCommunicationService.cs
  ViewModels/
    Shell/
      ShellViewModel.cs
      NavigationViewModel.cs
    Dashboard/
      DashboardViewModel.cs
    InspectionHistory/
      InspectionHistoryViewModel.cs
    AlarmLog/
      AlarmLogViewModel.cs
    RecipeManagement/
      RecipeManagementViewModel.cs
    ManualControl/
      ManualControlViewModel.cs
    Settings/
      SettingsViewModel.cs
  Views/
    Shell/
      ShellView.xaml
    Dashboard/
      DashboardView.xaml
    InspectionHistory/
      InspectionHistoryView.xaml
    AlarmLog/
      AlarmLogView.xaml
    RecipeManagement/
      RecipeManagementView.xaml
    ManualControl/
      ManualControlView.xaml
    Settings/
      SettingsView.xaml
  Resources/
    Styles/
      Colors.xaml
      Typography.xaml
      Buttons.xaml
      Cards.xaml
      DataGrid.xaml
    Templates/
    Icons/
  Data/
    AppDbContext.cs
    Seed/
      SeedData.cs
```

## 4. 폴더별 책임

### 4.1 Common

- 공통 베이스 클래스
- 커맨드 구현
- 상수, 유틸리티, 공통 헬퍼

### 4.2 Models

- 화면용 DTO가 아닌 도메인 의미를 가진 데이터 모델
- 검사, 알람, 레시피, 장비 상태 등 핵심 데이터 정의

### 4.3 Services

- ViewModel이 의존하는 기능 제공 계층
- 인터페이스와 구현체를 분리
- Mock, 저장소, 통신 구현을 하위 폴더로 구분

### 4.4 ViewModels

- 화면별 상태 관리
- 명령 바인딩
- 서비스 호출 및 결과 가공

### 4.5 Views

- XAML 화면
- DataTemplate 또는 UserControl 기반 페이지 구성

### 4.6 Resources

- 스타일과 리소스 딕셔너리
- 산업용 UI에 맞는 공통 색상/카드/버튼 스타일 관리

### 4.7 Data

- SQLite 초기화
- 샘플 데이터 시드
- 향후 DB 구성 공통 진입점

## 5. 화면 구성 방식

초기 구조는 `MainWindow`를 셸 컨테이너로 사용하고, 내부에서 화면을 전환한다.

권장 방식:

- `MainWindow`는 `ShellView` 역할
- 좌측 메뉴 + 상단 상태바 + 본문 콘텐츠 영역으로 구성
- 본문은 `CurrentViewModel` 또는 `CurrentPageKey`를 기준으로 전환
- 각 화면은 `UserControl`로 분리

예상 전환 구조:

- `ShellViewModel`
  - `CurrentViewModel`
  - `NavigateDashboardCommand`
  - `NavigateInspectionHistoryCommand`
  - `NavigateAlarmLogCommand`
  - `NavigateRecipeManagementCommand`
  - `NavigateManualControlCommand`
  - `NavigateSettingsCommand`

## 6. 핵심 Base 클래스 설계

### 6.1 ObservableObject

역할:

- `INotifyPropertyChanged` 구현
- ViewModel과 일부 상태 모델의 공통 기반

필수 기능:

- `SetProperty`
- `OnPropertyChanged`

### 6.2 ViewModelBase

역할:

- 모든 ViewModel의 기본 클래스
- 공통 상태 보유

권장 공통 속성:

- `Title`
- `IsBusy`
- `StatusMessage`

### 6.3 RelayCommand / AsyncRelayCommand

역할:

- 버튼과 메뉴 바인딩용 공통 커맨드
- 비동기 로직과 동기 로직 분리

## 7. Model 초안

### 7.1 InspectionRecord

주요 속성:

- `Id`
- `InspectedAt`
- `ProductId`
- `Result`
- `Score`
- `Comment`

### 7.2 AlarmRecord

주요 속성:

- `Id`
- `OccurredAt`
- `Level`
- `Category`
- `Message`
- `IsAcknowledged`

### 7.3 Recipe

주요 속성:

- `Id`
- `Name`
- `ProductName`
- `Threshold1`
- `Threshold2`
- `ProcessOption`
- `IsActive`
- `Description`

### 7.4 EquipmentStatus

주요 속성:

- `DeviceName`
- `ConnectionStatus`
- `CurrentState`
- `LastUpdatedAt`

### 7.5 InspectionSummary

주요 속성:

- `TotalCount`
- `OkCount`
- `NgCount`
- `YieldRate`

## 8. 서비스 인터페이스 설계 초안

### 8.1 IInspectionService

책임:

- 검사 이력 조회
- 검사 결과 생성
- 통계 집계 제공

예상 메서드:

- `GetRecentInspectionAsync()`
- `GetInspectionHistoryAsync(filter)`
- `GetInspectionSummaryAsync()`
- `GenerateMockInspectionAsync()`

### 8.2 IAlarmService

책임:

- 알람 등록
- 알람 목록 조회
- 확인 처리

예상 메서드:

- `GetAlarmsAsync(filter)`
- `AddAlarmAsync(record)`
- `AcknowledgeAlarmAsync(id)`

### 8.3 IRecipeService

책임:

- 레시피 CRUD

예상 메서드:

- `GetRecipesAsync()`
- `GetRecipeAsync(id)`
- `SaveRecipeAsync(recipe)`
- `DeleteRecipeAsync(id)`

### 8.4 IEquipmentStatusService

책임:

- 장비 상태 조회
- 연결 상태 관리
- Mock 상태 변경

예상 메서드:

- `GetEquipmentStatusesAsync()`
- `UpdateStatusAsync(status)`
- `SimulateConnectionAsync(deviceName, isConnected)`

### 8.5 IManualControlService

책임:

- 시작 / 정지 / 리셋
- 자동 / 수동 전환
- 장비별 수동 명령 처리

예상 메서드:

- `StartAsync()`
- `StopAsync()`
- `ResetAsync()`
- `SetModeAsync(mode)`
- `ExecuteDeviceCommandAsync(command)`

### 8.6 확장 인터페이스

향후 실제 연동을 위한 인터페이스:

- `IVisionService`
- `IIoControllerService`
- `IRobotService`
- `ICommunicationService`

현재 단계에서는 호출 경로만 설계하고 Mock 구현체로 대체한다.

## 9. ViewModel 책임 분리

### 9.1 ShellViewModel

책임:

- 전체 화면 전환
- 상단 공통 상태 표시
- 전역 알람 카운트 반영

보유 상태 예시:

- `CurrentViewModel`
- `CurrentPageTitle`
- `CurrentSystemTime`
- `GlobalConnectionSummary`

### 9.2 DashboardViewModel

책임:

- 대시보드 카드 데이터 관리
- 생산 집계 및 최근 검사 결과 표시
- 최근 알람 요약 표시

주요 속성 예시:

- `CurrentMode`
- `TotalCount`
- `OkCount`
- `NgCount`
- `LatestInspectionResult`
- `RecentAlarms`
- `EquipmentStatuses`

### 9.3 InspectionHistoryViewModel

책임:

- 조건 검색
- 검사 이력 목록 바인딩
- 상세 정보 갱신

주요 속성 예시:

- `SearchFrom`
- `SearchTo`
- `SelectedResultFilter`
- `Keyword`
- `Records`
- `SelectedRecord`

### 9.4 AlarmLogViewModel

책임:

- 알람 필터 관리
- 목록 로딩
- 확인 처리

### 9.5 RecipeManagementViewModel

책임:

- 레시피 목록
- 선택 레시피 편집
- 저장 / 삭제 / 신규 처리

### 9.6 ManualControlViewModel

책임:

- 공정 제어 명령 실행
- 장비별 수동 조작 처리
- 이벤트 결과 반영

### 9.7 SettingsViewModel

책임:

- 시스템 옵션 관리
- 저장소 / 통신 설정 관리

## 10. 데이터 흐름

기본 흐름은 다음과 같다.

1. View에서 명령이 발생한다.
2. ViewModel이 입력 값을 검증한다.
3. ViewModel이 서비스 인터페이스를 호출한다.
4. 서비스가 Mock 또는 저장소 구현체를 사용해 처리한다.
5. 결과 모델이 ViewModel로 반환된다.
6. ViewModel이 표시용 속성을 갱신한다.
7. View는 바인딩을 통해 화면을 자동 갱신한다.

중요 원칙:

- View는 서비스에 직접 접근하지 않는다.
- ViewModel은 DB나 통신 구현 세부사항을 모른다.
- 서비스 인터페이스가 구현 교체 지점이 된다.

## 11. 상태 관리 방식

초기에는 복잡한 전역 상태 저장소를 두지 않고, 다음 수준으로 관리한다.

- 전역 상태: `ShellViewModel`
- 화면별 상태: 각 ViewModel
- 도메인 상태: `Models`
- 장비 상태 변경 이벤트: 서비스에서 발행 후 ViewModel 갱신

필요 시 추가 가능한 항목:

- `IEventBus`
- `IMessageService`
- `INavigationService`

초기 MVP에서는 과도한 추상화보다 단순 명확한 구조를 우선한다.

## 12. 의존성 주입 초안

초기 구현에서는 `App.xaml.cs`에서 직접 조립하거나, 이후 DI 컨테이너를 붙일 수 있다.

1차 권장:

- 간단한 수동 주입
- `App.xaml.cs`에서 서비스와 ViewModel 생성

2차 권장:

- `Microsoft.Extensions.DependencyInjection` 도입
- 서비스/저장소/ViewModel 등록

초기 등록 예시:

- `IInspectionService -> MockInspectionService`
- `IAlarmService -> MockAlarmService`
- `IRecipeService -> MockRecipeService`
- `IEquipmentStatusService -> MockEquipmentStatusService`
- `IManualControlService -> MockManualControlService`

## 13. 초기 구현 우선순위

### 13.1 1단계

- `ObservableObject`
- `ViewModelBase`
- `RelayCommand`
- `ShellViewModel`
- `DashboardViewModel`
- 기본 네비게이션 구조

### 13.2 2단계

- 검사 이력 / 알람 / 레시피 ViewModel
- Mock 서비스 구현
- 더미 데이터 표시

### 13.3 3단계

- SQLite 저장소 연결
- 수동 제어 명령 반영
- 상태 시뮬레이션 추가

### 13.4 4단계

- 설정 화면
- 통신 인터페이스 정리
- 실제 장비 연동 준비

## 14. 권장 네이밍 규칙

- View: `DashboardView`
- ViewModel: `DashboardViewModel`
- 서비스 인터페이스: `IInspectionService`
- 서비스 구현체: `MockInspectionService`
- 모델: `InspectionRecord`
- 커맨드 메서드: `LoadCommand`, `SaveCommand`, `ResetCommand`

## 15. 주의사항

- ViewModel에 UI 전용 타입 의존성을 최소화한다.
- 코드비하인드는 화면 초기화나 특수 UI 처리 외에는 사용하지 않는다.
- 실제 장비 통신 로직을 ViewModel에 직접 넣지 않는다.
- 한 ViewModel이 다른 ViewModel의 내부 구현에 직접 의존하지 않도록 한다.
- 초기에는 단일 프로젝트로 가되, 구조가 커지면 추후 Class Library 분리를 검토한다.

## 16. 다음 구현 기준

이 문서를 기준으로 다음 작업을 진행한다.

- WPF 프로젝트 폴더 구조 정리
- Base/ViewModel/Command 공통 코드 추가
- Shell + Dashboard 화면 골격 구현
- Mock 서비스 연결
