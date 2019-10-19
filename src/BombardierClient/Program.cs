﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace BombardierClient
{
    class Program
    {
        private static readonly HttpClient _httpClient;
        private static readonly HttpClientHandler _httpClientHandler;

        private static Dictionary<PlatformID, string> _bombardierUrls = new Dictionary<PlatformID, string>()
        {
            { PlatformID.Win32NT, "https://github.com/codesenberg/bombardier/releases/download/v1.2.4/bombardier-windows-amd64.exe" },
            { PlatformID.Unix, "https://github.com/codesenberg/bombardier/releases/download/v1.2.4/bombardier-linux-amd64" },
        };

        static Program()
        {
            // Configuring the http client to trust the self-signed certificate
            _httpClientHandler = new HttpClientHandler();
            _httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            _httpClientHandler.AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate;

            _httpClient = new HttpClient(_httpClientHandler);
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("Bombardier Client");
            Console.WriteLine(String.Join(' ', args));

            var bombardierUrl = _bombardierUrls[Environment.OSVersion.Platform];
            var bombardierFileName = Path.GetFileName(bombardierUrl);

            using (var downloadStream = await _httpClient.GetStreamAsync(bombardierUrl))
            using (var fileStream = File.Create(bombardierFileName))
            {
                await downloadStream.CopyToAsync(fileStream);
            }
        }
    }
}
