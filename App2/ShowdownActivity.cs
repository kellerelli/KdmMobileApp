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
using Android.Util;

namespace App2
{
    [Activity(Label = "ShowdownActivity")]
    public class ShowdownActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ShowDown);
            Dictionary<String, int> AttrsForLevel = MainActivity.CurrentMonster.AttrsForLevel();
            LinearLayout monsterStats = FindViewById<LinearLayout> (Resource.Id.MonsterAttr);

            int scale = (int) Resources.DisplayMetrics.Density;

            foreach (KeyValuePair<String, int> entry in AttrsForLevel)
            {
                String AttrName = entry.Key;
                int AttrAmount = entry.Value;

                TableLayout attr_layout = new TableLayout(this);
                TableRow attr_row = new TableRow(this);

                TableLayout.LayoutParams parms = new TableLayout.LayoutParams(105, 35); //Width, Height
                TableLayout.LayoutParams buttonlay = new TableLayout.LayoutParams(35, 35);
                attr_layout.LayoutParameters= parms;

                Button addBtn = new Button(this);
            addBtn.Text = "+";

                Button minusBtn = new Button(this);
            minusBtn.Text = "-";

                TextView amount = new TextView(this);
            amount.Text = AttrAmount.ToString();

                TextView attr_name = new TextView(this);
            attr_name.Text = AttrName;

            attr_row.AddView(minusBtn);
            attr_row.AddView(amount);
            attr_row.AddView(addBtn);
            attr_layout.AddView(attr_row,0);

            monsterStats.AddView(attr_name);
            monsterStats.AddView(attr_layout);

                addBtn.Click += (sender, e) => {
                    amount.Text = (Convert.ToInt32(amount.Text) + 1).ToString();
                };


                minusBtn.Click += (sender, e) => {
                    amount.Text = (Convert.ToInt32(amount.Text) - 1).ToString();
                };

            }
            // Create your application here
        }
    }



}