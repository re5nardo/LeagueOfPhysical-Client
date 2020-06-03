using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSViewer : MonoBehaviour
{
	[SerializeField] private Text textFPS = null;

	private const int MAX_COUNT = 100;

	private List<float> m_listDeltaTime = new List<float>();  

	void Update()
	{
		m_listDeltaTime.Add(Time.deltaTime);

		if (m_listDeltaTime.Count > MAX_COUNT)
		{
			m_listDeltaTime.RemoveRange(0, m_listDeltaTime.Count - MAX_COUNT);
		}

        textFPS.text = string.Format("{0:#,##0} FPS", GetRecentFPS(1f));
	}

	public float GetRecentFPS(float fTime)
	{
		float fSumTime = 0;
		int nFrameCount = 0;

		for (int i = m_listDeltaTime.Count - 1; i >= 0; --i)
		{
			fSumTime += m_listDeltaTime[i];
			nFrameCount++;

			if (fSumTime >= fTime)
			{
				return fSumTime == 0 ? 0 : nFrameCount / fSumTime;
			}
		}

		return fSumTime == 0 ? 0 : nFrameCount / fSumTime;
	}
}
