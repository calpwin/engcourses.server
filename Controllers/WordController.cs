using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using EngCourses.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EngCourses.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WordController : ControllerBase
    {
        [HttpGet("getwords")]
        public IEnumerable<Word> GetWords()
        {
            var wordJson = string.Empty;
            var embeddedProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
            using var stream = embeddedProvider.GetFileInfo("Resources/words.json").CreateReadStream();
            wordJson = new StreamReader(stream).ReadToEnd();

            var words = JsonConvert.DeserializeObject<Word[]>(wordJson);

            return words;
        }
    }
}