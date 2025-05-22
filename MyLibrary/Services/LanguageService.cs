using MyLibrary.Resources.Languages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyLibrary.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly INotificationService _notificationService;
        public LanguageService(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public void SetLanguage(string language)
        {
            try
            {
                var languages = GetAvailableLanguages("MyLibrary.Resources.Languages.Strings", Assembly.GetExecutingAssembly());
                if (languages.Contains(new CultureInfo(language.ToLower())))
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
                }
                else
                {
                    _notificationService.ShowError(title: "Language Error", message: $"Language {language} is not available.");
                }
            }
            catch(Exception err)
            {
                _notificationService.ShowError(title: "Language Error", message: $"Error setting language: {err.Message}");
            }
        }
        private List<CultureInfo> GetAvailableLanguages(string baseName, Assembly assembly)
        {
            var cultures = new List<CultureInfo>();
            foreach(var culture in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                try
                {
                    ResourceManager rm = new ResourceManager(baseName, assembly);
                    var rs = rm.GetResourceSet(culture: culture, createIfNotExists: true, tryParents: false);
                    if(rs != null)
                    {
                        cultures.Add(culture);
                    }
                }
                catch (Exception err)
                {
                    _notificationService.ShowError(title: "Language Error", message: $"Error loading language {culture.Name}: {err.Message}");
                }
            }
            return cultures;
        }
    }
}
