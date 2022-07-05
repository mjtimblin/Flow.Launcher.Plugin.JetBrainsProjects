using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Flow.Launcher.Plugin;

namespace Flow.Launcher.Plugin.JetBrainsProjects
{
    public class JetBrainsProjects : IPlugin
    {
        private List<Product> _products;

        public void Init(PluginInitContext context)
        {
            _products = File.ReadLines($"{context.CurrentPluginMetadata.PluginDirectory}\\products.jsonl").ToList()
                .ConvertAll(line => JsonSerializer.Deserialize<Product>(line));
        }

        public List<Result> Query(Query query)
        {
            var results = new List<Result>();
            if (string.IsNullOrEmpty(query.Search)) return results;
            var keyword = query.Search;
            
            var searchList = _products.Where(x => x.Name.ToLower().Contains(keyword.ToLower()));
            
            results.AddRange(searchList.Select(s => new Result
            {
                Title = s.Name,
                SubTitle = s.FolderName,
                IcoPath = $"src\\Images\\{s.Name}.png"
            }));

            return results;
        }
    }
}