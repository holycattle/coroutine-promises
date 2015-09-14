/*
The MIT License (MIT)
Copyright (c) 2015 Joniel Ibasco

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

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

	public void Resolve()
	{
		isDone = true;
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
				if (!listPromises[i].IsDone && listPromises[i].Coroutine.MoveNext()) {
					continue;
				} else {
					if (listPromises[i].Promise != null)
					{
						listPromises.Add(listPromises[i].Promise);
					}
					listPromises.RemoveAt(i);
				}
			}
			yield return null;
		}
	}
}

public class AsyncBehaviour : MonoBehaviour {
	CoroutinePromise c;
	CoroutinePromise ca;
	void Start ()
	{
		c = new CoroutinePromise(CoroutineA());
		ca = c.Then(CoroutineB());
		CoroutinePromise cb = ca.Then(CoroutineC());
		StartPromiseSet(c);
		
		CoroutinePromise c2 = new CoroutinePromise(CoroutineA2());
		c2.Then(CoroutineB2()).Then(CoroutineC2());
		StartPromiseSet(c2);
	}
	
	float t = 0.0f;
	void Update()
	{
		t += Time.deltaTime;

		if (t >= 2f) c.Resolve();
		if (t >= 7f) ca.Resolve();
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