using RedBjorn.SuperTiles.Saves;
using RedBjorn.Utils;
using System;
using System.Collections.Generic;

namespace RedBjorn.SuperTiles
{
    /// <summary>
    /// Controller of save logic
    /// </summary>
    public class SaveController : Saves.ISaveWorkCallback, Saves.ISerializerCallback
    {
        /// <summary>
        /// Do the main work
        /// </summary>
        Saves.SaveWorker Worker;

        /// <summary>
        /// Contains object which can Serialize/Deserialize input object
        /// </summary>
        Saves.ISerializer Serializer;

        /// <summary>
        /// Actions to be played when save will be saved
        /// </summary>
        Dictionary<string, Action> OnSavedActions;

        /// <summary>
        /// Actions to be played when save will be loaded
        /// </summary>
        Dictionary<string, Action<GameSave>> OnLoadedActions;

        /// <summary>
        /// Actions to be played when save will be deleted
        /// </summary>
        Dictionary<string, Action> OnDeletedActions;

        SaveController()
        {
            OnSavedActions = new Dictionary<string, Action>();
            OnLoadedActions = new Dictionary<string, Action<GameSave>>();
            OnDeletedActions = new Dictionary<string, Action>();
            Serializer = new UnityJsonSerializer<GameSave>(this);
            Worker = new Saves.SaveWorkStandalone(this);
        }

        static SaveController CachedInstance;
        static SaveController Instance
        {
            get
            {
                if (CachedInstance == null)
                {
                    CachedInstance = new SaveController();
                }
                return CachedInstance;
            }
        }

        /// <summary>
        /// Save game into a file
        /// </summary>
        /// <param name="game">game state</param>
        /// <param name="savename">name of file</param>
        /// <param name="onSaved">action to be played on game will be saved</param>
        public static void SaveGame(GameEntity game, string savename, Action onSaved = null)
        {
            Instance.SaveGameInternal(game, savename, onSaved);
        }

        /// <summary>
        /// Load game from file
        /// </summary>
        /// <param name="savename">name of file</param>
        /// <param name="onLoaded">action to be played on game will be loaded</param>
        public static void LoadGame(string savename, Action<GameSave> onLoaded = null)
        {
            Instance.LoadGameInternal(savename, onLoaded);
        }

        /// <summary>
        /// Delete save file
        /// </summary>
        /// <param name="savename">name of file</param>
        /// <param name="onDeleted">action to be played on game will be deleted</param>
        public static void DeleteGame(string savename, Action onDeleted = null)
        {
            Instance.DeleteGameInternal(savename, onDeleted);
        }

        void SaveGameInternal(GameEntity game, string savename, Action onSaved)
        {
            var save = new GameSave()
            {
                State = game,
                Version = "1",
                Timestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff"),
            };
            OnSavedActions[savename] = onSaved;
            Log.I($"Serialization. Savename: {savename} started");
            Serializer.Serialize(save, savename);
        }

        void LoadGameInternal(string savename, Action<GameSave> onLoaded)
        {
            OnLoadedActions[savename] = onLoaded;
            Worker.Load(savename);
        }

        void DeleteGameInternal(string savename, Action onDeleted)
        {
            OnDeletedActions[savename] = onDeleted;
            Worker.Delete(savename);
        }

        void Saves.ISaveWorkCallback.OnSaveCompleted(bool success, byte[] data, string savename)
        {
            var action = OnSavedActions.TryGetOrDefault(savename);
            OnSavedActions[savename] = null;
            action.SafeInvoke();
        }

        void Saves.ISaveWorkCallback.OnLoadCompleted(bool success, byte[] bytes, string savename)
        {
            if (success)
            {
                Log.I($"Deserialization. Savename: {savename} started");
                Serializer.Deserialize(bytes, savename);
            }
        }

        void Saves.ISaveWorkCallback.OnDeleteCompleted(bool success, string savename)
        {
            var action = OnDeletedActions.TryGetOrDefault(savename);
            OnDeletedActions[savename] = null;
            action.SafeInvoke();
        }

        /// <summary>
        /// When serialization completed, file will be saved
        /// </summary>
        /// <param name="success">result</param>
        /// <param name="savename">name of file</param>
        /// <param name="serialized">savefile in bytes format</param>
        void Saves.ISerializerCallback.OnSerializeCompleted(bool success, string savename, byte[] serialized)
        {
            Log.I($"Serialization. Savename: {savename} completed. Success: {success}");
            if (success)
            {
                Worker.Save(serialized, savename);
            }
        }

        /// <summary>
        /// When deserialization complted, onLoaded action will be called
        /// </summary>
        /// <param name="success">result</param>
        /// <param name="savename">name of file</param>
        /// <param name="deserialized">deserialized object</param>
        void Saves.ISerializerCallback.OnDeserializeCompleted(bool success, string savename, object deserialized)
        {
            Log.I($"Deserialization. Savename: {savename} completed. Success: {success}");
            var action = OnLoadedActions.TryGetOrDefault(savename);
            OnLoadedActions[savename] = null;
            action.SafeInvoke(deserialized as GameSave);
        }
    }
}
