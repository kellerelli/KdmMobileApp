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
    [Activity(Label = "WeaopnSelectionActivity")]
    public class WeaopnSelectionActivity : ListActivity
    {
        Weapon weapon_clicked = null;
        List<string> available_weapons = new List<string>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //SetContentView(Resource.Layout.MonsterSelection);
            // Create your application here
            foreach (string weapon in MainActivity.weapons.Keys)
            {
                available_weapons.Add(weapon);
            }

            this.ListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, available_weapons);
        }

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
                MainActivity.CurrentWeapon = MainActivity.weapons[available_weapons[position]]; ;
                var main = new Intent(this, typeof(MainActivity));
                StartActivity(main);
                this.Finish();
        }
    }
}