﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SocialMediaDashboard.Common.Interfaces;
using System;
using System.IO;

namespace SocialMediaDashboard.Common.Options
{
    /// <inheritdoc cref="IWritableOptions"/>
    public class WritableOptions<T> : IWritableOptions<T> where T : class, new()
    {
        private readonly IHostEnvironment _environment;
        private readonly IOptionsMonitor<T> _options;
        private readonly string _section;
        private readonly string _file;

        public WritableOptions(IHostEnvironment environment,
                               IOptionsMonitor<T> options,
                               string section,
                               string file)
        {
            _environment = environment;
            _options = options;
            _section = section;
            _file = file;
        }

        public T Value => _options.CurrentValue;
        public T Get(string name) => _options.Get(name);

        /// <inheritdoc/>
        public void Update(Action<T> applyChanges)
        {
            applyChanges = applyChanges ?? throw new ArgumentNullException(nameof(applyChanges));

            var fileProvider = _environment.ContentRootFileProvider;
            var fileInfo = fileProvider.GetFileInfo(_file);
            var physicalPath = fileInfo.PhysicalPath;

            var jObject = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(physicalPath));
            var sectionObject = jObject.TryGetValue(_section, out JToken section) ?
                JsonConvert.DeserializeObject<T>(section.ToString()) : (Value ?? new T());

            applyChanges(sectionObject);

            jObject[_section] = JObject.Parse(JsonConvert.SerializeObject(sectionObject));
            File.WriteAllText(physicalPath, JsonConvert.SerializeObject(jObject, Formatting.Indented));
        }
    }
}
