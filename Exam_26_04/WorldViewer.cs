using Exam_26_04.DbManager;
using Exam_26_04.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam_26_04
{
    public class WorldViewer
    {
        const int MAINMENU_CNT = 4;
        const int CITIESMENU_CNT = 5;

        private List<Country> countries;
        private List<City> cities;
        private List<Street> streets;

        private CitiesService citiesService;
        private CountriesService countriesService;
        private StreetService streetService;

        public WorldViewer()
        {
            countries = new List<Country>();
            cities = new List<City>();
            streets = new List<Street>();

            citiesService = new CitiesService();
            countriesService = new CountriesService();
            streetService = new StreetService();
        }

        public void Run()
        {
            Console.WriteLine("Custom World Viewer and Editor v1.0.0.");
            Console.WriteLine("Добро пожаловать! Нажмите любую кнопку, чтобы продолжить.");
            Console.ReadLine();

            bool flag = true;
            while (flag)
            {
                cities = citiesService.SelectCities();
                countries = countriesService.SelectCountries();
                streets = streetService.SelectStreets();
                switch (MainMenu())
                {
                    case 1:
                        {
                            PrintCountries();
                            Console.WriteLine("Введите индекс: ");

                            if (int.TryParse(Console.ReadLine(), out int countryIndex))
                            {
                                if (countryIndex > 0 && countries.Count >= countryIndex)
                                {
                                    Console.WriteLine($"Города страны \"{countries[countryIndex - 1].Name}\":");
                                    PrintCities(countryIndex - 1);

                                    int cityIndex = 0;
                                    switch (CitiesMenu())
                                    {

                                        case 1:
                                            {
                                                Console.WriteLine("Введите название: ");
                                                City city = new City()
                                                {
                                                    Name = Console.ReadLine(),
                                                    CountryId = countries[countryIndex - 1].Id
                                                };
                                                citiesService.InsertCity(city);
                                                break;
                                            }
                                        case 2:
                                            {
                                                Console.WriteLine("Введите индекс: ");
                                                if (int.TryParse(Console.ReadLine(), out cityIndex))
                                                {
                                                    if (cityIndex > 0 && cities.Count >= cityIndex)
                                                    {
                                                        City city = cities[countryIndex - 1];
                                                        Console.WriteLine("Введите новое название: ");
                                                        string newName = Console.ReadLine();
                                                        citiesService.UpdateCity(city, newName);
                                                    }
                                                }
                                                break;
                                            }
                                        case 3:
                                            {
                                                Console.WriteLine("Введите индекс: ");
                                                if (int.TryParse(Console.ReadLine(), out cityIndex))
                                                {
                                                    if (cityIndex > 0 && cities.Count >= cityIndex)
                                                    {
                                                        City city = cities[countryIndex - 1];
                                                        citiesService.DeleteCity(city);
                                                    }
                                                }
                                                break;
                                            }
                                        case 4:
                                            {
                                                Console.WriteLine("Введите индекс города: ");
                                                if (int.TryParse(Console.ReadLine(), out cityIndex))
                                                {
                                                    if (cityIndex > 0 && cities.Count >= cityIndex)
                                                    {
                                                        PrintStreets(cityIndex - 1);
                                                        int menuStreet = StreetsMenu();
                                                        if (menuStreet == 1)
                                                        {
                                                            Console.WriteLine("Введите название улицы: ");
                                                            string streetName = Console.ReadLine();
                                                            Street street = new Street()
                                                            {
                                                                Name = streetName,
                                                                CityId = cities[cityIndex - 1].Id
                                                            };

                                                            streetService.InsertStreet(street);
                                                        }
                                                        else if (menuStreet == 2)
                                                        {
                                                            Console.WriteLine("Введите индекс улицы: ");
                                                            if (int.TryParse(Console.ReadLine(), out int index))
                                                            {
                                                                if (index > 0 && index <= streets.Count)
                                                                {
                                                                    streetService.DeleteStreet(streets[index - 1]);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                break;
                                            }
                                    }
                                }
                            }
                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine("Введите название страны!");
                            string cityName = Console.ReadLine();
                            countriesService.InsertCountry(new Country
                            {
                                Name = cityName
                            });
                            Console.ReadLine();
                            break;
                        }
                    case 3:
                        Environment.Exit(0);
                        break;
                }
            }
        }
        #region Menus
        private int MainMenu()
        {
            Console.Clear();
            Console.WriteLine("Выберите действие: ");
            Console.WriteLine("1) Выбрать страну");
            Console.WriteLine("2) Добавить страну");
            Console.WriteLine("3) Выйти из приложения");

            if (int.TryParse(Console.ReadLine(), out int menu))
            {
                if (menu > 0 && menu <= MAINMENU_CNT)
                {
                    return menu;
                }
            }
            return -1;
        }
        private int CitiesMenu()
        {
            Console.WriteLine("Введите действие: ");
            Console.WriteLine("1) Добавить город");
            Console.WriteLine("2) Изменить название города");
            Console.WriteLine("3) Удалить город");
            Console.WriteLine("4) Посмотреть улицы");
            Console.WriteLine("5) Вернуться");

            if (int.TryParse(Console.ReadLine(), out int menu))
            {
                if (menu > 0 && menu <= CITIESMENU_CNT)
                {
                    return menu;
                }
            }
            return -1;
        }
        private int StreetsMenu()
        {
            Console.WriteLine("Выберите действие: ");
            Console.WriteLine("1) Добавить улицу");
            Console.WriteLine("2) Удалить улице");

            if (int.TryParse(Console.ReadLine(), out int menu))
            {
                if (menu > 0 && menu <= 2)
                {
                    return menu;
                }
            }
            return -1;
        }
        #endregion
        #region Printers
        private void PrintStreets(int cityIndex)
        {
            streets = streetService.SelectStreets();
            foreach (var street in streets)
            {
                if (street.CityId == cities[cityIndex].Id)
                {
                    Console.WriteLine($"\t\t{street}");
                }
            }
        }
        private void PrintCities(int countryIndex)
        {
            cities = citiesService.SelectCities();
            foreach (var city in cities)
            {
                if (city.CountryId == countries[countryIndex].Id)
                {
                    Console.WriteLine($"\t{city}");
                }
            }
        }

        private void PrintCountries()
        {
            countries = countriesService.SelectCountries();

            for (int i = 0; i < countries.Count; i++)
            {
                Console.WriteLine($"название : {countries[i].Name}");
            }
        }
        #endregion

    }
}
