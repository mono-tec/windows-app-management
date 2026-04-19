using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TaskWorkerSample.Common;
using TaskWorkerSample.Configurations;
using TaskWorkerSample.Services;
using TaskWorkerSample.Services.Execution;
using TaskWorkerSample.Services.Interfaces;

namespace TaskWorkerSample;

/// <summary>
/// アプリケーションのエントリポイントです。
/// </summary>
public static class Program
{
    /// <summary>
    /// アプリケーションを起動します。
    /// </summary>
    /// <param name="args">コマンドライン引数です。</param>
    /// <returns>終了コードを返します。</returns>
    public static async Task<int> Main(string[] args)
    {
        string defaultConfigPath = Path.Combine(AppContext.BaseDirectory, "config", "appsettings.json");
        string resolvedConfigPath = ConfigPathResolver.Resolve(args);

        AppPaths appPaths = AppPaths.FromConfigPath(resolvedConfigPath);

        IConfiguration configuration = BuildConfiguration(resolvedConfigPath);
        AppSettings settings = configuration.Get<AppSettings>() ?? new AppSettings();

        // Serilog のロガーを構築します。設定ファイルからロギング設定を読み取ります。
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        try
        {
            // control フォルダを作成しておく
            Directory.CreateDirectory(appPaths.ControlDir);

            Log.Information("アプリケーションを開始します。");
            Log.Information("設定ファイルパス: {ConfigPath}", resolvedConfigPath);
            Log.Information("ベースディレクトリ: {BaseDir}", appPaths.BaseDir);

            // 起動前に停止フラグを確認する
            if (File.Exists(appPaths.StopFlagPath))
            {
                Log.Information("停止フラグを検出したため、処理を中止します。");
                return ExitCodes.Success;
            }

            if (!MutexHelper.TryAcquire(settings.MutexKey, out Mutex? mutex))
            {
                Log.Warning("多重起動を検知したため処理を終了します。");
                return ExitCodes.Duplicate;
            }

            using (mutex)
            {
                ServiceProvider provider = BuildServiceProvider(settings, appPaths, Log.Logger);

                ExecutionMode mode = ParseExecutionMode(settings.ExecutionMode);
                IExecutionServiceFactory factory = provider.GetRequiredService<IExecutionServiceFactory>();
                IExecutionService service = factory.Create(mode);

                using var cts = new CancellationTokenSource();
                int exitCode = await service.ExecuteAsync(cts.Token);

                Log.Information("アプリケーションを終了します。終了コード: {ExitCode}", exitCode);
                return exitCode;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "予期しない例外が発生しました。");
            return ExitCodes.Failure;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    /// <summary>
    /// 設定ファイルを読み込みます。
    /// </summary>
    /// <param name="configPath">採用する設定ファイルパスです。</param>
    /// <returns>構成情報を返します。</returns>
    private static IConfiguration BuildConfiguration(string configPath)
    {
        var builder = new ConfigurationBuilder();

        if (File.Exists(configPath))
        {
            builder.AddJsonFile(configPath, optional: false, reloadOnChange: false);
        }
        else
        {
            throw new FileNotFoundException("設定ファイルが見つかりません。", configPath);
        }

        return builder.Build();
    }

    /// <summary>
    /// DI コンテナを構築します。
    /// </summary>
    /// <param name="settings">アプリ設定です。</param>
    /// <param name="appPaths">アプリケーションパス情報です。</param>
    /// <param name="logger">ロガーです。</param>
    /// <returns>サービスプロバイダを返します。</returns>
    private static ServiceProvider BuildServiceProvider(AppSettings settings, AppPaths appPaths, ILogger logger)
    {
        var services = new ServiceCollection();

        services.AddSingleton(settings);
        services.AddSingleton(appPaths);
        services.AddSingleton(logger);
        services.AddSingleton<IClock, SystemClock>();
        services.AddSingleton<IPersonFactory, PersonFactory>();
        services.AddSingleton<IExecutionServiceFactory, ExecutionServiceFactory>();

        services.AddTransient<ContinuousExecutionService>();
        services.AddTransient<ImmediateExitService>();
        services.AddTransient<TimedExitExecutionService>();
        services.AddTransient<ExceptionExecutionService>();

        return services.BuildServiceProvider();
    }

    /// <summary>
    /// 実行モード文字列を列挙型へ変換します。
    /// </summary>
    /// <param name="value">設定ファイル上の実行モードです。</param>
    /// <returns>実行モードを返します。</returns>
    private static ExecutionMode ParseExecutionMode(string value)
    {
        return Enum.TryParse<ExecutionMode>(value, ignoreCase: true, out var mode)
            ? mode
            : ExecutionMode.Immediate;
    }
}