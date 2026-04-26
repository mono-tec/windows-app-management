; ============================================
; 共通インストーラ定義（ベース）
; ============================================
; 本ファイルは、すべての顧客・タスクで共通利用する設定を定義する
; 顧客ごとの差分は別ファイルで上書きすること
; ============================================

; --------------------------------------------
; 1. ビルド成果物関連
; --------------------------------------------
; アプリの発行フォルダ（Publish）
#define PublishDir        "..\\src\\TaskWorkerSample\\bin\\Release\\net10.0\\win-x64"

; タスクスケジューラ用テンプレートXML
#define TaskTemplateName  "TaskMaster_10min"

; --------------------------------------------
; 2. アプリ・ツール情報
; --------------------------------------------
; 実行ファイル名（exe本体）
#define AppCodeName       "TaskWorkerSample"

; 文字列置換ツール
#define ReplaceAppName    "InnoReplacer"

; --------------------------------------------
; 3. 会社・署名情報
; --------------------------------------------
; 会社名（Program Files / ProgramData 配下に使用）
#define CompName          "SampleCorp"

; 発行者名（インストーラ表示用）
#define PublisherName     "SAMPLE COMP CO.,LTD."

; --------------------------------------------
; 4. タスク実行環境
; --------------------------------------------
; タスク実行ユーザ（例：Local Service）
#define TaskUserId        "S-1-5-19"

; --------------------------------------------
; 5. リソース
; --------------------------------------------
; インストーラアイコン
#define IconFilePath      "..\\src\\TaskWorkerSample\\Resources\\taskworkersample_256.ico"

; --------------------------------------------
; 6. アプリ制御
; --------------------------------------------
; 停止制御フラグファイル名
#define AppStopFlgName    "stop-request.flag"


; ================================
; ■ セットアップ基本設定
; ================================
[Setup]
; 表示名
AppName={#AppName} {#AppSubName}

; インストール先（Program Files）
DefaultDirName={autopf}\{#CompName}\{#AppName}

; スタートメニュー
DefaultGroupName={#AppName}

; インストーラ作成後出力先
OutputDir=Output\{#CustomerCode}
OutputBaseFilename={#AppName}_{#CustomerCode}

; 圧縮
Compression=lzma
SolidCompression=yes

; アイコン
SetupIconFile="{#IconFilePath}"

; 64bit
ArchitecturesInstallIn64BitMode=x64os

; 発行者
AppPublisher={#PublisherName}

; アンインストール表示アイコン(アプリに表示)
UninstallDisplayIcon="{app}\{#AppCodeName}.exe"

; バージョン
AppVersion={#AppVersion}
AppVerName={#AppName} {#AppVersion}

[Languages]
Name: "japanese"; MessagesFile: "compiler:Languages\Japanese.isl"

; ================================
; ■ ファイル配置
; ================================
[Files]
; ビルド成果物を配布（Release ビルド済み想定）
Source: "{#PublishDir}\{#AppCodeName}.exe";        DestDir: "{app}"; Flags: ignoreversion

; ★ DLL 一括追加
Source: "{#PublishDir}\*.dll"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

; ★ pdb 一括追加
Source: "{#PublishDir}\*.pdb"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

; json
Source: "{#PublishDir}\*.json"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs
; 設定テンプレートは一時フォルダに配置
Source: "{#CustomerJsonDir}\{#SettingFileName}"; DestDir: "{tmp}"; Flags: ignoreversion

; 置換ツール（実行後に削除します）
Source: "{#ReplaceAppName}.exe"; DestDir: "{tmp}"; Flags: ignoreversion
Source: "{#TaskTemplateName}.xml";   DestDir: "{app}"; Flags: ignoreversion

; ================================
; ■ ディレクトリ作成
; ================================
[Dirs]
; ログディレクトリ（タスク単位）
Name: "{code:GetLogDir}"
; 設定ファイル配置ディレクトリ（ProgramData）
Name: "{code:GetSettingDir}"
; 制御フォルダ（stop-request.flag用）
Name: "{code:GetControlDir}"

[Icons]
Name: "{group}\{#AppName}"; Filename: "{app}\{#AppCodeName}.exe"


; ================================
; ■ パス生成関数
; ================================
[Code]
var
  Page1: TInputOptionWizardPage;

procedure InitializeWizard();
begin
  // Page 1：設定ファイル上書き設定
  Page1 :=
    CreateInputOptionPage(
      wpSelectDir,
      '設定ファイル',
      '既存の設定ファイルを上書きするか選択してください。',
      '通常はチェックを外したままにしてください。既存設定を保持できます。',
      False,
      False
    );

  Page1.Add('既存の設定ファイルを上書きする');
  Page1.Values[0] := False;
end;

function GetExeName(Param: string): string; begin Result := '{#AppCodeName}.exe'; end;
function GetWorkingDir(Param: string): string; begin Result := ExpandConstant('{app}'); end;
function GetLogDir(Param: string): string; begin Result := ExpandConstant('{commonappdata}\{#CompName}\{#TaskName}\logs'); end;
function GetSettingDir(Param: string): string; begin Result := ExpandConstant('{commonappdata}\{#CompName}\{#TaskName}\{#SettingDir}'); end;
function GetSettingPath(Param: string): string; begin Result := ExpandConstant('{commonappdata}\{#CompName}\{#TaskName}\{#SettingDir}\{#SettingFileName}'); end;
function GetControlDir(Param: string): string; begin Result := ExpandConstant('{commonappdata}\{#CompName}\{#TaskName}\control'); end;
function GetOverwriteConfig(): Boolean; begin  Result := Page1.Values[0]; end;

[Run]
; ================================
; ■ インストール後処理
; ================================
; ------------------------------
; ■ XML置換（タスク設定）
; ------------------------------
Filename: "{tmp}\{#ReplaceAppName}.exe"; Parameters: """{app}\{#TaskTemplateName}.xml"" ""#TASK_EXE_NAME#"" ""{code:GetExeName}"""; Flags: runhidden waituntilterminated
Filename: "{tmp}\{#ReplaceAppName}.exe"; Parameters: """{app}\{#TaskTemplateName}.xml"" ""#TASK_EXE_WORKING_DIR#"" ""{app}"""; Flags: runhidden waituntilterminated
Filename: "{tmp}\{#ReplaceAppName}.exe"; Parameters: """{app}\{#TaskTemplateName}.xml"" ""#TASK_USERID#"" ""{#TaskUserId}"""; Flags: runhidden waituntilterminated
Filename: "{tmp}\{#ReplaceAppName}.exe"; Parameters: """{app}\{#TaskTemplateName}.xml"" ""#TASK_EXE_ARGUMENTS#"" ""--config {code:GetSettingPath}"""; Flags: runhidden waituntilterminated
Filename: "{tmp}\{#ReplaceAppName}.exe"; Parameters: """{app}\{#TaskTemplateName}.xml"" ""#TASK_AUTHOR#"" ""{#CompName}"""; Flags: runhidden waituntilterminated
Filename: "{tmp}\{#ReplaceAppName}.exe"; Parameters: """{app}\{#TaskTemplateName}.xml"" ""#TASK_DESCRIPTION#"" ""{#TaskDescription}"""; Flags: runhidden waituntilterminated

; ------------------------------
; ■ JSON置換（UTF-8）
; ------------------------------
; ※ JSONは必ずUTF-8で処理する
; ※ パスは \\ でエスケープすること
Filename: "{tmp}\{#ReplaceAppName}.exe"; Parameters: """{tmp}\{#SettingFileName}"" ""#APP_MUTEX_KEY#"" ""{#AppMutexKey}""  utf8bom"; Flags: runhidden waituntilterminated
Filename: "{tmp}\{#ReplaceAppName}.exe"; Parameters: """{tmp}\{#SettingFileName}"" ""#COMP_NAME#"" ""{#CompName}""  utf8bom"; Flags: runhidden waituntilterminated
Filename: "{tmp}\{#ReplaceAppName}.exe"; Parameters: """{tmp}\{#SettingFileName}"" ""#TASK_NAME#"" ""{#TaskName}""  utf8bom"; Flags: runhidden waituntilterminated

; tmpから設定移動
; 既存設定を上書きする場合
Filename: "cmd.exe"; Parameters: "/C copy /Y ""{tmp}\{#SettingFileName}"" ""{code:GetSettingPath}"""; Flags: runhidden waituntilterminated; Check: GetOverwriteConfig

; 既存設定を保持する場合
Filename: "cmd.exe"; Parameters: "/C if not exist ""{code:GetSettingPath}"" copy /Y ""{tmp}\{#SettingFileName}"" ""{code:GetSettingPath}"""; Flags: runhidden waituntilterminated; Check: not GetOverwriteConfig

; ------------------------------
; ■ 不要な設定フォルダ削除
; ------------------------------
Filename: "cmd.exe"; Parameters: "/C rmdir /s /q ""{app}\{#SettingDir}"""; Flags: runhidden shellexec waituntilterminated

; ------------------------------
; ■ 権限設定
; ------------------------------
; ログ → サービスユーザ
Filename: "cmd.exe"; Parameters: "/C icacls ""{code:GetLogDir}"" /grant *{#TaskUserId}:(OI)(CI)M /T /C /Q"; Flags: runhidden waituntilterminated

; control,設定 → 全員
Filename: "cmd.exe"; Parameters: "/C icacls ""{code:GetSettingDir}"" /grant Everyone:(OI)(CI)M /T /C /Q"; Flags: runhidden waituntilterminated
Filename: "cmd.exe"; Parameters: "/C icacls ""{code:GetControlDir}"" /grant Everyone:(OI)(CI)M /T /C /Q"; Flags: runhidden waituntilterminated

; ------------------------------
; ■ 停止フラグ作成
; ------------------------------
Filename: "cmd.exe"; Parameters: "/C if not exist ""{code:GetControlDir}\{#AppStopFlgName}"" type nul > ""{code:GetControlDir}\{#AppStopFlgName}"""; Flags: runhidden waituntilterminated

; ------------------------------
; ■ タスク登録
; ------------------------------
Filename: "schtasks.exe"; Parameters: "/Delete /TN ""{#TaskName}"" /F"; Flags: runhidden waituntilterminated
Filename: "schtasks.exe"; Parameters: "/Create /TN ""{#TaskName}"" /XML ""{app}\{#TaskTemplateName}.xml"" /F"; Flags: runhidden waituntilterminated

[UninstallRun]
; アンインストール時：タスク削除（存在しなくてもOK）
Filename: "schtasks.exe"; Parameters: "/Delete /TN ""{#TaskName}"" /F";  Flags: runhidden waituntilterminated; RunOnceId: "Delete_{#TaskName}"
