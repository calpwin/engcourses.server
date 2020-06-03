using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EngCourses.Controllers;
using EngCourses.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging.EventSource;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Newtonsoft.Json;
using Xunit;

namespace EngCoursesUploader
{
    public class Uploader
    {
        const string Domain = "https://britlex.ru/";
        private readonly HttpClient Client = new HttpClient();

        private readonly string _projDir;
        private readonly string _resourcesDir;
        private readonly string _json;

        private List<Word> ExceptionWords = new List<Word>();

        public Uploader()
        {
            _projDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            _resourcesDir = Path.Combine(_projDir, "Resources");
            _json = File.ReadAllTextAsync(Path.Combine(_resourcesDir, "words.json")).Result;
        }

        [Fact]
        public async Task Upload()
        {
            var words = JsonConvert.DeserializeObject<Word[]>(_json);

            // foreach (var word in words.ToList().GetRange(1, 1000))
            foreach (var word in words)
            {
                try
                {
                    var fileName = Regex.Replace(word.EngText, @"[^A-Za-z\(\)_]", string.Empty);
                    var imageBytes = await DownloadFileASync(word.EngImage);
                    await File.WriteAllBytesAsync(Path.Combine(_resourcesDir, "Images", $"{word.Id}_{fileName}.jpg"),
                        imageBytes);

                    var audioBytes = await DownloadFileASync(word.EngAudio);
                    await File.WriteAllBytesAsync(Path.Combine(_resourcesDir, "Audio", $"{word.Id}_{fileName}.mp3"),
                        audioBytes);
                    
                    var audioBytesExtended = await DownloadFileASync(word.EngAudioExtend);
                    await File.WriteAllBytesAsync(Path.Combine(_resourcesDir, "Audio", $"{word.Id}_{fileName}_extended.mp3"),
                        audioBytesExtended);
                }
                catch (Exception exception)
                {
                    word.Exception = exception.ToString();
                    ExceptionWords.Add(word);
                }
            }

            await File.WriteAllTextAsync(Path.Combine(_resourcesDir, "exceptions.json"), JsonConvert.SerializeObject(ExceptionWords.ToArray()));
        }

        private async Task<byte[]> DownloadFileASync(string relevantUri)
        {
            relevantUri = relevantUri.Replace("\t", string.Empty).Trim();
            
            if (relevantUri.Length < 1) throw new Exception($"relevantUri length < 1 {relevantUri}");
            
            var urlStr = Domain + relevantUri;
            if (!Uri.TryCreate(urlStr, UriKind.Absolute, out var url)) throw new Exception($"Try create uri faild {urlStr}");

            var bytes = await Client.GetByteArrayAsync(url);
            
            if (bytes.Length < 10) throw new Exception($"Bytes.Length < 10. {bytes}");

            return bytes;
        }
    }
}