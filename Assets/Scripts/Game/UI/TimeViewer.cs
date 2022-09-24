using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeViewer : MonoBehaviour
{
	[SerializeField] private Text networkTime;
	[SerializeField] private Text gameTime;

	private void Update()
	{
		networkTime.text = $"{Mirror.NetworkTime.time:N1} (NetworkTime)";
		gameTime.text = $"{LOP.Game.Current.GameTime:N1} (GameTime)";
	}
}
