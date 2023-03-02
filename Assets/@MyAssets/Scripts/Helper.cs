using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;


public static class Helper
{
	/*static readonly string[] shortNames = new string[20]
	{
		 "Millions"     ,"Milliards",
		 "Billions"     ,"Billiards",
		 "Trillions"    ,"Trilliards",
		 "Quadrillions" ,"Quadrilliards",
		 "Quintillions" ,"Quintilliards",
		 "Sextillions"  ,"Sextilliards",
		 "Septillions"  ,"Septilliards",
		 "Octillions"   ,"Octilliards",
		 "Nonillions"   ,"Nonilliards",
		 "Decellions"   ,"Decilliards"
	};*/

	static readonly string[] shortNames = new string[4] { "K", "M", "B", "T" };

	public static string Abbreviate(double value)
	{
		int nZeros = (int)Math.Log10(value);
		int prefixIndex = nZeros / 3;
		prefixIndex -= 1; // We delete the Thousand from the function to start with Ten Thousand

		if (nZeros < 4) // If under the Ten Thousand, no need to convert
			return value.ToString("0");
		else if (prefixIndex > 3) // Overflow..
			prefixIndex = 3;

		double number = value / Math.Pow(10, (prefixIndex + 1) * 3);
		return string.Format("{0:00}{1}", number, shortNames[prefixIndex]);
	}

	public static void Abbreviate(double value, out string result) => result = Abbreviate(value);

	public static Vector3 FromFloatToVector3(float[] original) => new Vector3(original[0], original[1], original[2]);

	public static void FromFloatToVector3(float[] original, out Vector3 result) => result = FromFloatToVector3(original);

	public static float[] FromVector3ToFloat(Vector3 original) => new float[3] { original.x, original.y, original.z };

	public static void FromVector3ToFloat(Vector3 original, out float[] result) => result = FromVector3ToFloat(original);

	public static int RandomInt(int min, int max)
	{
		RandomInt(min, max, out int randomInt);
		return randomInt;
	}

	public static void RandomInt(int min, int max, out int result) => result = Random.Range(min, max);

	public static float RandomFloat(float min, float max)
	{
		RandomFloat(min, max, out float randomFloat);
		return randomFloat;
	}

	public static void RandomFloat(float min, float max, out float result) => result = Random.Range(min, max);

	public static void Find(string name, out GameObject original) => original = GameObject.Find(name);

	public static int RandomInt(int min, int max, IList<int> list)
	{
		RandomInt(min, max, list, out int randomInt);
		return randomInt;
	}

	public static void RandomInt(int min, int max, IList<int> list, out int result)
	{
		int i;
		do
		{
			RandomInt(min, max, out i);
		}
		while (list.Contains(i));
		result = i;
	}

	public static int RandomInt(int min, int max, int numberToMatch)
	{
		RandomInt(min, max, numberToMatch, out int randomInt);
		return randomInt;
	}

	public static void RandomInt(int min, int max, int numberToMatch, out int result)
	{
		int i;
		do
		{
			RandomInt(min, max, out i);
		}
		while (numberToMatch.Equals(i));
		result = i;
	}

	public static int DigitCount(int val)
	{
		int count = 0;
		while (val != 0)
		{
			val /= 10;
			count++;
		}
		return count;
	}

	public static void DigitCount(int val, out int result) => result = DigitCount(val);

	public static void FindNearestGameObject<T>(Transform source, IList<T> allTransforms, out Transform result) where T : Component
	{
		T nearestObj = null;
		float distance = Mathf.Infinity;
		for (int i = 0; i < allTransforms.Count; i++)
		{
			float difference = Vector3.Distance(source.position, allTransforms[i].transform.position);

			if (difference < distance)
			{
				distance = difference;
				nearestObj = allTransforms[i];
			}
		}
		result = nearestObj.transform;
	}

	public static void InvokeFromAssembly(string nameSpace, string className, string methodName, string assembly)
	{
		string _className = nameSpace + "." + className;
		string _assembly = assembly;
		string _type = _className + ", " + _assembly;
		Type type = Type.GetType(_type, true);

		if (type != null)
		{
			MethodInfo methodInfo = type.GetMethod(methodName);
			if (methodInfo != null)
			{
				methodInfo.Invoke(null, null);
			}
		}
	}

	public static StringBuilder Build(params string[] args)
	{
		StringBuilder builder = new StringBuilder();

		for (int i = 0; i < args.Length; i++)
		{
			builder.Append(args[i]);
		}

		return builder;
	}
}
