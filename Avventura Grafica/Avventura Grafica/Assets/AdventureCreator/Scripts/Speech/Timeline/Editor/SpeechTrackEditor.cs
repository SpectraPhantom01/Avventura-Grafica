#if UNITY_2017_1_OR_NEWER

using UnityEngine;
using UnityEditor;

namespace AC
{

	[CustomEditor (typeof(SpeechTrack))]
	public class SpeechTrackEditor : Editor
	{

		public override void OnInspectorGUI ()
		{
			SpeechTrack _target = (SpeechTrack) target;

			_target.ShowGUI ();

			if (GUI.changed)
			{
				EditorUtility.SetDirty (_target);
			}
		}
	}

}

#endif