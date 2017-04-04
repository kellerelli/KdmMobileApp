using Android.App;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using System.Json;
using System.Net;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Java.Util;
using Android.Content;

namespace App2
{
    [Activity(Label = "App2", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public static readonly Dictionary<string, Monster> monsters = new Dictionary<string, Monster>();
        public static readonly Dictionary<string, Weapon> weapons = new Dictionary<string, Weapon>();
        public static Monster CurrentMonster = null;
        public static Weapon CurrentWeapon = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            Button monster_button = FindViewById<Button>(Resource.Id.ChooseMonsterbutton);
            TextView monster_text = FindViewById<TextView>(Resource.Id.chosenMonster);

            Button weapon_button = FindViewById<Button>(Resource.Id.ChoosenWeaponButton);
            TextView weapon_text = FindViewById<TextView>(Resource.Id.ChosenWeapon);

            Button showdown_button = FindViewById<Button>(Resource.Id.StartShowdown);

            if (CurrentMonster != null)
            {
                monster_text.Text = CurrentMonster.name + " Level " + CurrentMonster.LevelFighting.level;
            }

            if (CurrentWeapon != null)
            {
                weapon_text.Text = CurrentWeapon.name;
            }

            monster_button.Click += async (sender, e) => {
                if (monsters.Count == 0)
                {
                    string url = "http://dry-mesa-82797.herokuapp.com/monsters?format=json";
                    // Fetch the weather information asynchronously, 
                    // parse the results, then update the screen:
                     JsonValue json = await FetchWeatherAsync(url);
                    ParseMonsters(json);
                }
                var activity2 = new Intent(this, typeof(MonsterSelectionActivity));
                StartActivity(activity2);
            };

            weapon_button.Click += async (sender, e) => {
                if (weapons.Count == 0)
                {
                    string url = "http://dry-mesa-82797.herokuapp.com/weapons?format=json";
                    // Fetch the weather information asynchronously, 
                    // parse the results, then update the screen:
                    JsonValue json = await FetchWeatherAsync(url);
                    ParseWeapons(json);
                }
                var activity2 = new Intent(this, typeof(WeaopnSelectionActivity));
                StartActivity(activity2);
            };

            showdown_button.Click += async (sender, e) => {
                if (weapons.Count == 0)
                {
                    string url = "http://dry-mesa-82797.herokuapp.com/weapons?format=json";
                    // Fetch the weather information asynchronously, 
                    // parse the results, then update the screen:
                    JsonValue json = await FetchWeatherAsync(url);
                    ParseWeapons(json);
                }
                var showdown_activity = new Intent(this, typeof(ShowdownActivity));
                StartActivity(showdown_activity);
            };

        }

        private void ParseMonsters(JsonValue responseString)
        {
            foreach (var monster in JObject.Parse(responseString))
            {
                Monster monster_obj = new Monster(monster.Key, monster.Value);
                monsters.Add(monster.Key, monster_obj);
            }
        }

        private void ParseWeapons(JsonValue responseString)
        {
            foreach (var weapon in JObject.Parse(responseString))
            {
                Weapon weapon_obj = JsonConvert.DeserializeObject<Weapon>(weapon.Value.ToString());
                weapons.Add(weapon.Key, weapon_obj);
            }
        }

    // Gets weather data from the passed URL.
    private async Task<JsonValue> FetchWeatherAsync(string url)
        {
            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["auth_token"] = "";
            request.Headers["key_id"] = "";

            // Send the request to the server and wait for the response:
            using (WebResponse response = await request.GetResponseAsync())
            {
                List<Monster> monster_response;
                string responseString = string.Empty;
                // Get a stream representation of the HTTP web response:
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                   responseString = sr.ReadToEnd();
                }
                response.Close();
                return responseString;
            }
        }

    }

    public class Level
    {
        public int accuracy { get; set; }
        public int damage { get; set; }
        public int evasion { get; set; }
        public string id { get; set; }
        public int level { get; set; }
        public int luck { get; set; }
        public int movement { get; set; }
        public int speed { get; set; }
        public int toughness { get; set; }
    }

    public class Monster
    {
        public Dictionary<int,Level> levels { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public Level LevelFighting { get; set; }
        public Monster(String name, JToken monster )
        {
            this.levels = new Dictionary<int, Level>();
            this.name = name;
            this.id = monster["id"].ToObject<String>();
            foreach ( var level_json in monster["levels"])
            {
                var level = JsonConvert.DeserializeObject<Level>(level_json.ToString());
                this.levels.Add(level.level, level);
            }
        }

        public Dictionary<String, int> AttrsForLevel()
        {
            Dictionary<String, int> listOfAttr = new Dictionary<String, int>();
            listOfAttr["accuracy"] = this.LevelFighting.accuracy;
            listOfAttr["damage"] = this.LevelFighting.damage;
            listOfAttr["evasion"] = this.LevelFighting.evasion;
            listOfAttr["luck"] = this.LevelFighting.luck;
            listOfAttr["movement"] = this.LevelFighting.movement;
            listOfAttr["speed"] = this.LevelFighting.speed;
            listOfAttr["toughness"] = this.LevelFighting.toughness;
            return listOfAttr;
        }

        public List<int> AvailableLevels()
        {
            List<int> available_levels = new List<int>();
            foreach (int level in levels.Keys)
            {
                available_levels.Add(level);
            }
            return available_levels;
        }
    }

    public class Weapon
    {
        public string name { get; set; }
        public int success { get; set; }
        public int speed { get; set; }
        public int strenght { get; set; }
    }

}

