﻿using ADOTabular;
using comm= DaxStudio.Common;
using DaxStudio.Common.Extensions;
using DaxStudio.Interfaces;
using DaxStudio.UI.Events;
using DaxStudio.UI.Interfaces;
using DaxStudio.UI.JsonConverters;
using DaxStudio.UI.Model;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Linq;

namespace DaxStudio.UI.Utils
{
    [PartCreationPolicy(CreationPolicy.Shared)]
    public abstract class JsonSettingProviderBase : ISettingProvider
    {
        public abstract string SettingsPath { get; }

        public string SettingsFile { get; }

        private IGlobalOptions _options;
        private IDictionary<string, object> _optionsDict;
        private static readonly object _locker = new Object();
        private RetryPolicy _retry;
        //[Import]
        public IGlobalOptions Options {
            get => _options;
            set {
                _options = value;
                _optionsDict = _options.AsDictionary();
            }
        }

        protected JsonSettingProviderBase()
        {
            // todo - if running portable use local path, otherwise use AppData
            SettingsFile = Path.Combine(SettingsPath, "settings.json");
            //ReadSettings();
            ConfigureRetryPolicy();
        }

        private void ConfigureRetryPolicy() {
            _retry = Policy
                  .Handle<System.IO.IOException>()
                  .WaitAndRetry(3, retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        (exception, timespan, retryCount, context) =>
                        {
                            
                            var msg =
                                $"An error occurred attempting to write to settings.json: {exception.Message}\n(retry: {retryCount})";
                            // TODO - pass in IEventAggregator instance
                            //_eventAggregator.PublishOnUIThreadAsync(new OutputMessage(MessageType.Warning, msg));
                            Log.Warning(exception, Common.Constants.LogMessageTemplate, nameof(JsonSettingProviderBase),
                                "RetryPolicy", msg);
                        }
            );
        }

        //private void ReadSettings()
        //{
        //    // deserialize JSON directly from a file
        //    using (StreamReader file = File.OpenText(settingsFile))
        //    {
        //        JsonSerializer serializer = new JsonSerializer();
        //        serializer.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
        //        serializer.NullValueHandling = NullValueHandling.Ignore;
        //        Options = (IGlobalOptions)serializer.Deserialize(file, typeof(GlobalOptions) );
        //    }

        //}

        #region Static Members
        public bool SettingsFileExists()
        {
            return File.Exists(SettingsFile);
        }

        public virtual bool IsRunningPortable => false;

        public string LogPath => Path.Combine(SettingsPath, "logs");

        #endregion


        public ObservableCollection<IDaxFile> GetFileMRUList()
        {
            return Options.RecentFiles;
        }
        

        public ObservableCollection<string> GetServerMRUList()
        {
            return Options.RecentServers;
        }

        public T GetValue<T>(string subKey, T defaultValue)
        {
            return ((T)_optionsDict[subKey]);
        }
        
        public void SetValue<T>(string subKey, T value, bool isInitializing, object options,  string propertyName)
        {
                if (isInitializing) return;
                _optionsDict[subKey] = value;
                // write json file
                SaveSettingsFile();
        }

        public void SetValue(string subKey, DateTime value, bool isInitializing, object options, string propertyName)
        {
                if (isInitializing) return;
                _optionsDict[subKey] = value.ToString(comm.Constants.IsoDateFormat, CultureInfo.InvariantCulture);
                // write json file
                SaveSettingsFile();
        }

        private void SaveSettingsFile()
        {
            _retry.Execute(() =>{ 
                lock (_locker)
                {
                    using (StreamWriter file = File.CreateText(SettingsFile))
                    {
                        JsonSerializer serializer = new JsonSerializer
                        {
                            Formatting = Formatting.Indented,
                            NullValueHandling = NullValueHandling.Ignore,
                            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
                        };
                        serializer.Serialize(file, Options);
                    }
                }
            });
        }

        public bool IsFileLoggingEnabled()
        {
            return GetValue("IsFileLoggingEnabled",false);
        }

        public void SaveFileMRUList(IDaxFile file)
        {
            var existingItem = Options.RecentFiles.FirstOrDefault(f => f.FullPath.Equals(file.FullPath, StringComparison.CurrentCultureIgnoreCase));
            // file does not exist in list so add it as the first item
            if (existingItem == null)
            {
                Options.RecentFiles.Insert(0, file);
                while (Options.RecentFiles.Count > comm.Constants.MaxRecentFiles)
                {
                    Options.RecentFiles.RemoveAt(Options.RecentFiles.Count - 1);
                }
                SaveSettingsFile();
                return;
            }

            var existingIndex = Options.RecentFiles.IndexOf(existingItem);
            // file is already first in the list so do nothing
            if (existingIndex == 0) return;

            // otherwise move the file to first in the list
            Options.RecentFiles.Move(existingIndex, 0);

            SaveSettingsFile();
        }

        public void SaveServerMRUList(string currentServer)
        {
            var existingIdx = Options.RecentServers.IndexOf(currentServer);

            if (!string.IsNullOrEmpty(currentServer))
            {
                // server is already first in the list
                if (existingIdx == 0) return; // do nothing

                if (existingIdx > 0)
                {
                    // server exists, make it first in the list
                    Options.RecentServers.Move(existingIdx, 0);
                }
                else
                {
                    // server does not exist in list, so insert it as the first item
                    Options.RecentServers.Insert(0, currentServer);
                    while (Options.RecentServers.Count > comm.Constants.MaxMruSize)
                    {
                        Options.RecentServers.RemoveAt(Options.RecentServers.Count - 1);
                    }
                }
            }
            SaveSettingsFile();
        }

        public void Initialize(IGlobalOptions options)
        {
            // load settings from settings.json
            var json = "{}";
            if (SettingsFileExists()) { json = File.ReadAllText(SettingsFile);  }

            var settings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                NullValueHandling = NullValueHandling.Ignore
            };
            settings.Converters.Add(new DaxFileConverter());
            try
            {
                JsonConvert.PopulateObject(json, options, settings);
            }
            catch(Exception ex)
            {
                Log.Error(ex, "{class} {method} {message}", nameof(JsonSettingProviderBase), nameof(Initialize), "Error reading json in settings file, using default settings");
                // using default settings by passing in an empty json object
                JsonConvert.PopulateObject("{}", options, settings);
            }
            Options = options;
            Options.IsRunningPortable = this.IsRunningPortable;
        }

        public void Reset()
        {
            File.Delete(SettingsFile);
        }
            
    }
}
