using UnityEngine;

namespace RandomIsleser
{
	[CreateAssetMenu(fileName = "ChestModel", menuName = AssetMenuNames.Models + "ChestModel")]
    public class ChestModel : SaveableObject
    {
	    public bool HasBeenOpened;
	    
	    public RewardModel Reward;

	    [System.NonSerialized] public ChestController Controller;

	    public override void Load(SOData data)
	    {
		    var chestData = data as ChestData;
		    if (chestData == null) 
			    return;
		    
		    HasBeenOpened = chestData.HasBeenOpened;
	    }

	    public override SOData GetData()
	    {
		    return new ChestData()
		    {
			    ID = ID,
			    HasBeenOpened = HasBeenOpened
		    };
	    }

	    protected override void Cleanup()
	    {
		    base.Cleanup();
		    
		    HasBeenOpened = false;
	    }
    }

    public class ChestData : SOData
    {
	    public bool HasBeenOpened;
    }
}
