﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SQLite;

namespace Encounter1
{
    [Activity(Label = "RegActivity")]
    public class RegActivity : Activity
    {
        EditText txtUsername;
        EditText txtPassword;
        Button btnCreate;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.NewUser);
            // Create your application here  
            btnCreate = FindViewById<Button>(Resource.Id.button1);
            txtUsername = FindViewById<EditText>(Resource.Id.editText2);
            txtPassword = FindViewById<EditText>(Resource.Id.editText1);
            btnCreate.Click += Btncreate_Click;
        }
        private void Btncreate_Click (object sender, EventArgs e)
        {
            try
            {
                string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "users.db3");
                var db = new SQLiteConnection(dpPath);
                db.CreateTable<LoginTable>();
                LoginTable tbl = new LoginTable
                {
                    username = txtUsername.Text,
                    password = txtPassword.Text
                };
                db.Insert(tbl);
                Toast.MakeText(this, "Record Added Successfully...,", ToastLength.Short).Show();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
            }
            StartActivity(typeof(MainActivity));
        }

    }





}