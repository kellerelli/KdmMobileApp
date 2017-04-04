using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App2
{
    [Activity(Label = "MonsterSelectionActivity")]
    public class MonsterSelectionActivity : ListActivity
    {
        Monster monster_clicked = null;
        List<string> available_monsters = new List<string>();
        List<int> available_levels = new List<int>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //SetContentView(Resource.Layout.MonsterSelection);
            // Create your application here
            foreach (string monster in MainActivity.monsters.Keys)
            {
                available_monsters.Add(monster);
            }

            this.ListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, available_monsters);
        }

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            if (monster_clicked == null)
            {
                monster_clicked =  MainActivity.monsters[available_monsters[position]];
                available_levels = monster_clicked.AvailableLevels();
                this.ListAdapter = new ArrayAdapter<int>(this, Android.Resource.Layout.SimpleListItem1, monster_clicked.AvailableLevels());
            } else {
                monster_clicked.LevelFighting = monster_clicked.levels[available_levels[position]];
                MainActivity.CurrentMonster = monster_clicked;
                var main = new Intent(this, typeof(MainActivity));
                StartActivity(main);
                this.Finish();
            }
        }
    }
}