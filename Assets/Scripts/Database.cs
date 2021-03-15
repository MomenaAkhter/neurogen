using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NeuroGen
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Model
    {
        public string content;
        public int extension_id;
        public float fitness;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ModelCollection
    {
        public IntPtr models;
        public int size;
    }

    public class Database
    {
#if UNITY_IPHONE
        [DllImport("__Internal")]
#else
        [DllImport("Database")]
#endif
        public static extern int GetSqliteVersion();

#if UNITY_IPHONE
        [DllImport("__Internal")]
#else
        [DllImport("Database")]
#endif
        public static extern int ConnectAndSetup(string path);

#if UNITY_IPHONE
        [DllImport("__Internal")]
#else
        [DllImport("Database")]
#endif
        public static extern int Disconnect();

#if UNITY_IPHONE
        [DllImport("__Internal")]
#else
        [DllImport("Database")]
#endif
        public static extern int AddModel(string content, int extensionId, float fitness);

        public static void AddModel(object model, int extensionId, float fitness)
        {
            AddModel(JsonUtility.ToJson(model), extensionId, fitness);
        }

#if UNITY_IPHONE
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
#else
        [DllImport("Database", CallingConvention = CallingConvention.Cdecl)]
#endif
        public static extern IntPtr GetModel(int id);

#if UNITY_IPHONE
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
#else
        [DllImport("Database", CallingConvention = CallingConvention.Cdecl)]
#endif
        public static extern void UnloadModel(IntPtr model);

#if UNITY_IPHONE
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
#else
        [DllImport("Database", CallingConvention = CallingConvention.Cdecl)]
#endif
        public static extern void UnloadCollection(IntPtr model);

#if UNITY_IPHONE
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
#else
        [DllImport("Database", CallingConvention = CallingConvention.Cdecl)]
#endif
        public static extern int TrimModelsTable(int count, int extensionId);

#if UNITY_IPHONE
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
#else
        [DllImport("Database", CallingConvention = CallingConvention.Cdecl)]
#endif
        private static extern IntPtr GetBestModelsCollection(int count, int extensionId);

#if UNITY_IPHONE
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
#else
        [DllImport("Database", CallingConvention = CallingConvention.Cdecl)]
#endif
        public static extern int GetExtensionId(string name);

        public static List<Model> GetBestModels(int count, int extensionId)
        {
            IntPtr collectionPtr = Database.GetBestModelsCollection(count, extensionId);
            ModelCollection collection = Marshal.PtrToStructure<ModelCollection>(collectionPtr);

            List<Model> models = new List<Model>();
            if (collection.size > 0)
            {
                IntPtr[] modelPointers = new IntPtr[collection.size];
                Marshal.Copy(collection.models, modelPointers, 0, collection.size);

                for (int i = 0; i < collection.size; i++)
                    models.Add(Marshal.PtrToStructure<Model>(modelPointers[i]));
            }
            Database.UnloadCollection(collectionPtr);

            return models;
        }
    }
}