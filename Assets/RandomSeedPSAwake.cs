using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSeedPSAwake : MonoBehaviour
{
	public ParticleSystem ps;
	// Start is called before the first frame update
	void Awake()
	{
		ps.randomSeed = System.Convert.ToUInt32(transform.GetSiblingIndex() * 77);
		ps.Play();
	}
}
