﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Common.BasicHelper.Core.Shell;

public static class CommandsExecutor
{
    /// <summary>
    /// 获取命令执行输出
    /// </summary>
    /// <param name="command">命令</param>
    /// <param name="args">参数</param>
    /// <param name="findInPath">是否在 Path 中寻找</param>
    /// <param name="action">对启动信息的行动</param>
    /// <returns>命令执行输出</returns>
    public static string GetExecutionResult
    (
        string command,
        string args,
        bool findInPath = false,
        Action<ProcessStartInfo>? action = null
    )
    {
        if (findInPath)
            command = EnvironmentHelper.GetFilePathInPaths(command);

        var psi = new ProcessStartInfo()
        {
            FileName = command,
            Arguments = args,
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = false,
            CreateNoWindow = true,
        };
        action?.Invoke(psi);

        var process = new Process
        {
            StartInfo = psi,
        };

        process.Start();

        var output = process.StandardOutput.ReadToEnd();

        process.WaitForExit();

        return output;
    }

    /// <summary>
    /// 异步获取命令执行输出
    /// </summary>
    /// <param name="command">命令</param>
    /// <param name="args">参数</param>
    /// <param name="findInPath">是否在 Path 中寻找</param>
    /// <param name="action">针对启动信息的动作</param>
    /// <param name="token">取消口令</param>
    /// <returns>命令执行输出</returns>
    public static async Task<string> GetExecutionResultAsync
    (
        string command,
        string args,
        bool findInPath = false,
        Action<ProcessStartInfo>? action = null,
        CancellationToken? token = null
    )
    {
        if (findInPath)
            await Task.Run(() => command = EnvironmentHelper.GetFilePathInPaths(command));

        var psi = new ProcessStartInfo()
        {
            FileName = command,
            Arguments = args,
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = false,
            CreateNoWindow = true,
        };
        action?.Invoke(psi);

        var process = new Process
        {
            StartInfo = psi,
        };

        process.Start();

        var output = process.StandardOutput.ReadToEnd();

        await Task.Run(() =>
        {
            if (token is null) process.WaitForExit();

            while (!process.HasExited)
            {
                if (token is not null && token.Value.IsCancellationRequested)
                {
                    process.Kill();

                    break;
                }
            }
        });

        return output;
    }

}

public static class CommandsExecutorExtensions
{

    /// <summary>
    /// 将当前字符串作为命令执行, 返回命令执行的输出
    /// </summary>
    /// <param name="command">命令</param>
    /// <param name="args">命令参数</param>
    /// <param name="action">针对启动信息的行动</param>
    /// <returns>执行的输出</returns>
    public static string ExecuteAsCommand
    (
        this string command,
        string? args = null,
        bool findInPath = true,
        Action<ProcessStartInfo>? action = null
    )
        => CommandsExecutor.GetExecutionResult
        (
            command,
            args ?? "",
            findInPath,
            action
        );


    /// <summary>
    /// 将当前字符串作为命令执行, 异步获取命令执行输出
    /// </summary>
    /// <param name="command">命令</param>
    /// <param name="args">参数</param>
    /// <param name="findInPath">是否在 Path 中寻找</param>
    /// <param name="action">针对启动信息的动作</param>
    /// <param name="token">取消口令</param>
    /// <returns>命令执行输出</returns>
    public static Task<string> ExecuteAsCommandAsync
    (
        this string command,
        string? args = null,
        bool findInPath = true,
        Action<ProcessStartInfo>? action = null,
        CancellationToken? token = default
    )
        => CommandsExecutor.GetExecutionResultAsync
        (
            command,
            args ?? "",
            findInPath,
            action,
            token
        );
}
