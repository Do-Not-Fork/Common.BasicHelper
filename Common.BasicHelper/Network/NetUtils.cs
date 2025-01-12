﻿using Common.BasicHelper.Utils.Extensions;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace Common.BasicHelper.Network;

public class NetUtils
{
    /// <summary>
    /// 检验是否拥有网络连接
    /// </summary>
    /// <param name="target">测试的目标</param>
    /// <param name="timeOutMilliseconds">等待毫秒数</param>
    /// <returns>是否拥有网络连接</returns>
    public static bool IsWebConected(string target, int timeOutMilliseconds)
    {
        var pingSender = new Ping();
        var pingOptions = new PingOptions
        {
            DontFragment = true
        };

        var data = "";
        var buffer = Encoding.UTF8.GetBytes(data);

        var pingReply = pingSender.Send(target,
            timeOutMilliseconds, buffer, pingOptions);
        var strInfo = pingReply.Status.ToString();

        if (strInfo.Equals("Success"))
            return true;
        return false;
    }

    /// <summary>
    /// 从服务器下载文件
    /// </summary>
    /// <param name="serverFilePath">服务器上的文件位置</param>
    /// <param name="targetPath">存储到本地的文件位置</param>
    public static void DownloadFile(string serverFilePath, string targetPath)
    {
        var request = (HttpWebRequest)WebRequest.Create(serverFilePath);
        var respone = request.GetResponse();

        var netStream = respone.GetResponseStream();
        var fileStream = new FileStream(targetPath, FileMode.Create);

        var read = new byte[1024];
        var realReadLen = netStream.Read(read, 0, read.Length);

        while (realReadLen > 0)
        {
            fileStream.Write(read, 0, realReadLen);
            realReadLen = netStream.Read(read, 0, read.Length);
        }

        fileStream.CloseAndDispose();
        netStream.CloseAndDispose();
        respone.CloseAndDispose();
    }

    /// <summary>
    /// 从服务器下载文件
    /// </summary>
    /// <param name="serverFilePath">服务器上的文件位置</param>
    /// <param name="targetPath">存储到本地的文件位置</param>
    public static void WebDownloadFile(string serverFilePath, string targetPath)
    {
        var webClient = new WebClient();

        webClient.DownloadFile(serverFilePath, targetPath);

        webClient.CloseAndDispose();
    }
}
