#if UNITY_SWITCH
using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace MossUtils
{
    public enum SaveSlotState
    {
        Empty = 0,
        Valid = 1,
        Corrupt = 2,
        Backup = 3,
    }
    
    [CreateAssetMenu(fileName = "SwitchSerialisationSettings", menuName = "MossUtils/Serialisation/SwitchSerialisationSettings")]
    public class SwitchSerialisationSettings : ScriptableObject
    {
        public string MountName = "Studio Name";
        public string SaveName = "SaveFileName";

        public string MetaDataFileName = "Meta data file name";
        public bool UseMetaData = false;
    }

    /// <summary>
    /// Handles the read/write operations for save data on the Switch
    /// </summary>
    public class PlatformSerialiserSwitch : ISaveHandler
    {
#region Variables

private nn.account.Uid _userID;
        private nn.fs.FileHandle _fileHandle = new nn.fs.FileHandle();
        
        private int _counter = 0;
        private int _saveData = 0;
        private int _loadData = 0;
        private int _maxSaveFileSize = 16384;
        
        private bool _canReloadandRestart = false;
        private bool _show = false;
        private bool _metaDataInitialised = false;
        private bool _isLoading = false;
        private bool _isSaving = false;
        
        private string _metaFilePath;
        
        private SwitchSerialisationSettings _serialisationSettings;
        #endregion //Variables
        
#region Initialisation
        public async Task Initialise(SwitchSerialisationSettings serialisationSettings)
        {
            _serialisationSettings = serialisationSettings;
            if (_serialisationSettings.UseMetaData)
                _metaFilePath  = string.Format("{0}:/{1}", _serialisationSettings.MountName, _serialisationSettings.MetaDataFileName);

            //mount the save data
            nn.account.Account.Initialize();
            nn.account.UserHandle userHandle = new nn.account.UserHandle();
            if (!nn.account.Account.TryOpenPreselectedUser(ref userHandle))
            {
                nn.Nn.Abort("Failed to open preselected user.");
            }
            
            nn.Result result = nn.account.Account.GetUserId(ref _userID, userHandle);
            result.abortUnlessSuccess();
            if (!result.IsSuccess())
            {
                Log(result.ToString());
                return;
            }
            result = nn.fs.SaveData.Mount(_serialisationSettings.MountName, _userID);
            result.abortUnlessSuccess();
            if (!result.IsSuccess())
            {
                Log(result.ToString());
                return;
            }

            if (_serialisationSettings.UseMetaData)
                InitialiseMetaData();
            await InitialiseSaveData();
        }
        
        private void InitialiseMetaData()
        {
            nn.fs.EntryType entryType = 0;
            nn.Result result = nn.fs.FileSystem.GetEntryType(ref entryType, _metaFilePath);
            if (result.IsSuccess())
            {
                _metaDataInitialised = true;
                return;
            }
            if (!nn.fs.FileSystem.ResultPathNotFound.Includes(result))
            {
                result.abortUnlessSuccess();
            }
            _metaDataInitialised = false;
        }

        private async Task InitialiseSaveData()
        {
            string saveFilePath = GetSavePath();

            var slotExists = await SlotExists();

            if (slotExists == SaveSlotState.Empty)
            {
                var result = nn.fs.File.Create(saveFilePath, _maxSaveFileSize);
                result.abortUnlessSuccess();
            }
        }
#endregion //Initialisation

#region Save Data
        /// <summary>
        /// Saves the provided data string as a text file in a save slot. 
        /// </summary>
        /// <param name="dataString">The data to be saved</param>
        /// <param name="slot">The slot index</param>
        public async Task<bool> SaveGame(SaveData dataToSave)
        {
            string dataString = JsonConvert.SerializeObject(dataToSave);
            
            int dataSize = dataString.Length * sizeof(char);
            byte[] data;
            using (BinaryWriter writer = new BinaryWriter(new MemoryStream(dataSize)))
            {
                writer.Write(dataString);
                writer.BaseStream.Close();
                data = (writer.BaseStream as MemoryStream).GetBuffer();
            }
            return await SaveData(data);
        }

        /// <summary>
        /// Saves the provided data string as a text file in a save slot. 
        /// </summary>
        /// <param name="data">The data to be saved</param>
        /// <param name="slot">The slot index</param>
        private async Task<bool> SaveData(byte[] data)
        {
            string saveFilePath = GetSavePath();
            _isSaving = true;
            UnityEngine.Switch.Notification.EnterExitRequestHandlingSection();
            nn.Result result;
            nn.fs.OpenFileMode fileMode = nn.fs.OpenFileMode.Write | nn.fs.OpenFileMode.AllowAppend;
            result = nn.fs.File.Open(ref _fileHandle, saveFilePath, fileMode);
            result.abortUnlessSuccess();
            if (!result.IsSuccess())
            {
                Log("Unable to open savefile");
                _isSaving = false;
                return false;
            }
            
            const int offset = 0;
            result = nn.fs.File.Write(_fileHandle, offset, data, data.LongLength, nn.fs.WriteOption.Flush);
            result.abortUnlessSuccess();
            if (!result.IsSuccess())
            {
                Log("Unable to write savefile");
                _isSaving = false;
                return false;
            }
            
            nn.fs.File.Close(_fileHandle);
            result = nn.fs.FileSystem.Commit(_serialisationSettings.MountName);
            result.abortUnlessSuccess();
            if (!result.IsSuccess())
            {
                Log("Unable to close savefile");
                _isSaving = false;
                return false;
            }
            UnityEngine.Switch.Notification.LeaveExitRequestHandlingSection();
            
            _isSaving = false;
            return true;
        }

        /// <summary>
        /// Loads the save data from the requested slot
        /// </summary>
        /// <param name="slot">The slot index</param>
        /// <returns>The loaded data string</returns>
        public async Task<string> LoadData()
        {
            var data = await LoadRawData();
            return ParseData(data);
        }

        /// <summary>
        /// Parse the given data into a string
        /// </summary>
        /// <param name="data">The byte array of data to convert to string</param>
        /// <returns>The loaded data string</returns>
        private string ParseData(byte[] data)
        {
            if (data == null || data.Length == 0) 
                return string.Empty;

            string parsedData = string.Empty;
            using (BinaryReader reader = new BinaryReader(new MemoryStream(data)))
            {
                parsedData = reader.ReadString();
            }
            return parsedData;
        }
        
        /// <summary>
        /// Loads the save data from the requested slot
        /// </summary>
        /// <param name="slot">The slot index</param>
        /// <returns>The loaded data byte stream</returns>
        private async Task<byte[]> LoadRawData()
        {
            string saveFilePath = GetSavePath();
            UnityEngine.Switch.Notification.EnterExitRequestHandlingSection();
            nn.fs.EntryType entryType = 0;
            nn.Result result = nn.fs.FileSystem.GetEntryType(ref entryType, saveFilePath);
            if (nn.fs.FileSystem.ResultPathNotFound.Includes(result))
            {
                Log("Can't find save file to load");
                return Array.Empty<byte>();
            }
            result.abortUnlessSuccess();
            result = nn.fs.File.Open(ref _fileHandle, saveFilePath, nn.fs.OpenFileMode.Read);
            result.abortUnlessSuccess();
            if (!result.IsSuccess())
            {
                Log(result.ToString());
                return Array.Empty<byte>();
            }
            long fileSize = 0;
            result = nn.fs.File.GetSize(ref fileSize, _fileHandle);
            result.abortUnlessSuccess();
            if (!result.IsSuccess())
            {
                Log(result.ToString());
                return Array.Empty<byte>();
            }
            byte[] data = new byte[fileSize];
            result = nn.fs.File.Read(_fileHandle, 0, data, fileSize);
            result.abortUnlessSuccess();
            if (!result.IsSuccess())
            {
                Log(result.ToString());
                return Array.Empty<byte>();
            }
            UnityEngine.Switch.Notification.LeaveExitRequestHandlingSection();
            nn.fs.File.Close(_fileHandle);
            return data;
        }
        /// <summary>
        /// Deletes the save data in a slot
        /// </summary>
        /// <returns>If the delete operation was successful</returns>
        public async Task<bool> DeleteSaveData()
        {
            await SaveData(null);
            return true;
        }
        /// <summary>
        /// Checks if a save slot save file exists
        /// </summary>
        /// <param name="slot">The slot index</param>
        /// <returns>If the save slot exists</returns>
        public async Task<SaveSlotState> SlotExists()
        {
            string saveFilePath = GetSavePath();
            nn.Result result;
            nn.fs.EntryType entryType = 0;
            result = nn.fs.FileSystem.GetEntryType(ref entryType, saveFilePath);
            
            if (result.IsSuccess())
                return SaveSlotState.Valid;
            if (!nn.fs.FileSystem.ResultPathNotFound.Includes(result)) 
            {
                result.abortUnlessSuccess();
            }
            return SaveSlotState.Empty;
        }
#endregion //Save Data
       
        private void Log(string msg)
        {
            Debug.Log($"[PlatformSerialiserSwitch] {msg}");
        }

        private string GetSavePath()
        {
            return $"{_serialisationSettings.MountName}:/{_serialisationSettings.SaveName}";
        }
    }
}
#endif