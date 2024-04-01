using RedBjorn.Utils;
using System;
using System.IO;
using UnityEngine;

namespace RedBjorn.SuperTiles.Saves
{
    public class SaveWorkStandalone : SaveWorker
    {
        string Extension = FileFormat.SaveData;
        static readonly string Directory = Path.Combine(Application.persistentDataPath, "Saves");

        public SaveWorkStandalone(ISaveWorkCallback callback) : base(callback)
        {

        }

        protected override void DoSave(byte[] data, string savename)
        {
            var filenameExt = string.Concat(savename, Extension);
            var path = Path.Combine(Directory, filenameExt);
            try
            {
                if (!System.IO.Directory.Exists(Directory))
                {
                    System.IO.Directory.CreateDirectory(Directory);
                    Log.I($"Directory at {Directory} created");
                }
                File.WriteAllBytes(path, data);
                OnSaveCompleted(true, data, savename);
            }
            catch (Exception e)
            {
                Log.E($"Can't save file at {path}. Cause {e}");
                OnSaveCompleted(false, data, savename);
            }
        }

        protected override void DoLoad(string savename)
        {
            var filenameExt = string.Concat(savename, Extension);
            var path = Path.Combine(Directory, filenameExt);
            try
            {
                if (File.Exists(path))
                {
                    var bytes = File.ReadAllBytes(path);
                    OnLoadCompleted(true, bytes, savename);
                }
                else
                {
                    Log.E($"Can't load file at {path}. No file at path");
                    OnLoadCompleted(false, null, savename);
                }
            }
            catch (Exception e)
            {
                Log.E($"Can't load file at {path}. Cause {e}");
                OnLoadCompleted(false, null, savename);
            }
        }

        protected override void DoDelete(string savename)
        {
            var filenameExt = string.Concat(savename, Extension);
            var path = Path.Combine(Directory, filenameExt);
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    OnDeleteCompleted(true, savename);
                }
                else
                {
                    Log.E($"Can't delete file at {path}. No file at path");
                    OnDeleteCompleted(false, savename);
                }
            }
            catch (Exception e)
            {
                Log.E($"Can't delete file at {path}. Cause {e}");
                OnDeleteCompleted(false, savename);
            }
        }
    }
}
