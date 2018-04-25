//package com.example.rotembrownstein.heathbeatapplication;
using Android.Content;

using Java.Net;
using System;
using Microsoft.WindowsAzure.MobileServices;
using Java.Lang;

namespace HBApp
{
    public class AzureServiceAdapter
    {
        private string mMobileBackendUrl = "http://ehealthheartrate.azurewebsites.net/";
        private Context mContext;
        private MobileServiceClient mClient;
        private static AzureServiceAdapter mInstance = null;

        private AzureServiceAdapter(Context context)
        {
            mContext = context;
            try
            {
                //mClient = new MobileServiceClient(mMobileBackendUrl, mContext);
                mClient = new MobileServiceClient(mMobileBackendUrl);
            }
            catch (MalformedURLException e)
            {
                //TO DO: exception
            }
        }

        public static void Initialize(Context context)
        {
            if (mInstance == null)
            {
                mInstance = new AzureServiceAdapter(context);
            }
        }

        public static AzureServiceAdapter getInstance()
        {
            if (mInstance == null)
            {
                throw new IllegalStateException("AzureServiceAdapter is not initialized");
            }
            return mInstance;
        }

        public MobileServiceClient getClient()
        {
            return mClient;
        } 
    }
}