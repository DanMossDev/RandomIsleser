using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomIsleser
{
    public class ChestController : MonoBehaviour, Interactable
    {
	    [SerializeField] private ChestModel _chestModel;
	    public Transform ChestOpenPoint;

	    private void OnEnable()
	    {
		    _chestModel.Controller = this;
	    }

	    private void OnDisable()
	    {
		    if (_chestModel.Controller == this)
			    _chestModel.Controller = null;
	    }

	    public void Interact()
	    {
		    if (_chestModel.HasBeenOpened)
			    return;

		    PlayerController.Instance.OpenChest(this, _chestModel);
	    }
    }
}
