using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public static class Extentions
{
#if UNITY_EDITOR
	public static void RemoveNullEntry(this UnityEventBase arg)
	{
		for (int i = 0; i < arg.GetPersistentEventCount(); i++)
		{
			var target = arg.GetPersistentTarget(i);
			if (target == null)
			{
				UnityEditor.Events.UnityEventTools.RemovePersistentListener(arg, i);
			}
		}
	}

	public static void RemoveAllEntry(this UnityEventBase arg)
	{
		for (int i = 0; i < arg.GetPersistentEventCount(); i++)
		{
			UnityEditor.Events.UnityEventTools.RemovePersistentListener(arg, i);
		}
	}
#endif
	public static Bounds GetBounds(this GameObject original)
	{
		Vector3 groupVectors = Vector3.zero;
		Vector3 groupCenter = Vector3.zero;

		Transform[] childrens = original.GetComponentsInChildren<Transform>().Where(tChild => tChild != original.transform).ToArray();

		foreach (Transform child in childrens)
		{
			groupVectors += child.position;
		}
		groupCenter = groupVectors / childrens.Length;

		Bounds bounds = new Bounds(groupCenter, Vector3.zero);

		foreach (Transform child in childrens)
		{
			if (child.TryGetComponent(out Renderer mr))
			{
				bounds.Encapsulate(mr.bounds);
			}
			else if (child.TryGetComponent(out Collider col))
			{
				bounds.Encapsulate(col.bounds);
			}
		}
		return bounds;
	}

	public static Vector2 With(this Vector2 original, float? x = null, float? y = null)
	{
		Vector2 vector = original;
		vector.x = x ?? original.x;
		vector.y = y ?? original.y;
		return vector;
	}

	public static Vector3 With(this Vector3 original, float? x = null, float? y = null, float? z = null)
	{
		Vector3 vector = original;
		vector.x = x ?? original.x;
		vector.y = y ?? original.y;
		vector.z = z ?? original.z;
		return vector;
	}

	public static Color32 With(this Color32 original, byte? r = null, byte? g = null, byte? b = null, byte? a = null)
	{
		Color32 color = original;
		color.r = r ?? original.r;
		color.g = g ?? original.g;
		color.b = b ?? original.b;
		color.a = a ?? original.a;
		return color;
	}

	public static Color With(this Color original, float? r = null, float? g = null, float? b = null, float? a = null)
	{
		Color color = original;
		color.r = r ?? original.r;
		color.g = g ?? original.g;
		color.b = b ?? original.b;
		color.a = a ?? original.a;
		return color;
	}

	public static void Show(this IList<Component> original)
	{
		for (int i = 0; i < original.Count; i++)
		{
			original[i].Show();
		}
	}

	public static void Show(this IList<GameObject> original)
	{
		for (int i = 0; i < original.Count; i++)
		{
			original[i].Show();
		}
	}

	public static void Hide(this IList<Component> original)
	{
		for (int i = 0; i < original.Count; i++)
		{
			original[i].Hide();
		}
	}

	public static void Hide(this IList<GameObject> original)
	{
		for (int i = 0; i < original.Count; i++)
		{
			original[i].Hide();
		}
	}

	public static void PlayIfNotPlaying(this AudioSource original)
	{
		if (!original.isPlaying) original.Play();
	}

	public static void StopAndClear(this ParticleSystem original, bool withChildren) => original.Stop(withChildren, ParticleSystemStopBehavior.StopEmittingAndClear);

	public static void StopEmmision(this ParticleSystem original, bool withChildren) => original.Stop(withChildren, ParticleSystemStopBehavior.StopEmitting);

	public static void RotateToDir(this Transform original, Vector3 lookDir, float dampTime)
	{
		Quaternion targetRotation = Quaternion.LookRotation(lookDir);
		original.rotation = Quaternion.Lerp(original.rotation, targetRotation, dampTime);
	}

	public static bool IsActive(this GameObject original) => original.activeSelf;
	public static bool IsActive(this Component original) => original.gameObject.activeSelf;
	public static void Show(this GameObject original) => original.SetActive(true);
	public static void Hide(this GameObject original) => original.SetActive(false);
	public static void Show(this Component original) => original.gameObject.SetActive(true);
	public static void Hide(this Component original) => original.gameObject.SetActive(false);
	public static Vector3 DirectionTo(this Transform source, Transform destination) => Vector3.Normalize(destination.position - source.position);
	public static Vector3 DirectionTo(this Transform source, Vector3 destination) => Vector3.Normalize(destination - source.position);
	public static float DistanceTo(this Transform source, Transform destination) => Vector3.Distance(source.position, destination.position);
	public static float DistanceTo(this Transform source, Vector3 destination) => Vector3.Distance(source.position, destination);

	/// <summary>
	/// Calculates the distance between two points.(Also multiply the Comparing Distance)
	/// </summary>
	/// <param name="source"></param>
	/// <param name="destination"></param>
	/// <param name="result"></param>
	public static void DistanceTo(this Transform source, Vector3 destination, out float result) => result = (source.position - destination).sqrMagnitude;

	/// <summary>
	/// Calculates the distance between two points.(Also multiply the Comparing Distance)
	/// </summary>
	/// <param name="source">Source Transform</param>
	/// <param name="destination">Destination Transform</param>
	/// <param name="result">Result Distance</param>
	public static void DistanceTo(this Transform source, Transform destination, out float result) => result = (source.position - destination.position).sqrMagnitude;

	public static Transform GetChild(this Component original, int childIndex)
	{
		return original.transform.GetChild(childIndex);
	}

	public static Transform FirstChild(this Component original)
	{
		if (original.transform.childCount > 0)
		{
			return original.transform.GetChild(0);
		}
		else
		{
			return original.transform;
		}
	}

	public static Transform LastChild(this Component original)
	{
		if (original.transform.childCount > 0)
		{
			return original.transform.GetChild(original.transform.childCount - 1);
		}
		else
		{
			return original.transform;
		}
	}

	public static IList<T> Shuffle<T>(this IList<T> list, int size)
	{
		System.Random rnd = new System.Random();
		var res = new T[size];

		res[0] = list[0];
		for (int i = 1; i < size; i++)
		{
			int j = rnd.Next(i);
			res[i] = res[j];
			res[j] = list[i];
		}
		return res;
	}

	public static IList<T> Shuffle<T>(this IList<T> list)
	{ return list.Shuffle(list.Count); }

	public static string JoinToString<T>(this IList<T> list, string separator)
	{
		StringBuilder builder = new StringBuilder();

		for (int i = 0; i < list.Count; i++)
		{
			builder.Append(list[i]).Append(separator);
		}
		return builder.ToString();
	}

	public static string JoinToString<T>(this IList<T> list, char separator)
	{
		StringBuilder builder = new StringBuilder();

		for (int i = 0; i < list.Count; i++)
		{
			builder.Append(list[i]).Append(separator);
		}
		return builder.ToString();
	}

	public static T Random<T>(this IList<T> original)
	{
		return original[UnityEngine.Random.Range(0, original.Count)];
	}

	public static T[] Add<T>(this T[] original, T item)
	{
		T[] temp = new T[original.Length + 1];
		original.CopyTo(temp, 0);
		temp[temp.Length - 1] = item;
		return temp;
	}

	public static T[] AddAt<T>(this T[] original, T item, int index)
	{
		T[] temp = new T[original.Length + 1];
		for (int i = 0, j = 0; i < temp.Length; i++)
		{
			if (i.Equals(index)) temp[i] = item;
			else temp[i] = original[j++];
		}
		return temp;
	}

	public static T[] Insert<T>(this T[] original, T item, int index)
	{
		T[] temp = new T[original.Length];
		original.CopyTo(temp, 0);
		temp[index] = item;
		return temp;
	}

	public static T[] Remove<T>(this T[] original, T item)
	{
		T[] temp = new T[original.Length - 1];
		for (int i = 0, j = 0; i < original.Length; i++)
		{
			if (original[i].Equals(item)) continue;
			temp[j] = original[i];
			j++;
		}
		return temp;
	}

	public static T[] RemoveAt<T>(this T[] original, int index)
	{
		T[] temp = new T[original.Length - 1];
		for (int i = 0, j = 0; i < original.Length; i++)
		{
			if (i.Equals(index)) continue;
			temp[j] = original[i];
			j++;
		}

		return temp;
	}

	public static T[] Clear<T>(this T[] original)
	{
		T[] temp = new T[0];
		return temp;
	}

	public static bool Contains<T>(this T[] original, T item)
	{
		bool has = false;
		for (int i = 0; i < original.Length; i++)
		{
			if (original[i].Equals(item))
			{
				has = true;
				break;
			}
		}
		return has;
	}
}
