using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Anapher.Wpf.Swan;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Projektwoche.Umfrage.Auswertung.Core;

namespace Projektwoche.Umfrage.Auswertung.ViewModels
{
    public class MainViewModel : PropertyChangedBase
    {
        private static readonly Uri BaseUri = new Uri(ConfigurationManager.AppSettings["url"]);
        private readonly HttpClient _httpClient = new HttpClient {BaseAddress = BaseUri};

        private readonly JsonSerializerSettings _jsonSettings =
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter> {new StringEnumConverter(true)}
            };

        private AsyncRelayCommand _createCommand;
        private int? _createdEntryId;
        private SurveyViewModel _currentSurvey = new SurveyViewModel();
        private bool _isCreated;
        private RelayCommand _resetValuesCommand;
        private ObservableCollection<SchoolClass> _schoolClasses;
        private AsyncRelayCommand _updateGamesCommand;

        public MainViewModel()
        {
            _httpClient.DefaultRequestHeaders.Add("CUSTOMAUTHORIZATION", ConfigurationManager.AppSettings["secret"]);
        }

        public SurveyViewModel CurrentSurvey
        {
            get => _currentSurvey;
            set => SetProperty(value, ref _currentSurvey);
        }

        public ObservableCollection<SchoolClass> SchoolClasses
        {
            get => _schoolClasses;
            private set => SetProperty(value, ref _schoolClasses);
        }

        public ObservableCollection<ComputerGame> ComputerGames { get; } = new ObservableCollection<ComputerGame>();

        public List<FavoriteDrink> Drinks { get; } =
            Enum.GetValues(typeof(FavoriteDrink)).Cast<FavoriteDrink>().ToList();

        public List<GamingPlatform> Platforms { get; } =
            Enum.GetValues(typeof(GamingPlatform)).Cast<GamingPlatform>().ToList();

        public List<GameGenre> GameGenres { get; } =
            Enum.GetValues(typeof(GameGenre)).Cast<GameGenre>().ToList();

        public AsyncRelayCommand UpdateGamesCommand
        {
            get
            {
                return _updateGamesCommand ??
                       (_updateGamesCommand = new AsyncRelayCommand(parameter => InitializeWebInfo(false)));
            }
        }

        public RelayCommand ResetValuesCommand
        {
            get
            {
                return _resetValuesCommand ?? (_resetValuesCommand = new RelayCommand(parameter =>
                {
                    if (MessageBoxEx.Show(Application.Current.MainWindow,
                            "Sind Sie sicher, dass Sie alle Daten zurücksetzten wollen?", "Warnung",
                            MessageBoxButton.OKCancel, MessageBoxImage.Stop) == MessageBoxResult.OK)
                        CurrentSurvey = new SurveyViewModel();
                }));
            }
        }

        public int? CreatedEntryId
        {
            get => _createdEntryId;
            set => SetProperty(value, ref _createdEntryId);
        }

        public bool IsCreated
        {
            get => _isCreated;
            set => SetProperty(value, ref _isCreated);
        }

        public AsyncRelayCommand CreateCommand
        {
            get
            {
                return _createCommand ?? (_createCommand = new AsyncRelayCommand(async parameter =>
                {
                    CreatedEntryId = null;

                    var result = JsonConvert.SerializeObject(CurrentSurvey, Formatting.Indented, _jsonSettings);
                    try
                    {
                        var response = await _httpClient.PostAsync("create.php",
                            new StringContent(result, Encoding.UTF8, "application/json"));
                        if (response.StatusCode != HttpStatusCode.Created)
                        {
                            var error = await response.Content.ReadAsStringAsync();
                            MessageBoxEx.Show(Application.Current.MainWindow,
                                "An error occurred when trying to create entry.\r\n" + error, "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                            return;
                        }

                        var id = int.Parse((await response.Content.ReadAsStringAsync()).Trim());
                        CreatedEntryId = id;
                        CurrentSurvey = new SurveyViewModel();

                        IsCreated = true;
                        Task.Delay(1000).ContinueWith(task => IsCreated = false);
                    }
                    catch (Exception e)
                    {
                        MessageBoxEx.Show(Application.Current.MainWindow,
                            e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }));
            }
        }

        public async Task InitializeWebInfo(bool updateClasses = true)
        {
            var result = await _httpClient.GetAsync("info.php");
            if (result.StatusCode != HttpStatusCode.OK)
            {
                MessageBoxEx.Show(Application.Current.MainWindow,
                    "Something went terribly wrong, received invalid status code from server: " +
                    result);
                return;
            }

            var data = JsonConvert.DeserializeAnonymousType(await result.Content.ReadAsStringAsync(),
                new {classes = new List<SchoolClass>(), computerGames = new List<ComputerGame>()}, _jsonSettings);

            if (updateClasses)
                SchoolClasses = new ObservableCollection<SchoolClass>(data.classes);

            var computerGamesToRemove = ComputerGames.ToList();
            foreach (var computerGame in data.computerGames)
            {
                if (ComputerGames.Contains(computerGame))
                {
                    ComputerGames.First(x => x.Equals(computerGame)).Name = computerGame.Name;
                    computerGamesToRemove.Remove(computerGame);
                    continue;
                }
                ComputerGames.Add(computerGame);
            }

            foreach (var computerGame in computerGamesToRemove)
                ComputerGames.Remove(computerGame);
        }
    }
}