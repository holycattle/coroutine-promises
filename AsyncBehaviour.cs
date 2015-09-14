using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoroutinePromise
{
	IEnumerator coroutine;
	CoroutinePromise promise;
	bool isDone = false;
	
	public CoroutinePromise(IEnumerator c)
	{
		coroutine = c;
	}
	
	public CoroutinePromise Then(IEnumerator newCoroutine)
	{
		promise = new CoroutinePromise(newCoroutine);
		return promise;
	}

	public CoroutinePromise Promise
	{
		get { return promise; }
	}

	public bool IsDone
	{
		get { return isDone; }
	}

	public IEnumerator Coroutine 
	{
		get { return coroutine; }
	}
}

public class CoroutinePromiseSet
{
	List<CoroutinePromise> listPromises = new List<CoroutinePromise>();

	public CoroutinePromiseSet(CoroutinePromise c)
	{
		listPromises.Add(c);
	}

	public IEnumerator SuperCoroutine()
	{
		while (listPromises.Count > 0)
		{
			for (int i = 0; i < listPromises.Count; i++)
			{
				bool x = listPromises[i].Coroutine.MoveNext();
				if (!listPromises[i].IsDone && x) {
					continue;
				} else {
					Debug.Log (listPromises[i].IsDone);
					Debug.Log(x);
					Debug.Break();
					if (listPromises[i].Promise != null)
						listPromises.Add(listPromises[i].Promise);
					listPromises.RemoveAt(i);
				}

			}
			yield return null;
		}
	}
}

public class AsyncBehaviour : MonoBehaviour {

	void Start ()
	{
		CoroutinePromise c = new CoroutinePromise(CoroutineA());
		c.Then(CoroutineB()).Then(CoroutineC());
		StartPromiseSet(c);

		CoroutinePromise c2 = new CoroutinePromise(CoroutineA2());
		c.Then(CoroutineB2()).Then(CoroutineC2());
		StartPromiseSet(c2);
	}

	float t = 0.0f;
	void Update()
	{
		t += Time.deltaTime;
	}

	public void StartPromiseSet(CoroutinePromise c)
	{
		CoroutinePromiseSet cSet = new CoroutinePromiseSet(c);
		StartCoroutine(cSet.SuperCoroutine());
	}

	IEnumerator CoroutineA()
	{
		while (t <= 5f)
		{
		//yield return new WaitForSeconds(3f);
			Debug.Log("a: "+t);
			yield return null;
		}
	}

	IEnumerator CoroutineB()
	{
		while (t <= 10f)
		{
			//yield return new WaitForSeconds(3f);
			Debug.Log("b: "+t);
			yield return null;
		}
	}

	IEnumerator CoroutineC()
	{
		while (t <= 15f)
		{
			//yield return new WaitForSeconds(3f);
			Debug.Log("c: "+t);
			yield return null;
		}
	}

	IEnumerator CoroutineA2()
	{
		while (t <= 5f)
		{
			//yield return new WaitForSeconds(3f);
			Debug.Log("a2: "+t);
			yield return null;
		}
	}
	
	IEnumerator CoroutineB2()
	{
		while (t <= 10f)
		{
			//yield return new WaitForSeconds(3f);
			Debug.Log("b2: "+t);
			yield return null;
		}
	}
	
	IEnumerator CoroutineC2()
	{
		while (t <= 15f)
		{
			//yield return new WaitForSeconds(3f);
			Debug.Log("c2: "+t);
			yield return null;
		}
	}
}